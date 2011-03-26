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
using System.Text;
using OpenMetaverse;

namespace SLNetworkComm
{
    public partial class SLNetCom
    {
        // For the NetcomSync stuff
        private delegate void OnClientLoginRaise(OpenMetaverse.LoginProgressEventArgs e);
        private delegate void OnClientLogoutRaise(EventArgs e);
        private delegate void OnClientDisconnectRaise(DisconnectedEventArgs e);
        private delegate void OnChatRaise(OpenMetaverse.ChatEventArgs e);
        private delegate void OnScriptDialogRaise(OpenMetaverse.ScriptDialogEventArgs e);
        private delegate void OnInstantMessageRaise(OpenMetaverse.InstantMessageEventArgs e);
        private delegate void OnAlertMessageRaise(OpenMetaverse.AlertMessageEventArgs e);
        private delegate void OnMoneyBalanceRaise(OpenMetaverse.BalanceEventArgs e);
        private delegate void OnScriptQuestionRaise(OpenMetaverse.ScriptQuestionEventArgs e);
        private delegate void OnLoadURLRaise(OpenMetaverse.LoadUrlEventArgs e);
        private delegate void OnTeleportStatusRaise(OpenMetaverse.TeleportEventArgs e);
       

        public event EventHandler<OverrideEventArgs> ClientLoggingIn;
        public event EventHandler<OpenMetaverse.LoginProgressEventArgs> ClientLoginStatus;
        public event EventHandler<OverrideEventArgs> ClientLoggingOut;
        public event EventHandler ClientLoggedOut;
        public event EventHandler<OpenMetaverse.DisconnectedEventArgs> ClientDisconnected;
        public event EventHandler<OpenMetaverse.ChatEventArgs> ChatReceived;
        public event EventHandler<OpenMetaverse.ScriptDialogEventArgs> ScriptDialogReceived;
        public event EventHandler<ChatSentEventArgs> ChatSent;
        public event EventHandler<OpenMetaverse.InstantMessageEventArgs> InstantMessageReceived;
        public event EventHandler<InstantMessageSentEventArgs> InstantMessageSent;
        public event EventHandler<TeleportingEventArgs> Teleporting;
        public event EventHandler<OpenMetaverse.TeleportEventArgs> TeleportStatusChanged;
        public event EventHandler<OpenMetaverse.AlertMessageEventArgs> AlertMessageReceived;
        public event EventHandler<OpenMetaverse.BalanceEventArgs> MoneyBalanceUpdated;
        public event EventHandler<OpenMetaverse.ScriptQuestionEventArgs> ScriptQuestionReceived;
        public event EventHandler<OpenMetaverse.LoadUrlEventArgs> LoadURLReceived;
        //public event EventHandler<FriendsManager.FriendshipOfferedEvent> FriendshipOfferRcvd; 
        ////client.Friends.OnFriendOnline += new FriendsManager.FriendOnlineEvent(Friends_OnFriendOnline);

        //protected virtual void OnFriendshipOfferRcvd(FriendsManager.FriendshipOfferedEvent e)
        //{
        //    if (FriendshipOfferRcvd != null) FriendshipOfferRcvd(this, e);
        //}

        protected virtual void OnClientLoggingIn(OverrideEventArgs e)
        {
            if (ClientLoggingIn != null) ClientLoggingIn(this, e);
        }

        protected virtual void OnLoadURL(OpenMetaverse.LoadUrlEventArgs e)
        {
            if (LoadURLReceived != null) LoadURLReceived(this, e);
        }

        protected virtual void OnClientLoginStatus(OpenMetaverse.LoginProgressEventArgs e)
        {
            if (ClientLoginStatus != null) ClientLoginStatus(this, e);
        }

        protected virtual void OnClientLoggingOut(OverrideEventArgs e)
        {
            if (ClientLoggingOut != null) ClientLoggingOut(this, e);
        }

        protected virtual void OnClientLoggedOut(EventArgs e)
        {
            if (ClientLoggedOut != null) ClientLoggedOut(this, e);
        }

        protected virtual void OnClientDisconnected(OpenMetaverse.DisconnectedEventArgs e)
        {
            if (ClientDisconnected != null) ClientDisconnected(this, e);
        }

        protected virtual void OnChatReceived(OpenMetaverse.ChatEventArgs e)
        {
            if (ChatReceived != null) ChatReceived(this, e);
        }

        protected virtual void OnScriptDialogReceived(OpenMetaverse.ScriptDialogEventArgs e)
        {
            if (ScriptDialogReceived != null) ScriptDialogReceived(this, e);
            
        }

        protected virtual void OnScriptQuestionReceived(OpenMetaverse.ScriptQuestionEventArgs e)
        {
            if (ScriptQuestionReceived != null) ScriptQuestionReceived(this, e);

        }

        protected virtual void OnChatSent(ChatSentEventArgs e)
        {
            if (ChatSent != null) ChatSent(this, e);
        }

        protected virtual void OnInstantMessageReceived(OpenMetaverse.InstantMessageEventArgs e)
        {
            if (InstantMessageReceived != null) InstantMessageReceived(this, e);
        }

        protected virtual void OnInstantMessageSent(InstantMessageSentEventArgs e)
        {
            if (InstantMessageSent != null) InstantMessageSent(this, e);
        }

        protected virtual void OnTeleporting(TeleportingEventArgs e)
        {
            if (Teleporting != null) Teleporting(this, e);
        }

        protected virtual void OnTeleportStatusChanged(OpenMetaverse.TeleportEventArgs e)
        {

            if (TeleportStatusChanged != null) TeleportStatusChanged(this, e);
        }

        protected virtual void OnAlertMessageReceived(OpenMetaverse.AlertMessageEventArgs e)
        {
            if (AlertMessageReceived != null) AlertMessageReceived(this, e);
        }

        protected virtual void OnMoneyBalanceUpdated(OpenMetaverse.BalanceEventArgs e)
        {
            if (MoneyBalanceUpdated != null) MoneyBalanceUpdated(this, e);
        }
    }
}
