//  Copyright (c) 2008 - 2014, www.metabolt.net (METAbolt)
//  Copyright (c) 2006-2008, Paul Clement (a.k.a. Delta)
//  All rights reserved.

//  Redistribution and use in source and binary forms, with or without modification, 
//  are permitted provided that the following conditions are met:

//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright notice, 
//    this list of conditions and the following disclaimer in the documentation 
//    and/or other materials provided with the distribution. 

//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
//  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
//  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
//  IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
//  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
//  NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
//  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
//  POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SLNetworkComm;
using OpenMetaverse;
using OpenMetaverse.Imaging;
using OpenMetaverse.Assets;
using ExceptionReporting;
using System.Threading;
using System.Text.RegularExpressions;
using System.Globalization;


namespace METAbolt
{
    public partial class frmProfile : Form
    {
        private METAboltInstance instance;
        private TabsConsole tabConsole;
        private SLNetCom netcom;
        private GridClient client;
        private string fullName;
        private UUID agentID;
        Avatar.AvatarProperties props;
        private bool aboutchanged = false;
        private bool lifeaboutchanged = false;
        private bool urlchanged = false;

        private UUID FLImageID = UUID.Zero;
        private UUID SLImageID = UUID.Zero;
        private UUID PickImageID = UUID.Zero;
        private UUID partner = UUID.Zero;
        private ExceptionReporter reporter = new ExceptionReporter();
        private UUID pickUUID = UUID.Zero;
        private string parcelname = string.Empty;
        private string simname = string.Empty;
        private int posX = 0;
        private int posY = 0;
        private int posZ = 0;
        //private bool displaynamechanged = false;
        //private string olddisplayname = string.Empty;
        private List<UUID> displaynames = new List<UUID>();
        string newname = string.Empty;
        const int WM_NCHITTEST = 0x0084;
        const int HTTRANSPARENT = -1;
        const int HTCLIENT = 1;
        private NumericStringComparer lvwColumnSorter;

        internal class ThreadExceptionHandler
        {
            public void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
            {
                ExceptionReporter reporter = new ExceptionReporter();
                reporter.Show(e.Exception);
            }
        }

        protected override void WndProc(ref Message m) 
        { 
            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST) 
            { 
                if (m.Result.ToInt32() == HTTRANSPARENT)            
                    m.Result = new IntPtr(HTCLIENT); 
            } 
        }

        public frmProfile(METAboltInstance instance, string fullName, UUID agentID)
        {
            InitializeComponent();

            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.instance = instance;
            netcom = this.instance.Netcom;
            client = this.instance.Client;
            this.fullName = fullName;
            this.agentID = agentID;
            tabConsole = instance.TabConsole;

            while (!IsHandleCreated)
            {
                // Force handle creation
                IntPtr temp = Handle;
            }

            this.txtOnline.Text = "";
            this.Text = fullName + " (profile) - METAbolt";

            AddClientEvents();
            AddNetcomEvents();

            InitializeProfile();

            if (agentID == client.Self.AgentID)
            {
                rtbAbout.ReadOnly = false;
                txtWebURL.ReadOnly = false;
                rtbAboutFL.ReadOnly = false;
                picSLImage.AllowDrop = true;
                picFLImage.AllowDrop = true;
                //txtDisplayName.ReadOnly = false;
                button7.Enabled = true;
                button8.Enabled = true;
                txtTitle.ReadOnly = false;
                txtDescription.ReadOnly = false;
            }

            lvwColumnSorter = new NumericStringComparer();
            lvGroups.ListViewItemSorter = lvwColumnSorter;
            lvwPicks.ListViewItemSorter = lvwColumnSorter;
        }

        ~frmProfile()
        {
            this.Dispose();
            //GC.Collect(); 
        }

        private void SetExceptionReporter()
        {
            reporter.Config.ShowSysInfoTab = false;   // alternatively, set properties programmatically
            reporter.Config.ShowFlatButtons = true;   // this particular config is code-only
            reporter.Config.CompanyName = "METAbolt";
            reporter.Config.ContactEmail = "metabolt@vistalogic.co.uk";
            reporter.Config.EmailReportAddress = "metabolt@vistalogic.co.uk";
            reporter.Config.WebUrl = "http://www.metabolt.net/metaforums/";
            reporter.Config.AppName = "METAbolt";
            reporter.Config.MailMethod = ExceptionReporting.Core.ExceptionReportInfo.EmailMethod.SimpleMAPI;
            reporter.Config.BackgroundColor = Color.White;
            reporter.Config.ShowButtonIcons = false;
            reporter.Config.ShowLessMoreDetailButton = true;
            reporter.Config.TakeScreenshot = true;
            reporter.Config.ShowContactTab = true;
            reporter.Config.ShowExceptionsTab = true;
            reporter.Config.ShowFullDetail = true;
            reporter.Config.ShowGeneralTab = true;
            reporter.Config.ShowSysInfoTab = true;
            reporter.Config.TitleText = "METAbolt Exception Reporter";
        }

        private void CleanUp()
        {
            client.Avatars.UUIDNameReply -= new EventHandler<UUIDNameReplyEventArgs>(Avatars_OnAvatarNames);
            client.Avatars.AvatarPropertiesReply -= new EventHandler<AvatarPropertiesReplyEventArgs>(Avatars_OnAvatarProperties);
            netcom.ClientLoggedOut -= new EventHandler(netcom_ClientLoggedOut);
            client.Avatars.AvatarGroupsReply -= new EventHandler<AvatarGroupsReplyEventArgs>(Avatars_OnGroupsReply);
            client.Avatars.PickInfoReply -= new EventHandler<PickInfoReplyEventArgs>(Avatars_OnPicksInfoReply);
            client.Avatars.AvatarPicksReply -= new EventHandler<AvatarPicksReplyEventArgs>(Avatars_OnPicksReply);
            client.Parcels.ParcelInfoReply -= new EventHandler<ParcelInfoReplyEventArgs>(Parcels_OnParcelInfoReply);
            client.Avatars.DisplayNameUpdate -= new EventHandler<DisplayNameUpdateEventArgs>(Avatar_DisplayNameUpdated);    
        }

        private void Avatar_DisplayNameUpdated(object sender, DisplayNameUpdateEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                //string old = e.OldDisplayName;
                string newname = e.DisplayName.DisplayName;

                if (!newname.ToLower(CultureInfo.CurrentCulture).Contains("resident") && !newname.ToLower(CultureInfo.CurrentCulture).Contains(" "))
                {
                    txtDisplayName.Text = newname;
                    button7.Enabled = false;
                }
                else
                {
                    txtDisplayName.Text = string.Empty;
                    button7.Enabled = true;
                }
            }));
        }

        private void AddClientEvents()
        {
            client.Avatars.UUIDNameReply += new EventHandler<UUIDNameReplyEventArgs>(Avatars_OnAvatarNames);
            client.Avatars.AvatarPropertiesReply += new EventHandler<AvatarPropertiesReplyEventArgs>(Avatars_OnAvatarProperties);
            client.Avatars.AvatarGroupsReply += new EventHandler<AvatarGroupsReplyEventArgs>(Avatars_OnGroupsReply);
            client.Avatars.PickInfoReply += new EventHandler<PickInfoReplyEventArgs>(Avatars_OnPicksInfoReply);
            client.Avatars.AvatarPicksReply += new EventHandler<AvatarPicksReplyEventArgs>(Avatars_OnPicksReply);
            client.Parcels.ParcelInfoReply += new EventHandler<ParcelInfoReplyEventArgs>(Parcels_OnParcelInfoReply);
            client.Avatars.DisplayNameUpdate += new EventHandler<DisplayNameUpdateEventArgs>(Avatar_DisplayNameUpdated);
        }

        private void AddNetcomEvents()
        {
            netcom.ClientLoggedOut += new EventHandler(netcom_ClientLoggedOut);
        }

        private void netcom_ClientLoggedOut(object sender, EventArgs e)
        {
            Close();
        }

        private void Avatars_OnPicksReply(object sender, AvatarPicksReplyEventArgs e)
        {
            if (e.AvatarID != agentID) return;

            this.BeginInvoke(new MethodInvoker(delegate()
            {
                PopulatePicksList(e.Picks);
                loadwait1.Visible = false;
            }));
        }

        private void PopulatePicksList(Dictionary<UUID, string> picks)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => PopulatePicksList(picks)));
                return;
            }

            lvwPicks.Items.Clear(); 

            foreach (KeyValuePair<UUID, string> pick in picks)
            {
                ListViewItem item = lvwPicks.Items.Add(pick.Value);
                item.Tag = pick.Key;
            }

            if (picks.Count < 10)
            {
                button8.Enabled = true;
            }
            else
            {
                button8.Enabled = false;
            }
        }

        private void Avatars_OnPicksInfoReply(object sender, PickInfoReplyEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => Avatars_OnPicksInfoReply(sender, e)));
                return;
            }

            this.BeginInvoke(new MethodInvoker(delegate()
            {
                txtTitle.Text = e.Pick.Name;
                txtDescription.Text = e.Pick.Desc;
                txtSlurl.Text = "None";
            }));

            pickUUID = e.Pick.ParcelID;
            client.Parcels.RequestParcelInfo(e.Pick.ParcelID);

            PickImageID = e.Pick.SnapshotID;

            if (!instance.ImageCache.ContainsImage(PickImageID))
            {
                client.Assets.RequestImage(PickImageID, ImageType.Normal, Assets_OnImageReceived);
            }
            else
            {
                BeginInvoke(
                    new OnSetPickImage(SetPickImage),
                    new object[] { PickImageID, instance.ImageCache.GetImage(PickImageID) });
            }
        }

        private void Parcels_OnParcelInfoReply(object sender, ParcelInfoReplyEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => Parcels_OnParcelInfoReply(sender, e)));
                return;
            }

            if (pickUUID != e.Parcel.ID) return;

            this.BeginInvoke(new MethodInvoker(delegate()
            {
                parcelname = e.Parcel.Name;
                simname = e.Parcel.SimName;

                posX = (int)e.Parcel.GlobalX % 256;
                posY = (int)e.Parcel.GlobalY % 256;
                posZ = (int)e.Parcel.GlobalZ % 256;

                txtSlurl.Text = parcelname + ", " + simname + "(" + posX.ToString(CultureInfo.CurrentCulture) + "," + posY.ToString(CultureInfo.CurrentCulture) + "," + posZ.ToString(CultureInfo.CurrentCulture) + ")";
            }));
        }

        private void Avatars_OnAvatarNames(object sender, UUIDNameReplyEventArgs e)
        {
            foreach (KeyValuePair<UUID, string> av in e.Names)
            {
                try
                {
                    BeginInvoke(new OnSetPartnerText(SetPartnerText), new object[] { av });
                    break;
                }
                catch
                {
                    ; 
                }
            }
        }

        private void Avatars_OnGroupsReply(object sender, AvatarGroupsReplyEventArgs e)
        {
            if (e.AvatarID != agentID) return;

            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => Avatars_OnGroupsReply(sender, e)));
                return;
            }

            //lvGroups.Items.Clear();  

            foreach (AvatarGroup group in e.Groups)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = group.GroupName;
                lvi.Tag = group;

                if (!lvGroups.Items.Contains(lvi))
                {
                    lvGroups.Items.Add(lvi);
                }
            }
        }

        private delegate void OnSetPartnerText(KeyValuePair<UUID, string> kvp);
        private void SetPartnerText(KeyValuePair<UUID, string> kvp)
        {
            if (partner == kvp.Key)
            {
                client.Avatars.AvatarPropertiesReply -= new EventHandler<AvatarPropertiesReplyEventArgs>(Avatars_OnAvatarProperties);
                txtPartner.Text = kvp.Value;
            }
        }

        //comes in on a separate thread
        private void Assets_OnImageReceived(TextureRequestState image, AssetTexture texture)
        {
            ManagedImage mImg;
            Image sImage = null;

            if (texture.AssetID != SLImageID && texture.AssetID != FLImageID)
            {
                if (texture.AssetID != PickImageID) return;

                OpenJPEG.DecodeToImage(texture.AssetData, out mImg, out sImage);
                System.Drawing.Image decodedImage1 = sImage;

                if (decodedImage1 != null)
                {
                    this.BeginInvoke(new MethodInvoker(delegate()
                    {
                        pictureBox1.Image = decodedImage1;
                        loadwait2.Visible = false;
                    }));

                    instance.ImageCache.AddImage(texture.AssetID, decodedImage1);
                }
            }
            else
            {
                //System.Drawing.Image decodedImage = ImageHelper.Decode(image.AssetData);
                //System.Drawing.Image decodedImage = OpenJPEGNet.OpenJPEG.DecodeToImage(image.AssetData);
                OpenJPEG.DecodeToImage(texture.AssetData, out mImg, out sImage);
                System.Drawing.Image decodedImage = sImage;

                if (decodedImage == null)
                {
                    if (texture.AssetID == SLImageID) BeginInvoke(new MethodInvoker(SetBlankSLImage));
                    else if (texture.AssetID == FLImageID) BeginInvoke(new MethodInvoker(SetBlankFLImage));

                    return;
                }

                instance.ImageCache.AddImage(texture.AssetID, decodedImage);

                try
                {
                    BeginInvoke(new OnSetProfileImage(SetProfileImage), new object[] { texture.AssetID, decodedImage });
                }
                catch { ; }

                //if (image.Success)
                //    picInsignia.Image = OpenJPEGNet.OpenJPEG.DecodeToImage(image.AssetData);
            }
        }

        private delegate void OnSetPickImage(UUID id, System.Drawing.Image image);
        private void SetPickImage(UUID id, System.Drawing.Image image)
        {
            if (id == PickImageID)
            {
                loadwait2.Visible = false;
                pictureBox1.Image = image;
            }
        }

        //called on GUI thread
        private delegate void OnSetProfileImage(UUID id, System.Drawing.Image image);
        private void SetProfileImage(UUID id, System.Drawing.Image image)
        {
            if (id == SLImageID)
            {
                picSLImage.Image = image;
                proSLImage.Visible = false;
            }
            else if (id == FLImageID)
            {
                picFLImage.Image = image;
                proFLImage.Visible = false;
            }
        }

        private void SetBlankSLImage()
        {
            picSLImage.BackColor = Color.FromKnownColor(KnownColor.Control);
            proSLImage.Visible = false;
        }

        private void SetBlankFLImage()
        {
            picFLImage.BackColor = Color.FromKnownColor(KnownColor.Control);
            proFLImage.Visible = false;
        }

        //comes in on separate thread
        private void Avatars_OnAvatarProperties(object sender, AvatarPropertiesReplyEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => Avatars_OnAvatarProperties(sender, e)));
                return;
            }

            if (e.AvatarID != agentID) return;

            props = e.Properties;

            FLImageID = props.FirstLifeImage;
            SLImageID = props.ProfileImage;

            if (instance.avtags.ContainsKey(e.AvatarID))
            {
                try
                {
                    string atag = instance.avtags[e.AvatarID];
                    txtTag.Text = atag;
                }
                catch { ; }
            }
            else
            {
                txtTag.Text = "avatar out of range";
            }

            if (SLImageID != UUID.Zero)
            {
                if (!instance.ImageCache.ContainsImage(SLImageID))
                    client.Assets.RequestImage(SLImageID, ImageType.Normal, Assets_OnImageReceived);
                else
                    BeginInvoke(
                        new OnSetProfileImage(SetProfileImage),
                        new object[] { SLImageID, instance.ImageCache.GetImage(SLImageID) });
            }
            else
            {
                BeginInvoke(new MethodInvoker(SetBlankSLImage));
            }

            if (FLImageID != UUID.Zero)
            {
                if (!instance.ImageCache.ContainsImage(FLImageID))
                    client.Assets.RequestImage(FLImageID, ImageType.Normal, Assets_OnImageReceived);
                else
                    BeginInvoke(
                        new OnSetProfileImage(SetProfileImage),
                        new object[] { FLImageID, instance.ImageCache.GetImage(FLImageID) });
            }
            else
            {
                BeginInvoke(new MethodInvoker(SetBlankFLImage));
            }

            this.BeginInvoke(
                new OnSetProfileProperties(SetProfileProperties),
                new object[] { props });
        }

        //called on GUI thread
        private delegate void OnSetProfileProperties(Avatar.AvatarProperties properties);
        private void SetProfileProperties(Avatar.AvatarProperties properties)
        {
            try
            {
                txtBornOn.Text = properties.BornOn;
                partner = properties.Partner;
                if (properties.Partner != UUID.Zero)
                {
                    if (!instance.avnames.ContainsKey(properties.Partner))
                    {
                        client.Avatars.RequestAvatarName(properties.Partner);
                    }
                    else
                    {
                        txtPartner.Text = instance.avnames[properties.Partner].ToString();
                    }
                }

                try
                {
                    if (fullName.EndsWith("Linden", StringComparison.CurrentCulture)) rtbAccountInfo.AppendText("Linden Lab Employee\n");
                    else rtbAccountInfo.AppendText("Resident\n");
                }
                catch { ; }

                if (properties.Identified && !properties.Transacted) rtbAccountInfo.AppendText("Payment Info On File\n");
                else if (properties.Transacted) rtbAccountInfo.AppendText("Payment Info Used\n");
                else rtbAccountInfo.AppendText("No Payment Info On File\n");

                if (properties.Online) txtOnline.Text = "Currently Online";
                else txtOnline.Text = "unknown";

                rtbAbout.AppendText(properties.AboutText);

                txtWebURL.Text = properties.ProfileURL;
                btnWebView.Enabled = btnWebOpen.Enabled = (txtWebURL.TextLength > 0);

                rtbAboutFL.AppendText(properties.FirstLifeText);

                txtUUID.Text = this.agentID.ToString();
            }
            catch (Exception exp)
            {
                OpenMetaverse.Logger.Log(String.Format(CultureInfo.CurrentCulture,"frmProfile.SetProfileProperties: {0}", exp.ToString()), Helpers.LogLevel.Error);
            }
        }

        private void InitializeProfile()
        {
            txtFullName.Text = fullName;
            btnOfferTeleport.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = btnPay.Enabled = (agentID != client.Self.AgentID);

            client.Avatars.RequestAvatarProperties(agentID);
            client.Avatars.RequestAvatarPicks(agentID);

            bool dnavailable = client.Avatars.DisplayNamesAvailable();

            if (dnavailable)
            {
                List<UUID> avIDs = new List<UUID>();
                avIDs.Add(agentID);
                client.Avatars.GetDisplayNames(avIDs, DisplayNameReceived);
            }

            //this.textBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBox1_DragDrop);
            //this.textBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBox1_DragEnter);
            //this.textBox1.DragOver += new System.Windows.Forms.DragEventHandler(this.textBox1_DragOver); 
        }

        private void DisplayNameReceived(bool success, AgentDisplayName[] names, UUID[] badIDs)
        {
            if (success)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    if (!names[0].DisplayName.ToLower(CultureInfo.CurrentCulture).Contains("resident") && !names[0].DisplayName.ToLower(CultureInfo.CurrentCulture).Contains(" "))
                    {
                        txtDisplayName.Text = names[0].DisplayName;
                    }
                    else
                    {
                        txtDisplayName.Text = string.Empty;
                    }
                }));
            }
        }

        private void frmProfile_FormClosing(object sender, FormClosingEventArgs e)
        {
            CleanUp();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnWebView_Click(object sender, EventArgs e)
        {
            WebBrowser web = new WebBrowser();
            web.Dock = DockStyle.Fill;

            string url = txtWebURL.Text;
            if (!url.StartsWith("http", StringComparison.CurrentCulture) && !url.StartsWith("https", StringComparison.CurrentCulture))
            {
                url = "http://" + url;
            }

            web.Url = new Uri(url);

            pnlWeb.Controls.Add(web);
        }

        private static void ProcessWebURL(string url)
        {
            if (url.StartsWith("http://", StringComparison.CurrentCulture) || url.StartsWith("ftp://", StringComparison.CurrentCulture))
                System.Diagnostics.Process.Start(url);
            else
                System.Diagnostics.Process.Start("http://" + url);
        }

        private void btnWebOpen_Click(object sender, EventArgs e)
        {
            ProcessWebURL(txtWebURL.Text);
        }

        private void rtbAbout_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            ProcessWebURL(e.LinkText);
        }

        private void rtbAboutFL_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            ProcessWebURL(e.LinkText);
        }

        private void btnOfferTeleport_Click(object sender, EventArgs e)
        {
            client.Self.SendTeleportLure(agentID, "Join me in " + client.Network.CurrentSim.Name + "!");
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            (new frmPay(instance, agentID, fullName)).Show(this);
        }

        private void frmProfile_Load(object sender, EventArgs e)
        {
            if (agentID == client.Self.AgentID)
            {
                button5.Visible = true;
            }

            // Load notes
            string logdir = instance.appdir;
            logdir += "\\Notes\\";
            LoadNotes(logdir);

            this.CenterToParent();
        }

        private void LoadNotes(string LogPath)
        {
            DirectoryInfo di = new DirectoryInfo(LogPath);
            FileSystemInfo[] files = di.GetFileSystemInfos();

            foreach (FileSystemInfo fi in files)
            {
                string inFile = fi.FullName;
                string finname = fi.Name;

                if (fullName != null)
                {
                    if (finname.Contains(fullName))
                    {
                        rtbNotes.LoadFile(inFile);
                    }
                }
            }
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode node = e.Data.GetData(typeof(TreeNode)) as TreeNode;

            if (node == null) return;

            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                InventoryBase io = (InventoryBase)node.Tag;

                if (node.Tag is InventoryFolder)
                {
                    //InventoryFolder folder = (InventoryFolder)io;
                    InventoryFolder folder = node.Tag as InventoryFolder;

                    client.Inventory.GiveFolder(folder.UUID, folder.Name, AssetType.Folder, agentID, true);
                    instance.TabConsole.DisplayChatScreen("Offered inventory folder " + folder.Name + " to " + fullName + ".");
                }
                else
                {
                    InventoryItem item = (InventoryItem)io;

                    if ((item.Permissions.OwnerMask & PermissionMask.Copy) != PermissionMask.Copy)
                    {
                        DialogResult res = MessageBox.Show("This is a 'no copy' item and you will lose ownership if you continue.", "Warning", MessageBoxButtons.OKCancel);

                        if (res == DialogResult.Cancel) return;
                    }

                    client.Inventory.GiveItem(item.UUID, item.Name, item.AssetType, agentID, true);
                    instance.TabConsole.DisplayChatScreen("Offered inventory item " + item.Name + " to " + fullName + ".");
                }
            }
        }

        private void tpgProfile_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void rtbAccountInfo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtOnline_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtBornOn_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void rtbAbout_TextChanged(object sender, EventArgs e)
        {
            aboutchanged = true;
        }

        private void picSLImage_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void rtbAbout_Leave(object sender, EventArgs e)
        {
            if (!aboutchanged) return;

            if (this.agentID != client.Self.AgentID) return;
        
            props.AboutText = rtbAbout.Text;

            client.Self.UpdateProfile(props); 
        }

        private void txtWebURL_Leave(object sender, EventArgs e)
        {
           if (!urlchanged) return;

           if (this.agentID != client.Self.AgentID) return;

            props.ProfileURL = txtWebURL.Text;

            client.Self.UpdateProfile(props);
        }

        private void rtbAboutFL_Leave(object sender, EventArgs e)
        {
            if (!lifeaboutchanged) return;

            if (this.agentID != client.Self.AgentID) return;
      
            props.FirstLifeText = rtbAboutFL.Text;

            client.Self.UpdateProfile(props);
        }

        private void txtWebURL_TextChanged(object sender, EventArgs e)
        {
            urlchanged = true;
        }

        private void rtbAboutFL_TextChanged(object sender, EventArgs e)
        {
            lifeaboutchanged = true;
        }

        private void picSLImage_DragEnter(object sender, DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(DataFormats.FileDrop))
            //{
            //    e.Effect = DragDropEffects.Move;
            //}
            //else
            //{
            //    e.Effect = DragDropEffects.None;
            //}

            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void picSLImage_DragDrop(object sender, DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(DataFormats.FileDrop))
            //{
            //    string s = (string)e.Data.GetData(DataFormats.FileDrop, false);

            //    char[] deli = ",".ToCharArray();
            //    string[] iDets = s.Split(deli);

            //    bool isimage = false;

            //    if (iDets[2].ToString() == "ImageJPEG")
            //    {
            //        isimage = true; 
            //    }
            //    else if (iDets[2].ToString() == "ImageTGA")
            //    {
            //        isimage = true;
            //    }
            //    else if (iDets[2].ToString() == "Texture")
            //    {
            //        isimage = true;
            //    }
            //    else if (iDets[2].ToString() == "TextureTGA")
            //    {
            //        isimage = true;
            //    }
            //    else
            //    {
            //        isimage = false;
            //    }

            //    if (!isimage) return;

            //    SLImageID = (UUID)iDets[3];

            //    props.ProfileImage = SLImageID;

            //    client.Self.UpdateProfile(props);

            //    proSLImage.Visible = true;

            //    if (!instance.ImageCache.ContainsImage(SLImageID))
            //    {
            //        client.Assets.RequestImage(SLImageID, ImageType.Normal, Assets_OnImageReceived);
            //    }
            //    else
            //    {
            //        picSLImage.Image = instance.ImageCache.GetImage(SLImageID);
            //        proSLImage.Visible = false;
            //    }
            //}

            TreeNode node = e.Data.GetData(typeof(TreeNode)) as TreeNode;

            if (node == null) return;

            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                InventoryBase io = (InventoryBase)node.Tag;

                if (node.Tag is InventoryFolder)
                {
                    //InventoryFolder folder = (InventoryFolder)io;
                    InventoryFolder folder = node.Tag as InventoryFolder;

                    client.Inventory.GiveFolder(folder.UUID, folder.Name, AssetType.Folder, agentID, true);
                    instance.TabConsole.DisplayChatScreen("Offered inventory folder " + folder.Name + " to " + fullName + ".");
                }
                else
                {
                    InventoryItem item = (InventoryItem)io;

                    if (agentID != client.Self.AgentID)
                    {
                        if ((item.Permissions.OwnerMask & PermissionMask.Copy) != PermissionMask.Copy)
                        {
                            DialogResult res = MessageBox.Show("This is a 'no copy' item and you will lose ownership if you continue.", "Warning", MessageBoxButtons.OKCancel);

                            if (res == DialogResult.Cancel) return;
                        }

                        client.Inventory.GiveItem(item.UUID, item.Name, item.AssetType, agentID, true);
                        instance.TabConsole.DisplayChatScreen("Offered inventory item " + item.Name + " to " + fullName + ".");
                    }
                    else
                    {
                        // Change the picture
                        if (item.AssetType == AssetType.ImageJPEG || item.AssetType == AssetType.ImageTGA || item.AssetType == AssetType.Texture || item.AssetType == AssetType.TextureTGA)
                        {
                            SLImageID = item.AssetUUID;

                            props.ProfileImage = SLImageID;

                            client.Self.UpdateProfile(props);

                            proSLImage.Visible = true;

                            if (!instance.ImageCache.ContainsImage(SLImageID))
                            {
                                client.Assets.RequestImage(SLImageID, ImageType.Normal, Assets_OnImageReceived);
                            }
                            else
                            {
                                picSLImage.Image = instance.ImageCache.GetImage(SLImageID);
                                proSLImage.Visible = false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("To change your picture you must drag and drop an image or a texture", "METAbolt");
                            return;
                        }
                    }
                }
            }
        }

        private void picSLImage_DragOver(object sender, DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(DataFormats.FileDrop))
            //{
            //    e.Effect = DragDropEffects.Move;
            //}
            //else
            //{
            //    e.Effect = DragDropEffects.None;
            //}

            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void picFLImage_DragOver(object sender, DragEventArgs e)
        {
            if (agentID != client.Self.AgentID) return;

            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void picFLImage_DragEnter(object sender, DragEventArgs e)
        {
            if (agentID != client.Self.AgentID) return;

            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void picFLImage_DragDrop(object sender, DragEventArgs e)
        {
            if (agentID != client.Self.AgentID) return;

            TreeNode node = e.Data.GetData(typeof(TreeNode)) as TreeNode;

            if (node == null) return;

            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                InventoryBase io = (InventoryBase)node.Tag;

                if (node.Tag is InventoryFolder)
                {
                    return;
                }
                else
                {
                    InventoryItem item = (InventoryItem)io;

                    if (agentID != client.Self.AgentID)
                    {
                        return;
                    }
                    else
                    {
                        // Change the picture
                        if (item.AssetType == AssetType.ImageJPEG || item.AssetType == AssetType.ImageTGA || item.AssetType == AssetType.Texture || item.AssetType == AssetType.TextureTGA)
                        {
                            SLImageID = item.AssetUUID;

                            props.ProfileImage = SLImageID;

                            client.Self.UpdateProfile(props);

                            proFLImage.Visible = true;

                            if (!instance.ImageCache.ContainsImage(SLImageID))
                            {
                                client.Assets.RequestImage(SLImageID, ImageType.Normal, Assets_OnImageReceived);
                            }
                            else
                            {
                                picFLImage.Image = instance.ImageCache.GetImage(SLImageID);
                                proFLImage.Visible = false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("To change your picture you must drag and drop an image or a texture", "METAbolt");
                            return;
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (instance.IsAvatarMuted(agentID, fullName))
            {
                MessageBox.Show(fullName + " is already in your mute list.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //DataRow dr = instance.MuteList.NewRow();
            //dr["uuid"] = agentID;
            //dr["mute_name"] = fullName;
            //instance.MuteList.Rows.Add(dr);

            instance.Client.Self.UpdateMuteListEntry(MuteType.Resident, agentID, fullName);

            MessageBox.Show(fullName + " is now muted.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);      
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Boolean fFound = true;

            client.Friends.FriendList.ForEach(delegate(FriendInfo friend)
            {
                if (friend.Name == fullName)
                {
                    fFound = false;
                }
            });

            if (fFound)
            {
                client.Friends.OfferFriendship(agentID);
            }
        }

        private void lvGroups_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                AvatarGroup group = (AvatarGroup)lvGroups.SelectedItems[0].Tag;

                frmGroupInfo frm = new frmGroupInfo(group, instance);
                frm.Show();
            }
            catch
            {
                ; 
            }
        }

        private void lvGroups_MouseEnter(object sender, EventArgs e)
        {
            lvGroups.Cursor = Cursors.Hand;
        }

        private void lvGroups_MouseLeave(object sender, EventArgs e)
        {
            lvGroups.Cursor = Cursors.Default;  
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (tabConsole.TabExists(fullName))
            {
                tabConsole.SelectTab(fullName);
                return;
            }

            tabConsole.AddIMTab(agentID, client.Self.AgentID ^ agentID, fullName);
            tabConsole.SelectTab(fullName);

            tabConsole.Focus();
        }

        private void lvwPicks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwPicks.SelectedItems.Count == 0) return;

            UUID pick = (UUID)lvwPicks.SelectedItems[0].Tag;

            loadwait2.Visible = true;

            if (agentID == client.Self.AgentID)
            {
                button11.Enabled = true;
            }

            client.Avatars.RequestPickInfo(agentID, pick);
            txtTitle.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtSlurl.Text = string.Empty;

            pictureBox1.Image = null;

            parcelname = string.Empty;
            simname = string.Empty;
            //pickUUID = UUID.Zero;  

            posX = 0;
            posY = 0;
            posZ = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(simname))
            {
                Vector3 pos = new Vector3();
                pos.X = (float)posX;
                pos.Y = (float)posY;
                pos.Z = (float)posZ;

                client.Self.Teleport(simname, pos);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //if (pickUUID == UUID.Zero) return;

            if (lvwPicks.SelectedItems.Count == 0)
            {
                MessageBox.Show("To DELETE a pick you need to select one from the list first", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UUID pick = (UUID)lvwPicks.SelectedItems[0].Tag;

            client.Self.PickDelete(pick);

            txtTitle.Text = string.Empty;
            txtDescription.Text = string.Empty;
            pictureBox1.Image = null;

            client.Avatars.RequestAvatarPicks(agentID);
        }

        private void txtPartner_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDisplayName_Leave(object sender, EventArgs e)
        {
            //if (displaynamechanged)
            //{
            //    DialogResult response = MessageBox.Show("You have changed your Display Name to '" + txtDisplayName.Text + "'.\nAre you sure you want to change to this name?", "METAbolt", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            //    if (response == DialogResult.OK)
            //    {
            //        client.Self.SetDisplayNameReply += new EventHandler<SetDisplayNameReplyEventArgs>(Self_SetDisplayNameReply);

            //        displaynames.Add(client.Self.AgentID);
            //        client.Avatars.GetDisplayNames(displaynames, DisplayNamesCallBack);
            //        displaynames.Clear();
            //    }
            //}

            //displaynamechanged = false;
        }

        private void Self_SetDisplayNameReply(object sender, SetDisplayNameReplyEventArgs e)
        {
            if (e.Status == 200)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    txtDisplayName.Text = e.DisplayName.DisplayName;
                    button9.Enabled = false; 
                }));
            }
            else
            {
                string reason = e.Reason;

                if (reason.Trim().ToLower(CultureInfo.CurrentCulture) == "bad request")
                {
                    MessageBox.Show("Display name could not be set.\nYou can only change your display name once per week!", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Display name could not be set.\nReason: " + reason, "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            this.BeginInvoke(new MethodInvoker(delegate()
            {
                pBar3.Visible = false;
            }));

            client.Self.SetDisplayNameReply -= new EventHandler<SetDisplayNameReplyEventArgs>(Self_SetDisplayNameReply);
        }

        private void txtDisplayName_TextChanged(object sender, EventArgs e)
        {
            //if (agentID == client.Self.AgentID)
            //{
            //    displaynamechanged = true;
            //}
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string logdir = instance.appdir;   // Application.StartupPath.ToString();
            logdir += "\\Notes\\";

            string filename = fullName;

            rtbNotes.SaveFile(logdir + filename + ".rtf");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            gbDisplayName.Visible = true;
            txtDisplayName.Enabled = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == textBox3.Text.Trim())
            {
                newname = textBox2.Text.Trim();

                displaynames.Add(client.Self.AgentID);
                client.Avatars.GetDisplayNames(displaynames, DisplayNamesCallBack);
                displaynames.Clear();
                pBar3.Visible = true;
            }
            else
            {
                MessageBox.Show("The names you entered do not match. Check your entries and re-try.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox2.Focus();
            }
        }

        private void DisplayNamesCallBack(bool status, AgentDisplayName[] nme, UUID[] badIDs)
        {
            if (status)
            {
                client.Self.SetDisplayNameReply += new EventHandler<SetDisplayNameReplyEventArgs>(Self_SetDisplayNameReply);
                client.Self.SetDisplayName(nme[0].DisplayName, newname);
            }
            else
            {
                MessageBox.Show("Could not retrieve old name.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            gbDisplayName.Visible = false;
            txtDisplayName.Enabled = true;
        }

        private void tpgFirstLife_Click(object sender, EventArgs e)
        {

        }

        private void loadwait2_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            UUID pick = UUID.Random();
            UUID pid = client.Parcels.RequestRemoteParcelID(instance.SIMsittingPos(), client.Network.CurrentSim.Handle, client.Network.CurrentSim.ID);

            client.Self.PickInfoUpdate(pick, false, pid, this.instance.MainForm.parcel.Name, client.Self.GlobalPosition, this.instance.MainForm.parcel.SnapshotID, this.instance.MainForm.parcel.Desc);
            client.Avatars.RequestAvatarPicks(agentID);

            button11.Enabled = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            UUID pick = (UUID)lvwPicks.SelectedItems[0].Tag;
            UUID pid = client.Parcels.RequestRemoteParcelID(instance.SIMsittingPos(), client.Network.CurrentSim.Handle, client.Network.CurrentSim.ID);

            client.Self.PickInfoUpdate(pick, false, pid, txtTitle.Text.Trim(), client.Self.GlobalPosition, this.instance.MainForm.parcel.SnapshotID, txtDescription.Text.Trim());
            client.Avatars.RequestAvatarPicks(agentID);

            button11.Enabled = false;
        }
    }
}