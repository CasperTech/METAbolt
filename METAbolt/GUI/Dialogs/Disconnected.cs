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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
using SLNetworkComm;

namespace METAbolt
{
    public partial class frmDisconnected : Form
    {
        private METAboltInstance instance;
        private string rea = string.Empty;  

        public frmDisconnected(METAboltInstance instance, string reason)
        {
            InitializeComponent();

            this.instance = instance;

            rea = reason;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            instance.MainForm.Close();
        }

        private void frmDisconnected_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void frmDisconnected_Load(object sender, EventArgs e)
        {
            lblMessage.Text = rea;

            if (instance.State.UnReadIMs > 0)
            {
                label2.Visible = true;
                label2.Text = "You have " + instance.State.UnReadIMs.ToString() + " unread IMs";
                button1.Visible = true;
            }
            else
            {
                label2.Visible = false;
                button1.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            instance.ReadIMs = true; 
            this.Close(); 
        }
    }
}