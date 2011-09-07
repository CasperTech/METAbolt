//  Copyright (c) 2008-2011, www.metabolt.net
//  All rights reserved.

//  Redistribution and use in source and binary forms, with or without modification, 
//  are permitted provided that the following conditions are met:

//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright notice, 
//    this list of conditions and the following disclaimer in the documentation 
//    and/or other materials provided with the distribution. 
//  * Neither the name METAbolt nor the names of its contributors may be used to 
//    endorse or promote products derived from this software without specific prior 
//    written permission. 

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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
//using SLNetworkComm;
using OpenMetaverse.Imaging;
using OpenMetaverse.Assets;
using OpenMetaverse.StructuredData;
using System.Media; 

namespace METAbolt
{
    public partial class frmGroupNotice : Form
    {
        private METAboltInstance instance;
        private GridClient client;
        //private SLNetCom netcom;
        private InstantMessage imsg;
        private UUID assetfolder = UUID.Zero;
        private AssetType assettype;
        private string filename = string.Empty;
        Group profile;

        public frmGroupNotice(METAboltInstance instance, InstantMessageEventArgs e)
        {
            InitializeComponent();
            this.instance = instance;
            client = this.instance.Client;
            //netcom = this.instance.Netcom;
            imsg = e.IM;

            Disposed += new EventHandler(GroupNotice_Disposed);

            client.Groups.GroupProfile += new EventHandler<GroupProfileEventArgs>(GroupProfileHandler);

            if (instance.Config.CurrentConfig.PlayGroupNoticeReceived)
            {
                SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.Group_Notice);
                simpleSound.Play();
                simpleSound.Dispose();
            }

            PrepareGroupNotice();

            timer1.Interval = instance.DialogTimeOut;
            timer1.Enabled = true;
            timer1.Start();
        }

        private void GroupNotice_Disposed(object sender, EventArgs e)
        {
            client.Groups.GroupProfile -= new EventHandler<GroupProfileEventArgs>(GroupProfileHandler);
        }

        private void frmGroupNotice_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            //PrepareGroupNotice();
        }

        private void PrepareGroupNotice()
        {
            //string fromAgent = imsg.FromAgentName;
            UUID fromAgentID = imsg.FromAgentID;
            //UUID Grp = UUID.Zero;

            client.Groups.RequestGroupProfile(fromAgentID);

            if (instance.State.Groups.ContainsKey(fromAgentID))
            {
                string grp = instance.State.Groups[fromAgentID].Name.ToUpper();
                label2.Text = "Sent by: " + imsg.FromAgentName + ", " + grp;
            }
            else
            {
                label2.Text = "Sent by: " + imsg.FromAgentName;
            }

            int rep = imsg.Message.IndexOf('|');
            string msgtitle = imsg.Message.Substring(0, rep);
            rtbTitle.Text = msgtitle;
            MakeBold(msgtitle, 0, FontStyle.Bold);

            DateTime dte = DateTime.Now;

            dte = this.instance.State.GetTimeStamp(dte);

            if (instance.Config.CurrentConfig.UseSLT)
            {
                string _timeZoneId = "Pacific Standard Time";
                DateTime startTime = DateTime.UtcNow;
                TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId);
                dte = TimeZoneInfo.ConvertTime(startTime, TimeZoneInfo.Utc, tst);
            }

            string atext = "\n" + dte.DayOfWeek.ToString() + "," + dte.ToString();
            rtbTitle.AppendText(atext);
            MakeBold(atext, (msgtitle.Length + 1), FontStyle.Regular);

            string msgbody = imsg.Message.Substring(rep + 1);
            rtbBody.Text = msgbody;

            try
            {
                // Check for attachment
                if (imsg.BinaryBucket[0] != 0)
                {

                    //string s1 = Utils.BytesToString(imsg.BinaryBucket[0]);

                    //OSD des = OSDParser.DeserializeLLSDBinary(imsg.BinaryBucket);

                    //OSDMap desmap = (OSDMap)des;
                    //UUID itemuuid = (UUID)desmap["item_id"].ToString();
                    //UUID owneruuid = (UUID)desmap["owner_id"].ToString();

                    assettype = (AssetType)imsg.BinaryBucket[1];

                    assetfolder = client.Inventory.FindFolderForType(assettype);

                    if (imsg.BinaryBucket.Length > 18)
                    {
                        filename = Utils.BytesToString(imsg.BinaryBucket, 18, imsg.BinaryBucket.Length - 19);
                    }
                    else
                    {
                        filename = string.Empty;  
                    }
                    //string[] filecontent = filename.Split('\b');

                    //if (filename.Contains('\b'))
                    //{
                    //    filename = filecontent[1];
                    //}
                    //else
                    //{
                    //    filename = filecontent[0];
                    //}

                    panel1.Visible = true;
                    label4.Visible = true;
                    label3.Text = filename;

                    if (filename.Length > label3.Size.Width)
                    {
                        filename = filename.Substring(0, label3.Size.Width - 3) + "...";
                    }

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
            }
            catch (Exception ex)
            {
                Logger.Log("Group notice attachment error: " + ex.Message, Helpers.LogLevel.Error);     
            }
        }

        private void MakeBold(string otext, int start, FontStyle bold)
        {
            Font nFont = new Font("Microsoft Sans Serif", 10, bold);
            rtbTitle.Select(start, otext.Length);
            rtbTitle.SelectionFont = nFont;
            nFont.Dispose(); 
        }

        private void GroupProfileHandler(object sender, GroupProfileEventArgs e)
        {
            profile = e.Group;

            if (imsg.FromAgentID != profile.ID) return;   

            client.Assets.RequestImage(profile.InsigniaID, ImageType.Normal, Assets_OnImageReceived);

            client.Groups.GroupProfile -= new EventHandler<GroupProfileEventArgs>(GroupProfileHandler);
        }

        void Assets_OnImageReceived(TextureRequestState image, AssetTexture texture)
        {
            if (texture.AssetID == profile.InsigniaID)
            {
                ManagedImage imgData;
                Image bitmap;

                OpenJPEG.DecodeToImage(texture.AssetData, out imgData, out bitmap);

                BeginInvoke(new MethodInvoker(delegate()
                {
                    picInsignia.Image = bitmap;
                }));                               
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmGroupNotice_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.instance.NoticeCount -= 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client.Self.InstantMessage(client.Self.Name, imsg.FromAgentID, string.Empty, imsg.IMSessionID, InstantMessageDialog.GroupNoticeInventoryAccepted, InstantMessageOnline.Offline, instance.SIMsittingPos(), client.Network.CurrentSim.RegionID, assetfolder.GetBytes());
            button1.Enabled = false;

            if (assettype != AssetType.Notecard && assettype != AssetType.LSLText)
            {
                MessageBox.Show("Attachment has been saved to your inventory", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                List<InventoryBase> contents = client.Inventory.FolderContents(assetfolder, client.Self.AgentID, false, true, InventorySortOrder.ByName | InventorySortOrder.ByDate, 5000);

                if (contents != null)
                {
                    foreach (InventoryBase ibase in contents)
                    {
                        InventoryItem item = (InventoryItem)ibase;

                        if (item.AssetType != AssetType.Folder)
                        {
                            if (item.Name.ToLower() == filename.ToLower())
                            {
                                //UUID itemid = item.AssetUUID;

                                switch (assettype)
                                {
                                    case AssetType.Notecard:
                                        (new frmNotecardEditor(instance, item)).Show();
                                        break;
                                    case AssetType.LSLText:
                                        (new frmScriptEditor(instance, item)).Show();
                                        break;
                                }

                                return;
                            }
                        }
                    }
                }
            }
        }

        static IEnumerable<T> ReverseIterator<T>(IList<T> list)
        {
            int count = list.Count;
            for (int i = count - 1; i >= 0; --i)
            {
                yield return list[i];
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
