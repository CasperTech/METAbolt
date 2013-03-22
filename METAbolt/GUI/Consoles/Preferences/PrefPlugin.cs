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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using METAxCommon;

namespace METAbolt
{
    public partial class PrefPlugin : UserControl, IPreferencePane
    {
        private METAboltInstance instance;
        private ConfigManager config;
        private bool pluginschanged = false;
        private string plugins = string.Empty;  

        public PrefPlugin(METAboltInstance instance)
        {
            InitializeComponent();

            this.instance = instance;
            config = this.instance.Config;

            if (this.instance.EList != null)
            {
                foreach (IExtension extOn in this.instance.EList)
                {
                    listBox1.Items.Add(extOn.Title);
                }
            }

            plugins = config.CurrentConfig.PluginsToLoad;

            if (!string.IsNullOrEmpty(plugins))
            {
                string[] lplugs = plugins.Split('|');

                foreach (string plug in lplugs)
                {
                    if (!string.IsNullOrEmpty(plug))
                    {
                        listBox2.Items.Add(plug);
                    }
                }
            }
        }

        #region IPreferencePane Members

        string IPreferencePane.Name
        {
            get { return " Plugins"; }
        }

        Image IPreferencePane.Icon
        {
            get { return Properties.Resources.plugin_icon; }
        }

        private void PrefPlugin_Load(object sender, EventArgs e)
        {

        }

        void IPreferencePane.SetPreferences()
        {
            plugins = string.Empty;
 
            foreach (string item in listBox2.Items)
            {
                plugins += item  + "|";
            }

            if (pluginschanged)
            {
                if (plugins.EndsWith("|"))
                {
                    plugins = plugins.Substring(0, plugins.Length - 1);    
                }

                config.CurrentConfig.PluginsToLoad = plugins;
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select a plugin to add first");
                return;
            }

            listBox2.Items.Add(listBox1.SelectedItem);
            pluginschanged = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select a plugin to remove first");
                return;
            }

            listBox2.Items.Remove(listBox2.SelectedItem);
            pluginschanged = true;
        }

        
    }
}
