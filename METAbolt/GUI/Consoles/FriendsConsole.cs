//  Copyright (c) 2008 - 2013, www.metabolt.net (METAbolt)
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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
//using SLNetworkComm;
using System.Media;
using ExceptionReporting;
using System.Threading;
using System.Globalization;

namespace METAbolt
{
    public partial class FriendsConsole : UserControl
    {
        private METAboltInstance instance;
        private GridClient client;
        private FriendInfo selectedFriend;
        //private SLNetCom netcom;
        private FileConfig fconfig;
        Dictionary<string, Dictionary<string, string>> fgrps;

        private bool settingFriend = false;
        private ExceptionReporter reporter = new ExceptionReporter();

        internal class ThreadExceptionHandler
        {
            public void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
            {
                ExceptionReporter reporter = new ExceptionReporter();
                reporter.Show(e.Exception);
            }
        }

        public FriendsConsole(METAboltInstance instance)
        {
            InitializeComponent();
            Disposed += new EventHandler(FriendsConsole_Disposed);

            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.instance = instance;
            client = this.instance.Client;
            //netcom = this.instance.Netcom;

            client.Friends.FriendshipTerminated += new EventHandler<FriendshipTerminatedEventArgs>(Friends_OnFriendTerminated);
            client.Friends.FriendshipResponse += new EventHandler<FriendshipResponseEventArgs>(Friends_OnFriendResponse);
            client.Friends.FriendNames += new EventHandler<FriendNamesEventArgs>(Friends_OnFriendNamesReceived);
            client.Friends.FriendOffline += new EventHandler<FriendInfoEventArgs>(Friends_OnFriendOffline);
            client.Friends.FriendOnline += new EventHandler<FriendInfoEventArgs>(Friends_OnFriendOnline);
            client.Friends.FriendRightsUpdate += new EventHandler<FriendInfoEventArgs>(Friends_OnFriendRights);
            //client.Avatars.DisplayNameUpdate += new EventHandler<DisplayNameUpdateEventArgs>(Avatar_DisplayNameUpdated);    

            //InitializeFriendsList();
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

        //private void Avatar_DisplayNameUpdated(object sender, DisplayNameUpdateEventArgs e)
        //{
        //    string old = e.OldDisplayName;
        //    string newname = e.DisplayName.DisplayName;
        //    //txtDisplayName.Text = newname;

        //    List<FriendInfo> friendslist = this.instance.State.AvatarFriends;

        //    if (friendslist.Count > 0)
        //    {
        //        foreach (FriendInfo friend in friendslist)
        //        {
        //            if (friend.UUID == e.DisplayName.ID)
        //            {
        //                friend.Name = e.DisplayName.DisplayName + " (" + e.DisplayName.UserName + ")";
        //            }
        //        }
        //    }
        //}

        public void InitializeFriendsList()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => InitializeFriendsList()));
                return;
            }

            List<FriendInfo> friendslist = client.Friends.FriendList.FindAll(delegate(FriendInfo friend) { return true; });

            this.instance.State.AvatarFriends = friendslist;

            if (friendslist.Count > 0)
            {
                lbxFriends.BeginUpdate();
                lbxFriends.Items.Clear();

                foreach (FriendInfo friend in friendslist)
                {
                    //bool dnavailable = client.Avatars.DisplayNamesAvailable();

                    //if (dnavailable)
                    //{
                    //    List<UUID> avIDs = new List<UUID>();
                    //    avIDs.Add(friend.UUID);
                    //    //client.Avatars.GetDisplayNames(avIDs, DisplayNameReceived);
                    //}

                    //lbxFriends.Items.Add(new FriendsListItem(friend));
                    lbxFriends.Items.Add(friend);
                }

                //lbxFriends.Sort();
                lbxFriends.EndUpdate();

                lblFriendName.Text = string.Empty;  
            }
        }

        private void DisplayNameReceived(bool success, AgentDisplayName[] names, UUID[] badIDs)
        {
            //if (success)
            //{
            //    this.BeginInvoke(new MethodInvoker(delegate()
            //    {
            //        List<FriendInfo> friendslist = this.instance.State.AvatarFriends;

            //        if (friendslist.Count > 0)
            //        {
            //            foreach (FriendInfo friend in friendslist)
            //            {
            //                if (friend.UUID == names[0].ID)
            //                {
            //                    if (names[0].DisplayName != string.Empty)
            //                    {
            //                        if (!names[0].DisplayName.ToLower().Contains("resident") && !names[0].DisplayName.ToLower().Contains(" "))
            //                        {
            //                            friend.Name = names[0].DisplayName + " (" + names[0].UserName + ")";

            //                            int s = lbxFriends.FindString(names[0].LegacyFullName);

            //                            if (s > -1)
            //                            {
            //                                lbxFriends.Items[s] = friend.Name;
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }));
            //}
        }

        public void FriendsConsole_Disposed(object sender, EventArgs e)
        {
            client.Friends.FriendshipTerminated -= new EventHandler<FriendshipTerminatedEventArgs>(Friends_OnFriendTerminated);
            client.Friends.FriendshipResponse -= new EventHandler<FriendshipResponseEventArgs>(Friends_OnFriendResponse);
            client.Friends.FriendNames -= new EventHandler<FriendNamesEventArgs>(Friends_OnFriendNamesReceived);
            client.Friends.FriendOffline -= new EventHandler<FriendInfoEventArgs>(Friends_OnFriendOffline);
            client.Friends.FriendOnline -= new EventHandler<FriendInfoEventArgs>(Friends_OnFriendOnline);
            client.Friends.FriendRightsUpdate -= new EventHandler<FriendInfoEventArgs>(Friends_OnFriendRights);
            //client.Avatars.DisplayNameUpdate -= new EventHandler<DisplayNameUpdateEventArgs>(Avatar_DisplayNameUpdated);

            try
            {
                if (fconfig != null)
                {
                    fconfig.Save();
                }
            }
            catch { ; }
        }

        private void RefreshFriendsList()
        {
            //if (InvokeRequired)
            //{
            //    BeginInvoke(new MethodInvoker(() => RefreshFriendsList()));
            //    return;
            //}

            if (cbofgroups.SelectedIndex == 0 || cbofgroups.SelectedIndex == -1)
            {
                InitializeFriendsList();
            }
            else
            {
                GetGroupFriends(cbofgroups.SelectedItem.ToString());
            }

            SetFriend(selectedFriend);
        }

        private void Friends_OnFriendResponse(object sender, FriendshipResponseEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => Friends_OnFriendResponse(sender, e)));
                return;
            }

            if (e.Accepted)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    RefreshFriendsList();
                }));
            }
        }

        private void Friends_OnFriendTerminated(object sender, FriendshipTerminatedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => Friends_OnFriendTerminated(sender, e)));
                return;
            }

            BeginInvoke(new MethodInvoker(delegate()
            {
                RemoveFriendFromAllGroups(e.AgentID.ToString()); 
                RefreshFriendsList();
            }));
        }

        private void RemoveFriendFromAllGroups(string uuid)
        {
            fgrps = fconfig.FriendGroups;

            foreach (KeyValuePair<string, Dictionary<string, string>> fr in fgrps)
            {
                string header = fr.Key.ToString();
                //Dictionary<string, string> rec = fr.Value;

                Dictionary<string, string> grps;

                fgrps.TryGetValue(header, out grps);

                foreach (KeyValuePair<string, string> s in grps)
                {
                    if (s.Key == uuid)
                    {
                        fconfig.removeFriendFromGroup(header,uuid);
                    }
                }
            }

            fgrps = fconfig.FriendGroups;
        }

        //Separate thread
        private void Friends_OnFriendOffline(object sender, FriendInfoEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => Friends_OnFriendOffline(sender, e)));
                return;
            }

            BeginInvoke(new MethodInvoker(delegate()
            {
                RefreshFriendsList();
            }));
        }

        //Separate thread
        private void Friends_OnFriendOnline(object sender, FriendInfoEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => Friends_OnFriendOnline(sender, e)));
                return;
            }

            BeginInvoke(new MethodInvoker(delegate()
            {
                RefreshFriendsList();
            }));
        }

        private void Friends_OnFriendRights(object sender, FriendInfoEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => Friends_OnFriendRights(sender, e)));
                return;
            }

            BeginInvoke(new MethodInvoker(delegate()
            {
                RefreshFriendsList();
            }));
        }

        private void Friends_OnFriendNamesReceived(object sender, FriendNamesEventArgs e)
        {
            if (InvokeRequired)
            {
                if (IsHandleCreated)
                {
                    BeginInvoke(new MethodInvoker(() => Friends_OnFriendNamesReceived(sender, e)));
                }

                return;
            }

            BeginInvoke(new MethodInvoker(delegate()
            {
                try
                {
                    if (IsHandleCreated)
                    {
                        RefreshFriendsList();
                    }
                }
                catch { ; }
            }));
        }

        private void SetFriend(FriendInfo friend)
        {
            //if (InvokeRequired)
            //{
            //    BeginInvoke(new MethodInvoker(() => SetFriend(friend)));
            //    return;
            //}

            try
            {
                if (friend == null) return;

                if (cbofgroups.SelectedIndex > 0)
                {
                    button2.Visible = true;
                    button2.Enabled = true;
                }
                else
                {
                    button2.Visible = false;
                    button2.Enabled = false; 
                }

                selectedFriend = friend;

                lblFriendName.Text = friend.Name + (friend.IsOnline ? " (online)" : " (offline)");

                btnRemove.Enabled = btnIM.Enabled = btnProfile.Enabled = btnOfferTeleport.Enabled = btnPay.Enabled = true;
                chkSeeMeOnline.Enabled = chkSeeMeOnMap.Enabled = chkModifyMyObjects.Enabled = true;
                chkSeeMeOnMap.Enabled = friend.CanSeeMeOnline;

                settingFriend = true;
                chkSeeMeOnline.Checked = friend.CanSeeMeOnline;
                chkSeeMeOnMap.Checked = friend.CanSeeMeOnMap;
                chkModifyMyObjects.Checked = friend.CanModifyMyObjects;
                settingFriend = false;
            }
            catch { ; }

        }

        private void lbxFriends_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index < 0) return;

            FriendInfo itemToDraw = (FriendInfo)lbxFriends.Items[e.Index];

            Brush textBrush = null;
            Font textFont = null;
            
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                textBrush = new SolidBrush(Color.FromKnownColor(KnownColor.HighlightText));
                textFont = new Font(e.Font, FontStyle.Bold);
            }
            else
            {
                textBrush = new SolidBrush(Color.FromKnownColor(KnownColor.ControlText));
                textFont = new Font(e.Font, FontStyle.Regular);
            }

            SizeF stringSize = e.Graphics.MeasureString(itemToDraw.Name, textFont);
            float stringX = e.Bounds.Left + 4 + Properties.Resources.green_orb.Width;
            float stringY = e.Bounds.Top + 2 + ((Properties.Resources.green_orb.Height / 2) - (stringSize.Height / 2));

            if (itemToDraw.IsOnline)
            {
                e.Graphics.DrawImage(Properties.Resources.green_orb, e.Bounds.Left + 2, e.Bounds.Top + 2);
            }
            else
            {
                e.Graphics.DrawImage(Properties.Resources.green_orb_off, e.Bounds.Left + 2, e.Bounds.Top + 2);
            }
            
            e.Graphics.DrawString(" " + itemToDraw.Name, textFont, textBrush, stringX, stringY);

            e.DrawFocusRectangle();

            textFont.Dispose();
            textBrush.Dispose();
            textFont = null;
            textBrush = null;
        }

        private void lbxFriends_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxFriends.SelectedItem == null) return;

            FriendInfo item = (FriendInfo)lbxFriends.SelectedItem;
            SetFriend(item);
        }

        private void btnIM_Click(object sender, EventArgs e)
        {
            string agentname = selectedFriend.Name;

            if (instance.TabConsole.TabExists(agentname))
            {
                instance.TabConsole.SelectTab(agentname);
                return;
            }

            instance.TabConsole.AddIMTab(selectedFriend.UUID, client.Self.AgentID ^ selectedFriend.UUID, agentname);
            instance.TabConsole.SelectTab(agentname);
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            (new frmProfile(instance, selectedFriend.Name, selectedFriend.UUID)).Show();
        }

        private void chkSeeMeOnline_CheckedChanged(object sender, EventArgs e)
        {
            if (settingFriend) return;

            SetRights();
        }

        private void chkSeeMeOnMap_CheckedChanged(object sender, EventArgs e)
        {
            if (settingFriend) return;

            SetRights();
        }

        private void chkModifyMyObjects_CheckedChanged(object sender, EventArgs e)
        {
            if (settingFriend) return;

            SetRights();
        }

        private void SetRights()
        {
            FriendRights rgts = FriendRights.None;

            if (chkSeeMeOnline.Checked)
            {
                rgts |= FriendRights.CanSeeOnline;
            }

            if (chkSeeMeOnMap.Checked)
            {
                rgts |= FriendRights.CanSeeOnMap;
            }
            if (chkModifyMyObjects.Checked)
            {
                rgts |= FriendRights.CanModifyObjects;
            }

            client.Friends.GrantRights(selectedFriend.UUID, rgts);
        }

        private void btnOfferTeleport_Click(object sender, EventArgs e)
        {
            client.Self.SendTeleportLure(selectedFriend.UUID, "Join me in " + client.Network.CurrentSim.Name + "!");
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            (new frmPay(instance, selectedFriend.UUID, selectedFriend.Name)).Show(this);
        }

        private void FriendsConsole_Load(object sender, EventArgs e)
        {
            InitializeFriendsList();

            //lbGroups.Items.Add("All");
            //lbGroups.SelectedIndex = 0;
            cbofgroups.Items.Add("...All friends");
            cbofgroups.SelectedIndex = 0;

            string fconffile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt\\" + client.Self.AgentID.ToString() + "_fr_groups.ini";

            if (!System.IO.File.Exists(fconffile))
            {
                System.IO.StreamWriter SW;

                SW = System.IO.File.CreateText(fconffile);
                SW.Dispose();
            }

            //lbxFriends.PreSelect  += new EventHandler(lbxFriends_PreSelect); 


            fconfig = new FileConfig(fconffile);
            fconfig.Load();

            LoadFriendGroups();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure you want to terminate\nyour friendship with " + selectedFriend.Name + "?", "METAbolt", MessageBoxButtons.YesNo);

            if (res == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            client.Friends.TerminateFriendship(selectedFriend.UUID);
        }

        private void lbxFriends_DoubleClick(object sender, EventArgs e)
        {
            //btnIM.PerformClick();
        }

        private void lbGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblGroupName.Text = lbGroups.SelectedItem.ToString();

            if (lbGroups.SelectedIndex != -1)
            {
                textBox2.Visible = true;
            }
            else
            {
                textBox2.Visible = false;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                fconfig.CreateGroup(txtGroup.Text.Trim());

                txtGroup.Text = string.Empty;
                //fgrps = fconfig.FriendGroups;

                LoadFriendGroups();
            }
            catch
            {
                //string exp = ex.Message; 
            }
        }

        private void LoadFriendGroups()
        {
            fgrps = fconfig.FriendGroups;

            cbofgroups.Items.Clear();
            lbGroups.Items.Clear();

            cbofgroups.Items.Add("...All friends"); 

            foreach (KeyValuePair<string, Dictionary<string, string>> fr in fgrps)
            {
                string header = fr.Key.ToString();
                //Dictionary<string, string> rec = fr.Value;

                cbofgroups.Items.Add(header);
                lbGroups.Items.Add(header);   
            }

            cbofgroups.Sorted = true;
            lbGroups.Sorted = true;
        }

        private void cbofgroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fconfig == null) return;

            button2.Enabled = false;

            if (cbofgroups.SelectedItem.ToString() == "...All friends")
            {
                RefreshFriendsList();
            }
            else
            {
                GetGroupFriends(cbofgroups.SelectedItem.ToString());
            }
        }

        private void GetGroupFriends(string group)
        {
            lblFriendName.Text = string.Empty;

            btnRemove.Enabled = btnIM.Enabled = btnProfile.Enabled = btnOfferTeleport.Enabled = btnPay.Enabled = false;
            chkSeeMeOnline.Enabled = chkSeeMeOnMap.Enabled = chkModifyMyObjects.Enabled = false;

            fgrps = fconfig.FriendGroups;

            Dictionary<string, string> grps;

            fgrps.TryGetValue(group, out grps);

            lbxFriends.BeginUpdate();
            lbxFriends.Items.Clear();

            foreach (KeyValuePair<string, string> s in grps)
            {
                List<FriendInfo> flist = this.instance.State.AvatarFriends;

                if (flist.Count > 0)
                {
                    foreach (FriendInfo friend in flist)
                    {
                        if (friend.Name.ToLower(CultureInfo.CurrentCulture) == s.Value.ToLower(CultureInfo.CurrentCulture))
                        {
                            lbxFriends.Items.Add(friend);
                        }
                    }
                }
            }

            lbxFriends.EndUpdate();
        }

        private void lbxFriends_MouseDown(object sender, MouseEventArgs e)
        {
            lbxFriends_SelectedIndexChanged(null, null);

            Point pt = new Point(e.X, e.Y);
            int index = lbxFriends.IndexFromPoint(pt);

            // Starts a drag-and-drop operation.
            if (index >= 0 && index < lbxFriends.Items.Count)
            {
                FriendInfo dltm = (FriendInfo)lbxFriends.Items[index];

                lbxFriends.DoDragDrop(dltm, DragDropEffects.Copy);
            }
        }

        private void textBox2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(FriendInfo)))
                e.Effect = DragDropEffects.Copy;
        }

        private void textBox2_DragDrop(object sender, DragEventArgs e)
        {
            if (lbGroups.SelectedIndex == -1)
            {
                MessageBox.Show("You must select a group from the list first.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            FriendInfo node = e.Data.GetData(typeof(FriendInfo)) as FriendInfo;

            if (node == null) return;

            if (e.Data.GetDataPresent(typeof(FriendInfo)))
            {
                fconfig.AddFriendToGroup(lbGroups.SelectedItem.ToString(), node.Name, node.UUID.ToString());
                MessageBox.Show(node.Name + " has been added to your '" + lbGroups.SelectedItem.ToString() + "' group.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cbofgroups_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fconfig.removeFriendFromGroup(lbGroups.SelectedItem.ToString(), selectedFriend.UUID.ToString());
        }       
    }
}
