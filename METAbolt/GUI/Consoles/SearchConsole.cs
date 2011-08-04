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
//using SLNetworkComm;
using System.Globalization;


namespace METAbolt
{
    public partial class SearchConsole : UserControl
    {
        private METAboltInstance instance;
        //private SLNetCom netcom;
        private GridClient client;

        private TabsConsole tabConsole;
        private FindPeopleConsole console;
        private FindEvents eventsconsole;
        private FindPlaces placesconsole;
        private FindGroups groupsconsole;
        private FindLand landconsole;

        private string lastQuery = string.Empty;
        private int startResult = 0;
        private UUID requestedgroupid = UUID.Zero;  

        private int totalResults = 0;
        private WebBrowser webBrowser;
        private string clickedurl;


        public SearchConsole(METAboltInstance instance)
        {
            InitializeComponent();

            this.instance = instance;
            //netcom = this.instance.Netcom;
            client = this.instance.Client;
            AddClientEvents();

            tabConsole = this.instance.TabConsole;

            console = new FindPeopleConsole(instance, UUID.Random());
            console.Dock = DockStyle.Fill;
            console.SelectedIndexChanged += new EventHandler(console_SelectedIndexChanged);
            pnlFindPeople.Controls.Add(console);

            eventsconsole = new FindEvents(instance, UUID.Random());
            eventsconsole.Dock = DockStyle.Fill;
            eventsconsole.SelectedIndexChanged += new EventHandler(eventsconsole_SelectedIndexChanged);
            pnlFindEvents.Controls.Add(eventsconsole);

            placesconsole = new FindPlaces(instance, UUID.Random());
            placesconsole.Dock = DockStyle.Fill;
            placesconsole.SelectedIndexChanged += new EventHandler(placesconsole_SelectedIndexChanged);
            pnlFindPlaces.Controls.Add(placesconsole);

            groupsconsole = new FindGroups(instance, UUID.Random());
            groupsconsole.Dock = DockStyle.Fill;
            groupsconsole.SelectedIndexChanged += new EventHandler(groupsconsole_SelectedIndexChanged);
            pnlFindGroups.Controls.Add(groupsconsole);

            landconsole = new FindLand(instance, UUID.Random());
            landconsole.Dock = DockStyle.Fill;
            landconsole.SelectedIndexChanged += new EventHandler(landconsole_SelectedIndexChanged);
            pnlFindLand.Controls.Add(landconsole);

            webBrowser = new WebBrowser();
            webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted);
            webBrowser.Navigating += new WebBrowserNavigatingEventHandler(webBrowser_Navigating);
            webBrowser.Url = new Uri("http://www.metabolt.net/metasearch.aspx");
            webBrowser.AllowNavigation = true;
            webBrowser.AllowWebBrowserDrop = false;
            webBrowser.Dock = DockStyle.Fill;
            webBrowser.IsWebBrowserContextMenuEnabled = false;
            webBrowser.ScriptErrorsSuppressed = true;
            webBrowser.ScrollBarsEnabled = true;
            webBrowser.NewWindow += new CancelEventHandler(webBrowser_NewWindow);
            panel4.Controls.Add(webBrowser);

            Disposed += new EventHandler(SearchConsole_Disposed);

            PopulateCbos();
        }

        private void PopulateCbos()
        {
            cboCategory.Items.Add(DirectoryManager.SearchTypeFlags.Any);
            cboCategory.Items.Add(DirectoryManager.SearchTypeFlags.Mainland);
            cboCategory.Items.Add(DirectoryManager.SearchTypeFlags.Estate);
            cboCategory.Items.Add(DirectoryManager.SearchTypeFlags.Auction);
            cboCategory.SelectedIndex = 0;

            cboArea.Items.Add("Any Size");
            cboArea.Items.Add("> 128 SqM");
            cboArea.Items.Add("> 500 SqM");
            cboArea.Items.Add("> 1k SqM");
            cboArea.Items.Add("> 4k SqM");
            cboArea.Items.Add("> 8k SqM");
            cboArea.Items.Add("> 16k SqM");
            cboArea.Items.Add("> 32k SqM");
            cboArea.SelectedIndex = 0;

            cboPrice.Items.Add("Any Price");
            cboPrice.Items.Add("< 50 L$");
            cboPrice.Items.Add("< 100 L$");
            cboPrice.Items.Add("< 1k L$");
            cboPrice.Items.Add("< 5k L$");
            cboPrice.Items.Add("< 10k L$");
            cboPrice.Items.Add("> 10k L$");
            cboPrice.SelectedIndex = 0;
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            HtmlElementCollection links = webBrowser.Document.Links;

            foreach (HtmlElement var in links)
            {
                var.AttachEventHandler("onclick", LinkClicked);
            }
        }

        private void webBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;

            HtmlElement link = webBrowser.Document.ActiveElement;
            clickedurl = link.GetAttribute("href");

            string murl = "http://www.metabolt.net/METAsearchRedirect.asp?URL=" + clickedurl;
            System.Diagnostics.Process.Start(murl);
        }

        private void LinkClicked(object sender, EventArgs e)
        {

            HtmlElement link = webBrowser.Document.ActiveElement;
            clickedurl = link.GetAttribute("href");
        }

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            //clickedurl = e.Url.ToString();
        }

        private void console_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (console.SelectedName == client.Self.Name)
            {
                btnNewIM.Enabled = btnNewIM.Enabled = button10.Enabled = btnFriend.Enabled = false;
                btnProfile.Enabled = true; 
            }
            else
            {
                btnNewIM.Enabled = btnNewIM.Enabled = button10.Enabled = btnFriend.Enabled = btnProfile.Enabled = (console.SelectedName != null);
            }
        }

        private void eventsconsole_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint eid = eventsconsole.SelectedEventUUID;
            client.Directory.EventInfoRequest(eid);
        }

        private void placesconsole_SelectedIndexChanged(object sender, EventArgs e)
        {
            client.Parcels.ParcelInfoReply += new EventHandler<ParcelInfoReplyEventArgs>(Parcels_OnParcelInfoReply);

            if (placesconsole.SelectedName.ID != UUID.Zero)
            {
                UUID pid = placesconsole.SelectedName.ID;
                client.Parcels.RequestParcelInfo(pid);
            }
        }

        private void Parcels_OnParcelInfoReply(object sender, ParcelInfoReplyEventArgs e)
        {
            client.Parcels.ParcelInfoReply -= new EventHandler<ParcelInfoReplyEventArgs>(Parcels_OnParcelInfoReply);
            placesconsole.DisplayPlace(e.Parcel);
        }

        private void landconsole_SelectedIndexChanged(object sender, EventArgs e)
        {
            client.Parcels.ParcelInfoReply += new EventHandler<ParcelInfoReplyEventArgs>(Land_OnParcelInfoReply);

            if (landconsole.SelectedName.ID != UUID.Zero)
            {
                UUID pid = landconsole.SelectedName.ID;
                client.Parcels.RequestParcelInfo(pid);
            }
        }

        private void Land_OnParcelInfoReply(object sender, ParcelInfoReplyEventArgs e)
        {
            client.Parcels.ParcelInfoReply -= new EventHandler<ParcelInfoReplyEventArgs>(Land_OnParcelInfoReply);
            landconsole.DisplayPlace(e.Parcel);
        }

        private void groupsconsole_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnJoin.Enabled = cmdInfo.Enabled = (groupsconsole.SelectedName != null);
        }

        private void AddClientEvents()
        {
            client.Directory.DirPeopleReply += new EventHandler<DirPeopleReplyEventArgs>(Directory_OnDirPeopleReply);
            client.Directory.DirEventsReply += new EventHandler<DirEventsReplyEventArgs>(Directory_OnEventsReply);
            client.Directory.DirPlacesReply += new EventHandler<DirPlacesReplyEventArgs>(Directory_OnDirPlacesReply);
            client.Directory.DirGroupsReply += new EventHandler<DirGroupsReplyEventArgs>(Directory_OnDirGroupsReply);
            client.Directory.DirLandReply += new EventHandler<DirLandReplyEventArgs>(Directory_OnDirLandReply);
        }

        private void SearchConsole_Disposed(object sender, EventArgs e)
        {
            client.Directory.DirPeopleReply -= new EventHandler<DirPeopleReplyEventArgs>(Directory_OnDirPeopleReply);
            client.Directory.DirEventsReply -= new EventHandler<DirEventsReplyEventArgs>(Directory_OnEventsReply);
            client.Directory.DirPlacesReply -= new EventHandler<DirPlacesReplyEventArgs>(Directory_OnDirPlacesReply);
            client.Directory.DirGroupsReply -= new EventHandler<DirGroupsReplyEventArgs>(Directory_OnDirGroupsReply);
            client.Directory.DirLandReply -= new EventHandler<DirLandReplyEventArgs>(Directory_OnDirLandReply);
            webBrowser.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted);
            webBrowser.Navigating -= new WebBrowserNavigatingEventHandler(webBrowser_Navigating);
        }

        //Separate thread
        private void Directory_OnDirPeopleReply(object sender, DirPeopleReplyEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { Directory_OnDirPeopleReply(sender, e); });
                return;
            }

            BeginInvoke((MethodInvoker)delegate { PeopleReply(e.QueryID, e.MatchedPeople); });
        }

        private void Directory_OnEventsReply(object sender, DirEventsReplyEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { Directory_OnEventsReply(sender, e); });
                return;
            }

            BeginInvoke((MethodInvoker)delegate { EventsReply(e.QueryID, e.MatchedEvents); });
        }

        private void Directory_OnDirPlacesReply(object sender, DirPlacesReplyEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { Directory_OnDirPlacesReply(sender, e); });
                return;
            }

            BeginInvoke((MethodInvoker)delegate { PlacesReply(e.QueryID, e.MatchedParcels); });
        }

        private void Directory_OnDirLandReply(object sender, DirLandReplyEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { Directory_OnDirLandReply(sender, e); });
                return;
            }

            BeginInvoke((MethodInvoker)delegate { LandReply(e.DirParcels); });
        }

        private void Directory_OnDirGroupsReply(object sender, DirGroupsReplyEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { Directory_OnDirGroupsReply(sender, e); });
                return;
            }

            BeginInvoke((MethodInvoker)delegate { GroupsReply(e.QueryID, e.MatchedGroups); });
        }

        private void GroupsReply(UUID queryID, List<DirectoryManager.GroupSearchData> matchedGroups)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { GroupsReply(queryID, matchedGroups); });
                return;
            }

            if (groupsconsole.QueryID != queryID) return;

            totalResults += matchedGroups.Count;
            lblGroupsFound.Text = totalResults.ToString(CultureInfo.CurrentCulture) + " groups found";

            txtGroups.Enabled = true;
            btnFindGroups.Enabled = true;

            btnNextGroups.Enabled = (totalResults > 100);
            btnPrevGroups.Enabled = (startResult > 0);
        }

        //UI thread
        private void PeopleReply(UUID queryID, List<DirectoryManager.AgentSearchData> matchedPeople)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { PeopleReply(queryID, matchedPeople); });
                return;
            }

            if (console.QueryID != queryID) return;

            totalResults += matchedPeople.Count;
            lblResultCount.Text = totalResults.ToString(CultureInfo.CurrentCulture) + " people found";

            txtPersonName.Enabled = true;
            btnFind.Enabled = true;

            btnNext.Enabled = (totalResults > 100);
            btnPrevious.Enabled = (startResult > 0);
        }

        private void EventsReply(UUID queryID, List<DirectoryManager.EventsSearchData> matchedEvents)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { EventsReply(queryID, matchedEvents); });
                return;
            }

            if (eventsconsole.QueryID != queryID) return;

            totalResults += matchedEvents.Count;
            lblEventsCount.Text = totalResults.ToString(CultureInfo.CurrentCulture) + " events found";

            txtEvents.Enabled = true;
            btnFindEvents.Enabled = true;

            btnNextEvents.Enabled = (totalResults > 100);
            btnPrevEvents.Enabled = (startResult > 0);
        }

        private void PlacesReply(UUID queryID, List<DirectoryManager.DirectoryParcel> matchedPlaces)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { PlacesReply(queryID, matchedPlaces); });
                return;
            }

            if (placesconsole.QueryID != queryID) return;

            totalResults += matchedPlaces.Count;
            lblPlacesCount.Text = totalResults.ToString(CultureInfo.CurrentCulture) + " places found";

            txtPlaces.Enabled = true;
            btnFindPlaces.Enabled = true;

            btnNextPlaces.Enabled = (totalResults > 100);
            btnPrevPlaces.Enabled = (startResult > 0);
        }

        private void LandReply(List<DirectoryManager.DirectoryParcel> matchedPlaces)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { LandReply(matchedPlaces); });
                return;
            }

            //if (placesconsole.QueryID != queryID) return;

            totalResults += matchedPlaces.Count;
            lblLandCount.Text = totalResults.ToString(CultureInfo.CurrentCulture) + " parcels found";

            txtLand.Enabled = true;
            btnFindLand.Enabled = true;

            btnNextLand.Enabled = (totalResults > 100);
            btnPrevLand.Enabled = (startResult > 0);
        }

        private void txtPersonName_TextChanged(object sender, EventArgs e)
        {
            btnFind.Enabled = (txtPersonName.Text.Trim().Length > 2);
        }

        private void txtEvents_TextChanged(object sender, EventArgs e)
        {
            btnFindEvents.Enabled = (txtEvents.Text.Trim().Length > 2);
        }

        private void txtPlaces_TextChanged(object sender, EventArgs e)
        {
            btnFindPlaces.Enabled = (txtPlaces.Text.Trim().Length > 2);
        }

        private void txtGroups_TextChanged(object sender, EventArgs e)
        {
            btnFindGroups.Enabled = (txtGroups.Text.Trim().Length > 2);
        }

        private void btnNewIM_Click(object sender, EventArgs e)
        {
            // V 0.9.1.6 change
            if (console.SelectedName == client.Self.Name)
                return;
            // end

            if (tabConsole.TabExists(console.SelectedName))
            {
                tabConsole.SelectTab(console.SelectedName);
                return;
            }

            tabConsole.AddIMTab(console.SelectedAgentUUID, client.Self.AgentID ^ console.SelectedAgentUUID, console.SelectedName);
            tabConsole.SelectTab(console.SelectedName);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            lastQuery = txtPersonName.Text;
            startResult = 0;
            StartFinding();
        }

        private void btnFindEvents_Click(object sender, EventArgs e)
        {
            lastQuery = txtEvents.Text;
            startResult = 0;
            //totalResults = 0;
            StartFindingEvents();
        }

        private void btnFindPlaces_Click(object sender, EventArgs e)
        {
            lastQuery = txtPlaces.Text;
            startResult = 0;
            //totalResults = 0;
            StartFindingPlaces();
        }

        private void btnFindGroups_Click(object sender, EventArgs e)
        {
            lastQuery = txtGroups.Text;
            startResult = 0;
            //totalResults = 0;
            StartFindingGroups();
        }

        private void txtPersonName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            if (txtPersonName.Text.Trim().Length < 3) return;

            lastQuery = txtPersonName.Text;
            startResult = 0;
            StartFinding();
        }

        private void StartFinding()
        {
            console.Controls["pPeople"].Visible = true;

            totalResults = 0;
            lblResultCount.Text = "Searching for " + lastQuery;

            txtPersonName.Enabled = false;
            btnFind.Enabled = false;
            btnNewIM.Enabled = false;
            btnProfile.Enabled = false;
            btnFriend.Enabled = false;
            btnPrevious.Enabled = false;
            btnNext.Enabled = false;

            try
            {
                console.ClearResults();
                console.QueryID = client.Directory.StartPeopleSearch(lastQuery, startResult);
            }
            catch
            {
                ;
            }
        }

        private void StartFindingEvents()
        {
            eventsconsole.Controls["pEvents"].Visible = true;
   
            totalResults = 0;
            lblEventsCount.Text = "Searching for " + lastQuery;

            txtEvents.Enabled = false;
            btnFindEvents.Enabled = false;
            btnPrevEvents.Enabled = false;
            btnNextEvents.Enabled = false;

            try
            {
                eventsconsole.ClearResults();
                eventsconsole.QueryID = client.Directory.StartEventsSearch(
                    lastQuery,
                    (uint)startResult);
            }
            catch
            {
                ;
            }
        }

        private void StartFindingPlaces()
        {
            placesconsole.Controls["pPlaces"].Visible = true;

            totalResults = 0;
            lblPlacesCount.Text = "Searching for " + lastQuery;

            txtPlaces.Enabled = false;
            btnFindPlaces.Enabled = false;
            btnPrevPlaces.Enabled = false;
            btnNextPlaces.Enabled = false;

            try
            {
                placesconsole.ClearResults();
                //placesconsole.QueryID = client.Directory.StartPlacesSearch(
                //    DirectoryManager.DirFindFlags.NameSort,
                //    ParcelCategory.Any,
                //    lastQuery,
                //    String.Empty,
                //    UUID.Zero,
                //    UUID.Random());

                placesconsole.QueryID = client.Directory.StartDirPlacesSearch(lastQuery, 
                    DirectoryManager.DirFindFlags.NameSort, 
                    ParcelCategory.Any,
                    startResult);
            }
            catch
            {
                ;
            }
        }

        private void StartFindingGroups()
        {
            groupsconsole.Controls["pGroups"].Visible = true;
            totalResults = 0;

            lblGroupsFound.Text = "Searching for " + lastQuery;

            txtGroups.Enabled = false;
            btnFindGroups.Enabled = false;
            btnNextGroups.Enabled = false;
            btnPrevGroups.Enabled = false;

            try
            {
                groupsconsole.ClearResults();
                groupsconsole.QueryID = client.Directory.StartGroupSearch(
                    lastQuery,
                    startResult,
                    DirectoryManager.DirFindFlags.NameSort |
                    DirectoryManager.DirFindFlags.SortAsc |
                    DirectoryManager.DirFindFlags.Groups
                    );
            }
            catch
            {
                ;
            }

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            startResult += 100;
            StartFinding();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            startResult -= 100;
            StartFinding();
        }

        private void btnNextEvents_Click(object sender, EventArgs e)
        {
            startResult += 100;
            StartFindingEvents();
        }

        private void btnPrevEvents_Click(object sender, EventArgs e)
        {
            startResult -= 100;
            StartFindingEvents();
        }

        private void btnPrevPlaces_Click(object sender, EventArgs e)
        {
            startResult -= 100;
            StartFindingPlaces();
        }

        private void btnNextPlaces_Click(object sender, EventArgs e)
        {
            startResult += 100;
            StartFindingPlaces();
        }

        private void btnNextGroups_Click(object sender, EventArgs e)
        {
            startResult += 100;
            StartFindingGroups();
        }

        private void btnPrevGroups_Click(object sender, EventArgs e)
        {
            startResult -= 100;
            StartFindingGroups();
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            (new frmProfile(instance, console.SelectedName, console.SelectedAgentUUID)).Show();
        }

        private void txtPersonName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) e.SuppressKeyPress = true;
        }

        private void txtEvents_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) e.SuppressKeyPress = true;
        }

        private void btnFriend_Click(object sender, EventArgs e)
        {
            // V 0.9.1.6 change
            if (console.SelectedName == client.Self.Name)
                return;
            // end
 
            string  sAvName = console.SelectedName;
            if (console.SelectedAgentUUID == null) return;

            Boolean fFound = true;

            client.Friends.FriendList.ForEach(delegate(FriendInfo friend)
            {
                if (friend.Name == sAvName)
                {
                    fFound = false;
                }
            });

            if (fFound)
            {
                client.Friends.OfferFriendship(console.SelectedAgentUUID);
            }
        }

        private void txtEvents_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            if (txtEvents.Text.Trim().Length < 3) return;

            lastQuery = txtEvents.Text;
            startResult = 0;
            StartFindingEvents();
        }

        private void txtPlaces_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) e.SuppressKeyPress = true;
        }
 
        private void txtPlaces_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            if (txtPlaces.Text.Trim().Length < 3) return;

            lastQuery = txtPlaces.Text;
            startResult = 0;
            StartFindingPlaces();
        }

        private void txtGroups_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) e.SuppressKeyPress = true;
        }

        private void txtGroups_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            if (txtGroups.Text.Trim().Length < 3) return;

            lastQuery = txtGroups.Text;
            startResult = 0;
            StartFindingGroups();
        }


        private void pnlFindEvents_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlFindPlaces_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            // V 0.9.1.6 change
            if (console.SelectedName == client.Self.Name)
                return;
            // end

            if (console.SelectedAgentUUID == null) return;

            client.Parcels.EjectUser(console.SelectedAgentUUID, true);
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            if (groupsconsole.SelectedName == null) return;

            client.Groups.GroupJoinedReply += new EventHandler<GroupOperationEventArgs>(Groups_OnGroupJoined);
            client.Groups.RequestJoinGroup(groupsconsole.SelectedGroupUUID);
        }

        void Groups_OnGroupJoined(object sender, GroupOperationEventArgs e)
        {
            client.Groups.GroupJoinedReply -= new EventHandler<GroupOperationEventArgs>(Groups_OnGroupJoined);
        }

        private void pnlFindPeople_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GroupProfileHandler(object sender, GroupProfileEventArgs e)
        {
            if (e.Group.ID != requestedgroupid) return;

            BeginInvoke(new MethodInvoker(delegate()
            {
                client.Groups.GroupProfile -= new EventHandler<GroupProfileEventArgs>(GroupProfileHandler);

                frmGroupInfo frm = new frmGroupInfo(e.Group, instance);
                frm.ShowDialog();
            }));
        }

        private void cmdInfo_Click(object sender, EventArgs e)
        {
            if (groupsconsole.SelectedName == null) return;

            client.Groups.GroupProfile += new EventHandler<GroupProfileEventArgs>(GroupProfileHandler);

            requestedgroupid = groupsconsole.SelectedGroupUUID;

            client.Groups.RequestGroupProfile(requestedgroupid);
        }

        private void btnNextEvents_Click_1(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnFindLand_Click(object sender, EventArgs e)
        {
            lastQuery = txtLand.Text;
            startResult = 0;
            //totalResults = 0;
            StartFindingLand();
        }

        private void StartFindingLand()
        {
            landconsole.Controls["pLand"].Visible = true;

            totalResults = 0;
            lblLandCount.Text = "Searching for " + lastQuery;

            txtLand.Enabled = false;
            btnFindLand.Enabled = false;
            btnPrevLand.Enabled = false;
            btnNextLand.Enabled = false;

            try
            {
                landconsole.ClearResults();

                DirectoryManager.SearchTypeFlags sflag = (DirectoryManager.SearchTypeFlags)cboCategory.SelectedItem;

                DirectoryManager.DirFindFlags  dflag = DirectoryManager.DirFindFlags.SortAsc | DirectoryManager.DirFindFlags.ForSale | DirectoryManager.DirFindFlags.PerMeterSort;

                if (checkBox1.Checked)
                {
                    dflag = dflag | DirectoryManager.DirFindFlags.IncludePG;
                }

                if (checkBox2.Checked)
                {
                    dflag = dflag | DirectoryManager.DirFindFlags.IncludeMature;
                }

                if (checkBox3.Checked)
                {
                    dflag = dflag | DirectoryManager.DirFindFlags.IncludeAdult;
                }


                int area = 0;
                int price = 0;

                switch (cboArea.SelectedIndex)
                {
                    case 0:
                        area = 65000;
                        break;

                    case 1:
                        area = 128;
                        break;

                    case 2:
                        area = 500;
                        break;

                    case 3:
                        area = 1000;
                        break;

                    case 4:
                        area = 4000;
                        break;

                    case 5:
                        area = 8000;
                        break;

                    case 6:
                        area = 16000;
                        break;

                    case 7:
                        area = 32000;
                        break; 
                }

                switch (cboPrice.SelectedIndex)
                {
                    case 0:
                        price = 10000000;
                        break;

                    case 1:
                        price = 50;
                        break;

                    case 2:
                        price = 100;
                        break;

                    case 3:
                        price = 1000;
                        break;

                    case 4:
                        price = 5000;
                        break;

                    case 5:
                        price = 10000;
                        break;

                    case 6:
                        price = 10000000;
                        break;
                }

                if (cboArea.SelectedIndex == 0 && cboPrice.SelectedIndex == 0)
                {
                    client.Directory.StartLandSearch(dflag, sflag, price, area, startResult);
                }
                else
                {
                    if (cboArea.SelectedIndex == 0 && cboPrice.SelectedIndex != 0)
                    {
                        client.Directory.StartLandSearch(DirectoryManager.DirFindFlags.SortAsc | DirectoryManager.DirFindFlags.ForSale | DirectoryManager.DirFindFlags.LimitByPrice | DirectoryManager.DirFindFlags.PerMeterSort | DirectoryManager.DirFindFlags.IncludeMature | DirectoryManager.DirFindFlags.IncludeAdult | DirectoryManager.DirFindFlags.IncludePG, sflag, price, area, startResult);
                    }
                    else
                    {
                        if (cboArea.SelectedIndex != 0 && cboPrice.SelectedIndex == 0)
                        {
                            client.Directory.StartLandSearch(dflag | DirectoryManager.DirFindFlags.LimitByArea, sflag, price, area, startResult);
                        }
                        else
                        {
                            client.Directory.StartLandSearch(dflag | DirectoryManager.DirFindFlags.LimitByArea | DirectoryManager.DirFindFlags.LimitByPrice, sflag, price, area, startResult);
                            //client.Directory.StartLandSearch(DirectoryManager.DirFindFlags.ForSale | DirectoryManager.DirFindFlags.PerMeterSort | DirectoryManager.DirFindFlags.IncludeMature | DirectoryManager.DirFindFlags.IncludeAdult | DirectoryManager.DirFindFlags.IncludePG, sflag, price, area, startResult);
                        }
                    }
                }
            }
            catch
            {
                ;
            }
        }

        private void btnNextLand_Click(object sender, EventArgs e)
        {
            startResult += 100;
            StartFindingLand();
        }

        private void btnPrevLand_Click(object sender, EventArgs e)
        {
            startResult -= 100;
            StartFindingLand();
        }
    }
}
