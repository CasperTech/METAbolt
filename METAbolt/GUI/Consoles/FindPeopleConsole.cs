//  Copyright (c) 2008 - 2011, www.metabolt.net (METAbolt)
//  Copyright (c) 2006-2008, Paul Clement (a.k.a. Delta)
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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
using SLNetworkComm;

namespace METAbolt
{
    public partial class FindPeopleConsole : UserControl
    {
        private METAboltInstance instance;
        private SLNetCom netcom;
        private GridClient client;

        private UUID queryID;
        private SafeDictionary<string, UUID> findPeopleResults;

        public event EventHandler SelectedIndexChanged;

        public FindPeopleConsole(METAboltInstance instance, UUID queryID)
        {
            InitializeComponent();

            findPeopleResults = new SafeDictionary<string, UUID>();
            this.queryID = queryID;

            this.instance = instance;
            netcom = this.instance.Netcom;
            client = this.instance.Client;
            AddClientEvents();
        }

        private void AddClientEvents()
        {
            client.Directory.DirPeopleReply += new EventHandler<DirPeopleReplyEventArgs>(Directory_OnDirPeopleReply);
        }

        //Separate thread
        private void Directory_OnDirPeopleReply(object sender, DirPeopleReplyEventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate {
                PeopleReply(e.QueryID, e.MatchedPeople); 
            });
        }

        //UI thread
        private void PeopleReply(UUID qqueryID, List<DirectoryManager.AgentSearchData> matchedPeople)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    PeopleReply(qqueryID, matchedPeople);
                }));

                return;
            }

            if (qqueryID != this.queryID) return;

            lvwFindPeople.BeginUpdate();

            foreach (DirectoryManager.AgentSearchData person in matchedPeople)
            {
                string fullName = person.FirstName + " " + person.LastName;

                if (!findPeopleResults.ContainsKey(fullName))
                {
                    findPeopleResults.Add(fullName, person.AgentID);
                }

                ListViewItem item = lvwFindPeople.Items.Add(fullName);
                item.SubItems.Add(person.Online ? "Yes" : "No");
            }

            lvwFindPeople.Sort();
            lvwFindPeople.EndUpdate();
            pPeople.Visible = false;  
        }

        public void ClearResults()
        {
            findPeopleResults.Clear();
            lvwFindPeople.Items.Clear();
        }

        private void lvwFindPeople_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedIndexChanged(e);
        }

        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndexChanged != null) SelectedIndexChanged(this, e);
        }

        public SafeDictionary<string, UUID> LLUUIDs
        {
            get { return findPeopleResults; }
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
                if (lvwFindPeople.SelectedItems == null) return -1;
                if (lvwFindPeople.SelectedItems.Count == 0) return -1;

                return lvwFindPeople.SelectedIndices[0];
            }
        }

        public string SelectedName
        {
            get
            {
                if (lvwFindPeople.SelectedItems == null) return string.Empty;
                if (lvwFindPeople.SelectedItems.Count == 0) return string.Empty;

                return lvwFindPeople.SelectedItems[0].Text;
            }
        }

        public bool SelectedOnlineStatus
        {
            get
            {
                if (lvwFindPeople.SelectedItems == null) return false;
                if (lvwFindPeople.SelectedItems.Count == 0) return false;

                string yesNo = lvwFindPeople.SelectedItems[0].SubItems[0].Text;

                if (yesNo == "Yes")
                    return true;
                else if (yesNo == "No")
                    return false;
                else
                    return false;
            }
        }

        public UUID SelectedAgentUUID
        {
            get
            {
                if (lvwFindPeople.SelectedItems == null) return UUID.Zero;
                if (lvwFindPeople.SelectedItems.Count == 0) return UUID.Zero;

                string name = lvwFindPeople.SelectedItems[0].Text;
                return findPeopleResults[name];
            }
        }

        private void pPeople_Click(object sender, EventArgs e)
        {

        }
    }
}
