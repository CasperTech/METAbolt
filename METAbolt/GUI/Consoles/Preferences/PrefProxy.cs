//  Copyright (c) 2008 - 2010, www.metabolt.net
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

namespace METAbolt
{
    public partial class PrefProxy : UserControl, IPreferencePane
    {
        private METAboltInstance instance;
        private ConfigManager config;

        public PrefProxy(METAboltInstance instance)
        {
            InitializeComponent();

            this.instance = instance;
            config = this.instance.Config;

            checkBox1.Checked = config.CurrentConfig.UseProxy;
            textBox1.Text = config.CurrentConfig.ProxyURL;
            textBox4.Text = config.CurrentConfig.ProxyPort;
            textBox2.Text = config.CurrentConfig.ProxyUser;
            textBox3.Text = config.CurrentConfig.ProxyPWD; 
        }

        #region IPreferencePane Members

        string IPreferencePane.Name
        {
            get { return " Proxy"; }
        }

        Image IPreferencePane.Icon
        {
            get { return Properties.Resources.proxy; }
        }

        void IPreferencePane.SetPreferences()
        {
            config.CurrentConfig.UseProxy = checkBox1.Checked;
            config.CurrentConfig.ProxyURL = textBox1.Text;
            config.CurrentConfig.ProxyPort = textBox4.Text;
            config.CurrentConfig.ProxyUser = textBox2.Text;
            config.CurrentConfig.ProxyPWD = textBox3.Text;
        }

        #endregion

        private void PrefParcelMusic_Load(object sender, EventArgs e)
        {

        }

        private void chkParcelMusic_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Enabled = checkBox1.Checked; 
        }
    }
}
