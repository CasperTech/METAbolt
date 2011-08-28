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
using SLNetworkComm;
using OpenMetaverse;
using Khendys.Controls;
using ExceptionReporting;
using System.Threading;

namespace METAbolt
{
    public partial class IMTabWindow : UserControl
    {
        private METAboltInstance instance;
        private SLNetCom netcom;
        private UUID target;
        private UUID session;
        private string toName;
        private IMTextManager textManager;
        private bool typing = false;
        private bool pasted = false;
        private ExceptionReporter reporter = new ExceptionReporter();
        //private const int WM_KEYUP = 0x101;
        private const int WM_KEYDOWN = 0x100;
        private TabsConsole tab;

        internal class ThreadExceptionHandler
        {
            public void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
            {
                ExceptionReporter reporter = new ExceptionReporter();
                reporter.Show(e.Exception);
            }
        }

        public IMTabWindow(METAboltInstance instance, UUID target, UUID session, string toName)
        {
            InitializeComponent();

            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.instance = instance;
            netcom = this.instance.Netcom;

            this.target = target;
            this.session = session;
            this.toName = toName;
            tab = instance.TabConsole;

            textManager = new IMTextManager(this.instance, new RichTextBoxPrinter(instance, rtbIMText), this.session, this.toName);
            this.Disposed += new EventHandler(IMTabWindow_Disposed);

            AddNetcomEvents();

            tsbTyping.ToolTipText = toName + " is typing";

            ApplyConfig(this.instance.Config.CurrentConfig);
            this.instance.Config.ConfigApplied += new EventHandler<ConfigAppliedEventArgs>(Config_ConfigApplied);

            CreateSmileys();

            if (this.instance.IMHistyoryExists(this.toName, false))
            {
                toolStripButton2.Enabled = true; 
            }
        }

        private void SetExceptionReporter()
        {
            reporter.Config.ShowSysInfoTab = false;   // alternatively, set properties programmatically
            reporter.Config.ShowFlatButtons = true;   // this particular config is code-only
            reporter.Config.CompanyName = "METAbolt";
            reporter.Config.ContactEmail = "metabolt@vistalogic.co.uk";
            reporter.Config.EmailReportAddress = "metabolt@vistalogic.co.uk";
            reporter.Config.WebUrl = "http://www.metabolt.net/metaforums/";
            reporter.Config.AppName = "METAbolt";
            reporter.Config.MailMethod = ExceptionReporting.Core.ExceptionReportInfo.EmailMethod.SimpleMAPI;
            reporter.Config.BackgroundColor = Color.White;
            reporter.Config.ShowButtonIcons = false;
            reporter.Config.ShowLessMoreDetailButton = true;
            reporter.Config.TakeScreenshot = true;
            reporter.Config.ShowContactTab = true;
            reporter.Config.ShowExceptionsTab = true;
            reporter.Config.ShowFullDetail = true;
            reporter.Config.ShowGeneralTab = true;
            reporter.Config.ShowSysInfoTab = true;
            reporter.Config.TitleText = "METAbolt Exception Reporter";
        }

        private void CreateSmileys()
        {
            EmoticonMenuItem _menuItem;

            _menuItem = new EmoticonMenuItem(Smileys.AngelSmile);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[0].Tag = (object)"angelsmile;";

            _menuItem = new EmoticonMenuItem(Smileys.AngrySmile);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[1].Tag = "angry;";

            _menuItem = new EmoticonMenuItem(Smileys.Beer);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[2].Tag = "beer;";

            //_menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.BrokenHeart);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[3].Tag = "brokenheart;";

            _menuItem = new EmoticonMenuItem(Smileys.bye);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[4].Tag = "bye";

            _menuItem = new EmoticonMenuItem(Smileys.clap);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[5].Tag = "clap;";

            _menuItem = new EmoticonMenuItem(Smileys.ConfusedSmile);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[6].Tag = ":S";

            _menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.cool);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[7].Tag = "cool;";

            _menuItem = new EmoticonMenuItem(Smileys.CrySmile);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[8].Tag = "cry;";

            //_menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.DevilSmile);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[9].Tag = "devil;";

            _menuItem = new EmoticonMenuItem(Smileys.duh);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[10].Tag = "duh;";

            _menuItem = new EmoticonMenuItem(Smileys.EmbarassedSmile);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[11].Tag = "embarassed;";

            _menuItem = new EmoticonMenuItem(Smileys.happy0037);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[12].Tag = ":)";

            _menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.heart);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[13].Tag = "heart;";

            _menuItem = new EmoticonMenuItem(Smileys.kiss);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[14].Tag = "muah;";

            //_menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.help);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[15].Tag = "help";

            _menuItem = new EmoticonMenuItem(Smileys.liar);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[16].Tag = "liar;";

            _menuItem = new EmoticonMenuItem(Smileys.lol);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[17].Tag = "lol";

            _menuItem = new EmoticonMenuItem(Smileys.oops);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[18].Tag = "oops";

            _menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.sad);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[19].Tag = ":(";

            _menuItem = new EmoticonMenuItem(Smileys.shhh);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[20].Tag = "shhh";

            //_menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.sigh);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[21].Tag = "sigh";

            _menuItem = new EmoticonMenuItem(Smileys.silenced);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[22].Tag = ":X";

            _menuItem = new EmoticonMenuItem(Smileys.think);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[23].Tag = "thinking;";

            _menuItem = new EmoticonMenuItem(Smileys.ThumbsUp);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[24].Tag = "thumbsup;";

            _menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.whistle);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[25].Tag = "whistle;";

            _menuItem = new EmoticonMenuItem(Smileys.wink);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[26].Tag = ";)";

            //_menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.wow);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[27].Tag = "wow";

            _menuItem = new EmoticonMenuItem(Smileys.yawn);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[28].Tag = "yawn;";

            _menuItem = new EmoticonMenuItem(Smileys.zzzzz);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[29].Tag = "zzzzz";

            _menuItem.Dispose(); 
        }

        // When an emoticon is clicked, insert its image into to RTF
        private void cmenu_Emoticons_Click(object _sender, EventArgs _args)
        {
            // Write the code here
            EmoticonMenuItem _item = (EmoticonMenuItem)_sender;

            cbxInput.Text += _item.Tag.ToString();
            cbxInput.Select(cbxInput.Text.Length, 0);
            //cbxInput.Focus();
        }

        private void netcom_InstantMessageReceived(object sender, InstantMessageEventArgs e)
        {
            if (e.IM.IMSessionID != session)
            {
                return;
            }

            if (e.IM.Dialog == InstantMessageDialog.StartTyping)
            {
                tsbTyping.Visible = true;
            }
            else
            {
                tsbTyping.Visible = false;
            }
        }

        private void Config_ConfigApplied(object sender, ConfigAppliedEventArgs e)
        {
            ApplyConfig(e.AppliedConfig);
        }

        private void ApplyConfig(Config config)
        {
            if (config.InterfaceStyle == 0) //System
                toolStrip1.RenderMode = ToolStripRenderMode.System;
            else if (config.InterfaceStyle == 1) //Office 2003
                toolStrip1.RenderMode = ToolStripRenderMode.ManagerRenderMode;

            //rtbIMText.BackColor = instance.Config.CurrentConfig.BgColour;
        }

        private void AddNetcomEvents()
        {
            netcom.ClientLoginStatus += new EventHandler<LoginProgressEventArgs>(netcom_ClientLoginStatus);
            netcom.ClientDisconnected += new EventHandler<DisconnectedEventArgs>(netcom_ClientDisconnected);
            netcom.InstantMessageReceived += new EventHandler<InstantMessageEventArgs>(netcom_InstantMessageReceived);
        }

        private void RemoveNetcomEvents()
        {
            netcom.ClientLoginStatus -= new EventHandler<LoginProgressEventArgs>(netcom_ClientLoginStatus);
            netcom.ClientDisconnected -= new EventHandler<DisconnectedEventArgs>(netcom_ClientDisconnected);
            this.instance.Config.ConfigApplied -= new EventHandler<ConfigAppliedEventArgs>(Config_ConfigApplied);
            netcom.InstantMessageReceived -= new EventHandler<InstantMessageEventArgs>(netcom_InstantMessageReceived);
        }

        private void netcom_ClientLoginStatus(object sender, LoginProgressEventArgs e)
        {
            if (e.Status != LoginStatus.Success) return;

            RefreshControls();
        }

        private void netcom_ClientDisconnected(object sender, DisconnectedEventArgs e)
        {
            RefreshControls();
        }

        private void IMTabWindow_Disposed(object sender, EventArgs e)
        {
            CleanUp();
        }

        public void CleanUp()
        {
            this.cbxInput.Text = string.Empty;  
            instance.TabConsole.RemoveTabEntry(SessionId.ToString());
            textManager.CleanUp();
            textManager = null;
            target = UUID.Zero;  
            RemoveNetcomEvents();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string message = cbxInput.Text;
            string message1 = string.Empty;
            string message2 = string.Empty;  

            if (message.Length == 0) return;

            if (message.Length > 1023)
            {
                message1 = message.Substring(0, 1022);
                netcom.SendInstantMessage(message1, target, session);

                if (message.Length > 2046)
                {
                    message2 = message.Substring(1023, 2046);
                    netcom.SendInstantMessage(message2, target, session);
                }
            }
            else
            {
                netcom.SendInstantMessage(message, target, session);
            }

            this.ClearIMInput();
        }

        private void cbxInput_TextChanged(object sender, EventArgs e)
        {
            RefreshControls();
        }

        private void RefreshControls()
        {
            if (!netcom.IsLoggedIn)
            {
                this.cbxInput.Enabled = false;
                btnSend.Enabled = false;
                this.cbxInput.Text = string.Empty;
                target = UUID.Zero;  
                return;
            }

            if (this.cbxInput.Text.Length > 0)
            {
                btnSend.Enabled = true;

                if (!typing)
                {
                    netcom.SendIMStartTyping(target, session);
                    typing = true;
                }
            }
            else
            {
                btnSend.Enabled = false;
                netcom.SendIMStopTyping(target, session);
                typing = false;
            }
        }

        private void cbxInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (pasted)
            {
                int pos = cbxInput.SelectionStart;
                cbxInput.SelectionLength = cbxInput.Text.Length - pos;
                cbxInput.Text = cbxInput.SelectedText;
                pasted = false;
            }

            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            if (cbxInput.Text.Length == 0) return;

            netcom.SendInstantMessage(cbxInput.Text, target, session);
            this.ClearIMInput();
        }

        private void ClearIMInput()
        {
            this.cbxInput.Items.Add(cbxInput.Text);
            this.cbxInput.Text = string.Empty;
        }

        public void SelectIMInput()
        {
            cbxInput.Select();
        }

        private void tbtnProfile_Click(object sender, EventArgs e)
        {
            (new frmProfile(instance, toName, target)).Show();
        }

        private void cbxInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) e.SuppressKeyPress = true;

            if (e.Control && e.KeyCode == Keys.V)
            {
                ClipboardAsync Clipboard2 = new ClipboardAsync();
                cbxInput.Text += Clipboard2.GetText(TextDataFormat.UnicodeText).Replace(Environment.NewLine, "\r\n");

                pasted = true; 
            }
        }

        public UUID TargetId
        {
            get { return target; }
            set { target = value; }
        }

        public string TargetName
        {
            get { return toName; }
            set { toName = value; }
        }

        public UUID SessionId
        {
            get { return session; }
            set { session = value; }
        }

        public IMTextManager TextManager
        {
            get { return textManager; }
            set { textManager = value; }
        }

        private void tbar_SendMessage_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void rtbIMText_TextChanged(object sender, EventArgs e)
        {

        }

        private void rtbIMText_LinkClicked_1(object sender, LinkClickedEventArgs e)
        {
            if (e.LinkText.StartsWith("http://slurl."))
            {
                try
                {
                    // Open up the TP form here
                    string[] split = e.LinkText.Split(new Char[] { '/' });
                    string sim = split[4].ToString();
                    double x = Convert.ToDouble(split[5].ToString());
                    double y = Convert.ToDouble(split[6].ToString());
                    double z = Convert.ToDouble(split[7].ToString());

                    (new frmTeleport(instance, sim, (float)x, (float)y, (float)z)).ShowDialog();
                }
                catch { ; }

            }
            if (e.LinkText.StartsWith("http://maps.secondlife"))
            {
                try
                {
                    // Open up the TP form here
                    string[] split = e.LinkText.Split(new Char[] { '/' });
                    string sim = split[4].ToString();
                    double x = Convert.ToDouble(split[5].ToString());
                    double y = Convert.ToDouble(split[6].ToString());
                    double z = Convert.ToDouble(split[7].ToString());

                    (new frmTeleport(instance, sim, (float)x, (float)y, (float)z)).ShowDialog();
                }
                catch { ; }

            }
            else if (e.LinkText.Contains("http://mbprofile:"))
            {
                try
                {
                    string[] split = e.LinkText.Split(new Char[] { '#' });
                    string avname = split[0].ToString();
                    split = e.LinkText.Split(new Char[] { ':' });
                    string elink = split[2].ToString();
                    split = elink.Split(new Char[] { '&' });

                    UUID avid = (UUID)split[0].ToString();

                    (new frmProfile(instance, avname, avid)).Show();
                }
                catch { ; }
            }
            else if (e.LinkText.StartsWith("http://") || e.LinkText.StartsWith("ftp://") || e.LinkText.StartsWith("https://"))
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            else
            {
                System.Diagnostics.Process.Start("http://" + e.LinkText);
            }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            // Create a SaveFileDialog to request a path and file name to save to.
            SaveFileDialog saveFile1 = new SaveFileDialog();

            string logdir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt";
            logdir += "\\Logs\\";

            saveFile1.InitialDirectory = logdir; 

            // Initialize the SaveFileDialog to specify the RTF extension for the file.
            saveFile1.DefaultExt = "*.rtf";
            saveFile1.Filter = "txt files (*.txt)|*.txt|RTF Files (*.rtf)|*.rtf";  //"RTF Files|*.rtf";
            saveFile1.Title = "Save chat contents to hard disk...";

            // Determine if the user selected a file name from the saveFileDialog.
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
               saveFile1.FileName.Length > 0)
            {
                if (saveFile1.FileName.Substring(saveFile1.FileName.Length - 3) == "rtf")
                {
                    // Save the contents of the RichTextBox into the file.
                    rtbIMText.SaveFile(saveFile1.FileName, RichTextBoxStreamType.RichText);
                }
                else
                {
                    rtbIMText.SaveFile(saveFile1.FileName, RichTextBoxStreamType.PlainText);
                }
            }

            saveFile1.Dispose(); 
        }

        private void tsbClear_Click(object sender, EventArgs e)
        {
            textManager.ClearAllText();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (instance.MuteList.Rows.Contains(target.ToString()))
            {
                MessageBox.Show(toName + " is already in your mute list.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataRow dr = instance.MuteList.NewRow();
            dr["uuid"] = target;
            dr["mute_name"] = toName;
            instance.MuteList.Rows.Add(dr);

            MessageBox.Show(toName + " is now muted.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);      
        }

        private void cbxInput_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected override bool ProcessKeyPreview(ref System.Windows.Forms.Message m)
        {
            const int WM_CHAR = 0x102;
            const int WM_SYSCHAR = 0x106;
            const int WM_SYSKEYDOWN = 0x104;
            //const int WM_SYSKEYUP = 0x105;
            const int WM_IME_CHAR = 0x286;

            KeyEventArgs e = null;

            if ((m.Msg != WM_CHAR) && (m.Msg != WM_SYSCHAR) && (m.Msg != WM_IME_CHAR))
            {
                e = new KeyEventArgs(((Keys)((int)((long)m.WParam))) | ModifierKeys);
                if ((m.Msg == WM_KEYDOWN) || (m.Msg == WM_SYSKEYDOWN))
                {
                    TrappedKeyDown(e);
                }

                if (e.SuppressKeyPress)
                {
                    tab.tabs["chat"].Select();
                    METAboltTab stab = tab.GetTab(toName);
                    stab.Close(); 
                }

                if (e.Handled)
                {
                    return e.Handled;
                }
            }

            return base.ProcessKeyPreview(ref m);
        }

        private void TrappedKeyDown(KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.X) 
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            frmHistory frm = new frmHistory(instance, toName, false);
            frm.Show();
        }
    }
}
