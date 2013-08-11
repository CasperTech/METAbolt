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
using SLNetworkComm;
using OpenMetaverse;
using System.Media;
using ExceptionReporting;
using System.Threading;
using System.Globalization;

namespace METAbolt
{
    public partial class TabsConsole : UserControl
    {
        private METAboltInstance instance;
        private SLNetCom netcom;
        private GridClient client;
        public ChatConsole chatConsole;
        public SafeDictionary<string, METAboltTab> tabs = new SafeDictionary<string, METAboltTab>();
        private string TabAgentName = "";
        //private bool isgroup = false;
        private bool stopnotify = false;
        private string avname = string.Empty;
        private METAboltTab selectedTab;
        private Form owner;
        private int tmoneybalance;
        private bool floading = true;
        private ExceptionReporter reporter = new ExceptionReporter();

        internal class ThreadExceptionHandler
        {
            public void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
            {
                ExceptionReporter reporter = new ExceptionReporter();
                reporter.Show(e.Exception);
            }
        }

        public TabsConsole(METAboltInstance instance)
        {
            InitializeComponent();

            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.instance = instance;
            netcom = this.instance.Netcom;
            client = this.instance.Client;
            AddNetcomEvents();

            InitializeMainTab();
            InitializeChatTab();

            ApplyConfig(this.instance.Config.CurrentConfig);
            this.instance.Config.ConfigApplied += new EventHandler<ConfigAppliedEventArgs>(Config_ConfigApplied);
        }

        //private ToolStripItem GetSelectedItem()
        //{
        //    ToolStripItem item = null;
        //    for (int i = 0; i < tabs.Count; i++)
        //    {
        //        if (tabs[i].Selected)
        //        {
        //            item = this.DisplayedItems[i];
        //        }
        //    }
        //    return item;
        //}

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

        private void Config_ConfigApplied(object sender, ConfigAppliedEventArgs e)
        {
            ApplyConfig(e.AppliedConfig);
        }

        private void ApplyConfig(Config config)
        {
            if (config.InterfaceStyle == 0) //System
                tstTabs.RenderMode = ToolStripRenderMode.System;
            else if (config.InterfaceStyle == 1) //Office 2003
                tstTabs.RenderMode = ToolStripRenderMode.ManagerRenderMode;

            stopnotify = config.DisableNotifications;

            if (config.DisableTrayIcon)
            {
                if (stopnotify)
                {
                    notifyIcon1.Visible = false;
                    config.HideMeta = false;
                }
                else
                {
                    if (!config.HideMeta)
                    {
                        notifyIcon1.Visible = false;
                    }
                    else
                    {
                        notifyIcon1.Visible = true;
                    }
                }
            }
            else
            {
                notifyIcon1.Visible = true;
            }

            // Menu positions

            Control control;

            //bool topofscreen = false;

            switch (instance.Config.CurrentConfig.FnMenuPos)
            {
                case "Top":
                    control = toolStripContainer1.TopToolStripPanel;
                    //topofscreen = true;
                    break;

                case "Bottom":
                    control = toolStripContainer1.BottomToolStripPanel;
                    break;

                case "Left":
                    control = toolStripContainer1.LeftToolStripPanel;
                    break;

                case "Right":
                    control = toolStripContainer1.RightToolStripPanel;
                    break;

                default:
                    control = toolStripContainer1.TopToolStripPanel;
                    break;
            }

            tstTabs.Parent = control;
            //topofscreen = false;
        }

        private void AddNetcomEvents()
        {
            netcom.ClientLoginStatus += new EventHandler<LoginProgressEventArgs>(netcom_ClientLoginStatus);
            netcom.ClientLoggedOut += new EventHandler(netcom_ClientLoggedOut);
            netcom.ClientDisconnected += new EventHandler<DisconnectedEventArgs>(netcom_ClientDisconnected);
            netcom.ChatReceived += new EventHandler<ChatEventArgs>(netcom_ChatReceived);
            netcom.ChatSent += new EventHandler<SLNetworkComm.ChatSentEventArgs>(netcom_ChatSent);
            netcom.AlertMessageReceived += new EventHandler<AlertMessageEventArgs>(netcom_AlertMessageReceived);
            netcom.InstantMessageReceived += new EventHandler<InstantMessageEventArgs>(netcom_InstantMessageReceived);
            client.Groups.CurrentGroups += new EventHandler<CurrentGroupsEventArgs>(Groups_OnCurrentGroups);
            client.Self.MoneyBalanceReply +=new EventHandler<MoneyBalanceReplyEventArgs>(Self_MoneyBalanceReply);
            client.Friends.FriendOffline += new EventHandler<FriendInfoEventArgs>(Friends_OnFriendOffline);
            client.Friends.FriendOnline += new EventHandler<FriendInfoEventArgs>(Friends_OnFriendOnline);
        }

        private void RemoveNetcomEvents()
        {
            netcom.ClientLoginStatus -= new EventHandler<LoginProgressEventArgs>(netcom_ClientLoginStatus);
            netcom.ClientLoggedOut -= new EventHandler(netcom_ClientLoggedOut);
            netcom.ClientDisconnected -= new EventHandler<DisconnectedEventArgs>(netcom_ClientDisconnected);
            netcom.ChatReceived -= new EventHandler<ChatEventArgs>(netcom_ChatReceived);
            netcom.ChatSent -= new EventHandler<SLNetworkComm.ChatSentEventArgs>(netcom_ChatSent);
            netcom.AlertMessageReceived -= new EventHandler<AlertMessageEventArgs>(netcom_AlertMessageReceived);
            netcom.InstantMessageReceived -= new EventHandler<InstantMessageEventArgs>(netcom_InstantMessageReceived);
            client.Groups.CurrentGroups -= new EventHandler<CurrentGroupsEventArgs>(Groups_OnCurrentGroups);
            this.instance.Config.ConfigApplied -= new EventHandler<ConfigAppliedEventArgs>(Config_ConfigApplied);
            client.Friends.FriendOffline -= new EventHandler<FriendInfoEventArgs>(Friends_OnFriendOffline);
            client.Friends.FriendOnline -= new EventHandler<FriendInfoEventArgs>(Friends_OnFriendOnline);
        }

        private void Groups_OnCurrentGroups(object sender, CurrentGroupsEventArgs e)
        {
            try
            {
                this.instance.State.Groups = e.Groups;
                BeginInvoke(new MethodInvoker(GetGroupsName));
            }
            catch (Exception ex)
            {
                //string exp = ex.Message;
                reporter.Show(ex);
            }
        }

        private void Self_MoneyBalanceReply(object sender, MoneyBalanceReplyEventArgs e)
        {
            if (floading)
            {
                tmoneybalance = e.Balance;
                floading = false;
                return;
            }

            TransactionInfo ti = e.TransactionInfo;

            if (ti.DestID != UUID.Zero && ti.SourceID != UUID.Zero)
            {
                if (instance.Config.CurrentConfig.PlayPaymentReceived)
                {
                    SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.MoneyBeep);
                    simpleSound.Play();
                    simpleSound.Dispose();
                }
            }

            //tabs["chat"].Highlight();

            int bal = e.Balance - tmoneybalance;

            string ttl = "METAbolt Alert";
            string body = string.Empty;

            if (bal > 0)
            {
                if (e.Success)
                {
                    if (ti.DestID != client.Self.AgentID)
                    {
                        body = e.Description;
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(e.Description))
                        {
                            string pfrm = string.Empty;

                            if (ti.TransactionType == 5008)
                            {
                                pfrm = " via " + ti.ItemDescription;
                            }

                            body = e.Description + pfrm;

                            body = body.Replace(".", string.Empty);   
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(ti.ItemDescription))
                            {
                                body = "You have received a payment of L$" + ti.Amount.ToString(CultureInfo.CurrentCulture) + " from " + ti.ItemDescription;
                            }
                            else
                            {
                                body = "You have received a payment of L$" + ti.Amount.ToString(CultureInfo.CurrentCulture);
                            }
                        }
                    }
                }                

                TrayNotifiy(ttl, body, false);
            }
            else
            {
                ////body = e.Description;
                //if (ti.DestID != client.Self.AgentID)
                //{

                //    if (!String.IsNullOrEmpty(ti.ItemDescription))
                //    {
                //        body = "You paid L$" + ti.Amount.ToString() + " to/for " + ti.ItemDescription;
                //    }
                //    else
                //    {
                //        body = "You paid L$" + ti.Amount.ToString();
                //    }
                //}
                //else
                //{
                //    body = e.Description;
                //}

                if (!String.IsNullOrEmpty(e.Description))
                {
                    body = e.Description;

                    TrayNotifiy(ttl, body, false);
                }
            }

            tmoneybalance = e.Balance;
        }

        //Separate thread
        private void Friends_OnFriendOffline(object sender, FriendInfoEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => Friends_OnFriendOffline(sender, e)));
                return;
            }

            if (e.Friend.Name != null)
            {
                if (!instance.Config.CurrentConfig.DisableFriendsNotifications)
                {
                    if (instance.Config.CurrentConfig.PlayFriendOffline)
                    {
                        SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.Friend_Off);
                        simpleSound.Play();
                        simpleSound.Dispose();
                    }

                    string ttl = "METAbolt Alert";
                    string body = e.Friend.Name + " is offline";
                    TrayNotifiy(ttl, body, false);
                }
            }
        }

        //Separate thread
        private void Friends_OnFriendOnline(object sender, FriendInfoEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => Friends_OnFriendOnline(sender, e)));
                return;
            }

            if (!string.IsNullOrEmpty(e.Friend.Name) && !string.IsNullOrEmpty(avname))
            {
                if (!instance.Config.CurrentConfig.DisableFriendsNotifications)
                {
                    if (instance.Config.CurrentConfig.PlayFriendOnline)
                    {
                        SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.Friend_On);
                        simpleSound.Play();
                        simpleSound.Dispose();
                    }

                    string ttl = "METAbolt Alert";
                    string body = e.Friend.Name + " is online";
                    TrayNotifiy(ttl, body, false);
                }
            }
        }

        private void GetGroupsName()
        {
            this.instance.State.GroupStore.Clear();

            foreach (Group group in this.instance.State.Groups.Values)
            {
                this.instance.State.GroupStore.Add(group.ID, group.Name);
            }
        }

        private void netcom_ClientLoginStatus(object sender, LoginProgressEventArgs e)
        {
            if (e.Status != LoginStatus.Success) return;

            try
            {
                if (e.Status == LoginStatus.Success)
                {
                    InitializeFriendsTab();
                    InitializeGroupsTab();
                    InitializeInventoryTab();
                    InitializeSearchTab();
                    //InitializeMapTab();
                    InitializeIMboxTab();

                    avname = netcom.LoginOptions.FullName;
                    notifyIcon1.Text = "METAbolt [" + avname + "]";

                    if (selectedTab.Name == "main")
                        tabs["chat"].Select();

                    //client.Groups.RequestCurrentGroups();
                    client.Self.RetrieveInstantMessages();
                }
            }
            catch (Exception ex)
            {
                Logger.Log("login (tabs console): " + ex.Message, Helpers.LogLevel.Error);
            }
        }

        private void netcom_ClientLoggedOut(object sender, EventArgs e)
        {
            TidyUp();

            TrayNotifiy("METAbolt - " + avname, "Logged out");
        }

        private void netcom_ClientDisconnected(object sender, DisconnectedEventArgs e)
        {
            if (e.Reason == NetworkManager.DisconnectType.ClientInitiated) return;

            TidyUp();

            notifyIcon1.Text = "METAbolt - " + avname + " [Disconnected]";
            TrayNotifiy("METAbolt - " + avname, "Disconnected");
        }

        private void TidyUp()
        {
            DisposeSearchTab();
            DisposeGroupsTab();
            DisposeInventoryTab();
            DisposeFriendsTab();
            DisposeIMboxTab();

            RemoveNetcomEvents();

            tabs["main"].Select();
        }

        private void netcom_AlertMessageReceived(object sender, AlertMessageEventArgs e)
        {
            tabs["chat"].Highlight();

            string ttl = "METAbolt Alert";
            string body = e.Message.ToString();
            TrayNotifiy(ttl, body);
        }

        private void netcom_ChatSent(object sender, ChatSentEventArgs e)
        {
            tabs["chat"].Highlight();
        }

        private void netcom_ChatReceived(object sender, ChatEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Message)) return;

            // Avoid form flash if RLV command
            if (e.SourceType == ChatSourceType.Object)
            {
                if (e.Message.StartsWith("@", StringComparison.CurrentCultureIgnoreCase)) return;
            }

            tabs["chat"].Highlight();
        }

        public void DisplayOnChat(InstantMessageEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    DisplayOnChat(e);
                }));

                return;
            }

            if (instance.IsAvatarMuted(e.IM.FromAgentID, e.IM.FromAgentName)) return;
            if (e.IM.Message.Contains(this.instance.Config.CurrentConfig.CommandInID)) return;
            if (e.IM.Message.Contains(this.instance.Config.CurrentConfig.IgnoreUID)) return;

            BeginInvoke(new MethodInvoker(delegate()
            {
                ChatBufferItem ready = new ChatBufferItem(DateTime.Now,
                           e.IM.FromAgentName + " (" + e.IM.FromAgentID.ToString() + "): " + e.IM.Message,
                           ChatBufferTextStyle.ObjectChat,
                           null,
                           e.IM.IMSessionID); //added by GM on 3-JUL-2009 - the FromAgentID

                chatConsole.ChatManager.ProcessBufferItem(ready, false);
            }));
        }

        public void DisplayChatScreen(string msg)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    DisplayChatScreen(msg);
                }));

                return;
            }

            BeginInvoke(new MethodInvoker(delegate()
            {
                ChatBufferItem ready = new ChatBufferItem(DateTime.Now,
                           msg,
                           ChatBufferTextStyle.Alert,
                           null,
                           UUID.Random()); //added by GM on 3-JUL-2009 - the FromAgentID

                chatConsole.ChatManager.ProcessBufferItem(ready, false);
            }));
        }

        private void netcom_InstantMessageReceived(object sender, InstantMessageEventArgs e)
        {
            //if (instance.IsAvatarMuted(e.IM.FromAgentID)) return;

            switch (e.IM.Dialog)
            {
                case InstantMessageDialog.MessageFromAgent:
                    //if (e.IM.FromAgentID != client.Self.AgentID)
                    //{
                    if (e.IM.FromAgentName.ToLower(CultureInfo.CurrentCulture) == "second life")
                    {
                        DisplayOnChat(e);
                        return;
                    }
                    else if (e.IM.FromAgentID == UUID.Zero)
                    {
                        // Marketplace Received item notification
                        //MessageBox.Show(e.IM.Message, "METAbolt");
                        (new frmMBmsg(instance, e.IM.Message)).ShowDialog(this);
                    }
                    else if (e.IM.IMSessionID == UUID.Zero)
                    {
                        if (e.IM.RegionID != UUID.Zero)
                        {
                            // Region message
                            String msg = "Region message from " + e.IM.FromAgentName + Environment.NewLine + Environment.NewLine;
                            msg += @e.IM.Message;

                            //MessageBox.Show(msg, "METAbolt");

                            (new frmMBmsg(instance, msg)).ShowDialog(this);
                        }
                        else
                        {
                            HandleIM(e);
                        }
                    }
                    else
                    {
                        HandleIM(e);
                    }
                    //}
                    
                    break;
                case InstantMessageDialog.SessionSend:
                //case InstantMessageDialog.SessionGroupStart:
                    HandleIM(e);
                    break;
                case InstantMessageDialog.MessageFromObject:
                    if (instance.IsObjectMuted(e.IM.FromAgentID, e.IM.FromAgentName)) return;
                    if (instance.State.IsBusy) return;
                    DisplayOnChat(e);
                    
                    break;

                case InstantMessageDialog.StartTyping:
                    if (TabExists(e.IM.FromAgentName))
                    {
                        // this is making the window flash and people don't like it
                        // so I am taking it out. LL
                        //METAboltTab tab = tabs[e.IM.FromAgentName.ToLower()];
                        //if (!tab.Highlighted) tab.PartialHighlight();
                    }

                    break;

                case InstantMessageDialog.StopTyping:
                    if (TabExists(e.IM.FromAgentName))
                    {
                        // this is making the window flash and people don't like it
                        // so I am taking it out. LL
                        //METAboltTab tab = tabs[e.IM.FromAgentName.ToLower()];
                        //if (!tab.Highlighted) tab.Unhighlight();
                    }

                    break;

                case InstantMessageDialog.RequestTeleport:
                    if (instance.IsAvatarMuted(e.IM.FromAgentID, e.IM.FromAgentName)) return;
                    HandleTP(e);
                    break;

                case InstantMessageDialog.FriendshipOffered:
                    if (instance.IsAvatarMuted(e.IM.FromAgentID, e.IM.FromAgentName)) return;
                    HandleFriendship(e);
                    break;

                case InstantMessageDialog.ConsoleAndChatHistory:
                    //HandleHistory(e);
                    break;

                case InstantMessageDialog.TaskInventoryOffered:
                case InstantMessageDialog.InventoryOffered:
                    //if (instance.IsAvatarMuted(e.IM.FromAgentID)) return;
                    HandleInventory(e);
                    break;

                case InstantMessageDialog.InventoryAccepted:
                    HandleInventoryReplyAccepted(e);
                    break;

                case InstantMessageDialog.InventoryDeclined:
                    HandleInventoryReplyDeclined(e);
                    break;

                case InstantMessageDialog.GroupInvitation:
                    if (instance.IsAvatarMuted(e.IM.FromAgentID, e.IM.FromAgentName)) return;
                    HandleGroupInvite(e);
                    break;

                case InstantMessageDialog.FriendshipAccepted:
                    HandleFriendshipAccepted(e);
                    break;

                case InstantMessageDialog.FriendshipDeclined:
                    HandleFriendshipDeclined(e);
                    break;

                case InstantMessageDialog.GroupNotice:
                    if (instance.IsAvatarMuted(e.IM.FromAgentID, e.IM.FromAgentName)) return;
                    HandleGroupNoticeReceived(e);
                    break;

                case InstantMessageDialog.GroupInvitationAccept:
                    HandleGroupInvitationAccept(e);
                    break;

                case InstantMessageDialog.GroupInvitationDecline:
                    HandleGroupInvitationDecline(e);
                    break;

                case InstantMessageDialog.MessageBox:
                    if (instance.IsObjectMuted(e.IM.FromAgentID, e.IM.FromAgentName)) return;
                    HandleMessageBox(e);
                    break;
            }
        }

        private void TrayNotifiy(string title, string msg)
        {
            if (instance.State.IsBusy) return;

            if (System.Text.RegularExpressions.Regex.IsMatch(msg.ToLower(CultureInfo.CurrentCulture).Trim(), "autopilot", System.Text.RegularExpressions.RegexOptions.IgnoreCase)) return;

            notifyIcon1.Text = UpdateIconTitle();

            try
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    //chatConsole.ChatManager.PrintMsg("\n" + msg + "\n");
                    chatConsole.ChatManager.PrintMsg(Environment.NewLine + getTimeStamp() + msg);
                }));
            }
            catch { ; }

            if (!stopnotify)
            {
                notifyIcon1.BalloonTipText = msg;
                notifyIcon1.BalloonTipTitle = title + " [" + avname + "]";
                notifyIcon1.ShowBalloonTip(2000);

                if (this.instance.Config.CurrentConfig.PlaySound)
                {
                    //System.Media.SystemSounds..Play();
                    SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.notify);
                    simpleSound.Play();
                    simpleSound.Dispose();
                }
            }
        }

        private void TrayNotifiy(string title, string msg, bool makesound)
        {
            if (instance.State.IsBusy) return;

            if (System.Text.RegularExpressions.Regex.IsMatch(msg.ToLower(CultureInfo.CurrentCulture).Trim(), "autopilot", System.Text.RegularExpressions.RegexOptions.IgnoreCase)) return;

            notifyIcon1.Text = UpdateIconTitle();

            BeginInvoke(new MethodInvoker(delegate()
            {
                ////chatConsole.ChatManager.PrintMsg("\n" + msg + "\n");
                ////chatConsole.ChatManager.PrintMsg(Environment.NewLine + getTimeStamp() + msg);
                //chatConsole.ChatManager.PrintMsg(Environment.NewLine + msg);
                chatConsole.ChatManager.PrintMsg(msg);
            }));

            if (!stopnotify)
            {
                notifyIcon1.BalloonTipText = msg;
                notifyIcon1.BalloonTipTitle = title + " [" + avname + "]";
                notifyIcon1.ShowBalloonTip(2000);

                if (this.instance.Config.CurrentConfig.PlaySound && makesound)
                {
                    //System.Media.SystemSounds..Play();
                    SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.notify);
                    simpleSound.Play();
                    simpleSound.Dispose();
                }
            }
        }

        private string getTimeStamp()
        {
            if (this.instance.Config.CurrentConfig.ChatTimestamps)
            {
                DateTime dte = DateTime.Now;
                dte = this.instance.State.GetTimeStamp(dte);

                return dte.ToString("[HH:mm] ", CultureInfo.CurrentCulture);
            }

            return string.Empty;  
        }

        private string UpdateIconTitle()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("METAbolt - ");

            if (netcom.IsLoggedIn)
            {
                sb.Append("[" + avname + "]");
            }
            else
            {
                sb.Append(avname + " [Logged out]");
            }

            string title =  sb.ToString();
            sb = null;

            return title;
        }

        private void HandleFriendshipAccepted(InstantMessageEventArgs e)
        {
            if (instance.State.IsBusy) return;

            string fromAgent = e.IM.FromAgentName;

            string ttl = "Friendship offered";
            string body = fromAgent + " has accepted your friendship offer";
            TrayNotifiy(ttl, body); 
        }

        private void HandleFriendshipDeclined(InstantMessageEventArgs e)
        {
            if (instance.State.IsBusy) return;

            string fromAgent = e.IM.FromAgentName;

            string ttl = "Friendship offered";
            string body = fromAgent + " has declined your friendship offer";
            TrayNotifiy(ttl, body);
        }

        private void HandleGroupNoticeReceived(InstantMessageEventArgs e)
        {
            //if (instance.IsAvatarMuted(e.IM.FromAgentID, e.IM.FromAgentName)) return;

            if (instance.Config.CurrentConfig.DisableGroupNotices)
            {
                return;
            }

            if (instance.State.IsBusy) return;

            // Count the ones already on display
            // to avoid flooding

            if (this.instance.NoticeCount < 9)
            {
                this.instance.NoticeCount += 1;
            }

            if (this.instance.NoticeCount < 9)
            {
                (new frmGroupNotice(instance, e)).Show(this);
            }
        }

        private void HandleGroupInvitationAccept(InstantMessageEventArgs e)
        {
            if (instance.State.IsBusy) return;

            string fromAgent = e.IM.FromAgentName;

            string ttl = "Group invitation";
            string body = fromAgent + " has accepted your group invitation";
            TrayNotifiy(ttl, body);
        }

        private void HandleGroupInvitationDecline(InstantMessageEventArgs e)
        {
            if (instance.State.IsBusy) return;

            string fromAgent = e.IM.FromAgentName;

            string ttl = "Group invitation";
            string body = fromAgent + " has declined your group invitation";
            TrayNotifiy(ttl, body);
        }

        private void HandleMessageBox(InstantMessageEventArgs e)
        {
            if (instance.IsAvatarMuted(e.IM.FromAgentID, e.IM.FromAgentName)) return;

            if (instance.State.IsBusy) return;

            //string ttl = "METAbolt";
            string body = @e.IM.Message;
            //TrayNotifiy(ttl, body);

            (new frmMBmsg(instance, body)).ShowDialog(this);
        }

        private void HandleIM(InstantMessageEventArgs e)
        {
            if (instance.IsAvatarMuted(e.IM.FromAgentID, e.IM.FromAgentName)) return;
            if (e.IM.Message.Contains(this.instance.Config.CurrentConfig.CommandInID)) return;
            if (e.IM.Message.Contains(this.instance.Config.CurrentConfig.IgnoreUID)) return;

            if (instance.IsGiveItem(e.IM.Message.ToLower(CultureInfo.CurrentCulture), e.IM.FromAgentID))
            {
                return;
            }

            if (e.IM.Dialog == InstantMessageDialog.SessionSend)
            {
                if (this.instance.State.GroupStore.ContainsKey(e.IM.IMSessionID))
                {
                    //if (null != client.Self.MuteList.Find(me => me.Type == MuteType.Group && (me.ID == e.IM.IMSessionID || me.ID == e.IM.FromAgentID))) return;

                    // Check to see if group IMs are disabled
                    if (instance.Config.CurrentConfig.DisableGroupIMs)
                        return;

                    if (instance.State.IsBusy) return;

                    if (TabExists(this.instance.State.GroupStore[e.IM.IMSessionID]))
                    {
                        METAboltTab tab = tabs[this.instance.State.GroupStore[e.IM.IMSessionID].ToLower(CultureInfo.CurrentCulture)];
                        if (!tab.Selected)
                        {
                            tab.Highlight();
                            tabs["imbox"].PartialHighlight();
                        }

                        return;
                    }
                    else
                    {
                        IMTabWindowGroup imTab = AddIMTabGroup(e);
                        tabs[imTab.TargetName.ToLower()].Highlight();
                        tabs["imbox"].IMboxHighlight();
                        if (tabs[imTab.TargetName.ToLower(CultureInfo.CurrentCulture)].Selected) tabs[imTab.TargetName.ToLower(CultureInfo.CurrentCulture)].Highlight();

                        return;
                    }
                }

                return;
            }

            if (TabExists(e.IM.FromAgentName))   //if (tabs.ContainsKey(e.IM.FromAgentName.ToLower()))
            {
                if (!tabs[e.IM.FromAgentName.ToLower(CultureInfo.CurrentCulture)].Selected)
                {
                    tabs["imbox"].PartialHighlight();
                }
            }
            else
            {
                tabs["imbox"].IMboxHighlight();
            }

            if (this.instance.MainForm.WindowState == FormWindowState.Minimized)
            {
                if (!stopnotify)
                {
                    string ttl = string.Empty;

                    avname = netcom.LoginOptions.FullName;

                    if (this.instance.State.GroupStore.ContainsKey(e.IM.IMSessionID))
                    {
                        ttl = "Group IM notification [" + avname + "]";
                    }
                    else
                    {
                        ttl = "IM notification [" + avname + "]";
                    }

                    string imsg = e.IM.Message;

                    if (imsg.Length > 125)
                    {
                        imsg = imsg.Substring(0, 125) + "...";
                    }

                    string body = e.IM.FromAgentName + ": " + imsg;

                    Notification notifForm = new Notification();
                    notifForm.Message = body;
                    notifForm.Title = ttl;
                    notifForm.Show();
                }
            }

            if (this.instance.State.GroupStore.ContainsKey(e.IM.IMSessionID))
            {
                //if (null != client.Self.MuteList.Find(me => me.Type == MuteType.Group && (me.ID == e.IM.IMSessionID || me.ID == e.IM.FromAgentID))) return;

                // Check to see if group IMs are disabled
                if (instance.Config.CurrentConfig.DisableGroupIMs)
                {
                    Group grp = this.instance.State.Groups[e.IM.IMSessionID];
                    client.Self.RequestLeaveGroupChat(grp.ID);
                    return;
                }

                if (instance.State.IsBusy)
                {
                    Group grp = this.instance.State.Groups[e.IM.IMSessionID];
                    client.Self.RequestLeaveGroupChat(grp.ID);
                    return;
                }

                if (TabExists(this.instance.State.GroupStore[e.IM.IMSessionID]))
                {
                    METAboltTab tab = tabs[this.instance.State.GroupStore[e.IM.IMSessionID].ToLower(CultureInfo.CurrentCulture)];
                    if (!tab.Selected) tab.PartialHighlight();
                    //Logger.Log("Stored|ExistingGroupTab:: " + e.IM.Message, Helpers.LogLevel.Debug);
                    return;
                }
                else
                {
                    //create a new tab
                    IMTabWindowGroup imTab = AddIMTabGroup(e);
                    tabs[imTab.TargetName.ToLower(CultureInfo.CurrentCulture)].Highlight();

                    if (instance.Config.CurrentConfig.PlayGroupIMreceived)
                    {
                        SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.Group_Im_received);
                        simpleSound.Play();
                        simpleSound.Dispose();
                    }

                    //Logger.Log("Stored|NewGroupTab:: " + e.IM.Message, Helpers.LogLevel.Debug);
                    return;
                }
            }
            else
            {
                if (TabExists(e.IM.FromAgentName))
                {
                    METAboltTab tab = tabs[e.IM.FromAgentName.ToLower(CultureInfo.CurrentCulture)];
                    if (!tab.Selected) tab.PartialHighlight();
                    return;
                }
                else
                {
                    IMTabWindow imTab = AddIMTab(e);
                    tabs[imTab.TargetName.ToLower(CultureInfo.CurrentCulture)].Highlight();

                    if (instance.Config.CurrentConfig.InitialIMReply.Length > 0)
                    {
                        client.Self.InstantMessage(e.IM.FromAgentID, instance.Config.CurrentConfig.InitialIMReply);
                    }

                    if (instance.Config.CurrentConfig.PlayIMreceived)
                    {
                        SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.IM_received);
                        simpleSound.Play();
                        simpleSound.Dispose();
                    }
                }
            }
        }

        private void HandleHistory(InstantMessageEventArgs e)
        {
            //string msg =  e.IM.Message;
            //if (TabExists(e.IM.FromAgentName))
            //{
            //    METAboltTab tab = tabs[e.IM.FromAgentName.ToLower()];
            //    if (!tab.Selected) tab.Highlight();
            //    return;
            //}
            //else
            //{
            //    IMTabWindow imTab = AddIMTab(e);
            //    tabs[imTab.TargetName.ToLower()].Highlight();
            //}
        }
        private void HandleTP(InstantMessageEventArgs e)
        {
            if (instance.IsAvatarMuted(e.IM.FromAgentID, e.IM.FromAgentName)) return;

            if (instance.State.IsBusy)
            {
                string responsemsg = this.instance.Config.CurrentConfig.BusyReply;
                client.Self.InstantMessage(client.Self.Name, e.IM.FromAgentID, responsemsg, e.IM.IMSessionID, InstantMessageDialog.BusyAutoResponse, InstantMessageOnline.Offline, instance.SIMsittingPos(), UUID.Zero, new byte[0]); 
                return;
            }

            string fromAgentID = e.IM.FromAgentID.ToString();
            string fromAgent = e.IM.FromAgentName;

            if (TabExists(fromAgentID))
                tabs[fromAgentID].Close();

            TPTabWindow tpTab = AddTPTab(e);
            tabs[tpTab.TargetUUID.ToString()].Highlight();

            string ttl = "METAbolt";
            string body = "You have received a Teleport request from " + fromAgent;
            TrayNotifiy(ttl, body);
        }

        private void HandleFriendship(InstantMessageEventArgs e)
        {
            if (instance.IsAvatarMuted(e.IM.FromAgentID, e.IM.FromAgentName)) return;

            if (instance.State.IsBusy)
            {
                string responsemsg = this.instance.Config.CurrentConfig.BusyReply;
                client.Self.InstantMessage(client.Self.Name, e.IM.FromAgentID, responsemsg, e.IM.IMSessionID, InstantMessageDialog.BusyAutoResponse, InstantMessageOnline.Offline, instance.SIMsittingPos(), UUID.Zero, new byte[0]);
                return;
            }

            string fromAgentID = e.IM.FromAgentID.ToString();
            string fromAgent = e.IM.FromAgentName;

            if (TabExists(fromAgentID))
                tabs[fromAgentID].Close();

            if (instance.Config.CurrentConfig.AutoAcceptFriends)
            {
                client.Friends.AcceptFriendship(e.IM.FromAgentID, e.IM.IMSessionID);
                return;
            }

            FRTabWindow frTab = AddFRTab(e);
            tabs[frTab.TargetUUID.ToString()].Highlight();

            string ttl = "METAbolt";
            string body = "You have received a Friendship offer " + fromAgent;
            TrayNotifiy(ttl, body);
        }

        private void HandleInventory(InstantMessageEventArgs e)
        {
            //if (e.IM.Dialog == InstantMessageDialog.TaskInventoryOffered)
            //{
            //    if (instance.IsObjectMuted(e.IM.FromAgentID, e.IM.FromAgentName))
            //        return;
            //}
            //else
            //{
            //    if (instance.IsAvatarMuted(e.IM.FromAgentID, e.IM.FromAgentName))
            //        return;
            //}

            //if (instance.IsAvatarMuted(e.IM.FromAgentID, MuteType.Object))
            //    return;

            //if (instance.State.IsBusy)
            //{
            //    string responsemsg = this.instance.Config.CurrentConfig.BusyReply;
            //    client.Self.InstantMessage(client.Self.Name, e.IM.FromAgentID, responsemsg, e.IM.IMSessionID, InstantMessageDialog.BusyAutoResponse, InstantMessageOnline.Offline, instance.SIMsittingPos(), UUID.Zero, new byte[0]);
            //    return;
            //}

            if (instance.IsObjectMuted(e.IM.FromAgentID, e.IM.FromAgentName)) return;

            AssetType type = (AssetType)e.IM.BinaryBucket[0];

            if (type == AssetType.Unknown) return;

            UUID oID = UUID.Zero;

            if (e.IM.BinaryBucket.Length == 17)
            {
                oID = new UUID(e.IM.BinaryBucket, 1);
            }

            UUID invfolder = UUID.Zero;

            if (type == AssetType.Folder)
            {
                invfolder = client.Inventory.Store.RootFolder.UUID;
            }
            else
            {
                invfolder = client.Inventory.FindFolderForType(type);
            }

            if (!instance.Config.CurrentConfig.DeclineInv)
            {
                try
                {
                    if (e.IM.BinaryBucket.Length > 0)
                    {
                        if (instance.Config.CurrentConfig.AutoAcceptItems)
                        {
                            if (e.IM.Dialog == InstantMessageDialog.InventoryOffered)
                            {
                                client.Self.InstantMessage(client.Self.Name, e.IM.FromAgentID, string.Empty, e.IM.IMSessionID, InstantMessageDialog.InventoryAccepted, InstantMessageOnline.Offline, instance.SIMsittingPos(), client.Network.CurrentSim.RegionID, invfolder.GetBytes());   //new byte[0]); // Accept Inventory Offer
                            }
                            else if (e.IM.Dialog == InstantMessageDialog.TaskInventoryOffered)
                            {
                                client.Self.InstantMessage(client.Self.Name, e.IM.FromAgentID, string.Empty, e.IM.IMSessionID, InstantMessageDialog.TaskInventoryAccepted, InstantMessageOnline.Offline, instance.SIMsittingPos(), client.Network.CurrentSim.RegionID, invfolder.GetBytes());   //new byte[0]); // Accept Inventory Offer
                            }

                            client.Inventory.RequestFetchInventory(oID, client.Self.AgentID);

                            string ttl = "METAbolt Alert";
                            string body = e.IM.FromAgentName + " has given you a " + type + " named " + e.IM.Message;

                            TrayNotifiy(ttl, body, false);

                            return;
                        }

                        //(new frmInvOffered(instance, e.IM, oID, type)).Show(this);
                        (new frmInvOffered(instance, e.IM, oID, type)).Show(this);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log("Inventory Received error: " + ex.Message, Helpers.LogLevel.Error);
                    //reporter.Show(ex);
                }
            }
            else
            {
                if (e.IM.BinaryBucket.Length > 0)
                {
                    if (e.IM.Dialog == InstantMessageDialog.InventoryOffered)
                    {
                        client.Self.InstantMessage(client.Self.Name, e.IM.FromAgentID, string.Empty, e.IM.IMSessionID, InstantMessageDialog.InventoryDeclined, InstantMessageOnline.Offline, instance.SIMsittingPos(), client.Network.CurrentSim.RegionID, invfolder.GetBytes()); // Decline Inventory Offer

                        try
                        {
                            //client.Inventory.RemoveItem(objectID);
                            //client.Inventory.RequestFetchInventory(oID, client.Self.AgentID);

                            InventoryBase item = client.Inventory.Store.Items[oID].Data;
                            UUID content = client.Inventory.FindFolderForType(AssetType.TrashFolder);

                            InventoryFolder folder = (InventoryFolder)client.Inventory.Store.Items[content].Data;

                            if (type != AssetType.Folder)
                            {
                                client.Inventory.Move(item, folder, item.Name);
                            }
                            else
                            {
                                client.Inventory.MoveFolder(oID, content, item.Name);
                            }
                        }
                        catch { ; }
                    }
                    else if (e.IM.Dialog == InstantMessageDialog.TaskInventoryOffered)
                    {
                        client.Self.InstantMessage(client.Self.Name, e.IM.FromAgentID, string.Empty, e.IM.IMSessionID, InstantMessageDialog.TaskInventoryDeclined, InstantMessageOnline.Offline, instance.SIMsittingPos(), client.Network.CurrentSim.RegionID, invfolder.GetBytes()); // Decline Inventory Offer
                    }
                }
            }
        }

        private void HandleInventoryReplyAccepted(InstantMessageEventArgs e)
        {
            if (instance.State.IsBusy) return;

            string ttl = "METAbolt";
            string body = e.IM.FromAgentName + " has accepted your invetory offer";
            TrayNotifiy(ttl, body);
        }

        private void HandleInventoryReplyDeclined(InstantMessageEventArgs e)
        {
            if (instance.State.IsBusy) return;

            string ttl = "METAbolt";
            string body = e.IM.FromAgentName + " has declined your invetory offer";
            TrayNotifiy(ttl, body);
        }

        private void HandleGroupInvite(InstantMessageEventArgs e)
        {
            if (instance.IsAvatarMuted(e.IM.FromAgentID, e.IM.FromAgentName)) return;

            if (instance.Config.CurrentConfig.DisableInboundGroupInvites)
                return;

            if (instance.State.IsBusy) return;

            string fromAgentID = e.IM.FromAgentID.ToString();

            if (TabExists(fromAgentID))
                tabs[fromAgentID].Close();

            GRTabWindow grTab = AddGRTab(e);
            tabs[grTab.TargetUUID.ToString()].Highlight();

            string ttl = "METAbolt";
            string body = "You have received a group invite";
            TrayNotifiy(ttl, body);
        }

        private void InitializeMainTab()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    InitializeMainTab();
                    //client.Self.RetrieveInstantMessages();
                }));

                return;
            }

            MainConsole mainConsole = new MainConsole(instance);
            mainConsole.Dock = DockStyle.Fill;
            mainConsole.Visible = false;

            toolStripContainer1.ContentPanel.Controls.Add(mainConsole);

            METAboltTab tab = AddTab("main", "Main", mainConsole);
            tab.AllowClose = false;
            tab.AllowDetach = false;
            tab.AllowMerge = false;

            mainConsole.RegisterTab(tab);

            ToolStripItem item = new ToolStripSeparator();

            tstTabs.Items.Add(item);
        }

        private void InitializeChatTab()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    InitializeChatTab();
                }));

                return;
            }

            chatConsole = new ChatConsole(instance);
            chatConsole.Dock = DockStyle.Fill;
            chatConsole.Visible = false;

            toolStripContainer1.ContentPanel.Controls.Add(chatConsole);

            METAboltTab tab = AddTab("chat", "Chat", chatConsole);
            tab.AllowClose = false;
            tab.AllowDetach = false;

            ToolStripItem item = new ToolStripSeparator();

            tstTabs.Items.Add(item);
        }

        private void InitializeFriendsTab()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    InitializeFriendsTab();
                }));

                return;
            }

            FriendsConsole friendsConsole = new FriendsConsole(instance);
            friendsConsole.Dock = DockStyle.Fill;
            friendsConsole.Visible = false;

            toolStripContainer1.ContentPanel.Controls.Add(friendsConsole);

            METAboltTab tab = AddTab("friends", "Friends", friendsConsole);
            tab.AllowClose = false;
            tab.AllowDetach = true;

            ToolStripItem item = new ToolStripSeparator();

            tstTabs.Items.Add(item);
        }

        private void InitializeIMboxTab()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    InitializeIMboxTab();
                }));

                return;
            }

            IMbox imboxConsole = new IMbox(instance);
            imboxConsole.Dock = DockStyle.Fill;
            imboxConsole.Visible = false;

            toolStripContainer1.ContentPanel.Controls.Add(imboxConsole);

            METAboltTab tab = AddTab("imbox", "IMbox", imboxConsole);
            tab.AllowClose = false;
            tab.AllowDetach = true;

            ToolStripItem item = new ToolStripSeparator();

            tstTabs.Items.Add(item);

            ToolStripItem item1 = new ToolStripSeparator();
            tstTabs.Items.Add(item1);
        }

        private void InitializeGroupsTab()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    InitializeGroupsTab();
                }));

                return;
            }

            try
            {
                GroupsConsole groupsConsole = new GroupsConsole(instance);
                groupsConsole.Dock = DockStyle.Fill;
                groupsConsole.Visible = false;

                toolStripContainer1.ContentPanel.Controls.Add(groupsConsole);

                METAboltTab tab = AddTab("groups", "Groups", groupsConsole);
                tab.AllowClose = false;
                tab.AllowDetach = true;
            }
            catch (Exception ex)
            {
                //Logger.Log("Group tab error: " + ex.Message, Helpers.LogLevel.Error);
                reporter.Show(ex);
            }

            ToolStripItem item = new ToolStripSeparator();

            tstTabs.Items.Add(item);
        }

        private void InitializeSearchTab()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    InitializeSearchTab();
                }));

                return;
            }

            SearchConsole searchConsole = new SearchConsole(instance);
            searchConsole.Dock = DockStyle.Fill;
            searchConsole.Visible = false;

            toolStripContainer1.ContentPanel.Controls.Add(searchConsole);

            METAboltTab tab = AddTab("search", "Search", searchConsole);
            tab.AllowClose = false;
            tab.AllowDetach = false;

            ToolStripItem item = new ToolStripSeparator();

            tstTabs.Items.Add(item);
        }

        private void InitializeInventoryTab()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    InitializeInventoryTab();
                }));

                return;
            }

            InventoryConsole invConsole = new InventoryConsole(instance);

            invConsole.Dock = DockStyle.Fill;
            invConsole.Visible = false;

            toolStripContainer1.ContentPanel.Controls.Add(invConsole);

            METAboltTab tab = AddTab("inventory", "Inventory", invConsole);
            tab.AllowClose = false;
            tab.AllowDetach = true;

            ToolStripItem item = new ToolStripSeparator();

            tstTabs.Items.Add(item);
        }

        private void DisposeFriendsTab()
        {
            ForceCloseTab("friends");
        }

        private void DisposeGroupsTab()
        {
            ForceCloseTab("groups");
        }

        private void DisposeSearchTab()
        {
            ForceCloseTab("search");
        }

        private void DisposeIMboxTab()
        {
            ForceCloseTab("imbox");
        }

        //private void DisposeMapTab()
        //{
        //    ForceCloseTab("map");
        //}

        private void DisposeInventoryTab()
        {
            ForceCloseTab("inventory");
        }

        private void ForceCloseTab(string name)
        {
            if (!TabExists(name)) return;

            METAboltTab tab = tabs[name];
            if (tab.Merged) SplitTab(tab);

            tab.AllowClose = true;
            tab.Close();
            tab = null;
        }

        public void AddTab(METAboltTab tab)
        {
            ToolStripButton button = (ToolStripButton)tstTabs.Items.Add(tab.Label);
            button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            button.Image = null;
            button.AutoToolTip = false;
            button.Tag = tab.Name;
            button.Click += new EventHandler(TabButtonClick);

            tab.Button = button;

            if (!tabs.ContainsKey(tab.Name))
            {
                tabs.Add(tab.Name, tab);
            }
        }

        public METAboltTab AddTab(string name, string label, Control control)
        {
            ToolStripButton button = (ToolStripButton)tstTabs.Items.Add("&"+label);
            button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            button.Image = null;
            button.AutoToolTip = false;
            button.Tag = name.ToLower(CultureInfo.CurrentCulture);
            button.Click += new EventHandler(TabButtonClick);

            METAboltTab tab = new METAboltTab(button, control, name.ToLower(CultureInfo.CurrentCulture), label);
            tab.TabAttached += new EventHandler(tab_TabAttached);
            tab.TabDetached += new EventHandler(tab_TabDetached);
            tab.TabSelected += new EventHandler(tab_TabSelected);
            tab.TabClosed += new EventHandler(tab_TabClosed);

            if (!tabs.ContainsKey(tab.Name))
            {
                tabs.Add(name.ToLower(CultureInfo.CurrentCulture), tab);
            }

            //ToolStripItem item = new ToolStripSeparator();

            //tstTabs.Items.Add(item);

            return tab;
        }

        private void tab_TabAttached(object sender, EventArgs e)
        {
            METAboltTab tab = (METAboltTab)sender;
            tab.Select();
        }

        private void tab_TabDetached(object sender, EventArgs e)
        {
            METAboltTab tab = (METAboltTab)sender;
            frmDetachedTab form = (frmDetachedTab)tab.Owner;

            form.ReattachStrip = tstTabs;
            form.ReattachContainer = toolStripContainer1.ContentPanel;
        }

        private void tab_TabSelected(object sender, EventArgs e)
        {
            METAboltTab tab = (METAboltTab)sender;

            if (selectedTab != null &&
                selectedTab != tab)
            { selectedTab.Deselect(); }
            
            selectedTab = tab;

            tbtnCloseTab.Enabled = tab.AllowClose;
            owner.AcceptButton = tab.DefaultControlButton;
        }

        private void tab_TabClosed(object sender, EventArgs e)
        {
            METAboltTab tab = (METAboltTab)sender;

            if (tabs.ContainsKey(tab.Name))
            {
                tabs.Remove(tab.Name);
            }

            tab = null;
        }

        private void TabButtonClick(object sender, EventArgs e)
        {
            ToolStripButton button = (ToolStripButton)sender;

            METAboltTab tab = tabs[button.Tag.ToString()];
            tab.Select();

            //METAboltTab stab = tab.GetTab("IMbox");

            if (button.Tag.ToString() != "main" && button.Tag.ToString() != "chat" && button.Tag.ToString() != "friends" && button.Tag.ToString() != "groups" && button.Tag.ToString() != "inventory" && button.Tag.ToString() != "search" && button.Tag.ToString() != "imbox")
            {
                string tabname = button.Tag.ToString();

                //if (button.Tag.ToString().StartsWith("IM"))
                //{
                //    tabname = button.Tag.ToString().Substring(3).Trim();
                //}
                //if (button.Tag.ToString().StartsWith("GIM"))
                //{
                //    tabname = button.Tag.ToString().Substring(4).Trim();
                //}

                if (!instance.ReadIMs)
                {
                    IMbox imtab = this.instance.imBox;
                    imtab.IMRead(tabname);
                }
            }
        }

        public void RemoveTabEntry(string tabname)
        {
            if (tabs.ContainsKey(tabname))
            {
                tabs.Remove(tabname);
            }
        }

        public void RemoveTabEntry(METAboltTab tab)
        {
            tab.Button.Dispose();

            if (tabs.ContainsKey(tab.Name))
            {
                tabs.Remove(tab.Name);
            }
        }

        //Used for outside classes that have a reference to TabsConsole
        public void SelectTab(string name)
        {
            tabs[name.ToLower(CultureInfo.CurrentCulture)].Select();
        }

        public bool TabExists(string name)
        {
            return tabs.ContainsKey(name.ToLower(CultureInfo.CurrentCulture));
        }

        public METAboltTab GetTab(string name)
        {
            return tabs[name.ToLower(CultureInfo.CurrentCulture)];
        }

        public void DisplayOnIM(IMTabWindow imTab, InstantMessageEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    DisplayOnIM(imTab, e);
                }));

                return;
            }

            imTab.TextManager.ProcessIM(e);
        }

        public void DisplayOnIMGroup(IMTabWindowGroup imTab, InstantMessageEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    DisplayOnIMGroup(imTab, e);
                }));

                return;
            }

            imTab.TextManager.ProcessIM(e);
        }

        public List<METAboltTab> GetOtherTabs()
        {
            List<METAboltTab> otherTabs = new List<METAboltTab>();

            foreach (ToolStripItem item in tstTabs.Items)
            {
                if (item.Tag == null) continue;
                if ((ToolStripButton)item == selectedTab.Button) continue;

                METAboltTab tab = tabs[item.Tag.ToString()];
                if (!tab.AllowMerge) continue;
                if (tab.Merged) continue;
                
                otherTabs.Add(tab); 
            }

            return otherTabs;
        }

        public IMTabWindow AddIMTab(InstantMessageEventArgs e)
        {
            TabAgentName = e.IM.FromAgentName;
            IMTabWindow imTab = AddIMTab(e.IM.FromAgentID, e.IM.IMSessionID, TabAgentName);

            DisplayOnIM(imTab, e); 

            return imTab;
        }

        public IMTabWindow AddIMTab(UUID target, UUID session, string targetName)
        {
            IMTabWindow imTab = new IMTabWindow(instance, target, session, targetName);
            imTab.Dock = DockStyle.Fill;
            toolStripContainer1.ContentPanel.Controls.Add(imTab);

            string tname = targetName;

            if (tname.Length > 9)
            {
                tname = tname.Substring(0, 7) + "..."; 
            }

            //METAboltTab tab = 
            AddTab(targetName, "IM: " + tname, imTab);
            imTab.SelectIMInput();

            return imTab;
        }

        public IMTabWindowGroup AddIMTabGroup(InstantMessageEventArgs e)
        {
            TabAgentName = this.instance.State.GroupStore[e.IM.IMSessionID];
            Group grp = this.instance.State.Groups[e.IM.IMSessionID];

            //UUID gsession = new UUID(e.IM.BinaryBucket, 2);

            IMTabWindowGroup imTab = AddIMTabGroup(e.IM.FromAgentID, e.IM.IMSessionID, TabAgentName, grp);

            DisplayOnIMGroup(imTab, e);
            //imTab.TextManager.ProcessIM(e);

            return imTab;
        }

        public IMTabWindowGroup AddIMTabGroup(UUID target, UUID session, string targetName, Group grp)
        {
            IMTabWindowGroup imTab = new IMTabWindowGroup(instance, session, target, targetName, grp);
            imTab.Dock = DockStyle.Fill;
            toolStripContainer1.ContentPanel.Controls.Add(imTab);

            string tname = targetName;

            if (tname.Length > 9)
            {
                tname = tname.Substring(0, 7) + "...";
            }

            //METAboltTab tab = 
            AddTab(targetName, "GIM: " + targetName, imTab);
            imTab.SelectIMInput();

            return imTab;
        }

        public TPTabWindow AddTPTab(InstantMessageEventArgs e)
        {
            TPTabWindow tpTab = new TPTabWindow(instance, e);
            tpTab.Dock = DockStyle.Fill;

            toolStripContainer1.ContentPanel.Controls.Add(tpTab);
            //METAboltTab tab = 
            AddTab(tpTab.TargetUUID.ToString(), "TP: " + tpTab.TargetName, tpTab);

            return tpTab;
        }

        public FRTabWindow AddFRTab(InstantMessageEventArgs e)
        {
            FRTabWindow frTab = new FRTabWindow(instance, e);
            frTab.Dock = DockStyle.Fill;

            toolStripContainer1.ContentPanel.Controls.Add(frTab);
            //METAboltTab tab = AddTab(frTab.TargetUUID.ToString(), "FR: " + frTab.TargetName, frTab);
            AddTab(frTab.TargetUUID.ToString(), "FR: " + frTab.TargetName, frTab);

            return frTab;
        }

        //public IITabWindow AddIITab(InstantMessageEventArgs e)
        //{
        //    IITabWindow iiTab = new IITabWindow(instance, e);
        //    iiTab.Dock = DockStyle.Fill;

        //    toolStripContainer1.ContentPanel.Controls.Add(iiTab);
        //    METAboltTab tab = AddTab(iiTab.TargetUUID.ToString(), "II: " + iiTab.TargetName, iiTab);

        //    return iiTab;
        //}

        public GRTabWindow AddGRTab(InstantMessageEventArgs e)
        {
            GRTabWindow grTab = new GRTabWindow(instance, e);
            grTab.Dock = DockStyle.Fill;

            toolStripContainer1.ContentPanel.Controls.Add(grTab);
            //METAboltTab tab = 
            AddTab(grTab.TargetUUID.ToString(), "GR: " + grTab.TargetName, grTab);

            return grTab;
        }

        private void tbtnTabOptions_Click(object sender, EventArgs e)
        {
            tmnuMergeWith.Enabled = selectedTab.AllowMerge;
            tmnuDetachTab.Enabled = selectedTab.AllowDetach;

            tmnuMergeWith.DropDown.Items.Clear();

            if (!selectedTab.AllowMerge) return;
            if (!selectedTab.Merged)
            {
                tmnuMergeWith.Text = "Merge With";

                List<METAboltTab> otherTabs = GetOtherTabs();

                tmnuMergeWith.Enabled = (otherTabs.Count > 0);
                if (!tmnuMergeWith.Enabled) return;

                foreach (METAboltTab tab in otherTabs)
                {
                    ToolStripItem item = tmnuMergeWith.DropDown.Items.Add(tab.Label);
                    item.Tag = tab.Name;
                    item.Click += new EventHandler(MergeItemClick);
                }
            }
            else
            {
                tmnuMergeWith.Text = "Split";
                tmnuMergeWith.Click += new EventHandler(SplitClick);
            }
        }

        private void MergeItemClick(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            METAboltTab tab = tabs[item.Tag.ToString()];

            selectedTab.MergeWith(tab);

            SplitContainer container = (SplitContainer)selectedTab.Control;
            toolStripContainer1.ContentPanel.Controls.Add(container);

            selectedTab.Select();
            RemoveTabEntry(tab);

            if (!tabs.ContainsKey(tab.Name))
            {
                tabs.Add(tab.Name, selectedTab);
            }
        }

        private void SplitClick(object sender, EventArgs e)
        {
            SplitTab(selectedTab);
            selectedTab.Select();
        }

        public void SplitTab(METAboltTab tab)
        {
            METAboltTab otherTab = tab.Split();
            if (otherTab == null) return;

            toolStripContainer1.ContentPanel.Controls.Add(tab.Control);
            toolStripContainer1.ContentPanel.Controls.Add(otherTab.Control);

            if (tabs.ContainsKey(tab.Name))
            {
                tabs.Remove(otherTab.Name);
            }
            AddTab(otherTab);
        }

        private void tmnuDetachTab_Click(object sender, EventArgs e)
        {
            if (!selectedTab.AllowDetach) return;

            tstTabs.Items.Remove(selectedTab.Button);
            selectedTab.Detach(instance);
            selectedTab.Owner.Show();

            tabs["chat"].Select();
        }

        private void tbtnCloseTab_Click(object sender, EventArgs e)
        {
            METAboltTab tab = selectedTab;

            tabs["chat"].Select();
            tab.Close();
            
            tab = null;
        }

        private void TabsConsole_Load(object sender, EventArgs e)
        {
            owner = this.FindForm();
        }

        private void tstTabs_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            this.instance.State.CurrentTab = e.ClickedItem.Text;
        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (instance.MainForm.WindowState == FormWindowState.Normal)
            {
                
                instance.MainForm.Hide();
                instance.MainForm.WindowState = FormWindowState.Minimized;
            }
            else
            {
                instance.MainForm.Show();
                instance.MainForm.WindowState = FormWindowState.Normal;
            }
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            instance.MainForm.Show();
            instance.MainForm.WindowState = FormWindowState.Normal;  
        }

        private void closeMETAboltToolStripMenuItem_Click(object sender, EventArgs e)
        {
            instance.MainForm.Close(); 
        }

        private void TabsConsole_SizeChanged(object sender, EventArgs e)
        {

        }

        private void TabsConsole_KeyUp(object sender, KeyEventArgs e)
        {
            
        }
    }
}
