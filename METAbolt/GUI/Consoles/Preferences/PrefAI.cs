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
    public partial class PrefAI : System.Windows.Forms.UserControl, IPreferencePane
    {
        private METAboltInstance instance;
        private ConfigManager config;
        private Popup toolTip3;
        private CustomToolTip customToolTip;
        //private GridClient client;
        private bool isloading = true;

        public PrefAI(METAboltInstance instance)
        {
            InitializeComponent();

            string msg4 = "Enable this option for your avatar to enter intelligent conversations (automated) with anyone that IMs it. Note that this feature only works via IM and not chat";
            toolTip3 = new Popup(customToolTip = new CustomToolTip(instance, msg4));
            toolTip3.AutoClose = false;
            toolTip3.FocusOnOpen = false;
            toolTip3.ShowingAnimation = toolTip3.HidingAnimation = PopupAnimations.Blend;

            this.instance = instance;
            //client = this.instance.Client;
            config = this.instance.Config;

            chkAI.Checked = config.CurrentConfig.AIon;
            chkReply.Checked = config.CurrentConfig.ReplyAI;
            textBox1.Text = config.CurrentConfig.ReplyText;
            checkBox2.Checked = false;   // config.CurrentConfig.MultiLingualAI;

            panel1.Enabled = groupBox1.Enabled = checkBox2.Enabled = chkAI.Checked;

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
            get { return " AI"; }
        }

        Image IPreferencePane.Icon
        {
            get { return Properties.Resources.AI; }
        }

        void IPreferencePane.SetPreferences()
        {
            instance.Config.CurrentConfig.AIon = chkAI.Checked;
            instance.Config.CurrentConfig.ReplyAI = chkReply.Checked;
            instance.Config.CurrentConfig.ReplyText = textBox1.Text;
            instance.Config.CurrentConfig.MultiLingualAI = false;  // checkBox2.Checked;
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

            panel1.Enabled = groupBox1.Enabled = checkBox2.Enabled = chkAI.Checked;   

            if (chkAI.Checked)
            {
                // Check to see if AI libraries have been installed
                string aimlDirectory = Application.StartupPath.ToString();
                aimlDirectory += "\\aiml\\";

                bool direxists = DirExists(aimlDirectory);

                if (!direxists)
                {
                    DialogResult resp = MessageBox.Show("You must first install the AI libraries. Do you want to download them now?", "METAbrain", MessageBoxButtons.YesNo);

                    if (resp == DialogResult.Yes)
                    {
                        try
                        {
                            // Open Windows Explorer
                            Process.Start("explorer.exe", Application.StartupPath.ToString());

                            // download the libraries
                            WebClient webClient = new WebClient();
                            webClient.DownloadFile("http://www.metabolt.net/dwl/METAbrain.zip", Application.StartupPath.ToString() + "\\METAbrain.zip");

                            FormFlash.Flash(instance.MainForm);

                            MessageBox.Show("Download complete. \n\nLook for 'METAbrain.zip' \n\nand unzip the contents into the folder \nthat's now visible in Windows Explorer.", "METAbrain");

                            webClient.Dispose(); 
                        }
                        catch (Exception ex)
                        {
                            string exp = ex.Message;
                            MessageBox.Show("There has been an error downloading the library file: \n\n" + exp + "\n\nDownload manually from here:\nhttp://www.metabolt.net/dwl/METAbrain.zip", "METAbrain");
                        }
                    }
                    else
                    {
                        chkAI.Checked = false;
                        return;
                    }
                }
            }
        }

        private static bool DirExists(string sDirName)
        {
            try
            {
                return (System.IO.Directory.Exists(sDirName));    //Check for file
            }
            catch (Exception)
            {
                return (false);                                 //Exception occured, return False
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string dir = Application.StartupPath.ToString() + "\\config\\Settings.xml";

            if (System.IO.File.Exists(dir))
            {
                Process.Start("notepad.exe", dir); 
            }
            else
            {
                MessageBox.Show("File: \n" + dir + "\n\n could not be found", "METAbolt");  
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string dir = Application.StartupPath.ToString() + "\\config";

            Process.Start("explorer.exe", dir);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string dir = Application.StartupPath.ToString() + "\\aiml"; ;

            if (System.IO.Directory.Exists(dir))
            {
                Process.Start("explorer.exe", dir);
            }
            else
            {
                MessageBox.Show("AIML libraries could not be found!\nAre you sure they are installed?","METAbolt");  
            }
        }
    }
}
