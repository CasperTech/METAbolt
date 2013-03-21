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
using PopupControl;
using OpenMetaverse;

namespace METAbolt
{
    public partial class frmMutes : Form
    {
        //private Popup toolTip;
        //private CustomToolTip customToolTip;

        private METAboltInstance instance;

        public frmMutes(METAboltInstance instance)
        {
            InitializeComponent();

            this.instance = instance;

            //GW.DataSource = instance.MuteList;

            //string msg1 = "To un-mute, select the whole row by clicking the arrow on the left of the row and hit the DEL button on your keyboard";
            //toolTip = new Popup(customToolTip = new CustomToolTip(instance, msg1));
            //toolTip.AutoClose = false;
            //toolTip.FocusOnOpen = false;
            //toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;

            instance.Client.Self.MuteListUpdated += new EventHandler<EventArgs>(MuteListUpdated);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //instance.MuteList = GW.DataSource;
            this.Close();
        }

        private void frmMutes_Load(object sender, EventArgs e)
        {
            this.CenterToParent();

            lvMutes.Columns[0].Width = lvMutes.Width - 25;
            LoadMutes();
        }

        private void LoadMutes()
        {
            lvMutes.BeginUpdate();
            lvMutes.Items.Clear();

            int cnt = 0;

            instance.Client.Self.MuteList.ForEach((MuteEntry entry) =>
            {
                string mutetype = string.Empty;

                ListViewItem item = new ListViewItem(entry.Name);
                item.Tag = entry;

                if (cnt == 1)
                {
                    item.BackColor = Color.GhostWhite; 
                    cnt = 0;
                }

                lvMutes.Items.Add(item);

                cnt = 1;
            }
            );

            lvMutes.EndUpdate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MuteEntry entry = (MuteEntry)lvMutes.SelectedItems[0].Tag;

            instance.Client.Self.RemoveMuteListEntry(entry.ID, entry.Name);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadMutes();
            instance.Client.Self.RequestMuteList();
        }

        private void lvMutes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvMutes.SelectedItems.Count == 0)
            {
                button2.Enabled = false;
            }
            else
            {
                button2.Enabled = true;
            }
        }

        private void frmMutes_FormClosing(object sender, FormClosingEventArgs e)
        {
            instance.Client.Self.MuteListUpdated += new EventHandler<EventArgs>(MuteListUpdated);
        }

        private void MuteListUpdated(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                if (IsHandleCreated)
                {
                    BeginInvoke(new MethodInvoker(() => MuteListUpdated(sender, e)));
                }
                return;
            }

            LoadMutes();
        }
    }
}
