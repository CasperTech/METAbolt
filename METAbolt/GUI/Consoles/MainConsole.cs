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
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
using SLNetworkComm;
using MD5library;
using System.Diagnostics;
using System.Linq;
using ExceptionReporting;
using System.Threading;
using System.Globalization; 

namespace METAbolt
{
    public partial class MainConsole : UserControl, IMETAboltTabControl
    {
        private METAboltInstance instance;
        private SLNetCom netcom;
        private WebBrowser webBrowser;
        private GridClient client;
        private string murl;
        private string clickedurl = string.Empty;
        //private WriteToRegistry METAreg = new WriteToRegistry();
        //private frmMain mform;
        //private frmPlayer fplayer;
        private Dictionary<string, string> MGrids = new Dictionary<string,string>();
        private ExceptionReporter reporter = new ExceptionReporter();
        private List<string> usernlist = new List<string>();

        internal class ThreadExceptionHandler
        {
            public void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
            {
                ExceptionReporter reporter = new ExceptionReporter();
                reporter.Show(e.Exception);
            }
        }

        public MainConsole(METAboltInstance instance)
        {
            InitializeComponent();

            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.instance = instance;
            netcom = this.instance.Netcom;
            client = this.instance.Client;
            AddNetcomEvents();

            //btnInfo_Click();
            if (webBrowser == null)
                this.InitializeWebBrowser();

            webBrowser.Visible = true;
            //btnInfo.Text = "Hide Grid Status";
            label7.Text = "V " + Properties.Resources.METAboltVersion; 

            Disposed += new EventHandler(MainConsole_Disposed);

            LoadGrids();
            InitGridCombo();
            cbxLocation.SelectedIndex = 0;
            InitializeConfig();
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

        private void LoadGrids()
        {
            try
            {
                MGrids.Clear();
 
                bool fext = System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt" + "\\Grids.txt");

                if (fext)
                {
                    string[] file = File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt" + "\\Grids.txt");

                    MGrids = (from p in file
                              let x = p.Split(',')
                              select x).ToDictionary(a => a[0], a => a[1]);
                }
                else
                {
                    CreateGridFile();

                    string[] file = File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt" + "\\Grids.txt");

                    MGrids = (from p in file
                              let x = p.Split(',')
                              select x).ToDictionary(a => a[0], a => a[1]);
                }
            }
            catch { ; }
        }

        static void CreateGridFile()
        {
            StreamWriter SW;

            SW = File.CreateText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt" + "\\Grids.txt");
            SW.WriteLine("3rd Rock Grid,http://grid.3rdrockgrid.com:8002");
            SW.WriteLine("Avatar Hangout,http://login.avatarhangout.com:8002");
            SW.WriteLine("Cyberlandia,http://hypergrid.cyberlandia.net:8002");
            SW.WriteLine("Francogrid,http://grid.francogrid.net:8002");
            SW.WriteLine("Legend City Online,http://login.legendcityonline.com");
            SW.WriteLine("InWorldz,http://inworldz.com:8002/");
            SW.WriteLine("OSGrid,http://login.osgrid.org");
            SW.WriteLine("ReactionGrid,http://reactiongrid.com:8008");
            SW.WriteLine("The Gor Grid,http://thegorgrid.com:8002");
            SW.WriteLine("The New World Grid,http://grid.newworldgrid.com:8002");
            SW.WriteLine("WorldSimTerra,http://wsterra.com:8002");
            SW.WriteLine("Your Alternative Life,http://grid.youralternativelife.com");

            SW.Dispose();
        }

        private void InitGridCombo()
        {
            cbxGrid.Items.Clear();

            cbxGrid.Items.Add("SL Main Grid (Agni)");
            cbxGrid.Items.Add("SL Beta Grid (Aditi)");

            foreach (KeyValuePair<string, string> entry in MGrids)
            {
                cbxGrid.Items.Add(entry.Key);  
            } 

            cbxGrid.Items.Add("Other...");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void SaveUserSettings()
        {
            instance.Config.CurrentConfig.FirstName = txtFirstName.Text;
            instance.Config.CurrentConfig.LastName = txtLastName.Text;

            // Save user list
            string ulist = string.Empty;

            foreach (string s in usernlist)
            {
                ulist += s + "|";
            }

            if (ulist.EndsWith("|"))
            {
                ulist = ulist.Substring(0, ulist.Length - 1);   
            }

            instance.Config.CurrentConfig.UserNameList = ulist; 

            instance.Config.CurrentConfig.iRemPWD = chkPWD.Checked;

            if (netcom.LoginOptions.IsPasswordMD5)
            {
                instance.Config.CurrentConfig.PasswordMD5 = txtPassword.Text;
            }
            else
            {
                instance.Config.CurrentConfig.PasswordMD5 = Utils.MD5(txtPassword.Text);
            }

            instance.Config.CurrentConfig.LoginLocationType = cbxLocation.SelectedIndex;
            instance.Config.CurrentConfig.LoginLocation = cbxLocation.Text;

            instance.Config.CurrentConfig.LoginGrid = cbxGrid.SelectedIndex;
            instance.Config.CurrentConfig.LoginUri = txtCustomLoginUri.Text;

            instance.Config.SaveCurrentConfig();  
        }

        private void AddNetcomEvents()
        {
            netcom.ClientLoggingIn += new EventHandler<OverrideEventArgs>(netcom_ClientLoggingIn);
            netcom.ClientLoginStatus += new EventHandler<LoginProgressEventArgs>(netcom_ClientLoginStatus);
            netcom.ClientLoggingOut += new EventHandler<OverrideEventArgs>(netcom_ClientLoggingOut);
            netcom.ClientLoggedOut += new EventHandler(netcom_ClientLoggedOut);
        }

        void MainConsole_Disposed(object sender, EventArgs e)
        {
            netcom.ClientLoggingIn -= new EventHandler<OverrideEventArgs>(netcom_ClientLoggingIn);
            netcom.ClientLoginStatus -= new EventHandler<LoginProgressEventArgs>(netcom_ClientLoginStatus);
            netcom.ClientLoggingOut -= new EventHandler<OverrideEventArgs>(netcom_ClientLoggingOut);
            netcom.ClientLoggedOut -= new EventHandler(netcom_ClientLoggedOut);
            webBrowser.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted);
            webBrowser.Navigating -= new WebBrowserNavigatingEventHandler(webBrowser_Navigating);
        }

        private class Item
        {
            public string Name;
            public string Value;

            public Item(string name, string value)
            {
                Name = name; Value = value;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        private void InitializeConfig()
        {
            // Populate usernames
            //usernlist
            string ulist = instance.Config.CurrentConfig.UserNameList;

            if (!string.IsNullOrEmpty(ulist))
            {
                string[] llist = ulist.Split('|');

                foreach (string s in llist)
                {
                    string[] llist1 = s.Split('\\');
                    usernlist.Add(s);
                    //cboUserList.Items.Add(s);

                    string epwd = string.Empty;  

                    if (llist1.Length == 2)
                    {
                        epwd = llist1[1];

                        if (!string.IsNullOrEmpty(epwd))
                        {
                            try
                            {
                                Crypto cryp = new Crypto(Crypto.SymmProvEnum.Rijndael);
                                string cpwd = cryp.Decrypt(epwd);
                                epwd = cpwd;
                            }
                            catch
                            {
                                epwd = string.Empty;  
                            }
                        }

                        cboUserList.Items.Add(new Item(llist1[0], epwd));
                    }
                    else
                    {
                        cboUserList.Items.Add(new Item(llist1[0], string.Empty));
                    }
                }
            }

            chkPWD.Checked = instance.Config.CurrentConfig.iRemPWD;
            txtFirstName.Text = instance.Config.CurrentConfig.FirstName;
            txtLastName.Text = instance.Config.CurrentConfig.LastName;

            if (instance.Config.CurrentConfig.iRemPWD)
            {
                string epwd = instance.Config.CurrentConfig.PasswordMD5;

                txtPassword.Text = epwd;
            }

            netcom.LoginOptions.IsPasswordMD5 = true;

            cbxLocation.SelectedIndex = instance.Config.CurrentConfig.LoginLocationType;
            cbxLocation.Text = instance.Config.CurrentConfig.LoginLocation;

            cbxGrid.SelectedIndex = instance.Config.CurrentConfig.LoginGrid;
            txtCustomLoginUri.Text = instance.Config.CurrentConfig.LoginUri;

            if (instance.ReBooted)
            {
                BeginLogin();
                //btnLogin.PerformClick();
                timer2.Enabled = true;
                timer2.Start(); 
            }
        }

        private void netcom_ClientLoginStatus(object sender, LoginProgressEventArgs e)
        {
            try
            {
                switch (e.Status)
                {
                    case LoginStatus.ConnectingToLogin:
                        lblLoginStatus.Text = "Connecting to login server...";
                        lblLoginStatus.ForeColor = Color.Black;
                        break;

                    case LoginStatus.ConnectingToSim:
                        lblLoginStatus.Text = "Connecting to region...";
                        lblLoginStatus.ForeColor = Color.Black;
                        break;

                    case LoginStatus.Redirecting:
                        lblLoginStatus.Text = "Redirecting...";
                        lblLoginStatus.ForeColor = Color.Black;
                        break;

                    case LoginStatus.ReadingResponse:
                        lblLoginStatus.Text = "Reading response...";
                        lblLoginStatus.ForeColor = Color.Black;
                        break;

                    case LoginStatus.Success:
                        //SetLang();
                        lblLoginStatus.Text = "Logged in as " + netcom.LoginOptions.FullName;
                        lblLoginStatus.ForeColor = Color.Blue;
     
                        string uname = client.Self.FirstName + " " + client.Self.LastName + "\\";

                        Wildcard wildcard = new Wildcard(client.Self.FirstName + " " + client.Self.LastName + "*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        List<string> torem = new List<string>();

                        client.Self.Movement.Camera.Far = (float)instance.Config.CurrentConfig.RadarRange;

                        foreach (string s in usernlist)
                        {
                            if(wildcard.IsMatch(s))
                            {
                                torem.Add(s);
                            }
                        }

                        foreach (string s in torem)
                        {
                            if (wildcard.IsMatch(s))
                            {
                                usernlist.Remove(s);
                            }
                        }

                        //string epwd1 = txtPassword.Text;

                        if (chkPWD.Checked)
                        {
                            string epwd = txtPassword.Text;
                            Crypto cryp = new Crypto(Crypto.SymmProvEnum.Rijndael);
                            string cpwd = cryp.Encrypt(epwd);

                            uname += cpwd;
                        }

                        usernlist.Add(uname);

                        btnLogin.Text = "Exit";
                        btnLogin.Enabled = true;

                        instance.ReBooted = false;
                        timer2.Enabled = false;
                        timer2.Stop();

                        try
                        {
                            SaveUserSettings();

                            string fname = client.Self.FirstName + "_" + client.Self.LastName;

                            if (chkCmd.Checked)
                            {
                                // create the CMD file
                                CreateCmdFile();

                                FileInfo newFileInfo = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt", fname + "_METAbolt.ini"));

                                if (!newFileInfo.Exists)
                                {
                                    string pth = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt", fname + "_METAbolt.ini");
                                    instance.Config.CurrentConfig.Save(pth);
                                }
                            }

                            //instance.Config.ChangeConfigFile(fname);
                            this.instance.ReapplyConfig(fname);

                            if (instance.Config.CurrentConfig.AIon)
                            {
                                instance.InitAI();
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Log("Error trying to save user settings to METAbolt.ini ", Helpers.LogLevel.Warning, ex);
                        }

                        LoadWebPage();

                        //SetLang();

                        break;

                    case LoginStatus.Failed:
                        lblLoginStatus.Text = e.Message;
                        lblLoginStatus.ForeColor = Color.Red;

                        //proLogin.Visible = false;

                        btnLogin.Text = "Retry";
                        btnLogin.Enabled = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Login (status): " + ex.Message, Helpers.LogLevel.Error);
            }
        }

        // TODO: This is buggy in libopenmv and causes all sorts of problems
        // DO NOT enable it until it is fixed
        //private void SetLang()
        //{
        //    CultureInfo cult = CultureInfo.CurrentCulture;
        //    string land = cult.TwoLetterISOLanguageName;

        //    AgentManager avm = new AgentManager(client);

        //    try
        //    {
        //        avm.UpdateAgentLanguage(land, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log("Agent Language: (relog can help) " + ex.Message, Helpers.LogLevel.Warning);
        //        //reporter.Show(ex);
        //    }
        //}

        private void netcom_ClientLoggedOut(object sender, EventArgs e)
        {
            pnlLoginPrompt.Visible = true;
            pnlLoggingIn.Visible = false;

            btnLogin.Text = "Login";
            btnLogin.Enabled = true;
        }

        private void netcom_ClientLoggingOut(object sender, OverrideEventArgs e)
        {
            btnLogin.Enabled = false;

            lblLoginStatus.Text = "Logging out...";
            lblLoginStatus.ForeColor = Color.FromKnownColor(KnownColor.ControlText);

            //proLogin.Visible = true;
        }

        private void netcom_ClientLoggingIn(object sender, OverrideEventArgs e)
        {
            lblLoginStatus.Text = "Logging in...";
            lblLoginStatus.ForeColor = Color.FromKnownColor(KnownColor.ControlText);

            //proLogin.Visible = true;
            pnlLoggingIn.Visible = true;
            pnlLoginPrompt.Visible = false;

            btnLogin.Enabled = false;
        }

        private void InitializeWebBrowser()
        {
            lblInitWebBrowser.Visible = true;
            lblInitWebBrowser.Refresh();

            //btnInfo.Enabled = false;
            //btnInfo.Refresh();

            string lkey = instance.Config.CurrentConfig.AdRemove;

            if (!string.IsNullOrEmpty(lkey))
            {
                METAMD5 md5 = new METAMD5();

                if (md5.ValidateMachinePasscode(lkey))
                {
                    murl = "http://www.metabolt.net/index.asp?user=login&nod=true&ver=" + Properties.Resources.METAboltVersion.ToString();
                }
                else
                {
                    murl = "http://www.metabolt.net/index.asp?user=login&nod=false&ver=" + Properties.Resources.METAboltVersion.ToString();
                }
            }
            else
            {
                murl = "http://www.metabolt.net/index.asp?user=login&nod=false&ver=" + Properties.Resources.METAboltVersion.ToString();
            }

            webBrowser = new WebBrowser();
            webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted);
            webBrowser.Navigating += new WebBrowserNavigatingEventHandler(webBrowser_Navigating);
            webBrowser.Url = new Uri(murl);
            webBrowser.AllowNavigation = true;
            //webBrowser.AllowWebBrowserDrop = false;
            webBrowser.Dock = DockStyle.Fill;
            webBrowser.IsWebBrowserContextMenuEnabled = false;
            webBrowser.ScriptErrorsSuppressed = true;
            webBrowser.ScrollBarsEnabled = true;
            webBrowser.NewWindow += new CancelEventHandler(webBrowser_NewWindow); 
            pnlLoginPage.Controls.Add(webBrowser);
        }

        private void LoadWebPage()
        {
            string lkey = instance.Config.CurrentConfig.AdRemove;

            if (!string.IsNullOrEmpty(lkey))
            {
                METAMD5 md5 = new METAMD5();

                if (md5.VerifyAdLicence(netcom.LoginOptions.FullName, client.Self.AgentID.ToString(), lkey))
                {
                    murl = "http://www.metabolt.net/index.asp?user=login&nod=true&ver=" + Properties.Resources.METAboltVersion.ToString();
                }
                else
                {
                    murl = "http://www.metabolt.net/index.asp?user=login&nod=false&ver=" + Properties.Resources.METAboltVersion.ToString();
                }
            }
            else
            {
                murl = "http://www.metabolt.net/index.asp?user=login&nod=false&ver=" + Properties.Resources.METAboltVersion.ToString();
            }

            webBrowser.Navigate(murl);
        }

        private void webBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(clickedurl))
            {
                HtmlElement link = webBrowser.Document.ActiveElement;
                clickedurl = link.GetAttribute("href");
            }

            e.Cancel = true;

            if (clickedurl.StartsWith("http://slurl."))
            {
                // Open up the TP form here
                string[] split = clickedurl.Split(new Char[] { '/' });
                string sim = split[4].ToString();
                double x = Convert.ToDouble(split[5].ToString());
                double y = Convert.ToDouble(split[6].ToString());
                double z = Convert.ToDouble(split[7].ToString());

                (new frmTeleport(instance, sim, (float)x, (float)y, (float)z)).Show();
                clickedurl = string.Empty;
                return;
            }
            if (clickedurl.StartsWith("http://maps.secondlife"))
            {
                // Open up the TP form here
                string[] split = clickedurl.Split(new Char[] { '/' });
                string sim = split[4].ToString();
                double x = Convert.ToDouble(split[5].ToString());
                double y = Convert.ToDouble(split[6].ToString());
                double z = Convert.ToDouble(split[7].ToString());

                (new frmTeleport(instance, sim, (float)x, (float)y, (float)z)).Show();
                clickedurl = string.Empty;
                return;
            }

            System.Diagnostics.Process.Start(clickedurl);
            clickedurl = string.Empty;  
        }

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            //if (clickedurl != string.Empty)
            //{
            //    e.Cancel = true;
            //    System.Diagnostics.Process.Start(e.Url.ToString());
            //}
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            lblInitWebBrowser.Visible = false;
            //btnInfo.Enabled = true;

            HtmlElementCollection links = webBrowser.Document.Links;

            foreach (HtmlElement var in links)
            {
                var.AttachEventHandler("onclick", LinkClicked);
            }
        }

        private void LinkClicked(object sender, EventArgs e)
        {

            HtmlElement link = webBrowser.Document.ActiveElement;
            clickedurl = link.GetAttribute("href");
        }

        private void BeginLogin()
        {
            try
            {
                if (string.IsNullOrEmpty(txtLastName.Text))
                {
                    txtLastName.Text = "Resident";
                }

                instance.LoggedIn = true;
                netcom.LoginOptions.FirstName = txtFirstName.Text;
                netcom.LoginOptions.LastName = txtLastName.Text;

                //instance.Config.SaveCurrentConfig();

                //string full_name = txtFirstName.Text + "_" + txtLastName.Text;
                //this.instance.ReapplyConfig(full_name);  

                // Fix thx to METAforum member Tipi (28/06/2010). I don't know how this
                // escaped us all these years. Embarassing :S
                string pwd = txtPassword.Text;

                if (pwd.Length > 16)
                {
                    pwd = pwd.Substring(0, 16);
                }

                netcom.LoginOptions.Password = pwd;   // txtPassword.Text;
                netcom.LoginOptions.UserAgent = Properties.Resources.METAboltTitle + " " + Properties.Resources.METAboltVersion;
                netcom.LoginOptions.Author = Properties.Resources.METAboltAuthor;

                switch (cbxLocation.SelectedIndex)
                {
                    case -1: //Custom
                        netcom.LoginOptions.StartLocation = StartLocationType.Custom;
                        netcom.LoginOptions.StartLocationCustom = cbxLocation.Text;
                        break;

                    case 0: //Home
                        netcom.LoginOptions.StartLocation = StartLocationType.Home;
                        break;

                    case 1: //Last
                        netcom.LoginOptions.StartLocation = StartLocationType.Last;
                        break;
                }

                switch (cbxGrid.SelectedIndex)
                {
                    case 0: //Main grid
                        netcom.LoginOptions.Grid = LoginGrid.MainGrid;
                        break;

                    case 1: //Beta grid
                        netcom.LoginOptions.Grid = LoginGrid.BetaGrid;
                        break;

                    default: //Custom or other
                        netcom.LoginOptions.Grid = LoginGrid.Custom;

                        string selectedgrid = cbxGrid.SelectedItem.ToString();

                        if (selectedgrid == "Other...")
                        {

                            if (txtCustomLoginUri.TextLength == 0 ||
                                txtCustomLoginUri.Text.Trim().Length == 0)
                            {
                                MessageBox.Show("You must specify the Login Uri to connect to a custom grid.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Check for http beginning
                            string hhder = string.Empty;

                            if (!txtCustomLoginUri.Text.StartsWith("http://"))
                            {
                                if (!txtCustomLoginUri.Text.StartsWith("https://"))
                                {
                                    hhder = "http://";
                                }
                            }

                            netcom.LoginOptions.GridCustomLoginUri = hhder + txtCustomLoginUri.Text;
                        }
                        else
                        {
                            if (MGrids.ContainsKey(selectedgrid))
                            {
                                netcom.LoginOptions.GridCustomLoginUri = MGrids[selectedgrid];
                            }
                        }

                        break;
                }

                lblLoginStatus.Text = "Logging in...";
                lblLoginStatus.ForeColor = Color.FromKnownColor(KnownColor.ControlText);

                pnlLoggingIn.Visible = true;
                pnlLoginPrompt.Visible = false;

                btnLogin.Enabled = false;

                client.Settings.HTTP_INVENTORY = !instance.Config.CurrentConfig.DisableHTTPinv;
                client.Settings.USE_LLSD_LOGIN = instance.Config.CurrentConfig.UseLLSD;
                //instance.SetSettings();  

                netcom.Login();
                DoBrowser();
            }
            catch (Exception ex)
            {
                Logger.Log("Login (main): " + ex.Message, Helpers.LogLevel.Error);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            switch (btnLogin.Text)
            {
                case "Login": instance.LogOffClicked = false; BeginLogin(); break;

                case "Retry":
                    pnlLoginPrompt.Visible = true;
                    pnlLoggingIn.Visible = false;
                    btnLogin.Text = "Login";
                    break;

                //case "Logout": this.instance.MainForm.Close(); break;
                case "Exit":
                    instance.LogOffClicked = true;  
                    instance.LoggedIn = false; 
                    pnlLoggingIn.Visible = false;
                    pnlLoginPrompt.Visible = true;
                    btnLogin.Enabled = true;
                    
                    //netcom.Logout();
                    try
                    {
                        if (netcom.IsLoggedIn) netcom.Logout();
                    }
                    catch (Exception ex)
                    {
                        string exp = ex.Message.ToString();
                        MessageBox.Show(exp);  
                    }

                    break;
            }
        }

        private void CreateCmdFile()
        {
            try
            {
                string cuser = txtFirstName.Text + "_" + txtLastName.Text;
                string textfile = cuser + ".bat";
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt", textfile);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (StreamWriter sr = File.CreateText(path))
                {
                    string line = "@ECHO OFF";
                    sr.WriteLine(line);
                    sr.WriteLine("");
                    sr.WriteLine("");

                    // Fix suggested on forums by Spirit
                    // http://www.metabolt.net/metaforums/yaf_postsm2417_using-bat-file.aspx#post2417
                    line = "START \"\" /D \"" + Application.StartupPath + "\\\" \"" + Application.StartupPath + "\\metabolt.exe" + "\"" + " " + cuser.Replace("_", " ") + " " + txtPassword.Text;
                    sr.WriteLine(line);

                    sr.Close();
                    sr.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Login (create cmd file): " + ex.Message, Helpers.LogLevel.Error);
            }
        }

        #region IMETAboltTabControl Members

        public void RegisterTab(METAboltTab tab)
        {
            tab.DefaultControlButton = btnLogin;
        }

        #endregion


        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            netcom.LoginOptions.IsPasswordMD5 = false;
        }

        private void pnlLoginPage_Paint(object sender, PaintEventArgs e)
        {

        }

        private void MainConsole_Load(object sender, EventArgs e)
        {

        }

        private void DoBrowser()
        {
            //string lkey = instance.Config.CurrentConfig.AdRemove;

            //if (lkey != string.Empty)
            //{
            //    METAMD5 md5 = new METAMD5();

            //    if (md5.VerifyAdLicence(netcom.LoginOptions.FullName, client.Self.AgentID.ToString(), lkey))
            //    {
            //        murl = "http://www.metabolt.net/index.asp?user=login&nod=true&ver=" + Properties.Resources.METAboltVersion.ToString();
            //    }
            //    else
            //    {
            //        murl = "http://www.metabolt.net/index.asp?user=login&nod=false&ver=" + Properties.Resources.METAboltVersion.ToString();
            //    }
            //}
            //else
            //{
            //    murl = "http://www.metabolt.net/index.asp?user=login&nod=false&ver=" + Properties.Resources.METAboltVersion.ToString();
            //}

            ////murl = "http://www.metabolt.net/index.asp?user=none&ver=" + Properties.Resources.METAboltVersion.ToString();

            webBrowser.Refresh();
        }

        private void chkPWD_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbxGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxGrid.SelectedItem.ToString() == "Other...") //Custom option is selected
            {
                txtCustomLoginUri.Enabled = true;
                txtCustomLoginUri.Text = "http://";
                txtCustomLoginUri.Select();

                cbxGrid.Width = 157;
                button1.Visible = true;
                button2.Visible = true; 
            }
            else
            {
                txtCustomLoginUri.Enabled = false;
                txtCustomLoginUri.Text = string.Empty;

                cbxGrid.Width = 210;
                button1.Visible = false;
                button2.Visible = false; 
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            BeginLogin();
        }

        private void cbxLocation_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtFirstName_Enter(object sender, EventArgs e)
        {
            txtFirstName.SelectionStart = 0;
            txtFirstName.SelectionLength = txtFirstName.Text.Length;
            txtFirstName.Text = txtFirstName.SelectedText;
        }

        private void txtLastName_Enter(object sender, EventArgs e)
        {
            //txtLastName.SelectionStart = 0;
            //txtLastName.SelectionLength = txtLastName.Text.Length;
            //txtLastName.SelectedText = txtLastName.SelectedText;
            txtLastName.SelectAll();
        }

        private void txtFirstName_Click(object sender, EventArgs e)
        {
            txtFirstName.SelectAll();
        }

        private void txtLastName_Click(object sender, EventArgs e)
        {
            txtLastName.SelectAll();
        }

        private void txtPassword_Click(object sender, EventArgs e)
        {
            txtPassword.SelectAll();
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            txtPassword.SelectAll();
        }

        private void cboUserList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboUserList.SelectedIndex == -1) return;

                Item itm = (Item)cboUserList.SelectedItem;

                string[] name = itm.Name.ToString().Split(' ');  // cboUserList.SelectedItem.ToString().Split(' ');

                txtFirstName.Text = name[0];
                txtLastName.Text = name[1];
                txtPassword.Text = itm.Value;   // cboUserList.SelectedValue.ToString(); 

                txtPassword.Focus();
            }
            catch { ; }
        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fullfile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt\\Grids.txt"; ;

            try
            {
                System.Diagnostics.Process.Start(fullfile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "METAbolt");  
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadGrids();
            InitGridCombo();
            cbxGrid.SelectedIndex = 0; 
        }
    }
}
