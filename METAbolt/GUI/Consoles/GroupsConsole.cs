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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
using SLNetworkComm;
using OpenMetaverse.Packets;
using ExceptionReporting;
using System.Threading;
using System.Linq;  


// Group List user control
// Added by Legoals Luke


namespace METAbolt
{
    public partial class GroupsConsole : UserControl
    {
        private METAboltInstance instance;
        private GridClient Client;
        private TabsConsole tabConsole;
        //GroupManager.GroupJoinedCallback gcallback;
        //GroupManager.GroupLeftCallback gleftcall; 
        private ExceptionReporter reporter = new ExceptionReporter();

        internal class ThreadExceptionHandler
        {
            public void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
            {
                ExceptionReporter reporter = new ExceptionReporter();
                reporter.Show(e.Exception);
            }
        }

        public GroupsConsole(METAboltInstance instance)
        {
            InitializeComponent();

            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.instance = instance;
            Client = this.instance.Client;

            Client.Groups.CurrentGroups += new EventHandler<CurrentGroupsEventArgs>(Groups_OnCurrentGroups);
            Client.Groups.RequestCurrentGroups();

            Client.Groups.GroupJoinedReply += new EventHandler<GroupOperationEventArgs>(Groups_OnGroupStateChanged);
            Client.Groups.GroupLeaveReply += new EventHandler<GroupOperationEventArgs>(Groups_OnGroupStateChanged);

            Disposed += new EventHandler(GroupsConsole_Disposed);
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

        public void GroupsConsole_Disposed(object sender, EventArgs e)
        {
            Client.Groups.CurrentGroups -= new EventHandler<CurrentGroupsEventArgs>(Groups_OnCurrentGroups);
            Client.Groups.GroupJoinedReply -= new EventHandler<GroupOperationEventArgs>(Groups_OnGroupStateChanged);
            Client.Groups.GroupLeaveReply -= new EventHandler<GroupOperationEventArgs>(Groups_OnGroupStateChanged);

        }

        private void UpdateGroups()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    UpdateGroups();
                }));

                return;
            }

            try
            {
                lstGroups.Items.Clear();

                lstGroups.Items.Add("_None");

                foreach (Group group in this.instance.State.Groups.Values)
                {
                    lstGroups.Items.Add(group);

                    if (Client.Self.ActiveGroup != UUID.Zero)
                    {
                        if (Client.Self.ActiveGroup == group.ID)
                        {
                            label1.Text = "Current group tag worn: " + group.Name;
                        }
                    }
                    else
                    {
                        label1.Text = "Current group tag worn: None";
                    }
                }

                lstGroups.Sorted = true;

                if (lstGroups.Items.Count > 0)
                {
                    int cnt = lstGroups.Items.Count - 1;
                    label6.Text = "Total: " + cnt + " groups";
                }
                else
                {
                    label6.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Groups Console error: " + ex.Message, Helpers.LogLevel.Error); 
            }
        }

   

        #region GUI Callbacks

        private void lstGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstGroups.SelectedIndex >= 0)
            {
                if ((string)lstGroups.Items[lstGroups.SelectedIndex].ToString() != "_None")
                {
                    cmdActivate.Enabled = cmdInfo.Enabled = cmdIM.Enabled = button4.Enabled = cmdLeave.Enabled = true;
                    label5.Text = "Group UUID: " + ((Group)lstGroups.Items[lstGroups.SelectedIndex]).ID.ToString();
                }
                else
                {
                    cmdActivate.Enabled = true;
                    cmdInfo.Enabled = button4.Enabled = cmdIM.Enabled = cmdLeave.Enabled = false;
                    label5.Text = string.Empty;
                }
            }
            else
            {
                cmdActivate.Enabled = cmdInfo.Enabled = cmdIM.Enabled = button4.Enabled = cmdLeave.Enabled = false;
                label5.Text = string.Empty;
            }
        }


        #endregion GUI Callbacks

        #region Network Callbacks

        private void Groups_OnCurrentGroups(object sender, CurrentGroupsEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    Groups_OnCurrentGroups(sender, e);
                }));

                return;
            }

            this.BeginInvoke(new MethodInvoker(delegate()
            {
                UpdateGroups();
            }));            
        }

        #endregion     

        private void GroupsConsole_Load(object sender, EventArgs e)
        {
            tabConsole = instance.TabConsole;
        }

        private void cmdInfo_Click_1(object sender, EventArgs e)
        {
            if (lstGroups.SelectedIndex >= 0 && lstGroups.Items[lstGroups.SelectedIndex].ToString() != "_None")
            {
                if (lstGroups.Items[lstGroups.SelectedIndex].ToString()  != "_None")
                {
                    Group group = (Group)lstGroups.Items[lstGroups.SelectedIndex];

                    //frmGroupInfo frm = new frmGroupInfo(group, instance);
                    //frm.ShowDialog();
                    (new frmGroupInfo(group, instance)).Show();
                }

                lstGroups.SetSelected(lstGroups.SelectedIndex, true);
            }
        }

        private void cmdActivate_Click(object sender, EventArgs e)
        {
            if (lstGroups.SelectedIndex >= 0)
            {
                if (lstGroups.Items[lstGroups.SelectedIndex].ToString() == "_None")
                {
                    Client.Groups.ActivateGroup(UUID.Zero);
                }
                else
                {
                    Group group = (Group)lstGroups.Items[lstGroups.SelectedIndex];
                    Client.Groups.ActivateGroup(group.ID);
                }
            }

            lstGroups.SetSelected(lstGroups.SelectedIndex, true);
        }

        private void cmdLeave_Click(object sender, EventArgs e)
        {
            if (lstGroups.SelectedIndex >= 0 && lstGroups.Items[lstGroups.SelectedIndex].ToString() != "_None")
            {
                DialogResult res = MessageBox.Show("Are you sure you want to LEAVE this Group?", "METAbolt", MessageBoxButtons.YesNo);

                if (res == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                Group group = (Group)lstGroups.Items[lstGroups.SelectedIndex];
                Client.Groups.LeaveGroup(group.ID);
            }
        }

        private void Groups_OnGroupStateChanged(object sender, GroupOperationEventArgs e)
        {
            Client.Groups.RequestCurrentGroups();
        }

        //private void Groups_OnGroupLeft(object sender, GroupOperationEventArgs e)
        //{
        //    Client.Groups.RequestCurrentGroups();
        //}

        private void cmdCreate_Click(object sender, EventArgs e)
        {
            if (lstGroups.SelectedIndex >= 0 && lstGroups.Items[lstGroups.SelectedIndex].ToString() != "_None")
            {
                if (lstGroups.Items[lstGroups.SelectedIndex].ToString() != "_None")
                {
                    Group group = (Group)lstGroups.Items[lstGroups.SelectedIndex];

                    if (tabConsole.TabExists(group.Name))
                    {
                        tabConsole.SelectTab(group.Name);
                        return;
                    }

                    tabConsole.AddIMTabGroup(group.ID, group.ID, group.Name, group);
                    tabConsole.SelectTab(group.Name);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cmdIM.Enabled = cmdActivate.Enabled = cmdInfo.Enabled = button4.Enabled = cmdLeave.Enabled = button1.Enabled = label5.Visible = false;
            panel1.Visible = true; 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            EnableNew();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Group newgroup = new Group();
            newgroup.Name = textBox1.Text;
            newgroup.Charter = textBox2.Text;
            newgroup.FounderID = Client.Self.AgentID;
            Client.Groups.RequestCreateGroup(newgroup);

            EnableNew();
        }

        private void EnableNew()
        {
            panel1.Visible = false;
            cmdIM.Enabled = cmdActivate.Enabled = cmdInfo.Enabled = button4.Enabled = cmdLeave.Enabled = button1.Enabled = label5.Visible = true; 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (lstGroups.SelectedIndex >= 0 && lstGroups.Items[lstGroups.SelectedIndex].ToString() != "_None")
            {
                Group group = (Group)lstGroups.Items[lstGroups.SelectedIndex];

                (new frmGive(instance, group.ID, UUID.Zero)).Show(this);

                lstGroups.SetSelected(lstGroups.SelectedIndex, true);
            }
        }
    }
}