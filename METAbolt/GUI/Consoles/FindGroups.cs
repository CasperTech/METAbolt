//  Copyright (c) 2008-2013, www.metabolt.net
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
//using SLNetworkComm;


namespace METAbolt
{
    public partial class FindGroups : UserControl
    {
        private METAboltInstance instance;
        //private SLNetCom netcom;
        private GridClient client;

        private UUID queryID;
        private SafeDictionary<string, UUID> findGroupsResults;

        public event EventHandler SelectedIndexChanged;
        private NumericStringComparer lvwColumnSorter;

        public FindGroups(METAboltInstance instance, UUID queryID)
        {
            InitializeComponent();

            findGroupsResults = new SafeDictionary<string, UUID>();
            this.queryID = queryID;

            this.instance = instance;
            //netcom = this.instance.Netcom;
            client = this.instance.Client;
            AddClientEvents();

            lvwColumnSorter = new NumericStringComparer();
            lvwFindGroups.ListViewItemSorter = lvwColumnSorter;
        }

        private void AddClientEvents()
        {
            client.Directory.DirGroupsReply += new EventHandler<DirGroupsReplyEventArgs>(Directory_OnDirGroupsReply);   
        }

        private void Directory_OnDirGroupsReply(object sender, DirGroupsReplyEventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                GroupsReply(e.QueryID, e.MatchedGroups);
            });
        }

        private void GroupsReply(UUID qqueryID, List<DirectoryManager.GroupSearchData> matchedGroups)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    GroupsReply(qqueryID, matchedGroups);
                }));

                return;
            }

            if (qqueryID != this.queryID) return;

            lvwFindGroups.BeginUpdate();

            foreach (DirectoryManager.GroupSearchData group in matchedGroups)
            {
                if (!findGroupsResults.ContainsKey(group.GroupName))
                {
                    findGroupsResults.Add(group.GroupName, group.GroupID);
                }

                ListViewItem item = lvwFindGroups.Items.Add(group.GroupName);
                item.Tag = group.GroupID;
                item.SubItems.Add("Total " + group.Members.ToString() + " members");
            }

            lvwFindGroups.Sort();
            lvwFindGroups.EndUpdate();
            pGroups.Visible = false;
        }

        public void ClearResults()
        {
            findGroupsResults.Clear();
            lvwFindGroups.Items.Clear();
        }

        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndexChanged != null) SelectedIndexChanged(this, e);
        }

        private void lvwFindGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedIndexChanged(e);
        }

        public SafeDictionary<string, UUID> LLUUIDs
        {
            get { return findGroupsResults; }
        }

        public UUID QueryID
        {
            get { return queryID; }
            set { queryID = value; }
        }

        public int SelectedIndex
        {
            get
            {
                if (lvwFindGroups.SelectedItems == null) return -1;
                if (lvwFindGroups.SelectedItems.Count == 0) return -1;

                return lvwFindGroups.SelectedIndices[0];
            }
        }

        public string SelectedName
        {
            get
            {
                if (lvwFindGroups.SelectedItems == null) return string.Empty;
                if (lvwFindGroups.SelectedItems.Count == 0) return string.Empty;

                return lvwFindGroups.SelectedItems[0].Text;
            }
        }

        public UUID SelectedGroupUUID
        {
            get
            {
                if (lvwFindGroups.SelectedItems == null) return UUID.Zero;
                if (lvwFindGroups.SelectedItems.Count == 0) return UUID.Zero;

                return (UUID)lvwFindGroups.SelectedItems[0].Tag;
            }
        }

        private void pGroups_Click(object sender, EventArgs e)
        {

        }

        private void lvwFindGroups_ColumnClick(object sender, ColumnClickEventArgs e)
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

            lvwFindGroups.Sort(); 
        }
    }
}
