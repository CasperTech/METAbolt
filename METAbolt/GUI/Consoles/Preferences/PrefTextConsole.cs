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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using PopupControl;
using System.Diagnostics;

namespace METAbolt
{
    public partial class PrefTextConsole : UserControl, IPreferencePane
    {
        private METAboltInstance instance;
        private ConfigManager config;
        private Popup toolTip;
        private CustomToolTip customToolTip;
        private bool nudchanged = false;

        public PrefTextConsole(METAboltInstance instance)
        {
            InitializeComponent();

            this.instance = instance;
            config = this.instance.Config;

            string msg1 = "Use this setting to limit the amount (lines) of text stored on your chat screen. Especially in busy areas we recommend using this feature so that your machine does not run out of memory. The recommended setting is 250.";
            toolTip = new Popup(customToolTip = new CustomToolTip(instance, msg1));
            toolTip.AutoClose = false;
            toolTip.FocusOnOpen = false;
            toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;

            chkChatTimestamps.Checked = config.CurrentConfig.ChatTimestamps;
            chkIMTimestamps.Checked = config.CurrentConfig.IMTimestamps;
            chkSmileys.Checked = config.CurrentConfig.ChatSmileys;
            nud.Value = config.CurrentConfig.lineMax;
            chkIMs.Checked = config.CurrentConfig.SaveIMs;
            chkChat.Checked = config.CurrentConfig.SaveChat;
            txtDir.Text = config.CurrentConfig.LogDir;
            chkGroupNotices.Checked = config.CurrentConfig.DisableGroupNotices;
            chkGIMs.Checked = config.CurrentConfig.DisableGroupIMs;    

            //if (config.CurrentConfig.BusyReply != string.Empty && config.CurrentConfig.BusyReply != null)
            //{
            textBox1.Text = config.CurrentConfig.BusyReply;
            //}

            // Initial IM feature thx to Elmo Clarity 20/12/2010
            textBox2.Text = config.CurrentConfig.InitialIMReply;
            
            chkSLT.Checked = config.CurrentConfig.UseSLT;
            chkSound.Checked = config.CurrentConfig.PlaySound; 
        }

        #region IPreferencePane Members

        string IPreferencePane.Name
        {
            get { return "Text"; }
        }

        Image IPreferencePane.Icon
        {
            get { return Properties.Resources.documents_32; }
        }

        void IPreferencePane.SetPreferences()
        {
            config.CurrentConfig.ChatTimestamps = chkChatTimestamps.Checked;
            config.CurrentConfig.IMTimestamps = chkIMTimestamps.Checked;
            config.CurrentConfig.ChatSmileys = chkSmileys.Checked;
            config.CurrentConfig.lineMax = Convert.ToInt32(nud.Value);
            config.CurrentConfig.UseSLT = chkSLT.Checked;
            config.CurrentConfig.PlaySound = chkSound.Checked;
            config.CurrentConfig.BusyReply = textBox1.Text;
            config.CurrentConfig.InitialIMReply = textBox2.Text;
            config.CurrentConfig.SaveIMs = chkIMs.Checked;
            config.CurrentConfig.SaveChat = chkChat.Checked;
            config.CurrentConfig.LogDir = txtDir.Text;
            config.CurrentConfig.DisableGroupIMs = chkGIMs.Checked;
            config.CurrentConfig.DisableGroupNotices = chkGroupNotices.Checked;  
        }

        #endregion

        private void chkIMTimestamps_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void PrefTextConsole_Load(object sender, EventArgs e)
        {

        }

        private void chkSmileys_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void chkSmileys_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            toolTip.Show(pictureBox1);
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void chkSLT_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.folderBrowser.SelectedPath = txtDir.Text;
   
            DialogResult result = this.folderBrowser.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtDir.Text = this.folderBrowser.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", txtDir.Text);
        }

        private void chkGIMs_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void nud_ValueChanged(object sender, EventArgs e)
        {
            if (nudchanged)
            {
                instance.Config.CurrentConfig.BufferApplied = true;
            }

            nudchanged = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
