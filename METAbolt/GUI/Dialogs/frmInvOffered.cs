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
using System.Media; 

namespace METAbolt
{
    public partial class frmInvOffered : Form
    {
        private METAboltInstance instance;
        private GridClient client;
        private InstantMessage msg;
        private UUID objectID;
        private bool diainv = true;

        public frmInvOffered(METAboltInstance instance, InstantMessage e, UUID objectID, AssetType type)
        {
            InitializeComponent();

            this.instance = instance;
            client = this.instance.Client;
            msg = e;
            this.objectID = objectID;
            this.Text += " [" + client.Self.Name + "]";

            if (e.Dialog == InstantMessageDialog.TaskInventoryOffered)
            {
                diainv = false;
                //this.Text = "Taskinventory item received";
            }

            lblSubheading.Text = "You have received a " + type.ToString() + " named '" + e.Message + "' from " + e.FromAgentName;

            if (instance.Config.CurrentConfig.PlayInventoryItemReceived)
            {
                SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.Item_received);
                simpleSound.Play();
                simpleSound.Dispose();
            }

            timer1.Interval = instance.DialogTimeOut;
            timer1.Enabled = true;
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client.Inventory.RemoveItem(objectID);

            if (instance.MuteList.Rows.Contains(msg.FromAgentID.ToString()))
            {
                MessageBox.Show(msg.FromAgentName + " is already in your mute list.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataRow dr = instance.MuteList.NewRow();
            dr["uuid"] = msg.FromAgentID;
            dr["mute_name"] = msg.FromAgentName;
            instance.MuteList.Rows.Add(dr);

            MessageBox.Show(msg.FromAgentName + " is now muted.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);      

            this.Close();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (diainv)
            {
                client.Self.InstantMessage(client.Self.Name, msg.FromAgentID, string.Empty, msg.IMSessionID, InstantMessageDialog.InventoryAccepted, InstantMessageOnline.Offline, instance.SIMsittingPos(), client.Network.CurrentSim.RegionID, new byte[0]); // Accept Inventory Offer

                client.Inventory.RequestFetchInventory(objectID, client.Self.AgentID);
            }
            else
            {
                client.Self.InstantMessage(client.Self.Name, msg.FromAgentID, string.Empty, msg.IMSessionID, InstantMessageDialog.TaskInventoryAccepted, InstantMessageOnline.Offline, instance.SIMsittingPos(), client.Network.CurrentSim.RegionID, new byte[0]); // Accept Inventory Offer
            }
            
            this.Close(); 
        }

        private void btnDecline_Click(object sender, EventArgs e)
        {
            if (diainv)
            {
                client.Self.InstantMessage(client.Self.Name, msg.FromAgentID, string.Empty, msg.IMSessionID, InstantMessageDialog.InventoryDeclined, InstantMessageOnline.Offline, instance.SIMsittingPos(), client.Network.CurrentSim.RegionID, new byte[0]); // Decline Inventory Offer

                try
                {
                    client.Inventory.RemoveItem(objectID);
                }
                catch { ; }
            }
            else
            {
                client.Self.InstantMessage(client.Self.Name, msg.FromAgentID, string.Empty, msg.IMSessionID, InstantMessageDialog.TaskInventoryDeclined, InstantMessageOnline.Offline, instance.SIMsittingPos(), client.Network.CurrentSim.RegionID, new byte[0]); // Decline Inventory Offer
            }

            this.Close();
        }

        private void frmInvOffered_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btnDecline.PerformClick();
        }
    }
}
