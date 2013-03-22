//  Copyright (c) 2008-2013, www.metabolt.net
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
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using SLNetworkComm;
using OpenMetaverse;
using PopupControl;

namespace METAbolt
{
    public partial class frmTPhistory : Form
    {
        //string XmlFile = "TP_History.xml";
        private METAboltInstance instance;
        private SLNetCom netcom;
        private GridClient client;

        private Popup toolTip;
        private CustomToolTip customToolTip;

        public frmTPhistory(METAboltInstance instance)
        {
            InitializeComponent();
            this.instance = instance;
            client = this.instance.Client;
            netcom = this.instance.Netcom;

            dataGridView1.DataSource = instance.TP;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

            string msg1 = "To delete a history record, select the whole row by clicking the arrow on the left of the row and hit the DEL button on your keyboard";
            toolTip = new Popup(customToolTip = new CustomToolTip(instance, msg1));
            toolTip.AutoClose = false;
            toolTip.FocusOnOpen = false;
            toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                int cnt = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);

                if (cnt > 0)
                {
                    int ind = dataGridView1.SelectedRows[0].Index; 

                    button2.Enabled = true;
                    button4.Enabled = true;
                    textBox1.Text = dataGridView1.Rows[ind].Cells[1].Value.ToString();
                    textBox2.Text = dataGridView1.Rows[ind].Cells[2].Value.ToString();
                }
                else
                {
                    button2.Enabled = false;
                    button4.Enabled = false;
                    textBox1.Text = string.Empty;
                    textBox2.Text = string.Empty;
                }
            }
            catch
            {
                ; 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();   
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                // Open up the TP form here
                string[] split = textBox2.Text.Split(new Char[] { '/' });
                string sim = split[4].ToString();
                double x = Convert.ToDouble(split[5].ToString());
                double y = Convert.ToDouble(split[6].ToString());
                double z = Convert.ToDouble(split[7].ToString());

                //(new frmTeleport(instance, sim, (float)x, (float)y, (float)z)).ShowDialog();

                netcom.Teleport(sim.Trim(), new Vector3((float)x, (float)y, (float)z));
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string file = textBox1.Text;

            if (file.Length > 32)
            {
                file = file.Substring(0, 32);
            }

            string pos = instance.SIMsittingPos().X.ToString() + ", " + instance.SIMsittingPos().Y.ToString() + ", " + instance.SIMsittingPos().Z.ToString();

            string desc = file + ", " + client.Network.CurrentSim.Name + " (" + pos + ")";

            client.Inventory.RequestCreateItem(client.Inventory.FindFolderForType(AssetType.Landmark),
                    file, desc, AssetType.Landmark, UUID.Random(), InventoryType.Landmark, PermissionMask.All,
                    delegate(bool success, InventoryItem item)
                    {
                        if (!success)
                        {
                            MessageBox.Show("Landmark could not be created", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            MessageBox.Show("The location has been successfully saved as a \nLandmark in your 'Landmarks' folder.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                );  
        }

        private void frmTPhistory_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
        }

        private void picHelp_MouseHover(object sender, EventArgs e)
        {
            toolTip.Show(picHelp);
        }

        private void picHelp_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Close();
        }
    }
}
