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

namespace METAbolt
{
    public partial class frmPay : Form
    {
        private METAboltInstance instance;
        private GridClient client;
        private UUID target = UUID.Zero;
        //private string name;
        private Primitive Prim = null;

        public frmPay(METAboltInstance instance, UUID target, string name)
        {
            InitializeComponent();

            this.instance = instance;
            client = this.instance.Client;

            this.target = target;
            //this.name = txtPerson.Text = name;
        }

        public frmPay(METAboltInstance instance, UUID target, string name, int sprice)
        {
            InitializeComponent();

            this.instance = instance;
            client = this.instance.Client;

            this.target = target;
            //this.name = txtPerson.Text = name;
            this.nudAmount.Value = (decimal)sprice;
        }

        public frmPay(METAboltInstance instance, UUID target, string name, int sprice, Primitive prim)
        {
            InitializeComponent();

            this.instance = instance;
            client = this.instance.Client;

            this.target = target;
            //this.name = txtPerson.Text = name;
            this.nudAmount.Value = (decimal)sprice;
            this.Prim = prim;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            int iprice = (int)nudAmount.Value;

            if (Prim != null)
            {
                SaleType styp = Prim.Properties.SaleType;
                UUID folderid = client.Inventory.FindFolderForType(AssetType.Object);   // instance.Config.CurrentConfig.ObjectsFolder;

                client.Objects.BuyObject(client.Network.CurrentSim, Prim.LocalID, styp, iprice, UUID.Zero, folderid);            
            }
            else
            {
                // TODO: UUID.Zero could be Governor Linden...I haven't checked.
                // This needs to be checked and removed when we introduce
                // purchase of Land and other payments to Linden etc.
                if (target != UUID.Zero)
                {
                    if (!string.IsNullOrEmpty(textBox1.Text) && textBox1.Text.Trim().Length > 1)
                    {
                        client.Self.GiveAvatarMoney(target, iprice);
                    }
                    else
                    {
                        client.Self.GiveAvatarMoney(target, iprice, textBox1.Text.Trim());
                    }
                }
            }

            this.Close();
        }

        private void frmPay_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
        }
    }
}