//  Copyright (c) 2008 - 2011, www.metabolt.net (METAbolt)
//  Copyright (c) 2007-2009, openmetaverse.org
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
using System.Text;
using System.Windows.Forms;
using System.IO;
using OpenMetaverse;
using OpenMetaverse.Imaging;
using OpenMetaverse.Assets;
using OpenMetaverse.Packets; 
using SLNetworkComm;
using ExceptionReporting;
using System.Threading;
using System.Management;
using System.Globalization;


namespace METAbolt
{
    public partial class frmGroupInfo : Form
    {
        private METAboltInstance instance;
        //private ITextPrinter textPrinter;
        //private List<ChatBufferItem> textBuffer;
        private Group Group;
        private GridClient Client;
        private Group Profile = new Group();
        private Dictionary<UUID, GroupMember> Members = new Dictionary<UUID, GroupMember>();
        private Dictionary<UUID, GroupTitle> Titles = new Dictionary<UUID, GroupTitle>();
        private Dictionary<UUID, GroupMemberData> MemberData = new Dictionary<UUID, GroupMemberData>();
        private Dictionary<UUID, string> Names = new Dictionary<UUID, string>();
        private SLNetCom netcom;
        private InstantMessage imsg;
        private UUID objID = UUID.Zero;
        private bool floading = true;
        private bool tchanged = false;
        private UUID grouptitles = UUID.Zero;
        private UUID groupmembers = UUID.Zero;
        private bool ejectpower = false;
        private ExceptionReporter reporter = new ExceptionReporter();
        private UUID grpid = UUID.Zero;
        private Dictionary<UUID, GroupRole> grouproles;
        private List<KeyValuePair<UUID, UUID>> grouprolesavs;
        //private UUID grouprolesreply = UUID.Zero;
        //private UUID grouprolemembersid = UUID.Zero;
        private UUID founderid = UUID.Zero;
        //private GroupMemberData currentmember = new GroupMemberData();
        private bool checkignore = false;
        private NumericStringComparer lvwColumnSorter;

        internal class ThreadExceptionHandler
        {
            public void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
            {
                ExceptionReporter reporter = new ExceptionReporter();
                reporter.Show(e.Exception);
            }
        }

        public frmGroupInfo(Group group, METAboltInstance instance)
        {
            InitializeComponent();

            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.instance = instance;
            netcom = instance.Netcom;
            Client = instance.Client;
            this.Group = group;
            grpid = group.ID;

            //while (!IsHandleCreated)
            //{
            //    // Force handle creation
            //    IntPtr temp = Handle;
            //}

            AddGEvents();

            lvwColumnSorter = new NumericStringComparer();
            lstMembers.ListViewItemSorter = lvwColumnSorter;
            lstMembers2.ListViewItemSorter = lvwColumnSorter;
            lstNotices.ListViewItemSorter = lvwColumnSorter;

            GetDets();
        }

        public frmGroupInfo(AvatarGroup group, METAboltInstance instance)
        {
            InitializeComponent();

            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.instance = instance;
            netcom = instance.Netcom;
            Client = instance.Client;
            grpid = group.GroupID;

            //while (!IsHandleCreated)
            //{
            //    // Force handle creation
            //    IntPtr temp = Handle;
            //}

            AddGEvents();

            lvwColumnSorter = new NumericStringComparer();
            lstMembers.ListViewItemSorter = lvwColumnSorter;
            lstMembers2.ListViewItemSorter = lvwColumnSorter;
            lstNotices.ListViewItemSorter = lvwColumnSorter;

            Client.Groups.RequestGroupProfile(group.GroupID);
            groupmembers = Client.Groups.RequestGroupMembers(group.GroupID);
            grouptitles = Client.Groups.RequestGroupTitles(group.GroupID);
            // and the notices
            Client.Groups.RequestGroupNoticesList(group.GroupID);
            Client.Groups.RequestGroupRoles(group.GroupID);
        }

        public frmGroupInfo(UUID groupid, METAboltInstance instance)
        {
            InitializeComponent();

            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.instance = instance;
            netcom = instance.Netcom;
            Client = instance.Client;
            grpid = groupid;

            //while (!IsHandleCreated)
            //{
            //    // Force handle creation
            //    IntPtr temp = Handle;
            //}

            AddGEvents();

            lvwColumnSorter = new NumericStringComparer();
            lstMembers.ListViewItemSorter = lvwColumnSorter;
            lstMembers2.ListViewItemSorter = lvwColumnSorter;
            lstNotices.ListViewItemSorter = lvwColumnSorter;

            Client.Groups.RequestGroupProfile(groupid);
            groupmembers = Client.Groups.RequestGroupMembers(groupid);
            grouptitles = Client.Groups.RequestGroupTitles(groupid);
            // and the notices
            Client.Groups.RequestGroupNoticesList(groupid);
            Client.Groups.RequestGroupRoles(groupid);
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

        private void AddGEvents()
        {
            grouprolesavs = new List<KeyValuePair<UUID, UUID>>();
            grouproles = new Dictionary<UUID, GroupRole>();

            Client.Groups.GroupProfile += new EventHandler<GroupProfileEventArgs>(GroupProfileHandler);
            Client.Avatars.UUIDNameReply += new EventHandler<UUIDNameReplyEventArgs>(AvatarNamesHandler);
            netcom.InstantMessageReceived += new EventHandler<InstantMessageEventArgs>(netcom_InstantMessageReceived);
            Client.Groups.GroupMembersReply += new EventHandler<GroupMembersReplyEventArgs>(GroupMembersHandler);
            Client.Groups.GroupTitlesReply += new EventHandler<GroupTitlesReplyEventArgs>(GroupTitlesHandler);
            Client.Groups.GroupNoticesListReply += new EventHandler<GroupNoticesListReplyEventArgs>(GroupNoticesHandler);
            Client.Groups.GroupLeaveReply += new EventHandler<GroupOperationEventArgs>(Groups_OnGroupLeft);
            Client.Groups.GroupRoleDataReply += new EventHandler<GroupRolesDataReplyEventArgs>(Groups_OnGroupRoleDataReply);
            Client.Groups.GroupRoleMembersReply += new EventHandler<GroupRolesMembersReplyEventArgs>(Groups_OnGroupRoleMembersReply);
        }

        private void GetDets()
        {
            // Request the group information
            Client.Groups.RequestGroupProfile(Group.ID);
            groupmembers = Client.Groups.RequestGroupMembers(Group.ID);
            grouptitles = Client.Groups.RequestGroupTitles(Group.ID);
            // and the notices
            Client.Groups.RequestGroupNoticesList(Group.ID);
            Client.Groups.RequestGroupRoles(Group.ID); 
        }

        ~frmGroupInfo()
        {
            form_dispose(); 
        }

        private void form_dispose()
        {
            Client.Groups.GroupProfile -= new EventHandler<GroupProfileEventArgs>(GroupProfileHandler);
            Client.Avatars.UUIDNameReply -= new EventHandler<UUIDNameReplyEventArgs>(AvatarNamesHandler);
            Client.Groups.GroupMembersReply -= new EventHandler<GroupMembersReplyEventArgs>(GroupMembersHandler);
            Client.Groups.GroupTitlesReply -= new EventHandler<GroupTitlesReplyEventArgs>(GroupTitlesHandler);
            Client.Groups.GroupNoticesListReply -= new EventHandler<GroupNoticesListReplyEventArgs>(GroupNoticesHandler);
            Client.Groups.GroupLeaveReply -= new EventHandler<GroupOperationEventArgs>(Groups_OnGroupLeft);
            Client.Groups.GroupRoleDataReply -= new EventHandler<GroupRolesDataReplyEventArgs>(Groups_OnGroupRoleDataReply);
            Client.Groups.GroupRoleMembersReply -= new EventHandler<GroupRolesMembersReplyEventArgs>(Groups_OnGroupRoleMembersReply);

            netcom.InstantMessageReceived -= new EventHandler<InstantMessageEventArgs>(netcom_InstantMessageReceived);

            Members.Clear();
            Titles.Clear();
            MemberData.Clear();
            Names.Clear();
            grouproles.Clear();
            grouprolesavs.Clear();

            this.Dispose();
            //GC.Collect();
        }

        public void GroupNoticesHandler(object sender, GroupNoticesListReplyEventArgs e)
        {
            if (e.GroupID != grpid)
                return;

            this.BeginInvoke(new MethodInvoker(delegate()
            {
                UpdateNotices(e.Notices);
            }));
        }

        private void Groups_OnGroupRoleMembersReply(object sender, GroupRolesMembersReplyEventArgs e)
        {
            if (e.GroupID != grpid) return;

            grouprolesavs = e.RolesMembers;
        }

        private void Groups_OnGroupRoleDataReply(object sender, GroupRolesDataReplyEventArgs e)
        {
            if (e.GroupID != grpid) return;

            try
            {
                grouproles = e.Roles;
            }
            catch { ; }

            Client.Groups.RequestGroupRolesMembers(grpid);

            this.BeginInvoke(new MethodInvoker(delegate()
            {
                PopulateRoles();
            }));
        }

        private void PopulateRoles()
        {
            lstRoles.Items.Clear();

            foreach (GroupRole role in grouproles.Values)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = role.Name;
                lvi.Tag = role;
                //lstRoles.Items.Add(lvi);
                lvi.SubItems.Add(role.ID.ToString());

                //ListViewItem.ListViewSubItem lvsi = new ListViewItem.ListViewSubItem();
                //lvsi.Text = role.ID.ToString();
                //lvi.SubItems.Add(lvsi);
                lstRoles.Items.Add(lvi);
            }
        }

        private void UpdateNotices(List<GroupNoticesListEntry> notices)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    UpdateNotices(notices);
                }));

                return;
            }

            //lstNotices.Items.Clear();  

            foreach (GroupNoticesListEntry notice in notices)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = notice.Subject;

                ListViewItem.ListViewSubItem lvsi = new ListViewItem.ListViewSubItem();
                lvsi.Text = notice.FromName;
                lvi.SubItems.Add(lvsi);

                DateTime ndte = Utils.UnixTimeToDateTime(notice.Timestamp);

                if (!instance.Config.CurrentConfig.UseSLT)
                {
                    ndte = ndte.ToLocalTime();
                }

                lvsi = new ListViewItem.ListViewSubItem();
                lvsi.Text = ndte.ToShortDateString();
                lvi.SubItems.Add(lvsi);

                lvi.Tag = notice;
                lstNotices.Items.Add(lvi);
            }
        }

        private void netcom_InstantMessageReceived(object sender, OpenMetaverse.InstantMessageEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    netcom_InstantMessageReceived(sender, e);
                }));

                return;
            }

            imsg = e.IM;
 
            if (e.IM.IMSessionID == UUID.Zero) return;

            try
            {
                if (e.IM.Dialog == InstantMessageDialog.GroupNoticeRequested)
                {
                    UUID fromAgentID = new UUID(e.IM.BinaryBucket, 2);

                    if (grpid == fromAgentID)
                    {
                        //string noticemsg =  e.IM.Message;

                        panel1.Visible = true;
                        panel2.Visible = false;

                        char[] deli = "|".ToCharArray();
                        string[] Msg = imsg.Message.Split(deli);
                        textBox5.Text = Msg[1].Replace("\n", System.Environment.NewLine);
                    }
                }
            }
            catch
            {
                ;
            }
        }

        private void GroupProfileHandler(object sender, GroupProfileEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    GroupProfileHandler(sender, e);
                }));

                return;
            }

            if (grpid != e.Group.ID) return;  

            this.Group = e.Group; 
            Group profile = e.Group;
            Profile = profile;

            if (this.Group.InsigniaID != null && this.Group.InsigniaID != UUID.Zero)
                Client.Assets.RequestImage(this.Group.InsigniaID, ImageType.Normal,
                    delegate(TextureRequestState state, AssetTexture assetTexture)
                    {
                        ManagedImage imgData;
                        Image bitmap;

                        if (state != TextureRequestState.Timeout || state != TextureRequestState.NotFound)
                        {
                            OpenJPEG.DecodeToImage(assetTexture.AssetData, out imgData, out bitmap);
                            picInsignia.Image = bitmap;
                            UpdateInsigniaProgressText("Progress...");
                        }
                        if (state == TextureRequestState.Finished)
                        {
                            UpdateInsigniaProgressText("");
                        }
                    }, true);

            this.BeginInvoke(new MethodInvoker(delegate()
            {
                if (!instance.avnames.ContainsKey(e.Group.FounderID))
                {
                    founderid = e.Group.FounderID;
                    Client.Avatars.RequestAvatarName(founderid);
                }
                else
                {
                    lblFoundedBy.Text = "Founded by " + instance.avnames[e.Group.FounderID].ToString();
                }

                UpdateProfile();
            }));
        }

        private void UpdateInsigniaProgressText(string resultText)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    UpdateInsigniaProgressText(resultText);
                }));

                return;
            }

            labelInsigniaProgress.Text = resultText;
        }

        void Groups_OnGroupLeft(object sender, GroupOperationEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    Groups_OnGroupLeft(sender, e);
                }));

                return;
            }

            groupmembers = Client.Groups.RequestGroupMembers(Group.ID);
        }

        private void UpdateProfile()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    UpdateProfile();
                }));

                return;
            }

            lblGroupName.Text = Profile.Name;
            txtCharter.Text = Profile.Charter;

            //chkListInProfile.Checked = Profile.ListInProfile;
            //chkGroupNotices.Checked = Profile.AcceptNotices;
            chkListInProfile.Checked = instance.State.Groups[Profile.ID].ListInProfile;
            chkGroupNotices.Checked = instance.State.Groups[Profile.ID].AcceptNotices;

            chkPublish.Checked = Profile.AllowPublish;
            chkOpenEnrollment.Checked = Profile.OpenEnrollment;
            chkMature.Checked = Profile.MaturePublish;

            chkFee.Checked = (Profile.MembershipFee != 0);

            try
            {
                if (Profile.MembershipFee > Convert.ToInt32(numFee.Maximum))
                {
                    numFee.Maximum = Convert.ToDecimal(Profile.MembershipFee);
                }

                numFee.Value = Profile.MembershipFee;
            }

            catch { ; }

            //chkGroupNotices.Checked = Profile.AcceptNotices;
            textBox2.Text = "Group UUID: " + Profile.ID.ToString();

            floading = false;
        }

        private void AvatarNamesHandler(object sender, UUIDNameReplyEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    AvatarNamesHandler(sender, e);
                }));

                return;
            }
               
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                foreach (KeyValuePair<UUID, string> av in e.Names)
                {
                    try
                    {
                        if (!instance.avnames.ContainsKey(av.Key))
                        {
                            instance.avnames.Add(av.Key, av.Value);
                        }

                        if (av.Key == founderid)
                        {
                            lblFoundedBy.Text = "Founded by " + av.Value;
                        }
   
                        if (!MemberData.ContainsKey(av.Key)) return;

                        ListViewItem foundItem = lstMembers.FindItemWithText(av.Key.ToString(), false, 0, true);

                        if (foundItem != null)
                        {
                            foundItem.Text = av.Value;
                        }

                        foundItem = lstMembers2.FindItemWithText(av.Key.ToString(), false, 0, true);

                        if (foundItem != null)
                        {
                            foundItem.Text = av.Value;
                        }

                        GroupMemberData memberData = new GroupMemberData();

                        memberData = MemberData[av.Key];
                        memberData.Name = av.Value;

                        lock (MemberData)
                        {
                            MemberData.Remove(av.Key);
                            MemberData.Add(av.Key, memberData);
                        }
                    }
                    catch
                    {
                        ; 
                    }
                }

                lstMembers.Sort();
                lstMembers2.Sort();
            }));
        }

        private void GroupMembersHandler(object sender, GroupMembersReplyEventArgs e)
        {
            if (groupmembers != e.RequestID) return;

            if (grpid != e.GroupID) return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    GroupMembersHandler(sender, e);
                }));

                return;
            } 

            Members = e.Members;

            this.BeginInvoke(new MethodInvoker(delegate()
            {
                UpdateMembers();
            })); 
        }

        private void UpdateMembers()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    UpdateMembers();
                }));

                return;
            }
            else
            {
                List<UUID> requestids = new List<UUID>();

                lstMembers.Items.Clear();
                lstMembers2.Items.Clear();

                txtCharter.Enabled = false;
                chkPublish.Enabled = false;
                chkOpenEnrollment.Enabled = false;
                chkFee.Enabled = false;
                numFee.Enabled = false;
                chkMature.Enabled = false;

                foreach (GroupMember member in Members.Values)
                {
                    GroupMemberData memberData = new GroupMemberData();
                    memberData.ID = member.ID;
                    memberData.IsOwner = member.IsOwner;

                    // TPV change just incase!!! 31 Mar 2010
                    if (Client.Self.AgentID == member.ID && member.IsOwner)
                    {
                        button1.Enabled = true;
                    }

                    memberData.LastOnline = member.OnlineStatus;
                    memberData.Powers = (ulong)member.Powers;
                    memberData.Title = member.Title;
                    memberData.Contribution = member.Contribution;

                    if (member.ID == Client.Self.AgentID)
                    {
                        cmdEject.Enabled = ejectpower = ((member.Powers & GroupPowers.Eject) != 0);
                        button6.Enabled = false;

                        button4.Visible = ((member.Powers & GroupPowers.CreateRole) != 0);
                        button5.Visible = ((member.Powers & GroupPowers.DeleteRole) != 0);

                        //try
                        //{
                        //    chkListInProfile.Checked = instance.State.Groups[Profile.ID].ListInProfile;
                        //    chkGroupNotices.Checked = instance.State.Groups[Profile.ID].AcceptNotices;
                        //}
                        //catch { ; }

                        if (instance.State.Groups.ContainsKey(Profile.ID))
                        {
                            if ((member.Powers & GroupPowers.ChangeIdentity) != 0)
                            {
                                txtCharter.Enabled = true;
                            }

                            if ((member.Powers & GroupPowers.ChangeOptions) != 0)
                            {
                                chkPublish.Enabled = true;
                                chkOpenEnrollment.Enabled = true;
                                chkFee.Enabled = true;
                                numFee.Enabled = true;
                                chkMature.Enabled = true;
                            }

                            chkListInProfile.Enabled = true;
                            chkGroupNotices.Enabled = true;
                        }
                        else
                        {
                            chkListInProfile.Enabled = false;
                            chkGroupNotices.Enabled = false;
                        }
                    }

                    ListViewItem lvi = new ListViewItem();

                    if (!instance.avnames.ContainsKey(member.ID))
                    {
                        lvi.Text = member.ID.ToString();
                    }
                    else
                    {
                        lvi.Text = memberData.Name = instance.avnames[member.ID];
                    }

                    ListViewItem.ListViewSubItem lvsi = new ListViewItem.ListViewSubItem();
                    lvsi.Text = member.Title;
                    lvi.SubItems.Add(lvsi);

                    lvsi = new ListViewItem.ListViewSubItem();
                    lvsi.Text = member.OnlineStatus;
                    lvi.SubItems.Add(lvsi);

                    lvi.Tag = memberData;
                    lvi.ToolTipText = "Double click to view " + lvi.Text + "'s profile";

                    lstMembers.Items.Add(lvi);

                    if (!MemberData.ContainsKey(member.ID))
                    {
                        MemberData.Add(member.ID, memberData);
                    }

                    lvi = null;

                    lvi = new ListViewItem();
                    if (!instance.avnames.ContainsKey(member.ID))
                    {
                        lvi.Text = member.ID.ToString();
                        requestids.Add(member.ID);
                    }
                    else
                    {
                        lvi.Text = instance.avnames[member.ID];
                    }

                    lvsi = new ListViewItem.ListViewSubItem();
                    lvsi.Text = member.Contribution.ToString(CultureInfo.CurrentCulture);
                    lvi.SubItems.Add(lvsi);

                    lvsi = new ListViewItem.ListViewSubItem();
                    lvsi.Text = member.OnlineStatus;
                    lvi.SubItems.Add(lvsi);

                    lvi.Tag = member;

                    lstMembers2.Items.Add(lvi);
                }

                label10.Visible = false;

                lstMembers.Sort();
                lstMembers2.Sort();

                if (requestids.Count > 0)
                {
                    Client.Avatars.RequestAvatarNames(requestids);
                }
            }
        }

        private void GroupTitlesHandler(object sender, GroupTitlesReplyEventArgs e)
        {
            if (grouptitles  != e.RequestID)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    GroupTitlesHandler(sender, e);
                }));

                return;
            }

            Titles = e.Titles;

            //UpdateTitles();
        }

        ////private void UpdateTitles()
        ////{
        ////    if (this.InvokeRequired)
        ////    {
        ////        this.BeginInvoke(new MethodInvoker(delegate()
        ////        {
        ////            UpdateTitles();
        ////        }));

        ////        return;
        ////    }
        ////    else
        ////    {
        ////        lock (Titles)
        ////        {
        ////            lstRoles.Items.Clear();

        ////            foreach (KeyValuePair<UUID, GroupTitle> kvp in Titles)
        ////            {
        ////                ListViewItem lvi = new ListViewItem();
        ////                lvi.Text = kvp.Value.Title.ToString()   ;

        ////                ListViewItem.ListViewSubItem lvsi = new ListViewItem.ListViewSubItem();
        ////                lvsi.Text = kvp.Key.ToString();
        ////                lvi.SubItems.Add(lvsi);
        ////                lstRoles.Items.Add(lvi);
        ////            }
        ////        }
        ////    }
        ////}

        private void cmdEject_Click(object sender, EventArgs e)
        {
            if (!ejectpower) return;

            if (lstMembers2.SelectedItems.Count  > 0)   // && lstMembers2.Columns[0].ToString != "none")
            {
                for (int i = lstMembers2.SelectedItems.Count - 1; i >= 0; i--)
                {
                   string  li = lstMembers2.SelectedItems[i].Text ;  

                    foreach (GroupMemberData entry in MemberData.Values)
                    {
                        if (li == entry.Name)
                        {
                            try
                            {
                                Client.Groups.EjectUser(Group.ID, entry.ID);
                                break; 
                            }
                            catch (Exception ex)
                            {
                                Logger.Log("Eject from group: " + ex.Message, Helpers.LogLevel.Warning);    
                            }
                        }
                    }
                }

                try
                {
                    groupmembers = Client.Groups.RequestGroupMembers(Group.ID);
                }
                catch
                {
                    ; 
                }
            }
        }

        private void tabGeneral_Click(object sender, EventArgs e)
        {

        }

        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            // Request the group information
            Client.Groups.RequestGroupProfile(Group.ID);
            groupmembers = Client.Groups.RequestGroupMembers(Group.ID);
            grouptitles = Client.Groups.RequestGroupTitles(Group.ID);
            Client.Groups.RequestGroupNoticesList(Group.ID);
            Client.Groups.RequestGroupRoles(Group.ID); 
        }

        private void cmdApply_Click(object sender, EventArgs e)
        {
            GetDets();
            //this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            //GetDets();
            this.Close();
        }

        private void lstMembers2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //chkListRoles.Items.Clear();
            lvAssignedRoles.Items.Clear();  

            if (lstMembers2.SelectedItems.Count > 0)   // && lstMembers2.Columns[0].ToString != "none")
            {
                string li = lstMembers2.SelectedItems[0].Text;
                UUID avid = UUID.Zero;  

                foreach (GroupMemberData entry in MemberData.Values)
                {
                    if (li == entry.Name)
                    {
                        avid = entry.ID;
                        break; 
                    }
                }

                checkignore = true;

                ListViewItem everyone = new ListViewItem();
                everyone.Text = "Everyone";
                everyone.Checked = true;
                //everyone.Tag = null;
                lvAssignedRoles.Items.Add(everyone);

                GroupPowers power = GroupPowers.None;

                foreach (GroupRole role in grouproles.Values)
                {
                    if (role.Name.ToLower(CultureInfo.CurrentCulture) != "everyone")
                    {
                        UUID roleid = role.ID;

                        ListViewItem nme = new ListViewItem();
                        nme.Text = role.Name;
                        nme.Tag = role;
                        
                        bool ismember = false;

                        foreach (var el in grouprolesavs)
                        {
                            if (el.Value == avid && el.Key == roleid)
                            {
                                ismember = true;
                                power = role.Powers;
                            }
                        }

                        nme.Checked = ismember;
                        lvAssignedRoles.Items.Add(nme);
                    }

                    lvwAble.Items.Clear();

                    foreach (GroupPowers p in Enum.GetValues(typeof(GroupPowers)))
                    {
                        if (p != GroupPowers.None && (power & p) == p)
                        {
                            lvwAble.Items.Add(p.ToString());
                        }
                    }
                }

                checkignore = false;
            }
        }

        private void frmGroupInfo_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
        }

        private void lstRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string li = UUID.Zero.ToString();
   
            if (lstRoles.SelectedItems.Count > 0)
            {
                for (int i = lstRoles.SelectedItems.Count - 1; i >= 0; i--)
                {
                    li = lstRoles.SelectedItems[i].SubItems[1].Text;
                    textBox3.Text = li;
                }

                //button5.Enabled = true;

                lvRoleMembers.Items.Clear();

                if ((UUID)li == UUID.Zero)
                {
                    //lvRoleMembers

                    foreach (GroupMemberData data in MemberData.Values)
                    {
                        lvRoleMembers.Items.Add(data.Name);
                    }
                }
                else
                {
                    foreach (KeyValuePair<UUID, UUID> name in grouprolesavs)
                    {
                        if (name.Key == (UUID)li)
                        {
                            if (instance.avnames.ContainsKey(name.Value))
                            {
                                lvRoleMembers.Items.Add(instance.avnames[name.Value]);
                            }
                        }
                    }
                }
            }
        }

        private void picInsignia_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.SelectAll();  
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            textBox3.SelectAll();
        }

        private void btnInvite_Click(object sender, EventArgs e)
        {
            if (lstRoles.SelectedItems.Count > 0)
            {
                UUID li = (UUID)textBox3.Text;
                (new frmGive(instance, grpid, li)).Show(this);
            }
            else
            {
                MessageBox.Show("First you must select a role from the list above.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void lstMembers_DoubleClick(object sender, EventArgs e)
        {
            if (lstMembers.SelectedItems.Count == 1)
            {
                string li = lstMembers.SelectedItems[0].Text;

                foreach (GroupMemberData entry in MemberData.Values)
                {
                    if (li == entry.Name)
                    {
                        (new frmProfile(instance, entry.Name, entry.ID)).Show();
                    }
                }
            }
        }

        private void lstMembers2_DoubleClick(object sender, EventArgs e)
        {
            if (lstMembers2.SelectedItems.Count == 1)
            {
                string li = lstMembers2.SelectedItems[0].Text;

                foreach (GroupMemberData entry in MemberData.Values)
                {
                    if (li == entry.Name)
                    {
                        (new frmProfile(instance, entry.Name, entry.ID)).Show();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lstMembers2.Items.Count  > 0)   // && lstMembers2.Columns[0].ToString != "none")
            {
                Stream tstream;
                saveFileDialog1.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if ((tstream = saveFileDialog1.OpenFile()) != null)
                        {
                            StreamWriter SW = new StreamWriter(tstream);

                            SW.WriteLine("Group: " + this.Group.Name + ",UUID: " + grpid.ToString() + ",Ttl members: " + lstMembers2.Items.Count.ToString(CultureInfo.CurrentCulture) + ",");
                            SW.WriteLine(",,,");
                            SW.WriteLine("Name,UUID,Title,Last online");

                            foreach (GroupMemberData entry in MemberData.Values)
                            {
                                try
                                {
                                    string line = entry.Name + "," + entry.ID.ToString() + "," + entry.Title + "," + entry.LastOnline.ToString(CultureInfo.CurrentCulture);
                                    SW.WriteLine(line);
                                }
                                catch (Exception ex)
                                {
                                    Logger.Log("Group list export: " + ex.Message, Helpers.LogLevel.Warning);
                                }
                            }

                            SW.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);   
                    }
                }
             }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("You must enter a 'subject' first");  
                return;
            }

            // Send the notice
            GroupNotice sgnotice = new GroupNotice();

            sgnotice.Subject = textBox1.Text;
            sgnotice.Message = textBox4.Text;

            if (objID != UUID.Zero)
            {
                sgnotice.AttachmentID = objID;
                sgnotice.OwnerID = Client.Self.AgentID;
                sgnotice.SerializeAttachment();
            }

            Client.Groups.SendGroupNotice(grpid, sgnotice);

            // Set everything back to defaults
            label3.Text = "Archived Notice";
            panel1.Visible = true;
            panel2.Visible = false;
            button2.Enabled = false;
            objID = UUID.Zero;
            textBox6.Text = "Drag-Drop attachment item here";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label3.Text = "Create a Notice";
            panel1.Visible = false;
            panel2.Visible = true;
            button2.Enabled = true; 
        }

        private void tabNotices_Click(object sender, EventArgs e)
        {

        }

        private void cmdRefreshNotices_Click(object sender, EventArgs e)
        {
            lstNotices.Items.Clear();
            Client.Groups.RequestGroupNoticesList(grpid);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lstNotices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstNotices.SelectedItems.Count > 0)
            {
                textBox5.Text = string.Empty;

                GroupNoticesListEntry gnotice = (GroupNoticesListEntry)lstNotices.SelectedItems[0].Tag;
                Client.Groups.RequestGroupNotice(gnotice.NoticeID);   
            }
        }

        private void textBox6_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode node = e.Data.GetData(typeof(TreeNode)) as TreeNode;

            if (node == null) return;

            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                InventoryBase io = (InventoryBase)node.Tag;

                if (node.Tag is InventoryItem)
                {
                    InventoryItem item = (InventoryItem)io;

                    if ((item.Permissions.OwnerMask & PermissionMask.Copy) != PermissionMask.Copy)
                    {
                        MessageBox.Show("This is a 'no copy' item. You can't attach no transfer/copy items.", "Warning", MessageBoxButtons.OK);
                        return;
                    }

                    objID = item.UUID;
                    textBox6.Text = item.Name;
                }
            }
        }

        private void textBox6_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBox6_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void chkPublish_CheckedChanged(object sender, EventArgs e)
        {
            if (floading) return;

            Profile.AllowPublish = chkPublish.Checked;
            Client.Groups.UpdateGroup(Profile.ID, Profile);      
        }

        private void chkOpenEnrollment_CheckedChanged(object sender, EventArgs e)
        {
            if (floading) return;

            Profile.OpenEnrollment = chkOpenEnrollment.Checked;
            Client.Groups.UpdateGroup(Profile.ID, Profile);
        }

        private void chkFee_CheckedChanged(object sender, EventArgs e)
        {
            if (floading) return;

            if (!chkFee.Checked)
            {
                Profile.MembershipFee = 0;
                Client.Groups.UpdateGroup(Profile.ID, Profile);
            }

            numFee.Enabled = chkFee.Checked;
        }

        private void numFee_ValueChanged(object sender, EventArgs e)
        {
            if (floading) return;

            Profile.MembershipFee = Convert.ToInt32(numFee.Value);
            Client.Groups.UpdateGroup(Profile.ID, Profile);
        }

        private void chkGroupNotices_CheckedChanged(object sender, EventArgs e)
        {
            if (floading) return;

            //Profile.AcceptNotices = chkGroupNotices.Checked;
            //Client.Groups.UpdateGroup(Profile.ID, Profile);
            Client.Groups.SetGroupAcceptNotices(Profile.ID, chkGroupNotices.Checked, chkListInProfile.Checked);
            Client.Groups.RequestCurrentGroups();

            //instance.State.Groups[Profile.ID].AcceptNotices = chkGroupNotices.Checked;
        }

        private void chkMature_CheckedChanged(object sender, EventArgs e)
        {
            if (floading) return;

            Profile.MaturePublish = chkMature.Checked;
            Client.Groups.UpdateGroup(Profile.ID, Profile);
        }

        private void txtCharter_Leave(object sender, EventArgs e)
        {
            if (tchanged)
            {
                Profile.Charter = txtCharter.Text;
                Client.Groups.UpdateGroup(Profile.ID, Profile);
                tchanged = false;
            }
        }

        private void txtCharter_TextChanged(object sender, EventArgs e)
        {
            if (floading) return;

            tchanged = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
        }

        private void frmGroupInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            form_dispose();
        }

        private void lstMembers_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lstMembers.Sort();
        }

        private void lstMembers2_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lstMembers2.Sort();
        }

        private void lstNotices_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lstNotices.Sort();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (lstRoles.SelectedItems.Count > 0)
            {
                //instance.State.Groups[grpid]
                UUID li = (UUID)textBox3.Text;
                Client.Groups.DeleteRole(grpid, li);
            }
            else
            {
                MessageBox.Show("First you must select a role from the list above.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Client.Groups.RequestJoinGroup(grpid);
        }

        private void lstMembers_MouseEnter(object sender, EventArgs e)
        {
            lstMembers.Cursor = Cursors.Hand;
        }

        private void lstMembers_MouseLeave(object sender, EventArgs e)
        {
            lstMembers.Cursor = Cursors.Default;
        }

        private void lstMembers2_MouseEnter(object sender, EventArgs e)
        {
            lstMembers2.Cursor = Cursors.Hand;
        }

        private void lstMembers2_MouseLeave(object sender, EventArgs e)
        {
            lstMembers2.Cursor = Cursors.Default;
        }

        private void chkListInProfile_CheckedChanged(object sender, EventArgs e)
        {
            if (floading) return;

            Client.Groups.SetGroupAcceptNotices(Profile.ID, chkGroupNotices.Checked, chkListInProfile.Checked);
            Client.Groups.RequestCurrentGroups();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            GroupRole role = new GroupRole();
            role.Name = textBox7.Text;
            role.Title = textBox8.Text;
            role.Description = textBox9.Text;

            //GroupPowers powers = new GroupPowers();
            //role.Powers

            Client.Groups.CreateRole(grpid, role);

            panel3.Visible = false;
            textBox7.Text = string.Empty;
            textBox8.Text = string.Empty;
            textBox9.Text = string.Empty;
        }

        private void lvAssignedRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvwAble.Items.Clear();

            if (lvAssignedRoles.SelectedItems.Count > 0)   // && lstMembers2.Columns[0].ToString != "none")
            {
                if (lvAssignedRoles.SelectedItems[0].Tag == null)
                {
                    // everyone
                    GroupPowers power = GroupPowers.None;

                    foreach (GroupPowers p in Enum.GetValues(typeof(GroupPowers)))
                    {
                        if (p != GroupPowers.None && (power & p) == p)
                        {
                            lvwAble.Items.Add(p.ToString());
                        }
                    }
                }
                else
                {
                    GroupPowers power = GroupPowers.None;
                    GroupRole role = (GroupRole)lvAssignedRoles.SelectedItems[0].Tag;

                    power = role.Powers;

                    foreach (GroupPowers p in Enum.GetValues(typeof(GroupPowers)))
                    {
                        if (p != GroupPowers.None && (power & p) == p)
                        {
                            lvwAble.Items.Add(p.ToString());
                        }
                    }
                }
            }
        }

        private void lvAssignedRoles_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (floading) return;

            if (checkignore) return;

            ListViewItem item = e.Item as ListViewItem;
            //GroupRole role = (GroupRole)lvAssignedRoles.SelectedItems[0].Tag;

            GroupRole role;

            if (item.Tag != null)
            {
                role = (GroupRole)item.Tag;
            }
            else
            {
                role.Name = "Everyone";
                role.ID = UUID.Zero;
                role.Powers = GroupPowers.None;
            }


            string li = lstMembers2.SelectedItems[0].Text;
            UUID avid = UUID.Zero;

            try
            {
                foreach (GroupMemberData entry in MemberData.Values)
                {
                    if (li == entry.Name)
                    {
                        avid = entry.ID;
                        break;
                    }
                }

                if (item.Checked)
                {
                    foreach (var el in grouprolesavs)
                    {
                        if (el.Value == avid && el.Key == role.ID)
                        {
                            grouprolesavs.Add(new KeyValuePair<UUID, UUID>(role.ID, avid));
                        }
                    }
                }
                else
                {
                    foreach (var el in grouprolesavs)
                    {
                        if (el.Value == avid && el.Key == role.ID)
                        {
                            grouprolesavs.Remove(new KeyValuePair<UUID, UUID>(role.ID, avid));
                        }
                    }
                }
            }
            catch { ; }

            // The section below has been borrowed from Radegast (c) 2009-2011 Radegast Development Team
            List<GroupRoleChangesPacket.RoleChangeBlock> changes = new List<GroupRoleChangesPacket.RoleChangeBlock>();
            GroupRoleChangesPacket.RoleChangeBlock rc = new GroupRoleChangesPacket.RoleChangeBlock();

            rc.MemberID = avid;
            rc.RoleID = role.ID;
            rc.Change = item.Checked ? 0u : 1u;
            changes.Add(rc);

            GroupRoleChangesPacket packet = new GroupRoleChangesPacket();
            packet.AgentData.AgentID = Client.Self.AgentID;
            packet.AgentData.SessionID = Client.Self.SessionID;
            packet.AgentData.GroupID = Profile.ID;
            packet.RoleChange = changes.ToArray();
            Client.Network.CurrentSim.SendPacket(packet);
        }
    }

    public class GroupMemberData
    {
        public UUID ID;
        public string Name;
        public string Title;
        public string LastOnline;
        public ulong Powers;
        public bool IsOwner;
        public int Contribution;
    }
}
