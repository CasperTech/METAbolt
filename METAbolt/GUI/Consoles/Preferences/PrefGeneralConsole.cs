//  Copyright (c) 2008 - 2013, www.metabolt.net (METAbolt)
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
using OpenMetaverse;
using System.Diagnostics;
using System.Net;
using System.IO; 
//using System.Web;
//using System.Web.UI;
//using System.IO;
using System.Globalization;


namespace METAbolt
{
    public partial class PrefGeneralConsole : System.Windows.Forms.UserControl, IPreferencePane
    {
        private METAboltInstance instance;
        private ConfigManager config;
        private Popup toolTip;
        //private Popup toolTip1;
        private Popup toolTip2;
        
        private Popup toolTip4;
        private Popup toolTip5;
        private Popup toolTip6;
        private Popup toolTip7;
        private Popup toolTip8;
        private Popup toolTip9;
        private CustomToolTip customToolTip;
        private GridClient client;
        private bool loadingtimer = true;
        private bool loadingtimer1 = true;
        private string headerfont = "Tahoma";
        private string headerfontstyle = "Regular";
        private float headerfontsize = 8.5f;
        private string textfont = "Tahoma";
        private string textfontstyle = "Regular";
        private float textfontsize = 8.5f;
        private bool loading = true;
        //private bool restart = false;

        public PrefGeneralConsole(METAboltInstance instance)
        {
            InitializeComponent();

            string msg1 = "Disables toolbar nofications that popup from the bottom right hand corner of your screen. Be warned that important system information will not be displayed if this is disabled.";
            toolTip = new Popup(customToolTip = new CustomToolTip(instance, msg1));
            toolTip.AutoClose = false;
            toolTip.FocusOnOpen = false;
            toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;

            string msg3 = "If you will be crossing SIMs or will be near SIM borders and want to see the avatars in the next SIM listed on your radar then you must enable this option. This option requires a restart.";
            toolTip2 = new Popup(customToolTip = new CustomToolTip(instance, msg3));
            toolTip2.AutoClose = false;
            toolTip2.FocusOnOpen = false;
            toolTip2.ShowingAnimation = toolTip2.HidingAnimation = PopupAnimations.Blend;

            string msg5 = "Approximately 1 minute after login the avatar will autosit on an object that has its UUID in the object description. The object needs to be within 10 metre radius of the avatar.";
            toolTip4 = new Popup(customToolTip = new CustomToolTip(instance, msg5));
            toolTip4.AutoClose = false;
            toolTip4.FocusOnOpen = false;
            toolTip4.ShowingAnimation = toolTip4.HidingAnimation = PopupAnimations.Blend;

            string msg6 = "Sets radar range (in metres) throughout the application for objects & avatars. Lower setting means using less bandwidth. Default is 64m. If set to 10, avatars & objects outside the 10m range will be ignored.";
            toolTip5 = new Popup(customToolTip = new CustomToolTip(instance, msg6));
            toolTip5.AutoClose = false;
            toolTip5.FocusOnOpen = false;
            toolTip5.ShowingAnimation = toolTip5.HidingAnimation = PopupAnimations.Blend;

            string msg7 = "Sets the default object range used on the 'Object Manager' screen. The maximum value of this setting can not be greater than the selected 'radar range' value above.";
            toolTip6 = new Popup(customToolTip = new CustomToolTip(instance, msg7));
            toolTip6.AutoClose = false;
            toolTip6.FocusOnOpen = false;
            toolTip6.ShowingAnimation = toolTip6.HidingAnimation = PopupAnimations.Blend;

            string msg8 = "Group Inviter users need to use the generated password or type your own in. Enter the same password into the Group Inviter in SL. Use 'reset' button to generate new one. For your security THIS FIELD MUST NOT BE BLANK.";
            toolTip7 = new Popup(customToolTip = new CustomToolTip(instance, msg8));
            toolTip7.AutoClose = false;
            toolTip7.FocusOnOpen = false;
            toolTip7.ShowingAnimation = toolTip7.HidingAnimation = PopupAnimations.Blend;

            string msg9 = "For this to work you need to create a folder called 'GroupMan Items' under the root of your inventory and place your give away items in it.";
            toolTip8 = new Popup(customToolTip = new CustomToolTip(instance, msg9));
            toolTip8.AutoClose = false;
            toolTip8.FocusOnOpen = false;
            toolTip8.ShowingAnimation = toolTip8.HidingAnimation = PopupAnimations.Blend;

            string msg10 = "If unchecked and a master avatar and/or object UUID is not specified, LSL commands from all avatars and objects (with MD5'ed METAbolt passwords in the command) will be accepted and processed.";
            toolTip9 = new Popup(customToolTip = new CustomToolTip(instance, msg10));
            toolTip9.AutoClose = false;
            toolTip9.FocusOnOpen = false;
            toolTip9.ShowingAnimation = toolTip9.HidingAnimation = PopupAnimations.Blend;
  
            this.instance = instance;
            client = this.instance.Client;
            config = this.instance.Config;

            if (config.CurrentConfig.InterfaceStyle == 0)
                rdoSystemStyle.Checked = true;
            else if (config.CurrentConfig.InterfaceStyle == 1)
                rdoOfficeStyle.Checked = true;

            //chkRadar.Checked = config.CurrentConfig.iRadar;
            chkConnect4.Checked = config.CurrentConfig.Connect4;    
            chkNotifications.Checked = config.CurrentConfig.DisableNotifications;
            chkFriends.Checked = config.CurrentConfig.DisableFriendsNotifications; 
            chkAutoSit.Checked = config.CurrentConfig.AutoSit;

            try
            {
                tBar1.Value = config.CurrentConfig.RadarRange;
            }
            catch
            {
                tBar1.Value = tBar1.Maximum;
                MessageBox.Show("Your radar setting was greater than the maximum allowed.\nIt has been changed to " + tBar1.Maximum.ToString(), "METAbolt"); 
            }

            textBox1.Text = tBar1.Value.ToString(CultureInfo.CurrentCulture);

            tbar2.Maximum = tBar1.Value;
            tbar2.Value = config.CurrentConfig.ObjectRange;

            textBox2.Text = tbar2.Value.ToString(CultureInfo.CurrentCulture);
            textBox3.Text = config.CurrentConfig.GroupManPro;
            chkHide.Checked = config.CurrentConfig.HideMeta;
            chkInvites.Checked = config.CurrentConfig.DisableInboundGroupInvites;
            chkLookAt.Checked = config.CurrentConfig.DisableLookAt;
            checkBox1.Checked = config.CurrentConfig.GivePresent;
            checkBox2.Checked = config.CurrentConfig.AutoRestart;
            nUD1.Value = config.CurrentConfig.LogOffTime;
            nUD2.Value = config.CurrentConfig.ReStartTime;
            textBox4.Text = client.Settings.ASSET_CACHE_DIR;
            checkBox13.Checked = config.CurrentConfig.HideDisconnectPrompt;
            chkDisableRadar.Checked = config.CurrentConfig.DisableRadar;
            chkRestrictRadar.Checked = config.CurrentConfig.RestrictRadar;
            chkVoice.Checked = config.CurrentConfig.DisableVoice;
            chkFavs.Checked = config.CurrentConfig.DisableFavs;
            cbHHTPInv.Checked = config.CurrentConfig.DisableHTTPinv; 

            if (config.CurrentConfig.BandwidthThrottle > 500.0f)
            {
                config.CurrentConfig.BandwidthThrottle = 500.0f;
            }

            if (config.CurrentConfig.BandwidthThrottle < 500.0f)
            {
                radioButton2.Checked = true;
                trackBar1.Enabled = true;

                trackBar1.Value = Convert.ToInt32(config.CurrentConfig.BandwidthThrottle);
                label19.Text = trackBar1.Value.ToString();
            }
            else
            {
                radioButton1.Checked = true;
                trackBar1.Enabled = false;

                trackBar1.Value = 500;
                label19.Text = "500";
            }

            SetBarValue();

            comboBox1.SelectedIndex = 0; 

            if (config.CurrentConfig.ClassicChatLayout)
            {
                checkBox4.Checked = false;
            }
            else
            {
                checkBox4.Checked = true;
            }

            textBox7.BackColor = textBox6.BackColor = config.CurrentConfig.HeaderBackColour;
            //textBox9.BackColor = config.CurrentConfig.BgColour;

            if (config.CurrentConfig.HeaderFont != null)
            {
                headerfont = config.CurrentConfig.HeaderFont;
                headerfontstyle = config.CurrentConfig.HeaderFontStyle;
                headerfontsize = config.CurrentConfig.HeaderFontSize;

                FontStyle fontsy;

                switch (headerfontstyle.ToLower(CultureInfo.CurrentCulture))
                {
                    case "bold":
                        fontsy = FontStyle.Bold;
                        break;
                    case "italic":
                        fontsy = FontStyle.Italic;
                        break;
                    default:
                        fontsy = FontStyle.Regular;
                        break;
                }

                textBox7.Font = new Font(headerfont, headerfontsize, fontsy);
                textBox7.Text = "size " + headerfontsize.ToString(CultureInfo.CurrentCulture);
            }

            if (config.CurrentConfig.TextFont != null)
            {
                textfont = config.CurrentConfig.TextFont;
                textfontstyle = config.CurrentConfig.TextFontStyle;
                textfontsize = config.CurrentConfig.TextFontSize;

                FontStyle fontst;

                switch (textfontstyle.ToLower(CultureInfo.CurrentCulture))
                {
                    case "bold":
                        fontst = FontStyle.Bold;
                        break;
                    case "italic":
                        fontst = FontStyle.Italic;
                        break;
                    default:
                        fontst = FontStyle.Regular;
                        break;
                }

                textBox8.Font = new Font(textfont, textfontsize, fontst);
                textBox8.Text = "size " + textfontsize.ToString(CultureInfo.CurrentCulture);
            }

            checkBox6.Checked = config.CurrentConfig.PlayFriendOnline;
            checkBox7.Checked = config.CurrentConfig.PlayFriendOffline;
            checkBox8.Checked = config.CurrentConfig.PlayIMreceived;
            checkBox9.Checked = config.CurrentConfig.PlayGroupIMreceived;
            checkBox10.Checked = config.CurrentConfig.PlayGroupNoticeReceived;
            checkBox11.Checked = config.CurrentConfig.PlayInventoryItemReceived;
            checkBox5.Checked = config.CurrentConfig.PlayPaymentReceived;
            chkMinimised.Checked = config.CurrentConfig.StartMinimised;
            txtAdRemove.Text = config.CurrentConfig.AdRemove.Trim();
            txtMavatar.Text = config.CurrentConfig.MasterAvatar.Trim();
            txtMObject.Text = config.CurrentConfig.MasterObject.Trim();
            chkAutoTransfer.Checked = config.CurrentConfig.AutoTransfer;
            chkTray.Checked = config.CurrentConfig.DisableTrayIcon;
            chkTyping.Checked = config.CurrentConfig.DisableTyping;
            chkAutoFriend.Checked = config.CurrentConfig.AutoAcceptFriends;
            checkBox12.Checked = config.CurrentConfig.EnforceLSLsecurity;
            chkLSL.Checked = config.CurrentConfig.DisplayLSLcommands;
            //cbTag.Checked = config.CurrentConfig.BroadcastID;          

            if (config.CurrentConfig.DeclineInv)
            {
                comboBox1.SelectedIndex = 1; 
            }

            if (config.CurrentConfig.AutoAcceptItems)
            {
                comboBox1.SelectedIndex = 2;
            }
        }

        #region IPreferencePane Members

        string IPreferencePane.Name
        {
            get { return "General"; }
        }

        Image IPreferencePane.Icon
        {
            get { return Properties.Resources.applications_32; }
        }

        void IPreferencePane.SetPreferences()
        {
            if (rdoSystemStyle.Checked)
                config.CurrentConfig.InterfaceStyle = 0;
            else if (rdoOfficeStyle.Checked)
                config.CurrentConfig.InterfaceStyle = 1;

            //instance.Config.CurrentConfig.iRadar = chkRadar.Checked;
            instance.Config.CurrentConfig.Connect4 = chkConnect4.Checked;
            instance.Config.CurrentConfig.DisableNotifications = chkNotifications.Checked;
            config.CurrentConfig.DisableFriendsNotifications = chkFriends.Checked;  
            instance.Config.CurrentConfig.AutoSit = chkAutoSit.Checked;
            instance.Config.CurrentConfig.RadarRange = tBar1.Value;
            instance.Config.CurrentConfig.ObjectRange = tbar2.Value;
            instance.Config.CurrentConfig.GroupManPro = textBox3.Text.Trim();
            instance.Config.CurrentConfig.HideMeta = chkHide.Checked;
            instance.Config.CurrentConfig.DisableInboundGroupInvites = chkInvites.Checked;
            instance.Config.CurrentConfig.DisableLookAt = chkLookAt.Checked;
            instance.Config.CurrentConfig.GivePresent = checkBox1.Checked;
            instance.Config.CurrentConfig.AutoRestart = checkBox2.Checked;
            instance.Config.CurrentConfig.LogOffTime = Convert.ToInt32(nUD1.Value);
            instance.Config.CurrentConfig.ReStartTime = Convert.ToInt32(nUD2.Value);
            instance.Config.CurrentConfig.BandwidthThrottle = Convert.ToSingle(trackBar1.Value);
            instance.Config.CurrentConfig.HideDisconnectPrompt = checkBox13.Checked;
            instance.Config.CurrentConfig.DisableRadar = chkDisableRadar.Checked;
            instance.Config.CurrentConfig.RestrictRadar = chkRestrictRadar.Checked;
            instance.Config.CurrentConfig.DisableVoice = chkVoice.Checked;  
            instance.Config.CurrentConfig.DisableFavs = chkFavs.Checked;
            instance.Config.CurrentConfig.DisableHTTPinv = cbHHTPInv.Checked;

            if (checkBox4.Checked)
            {
                instance.Config.CurrentConfig.ClassicChatLayout = false;
            }
            else
            {
                instance.Config.CurrentConfig.ClassicChatLayout = true;
            }

            client.Self.Movement.Camera.Far = (float)tBar1.Value;

            instance.Config.CurrentConfig.HeaderFont = headerfont;
            instance.Config.CurrentConfig.HeaderFontStyle = headerfontstyle;
            instance.Config.CurrentConfig.HeaderFontSize = headerfontsize;
            instance.Config.CurrentConfig.HeaderBackColour = textBox6.BackColor;
            //instance.Config.CurrentConfig.BgColour = textBox9.BackColor;
            instance.Config.CurrentConfig.TextFont = textfont;
            instance.Config.CurrentConfig.TextFontStyle = textfontstyle;
            instance.Config.CurrentConfig.TextFontSize = textfontsize;

            client.Self.Movement.Camera.Far = tBar1.Value;

            config.CurrentConfig.PlayFriendOnline = checkBox6.Checked;
            config.CurrentConfig.PlayFriendOffline = checkBox7.Checked;
            config.CurrentConfig.PlayIMreceived = checkBox8.Checked;
            config.CurrentConfig.PlayGroupIMreceived = checkBox9.Checked;
            config.CurrentConfig.PlayGroupNoticeReceived = checkBox10.Checked;
            config.CurrentConfig.PlayInventoryItemReceived = checkBox11.Checked;
            config.CurrentConfig.PlayPaymentReceived = checkBox5.Checked;
            config.CurrentConfig.StartMinimised = chkMinimised.Checked;
            config.CurrentConfig.AdRemove = txtAdRemove.Text.Trim();
            config.CurrentConfig.MasterAvatar = txtMavatar.Text.Trim();
            config.CurrentConfig.MasterObject = txtMObject.Text.Trim(); 
            config.CurrentConfig.AutoTransfer = chkAutoTransfer.Checked;
            config.CurrentConfig.DisableTrayIcon = chkTray.Checked;
            config.CurrentConfig.DisableTyping = chkTyping.Checked;
            config.CurrentConfig.AutoAcceptFriends = chkAutoFriend.Checked;
            config.CurrentConfig.EnforceLSLsecurity = checkBox12.Checked;
            config.CurrentConfig.DisplayLSLcommands = chkLSL.Checked;
            //config.CurrentConfig.BroadcastID = cbTag.Checked;

            if (comboBox1.SelectedIndex == 0)
            {
                instance.Config.CurrentConfig.DeclineInv = false;  // chkDeclineInv.Checked;
                config.CurrentConfig.AutoAcceptItems = false;
                return;
            }

            if (comboBox1.SelectedIndex == 1)
            {
                instance.Config.CurrentConfig.DeclineInv = true;  // chkDeclineInv.Checked;
                config.CurrentConfig.AutoAcceptItems = false;
                return;
            }

            if (comboBox1.SelectedIndex == 2)
            {
                config.CurrentConfig.AutoAcceptItems = true;   // chkAutoAccept.Checked;
                instance.Config.CurrentConfig.DeclineInv = false;  // chkDeclineInv.Checked;
                return;
            }
        }

        #endregion

        private void PrefGeneralConsole_Load(object sender, EventArgs e)
        {

        }

        private void rdoOfficeStyle_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkRadar_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBox1.Checked)
            //{
            //    groupBox2.Visible = true;
            //}
            //else
            //{
            //    groupBox2.Visible = false;
            //}
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            //toolTip1.Show(pictureBox2);
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            //toolTip1.Close();
        }

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            toolTip2.Show(pictureBox3);
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            toolTip2.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void tBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = tBar1.Value.ToString(CultureInfo.CurrentCulture);
            tbar2.Maximum = tBar1.Value;
            client.Self.Movement.Camera.Far = (float)tBar1.Value;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
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

        private void picAutoSit_MouseHover(object sender, EventArgs e)
        {
            toolTip4.Show(picAutoSit);
        }

        private void picAutoSit_MouseLeave(object sender, EventArgs e)
        {
            toolTip4.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void picAutoSit_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            toolTip5.Show(pictureBox4);
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            toolTip5.Close();
        }

        private void tbar2_Scroll(object sender, EventArgs e)
        {
            textBox2.Text = tbar2.Value.ToString(CultureInfo.CurrentCulture);
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox5_MouseHover(object sender, EventArgs e)
        {
            toolTip6.Show(pictureBox5);
        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            toolTip6.Close();
        }

        private void rdoSystemStyle_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkAutoSit_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void pictureBox6_MouseHover(object sender, EventArgs e)
        {
            toolTip7.Show(pictureBox6);
        }

        private void pictureBox6_MouseLeave(object sender, EventArgs e)
        {
            toolTip7.Close();
        }

        private void chkNotifications_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkNotifications.Checked)
            //{
            //    chkHide.Checked = false;
            //    chkHide.Enabled = false;
            //}
            //else
            //{
            //    //chkHide.Checked = true;
            //    chkHide.Enabled = true;
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            instance.Config.CurrentConfig.GroupManPro = GetRandomPassword(15);
            textBox3.Text = instance.Config.CurrentConfig.GroupManPro;
        }

        public string GetRandomPassword(int length)
        {
            char[] chars = "$%@abcdefghijklmnopqrstuvwxyz1234567890?!*ABCDEFGHIJKLMNOPQRSTUVWXYZ#^&".ToCharArray();
            string password = string.Empty;
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int x = random.Next(1, chars.Length);

                // Don't allow repetition of characters
                if (!password.Contains(chars.GetValue(x).ToString()))
                    password += chars.GetValue(x);
                else
                    i--;
            }
            return password;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string folder = client.Settings.ASSET_CACHE_DIR;

            if (!Directory.Exists(folder))
            {
                return;
            }

            string[] files = Directory.GetFiles(@folder);

            try
            {
                foreach (string file in files)
                    File.Delete(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show("There has been an error: " + ex.Message, "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Cache files have been cleared successfully.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", client.Settings.ASSET_CACHE_DIR);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void picAutoSit_Click_1(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void chkConnect4_CheckedChanged(object sender, EventArgs e)
        {

        }

        //private void trackBar1_Scroll(object sender, EventArgs e)
        //{
        //    textBox5.Text = trackBar1.Value.ToString();    
        //}

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void nUD1_ValueChanged(object sender, EventArgs e)
        {
            if (loadingtimer)
            {
                loadingtimer = false;
                return;
            }

            instance.Config.CurrentConfig.LogOffTimerChanged = true;
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                trackBar1.Value = 50;
                chkConnect4.Checked = false;
                tBar1.Value = 25;
                tbar2.Value = 10;
            }
            else
            {
                trackBar1.Value = 500;
                tBar1.Value = 64;
                tbar2.Value = 20;
            }
        }

        //private void trackBar1_ValueChanged(object sender, EventArgs e)
        //{
        //    SetBarValue();
        //}

        private void SetBarValue()
        {
            //textBox5.Text = trackBar1.Value.ToString();

            //if (trackBar1.Value > 1500)
            //{
            //    label12.Text = "Broadband";
            //}
            //else if (trackBar1.Value > 500)
            //{
            //    label12.Text = "Cable";
            //}
            //else if (trackBar1.Value > 60)
            //{
            //    label12.Text = "DSL";
            //}
            //else
            //{
            //    label12.Text = "Dial-up";
            //}
        }

        private void tBar1_ValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = tBar1.Value.ToString(CultureInfo.CurrentCulture);    
        }

        private void tbar2_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = tbar2.Value.ToString(CultureInfo.CurrentCulture);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                groupBox3.Enabled = true;
            }
            else
            {
                groupBox3.Enabled = false; 
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            textBox7.BackColor =  textBox6.BackColor = colorDialog1.Color;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                fontDialog1.ShowDialog();
                textBox7.Font = new Font(fontDialog1.Font.FontFamily, fontDialog1.Font.Size, fontDialog1.Font.Style);
                textBox7.Text = "size " + fontDialog1.Font.Size.ToString(CultureInfo.CurrentCulture);

                headerfont = fontDialog1.Font.FontFamily.Name.ToString(CultureInfo.CurrentCulture);
                headerfontstyle = fontDialog1.Font.Style.ToString();
                headerfontsize = fontDialog1.Font.Size;  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);   
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                fontDialog1.ShowDialog();
                textBox8.Font = new Font(fontDialog1.Font.FontFamily, fontDialog1.Font.Size, fontDialog1.Font.Style);
                textBox8.Text = "size " + fontDialog1.Font.Size.ToString(CultureInfo.CurrentCulture);

                textfont = fontDialog1.Font.FontFamily.Name.ToString(CultureInfo.CurrentCulture);
                textfontstyle = fontDialog1.Font.Style.ToString();
                textfontsize = fontDialog1.Font.Size;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox7_MouseHover(object sender, EventArgs e)
        {
            toolTip8.Show(pictureBox7);
        }

        private void pictureBox7_MouseLeave(object sender, EventArgs e)
        {
            toolTip8.Close();
        }

        private void chkHide_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkTray_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTray.Checked)
            {
                chkNotifications.Checked = true;
                chkNotifications.Enabled = false;
            }
            else
            {
                chkNotifications.Enabled = true;
            }
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            textBox8.BackColor = textBox5.BackColor = colorDialog1.Color;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            textBox9.BackColor = colorDialog1.Color;
        }

        private void nUD2_ValueChanged(object sender, EventArgs e)
        {
            if (loadingtimer1)
            {
                loadingtimer1 = false;
                return;
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_MouseHover_1(object sender, EventArgs e)
        {
            toolTip9.Show(pictureBox2);
        }

        private void pictureBox2_MouseLeave_1(object sender, EventArgs e)
        {
            toolTip9.Close();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) trackBar1.Enabled = false;

            trackBar1.Value = 500; 
            label19.Text = "500";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked) trackBar1.Enabled = true;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label19.Text = trackBar1.Value.ToString();    
        }

        private void chkDisableRadar_CheckedChanged(object sender, EventArgs e)
        {
            //if (loading)
            //{
            //    loading = false;
            //    return;
            //}

            //if (!chkDisableRadar.Checked && chkDisableRadar.Checked != restart)
            //{
            //    MessageBox.Show("If you 'Apply' this change you will need to re-start METAbolt", "METAbolt");
            //}
        }
    }
}
