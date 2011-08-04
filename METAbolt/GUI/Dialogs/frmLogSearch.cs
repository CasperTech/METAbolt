//  Copyright (c) 2008 - 2011, www.metabolt.net (METAbolt)
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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Threading;
using ExceptionReporting;
using System.Globalization;


namespace METAbolt
{
    public partial class frmLogSearch : Form
    {

        private METAboltInstance instance;
        private List<string> LogFiles;
        private List<string> FoundFiles;
        private string LogPath = string.Empty;
        private string filetype = "ALL";

        private ExceptionReporter reporter = new ExceptionReporter();

        internal class ThreadExceptionHandler
        {
            public void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
            {
                ExceptionReporter reporter = new ExceptionReporter();
                reporter.Show(e.Exception);
            }
        }

        public frmLogSearch(METAboltInstance instance)
        {
            InitializeComponent();

            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.instance = instance;
            LogFiles = new List<string>();
            FoundFiles = new List<string>();

            LogPath = instance.Config.CurrentConfig.LogDir;
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

        private void frmLogSearch_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            GetLogFiles(filetype);
            textBox1.Focus();
        }

        private void GetLogFiles(string type)
        {

            DirectoryInfo di = new DirectoryInfo(LogPath);
            FileInfo[] files = di.GetFiles();

            Array.Sort(files, CompareFileByDate);
            Array.Reverse(files);   // Descending 

            listBox1.Items.Clear();
            LogFiles.Clear();

            foreach (FileInfo fi in di.GetFiles())
            {
                string inFile = fi.FullName;
                string finname = fi.Name;

                if (filetype != "ALL")
                {
                    if (finname.ToUpper(CultureInfo.CurrentCulture).StartsWith(filetype, StringComparison.CurrentCulture))
                    {
                        LogFiles.Add(inFile);
                        listBox1.Items.Add(finname);
                    }
                }
                else
                {
                    LogFiles.Add(inFile);
                    listBox1.Items.Add(finname);
                }
            }

            label3.Text = "Total " + listBox1.Items.Count.ToString(CultureInfo.CurrentCulture) + " files.";   
        }

        private static int CompareFileByDate(FileSystemInfo f1, FileSystemInfo f2)
        {
            return DateTime.Compare(f1.LastWriteTime, f2.LastWriteTime);
        }

        private void FindText(string fName)
        {
            string[] s_arr = Regex.Split(fName, @"(\\)");
            string name = s_arr[s_arr.Length - 1];   

            StreamReader testTxt = new StreamReader(fName);
            string allRead = testTxt.ReadToEnd().ToLower(CultureInfo.CurrentCulture);
            testTxt.Close();

            string regMatch = textBox1.Text.ToLower(CultureInfo.CurrentCulture); 

            if (Regex.IsMatch(allRead, regMatch))
            {
                FoundFiles.Add(name); 
            }

            testTxt.Dispose();  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("You must enter a search term first.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  
                return;
            }

            listBox2.Items.Clear();
            FoundFiles.Clear();
            label4.Text = string.Empty;

            // Iterate log files here
            foreach (string term in LogFiles)
            {
                FindText(term);
            }

            if (FoundFiles.Count > 0)
            {
                foreach (string term in FoundFiles)
                {
                    listBox2.Items.Add(term);  
                }

                label4.Text = "Search term found in " + FoundFiles.Count.ToString(CultureInfo.CurrentCulture) + " files:";  
                //button2.Enabled = button3.Enabled = true;

                if (FoundFiles.Count > 1)
                {
                    button3.Enabled = true;
                }
            }
            else
            {
                label4.Text = "Zero results found";  
                button2.Enabled = button3.Enabled = false;
            }   
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string fullfile = LogPath + listBox1.Items[listBox1.SelectedIndex];

                if (checkBox1.Checked)
                {
                    System.Diagnostics.Process.Start(fullfile);
                }
                else
                {
                    (new frmNotecard(instance, fullfile, textBox1.Text)).Show();
                }
            }
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                string fullfile = LogPath + listBox2.Items[listBox2.SelectedIndex];

                if (checkBox1.Checked)
                {
                    System.Diagnostics.Process.Start(fullfile);
                }
                else
                {
                    (new frmNotecard(instance, fullfile, textBox1.Text)).Show();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                string fullfile = LogPath + listBox2.Items[listBox2.SelectedIndex];

                if (checkBox1.Checked)
                {
                    System.Diagnostics.Process.Start(fullfile);
                }
                else
                {
                    (new frmNotecard(instance, fullfile, textBox1.Text)).Show();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (string fnd in FoundFiles)
            {
                string fullfile = LogPath + fnd;

                if (checkBox1.Checked)
                {
                    System.Diagnostics.Process.Start(fullfile);
                }
                else
                {
                    (new frmNotecard(instance, fullfile, textBox1.Text)).Show();
                }
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItems.Count > 0)
            {
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.SelectionStart = 0;
            textBox1.SelectionLength = textBox1.Text.Length;
            textBox1.SelectAll(); 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", instance.Config.CurrentConfig.LogDir);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            filetype = "ALL";
            GetLogFiles(filetype);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            filetype = "CHAT";
            GetLogFiles(filetype);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            filetype = "IM";
            GetLogFiles(filetype);
        }
    }
}
