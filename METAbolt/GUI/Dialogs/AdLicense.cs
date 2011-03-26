using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SLNetworkComm;
using OpenMetaverse;
using MD5library;
using System.Runtime.InteropServices;

namespace METAbolt
{
    public partial class AdLicense : Form
    {
        private METAboltInstance instance;
        private SLNetCom netcom;
        private GridClient client;
        private string licencetype = string.Empty;  

        public AdLicense(METAboltInstance instance)
        {
            InitializeComponent();

            this.instance = instance;
            client = this.instance.Client;
            netcom = this.instance.Netcom;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            METAMD5 md5 = new METAMD5();

            textBox1.Text = md5.GetMachinePassKey();
            licencetype = "machine";
            GenenerateEmail("machine");
            button3.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (netcom.IsLoggedIn)
            {
                METAMD5 md5 = new METAMD5();

                textBox1.Text = md5.GetAvatarKey(client.Self.FirstName + " " + client.Self.LastName, client.Self.AgentID.ToString());
                licencetype = "avatar";
                GenenerateEmail("avatar");
                button3.Enabled = true; 
            }
            else
            {
                MessageBox.Show("You need to be logged into SL to use this function.", "METAbolt Ad Licensing", MessageBoxButtons.OK, MessageBoxIcon.Information);   
            }
        }

        private void GenenerateEmail(string type)
        {
            string body = string.Empty;

            textBox2.Text = string.Empty;
            label5.Text = string.Empty;   
  
            if (type == "machine")
            {
                body = "SEND TO: metabolt@vistalogic.co.uk" + Environment.NewLine;
                body += Environment.NewLine;
                body += "\n[License application to remove METAbolt ads on this machine]" + Environment.NewLine;
                body += "Machine passkey: " + textBox1.Text + Environment.NewLine + Environment.NewLine;
            }
            else
            {
                body = "SEND TO: metabolt@vistalogic.co.uk" + Environment.NewLine;
                body += Environment.NewLine;
                body += "\n[License application to remove METAbolt ads for avatar]" + Environment.NewLine;
                body += Environment.NewLine;
                body += "Avatar passkey: " + textBox1.Text + Environment.NewLine;
                body += "License for avatar: " + client.Self.FirstName + " " + client.Self.LastName + Environment.NewLine + Environment.NewLine;
            }

            //body += "NOTE:" + Environment.NewLine;
            //body += "YOU MUST NOT CHANGE ANY OF THE INFORMATION ABOVE OR THE LICENSE KEY WE PROVIDE WILL NOT WORK. THE FEE YOU PAY IS NON REFUNDABLE ONCE WE HAVE ISSUED YOU YOUR KEY." + Environment.NewLine + Environment.NewLine + "NO PRIVATE INFORMATION IS COLLECTED OR STORED BY THE METABOLT ADMINISTRATION IN THIS PROCESS. THE ABOVE INFORMATION IS REQUIRED TO GENERATE THE LICENSE KEY TO BE PROVIDED FOR YOU TO ENTER INTO THE METABOLT APPLICATION. ONCE THE KEY IS PROVIDED IT WORKS LOCALLY ON YOUR MACHINE. NOTHING IS TRANSMITTED OR CHECKED EVER. LICENSE KEYS ARE NOT STORED ANYWHERE OTHER THAN YOUR MACHINE. METABOLT RECOMMENDS YOU KEEP A COPY OF LICENSE KEY/S IN A SAFE PLACE SO THAT YOU CAN REUSE IT IF YOU HAPPEN TO LOSE YOUR APPLICATION SETTINGS OR REMOVE METABOLT FROM YOUR MACHINE FOR WHATEVER THE REASON.";
            //body += Environment.NewLine + Environment.NewLine + "LICENSE PRICES ARE PUBLISHED ON THE METAFORUMS (WWW.METABOLT.NET/METAFORUMS/). ONCE YOU HAVE SENT YOUR EMAIL YOU WILL NEED TO MAKE THE LICENSE PAYMENT AS DESCRIBED. AFTER YOUR PAYMENT IS RECEIVED PLEASE ALLOW UP TO 24 HOURS TO RECEIVE YOUR LICENSE KEY. YOUR LICENSE KEY WILL BE EMAILED TO THE ADDRESS YOU HAVE EMAILED YOUR APPLICATION FROM.";

            textBox2.Text = body;
            label5.Text = "Copy and email to metabolt@vistalogic.co.uk as is.";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox2.Text);
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr ShellExecute(IntPtr hwnd,
                                          string lpOperation,
                                          string lpFile,
                                          string lpParameters,
                                          string lpDirectory,
                                          int nShowCmd
                                          );

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShellExecute(this.Handle, "open", "http://www.metabolt.net/metawiki/Ad-Removal-License.ashx?NoRedirect=1", null, null, 0);
        }

        private void AdLicense_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
        }
    }
}
