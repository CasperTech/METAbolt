//  Copyright (c) 2008-2011, www.metabolt.net
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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Web.UI.WebControls;
using System.Diagnostics;
using IWshRuntimeLibrary;
 

namespace METAbolt
{
    public partial class frmBackup : Form
    {
        string currentDirectory =string.Empty;
        //string destinationDirectory = string.Empty;

        public frmBackup()
        {
            InitializeComponent();
        }

        private void frmBackup_Load(object sender, EventArgs e)
        {
            this.CenterToParent();

            label2.Text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt";    //Application.StartupPath.ToString();
            label8.Text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt";    //Application.StartupPath.ToString();

            currentDirectory = @label2.Text;
            currentDirectory += "\\";

            DirectoryInfo dir = new  DirectoryInfo(currentDirectory);

            FileInfo[] rgFiles = dir.GetFiles("*.cmd");

            foreach (FileInfo fi in rgFiles)
            {
                listBox1.Items.Add(fi.Name);  
            }

            rgFiles = dir.GetFiles("*.bat");

            foreach (FileInfo fi in rgFiles)
            {
                listBox1.Items.Add(fi.Name);
            }

            rgFiles = dir.GetFiles("*.ini");

            foreach (FileInfo fi in rgFiles)
            {
                listBox1.Items.Add(fi.Name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label5.Text = string.Empty;

            DialogResult result = this.folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = this.folderBrowser.SelectedPath;
                textBox1.Text += @"\"; 
                button2.Enabled = true;
                button4.Enabled = true;   
            }
            else
            {
                button2.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("You must select a destination folder first", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; 
            }

            string filename = string.Empty;
            string destFile = string.Empty; 

            for (int a = 0; a < listBox1.Items.Count; a++)
            {
                filename = currentDirectory + listBox1.Items[a].ToString();
                destFile = @textBox1.Text + listBox1.Items[a].ToString();

                try
                {
                    System.IO.File.Copy(filename, destFile, true);
                }
                catch (Exception ex)
                {
                    string nexp = ex.InnerException.ToString();
                    label5.Text = nexp; 
                    return; 
                }
            }

            listBox1.Items.Clear();   
            label5.Text = "Backup/s completed to destination folder.";
            //button4.Enabled = true;  
        }

        private void CreateBatFile()
        {
            string cuser = "METAbolt";
            string textfile = cuser + ".bat";
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt", textfile);
            string scfile = "METAbolt BAT.lnk";
            string sc = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), scfile);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            using (StreamWriter sr = System.IO.File.CreateText(path))
            {
                string line = "@ECHO OFF";
                sr.Write(line);
                sr.WriteLine("");
                sr.WriteLine("");
                sr.WriteLine("METAbolt.exe");

                //for (int a = 0; a < files.Length; a++)
                //{
                //    if (files[a] != null)
                //    {
                //        sr.WriteLine(files[a]);
                //        sr.WriteLine("");
                //    }
                //}

                sr.Close();
                sr.Dispose();
            }

            //// now create desktop shortcut
            //WshShell shell = new WshShell();
            //IWshShortcut link = (IWshShortcut)shell.CreateShortcut(sc);
            //link.TargetPath = path;
            //link.Description = "Start multiple instances";
            ////link.IconLocation = Environment.CurrentDirectory + ""; 
            //link.Save();

            CreateBatFileShortcut(path, sc);
        }

        private void CreateBatFileShortcut(string path, string scp)
        {
            ////string cuser = "METAbolt";
            //string textfile = cuser + ".bat";
            //string path = Path.Combine(Environment.CurrentDirectory, textfile);
            //string scfile = "METAbolt- " + cuser + " BAT.lnk";
            string sc = scp;   // Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), scfile);

            // now create desktop shortcut
            WshShell shell = new WshShell();
            IWshShortcut link = (IWshShortcut)shell.CreateShortcut(sc);
            link.TargetPath = path;
            link.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt";    //Application.StartupPath.ToString();
            link.Description = "METAbolt BAT shortcut";
            //link.IconLocation = Environment.CurrentDirectory + ""; 
            link.Save();
        }

        private void CreateBatFileShortcut(string path, string scp, string fname)
        {
            ////string cuser = "METAbolt";
            //string textfile = cuser + ".bat";
            //string path = Path.Combine(Environment.CurrentDirectory, textfile);
            //string scfile = "METAbolt- " + cuser + " BAT.lnk";
            string sc = scp;   // Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), scfile);

            // now create desktop shortcut
            WshShell shell = new WshShell();
            IWshShortcut link = (IWshShortcut)shell.CreateShortcut(sc);
            link.TargetPath = path;
            link.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt";    //Application.StartupPath.ToString(); 
            link.Description = "METAbolt_" + fname + " BAT shortcut";
            //link.IconLocation = Environment.CurrentDirectory + ""; 
            link.Save();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();  
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", textBox1.Text); 
        }

        private void button7_Click(object sender, EventArgs e)
        {
            label5.Text = string.Empty;
  
            DialogResult result = this.folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox2.Text = this.folderBrowser.SelectedPath;
                textBox2.Text += "\\";
                button6.Enabled = true;
                button5.Enabled = true;  

                SetRestoreDirectory();
            }
            else
            {
                button6.Enabled = false;
                textBox2.Text = "[ select source folder. Click on SELECT button ]";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("You must select a source folder first", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string filename = string.Empty;
            string destFile = string.Empty;
            string[] cmdfiles = new string[listBox2.Items.Count];

            for (int a = 0; a < listBox2.Items.Count; a++)
            {
                filename = @textBox2.Text + listBox2.Items[a].ToString();
                destFile = currentDirectory + listBox2.Items[a].ToString();

                try
                {
                    System.IO.File.Copy(filename, destFile, true);

                    if (destFile.EndsWith(".cmd") || destFile.EndsWith(".bat"))
                    {
                        cmdfiles[a] = "CALL " + destFile;

                        if (checkBox1.Checked)
                        {
                            string fname = listBox2.Items[a].ToString();
                            char[] deli = ".".ToCharArray();

                            string[] names = fname.Split(deli);

                            string cuser = names[0];
                            string textfile = cuser + ".bat";
                            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt", textfile);
                            string scfile = "METAbolt_" + cuser + " BAT.lnk";
                            string sc = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), scfile);

                            CreateBatFileShortcut(path, sc, cuser);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string nexp = ex.Message.ToString();
                    label5.Text = nexp;
                    return;
                }
            }

            if (checkBox2.Checked)
            {
                CreateBatFile();
            }

            listBox2.Items.Clear();
            label5.Text = "Restore complete.";
            //button5.Enabled = true;  
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                listBox1.Items.Clear();

                currentDirectory = @label2.Text;
                currentDirectory += "\\";

                DirectoryInfo dir = new DirectoryInfo(currentDirectory);

                FileInfo[] rgFiles = dir.GetFiles("*.cmd");

                foreach (FileInfo fi in rgFiles)
                {
                    listBox1.Items.Add(fi.Name);
                }

                rgFiles = dir.GetFiles("*.bat");

                foreach (FileInfo fi in rgFiles)
                {
                    listBox1.Items.Add(fi.Name);
                }

                rgFiles = dir.GetFiles("*.ini");

                foreach (FileInfo fi in rgFiles)
                {
                    listBox1.Items.Add(fi.Name);
                }

                label5.Text = string.Empty;
                //button4.Enabled = false; 
            }
            else
            {
                if (textBox2.Text != "[ select source folder. Click on SELECT button ]")
                {
                    SetRestoreDirectory();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", label8.Text);
        }

        private void SetRestoreDirectory()
        {
            listBox2.Items.Clear();  
            
            string restoreDirectory = @textBox2.Text;

            DirectoryInfo dir = new DirectoryInfo(restoreDirectory);

            FileInfo[] rgFiles = dir.GetFiles("*.cmd");

            foreach (FileInfo fi in rgFiles)
            {
                listBox2.Items.Add(fi.Name);
            }

            rgFiles = dir.GetFiles("*.bat");

            foreach (FileInfo fi in rgFiles)
            {
                listBox2.Items.Add(fi.Name);
            }

            rgFiles = dir.GetFiles("*.ini");

            foreach (FileInfo fi in rgFiles)
            {
                listBox2.Items.Add(fi.Name);
            }

            label5.Text = string.Empty;
            //button5.Enabled = false; 
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
