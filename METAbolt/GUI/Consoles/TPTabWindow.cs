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
using SLNetworkComm;
using System.Threading;

namespace METAbolt
{
    public partial class TPTabWindow : UserControl
    {
        private METAboltInstance instance;
        private SLNetCom netcom;
        private GridClient client;
        private string targetName;
        private UUID targetUUID = UUID.Zero;
        private ManualResetEvent TPEvent = new ManualResetEvent(false);
        private UUID targetSession = UUID.Zero;  

        public TPTabWindow(METAboltInstance instance, InstantMessageEventArgs e)
        {
            InitializeComponent();

            this.instance = instance;
            netcom = this.instance.Netcom;
            client = this.instance.Client;

            Disposed += new EventHandler(TPTabWindow_Disposed);

            ProcessEventArgs(e);

            netcom.TeleportStatusChanged += new EventHandler<TeleportEventArgs>(netcom_TeleportStatusChanged);
        }

        private void TPTabWindow_Disposed(object sender, EventArgs e)
        {
            netcom.TeleportStatusChanged -= new EventHandler<TeleportEventArgs>(netcom_TeleportStatusChanged);
        }

        private void netcom_TeleportStatusChanged(object sender, TeleportEventArgs e)
        { 
            switch (e.Status)
            {
                case TeleportStatus.Failed:
                    MessageBox.Show(e.Message, "Teleport", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case TeleportStatus.Finished:
                    //try
                    //{
                    //    client.Appearance.SetPreviousAppearance(false);
                    //}
                    //catch (Exception exp)
                    //{
                    //    Logger.Log("TPTabWindow: " + exp.InnerException.ToString(), Helpers.LogLevel.Error);
                    //}
                    break;
            }
        }

        private void ProcessEventArgs(InstantMessageEventArgs e)
        {
            targetName = e.IM.FromAgentName;
            targetUUID = e.IM.FromAgentID;
            targetSession = e.IM.IMSessionID; 

            lblSubheading.Text =
                "Received teleport offer from " + targetName + " with message:";

            rtbOfferMessage.AppendText(e.IM.Message);
        }

        public void CloseTab()
        {
            instance.TabConsole.GetTab("chat").Select();
            instance.TabConsole.GetTab(targetUUID.ToString()).Close();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (instance.State.IsSitting)
            {
                client.Self.Stand();
                instance.State.SetStanding();
                TPEvent.WaitOne(2000, false);
            }

            client.Self.TeleportLureRespond(targetUUID, targetSession, true);
            CloseTab();
        }

        private void btnDecline_Click(object sender, EventArgs e)
        {
            client.Self.TeleportLureRespond(targetUUID, UUID.Random(), false);
            CloseTab();
        }

        public string TargetName
        {
            get { return targetName; }
        }

        public UUID TargetUUID
        {
            get { return targetUUID; }
        }

        private void TPTabWindow_Load(object sender, EventArgs e)
        {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void rtbOfferMessage_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (e.LinkText.StartsWith("http://slurl."))
            {
                // Open up the TP form here
                string encoded = System.Web.HttpUtility.UrlDecode(e.LinkText);
                string[] split = encoded.Split(new Char[] { '/' });
                //string[] split = e.LinkText.Split(new Char[] { '/' });
                string sim = split[4].ToString();
                double x = Convert.ToDouble(split[5].ToString());
                double y = Convert.ToDouble(split[6].ToString());
                double z = Convert.ToDouble(split[7].ToString());

                (new frmTeleport(instance, sim, (float)x, (float)y, (float)z, false)).Show();

            }
            else if (e.LinkText.StartsWith("http://maps.secondlife"))
            {
                // Open up the TP form here
                string encoded = System.Web.HttpUtility.UrlDecode(e.LinkText);
                string[] split = encoded.Split(new Char[] { '/' });
                //string[] split = e.LinkText.Split(new Char[] { '/' });
                string sim = split[4].ToString();
                double x = Convert.ToDouble(split[5].ToString());
                double y = Convert.ToDouble(split[6].ToString());
                double z = Convert.ToDouble(split[7].ToString());

                (new frmTeleport(instance, sim, (float)x, (float)y, (float)z, true)).Show();

            }
            else if (e.LinkText.Contains("http://mbprofile:"))
            {
                string encoded = System.Web.HttpUtility.UrlDecode(e.LinkText);
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
            //else if (e.LinkText.Contains("secondlife:///"))
            //{
            //    // Open up the Group Info form here
            //    //string[] split = e.LinkText.Split(new Char[] { '/' });
            //    //UUID uuid = (UUID)split[4].ToString();

            //    //frmGroupInfo frm = new frmGroupInfo(uuid, instance);
            //    //frm.Show();
            //}
            else if (e.LinkText.StartsWith("http://") || e.LinkText.StartsWith("ftp://") || e.LinkText.StartsWith("https://"))
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            else
            {
                System.Diagnostics.Process.Start("http://" + e.LinkText);
            }
        }
    }
}
