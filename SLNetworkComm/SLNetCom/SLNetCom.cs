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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Timers;
using OpenMetaverse;
using OpenMetaverse.Packets;
using System.Net;
using System.Management; 


namespace SLNetworkComm
{
    /// <summary>
    /// SLNetCom is a class built on top of OpenMetaverse that provides a way to
    /// raise events on the proper thread (for GUI apps especially).
    /// </summary>
    public partial class SLNetCom
    {
        private GridClient client;
        private LoginOptions loginOptions;

        private bool loggingIn = false;
        private bool loggedIn = false;
        private bool teleporting = false;

        private const string MainGridLogin = @"https://login.agni.lindenlab.com/cgi-bin/login.cgi";
        private const string BetaGridLogin = @"https://login.aditi.lindenlab.com/cgi-bin/login.cgi";

        // NetcomSync is used for raising certain events on the
        // GUI/main thread. Useful if you're modifying GUI controls
        // in the client app when responding to those events.
        private ISynchronizeInvoke netcomSync;

        public SLNetCom(GridClient client)
        {
            this.client = client;
            loginOptions = new LoginOptions();

            AddClientEvents();
        }

        private void AddClientEvents()
        {
            client.Self.ChatFromSimulator += new EventHandler<OpenMetaverse.ChatEventArgs>(Self_OnChat);
            client.Self.IM += new EventHandler<OpenMetaverse.InstantMessageEventArgs>(Self_OnInstantMessage);
            client.Self.ScriptDialog += new EventHandler<OpenMetaverse.ScriptDialogEventArgs>(Self_OnDialog);
            client.Self.MoneyBalance += new EventHandler<OpenMetaverse.BalanceEventArgs>(Avatar_OnBalanceUpdated);
            client.Self.TeleportProgress += new EventHandler<OpenMetaverse.TeleportEventArgs>(Self_OnTeleport);
            //client.Network.Connected += new NetworkManager.ConnectedCallback(Network_OnConnected);
            client.Network.Disconnected += new EventHandler<OpenMetaverse.DisconnectedEventArgs>(Network_OnDisconnected);
            client.Network.LoginProgress += new EventHandler<OpenMetaverse.LoginProgressEventArgs>(Network_OnLogin);
            client.Network.LoggedOut += new EventHandler<OpenMetaverse.LoggedOutEventArgs>(Network_OnLogoutReply);
            client.Self.ScriptQuestion += new EventHandler<OpenMetaverse.ScriptQuestionEventArgs>(Self_OnDialogQuestion);
            client.Self.LoadURL += new EventHandler<OpenMetaverse.LoadUrlEventArgs>(Self_OnURLLoad);
            client.Self.AlertMessage += new EventHandler<OpenMetaverse.AlertMessageEventArgs>(Self_AlertMessage);
        }

        private void Self_OnInstantMessage(object sender, OpenMetaverse.InstantMessageEventArgs ea)
        {
            try
            {
                if (netcomSync != null)
                    netcomSync.BeginInvoke(new OnInstantMessageRaise(OnInstantMessageReceived), new object[] { ea });
                else
                    OnInstantMessageReceived(ea);

            }
            catch (Exception exp)
            {
                OpenMetaverse.Logger.Log(exp.Message.ToString(), Helpers.LogLevel.Error);
            }
        }

        private void Network_OnLogin(object sender, OpenMetaverse.LoginProgressEventArgs ea)
        {
            try
            {
                if (ea.Status == LoginStatus.Success) loggedIn = true;

                if (netcomSync != null)
                    netcomSync.BeginInvoke(new OnClientLoginRaise(OnClientLoginStatus), new object[] { ea });
                else
                    OnClientLoginStatus(ea);

                client.Self.RequestBalance();
            }
            catch (Exception ex)
            {
                Logger.Log("SLnetcomm (onlogin) " + ex.Message, Helpers.LogLevel.Error);
            }
        }

        private void Network_OnLogoutReply(object sender, LoggedOutEventArgs ea)
        {
            try
            {
                loggedIn = false;

                if (netcomSync != null)
                    netcomSync.BeginInvoke(new OnClientLogoutRaise(OnClientLoggedOut), new object[] { EventArgs.Empty });
                else
                    OnClientLoggedOut(EventArgs.Empty);
            }
            catch
            {
                ;
            }
        }

        private void Self_OnTeleport(object sender, TeleportEventArgs ea)
        {
            try
            {
                if (ea.Status == TeleportStatus.Finished || ea.Status == TeleportStatus.Failed)
                    teleporting = false;

                if (netcomSync != null)
                    netcomSync.BeginInvoke(new OnTeleportStatusRaise(OnTeleportStatusChanged), new object[] { ea });
                else
                    OnTeleportStatusChanged(ea);
            }
            catch
            {
                ;
            }
        }

        private void Self_OnChat(object sender, OpenMetaverse.ChatEventArgs ea)
        {
            try
            {
                if (netcomSync != null)
                    netcomSync.BeginInvoke(new OnChatRaise(OnChatReceived), new object[] { ea });
                else
                    OnChatReceived(ea);
            }
            catch
            {
                ;
            }
        }

        private void Self_OnDialog(object sender, OpenMetaverse.ScriptDialogEventArgs ea)
        {
            try
            {
                if (netcomSync != null)
                    netcomSync.BeginInvoke(new OnScriptDialogRaise(OnScriptDialogReceived), new object[] { ea });
                else
                    OnScriptDialogReceived(ea);
            }
            catch
            {
                ;
            }
        }

        private void Self_OnDialogQuestion(object sender, OpenMetaverse.ScriptQuestionEventArgs ea)
        {
            try
            {
                if (netcomSync != null)
                    netcomSync.BeginInvoke(new OnScriptQuestionRaise(OnScriptQuestionReceived), new object[] { ea });
                else
                    OnScriptQuestionReceived(ea);
            }
            catch
            {
                ;
            }
        }

        private void Self_OnURLLoad(object sender, LoadUrlEventArgs ea)
        {
            if (netcomSync != null)
                netcomSync.BeginInvoke(new OnLoadURLRaise(OnLoadURL), new object[] { ea });
            else
                OnLoadURL(ea);
        }

        private void Network_OnDisconnected(object sender, DisconnectedEventArgs ea)
        {
            if (!loggedIn) return;

            loggedIn = false;

            try
            {
                if (netcomSync != null)
                    netcomSync.BeginInvoke(new OnClientDisconnectRaise(OnClientDisconnected), new object[] { ea });
                else
                    OnClientDisconnected(ea);
            }
            catch
            {
                ;
            }
        }

        private void Avatar_OnBalanceUpdated(object sender, BalanceEventArgs ea)
        {
            try
            {
                if (netcomSync != null)
                    netcomSync.BeginInvoke(new OnMoneyBalanceRaise(OnMoneyBalanceUpdated), new object[] { ea });
                else
                    OnMoneyBalanceUpdated(ea);
            }
            catch
            {
                ;
            }
        }

        void Self_AlertMessage(object sender, OpenMetaverse.AlertMessageEventArgs ea)
        {
            if (netcomSync != null)
                netcomSync.BeginInvoke(new OnAlertMessageRaise(OnAlertMessageReceived), new object[] { ea });
            else
                OnAlertMessageReceived(ea);
        }

        public void Login()
        {
            try
            {
                loggingIn = true;

                OverrideEventArgs ea = new OverrideEventArgs();
                OnClientLoggingIn(ea);

                if (ea.Cancel)
                {
                    loggingIn = false;
                    return;
                }

                if (string.IsNullOrEmpty(loginOptions.FirstName) ||
                    string.IsNullOrEmpty(loginOptions.LastName) ||
                    string.IsNullOrEmpty(loginOptions.Password))
                {
                    OnClientLoginStatus(
                        new LoginProgressEventArgs(LoginStatus.Failed, "One or more fields are blank.", string.Empty));
                }

                string startLocation = string.Empty;

                switch (loginOptions.StartLocation)
                {
                    case StartLocationType.Home: startLocation = "home"; break;
                    case StartLocationType.Last: startLocation = "last"; break;

                    case StartLocationType.Custom:
                        startLocation = loginOptions.StartLocationCustom.Trim();

                        StartLocationParser parser = new StartLocationParser(startLocation);
                        startLocation = NetworkManager.StartLocation(parser.Sim, parser.X, parser.Y, parser.Z);

                        break;
                }

                string password;

                //if (loginOptions.IsPasswordMD5)
                //    password = loginOptions.Password;
                //else
                //    password = OpenMetaverse.Utils.MD5(loginOptions.Password);

                password = OpenMetaverse.Utils.MD5(loginOptions.Password);

                string[] agt = loginOptions.UserAgent.Split(' ');
 
                LoginParams loginParams = client.Network.DefaultLoginParams(
                    loginOptions.FirstName, loginOptions.LastName, password,
                    agt[0], loginOptions.UserAgent.Substring(9));

                loginParams.Start = startLocation;

                loginParams.AgreeToTos = true;
                loginParams.Channel = "METAbolt";
                loginParams.Author = loginOptions.Author;
                
                //loginParams.MAC = GetMACAddress();
                //loginParams.MethodName = string.Empty;
                //loginParams.Platform = "Windows";
                //loginParams.ReadCritical = false;
                //loginParams.Version = ;  

                // V 0.9.1.6 change
                switch (loginOptions.Grid)
                {
                    case LoginGrid.MainGrid: client.Settings.LOGIN_SERVER = MainGridLogin; loginParams.URI = MainGridLogin; break;
                    case LoginGrid.BetaGrid: client.Settings.LOGIN_SERVER = BetaGridLogin; loginParams.URI = BetaGridLogin; break;
                    case LoginGrid.Custom: client.Settings.LOGIN_SERVER = loginOptions.GridCustomLoginUri; loginParams.URI = loginOptions.GridCustomLoginUri; break;
                }

                client.Network.BeginLogin(loginParams);
            }
            catch (Exception ex)
            {
                Logger.Log("Connection to SL failed", Helpers.LogLevel.Warning, ex);
            }
        }

        public string GetMACAddress()
        {
            ManagementObjectSearcher query = null;
            ManagementObjectCollection queryCollection = null;

            string macad = string.Empty;  

            try
            {
                query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");

                queryCollection = query.Get();

                foreach (ManagementObject mo in queryCollection)
                {
                    if (mo["MacAddress"] != null)
                    {
                        macad = mo["MacAddress"].ToString();
                    }
                }
            }
            catch
            {
                macad = string.Empty;  
            }

            return macad; 
        } 


        public void Logout()
        {
            if (!loggedIn)
            {
                OnClientLoggedOut(EventArgs.Empty);
                return;
            }

            OverrideEventArgs ea = new OverrideEventArgs();
            OnClientLoggingOut(ea);
            if (ea.Cancel) return;

            client.Network.Logout();
        }

        public void ChatOut(string chat, ChatType type, int channel)
        {
            if (!loggedIn) return;

            client.Self.Chat(chat, channel, type);
            OnChatSent(new ChatSentEventArgs(chat, type, channel));
        }

        public void SendInstantMessage(string message, UUID target, UUID session)
        {
            if (!loggedIn) return;

            //client.Self.InstantMessage(target, message, session);

            client.Self.InstantMessage(
                loginOptions.FullName, target, message, session, InstantMessageDialog.MessageFromAgent,
                InstantMessageOnline.Online, client.Self.SimPosition, client.Network.CurrentSim.ID, null);

            OnInstantMessageSent(new InstantMessageSentEventArgs(message, target, session, DateTime.Now));
        }

        public void SendInstantMessageGroup(string message, UUID target, UUID session)
        {
            if (!loggedIn) return;

            //client.Self.InstantMessageGroup(target, message);
            client.Self.InstantMessageGroup(session, message);

            OnInstantMessageSent(new InstantMessageSentEventArgs(message, target, session, DateTime.Now, true));
        }

        public void SendIMStartTyping(UUID target, UUID session)
        {
            if (!loggedIn) return;

            client.Self.InstantMessage(
                loginOptions.FullName, target, "typing", session, InstantMessageDialog.StartTyping,
                InstantMessageOnline.Online, client.Self.SimPosition, client.Network.CurrentSim.ID, null);
        }

        public void SendIMStopTyping(UUID target, UUID session)
        {
            if (!loggedIn) return;

            client.Self.InstantMessage(
                loginOptions.FullName, target, "typing", session, InstantMessageDialog.StopTyping,
                InstantMessageOnline.Online, client.Self.SimPosition, client.Network.CurrentSim.ID, null);
        }

        public void Teleport(string sim, Vector3 coordinates)
        {
            if (!loggedIn) return;
            if (teleporting) return;

            TeleportingEventArgs ea = new TeleportingEventArgs(sim, coordinates);
            OnTeleporting(ea);
            if (ea.Cancel) return;

            teleporting = true;
            client.Self.Teleport(sim, coordinates);
        }

        public bool IsLoggingIn
        {
            get { return loggingIn; }
        }

        public bool IsLoggedIn
        {
            get { return loggedIn; }
        }

        public bool IsTeleporting
        {
            get { return teleporting; }
        }

        public LoginOptions LoginOptions
        {
            get { return loginOptions; }
            set { loginOptions = value; }
        }

        public ISynchronizeInvoke NetcomSync
        {
            get { return netcomSync; }
            set { netcomSync = value; }
        }
    }
}
