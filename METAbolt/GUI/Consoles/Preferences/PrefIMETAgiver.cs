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
using PopupControl;
using OpenMetaverse;
using System.Diagnostics;
using System.Net;

namespace METAbolt
{
    public partial class PrefMETAgiver : System.Windows.Forms.UserControl, IPreferencePane
    {
        private METAboltInstance instance;
        private ConfigManager config;
        private Popup toolTip;
        private CustomToolTip customToolTip;
        private GridClient client;
        //private bool isloading = true;

        public PrefMETAgiver(METAboltInstance instance)
        {
            InitializeComponent();

            string msg = "To delete an entry, select the whole row by clicking the arrow on the left of the row and hit the DEL button on your keyboard";
            toolTip = new Popup(customToolTip = new CustomToolTip(instance, msg));
            toolTip.AutoClose = false;
            toolTip.FocusOnOpen = false;
            toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;

            this.instance = instance;
            client = this.instance.Client;
            config = this.instance.Config;

            GW.DataSource = instance.GiverItems;

            //isloading = false;
        }

        private void picAI_Click(object sender, EventArgs e)
        {

        }

        private void PrefAI_Load(object sender, EventArgs e)
        {

        }

        #region IPreferencePane Members

        string IPreferencePane.Name
        {
            get { return "  METAcourier"; }
        }

        Image IPreferencePane.Icon
        {
            get { return Properties.Resources.top_hat; }
        }

        void IPreferencePane.SetPreferences()
        {
            //instance.Config.CurrentConfig.DisableMipmaps = chkAI.Checked;
        }

        #endregion

        private void picHelp_MouseHover(object sender, EventArgs e)
        {
            toolTip.Show(picHelp);
        }

        private void picHelp_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Close();
        }

        private void textBox2_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode node = e.Data.GetData(typeof(TreeNode)) as TreeNode;

            if (node == null) return;

            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                InventoryBase io = (InventoryBase)node.Tag;

                if (node.Tag is InventoryFolder)
                {
                    // Folder are not supported
                    MessageBox.Show("Folders are not supported. You can only enter inventory items.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    InventoryItem item = (InventoryItem)io;

                    if ((item.Permissions.OwnerMask & PermissionMask.Copy) != PermissionMask.Copy)
                    {
                        DialogResult res = MessageBox.Show("This is a 'no copy' item and you will lose ownership if you continue.", "Warning", MessageBoxButtons.OKCancel);

                        if (res == DialogResult.Cancel) return;
                    }

                    if (string.IsNullOrEmpty(textBox1.Text))
                    {
                        MessageBox.Show("Command cannot be empty. Enter a UNIQUE command first.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    if (instance.GiverItems.Rows.Contains(textBox1.Text))
                    {
                        MessageBox.Show(textBox1.Text + " command is already in your list of items.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    InventoryItem iitem = (InventoryItem)io;

                    DataRow dr = instance.GiverItems.NewRow();
                    dr["Command"] = textBox1.Text.Trim();
                    dr["UUID"] = iitem.UUID;
                    dr["Name"] = iitem.Name;
                    dr["AssetType"] = iitem.AssetType;

                    instance.GiverItems.Rows.Add(dr);

                    textBox1.Text = string.Empty;   

                    GW.Refresh();
                }
            }
        }

        private void textBox2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBox2_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
    }
}
