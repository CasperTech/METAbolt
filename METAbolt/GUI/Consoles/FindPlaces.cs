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
    public partial class FindPlaces : UserControl
    {
        private METAboltInstance instance;
        //private SLNetCom netcom;
        private GridClient client;
        private float fX;
        private float fY;
        private float fZ;
        //private string sSIM;

        private UUID queryID;
        private SafeDictionary<string, DirectoryManager.DirectoryParcel> findPlacesResults;
        //private DirectoryManager.DirectoryParcel EmptyPlace;

        public event EventHandler SelectedIndexChanged;
        private NumericStringComparer lvwColumnSorter;

        public FindPlaces(METAboltInstance instance, UUID queryID)
        {
            InitializeComponent();

            findPlacesResults = new SafeDictionary<string, DirectoryManager.DirectoryParcel>();
            this.queryID = queryID;

            this.instance = instance;
            //netcom = this.instance.Netcom;
            client = this.instance.Client;
            AddClientEvents();

            lvwColumnSorter = new NumericStringComparer();
            lvwFindPlaces.ListViewItemSorter = lvwColumnSorter;
        }

        private void AddClientEvents()
        {
            client.Directory.DirPlacesReply += new EventHandler<DirPlacesReplyEventArgs>(Directory_OnPlacesReply);
        }

        //Separate thread
        private void Directory_OnPlacesReply(object sender, DirPlacesReplyEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    Directory_OnPlacesReply(sender, e);
                }));

                return;
            }

            BeginInvoke(new MethodInvoker(delegate()
            {
                PlacesReply(e.QueryID, e.MatchedParcels);
            }));
        }

        private void PlacesReply(UUID qqueryID, List<DirectoryManager.DirectoryParcel> matchedPlaces)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => PlacesReply(qqueryID, matchedPlaces)));
                return;
            }

            if (qqueryID != this.queryID) return;

            lvwFindPlaces.BeginUpdate();

            int icnt = 0;

            foreach (DirectoryManager.DirectoryParcel places in matchedPlaces)
            {
                try
                {
                    string fullName = places.Name;
                    bool fx = false;

                    if (findPlacesResults.ContainsKey(fullName))
                    {
                        //DirectoryManager.DirectoryParcel pcl = findPlacesResults[fullName];
                        fx = true; 
                    }

                    if (!fx)
                    {
                        findPlacesResults.Add(fullName, places);
                    }
                    else
                    {
                        fullName += " (" + icnt.ToString() + ")"; 
                        findPlacesResults.Add(fullName, places);
                    }

                    ListViewItem item = lvwFindPlaces.Items.Add(fullName);
                    item.SubItems.Add(places.Dwell.ToString());   // + "-" + events.Time);
                }
                catch
                {
                    ;
                }

                icnt += 1;
            }

            lvwFindPlaces.Sort();
            lvwFindPlaces.EndUpdate();
            pPlaces.Visible = false; 
        }

        // UI thread
        public void DisplayPlace(ParcelInfo place)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => DisplayPlace(place)));
                return;
            }

            if (place.Name == null)
                return;

            string sForSale = "";

            if (place.SalePrice > 0)
            {
                sForSale = "For Sale for L$" + place.SalePrice.ToString();   
            }

            txtName.Text = place.Name;

            txtDescription.Text = place.Description;
            txtInformation.Text = "Traffic: " + place.Dwell + " Area: " + place.ActualArea.ToString() + " sq. m. " + sForSale;


            // Convert Global pos to local
            float locX = (float)place.GlobalX; ;
            float locY = (float)place.GlobalY;
            float locX1;
            float locY1;
            Helpers.GlobalPosToRegionHandle(locX, locY, out locX1, out locY1);

            fX = locX1;
            fY = locY1;
            fZ = (float)place.GlobalZ;
            //sSIM = place.SimName;  

            txtLocation.Text = place.SimName.ToString() + " " + fX.ToString() + ", " + fY.ToString() + ", " + fZ.ToString();
        }

        public void ClearResults()
        {
            findPlacesResults.Clear();
            lvwFindPlaces.Items.Clear();
            button1.Enabled = false;
        }

        private void lvwFindPlaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedIndexChanged(e);

            button1.Enabled = true;
        }

        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndexChanged != null) SelectedIndexChanged(this, e);
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
                if (lvwFindPlaces.SelectedItems == null) return -1;
                if (lvwFindPlaces.SelectedItems.Count == 0) return -1;

                return lvwFindPlaces.SelectedIndices[0];
            }
        }

        public DirectoryManager.DirectoryParcel SelectedName
        {
            get
            {
                DirectoryManager.DirectoryParcel pcl = new DirectoryManager.DirectoryParcel();

                pcl.ID = UUID.Zero;
                pcl.Name = string.Empty;

                if (lvwFindPlaces.SelectedItems == null) return pcl;
                if (lvwFindPlaces.SelectedItems.Count == 0) return pcl;

                string name = lvwFindPlaces.SelectedItems[0].Text;
                return findPlacesResults[name];
            }
        }

        private void FindPlaces_Load(object sender, EventArgs e)
        {

        }

        private void lvwFindPlaces_ColumnClick(object sender, ColumnClickEventArgs e)
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
            lvwFindPlaces.Sort();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //(new frmTeleport(instance, sSIM, fX, fY, fZ)).ShowDialog();

            if (instance.State.IsSitting)
            {
                client.Self.Stand();
                instance.State.SetStanding();
            }

            Vector3 posn = new Vector3();
            posn.X = fX;
            posn.Y = fY;
            posn.Z = fZ;

            string sLoc = txtLocation.Text;

            char[] deli = " ".ToCharArray();
            string[] iDets = sLoc.Split(deli);

            (new frmTeleport(instance, iDets[0].ToString(), fX, fY, fZ, false)).Show();

            //client.Self.Teleport(sSIM, posn);    
        }
    }
}
