using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
using SLNetworkComm;
using ExceptionReporting;
using System.Threading;
using PopupControl;
using System.Globalization;
using System.Text.RegularExpressions;

namespace METAbolt
{
    public partial class IMbox : UserControl
    {
        private METAboltInstance instance;
        //private GridClient client;
        private SLNetCom netcom;
        private TabsConsole tabsconsole;
        private Popup toolTip;
        private CustomToolTip customToolTip;

        private ExceptionReporter reporter = new ExceptionReporter();

        internal class ThreadExceptionHandler
        {
            public void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
            {
                ExceptionReporter reporter = new ExceptionReporter();
                reporter.Show(e.Exception);
            }
        }

        public IMbox(METAboltInstance instance)
        {
            InitializeComponent();

            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.instance = instance;
            //client = this.instance.Client;
            netcom = this.instance.Netcom;

            string msg1 = "To view IMs, double click on an IM session from the list.\nWhen the IMbox tab turns BLUE it means there is a new IM.\nThis tab can be detached from the 'PC' icon on the right.";
            toolTip = new Popup(customToolTip = new CustomToolTip(instance, msg1));
            toolTip.AutoClose = false;
            toolTip.FocusOnOpen = false;
            toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;

            tabsconsole = instance.TabConsole;
            this.instance.imBox = this;

            Disposed += new EventHandler(IMbox_Disposed);

            netcom.InstantMessageReceived += new EventHandler<InstantMessageEventArgs>(netcom_InstantMessageReceived);
            this.instance.Config.ConfigApplied += new EventHandler<ConfigAppliedEventArgs>(Config_ConfigApplied);

            label5.Text = this.instance.Config.CurrentConfig.BusyReply;

            label6.Text = this.instance.Config.CurrentConfig.InitialIMReply;
            label8.Visible = this.instance.Config.CurrentConfig.DisableGroupIMs;
            label9.Visible = this.instance.Config.CurrentConfig.ReplyAI;

            if (string.IsNullOrEmpty(label5.Text))
            {
                label5.Text = "<empty>";
            }

            if (string.IsNullOrEmpty(label6.Text))
            {
                label6.Text = "<empty>";
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

        public void IMbox_Disposed(object sender, EventArgs e)
        {
            netcom.InstantMessageReceived -= new EventHandler<InstantMessageEventArgs>(netcom_InstantMessageReceived);
            this.instance.Config.ConfigApplied -= new EventHandler<ConfigAppliedEventArgs>(Config_ConfigApplied);
        }

        private void Config_ConfigApplied(object sender, ConfigAppliedEventArgs e)
        {
            //update with new config
            label5.Text = e.AppliedConfig.BusyReply;

            label6.Text = e.AppliedConfig.InitialIMReply;
            label8.Visible = e.AppliedConfig.DisableGroupIMs;
            label9.Visible = e.AppliedConfig.ReplyAI;

            if (string.IsNullOrEmpty(label5.Text))
            {
                label5.Text = "<empty>";
            }

            if (string.IsNullOrEmpty(label6.Text))
            {
                label6.Text = "<empty>";
            }
        }

        private void netcom_InstantMessageReceived(object sender, InstantMessageEventArgs e)
        {
            if (instance.IsAvatarMuted(e.IM.FromAgentID))
                return;

            if (tabsconsole.tabs.ContainsKey(e.IM.FromAgentName.ToLower()))
            {
                if (tabsconsole.tabs[e.IM.FromAgentName.ToLower()].Selected)
                {
                    return;
                }
            }

            switch (e.IM.Dialog)
            {
                case InstantMessageDialog.MessageFromAgent:
                    if (e.IM.FromAgentName.ToLower() == "second life")
                    {
                        return;
                    }

                    HandleIM(e);
                    break;
                case InstantMessageDialog.SessionSend:
                    HandleIM(e);
                    break;
                case InstantMessageDialog.StartTyping:
                    return;
                case InstantMessageDialog.StopTyping:
                    return;
            }
        }

        private void HandleIM(InstantMessageEventArgs e)
        {
            //if (e.IM.Dialog == InstantMessageDialog.SessionSend)
            //{
            //    // new IM
            //}

            string TabAgentName = string.Empty;

            if (this.instance.State.GroupStore.ContainsKey(e.IM.IMSessionID))
            {
                // Check to see if group IMs are disabled
                if (instance.Config.CurrentConfig.DisableGroupIMs) return;

                TabAgentName = this.instance.State.GroupStore[e.IM.IMSessionID];
            }
            else
            {
                TabAgentName = e.IM.FromAgentName;
            }

            int s = lbxIMs.FindString(TabAgentName);

            if (s == -1)
            {
                lbxIMs.BeginUpdate();
                lbxIMs.Items.Add(TabAgentName + " (1)");
                lbxIMs.EndUpdate();
            }
            else
            {
                string fullName = Convert.ToString(lbxIMs.Items[s]);
                string imcount = string.Empty;
                int cnt = 0;

                if (fullName.Contains("("))
                {
                    try
                    {
                        string[] splits = fullName.Split('(');

                        fullName = splits[0].ToString().Trim();
                        imcount = splits[1].ToString().Trim();
                        string[] splits1 = imcount.Split(')');

                        try
                        {
                            imcount = splits1[0].ToString().Trim();
                            cnt = Convert.ToInt32(imcount) + 1;
                        }
                        catch { cnt = 1; }

                        fullName = TabAgentName + " (" + cnt.ToString() + ")";

                        lbxIMs.BeginUpdate();
                        lbxIMs.Items[s] = fullName;
                        lbxIMs.EndUpdate();
                    }
                    catch { ; }
                }

            }

            SetSets();
        }

        private void IMbox_Load(object sender, EventArgs e)
        {

        }

        private void SetSets()
        {
            if (lbxIMs.Items.Count > 0)
            {
                label1.Visible = false;
            }
            else
            {
                label1.Visible = true;
                tabsconsole.tabs["imbox"].Unhighlight();
            }

            label3.Text = lbxIMs.Items.Count.ToString();
            instance.State.UnReadIMs = lbxIMs.Items.Count;

            lbxIMs.SelectedIndex = -1;
        }

        private void lbxIMs_DoubleClick(object sender, EventArgs e)
        {
            if (lbxIMs.SelectedItem == null) return;

            string fullName = lbxIMs.SelectedItem.ToString();
            int selinx = lbxIMs.SelectedIndex;

            string[] splits = fullName.Split('(');

            fullName = splits[0].ToString().Trim();

            lbxIMs.Items.RemoveAt(selinx);

            SetSets();

            if (tabsconsole.TabExists(fullName))
            {
                tabsconsole.SelectTab(fullName);
                return;
            }
        }

        public void IMRead(string fullName)
        {
            int s = lbxIMs.FindString(fullName);

            if (s > -1)
            {
                lbxIMs.Items.RemoveAt(s);
            }

            SetSets();
        }

        private void picAutoSit_Click(object sender, EventArgs e)
        {

        }

        private void picAutoSit_MouseHover(object sender, EventArgs e)
        {
            toolTip.Show(picAutoSit);
        }

        private void picAutoSit_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Close();
        }

        private void lbxIMs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxIMs.SelectedItem == null)
            {
                btnView.Enabled = false;
                return;
            }

            btnView.Enabled = true; 
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (lbxIMs.SelectedItem == null)
            {
                btnView.Enabled = false;
                return;
            }

            string fullName = lbxIMs.SelectedItem.ToString();
            int selinx = lbxIMs.SelectedIndex;

            string[] splits = fullName.Split('(');

            fullName = splits[0].ToString().Trim();

            lbxIMs.Items.RemoveAt(selinx);

            SetSets();

            if (tabsconsole.TabExists(fullName))
            {
                tabsconsole.SelectTab(fullName);
                return;
            }
        }
    }
}
