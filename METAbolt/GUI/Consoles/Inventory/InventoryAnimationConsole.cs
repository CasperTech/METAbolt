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
using System.Drawing;
using System.Data;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
//using SLNetworkComm;
using OpenMetaverse;

namespace METAbolt
{
    public partial class InventoryAnimationConsole : UserControl
    {
        private METAboltInstance instance;
        //private SLNetCom netcom;
        private GridClient client;
        private InventoryItem item;

        public InventoryAnimationConsole(METAboltInstance instance, InventoryItem item)
        {
            InitializeComponent();

            this.instance = instance;
            //netcom = this.instance.Netcom;
            client = this.instance.Client;
            this.item = item;

            this.Disposed += new EventHandler(InventoryAnimation_Disposed);
        }

        private void InventoryAnimation_Disposed(object sender, EventArgs e)
        {
            
        }

        private void AddClientEvents()
        {
            
        }

        private void btnAnimate_Click(object sender, EventArgs e)
        {
            UUID AnimationID = new UUID(item.AssetUUID.ToString());

            Dictionary<UUID, bool> bAnim = new Dictionary<UUID, bool>();
            bAnim.Add(AnimationID, true);
            client.Self.Animate(bAnim, true);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            UUID AnimationID = new UUID(item.AssetUUID.ToString());

            Dictionary<UUID, bool> bAnim = new Dictionary<UUID, bool>();
            bAnim.Add(AnimationID, false);

            client.Self.Animate(bAnim, true);      
        }

        private void InventoryAnimationConsole_Load(object sender, EventArgs e)
        {

        }
    }
}
