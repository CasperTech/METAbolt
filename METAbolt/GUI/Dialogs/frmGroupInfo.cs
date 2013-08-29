//  Copyright (c) 2008 - 2013, www.metabolt.net (METAbolt)
//  Copyright (c) 2007-2009, openmetaverse.org
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
using System.Text;
using System.Windows.Forms;
using System.IO;
using OpenMetaverse;
using OpenMetaverse.Imaging;
using OpenMetaverse.Assets;
using OpenMetaverse.Packets; 
using SLNetworkComm;
using ExceptionReporting;
using System.Threading;
using System.Management;
using System.Globalization;
using System.Web; 


namespace METAbolt
{
    public partial class frmGroupInfo : Form
    {
        private METAboltInstance instance;
        //private ITextPrinter textPrinter;
        //private List<ChatBufferItem> textBuffer;
        private Group Group;
        private GridClient Client;
        private Group Profile = new Group();
        private Dictionary<UUID, GroupMember> Members = new Dictionary<UUID, GroupMember>();
        private Dictionary<UUID, GroupTitle> Titles = new Dictionary<UUID, GroupTitle>();
        private Dictionary<UUID, GroupMemberData> MemberData = new Dictionary<UUID, GroupMemberData>();
        private List<GroupMemberData> SortedMembers = new List<GroupMemberData>();
        private Dictionary<UUID, string> GrupMemberNames = new Dictionary<UUID, string>();
        private SLNetCom netcom;
        private InstantMessage imsg;
        private UUID objID = UUID.Zero;
        private bool floading = true;
        private bool tchanged = false;
        private UUID grouptitles = UUID.Zero;
        private UUID groupmembers = UUID.Zero;
        private bool ejectpower = false;
        private ExceptionReporter reporter = new ExceptionReporter();
        private UUID grpid = UUID.Zero;
        private Dictionary<UUID, GroupRole> grouproles;
        private List<KeyValuePair<UUID, UUID>> grouprolesavs;
        //private UUID grouprolesreply = UUID.Zero;
        //private UUID grouprolemembersid = UUID.Zero;
        private UUID founderid = UUID.Zero;
        //private GroupMemberData currentmember = new GroupMemberData();
        private bool checkignore = false;
        //private NumericStringComparer lvwColumnSorter;
        private NumericStringComparerDateGroups lvwDateColumnSorter;
        private AssetType assettype;
        private UUID assetfolder = UUID.Zero;
        private string filename = string.Empty;
        private string ejecttedgroupmember = string.Empty;
        private UUID ejectedmemberid = UUID.Zero;
        private MemberSorter sortedlist = new MemberSorter();
        private bool userisowner = false;
        private UUID groupmembersrequest = UUID.Zero;

        internal class ThreadExceptionHandler
        {
            public void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
            {
                ExceptionReporter reporter = new ExceptionReporter();
                reporter.Show(e.Exception);
            }
        }

        public frmGroupInfo(Group group, METAboltInstance instance)
        {
            InitializeComponent();

            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.instance = instance;
            netcom = instance.Netcom;
            Client = instance.Client;
            this.Group = group;
            grpid = group.ID;

            //while (!IsHandleCreated)
            //{
            //    // Force handle creation
            //    IntPtr temp = Handle;
            //}

            AddGEvents();

            //lvwColumnSorter = new NumericStringComparer();
            lvwDateColumnSorter = new NumericStringComparerDateGroups();

            //lstMembers.ListViewItemSorter = lvwDateColumnSorter;
            //lstMembers2.ListViewItemSorter = lvwDateColumnSorter;
            lstNotices.ListViewItemSorter = lvwDateColumnSorter;
            //lstRoles.ListViewItemSorter = lvwColumnSorter;

            GetDets();
        }

        public frmGroupInfo(AvatarGroup group, METAboltInstance instance)
        {
            InitializeComponent();

            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.instance = instance;
            netcom = instance.Netcom;
            Client = instance.Client;
            grpid = group.GroupID;

            //while (!IsHandleCreated)
            //{
            //    // Force handle creation
            //    IntPtr temp = Handle;
            //}

            AddGEvents();

            //lvwColumnSorter = new NumericStringComparer();
            lvwDateColumnSorter = new NumericStringComparerDateGroups();

            //lstMembers.ListViewItemSorter = lvwDateColumnSorter;
            //lstMembers2.ListViewItemSorter = lvwDateColumnSorter;
            lstNotices.ListViewItemSorter = lvwDateColumnSorter;

            GetDets();
        }

        public frmGroupInfo(UUID groupid, METAboltInstance instance)
        {
            InitializeComponent();

            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.instance = instance;
            netcom = instance.Netcom;
            Client = instance.Client;
            grpid = groupid;

            //while (!IsHandleCreated)
            //{
            //    // Force handle creation
            //    IntPtr temp = Handle;
            //}

            AddGEvents();

            //lvwColumnSorter = new NumericStringComparer();
            lvwDateColumnSorter = new NumericStringComparerDateGroups();

            //lstMembers.ListViewItemSorter = lvwDateColumnSorter;
            //lstMembers2.ListViewItemSorter = lvwDateColumnSorter;
            lstNotices.ListViewItemSorter = lvwDateColumnSorter;

            GetDets();
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

        private void AddGEvents()
        {
            grouprolesavs = new List<KeyValuePair<UUID, UUID>>();
            grouproles = new Dictionary<UUID, GroupRole>();

            Client.Groups.GroupProfile += new EventHandler<GroupProfileEventArgs>(GroupProfileHandler);
            Client.Avatars.UUIDNameReply += new EventHandler<UUIDNameReplyEventArgs>(AvatarNamesHandler);
            netcom.InstantMessageReceived += new EventHandler<InstantMessageEventArgs>(netcom_InstantMessageReceived);
            Client.Groups.GroupMembersReply += new EventHandler<GroupMembersReplyEventArgs>(GroupMembersHandler);
            Client.Groups.GroupTitlesReply += new EventHandler<GroupTitlesReplyEventArgs>(GroupTitlesHandler);
            Client.Groups.GroupNoticesListReply += new EventHandler<GroupNoticesListReplyEventArgs>(GroupNoticesHandler);
            Client.Groups.GroupLeaveReply += new EventHandler<GroupOperationEventArgs>(Groups_OnGroupLeft);
            Client.Groups.GroupRoleDataReply += new EventHandler<GroupRolesDataReplyEventArgs>(Groups_OnGroupRoleDataReply);
            Client.Groups.GroupRoleMembersReply += new EventHandler<GroupRolesMembersReplyEventArgs>(Groups_OnGroupRoleMembersReply);
            Client.Groups.GroupMemberEjected += new EventHandler<GroupOperationEventArgs>(Groups_GroupMemberEjected);
            Client.Groups.GroupJoinedReply += new EventHandler<GroupOperationEventArgs>(Groups_GroupMemberJoined);
        }

        private void GetDets()
        {
            lstMembers.Items.Clear();
            lstMembers2.Items.Clear();

            txtCharter.Enabled = false;
            chkPublish.Enabled = false;
            chkOpenEnrollment.Enabled = false;
            chkFee.Enabled = false;
            numFee.Enabled = false;
            chkMature.Enabled = false;

            // Request the group information
            Client.Groups.RequestGroupProfile(Group.ID);
            //groupmembers = Client.Groups.RequestGroupMembers(Group.ID);
            grouptitles = Client.Groups.RequestGroupTitles(Group.ID);
            // and the notices
            Client.Groups.RequestGroupNoticesList(Group.ID);
            Client.Groups.RequestGroupRoles(Group.ID); 
        }

        ~frmGroupInfo()
        {
            form_dispose(); 
        }

        private void Groups_GroupMemberJoined(object sender, GroupOperationEventArgs e)
        {
            if (e.GroupID != grpid) return;

            //if (e.Success)
            //{
            //    groupmembers = Client.Groups.RequestGroupMembers(Group.ID);
            //}
        }

        private void Groups_GroupMemberEjected(object sender, GroupOperationEventArgs e)
        {
            if (e.GroupID != grpid) return;

            if (e.Success)
            {
                //groupmembers = Client.Groups.RequestGroupMembers(Profile.ID);
                Members.Remove(ejectedmemberid);
                instance.TabConsole.DisplayChatScreen("Group member ejected: " + ejecttedgroupmember);

                label16.Text = Members.Count.ToString() + " members";
            }
            else
            {
                instance.TabConsole.DisplayChatScreen("Failed to eject group member: " + ejecttedgroupmember);
            }

            ejectedmemberid = UUID.Zero;
            ejecttedgroupmember = string.Empty;
        }

        private void form_dispose()
        {
            Client.Groups.GroupProfile -= new EventHandler<GroupProfileEventArgs>(GroupProfileHandler);
            Client.Avatars.UUIDNameReply -= new EventHandler<UUIDNameReplyEventArgs>(AvatarNamesHandler);
            Client.Groups.GroupMembersReply -= new EventHandler<GroupMembersReplyEventArgs>(GroupMembersHandler);
            Client.Groups.GroupTitlesReply -= new EventHandler<GroupTitlesReplyEventArgs>(GroupTitlesHandler);
            Client.Groups.GroupNoticesListReply -= new EventHandler<GroupNoticesListReplyEventArgs>(GroupNoticesHandler);
            Client.Groups.GroupLeaveReply -= new EventHandler<GroupOperationEventArgs>(Groups_OnGroupLeft);
            Client.Groups.GroupRoleDataReply -= new EventHandler<GroupRolesDataReplyEventArgs>(Groups_OnGroupRoleDataReply);
            Client.Groups.GroupRoleMembersReply -= new EventHandler<GroupRolesMembersReplyEventArgs>(Groups_OnGroupRoleMembersReply);
            Client.Groups.GroupMemberEjected += new EventHandler<GroupOperationEventArgs>(Groups_GroupMemberEjected);

            netcom.InstantMessageReceived -= new EventHandler<InstantMessageEventArgs>(netcom_InstantMessageReceived);

            Members.Clear();
            Titles.Clear();
            MemberData.Clear();
            GrupMemberNames.Clear();
            grouproles.Clear();
            grouprolesavs.Clear();

            this.Dispose();
            //GC.Collect();
        }

        public void GroupNoticesHandler(object sender, GroupNoticesListReplyEventArgs e)
        {
            if (e.GroupID != grpid)
                return;

            this.BeginInvoke(new MethodInvoker(delegate()
            {
                UpdateNotices(e.Notices);
            }));
        }

        private void Groups_OnGroupRoleMembersReply(object sender, GroupRolesMembersReplyEventArgs e)
        {
            if (e.GroupID != grpid) return;

            if (e.RequestID != groupmembersrequest) return;

            grouprolesavs = e.RolesMembers;

            userisowner = IsGroupOwner();
        }

        private void Groups_OnGroupRoleDataReply(object sender, GroupRolesDataReplyEventArgs e)
        {
            if (e.GroupID != grpid) return;

            try
            {
                grouproles = e.Roles;
            }
            catch { ; }

            groupmembersrequest = Client.Groups.RequestGroupRolesMembers(grpid);

            this.BeginInvoke(new MethodInvoker(delegate()
            {
                PopulateRoles();
            }));
        }

        private void PopulateRoles()
        {
            lstRoles.Items.Clear();

            foreach (GroupRole role in grouproles.Values)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = role.Name;
                lvi.Tag = role;
                //lstRoles.Items.Add(lvi);
                lvi.SubItems.Add(role.ID.ToString());

                //ListViewItem.ListViewSubItem lvsi = new ListViewItem.ListViewSubItem();
                //lvsi.Text = role.ID.ToString();
                //lvi.SubItems.Add(lvsi);
                lstRoles.Items.Add(lvi);
            }
        }

        private void UpdateNotices(List<GroupNoticesListEntry> notices)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    UpdateNotices(notices);
                }));

                return;
            }

            //lstNotices.Items.Clear();  

            foreach (GroupNoticesListEntry notice in notices)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = notice.Subject;

                ListViewItem.ListViewSubItem lvsi = new ListViewItem.ListViewSubItem();
                lvsi.Text = notice.FromName;
                lvi.SubItems.Add(lvsi);

                DateTime ndte = Utils.UnixTimeToDateTime(notice.Timestamp);

                if (!instance.Config.CurrentConfig.UseSLT)
                {
                    ndte = ndte.ToLocalTime();
                }

                lvsi = new ListViewItem.ListViewSubItem();
                lvsi.Text = ndte.ToShortDateString();
                lvi.SubItems.Add(lvsi);

                if (notice.HasAttachment)
                {
                    lvi.ImageIndex = GetAttachment(notice.AssetType);
                }

                lvi.Tag = notice;
                lstNotices.Items.Add(lvi);
            }
        }

        private static int GetAttachment(AssetType attype)
        {
            int at = 0;

            switch (attype)
            {
                case AssetType.Notecard:
                    at = 0;
                    break;
                case AssetType.LSLText:
                    at = 1;
                    break;
                case AssetType.Landmark:
                    at = 2;
                    break;
                case AssetType.Texture:
                    at = 3;
                    break;
                case AssetType.Clothing:
                    at = 4;
                    break;
                case AssetType.Object:
                    at = 5;
                    break;
                default:
                    at = 6;
                    break;
            }

            return at;
        }

        private void netcom_InstantMessageReceived(object sender, OpenMetaverse.InstantMessageEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    netcom_InstantMessageReceived(sender, e);
                }));

                return;
            }

            imsg = e.IM;
 
            if (e.IM.IMSessionID == UUID.Zero) return;

            try
            {
                if (e.IM.Dialog == InstantMessageDialog.GroupNoticeRequested)
                {
                    UUID fromAgentID = new UUID(e.IM.BinaryBucket, 2);

                    if (grpid == fromAgentID)
                    {
                        //string noticemsg =  e.IM.Message;

                        panel1.Visible = true;
                        panel2.Visible = false;

                        char[] deli = "|".ToCharArray();
                        string[] Msg = imsg.Message.Split(deli);
                        textBox5.Text = instance.CleanReplace("\n", System.Environment.NewLine, Msg[1]);

                        label7.Text = string.Empty;
                        label7.TextAlign = ContentAlignment.MiddleLeft;
                        label7.Refresh();

                        // Check for attachment
                        if (imsg.BinaryBucket[0] != 0)
                        {
                            assettype = (AssetType)imsg.BinaryBucket[1];

                            assetfolder = Client.Inventory.FindFolderForType(assettype);

                            if (imsg.BinaryBucket.Length > 18)
                            {
                                filename = Utils.BytesToString(imsg.BinaryBucket, 18, imsg.BinaryBucket.Length - 19);
                            }
                            else
                            {
                                filename = string.Empty;
                            }

                            label7.Text = filename;
                            label7.TextAlign = ContentAlignment.MiddleRight;
                            label7.Refresh();

                            pictureBox1.Visible = true;

                            switch (assettype)
                            {
                                case AssetType.Notecard:
                                    pictureBox1.Image = Properties.Resources.documents_16;
                                    break;
                                case AssetType.LSLText:
                                    pictureBox1.Image = Properties.Resources.lsl_scripts_16;
                                    break;
                                case AssetType.Landmark:
                                    pictureBox1.Image = Properties.Resources.lm;
                                    break;
                                case AssetType.Texture:
                                    pictureBox1.Image = Properties.Resources.texture;
                                    break;
                                case AssetType.Clothing:
                                    pictureBox1.Image = Properties.Resources.wear;
                                    break;
                                case AssetType.Object:
                                    pictureBox1.Image = Properties.Resources.objects;
                                    break;
                                default:
                                    pictureBox1.Image = Properties.Resources.applications_16;
                                    break;
                            }
                        }
                        else
                        {
                            pictureBox1.Visible = false;
                        }
                    }
                }
            }
            catch
            {
                ;
            }
        }

        private void GroupProfileHandler(object sender, GroupProfileEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    GroupProfileHandler(sender, e);
                }));

                return;
            }

            if (grpid != e.Group.ID) return;  

            this.Group = e.Group; 
            Profile = e.Group;

            this.BeginInvoke(new MethodInvoker(delegate()
            {
                label16.Text = Profile.GroupMembershipCount.ToString() + " members";

                UpdateProfile();

                if (!instance.avnames.ContainsKey(e.Group.FounderID))
                {
                    founderid = e.Group.FounderID;
                    Client.Avatars.RequestAvatarName(founderid);
                }
                else
                {
                    lblFoundedBy.Text = "Founded by " + instance.avnames[e.Group.FounderID].ToString();
                }

                if (this.Group.InsigniaID != null && this.Group.InsigniaID != UUID.Zero)
                    Client.Assets.RequestImage(this.Group.InsigniaID, ImageType.Normal,
                        delegate(TextureRequestState state, AssetTexture assetTexture)
                        {
                            ManagedImage imgData;
                            Image bitmap;

                            if (state != TextureRequestState.Timeout || state != TextureRequestState.NotFound)
                            {
                                OpenJPEG.DecodeToImage(assetTexture.AssetData, out imgData, out bitmap);
                                picInsignia.Image = bitmap;
                                UpdateInsigniaProgressText("Progress...");
                            }
                            if (state == TextureRequestState.Finished)
                            {
                                UpdateInsigniaProgressText("");
                            }
                        }, true);

                label10.Text = "Loading...";

                groupmembers = Client.Groups.RequestGroupMembers(Profile.ID);                
            }));
        }

        private void UpdateInsigniaProgressText(string resultText)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    UpdateInsigniaProgressText(resultText);
                }));

                return;
            }

            labelInsigniaProgress.Text = resultText;
        }

        void Groups_OnGroupLeft(object sender, GroupOperationEventArgs e)
        {
            //if (this.InvokeRequired)
            //{
            //    this.BeginInvoke(new MethodInvoker(delegate()
            //    {
            //        Groups_OnGroupLeft(sender, e);
            //    }));

            //    return;
            //}

            //groupmembers = Client.Groups.RequestGroupMembers(Group.ID);
        }

        private void UpdateProfile()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    UpdateProfile();
                }));

                return;
            }

            lblGroupName.Text = Profile.Name;
            txtCharter.Text = Profile.Charter;            

            if (instance.State.Groups.ContainsKey(Profile.ID))
            {
                chkListInProfile.Checked = instance.State.Groups[Profile.ID].ListInProfile;
                chkGroupNotices.Checked = instance.State.Groups[Profile.ID].AcceptNotices;
            }
            else
            {
                chkListInProfile.Enabled = false;
                chkGroupNotices.Enabled = false;

            }

            chkPublish.Checked = Profile.AllowPublish;
            chkOpenEnrollment.Checked = Profile.OpenEnrollment;
            chkMature.Checked = Profile.MaturePublish;

            chkFee.Checked = (Profile.MembershipFee != 0);

            try
            {
                if (Profile.MembershipFee > Convert.ToInt32(numFee.Maximum))
                {
                    numFee.Maximum = Convert.ToDecimal(Profile.MembershipFee);
                }

                numFee.Value = Profile.MembershipFee;
            }
            catch { ; }

            textBox2.Text = "Group UUID: " + Profile.ID.ToString();

            floading = false;
        }

        private void AvatarNamesHandler(object sender, UUIDNameReplyEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    AvatarNamesHandler(sender, e);
                }));

                return;
            }
               
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                List<UUID> memkeys = new List<UUID>();

                bool isgroupmembers = false;

                foreach (KeyValuePair<UUID, string> av in e.Names)
                {
                    if (Members.ContainsKey(av.Key))
                    {
                        isgroupmembers = true;

                        try
                        {
                            if (!GrupMemberNames.ContainsKey(av.Key))
                            {
                                GrupMemberNames.Add(av.Key, av.Value);
                            }

                            if (!memkeys.Contains(av.Key))
                            {
                                memkeys.Add(av.Key);
                            }

                            SortedMembers.Remove(MemberData[av.Key]);
                            MemberData[av.Key].Name = av.Value;
                            SortedMembers.Add(MemberData[av.Key]);


                            if (av.Key == founderid)
                            {
                                lblFoundedBy.Text = "Founded by " + av.Value;
                            }                            
                        }
                        catch
                        {
                            ;
                        }
                    }
                }

                WorkPool.QueueUserWorkItem(sync =>
                {
                    if (isgroupmembers) UpdateMembers(memkeys);
                });   
            }));
        }

        private void GroupMembersHandler(object sender, GroupMembersReplyEventArgs e)
        {
            if (groupmembers != e.RequestID) return;

            if (grpid != e.GroupID) return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    GroupMembersHandler(sender, e);
                }));

                return;
            }

            this.BeginInvoke(new MethodInvoker(delegate()
            {
                lstMembers.VirtualListSize = 0;
                lstMembers2.VirtualListSize = 0;
            }));

            List<UUID> requestids = new List<UUID>();

            foreach (var member in e.Members)
            {
                if (!Members.ContainsKey(member.Key))
                {
                    Members.Add(member.Key, member.Value);

                    if (!GrupMemberNames.ContainsKey(member.Key))
                    {
                        requestids.Add(member.Key);
                    }
                }
            }

            foreach (GroupMember member in Members.Values)
            {
                GroupMemberData memberData = new GroupMemberData();
                memberData.ID = member.ID;
                memberData.IsOwner = member.IsOwner;

                string lastonlinedate = member.OnlineStatus;

                if (member.OnlineStatus.ToLower(CultureInfo.CurrentCulture) != "online")
                {
                    lastonlinedate = ConvertDateTime(member.OnlineStatus).ToShortDateString();
                }

                memberData.LastOnline = lastonlinedate;
                memberData.Powers = (ulong)member.Powers;
                memberData.Title = member.Title;
                memberData.Contribution = member.Contribution;

                if (GrupMemberNames.ContainsKey(member.ID))
                {
                    memberData.Name = GrupMemberNames[member.ID];
                }
                else
                {
                    memberData.Name = "???";
                }

                if (!MemberData.ContainsKey(member.ID))
                {
                    MemberData.Add(member.ID, memberData);
                    SortedMembers.Add(memberData);
                }                
            }

            this.BeginInvoke(new MethodInvoker(delegate()
                {
                    lstMembers.VirtualListSize = SortedMembers.Count;
                    lstMembers2.VirtualListSize = SortedMembers.Count;
                }));

            if (requestids.Count > 0)
            {
                if (requestids.Count > 80)
                {
                    SendNameRequsts(requestids);               
                }
                else
                {
                    Client.Avatars.RequestAvatarNames(requestids);
                }
            }
            else
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                    {
                        label10.Visible = false;
                    }));
            }
        }

        private void SendNameRequsts(List<UUID> namelist)
        {
            this.BeginInvoke(new MethodInvoker(delegate()
                    {
                        List<List<UUID>> chunks = splitList(namelist);

                        pBar1.Visible = true;
                        pBar1.Maximum = chunks.Count;

                        foreach (List<UUID> chunklist in chunks)
                        {
                            pBar1.Value += 1;

                            Client.Avatars.RequestAvatarNames(chunklist);
                            Thread.Sleep(100);
                        }

                        pBar1.Visible = false;
                    }));
        }

        public static DateTime ConvertDateTime(string Date)
        {
            DateTime date = new DateTime();

            if (String.IsNullOrEmpty(Date)) return date; 

            try
            {
                string CurrentPattern = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
                string[] Split = new string[] { "-", "/", @"\", "." };
                string[] Patternvalue = CurrentPattern.Split(Split, StringSplitOptions.None);
                string[] DateSplit = Date.Split(Split, StringSplitOptions.None);
                string NewDate = "";

                if (Patternvalue[0].ToLower(CultureInfo.CurrentCulture).Contains("d") == true && Patternvalue[1].ToLower(CultureInfo.CurrentCulture).Contains("m") == true && Patternvalue[2].ToLower(CultureInfo.CurrentCulture).Contains("y") == true)
                {
                    NewDate = DateSplit[1] + "/" + DateSplit[0] + "/" + DateSplit[2];
                }
                else if (Patternvalue[0].ToLower(CultureInfo.CurrentCulture).Contains("m") == true && Patternvalue[1].ToLower(CultureInfo.CurrentCulture).Contains("d") == true && Patternvalue[2].ToLower(CultureInfo.CurrentCulture).Contains("y") == true)
                {
                    NewDate = DateSplit[0] + "/" + DateSplit[1] + "/" + DateSplit[2];
                }
                else if (Patternvalue[0].ToLower(CultureInfo.CurrentCulture).Contains("y") == true && Patternvalue[1].ToLower(CultureInfo.CurrentCulture).Contains("m") == true && Patternvalue[2].ToLower(CultureInfo.CurrentCulture).Contains("d") == true)
                {
                    NewDate = DateSplit[2] + "/" + DateSplit[0] + "/" + DateSplit[1];
                }
                else if (Patternvalue[0].ToLower(CultureInfo.CurrentCulture).Contains("y") == true && Patternvalue[1].ToLower(CultureInfo.CurrentCulture).Contains("d") == true && Patternvalue[2].ToLower(CultureInfo.CurrentCulture).Contains("m") == true)
                {
                    NewDate = DateSplit[2] + "/" + DateSplit[1] + "/" + DateSplit[0];
                }

                date = DateTime.Parse(NewDate, Thread.CurrentThread.CurrentCulture);
            }
            catch
            {
                //string exp = ex.Message; 
            }
            finally
            {

            }

            return date;

        }

        private void UpdateMembers(List<UUID> lst)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    UpdateMembers(lst);
                }));

                return;
            }
            else
            {
                try
                {
                    label10.Visible = false;

                    //List<UUID> requestids = new List<UUID>();

                    //bool isgmember = false;

                    foreach (UUID memid in lst)
                    {
                        if (Members.ContainsKey(memid))
                        {
                            GroupMember member = Members[memid];

                            if (member.ID == Client.Self.AgentID)
                            {
                                //isgmember = true;

                                button6.Enabled = false;

                                //bool isownerlike = HasGroupPower(GroupPowers.LandSetSale);

                                if (member.IsOwner || userisowner)
                                {
                                    cmdEject.Enabled = ejectpower = true;
                                    //button6.Enabled = true;

                                    button4.Visible = true;
                                    button5.Visible = true;

                                    txtCharter.Enabled = true;

                                    chkPublish.Enabled = true;
                                    chkOpenEnrollment.Enabled = true;
                                    chkFee.Enabled = true;
                                    numFee.Enabled = true;
                                    chkMature.Enabled = true;

                                    chkListInProfile.Enabled = true;
                                    chkGroupNotices.Enabled = true;

                                    button1.Enabled = true;
                                }
                                else
                                {
                                    //cmdEject.Enabled = ejectpower = ((member.Powers & GroupPowers.Eject) != 0);
                                    cmdEject.Enabled = ejectpower = HasGroupPower(GroupPowers.Eject);

                                    //button6.Enabled = false;

                                    button4.Visible = HasGroupPower(GroupPowers.CreateRole);   // ((member.Powers & GroupPowers.CreateRole) != 0);
                                    button5.Visible = HasGroupPower(GroupPowers.DeleteRole);   // ((member.Powers & GroupPowers.DeleteRole) != 0);
                                    button3.Visible = HasGroupPower(GroupPowers.SendNotices);

                                    if (instance.State.Groups.ContainsKey(Profile.ID))
                                    {
                                        if (HasGroupPower(GroupPowers.ChangeIdentity))   //(member.Powers & GroupPowers.ChangeIdentity) != 0)
                                        {
                                            txtCharter.Enabled = true;
                                        }

                                        if (HasGroupPower(GroupPowers.ChangeOptions))   //(member.Powers & GroupPowers.ChangeOptions) != 0)
                                        {
                                            chkPublish.Enabled = true;
                                            chkOpenEnrollment.Enabled = true;
                                            chkFee.Enabled = true;
                                            numFee.Enabled = true;
                                            chkMature.Enabled = true;
                                        }
                                        else
                                        {
                                            chkPublish.Enabled = false;
                                            chkOpenEnrollment.Enabled = false;
                                            chkFee.Enabled = false;
                                            numFee.Enabled = false;
                                            chkMature.Enabled = false;
                                        }

                                        chkListInProfile.Enabled = true;
                                        chkGroupNotices.Enabled = true;
                                    }
                                    else
                                    {
                                        chkListInProfile.Enabled = false;
                                        chkGroupNotices.Enabled = false;
                                    }
                                }
                            }
                        }
                    }

                    //this.Refresh();

                    //SortMeberData();
                    

                    //MemberData = sortedDictionary1;

                    SortedMembers.Sort(sortedlist);

                    lstMembers.Invalidate();
                    lstMembers2.Invalidate();
                }
                catch (Exception ex)
                {
                    //string exp = ex.Message;
                }
            }
        }

        //private void SortMeberData()
        //{
        //    List<string> sodic = new List<string>();

        //    foreach (GroupMemberData entry in MemberData.Values)
        //    {
        //        sodic.Add(entry.Name);
        //    }

        //    sodic.Sort();

        //    Dictionary<UUID, GroupMemberData> SortedMemberData = new Dictionary<UUID, GroupMemberData>();

        //    foreach (string id in sodic)
        //    {
        //        UUID suuid = UUID.Zero;
        //        GroupMemberData smsmdat = null;

        //        foreach (GroupMemberData entry in MemberData.Values)
        //        {
        //            if (entry.Name == id)
        //            {
        //                suuid = entry.ID;
        //                smsmdat = entry;
        //                break;
        //            }
        //        }

        //        SortedMemberData.Add(suuid, smsmdat);
        //    }

        //    MemberData = SortedMemberData;
        //}

        public static List<List<UUID>> splitList(List<UUID> locations, int nSize = 80)
        {
            List<List<UUID>> list = new List<List<UUID>>();

            for (int i = 0; i < locations.Count; i += nSize)
            {
                list.Add(locations.GetRange(i, Math.Min(nSize, locations.Count - i)));
            }

            return list;
        } 

        private bool HasGroupPower(GroupPowers pwr)
        {
            //if (!instance.State.Groups.ContainsKey(groupID)) return false;

            //return (instance.State.Groups[groupID].Powers & power) != 0;

            GroupPowers power = GroupPowers.None;

            foreach (GroupRole role in grouproles.Values)
            {
                if (role.Name.ToLower(CultureInfo.CurrentCulture) != "everyone")
                {
                    UUID roleid = role.ID;

                    foreach (var el in grouprolesavs)
                    {
                        if (el.Value == Client.Self.AgentID && el.Key == roleid)
                        {
                            power = role.Powers;

                            foreach (GroupPowers p in Enum.GetValues(typeof(GroupPowers)))
                            {
                                if (p != GroupPowers.None && (power & p) == p)
                                {
                                    if (p == pwr)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool IsGroupOwner()
        {
            foreach (GroupRole role in grouproles.Values)
            {
                if (role.Name.ToLower(CultureInfo.CurrentCulture) == "owners")
                {
                    UUID roleid = role.ID;

                    foreach (var el in grouprolesavs)
                    {
                        if (el.Value == Client.Self.AgentID && el.Key == roleid)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void GroupTitlesHandler(object sender, GroupTitlesReplyEventArgs e)
        {
            if (grouptitles  != e.RequestID)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    GroupTitlesHandler(sender, e);
                }));

                return;
            }

            Titles = e.Titles;

            //UpdateTitles();
        }

        ////private void UpdateTitles()
        ////{
        ////    if (this.InvokeRequired)
        ////    {
        ////        this.BeginInvoke(new MethodInvoker(delegate()
        ////        {
        ////            UpdateTitles();
        ////        }));

        ////        return;
        ////    }
        ////    else
        ////    {
        ////        lock (Titles)
        ////        {
        ////            lstRoles.Items.Clear();

        ////            foreach (KeyValuePair<UUID, GroupTitle> kvp in Titles)
        ////            {
        ////                ListViewItem lvi = new ListViewItem();
        ////                lvi.Text = kvp.Value.Title.ToString()   ;

        ////                ListViewItem.ListViewSubItem lvsi = new ListViewItem.ListViewSubItem();
        ////                lvsi.Text = kvp.Key.ToString();
        ////                lvi.SubItems.Add(lvsi);
        ////                lstRoles.Items.Add(lvi);
        ////            }
        ////        }
        ////    }
        ////}

        private void cmdEject_Click(object sender, EventArgs e)
        {
            if (!ejectpower) return;

            if (lstMembers2.SelectedIndices.Count > 0)   // && lstMembers2.Columns[0].ToString != "none")
            {
                //string li = lstMembers2.SelectedIndices[0];
                int sel = lstMembers2.SelectedIndices[0];
                int ctr = 0;

                UUID avid = UUID.Zero;

                foreach (GroupMemberData entry in SortedMembers)
                {
                    if (ctr == sel)
                    {
                        avid = entry.ID;
                        break;
                    }

                    ctr += 1;
                }

                if (avid != UUID.Zero)
                {
                    try
                    {
                        Client.Groups.EjectUser(Group.ID, avid);
                        ejecttedgroupmember = MemberData[avid].Name;
                        ejectedmemberid = avid;
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Eject from group: " + ex.Message, Helpers.LogLevel.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Select a member to eject!", "METAbolt");
            }
        }

        private void tabGeneral_Click(object sender, EventArgs e)
        {

        }

        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            lstNotices.Items.Clear();
            lstMembers.Items.Clear();
            lstMembers2.Items.Clear();

            label10.Visible = true;

            //// Request the group information
            //Client.Groups.RequestGroupProfile(Group.ID);
            groupmembers = Client.Groups.RequestGroupMembers(Group.ID);
            grouptitles = Client.Groups.RequestGroupTitles(Group.ID);

            Members = new Dictionary<UUID, GroupMember>();
            MemberData.Clear();
            SortedMembers.Clear();
            Titles.Clear();
            grouproles.Clear();
            grouprolesavs.Clear();

            Client.Groups.RequestGroupNoticesList(Group.ID);
            Client.Groups.RequestGroupRoles(Group.ID); 
        }

        private void cmdApply_Click(object sender, EventArgs e)
        {
            GetDets();
            //this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            //GetDets();
            this.Close();
        }

        private void lstMembers2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //chkListRoles.Items.Clear();
            lvAssignedRoles.Items.Clear();

            if (lstMembers2.SelectedIndices.Count > 0)   // && lstMembers2.Columns[0].ToString != "none")
            {
                //string li = lstMembers2.SelectedIndices[0];
                int sel = lstMembers2.SelectedIndices[0];
                int ctr = 0;

                UUID avid = UUID.Zero;

                foreach (GroupMemberData entry in SortedMembers)
                {
                    if (ctr == sel)
                    {
                        avid = entry.ID;
                        break; 
                    }

                    ctr += 1;
                }

                if (avid == UUID.Zero) return;

                checkignore = true;

                ListViewItem everyone = new ListViewItem();
                everyone.Text = "Everyone";
                everyone.Checked = true;
                //everyone.Tag = null;
                lvAssignedRoles.Items.Add(everyone);

                GroupPowers power = GroupPowers.None;

                foreach (GroupRole role in grouproles.Values)
                {
                    if (role.Name.ToLower(CultureInfo.CurrentCulture) != "everyone")
                    {
                        UUID roleid = role.ID;

                        ListViewItem nme = new ListViewItem();
                        nme.Text = role.Name;
                        nme.Tag = role;
                        
                        bool ismember = false;

                        foreach (var el in grouprolesavs)
                        {
                            if (el.Value == avid && el.Key == roleid)
                            {
                                ismember = true;
                                power = role.Powers;
                            }
                        }

                        nme.Checked = ismember;
                        lvAssignedRoles.Items.Add(nme);
                    }

                    //lvwAble.Items.Clear();

                    //foreach (GroupPowers p in Enum.GetValues(typeof(GroupPowers)))
                    //{
                    //    if (p != GroupPowers.None && (power & p) == p)
                    //    {
                    //        lvwAble.Items.Add(p.ToString());
                    //    }
                    //}
                }

                checkignore = false;
            }
        }

        private void frmGroupInfo_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
        }

        private void lstRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string li = UUID.Zero.ToString();
   
            if (lstRoles.SelectedItems.Count > 0)
            {
                for (int i = lstRoles.SelectedItems.Count - 1; i >= 0; i--)
                {
                    li = lstRoles.SelectedItems[i].SubItems[1].Text;
                    textBox3.Text = li;
                }

                //button5.Enabled = true;

                lvRoleMembers.Items.Clear();
                lvwAbilities.Items.Clear();

                if ((UUID)li == UUID.Zero)
                {
                    //lvRoleMembers

                    foreach (GroupMemberData data in MemberData.Values)
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = data.Name;
                        lvi.Tag = data.ID;
                        
                        lvRoleMembers.Items.Add(lvi);
                    }
                }
                else
                {
                    foreach (KeyValuePair<UUID, UUID> name in grouprolesavs)
                    {
                        if (name.Key == (UUID)li)
                        {
                            if (instance.avnames.ContainsKey(name.Value))
                            {
                                ListViewItem lvi = new ListViewItem();
                                lvi.Text = instance.avnames[name.Value];
                                lvi.Tag = name.Value;

                                lvRoleMembers.Items.Add(lvi);
                            }
                        }
                    }
                }
            }
        }

        private void picInsignia_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.SelectAll();  
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            textBox3.SelectAll();
        }

        private void btnInvite_Click(object sender, EventArgs e)
        {
            if (lstRoles.SelectedItems.Count > 0)
            {
                UUID li = (UUID)textBox3.Text;
                (new frmGive(instance, grpid, li)).Show(this);
            }
            else
            {
                MessageBox.Show("Select a role to invite to from under the 'Role' tab above first.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void lstMembers_DoubleClick(object sender, EventArgs e)
        {
            //if (lstMembers.SelectedItems.Count == 1)
            //{
            //    string li = lstMembers.SelectedItems[0].Text;

            //    foreach (GroupMemberData entry in MemberData.Values)
            //    {
            //        if (li == entry.Name)
            //        {
            //            (new frmProfile(instance, entry.Name, entry.ID)).Show();
            //        }
            //    }
            //}
        }

        private void lstMembers2_DoubleClick(object sender, EventArgs e)
        {
            //if (lstMembers2.SelectedItems.Count == 1)
            //{
            //    string li = lstMembers2.SelectedItems[0].Text;

            //    foreach (GroupMemberData entry in MemberData.Values)
            //    {
            //        if (li == entry.Name)
            //        {
            //            (new frmProfile(instance, entry.Name, entry.ID)).Show();
            //        }
            //    }
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lstMembers2.Items.Count  > 0)   // && lstMembers2.Columns[0].ToString != "none")
            {
                Stream tstream;
                saveFileDialog1.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if ((tstream = saveFileDialog1.OpenFile()) != null)
                        {
                            StreamWriter SW = new StreamWriter(tstream);

                            SW.WriteLine("Group: " + this.Group.Name + ",UUID: " + grpid.ToString() + ",Ttl members: " + lstMembers2.Items.Count.ToString(CultureInfo.CurrentCulture) + ",");
                            SW.WriteLine(",,,");
                            SW.WriteLine("Name,UUID,Title,Last online");

                            foreach (GroupMemberData entry in MemberData.Values)
                            {
                                try
                                {
                                    string line = entry.Name + "," + entry.ID.ToString() + "," + entry.Title + "," + entry.LastOnline.ToString(CultureInfo.CurrentCulture);
                                    SW.WriteLine(line);
                                }
                                catch (Exception ex)
                                {
                                    Logger.Log("Group list export: " + ex.Message, Helpers.LogLevel.Warning);
                                }
                            }

                            SW.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);   
                    }
                }
             }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("You must enter a 'subject' first");  
                return;
            }

            // Send the notice
            GroupNotice sgnotice = new GroupNotice();

            sgnotice.Subject = textBox1.Text;
            sgnotice.Message = textBox4.Text;

            if (objID != UUID.Zero)
            {
                sgnotice.AttachmentID = objID;
                sgnotice.OwnerID = Client.Self.AgentID;
                sgnotice.SerializeAttachment();
            }

            Client.Groups.SendGroupNotice(grpid, sgnotice);

            // Set everything back to defaults
            label3.Text = "Archived Notice";
            panel1.Visible = true;
            panel2.Visible = false;
            button2.Enabled = false;
            objID = UUID.Zero;
            textBox6.Text = "Drag-Drop attachment item here";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label3.Text = "Create a Notice";
            panel1.Visible = false;
            panel2.Visible = true;
            button2.Enabled = true; 
        }

        private void tabNotices_Click(object sender, EventArgs e)
        {

        }

        private void cmdRefreshNotices_Click(object sender, EventArgs e)
        {
            lstNotices.Items.Clear();
            Client.Groups.RequestGroupNoticesList(grpid);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lstNotices_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;

            if (lstNotices.SelectedItems.Count > 0)
            {
                textBox5.Text = string.Empty;

                GroupNoticesListEntry gnotice = (GroupNoticesListEntry)lstNotices.SelectedItems[0].Tag;
                Client.Groups.RequestGroupNotice(gnotice.NoticeID);   
            }
        }

        private void textBox6_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode node = e.Data.GetData(typeof(TreeNode)) as TreeNode;

            if (node == null) return;

            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                InventoryBase io = (InventoryBase)node.Tag;

                if (node.Tag is InventoryItem)
                {
                    InventoryItem item = (InventoryItem)io;

                    if ((item.Permissions.OwnerMask & PermissionMask.Copy) != PermissionMask.Copy)
                    {
                        MessageBox.Show("This is a 'no copy' item. You can't attach no transfer/copy items.", "Warning", MessageBoxButtons.OK);
                        return;
                    }

                    objID = item.UUID;
                    textBox6.Text = item.Name;
                }
            }
        }

        private void textBox6_DragEnter(object sender, DragEventArgs e)
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

        private void textBox6_DragOver(object sender, DragEventArgs e)
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

        private void chkPublish_CheckedChanged(object sender, EventArgs e)
        {
            if (floading) return;

            Profile.AllowPublish = chkPublish.Checked;
            Client.Groups.UpdateGroup(Profile.ID, Profile);      
        }

        private void chkOpenEnrollment_CheckedChanged(object sender, EventArgs e)
        {
            if (floading) return;

            Profile.OpenEnrollment = chkOpenEnrollment.Checked;
            Client.Groups.UpdateGroup(Profile.ID, Profile);
        }

        private void chkFee_CheckedChanged(object sender, EventArgs e)
        {
            if (floading) return;

            if (!chkFee.Checked)
            {
                Profile.MembershipFee = 0;
                Client.Groups.UpdateGroup(Profile.ID, Profile);
            }

            numFee.Enabled = chkFee.Checked;
        }

        private void numFee_ValueChanged(object sender, EventArgs e)
        {
            if (floading) return;

            Profile.MembershipFee = Convert.ToInt32(numFee.Value);
            Client.Groups.UpdateGroup(Profile.ID, Profile);
        }

        private void chkGroupNotices_CheckedChanged(object sender, EventArgs e)
        {
            if (floading) return;

            //Profile.AcceptNotices = chkGroupNotices.Checked;
            //Client.Groups.UpdateGroup(Profile.ID, Profile);
            Client.Groups.SetGroupAcceptNotices(Profile.ID, chkGroupNotices.Checked, chkListInProfile.Checked);
            Client.Groups.RequestCurrentGroups();

            //instance.State.Groups[Profile.ID].AcceptNotices = chkGroupNotices.Checked;
        }

        private void chkMature_CheckedChanged(object sender, EventArgs e)
        {
            if (floading) return;

            Profile.MaturePublish = chkMature.Checked;
            Client.Groups.UpdateGroup(Profile.ID, Profile);
        }

        private void txtCharter_Leave(object sender, EventArgs e)
        {
            if (tchanged)
            {
                Profile.Charter = txtCharter.Text;
                Client.Groups.UpdateGroup(Profile.ID, Profile);
                tchanged = false;
            }
        }

        private void txtCharter_TextChanged(object sender, EventArgs e)
        {
            if (floading) return;

            tchanged = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
        }

        private void frmGroupInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            form_dispose();
        }

        private void lstMembers_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (sortedlist.CurrentOrder == MemberSorter.SortOrder.Ascending)
            {
                sortedlist.CurrentOrder = MemberSorter.SortOrder.Descending;
            }
            else
            {
                sortedlist.CurrentOrder = MemberSorter.SortOrder.Ascending;
            }

            if (e.Column == 0)
            {
                sortedlist.SortBy = MemberSorter.SortByColumn.Name;
            }
            else if (e.Column == 1)
            {
                sortedlist.SortBy = MemberSorter.SortByColumn.Title;
            }
            else if (e.Column == 2)
            {
                sortedlist.SortBy = MemberSorter.SortByColumn.LastOnline;
            }

            SortedMembers.Sort(sortedlist);

            lstMembers.Invalidate();
        }

        private void lstMembers2_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (sortedlist.CurrentOrder == MemberSorter.SortOrder.Ascending)
            {
                sortedlist.CurrentOrder = MemberSorter.SortOrder.Descending;
            }
            else
            {
                sortedlist.CurrentOrder = MemberSorter.SortOrder.Ascending;
            }

            if (e.Column == 0)
            {
                sortedlist.SortBy = MemberSorter.SortByColumn.Name;
            }
            else if (e.Column == 1)
            {
                sortedlist.SortBy = MemberSorter.SortByColumn.Contribution;
            }
            else if (e.Column == 2)
            {
                sortedlist.SortBy = MemberSorter.SortByColumn.LastOnline;
            }

            SortedMembers.Sort(sortedlist);

            lstMembers2.Invalidate();
        }

        private void lstNotices_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwDateColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwDateColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwDateColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwDateColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwDateColumnSorter.SortColumn = e.Column;
                lvwDateColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lstNotices.Sort();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (lstRoles.SelectedItems.Count > 0)
            {
                //instance.State.Groups[grpid]
                UUID li = (UUID)textBox3.Text;
                Client.Groups.DeleteRole(grpid, li);
            }
            else
            {
                MessageBox.Show("First you must select a role from the list above.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (numFee.Value > 0)
            {
                DialogResult res = MessageBox.Show("Are you sure you want to JOIN this Group for L$" + numFee.Value.ToString(CultureInfo.CurrentCulture) + "?", "METAbolt", MessageBoxButtons.YesNo);

                if (res == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            Client.Groups.RequestJoinGroup(grpid);
        }

        private void lstMembers_MouseEnter(object sender, EventArgs e)
        {
            lstMembers.Cursor = Cursors.Hand;
        }

        private void lstMembers_MouseLeave(object sender, EventArgs e)
        {
            lstMembers.Cursor = Cursors.Default;
        }

        private void lstMembers2_MouseEnter(object sender, EventArgs e)
        {
            lstMembers2.Cursor = Cursors.Hand;
        }

        private void lstMembers2_MouseLeave(object sender, EventArgs e)
        {
            lstMembers2.Cursor = Cursors.Default;
        }

        private void chkListInProfile_CheckedChanged(object sender, EventArgs e)
        {
            if (floading) return;

            Client.Groups.SetGroupAcceptNotices(Profile.ID, chkGroupNotices.Checked, chkListInProfile.Checked);
            Client.Groups.RequestCurrentGroups();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            GroupRole role = new GroupRole();
            role.Name = textBox7.Text;
            role.Title = textBox8.Text;
            role.Description = textBox9.Text;

            //GroupPowers powers = new GroupPowers();
            //role.Powers

            Client.Groups.CreateRole(grpid, role);

            panel3.Visible = false;
            textBox7.Text = string.Empty;
            textBox8.Text = string.Empty;
            textBox9.Text = string.Empty;
        }

        private void lvAssignedRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvwAble.Items.Clear();

            if (lvAssignedRoles.SelectedItems.Count > 0)   // && lstMembers2.Columns[0].ToString != "none")
            {
                if (lvAssignedRoles.SelectedItems[0].Tag == null)
                {
                    //// everyone
                    //GroupPowers power = GroupPowers.None;

                    //foreach (GroupPowers p in Enum.GetValues(typeof(GroupPowers)))
                    //{
                    //    if (p != GroupPowers.None && (power & p) == p)
                    //    {
                    //        lvwAble.Items.Add(p.ToString());
                    //    }
                    //}

                    foreach (GroupRole role in grouproles.Values)
                    {
                        if (role.Name.ToLower(CultureInfo.CurrentCulture) == "everyone")
                        {
                            GroupPowers power = role.Powers;

                            foreach (GroupPowers p in Enum.GetValues(typeof(GroupPowers)))
                            {
                                if (p != GroupPowers.None && (power & p) == p)
                                {
                                    lvwAble.Items.Add(p.ToString());
                                }
                            }
                        }
                    }
                }
                else
                {
                    GroupPowers power = GroupPowers.None;
                    GroupRole role = (GroupRole)lvAssignedRoles.SelectedItems[0].Tag;

                    power = role.Powers;

                    foreach (GroupPowers p in Enum.GetValues(typeof(GroupPowers)))
                    {
                        if (p != GroupPowers.None && (power & p) == p)
                        {
                            lvwAble.Items.Add(p.ToString());
                        }
                    }
                }
            }
        }

        private void lvAssignedRoles_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (floading) return;

            if (checkignore) return;

            ListViewItem item = e.Item as ListViewItem;
            
            if (!HasGroupPower(GroupPowers.AssignMember))
            {
                //if (item.Checked)
                //{
                //    item.Checked = true;
                //}
                //else
                //{
                //    item.Checked = false;
                //}

                return;
            }

            GroupRole role;

            if (item.Tag != null)
            {
                role = (GroupRole)item.Tag;
            }
            else
            {
                role.Name = "Everyone";
                role.ID = UUID.Zero;
                role.Powers = GroupPowers.None;
            }


            string li = lstMembers2.SelectedItems[0].Text;
            UUID avid = UUID.Zero;

            try
            {
                foreach (GroupMemberData entry in MemberData.Values)
                {
                    if (li == entry.Name)
                    {
                        avid = entry.ID;
                        break;
                    }
                }

                if (item.Checked)
                {
                    foreach (var el in grouprolesavs)
                    {
                        if (el.Value == avid && el.Key == role.ID)
                        {
                            grouprolesavs.Add(new KeyValuePair<UUID, UUID>(role.ID, avid));
                        }
                    }
                }
                else
                {
                    foreach (var el in grouprolesavs)
                    {
                        if (el.Value == avid && el.Key == role.ID)
                        {
                            grouprolesavs.Remove(new KeyValuePair<UUID, UUID>(role.ID, avid));
                        }
                    }
                }
            }
            catch { ; }

            // The section below has been borrowed from Radegast (c) 2009-2011 Radegast Development Team
            List<GroupRoleChangesPacket.RoleChangeBlock> changes = new List<GroupRoleChangesPacket.RoleChangeBlock>();
            GroupRoleChangesPacket.RoleChangeBlock rc = new GroupRoleChangesPacket.RoleChangeBlock();

            rc.MemberID = avid;
            rc.RoleID = role.ID;
            rc.Change = item.Checked ? 0u : 1u;
            changes.Add(rc);

            GroupRoleChangesPacket packet = new GroupRoleChangesPacket();
            packet.AgentData.AgentID = Client.Self.AgentID;
            packet.AgentData.SessionID = Client.Self.SessionID;
            packet.AgentData.GroupID = Profile.ID;
            packet.RoleChange = changes.ToArray();
            Client.Network.CurrentSim.SendPacket(packet);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Client.Self.InstantMessage(Client.Self.Name, imsg.FromAgentID, string.Empty, imsg.IMSessionID, InstantMessageDialog.GroupNoticeInventoryAccepted, InstantMessageOnline.Offline, instance.SIMsittingPos(), Client.Network.CurrentSim.RegionID, assetfolder.GetBytes());
            //button1.Enabled = false;

            //if (assettype != AssetType.Notecard && assettype != AssetType.LSLText)
            //{
            //    MessageBox.Show("Attachment has been saved to your inventory", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //else
            //{
            //    List<InventoryBase> contents = Client.Inventory.FolderContents(assetfolder, Client.Self.AgentID, false, true, InventorySortOrder.ByName | InventorySortOrder.ByDate, 5000);

            //    if (contents != null)
            //    {
            //        foreach (InventoryBase ibase in contents)
            //        {
            //            if (ibase is InventoryItem)
            //            {
            //                if (ibase.Name.ToLower() == filename.ToLower())
            //                {
            //                    //UUID itemid = item.AssetUUID;
            //                    InventoryItem item = (InventoryItem)ibase;

            //                    switch (assettype)
            //                    {
            //                        case AssetType.Notecard:
            //                            (new frmNotecardEditor(instance, item)).Show();
            //                            break;
            //                        case AssetType.LSLText:
            //                            (new frmScriptEditor(instance, item)).Show();
            //                            break;
            //                    }

            //                    return;
            //                }
            //            }
            //        }
            //    }
            //}

            MessageBox.Show("Attachment has been saved to your inventory", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBox5_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (e.LinkText.StartsWith("http://slurl.", StringComparison.CurrentCultureIgnoreCase))
            {
                try
                {
                    // Open up the TP form here
                    string encoded = HttpUtility.UrlDecode(e.LinkText);
                    string[] split = encoded.Split(new Char[] { '/' });
                    //string[] split = e.LinkText.Split(new Char[] { '/' });
                    string sim = split[4].ToString();
                    double x = Convert.ToDouble(split[5].ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
                    double y = Convert.ToDouble(split[6].ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
                    double z = Convert.ToDouble(split[7].ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);

                    (new frmTeleport(instance, sim, (float)x, (float)y, (float)z, false)).Show();
                }
                catch { ; }

            }
            else if (e.LinkText.StartsWith("http://maps.secondlife"))
            {
                try
                {
                    // Open up the TP form here
                    string encoded = HttpUtility.UrlDecode(e.LinkText);
                    string[] split = encoded.Split(new Char[] { '/' });
                    //string[] split = e.LinkText.Split(new Char[] { '/' });
                    string sim = split[4].ToString();
                    double x = Convert.ToDouble(split[5].ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
                    double y = Convert.ToDouble(split[6].ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
                    double z = Convert.ToDouble(split[7].ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);

                    (new frmTeleport(instance, sim, (float)x, (float)y, (float)z, true)).Show();
                }
                catch { ; }

            }
            else if (e.LinkText.Contains("http://mbprofile:"))
            {
                try
                {
                    string encoded = HttpUtility.UrlDecode(e.LinkText);
                    string[] split = encoded.Split(new Char[] { '/' });
                    //string[] split = e.LinkText.Split(new Char[] { '#' });
                    string aavname = split[0].ToString();
                    string[] avnamesplit = aavname.Split(new Char[] { '#' });
                    aavname = avnamesplit[0].ToString();

                    split = e.LinkText.Split(new Char[] { ':' });
                    string elink = split[2].ToString();
                    split = elink.Split(new Char[] { '&' });

                    UUID avid = (UUID)split[0].ToString();

                    (new frmProfile(instance, aavname, avid)).Show();
                }
                catch { ; }
            }
            else if (e.LinkText.Contains("http://secondlife:///"))
            {
                // Open up the Group Info form here
                string encoded = HttpUtility.UrlDecode(e.LinkText);
                string[] split = encoded.Split(new Char[] { '/' });
                //string[] split = e.LinkText.Split(new Char[] { '/' });
                UUID uuid = (UUID)split[7].ToString();

                if (uuid != UUID.Zero && split[6].ToString().ToLower(CultureInfo.CurrentCulture) == "group")
                {
                    frmGroupInfo frm = new frmGroupInfo(uuid, instance);
                    frm.Show();
                }
            }
            else if (e.LinkText.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) || e.LinkText.StartsWith("ftp://", StringComparison.CurrentCultureIgnoreCase) || e.LinkText.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            else
            {
                System.Diagnostics.Process.Start("http://" + e.LinkText);
            }
        }

        private void lstMembers_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            GroupMemberData fmem = null;

            //int i = 0;

            try
            {
                //foreach (GroupMemberData entry in MemberData.Values)
                //{
                //    if (i == e.ItemIndex)
                //    {
                //        fmem = entry;
                //        break;
                //    }

                //    i += 1;
                //}

                fmem = SortedMembers[e.ItemIndex];
            }
            catch
            {
                e.Item = new ListViewItem();
                return;
            }

            ListViewItem lvi = new ListViewItem();
            lvi.Text = fmem.Name;

            ListViewItem.ListViewSubItem lvsi = new ListViewItem.ListViewSubItem();

            lvsi.Text = fmem.Title;
            lvi.SubItems.Add(lvsi);

            lvsi = new ListViewItem.ListViewSubItem();
            lvsi.Text = fmem.LastOnline;   // member.OnlineStatus;
            lvi.SubItems.Add(lvsi);

            lvi.Tag = fmem;
            lvi.ToolTipText = "Double click to view " + lvi.Text + "'s profile";

            e.Item = lvi;
        }

        private void lstMembers2_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            GroupMemberData fmem = null;

            //int i = 0;

            try
            {
                //foreach (GroupMember member in Members.Values)
                //{
                //    if (i == e.ItemIndex)
                //    {
                //        fmem = member;
                //        break;
                //    }

                //    i += 1;
                //}

                fmem = SortedMembers[e.ItemIndex];
            }
            catch
            {
                e.Item = new ListViewItem();
                return;
            }

            ListViewItem lvi = new ListViewItem();
            lvi.Text = fmem.Name;

            ListViewItem.ListViewSubItem lvsi = new ListViewItem.ListViewSubItem();
            lvsi.Text = fmem.Contribution.ToString(CultureInfo.CurrentCulture);
            lvi.SubItems.Add(lvsi);

            lvsi = new ListViewItem.ListViewSubItem();
            lvsi.Text = fmem.LastOnline;   // member.OnlineStatus;
            lvi.SubItems.Add(lvsi);

            lvi.Tag = Members[fmem.ID];

            e.Item = lvi;
        }

        private void lstMembers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem grpitem = lstMembers.GetItemAt(e.X, e.Y);

            if (grpitem != null)
            {
                foreach (GroupMemberData entry in MemberData.Values)
                {
                    if (grpitem.Text == entry.Name)
                    {
                        (new frmProfile(instance, entry.Name, entry.ID)).Show();
                    }
                }
            }
        }

        private void lstMembers2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem grpitem = lstMembers2.GetItemAt(e.X, e.Y);

            if (grpitem != null)
            {
                foreach (GroupMemberData entry in MemberData.Values)
                {
                    if (grpitem.Text == entry.Name)
                    {
                        (new frmProfile(instance, entry.Name, entry.ID)).Show();
                    }
                }
            }
        }

        private void lstMembers_SearchForVirtualItem(object sender, SearchForVirtualItemEventArgs e)
        {
            double x = 0;

            if (Double.TryParse(e.Text, out x))
            {
                x = Math.Sqrt(x);
                x = Math.Round(x);
                e.Index = (int)x;
            }
        }

        private void lstMembers2_SearchForVirtualItem(object sender, SearchForVirtualItemEventArgs e)
        {
            double x = 0;

            if (Double.TryParse(e.Text, out x))
            {
                x = Math.Sqrt(x);
                x = Math.Round(x);
                e.Index = (int)x;
            }
        }

        private void lvRoleMembers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvRoleMembers.SelectedItems.Count > 0)
            {
                lvwAbilities.Items.Clear();

                string selectedrole = lstRoles.SelectedItems[0].Text;
                UUID selectedmember = (UUID)lvRoleMembers.SelectedItems[0].Tag;

                foreach (GroupRole role in grouproles.Values)
                {
                    if (role.Name.ToLower(CultureInfo.CurrentCulture) == selectedrole.ToLower(CultureInfo.CurrentCulture))
                    {
                        UUID roleid = role.ID;

                        foreach (var el in grouprolesavs)
                        {
                            if (el.Value == selectedmember && el.Key == roleid)
                            {
                                 GroupPowers power = role.Powers;

                                foreach (GroupPowers p in Enum.GetValues(typeof(GroupPowers)))
                                {
                                    if (p != GroupPowers.None && (power & p) == p)
                                    {
                                        lvwAbilities.Items.Add(p.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public class MemberSorter : IComparer<GroupMemberData>
    {
        public enum SortByColumn
        {
            Name,
            Title,
            LastOnline,
            Contribution
        }

        public enum SortOrder
        {
            Ascending,
            Descending
        }

        public SortOrder CurrentOrder = SortOrder.Ascending;
        public SortByColumn SortBy = SortByColumn.Name;

        public int Compare(GroupMemberData member1, GroupMemberData member2)
        {
            try
            {
                switch (SortBy)
                {
                    case SortByColumn.Name:
                        if (CurrentOrder == SortOrder.Ascending)
                        {
                            return string.Compare(member1.Name, member2.Name);
                        }
                        else
                        {
                            return string.Compare(member2.Name, member1.Name);
                        }

                    case SortByColumn.LastOnline:
                        string a = member1.LastOnline;
                        string b = member2.LastOnline;
                        string[] d1;
                        string[] d2;
                        int compareResult;

                        if (a.Contains("/"))
                        {
                            d1 = a.Split('/');
                            a = d1[2] + d1[1] + d1[0];
                        }

                        if (b.Contains("/"))
                        {
                            d2 = b.Split('/');
                            b = d2[2] + d2[1] + d2[0];
                        }

                        compareResult = SafeNativeDateMethods.StrCmpLogicalW(a, b);

                        if (CurrentOrder == SortOrder.Ascending)
                        {
                            return compareResult;
                        }
                        else
                        {
                            return (-compareResult);
                        }

                    case SortByColumn.Contribution:
                        if (member1.Contribution < member2.Contribution)
                        {
                            return CurrentOrder == SortOrder.Ascending ? -1 : 1;
                        }
                        else if (member1.Contribution > member2.Contribution)
                        {
                            return CurrentOrder == SortOrder.Ascending ? 1 : -1;
                        }
                        else
                        {
                            return 0;
                        }

                    case SortByColumn.Title:
                        if (CurrentOrder == SortOrder.Ascending)
                        {
                            return string.Compare(member1.Title, member2.Title);
                        }
                        else
                        {
                            return string.Compare(member2.Title, member1.Title);
                        }
                }

                return 0;
            }
            catch { return 0; }
        }        
    }

    public class GroupMemberData
    {
        public UUID ID;
        public string Name;
        public string Title;
        public string LastOnline;
        public ulong Powers;
        public bool IsOwner;
        public int Contribution;
    }
}
