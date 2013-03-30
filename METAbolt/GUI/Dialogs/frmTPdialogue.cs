using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
using SLNetworkComm;

namespace METAbolt
{
    public partial class frmTPdialogue : Form
    {
        private METAboltInstance instance;
        private SLNetCom netcom;

        public frmTPdialogue(METAboltInstance instance)
        {
            InitializeComponent();

            this.instance = instance;
            netcom = this.instance.Netcom;

            netcom.TeleportStatusChanged += new EventHandler<TeleportEventArgs>(netcom_TeleportStatusChanged);
        }

        private void netcom_TeleportStatusChanged(object sender, TeleportEventArgs e)
        {
            switch (e.Status)
            {
                case TeleportStatus.Progress:
                    label1.Text = e.Message;

                    break;

                case TeleportStatus.Failed:
                    label1.Text = "Teleport Failed";

                    break;

                case TeleportStatus.Finished:
                    label1.Text = "Teleport Successful";
                    //this.Close();

                    break;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Dispose(); 
        }

        private void frmTPdialogue_FormClosing(object sender, FormClosingEventArgs e)
        {
            netcom.TeleportStatusChanged -= new EventHandler<TeleportEventArgs>(netcom_TeleportStatusChanged);
        }
    }
}
