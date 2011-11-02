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
//using SLNetworkComm;
using OpenMetaverse;

namespace METAbolt
{
    public partial class frmGive : Form
    {
        private METAboltInstance instance;
        //private SLNetCom netcom;
        private GridClient client;
        private InventoryItem item;
        private UUID queryID;
        bool alreadyFocused;
        Dictionary<UUID, Group> Groups = new Dictionary<UUID,Group>();
        Dictionary<UUID, GroupMember> Members = new Dictionary<UUID, GroupMember>();
        SafeDictionary<UUID, GroupMemberData> MemberData = new SafeDictionary<UUID, GroupMemberData>();
        SafeDictionary<UUID, string> Names = new SafeDictionary<UUID, string>();
        private UUID grp = UUID.Zero;
        private UUID rle = UUID.Zero;
        private bool inventorymode = true;
        private bool groupmode = false;
        private NumericStringComparer lvwColumnSorter;

        public frmGive(METAboltInstance instance, InventoryItem item)
        {
            InitializeComponent();

            this.instance = instance;
            //netcom = this.instance.Netcom;
            client = this.instance.Client;
            this.item = item;

            textBox1.GotFocus += textBox1_GotFocus; 
            textBox1.MouseUp += textBox1_MouseUp; 
            textBox1.Leave += textBox1_Leave;

            client.Directory.DirPeopleReply += new EventHandler<DirPeopleReplyEventArgs>(Directory_OnDirPeopleReply);
            client.Groups.GroupMembersReply += new EventHandler<GroupMembersReplyEventArgs>(GroupMembersHandler);
            client.Avatars.UUIDNameReply += new EventHandler<UUIDNameReplyEventArgs>(AvatarNamesHandler);

            groupmode = false;

            client.Groups.RequestCurrentGroups();

            lvwColumnSorter = new NumericStringComparer();
            lvwFindFriends.ListViewItemSorter = lvwColumnSorter;
            lvwSelected.ListViewItemSorter = lvwColumnSorter;
        }

        public frmGive(METAboltInstance instance, UUID group, UUID role)
        {
            InitializeComponent();

            this.Text = "Group invite avatar selection ";
            btnGive.Text = "Invite";
            //tabControl1.TabPages[1].Hide();
            tabControl1.TabPages.RemoveAt(1);

            inventorymode = false; 

            this.instance = instance;
            //netcom = this.instance.Netcom;
            client = this.instance.Client;
            
            this.grp = group;
            this.rle = role; 

            textBox1.GotFocus += textBox1_GotFocus;
            textBox1.MouseUp += textBox1_MouseUp;
            textBox1.Leave += textBox1_Leave;

            client.Directory.DirPeopleReply += new EventHandler<DirPeopleReplyEventArgs>(Directory_OnDirPeopleReply);
            client.Avatars.UUIDNameReply += new EventHandler<UUIDNameReplyEventArgs>(AvatarNamesHandler);

            groupmode = true;
        }

        public frmGive(METAboltInstance instance, UUID group, UUID role, string name)
        {
            InitializeComponent();

            this.Text = "Group invite avatar selection ";
            btnGive.Text = "Invite";
            //tabControl1.TabPages[1].Hide();
            tabControl1.TabPages.RemoveAt(1);

            inventorymode = false;

            this.instance = instance;
            //netcom = this.instance.Netcom;
            client = this.instance.Client;

            this.grp = group;
            this.rle = role;
            //this.name = name;

            textBox1.GotFocus += textBox1_GotFocus;
            textBox1.MouseUp += textBox1_MouseUp;
            textBox1.Leave += textBox1_Leave;

            client.Directory.DirPeopleReply += new EventHandler<DirPeopleReplyEventArgs>(Directory_OnDirPeopleReply);
            client.Avatars.UUIDNameReply += new EventHandler<UUIDNameReplyEventArgs>(AvatarNamesHandler);

            groupmode = true;
        }

        //Separate thread
        private void Directory_OnDirPeopleReply(object sender, DirPeopleReplyEventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate { 
                PeopleReply(queryID, e.MatchedPeople); 
            });
        }

        //UI thread
        private void PeopleReply(UUID qqueryID, List<DirectoryManager.AgentSearchData> matchedPeople)
        {
            if (qqueryID != this.queryID) return;

            lvwFindFriends.BeginUpdate();

            foreach (DirectoryManager.AgentSearchData person in matchedPeople)
            {
                if (person.AgentID != UUID.Zero && person.FirstName.Length > 0)
                {
                    string fullName = person.FirstName + " " + person.LastName;

                    ListViewItem Item = lvwFindFriends.Items.Add(fullName);
                    Item.Tag = person.AgentID;
                }
            }

            //lvwFindFriends.Sort();
            lvwFindFriends.EndUpdate();

            pic1.Visible = false;

            btnGive.Enabled = true;
            btnFind.Enabled = true;
        }

        private void UpdateGroups()
        {
            lock (comboBox1)
            {
                comboBox1.Items.Clear();
                comboBox1.Text = "Select group...";

                Groups = instance.State.Groups;   

                foreach (Group group in Groups.Values)
                {
                    comboBox1.Items.Add(group);
                }

                comboBox1.Sorted = true;
            }
        }

        private void UpdateFriendslist()
        {
            List<FriendInfo> friendslist;

            if (this.instance.State.AvatarFriends == null)
            {
                friendslist = client.Friends.FriendList.FindAll(delegate(FriendInfo friend) { return true; });

                this.instance.State.AvatarFriends = friendslist;

                //MessageBox.Show("You must first initialise your friends-list by selecting the 'Friends' tab once.\n\nClose this window and select the Friends tab, then re-try", "METAbolt");
                //return;
            }

            friendslist = this.instance.State.AvatarFriends;

            if (friendslist.Count > 0)
            {
                lvwFindFriends.BeginUpdate();
                //lvwFindFriends.Items.Clear();

                foreach (FriendInfo friend in friendslist)
                {
                    //ListViewItem lvi = new ListViewItem();
                    ListViewItem Item = lvwFindFriends.Items.Add(friend.Name);
                    //Item.Text = friend.Name; 
                    Item.Tag = (UUID)friend.UUID;

                    //lvwFindFriends.Items.Add(Item);
                }

                //lvwFindFriends.Sort();
                lvwFindFriends.EndUpdate();
            }
        }

        private void GroupMembersHandler(object sender, GroupMembersReplyEventArgs e)
        {
            Members = e.Members;

            BeginInvoke((MethodInvoker)delegate
            {
                UpdateMembers();
            }); 
        }

        private void UpdateMembers()
        {
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateMembers));
                return;
            }
            else
            {
                List<UUID> requestids = new List<UUID>();

                lock (Members)
                {
                    lock (MemberData)
                    {
                        foreach (GroupMember member in Members.Values)
                        {
                            GroupMemberData memberData = new GroupMemberData();
                            memberData.ID = member.ID;
                            MemberData[member.ID] = memberData;

                            // Add this ID to the name request batch
                            if (!instance.avnames.ContainsKey(member.ID))
                            {
                                requestids.Add(member.ID);
                            }
                            else
                            {
                                Names[member.ID] = instance.avnames[member.ID];
                                UpdateNames();
                            }
                        }
                    }
                }

                if (requestids.Count > 0) client.Avatars.RequestAvatarNames(requestids);
            }
        }

        private void frmGive_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            textBox1.Focus();  
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int c = lvwSelected.Items.Count;
            pb1.Visible = true;
            pb1.Value = 0;
            pb1.Maximum = c;

            UUID avid = UUID.Zero;

            if (inventorymode)
            {
                for (int i = 0; i < c; i++)
                {
                    avid = (UUID)lvwSelected.Items[i].Tag;

                    if ((item.Permissions.OwnerMask & PermissionMask.Transfer) == PermissionMask.Transfer)
                    {
                        if ((item.Permissions.OwnerMask & PermissionMask.Copy) != PermissionMask.Copy)
                        {
                            DialogResult res = MessageBox.Show("This item is NO COPY!\nIf you give it away you will lose ownership.\n\nAre you sure you want to continue?", "METAbolt", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                            if (res == DialogResult.Yes)
                            {
                                client.Inventory.GiveItem(item.UUID, item.Name, item.AssetType, avid, false);
                                client.Inventory.Store.RemoveNodeFor(item);
                                pb1.Value = 0;
                                pb1.Visible = false;
                                return;
                            }
                        }
                        else
                        {
                            client.Inventory.GiveItem(item.UUID, item.Name, item.AssetType, avid, false);
                        }

                        pb1.Value += 1;
                    }
                    else
                    {
                        MessageBox.Show("This item is NON transferable! Operation cancelled.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pb1.Value = 0;
                        pb1.Visible = false;
                        return;
                    }
                } 
            }
            else
            {
                List<UUID> roles = new List<UUID>();
                roles.Add(rle);

                for (int i = 0; i < c; i++)
                {
                    avid = (UUID)lvwSelected.Items[i].Tag;
                    client.Groups.Invite(grp, roles, avid);

                    pb1.Value += 1;
                }
            }

            pb1.Visible = false;
            label1.Visible = true;
            label1.Text = "SEND/GIVE COMPLETED TO " + c.ToString() + " AVATARS.";  
        }

        private void StartFindingGroups()
        {
            try
            {
                lvwFindFriends.Items.Clear();
                pic1.Visible = true;
                queryID = client.Directory.StartPeopleSearch(textBox1.Text, 0);
            }
            catch
            {
                ;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) e.SuppressKeyPress = true;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            if (textBox1.Text.Trim().Length < 3) return;

            StartFindingGroups();
        }

        private void lvwFindPeople_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lvwFindFriends.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select a name first");
                return;
            }

            label1.Visible = false;  

            lvwSelected.BeginUpdate();
            ListView.SelectedListViewItemCollection vs = lvwFindFriends.SelectedItems;

            for (int i = 0; i < vs.Count; i++)
            {
                ListViewItem Item = lvwSelected.Items.Add(lvwFindFriends.Items[vs[i].Index].Text);
                Item.Tag = (UUID)lvwFindFriends.Items[vs[i].Index].Tag;
            }

            lvwSelected.Sort();
            lvwSelected.EndUpdate();

            btnGive.Enabled = button2.Enabled = button4.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            btnFind.Enabled = (textBox1.Text.Trim().Length > 2 | textBox1.Text != "enter avatar name");
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            StartFindingGroups();
            btnGive.Enabled = false; 
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (lvwSelected.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select a name first");
                return;
            }

            lvwSelected.SelectedItems[0].Remove();
            btnGive.Enabled = button2.Enabled = button4.Enabled = lvwSelected.Items.Count > 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lvwSelected.Items.Clear();
            btnGive.Enabled = button2.Enabled = button4.Enabled = lvwSelected.Items.Count > 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lvwSelected.BeginUpdate();

            for (int i = 0; i < lvwFindFriends.Items.Count; i++)
            {
                ListViewItem Item = lvwSelected.Items.Add(lvwFindFriends.Items[i].Text);
                Item.Tag = (UUID)lvwFindFriends.Items[i].Tag;
            }

            label1.Visible = false; 

            lvwSelected.Sort();
            lvwSelected.EndUpdate();

            btnGive.Enabled = button2.Enabled = button4.Enabled = lvwSelected.Items.Count > 0;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            alreadyFocused = false;
        }

        private void textBox1_GotFocus(object sender, EventArgs e)
        {   
            // Select all text only if the mouse isn't down.    
            if (MouseButtons == MouseButtons.None)    
            {        
                this.textBox1.SelectAll();
            }
        }

        private void textBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!alreadyFocused && this.textBox1.SelectionLength == 0) 
            { 
                alreadyFocused = true; 
                this.textBox1.SelectAll(); 
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmGive_FormClosing(object sender, FormClosingEventArgs e)
        {
            textBox1.GotFocus -= textBox1_GotFocus;
            textBox1.MouseUp -= textBox1_MouseUp;
            textBox1.Leave -= textBox1_Leave;

            client.Directory.DirPeopleReply -= new EventHandler<DirPeopleReplyEventArgs>(Directory_OnDirPeopleReply);
            client.Groups.GroupMembersReply -= new EventHandler<GroupMembersReplyEventArgs>(GroupMembersHandler);
            client.Avatars.UUIDNameReply -= new EventHandler<UUIDNameReplyEventArgs>(AvatarNamesHandler);
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            lvwFindFriends.Items.Clear();

            if (!groupmode)
            {
                if (e.TabPageIndex == 2)
                {
                    UpdateFriendslist();
                }
                else if (e.TabPageIndex == 1)
                {
                    UpdateGroups();
                }
            }
            else
            {
                if (e.TabPageIndex == 1)
                {
                    UpdateFriendslist();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvwFindFriends.Items.Clear();

            Group group = (Group)comboBox1.Items[comboBox1.SelectedIndex];
            client.Groups.RequestGroupMembers(group.ID);

            pic1.Visible = true;
        }

        private void AvatarNamesHandler(object sender, UUIDNameReplyEventArgs e)
        {
            lock (Names)
            {
                foreach (KeyValuePair<UUID, string> av in e.Names)
                {
                    Names[av.Key] = av.Value;
                }
            }

            BeginInvoke((MethodInvoker)delegate
            {
                UpdateNames();
            });
        }

        private void UpdateNames()
        {
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateNames));
                return;
            }
            else
            {
                lock (Names)
                {
                    lock (MemberData)
                    {
                        foreach (KeyValuePair<UUID, string> nname in Names)
                        {
                            if (!MemberData.ContainsKey(nname.Key))
                            {
                                MemberData[nname.Key] = new GroupMemberData();
                            }

                            MemberData[nname.Key].Name = nname.Value;
                        }
                    }
                }

                UpdateMemberList();
            }
        }

        private void UpdateMemberList()
        {
            // General tab list
            lock (lvwFindFriends)
            {
                lvwFindFriends.BeginUpdate();
                lvwFindFriends.Items.Clear();

                foreach (GroupMemberData entry in MemberData.Values)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = entry.Name;
                    lvi.Tag = entry.ID;  

                    lvwFindFriends.Items.Add(lvi);
                }

                //lvwFindFriends.Sort();
                lvwFindFriends.EndUpdate();

                pic1.Visible = false; 
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void lvwFindFriends_ColumnClick(object sender, ColumnClickEventArgs e)
        {
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
            lvwFindFriends.Sort();
        }

        private void lvwSelected_ColumnClick(object sender, ColumnClickEventArgs e)
        {
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
            lvwSelected.Sort();
        }
    }
}
