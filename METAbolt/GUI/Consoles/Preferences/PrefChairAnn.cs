//  Copyright (c) 2008 - 2014, www.metabolt.net (METAbolt)
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
using System.Globalization;

namespace METAbolt
{
    public partial class PrefChairAnn : UserControl, IPreferencePane
    {
        private METAboltInstance instance;
        private ConfigManager config;
        private Popup toolTip1;
        private CustomToolTip customToolTip;

        public PrefChairAnn(METAboltInstance instance)
        {
            InitializeComponent();

            this.instance = instance;
            config = this.instance.Config;

            string msg2 = "Send messages to the Group UUIDs entered below, blank for no group. You can copy the UUID for a group you belong to from the Group window.";
            toolTip1 = new Popup(customToolTip = new CustomToolTip(instance, msg2));
            toolTip1.AutoClose = false;
            toolTip1.FocusOnOpen = false;
            toolTip1.ShowingAnimation = toolTip1.HidingAnimation = PopupAnimations.Blend;

            textBox1.Text = config.CurrentConfig.ChairAnnouncerUUID.ToString();
            textBox2.Text = config.CurrentConfig.ChairAnnouncerInterval.ToString(CultureInfo.CurrentCulture);

            checkBox1.Enabled = true;
            checkBox1.Checked = config.CurrentConfig.ChairAnnouncerEnabled;
            checkBox2.Checked = config.CurrentConfig.ChairAnnouncerChat;

            textBox3.Text = config.CurrentConfig.ChairAnnouncerGroup1.ToString();
            textBox4.Text = config.CurrentConfig.ChairAnnouncerGroup2.ToString();
            textBox5.Text = config.CurrentConfig.ChairAnnouncerGroup3.ToString();
            textBox6.Text = config.CurrentConfig.ChairAnnouncerGroup4.ToString();
            textBox7.Text = config.CurrentConfig.ChairAnnouncerGroup5.ToString();
            textBox8.Text = config.CurrentConfig.ChairAnnouncerGroup6.ToString();
            //added by GM on 1-APR-2010
            textBox9.Text = config.CurrentConfig.ChairAnnouncerAdvert;
        }

        #region IPreferencePane Members

        string IPreferencePane.Name
        {
            get { return " Chair Announcer"; }
        }

        Image IPreferencePane.Icon
        {
            get { return Properties.Resources.ChairAnn; }
        }

        void IPreferencePane.SetPreferences()
        {
            
            config.CurrentConfig.ChairAnnouncerUUID = UUID.Parse(textBox1.Text);
            config.CurrentConfig.ChairAnnouncerInterval = Convert.ToInt32(textBox2.Text, CultureInfo.CurrentCulture);
            config.CurrentConfig.ChairAnnouncerEnabled = checkBox1.Checked;
            config.CurrentConfig.ChairAnnouncerChat = checkBox2.Checked;
            config.CurrentConfig.ChairAnnouncerGroup1 = UUID.Parse(textBox3.Text);
            config.CurrentConfig.ChairAnnouncerGroup2 = UUID.Parse(textBox4.Text);
            config.CurrentConfig.ChairAnnouncerGroup3 = UUID.Parse(textBox5.Text);
            config.CurrentConfig.ChairAnnouncerGroup4 = UUID.Parse(textBox6.Text);
            config.CurrentConfig.ChairAnnouncerGroup5 = UUID.Parse(textBox7.Text);
            config.CurrentConfig.ChairAnnouncerGroup6 = UUID.Parse(textBox8.Text);
            config.CurrentConfig.ChairAnnouncerAdvert = textBox9.Text;
            
        }

        #endregion

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = textBox2.Enabled = 
            textBox3.Enabled = textBox4.Enabled =
            textBox5.Enabled = textBox6.Enabled =
            textBox7.Enabled = textBox8.Enabled =
            //added by GM on 1-APR-2009
            textBox9.Enabled =
            checkBox2.Enabled = checkBox1.Checked;
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show(pictureBox2);
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Close();
        }
    }
}
