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
using System.Text;
using System.Windows.Forms;
using PopupControl;
using OpenMetaverse;
using System.Diagnostics;
using System.Net;

namespace METAbolt
{
    public partial class Pref3D : System.Windows.Forms.UserControl, IPreferencePane
    {
        private METAboltInstance instance;
        private ConfigManager config;
        private Popup toolTip3;
        private CustomToolTip customToolTip;
        //private GridClient client;
        private bool isloading = true;

        public Pref3D(METAboltInstance instance)
        {
            InitializeComponent();

            string msg4 = "If META3D is not rendering textures and displaying them as WHITE surfaces, then disable mipmaps";
            toolTip3 = new Popup(customToolTip = new CustomToolTip(instance, msg4));
            toolTip3.AutoClose = false;
            toolTip3.FocusOnOpen = false;
            toolTip3.ShowingAnimation = toolTip3.HidingAnimation = PopupAnimations.Blend;

            this.instance = instance;
            //client = this.instance.Client;
            config = this.instance.Config;

            chkAI.Checked = config.CurrentConfig.DisableMipmaps;

            isloading = false;
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
            get { return "  META3D"; }
        }

        Image IPreferencePane.Icon
        {
            get { return Properties.Resources._3d; }
        }

        void IPreferencePane.SetPreferences()
        {
            instance.Config.CurrentConfig.DisableMipmaps = chkAI.Checked;
        }

        #endregion

        private void picAI_MouseHover(object sender, EventArgs e)
        {
            toolTip3.Show(picAI);
        }

        private void picAI_MouseLeave(object sender, EventArgs e)
        {
            toolTip3.Close();
        }

        private void chkAI_CheckedChanged(object sender, EventArgs e)
        {
            if (isloading)
                return; 
        }
    }
}
