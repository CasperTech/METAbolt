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
using OpenMetaverse;
using OpenMetaverse.Packets;
using PopupControl;

namespace METAbolt
{
    public partial class frmStats : Form
    {
        private METAboltInstance instance;
        private GridClient client;
        private Simulator sim;
        private int score = 10;
        private Popup toolTip;
        private CustomToolTip customToolTip;

        public frmStats(METAboltInstance instance)
        {
            InitializeComponent();

            string msg1 = "Click for online help/guidance";
            toolTip = new Popup(customToolTip = new CustomToolTip(instance, msg1));
            toolTip.AutoClose = false;
            toolTip.FocusOnOpen = false;
            toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;

            this.instance = instance;
            client = this.instance.Client;

            sim = client.Network.CurrentSim;

            client.Network.SimChanged += new EventHandler<SimChangedEventArgs>(Network_SimChanged);
        }

        void Network_SimChanged(object sender, SimChangedEventArgs e)
        {
            sim = client.Network.CurrentSim;
        }

        private void frmStats_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            
            GetStats();
        }

        private void GetStats()
        {
            lbIssues.Items.Clear();

            try
            {
                label1.Text = "Name: " + sim.ToString();

                label22.Text = "Version: " + sim.SimVersion;
                label23.Text = "Location: " + sim.Name;

                label2.Text = "Dilation: " + sim.Stats.Dilation.ToString();
                progressBar1.Value = (int)(sim.Stats.Dilation * 10);

                label24.Text = sim.Stats.INPPS.ToString();
                label25.Text = sim.Stats.OUTPPS.ToString();

                label26.Text = sim.Stats.ResentPackets.ToString();
                label27.Text = sim.Stats.ReceivedResends.ToString();

                label28.Text = client.Network.InboxCount.ToString();

                label7.Text = "FPS: " + sim.Stats.FPS.ToString();
                progressBar7.Value = (int)sim.Stats.FPS;

                label8.Text = "Physics FPS: " + sim.Stats.PhysicsFPS.ToString();
                progressBar8.Value = (int)sim.Stats.PhysicsFPS;

                label29.Text = sim.Stats.AgentUpdates.ToString();

                label10.Text = "Objects: " + sim.Stats.Objects.ToString();
                progressBar10.Value = (int)sim.Stats.Objects;

                label30.Text = sim.Stats.ScriptedObjects.ToString();

                label12.Text = "Frame Time: " + sim.Stats.FrameTime.ToString();
                progressBar15.Value = (int)sim.Stats.FrameTime;

                label34.Text = sim.Stats.NetTime.ToString();

                label35.Text = sim.Stats.ImageTime.ToString();

                label36.Text = sim.Stats.PhysicsTime.ToString();

                label37.Text = sim.Stats.ScriptTime.ToString();

                label38.Text = sim.Stats.OtherTime.ToString();

                label32.Text = sim.Stats.Agents.ToString();

                label33.Text = sim.Stats.ChildAgents.ToString();

                label31.Text = sim.Stats.ActiveScripts.ToString();

                ScorePerformance();
            }
            catch
            {
                ;
            }
        }

        private void ScorePerformance()
        {
            float dil = sim.Stats.Dilation;
            int fm = sim.Stats.FPS;

            if (sim.Stats.PendingDownloads > 1)
            {
                lbIssues.Items.Add("Expect rezzing issues and delays in viewing notecards/scripts.");
            }

            if (sim.Stats.PendingUploads > 0)
            {
                lbIssues.Items.Add("Expect teleport issues.");
            }

            if (sim.Stats.PhysicsFPS > 5000)
            {
                lbIssues.Items.Add("SIM wide physics issues");
            }

            if (dil > 0.94)
            {
                //lblScore.Text = "Excellent";
                // Excellent
                if (fm > 29)
                {
                    // Excellent
                    //lblScore.Text = "Excellent";
                    lblScore.ForeColor = Color.Green;
                    score = 5;
                }
                else if (fm > 14 && fm < 30)
                {
                    // Good
                    //lblScore.Text = "Good";
                    lblScore.ForeColor = Color.RoyalBlue;
                    score = 3;
                }
                else
                {
                    // Poor
                    //lblScore.Text = "Poor";
                    lbIssues.Items.Add("Physics running-speed is almost zero i.e. barely running");
                    lblScore.ForeColor = Color.Red;
                    score = 1;
                }
            }
            else if (dil < 0.95 && dil > 0.5)
            {
                lbIssues.Items.Add("Physics is running at half speed.");

                // Good
                if (fm > 29)
                {
                    // Excellent
                    //lblScore.Text = "Good";
                    lblScore.ForeColor = Color.RoyalBlue;
                    score = 3;
                }
                else if (fm > 14 && fm < 30)
                {
                    lbIssues.Items.Add("Script running speed is reduced due to low dilation.");
                    // Good
                    //lblScore.Text = "Average";
                    lblScore.ForeColor = Color.Black;
                    score = 2;
                }
                else
                {
                    // Poor
                    //lblScore.Text = "Very Poor";
                    
                    lblScore.ForeColor = Color.Red;
                    score = 1;
                }
            }
            else
            {
                // Poor
                //lblScore.Text = "Extremely Poor";
                lbIssues.Items.Add("Physics running-speed is almost zero i.e. barely running");
                lblScore.ForeColor = Color.Red;
                score = 1;
            }

            score += CalcFT();

            pbScore.Value = score; 
            lblScore.Text = score.ToString();
        }

        private int CalcFT()
        {
            float ft = sim.Stats.FrameTime;

            if (ft < 22.1)
            {
                return 5;
            }
            else if (ft > 22.1 && ft < 22.5)
            {
                lbIssues.Items.Add("Healthy SIM but too many scripts/agents is causing script execution slow-down.");   
                return 3;
            }
            else
            {
                lbIssues.Items.Add("There is a severe load on the SIM due to physics or too many agents. Expect lag.");
                return 1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GetStats();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void progressBar3_Click(object sender, EventArgs e)
        {

        }

        private void progressBar9_Click(object sender, EventArgs e)
        {

        }

        private void pbScore_Click(object sender, EventArgs e)
        {

        }

        private void pbHelp_MouseHover(object sender, EventArgs e)
        {
            toolTip.Show(pbHelp);
        }

        private void pbHelp_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Close();
        }

        private void pbHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://wiki.secondlife.com/wiki/Statistics_Bar_Guide");
        }
    }
}
