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
    public partial class IITabWindow : UserControl
    {
        private METAboltInstance instance;
        private SLNetCom netcom;
        private GridClient client;
        private string targetName;
        private UUID targetUUID;
        private UUID isession;

        public IITabWindow(METAboltInstance instance, InstantMessage e)
        {
            InitializeComponent();

            this.instance = instance;
            netcom = this.instance.Netcom;
            client = this.instance.Client;
            ProcessEventArgs(e);
        }

        private void ProcessEventArgs(InstantMessage e)
        {
            targetName = e.FromAgentName;
            targetUUID = e.FromAgentID;
            isession = e.IMSessionID;
            string gmsg = e.Message.ToString();
            

            // This was causing a crash when an inventory item is received
            // there is no ":" why was it looking for it???
            // section now removed and replaced with something that makes sense
            //
            //string[] split = gmsg.Split(new Char[] { ':' });
            //lblSubheading.Text = split[0].ToString() + "\n \n" + split[1].ToString();
            lblSubheading.Text = "You have received an inventory item named '" + gmsg + "' from " + targetName;
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


        // The below are to be done in the future
        // at the moment METAbolt auto accepts all inventory offers...
        // not good but it's fine for the moment
        private void btnAccept_Click_1(object sender, EventArgs e)
        {
            client.Self.InstantMessage(client.Self.Name, targetUUID, string.Empty, isession, InstantMessageDialog.InventoryAccepted, InstantMessageOnline.Offline, client.Self.SimPosition, UUID.Zero, new byte[0]); // Accept Inventory Offer
            CloseTab();
        }

        private void btnDecline_Click_1(object sender, EventArgs e)
        {
            client.Self.InstantMessage(client.Self.Name, targetUUID, string.Empty, isession, InstantMessageDialog.InventoryDeclined, InstantMessageOnline.Offline, client.Self.SimPosition, UUID.Zero, new byte[0]); // Decline Inventory Offer
            CloseTab();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void lblSubheading_Click(object sender, EventArgs e)
        {

        }
    }
}