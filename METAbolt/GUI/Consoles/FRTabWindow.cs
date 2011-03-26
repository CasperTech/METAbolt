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
using SLNetworkComm;

namespace METAbolt
{
    public partial class FRTabWindow : UserControl
    {
        private METAboltInstance instance;
        private SLNetCom netcom;
        private GridClient client;
        private string targetName;
        private UUID targetUUID;
        private UUID isession;
        private FriendsConsole fconsole;

        public FRTabWindow(METAboltInstance instance, InstantMessageEventArgs e)
        {
            InitializeComponent();

            this.instance = instance;
            netcom = this.instance.Netcom;
            client = this.instance.Client;
            ProcessEventArgs(e);
        }

        private void ProcessEventArgs(InstantMessageEventArgs e)
        {
            targetName = e.IM.FromAgentName;
            targetUUID = e.IM.FromAgentID;
            isession = e.IM.IMSessionID;

            lblSubheading.Text =
                "You have received a Friendship invite from " + targetName + "";

            //rtbOfferMessage.AppendText(e.IM.Message);
        }

        public void CloseTab()
        {
            instance.TabConsole.GetTab("chat").Select();
            instance.TabConsole.GetTab(targetUUID.ToString()).Close();
        }

        public string TargetName
        {
            get { return targetName; }
        }

        public UUID TargetUUID
        {
            get { return targetUUID; }
        }

        public UUID iSession
        {
            get { return isession; }
        }

        private void btnAccept_Click_1(object sender, EventArgs e)
        {
            client.Friends.AcceptFriendship(targetUUID, isession);

            fconsole = new FriendsConsole(instance); 
            fconsole.InitializeFriendsList();
            //BeginInvoke(new MethodInvoker(fconsole.InitializeFriendsList()));
            //BeginInvoke(new MethodInvoker(fconsole.InitializeFriendsList()));
            CloseTab();
        }

        private void btnDecline_Click_1(object sender, EventArgs e)
        {
            client.Friends.DeclineFriendship(targetUUID, isession);
            CloseTab();
        }
    }
}
