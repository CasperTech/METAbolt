//  Copyright (c) 2008 - 2011, www.metabolt.net (METAbolt)
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

namespace METAbolt
{
    public partial class FriendsConsole : UserControl
    {
        private METAboltInstance instance;
        private GridClient client;
        private FriendInfo selectedFriend;
        //private SLNetCom netcom;

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
                    bool dnavailable = client.Avatars.DisplayNamesAvailable();

                    if (dnavailable)
                    {
                        List<UUID> avIDs = new List<UUID>();
                        avIDs.Add(friend.UUID);
                        //client.Avatars.GetDisplayNames(avIDs, DisplayNameReceived);
                    }

                    lbxFriends.Items.Add(new FriendsListItem(friend));
                }

                //lbxFriends.Sort();
                lbxFriends.EndUpdate();
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
        }

        private void RefreshFriendsList()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => RefreshFriendsList()));
                return;
            }

            InitializeFriendsList();
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
                RefreshFriendsList();
            }));
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
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => SetFriend(friend)));
                return;
            }

            if (friend == null) return;
            selectedFriend = friend;

            lblFriendName.Text = friend.Name + (friend.IsOnline ? " (online)" : " (offline)");

            btnRemove.Enabled = btnIM.Enabled = btnProfile.Enabled = btnOfferTeleport.Enabled = btnPay.Enabled = true;

            if (!friend.IsOnline)
            {
                btnOfferTeleport.Enabled = false;
            }

            chkSeeMeOnline.Enabled = chkSeeMeOnMap.Enabled = chkModifyMyObjects.Enabled = true;
            chkSeeMeOnMap.Enabled = friend.CanSeeMeOnline;

            settingFriend = true;
            chkSeeMeOnline.Checked = friend.CanSeeMeOnline;
            chkSeeMeOnMap.Checked = friend.CanSeeMeOnMap;
            chkModifyMyObjects.Checked = friend.CanModifyMyObjects;
            settingFriend = false;
        }

        private void lbxFriends_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index < 0) return;

            FriendsListItem itemToDraw = (FriendsListItem)lbxFriends.Items[e.Index];
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

            SizeF stringSize = e.Graphics.MeasureString(itemToDraw.Friend.Name, textFont);
            float stringX = e.Bounds.Left + 4 + Properties.Resources.GreenOrb_16.Width;
            float stringY = e.Bounds.Top + 2 + ((Properties.Resources.GreenOrb_16.Height / 2) - (stringSize.Height / 2));

            //if (itemToDraw.Friend.IsOnline)
            //{
            //    e.Graphics.DrawImage(Properties.Resources.GreenOrb_16, e.Bounds.Left + 2, e.Bounds.Top + 2);
            //}
            //else
            //{
            //    e.Graphics.DrawImage(Properties.Resources.GreenOrbFaded_16, e.Bounds.Left + 2, e.Bounds.Top + 2);
            //}

            if (itemToDraw.Friend.IsOnline)
            {
                e.Graphics.DrawImage(Properties.Resources.green_orb, e.Bounds.Left + 2, e.Bounds.Top + 2);
            }
            else
            {
                e.Graphics.DrawImage(Properties.Resources.green_orb_off, e.Bounds.Left + 2, e.Bounds.Top + 2);
            }
                        
            e.Graphics.DrawString(itemToDraw.Friend.Name, textFont, textBrush, stringX, stringY);

            e.DrawFocusRectangle();

            textFont.Dispose();
            textBrush.Dispose();
            textFont = null;
            textBrush = null;
        }

        private void lbxFriends_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxFriends.SelectedItem == null) return;

            FriendsListItem item = (FriendsListItem)lbxFriends.SelectedItem;
            SetFriend(item.Friend);
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
            (new frmPay(instance, selectedFriend.UUID, selectedFriend.Name)).ShowDialog();
        }

        private void FriendsConsole_Load(object sender, EventArgs e)
        {
            InitializeFriendsList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client.Friends.TerminateFriendship(selectedFriend.UUID);
        }

        private void lbxFriends_DoubleClick(object sender, EventArgs e)
        {
            btnIM.PerformClick();
        }
    }
}
