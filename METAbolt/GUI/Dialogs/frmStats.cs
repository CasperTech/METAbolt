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

namespace METAbolt
{
    public partial class frmStats : Form
    {
        private METAboltInstance instance;
        //private SLNetCom netcom;
        private GridClient client;

        public frmStats(METAboltInstance instance)
        {
            InitializeComponent();

            this.instance = instance;
            client = this.instance.Client;
            //netcom = this.instance.Netcom;
        }

        private void frmStats_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            
            GetStats();
        }

        private void GetStats()
        {
            //StringBuilder output = new StringBuilder();

            try
            {
            lock (client.Network.Simulators)
            {
                Simulator sim = client.Network.Simulators[0];
                label1.Text = "Name: " + sim.ToString();

                label22.Text = "Version: " + sim.SimVersion;
                label23.Text = "Location: " + sim.ColoLocation; 
                
                label2.Text = "Dilation: " + sim.Stats.Dilation;
                progressBar1.Value = (int)(sim.Stats.Dilation * 10);
 
                label3.Text = "In BPS: " + sim.Stats.IncomingBPS;
                if ((int)sim.Stats.IncomingBPS > progressBar2.Maximum)
                    progressBar2.Maximum = (int)sim.Stats.IncomingBPS;
                progressBar2.Value = (int)sim.Stats.IncomingBPS;

                label4.Text = "Out BPS: " + sim.Stats.OutgoingBPS;
                if ((int)sim.Stats.OutgoingBPS > progressBar3.Maximum)
                    progressBar3.Maximum = (int)sim.Stats.OutgoingBPS;
                progressBar3.Value = (int)sim.Stats.OutgoingBPS;

                label5.Text = "Resent Packets: " + sim.Stats.ResentPackets;
                if ((int)sim.Stats.ResentPackets > progressBar4.Maximum)
                    progressBar4.Maximum = (int)sim.Stats.ResentPackets;
                progressBar4.Value = (int)sim.Stats.ResentPackets;

                label6.Text = "Received Resends: " + sim.Stats.ReceivedResends;
                if ((int)sim.Stats.ReceivedResends > progressBar5.Maximum)
                    progressBar5.Maximum = (int)sim.Stats.ReceivedResends;
                progressBar5.Value = (int)sim.Stats.ReceivedResends;
            }

            Simulator csim = client.Network.CurrentSim;
            label21.Text = "Packets in queue: " + client.Network.InboxCount;
            if (client.Network.InboxCount > progressBar6.Maximum)
                progressBar6.Maximum = client.Network.InboxCount;
            progressBar6.Value = (int)client.Network.InboxCount;

            label7.Text = "FPS: " + csim.Stats.FPS;
            progressBar7.Value = (int)csim.Stats.FPS;

            label8.Text = "Physics FPS: " + csim.Stats.PhysicsFPS;
            progressBar8.Value = (int)csim.Stats.PhysicsFPS;

            label9.Text = "Agent Updates: " + csim.Stats.AgentUpdates;
            if ((int)csim.Stats.AgentUpdates > progressBar9.Maximum)
                progressBar9.Maximum = (int)csim.Stats.AgentUpdates;
            progressBar9.Value = (int)csim.Stats.AgentUpdates;

            label10.Text = "Objects: " + csim.Stats.Objects;
            progressBar10.Value = (int)csim.Stats.Objects;

            label11.Text = "Scripted Objects: " + csim.Stats.ScriptedObjects;
            progressBar11.Value = (int)csim.Stats.ScriptedObjects;

            label12.Text = "Frame Time: " + csim.Stats.FrameTime;
            progressBar15.Value = (int)csim.Stats.FrameTime;

            label13.Text = "Net Time: " + csim.Stats.NetTime;
            progressBar16.Value = (int)csim.Stats.NetTime;

            label14.Text = "Image Time" + csim.Stats.ImageTime;
            progressBar17.Value = (int)csim.Stats.ImageTime;

            label15.Text = "Physics Time: " + csim.Stats.PhysicsTime;
            progressBar18.Value = (int)csim.Stats.PhysicsTime;

            label16.Text = "Script Time: " + csim.Stats.ScriptTime;
            progressBar19.Value = (int)csim.Stats.ScriptTime;

            label17.Text = "Other Time" + csim.Stats.OtherTime;
            progressBar20.Value = (int)csim.Stats.OtherTime;

            label18.Text = "Agents: " + csim.Stats.Agents;
            progressBar13.Value = (int)csim.Stats.Agents;

            label19.Text = "Child Agents: " + csim.Stats.ChildAgents;
            progressBar14.Value = (int)csim.Stats.ChildAgents;

            label20.Text = "Active Scripts" + csim.Stats.ActiveScripts;
            progressBar12.Value = (int)csim.Stats.ActiveScripts;

            }
            catch
            {
                ;
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
    }
}
