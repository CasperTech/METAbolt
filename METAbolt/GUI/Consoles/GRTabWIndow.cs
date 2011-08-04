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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
//using SLNetworkComm;

namespace METAbolt
{
    public partial class GRTabWindow : UserControl
    {
        private METAboltInstance instance;
        //private SLNetCom netcom;
        private GridClient client;
        private string targetName;
        private UUID targetUUID;
        private UUID isession;

        public GRTabWindow(METAboltInstance instance, InstantMessageEventArgs e)
        {
            InitializeComponent();

            this.instance = instance;
            //netcom = this.instance.Netcom;
            client = this.instance.Client;
            ProcessEventArgs(e);
        }

        private void ProcessEventArgs(InstantMessageEventArgs e)
        {
            string[] split;

            try
            {
                targetName = e.IM.FromAgentName;
                targetUUID = e.IM.FromAgentID;
                isession = e.IM.IMSessionID;
                string gmsg = e.IM.Message.ToString();

                split = gmsg.Split(new Char[] { ':' });

                if (split.Rank > 1)
                {

                    lblSubheading.Text = split[0].ToString() + "\n \n" + split[1].ToString();
                }
                else
                {
                    lblSubheading.Text = split[0].ToString();
                }
            }
            catch { ; }
        }

        public void CloseTab()
        {
            try
            {
                instance.TabConsole.GetTab("chat").Select();
                instance.TabConsole.GetTab(targetUUID.ToString()).Close();
            }
            catch
            {
                ;
            }
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

        private void btnAccept_Click(object sender, EventArgs e)
        {
            // There is a bug here which needs to be looked at some stage
            try
            {
                client.Self.InstantMessage(client.Self.Name, targetUUID, string.Empty, isession, InstantMessageDialog.GroupInvitationAccept, InstantMessageOnline.Offline, instance.SIMsittingPos(), UUID.Zero, new byte[0]); // Accept Group Invitation (Join Group)
                CloseTab();
            }
            catch
            {
                ; 
            }
        }

        private void btnDecline_Click(object sender, EventArgs e)
        {
            client.Self.InstantMessage(client.Self.Name, targetUUID, string.Empty, isession, InstantMessageDialog.GroupInvitationDecline, InstantMessageOnline.Offline, instance.SIMsittingPos(), UUID.Zero, new byte[0]); // Decline Group Invitation
            CloseTab();
        }
    }
}