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
using System.Linq;
using System.Globalization;

namespace METAbolt
{
    public partial class FindLand : UserControl
    {
        private METAboltInstance instance;
        //private SLNetCom netcom;
        private GridClient client;
        private float fX;
        private float fY;
        private float fZ;
        //private string sSIM;

        private UUID queryID;
        private SafeDictionary<string, DirectoryManager.DirectoryParcel> findLandResults;
        //private DirectoryManager.DirectoryParcel EmptyPlace;

        public event EventHandler SelectedIndexChanged;
        private NumericStringComparer lvwColumnSorter;

        public FindLand(METAboltInstance instance, UUID queryID)
        {
            InitializeComponent();

            findLandResults = new SafeDictionary<string, DirectoryManager.DirectoryParcel>();
            this.queryID = queryID;

            this.instance = instance;
            //netcom = this.instance.Netcom;
            client = this.instance.Client;

            AddClientEvents();

            lvwColumnSorter = new NumericStringComparer();
            lvwFindLand.ListViewItemSorter = lvwColumnSorter;
        }

        private void AddClientEvents()
        {
            client.Directory.DirLandReply += new EventHandler<DirLandReplyEventArgs>(Directory_OnLandReply);
        }

        //Separate thread
        private void Directory_OnLandReply(object sender, DirLandReplyEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    Directory_OnLandReply(sender, e);
                }));

                return;
            }

            BeginInvoke(new MethodInvoker(delegate()
            {
                LandReply(e.DirParcels);
            }));
        }

        private void LandReply(List<DirectoryManager.DirectoryParcel> matchedPlaces)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => LandReply(matchedPlaces)));
                return;
            }

            //if (queryID != this.queryID) return;

            lvwFindLand.BeginUpdate();

            int icnt = 0;

            foreach (DirectoryManager.DirectoryParcel places in matchedPlaces)
            {
                try
                {
                    string fullName = places.Name;
                    bool fx = false;

                    if (findLandResults.ContainsKey(fullName))
                    {
                        //DirectoryManager.DirectoryParcel pcl = findLandResults[fullName];
                        fx = true; 
                    }

                    if (!fx)
                    {
                        findLandResults.Add(fullName, places);
                    }
                    else
                    {
                        fullName += " (" + icnt.ToString(CultureInfo.CurrentCulture) + ")";
                        findLandResults.Add(fullName, places);
                    }

                    //ListViewItem item = lvwFindLand.Items.Add(fullName);
                    //item.SubItems.Add(places.ActualArea.ToString());
                    //item.SubItems.Add(places.SalePrice.ToString());
                      
                    //double pricesqm = (Convert.ToDouble(places.SalePrice) / Convert.ToDouble(places.ActualArea));
                    //item.SubItems.Add(pricesqm.ToString("N2"));
                }
                catch
                {
                    ;
                }

                icnt += 1;
            }

            var items = from k in findLandResults.Keys
                        orderby (Convert.ToDouble(findLandResults[k].SalePrice) / Convert.ToDouble(findLandResults[k].ActualArea)) ascending
                        select k;

            foreach (string k in items)
            {
                ListViewItem item = lvwFindLand.Items.Add(k);
                item.SubItems.Add(findLandResults[k].ActualArea.ToString(CultureInfo.CurrentCulture));
                item.SubItems.Add(findLandResults[k].SalePrice.ToString(CultureInfo.CurrentCulture));

                double pricesqm = (Convert.ToDouble(findLandResults[k].SalePrice) / Convert.ToDouble(findLandResults[k].ActualArea));
                item.SubItems.Add(pricesqm.ToString("N3", CultureInfo.CurrentCulture));
            }

            //lvwFindLand.Sort();
            lvwFindLand.EndUpdate();
            pLand.Visible = false; 
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
                sForSale = "For Sale for L$" + place.SalePrice.ToString(CultureInfo.CurrentCulture);   
            }

            txtName.Text = place.Name;

            txtDescription.Text = place.Description;
            txtInformation.Text = "Traffic: " + place.Dwell + " Area: " + place.ActualArea.ToString(CultureInfo.CurrentCulture) + " sq. m. " + sForSale;
            chkMature.Checked = place.Mature;   

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

            txtLocation.Text = place.SimName.ToString(CultureInfo.CurrentCulture) + " " + fX.ToString(CultureInfo.CurrentCulture) + ", " + fY.ToString(CultureInfo.CurrentCulture) + ", " + fZ.ToString(CultureInfo.CurrentCulture);
        }

        public void ClearResults()
        {
            findLandResults.Clear();
            lvwFindLand.Items.Clear();
            button1.Enabled = false;
        }

        private void lvwFindLand_SelectedIndexChanged(object sender, EventArgs e)
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
                if (lvwFindLand.SelectedItems == null) return -1;
                if (lvwFindLand.SelectedItems.Count == 0) return -1;

                return lvwFindLand.SelectedIndices[0];
            }
        }

        public DirectoryManager.DirectoryParcel SelectedName
        {
            get
            {
                DirectoryManager.DirectoryParcel pcl = new DirectoryManager.DirectoryParcel();

                pcl.ID = UUID.Zero;
                pcl.Name = string.Empty;

                if (lvwFindLand.SelectedItems == null) return pcl;
                if (lvwFindLand.SelectedItems.Count == 0) return pcl;

                string name = lvwFindLand.SelectedItems[0].Text;
                return findLandResults[name];
            }
        }

        private void lvwFindLand_ColumnClick(object sender, ColumnClickEventArgs e)
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
            lvwFindLand.Sort();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
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
        }
    }
}
