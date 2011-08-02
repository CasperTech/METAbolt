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
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
using OpenMetaverse.Packets;
using SLNetworkComm;
using System.Globalization;


namespace METAbolt
{
    public partial class frmDialog : Form
    {
        private METAboltInstance instance;
        //private SLNetCom netcom;
        private GridClient client;
        private ScriptDialogEventArgs ed;


        public frmDialog(METAboltInstance instance, ScriptDialogEventArgs e)
        {
            InitializeComponent();

            this.instance = instance;
            client = this.instance.Client;
            this.ed = e;

            timer1.Interval = instance.DialogTimeOut;
            timer1.Enabled = true;
            timer1.Start();  
        }

        private void Dialog_Load(object sender, EventArgs e)
        {
            this.CenterToParent();

            lblTitle.Text = ed.FirstName + "'s " + ed.ObjectName;
            string smsg = ed.Message;
            //txtMessage.Text = smsg;

            char[] deli = "\n".ToCharArray();
            string[] sGrp = smsg.Split(deli);
            txtMessage.Lines = sGrp;
            //label2.Text = smsg;  

            List<string> btns = ed.ButtonLabels;

            int count = btns.Count;

            for (int i = 0; i < count; i++)
            {
                cboReply.Items.Add(i.ToString(CultureInfo.CurrentCulture) + "-" + btns[i]);     
            }

            if (this.instance.DialogCount == 7)
            {
                button3.Visible = true;
                this.BackColor = Color.Red;  
            }
            else
            {
                button3.Visible = false;
                this.BackColor = Color.White;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            CleanUp();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cboReply.Text != "Select...")
            {
                char[] deli = "-".ToCharArray();
                string[] sGrp = cboReply.Text.Split(deli);

                int butindex = cboReply.SelectedIndex;   //(int)sGrp[2];
                string butlabel = sGrp[1];

                client.Self.ReplyToScriptDialog(ed.Channel, butindex, butlabel, ed.ObjectID);

                CleanUp();
            }
            else
            {
                MessageBox.Show("\n Select a 'Reply Option' from the dropdown box \n","METAbolt",MessageBoxButtons.OK);   
            }
        }

        private void CleanUp()
        {
            this.instance.DialogCount -= 1;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.instance.DialogCount = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (instance.IsAvatarMuted(ed.ObjectID))
                return;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            button2.PerformClick();  
        }
    }
}
