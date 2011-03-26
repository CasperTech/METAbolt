using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SLNetworkComm;
using System.Threading;
using OpenMetaverse;
using OpenMetaverse.Utilities;

namespace METAbolt
{
    public partial class frmLand : Form
    {
        private METAboltInstance instance;
        private GridClient client;
        public Dictionary<int, Parcel> LandResults = new Dictionary<int,Parcel>();
        public event EventHandler SelectedIndexChanged;
        private AutoResetEvent ParcelsDownloaded = new AutoResetEvent(false);



        public frmLand(METAboltInstance instance)
        {
            InitializeComponent();

            this.instance = instance;
            client = this.instance.Client;

            label2.Text = "";
            label3.Text = "";

            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            label9.Text = "";
            label10.Text = "";

            client.Network.OnDisconnected += new NetworkManager.DisconnectedCallback(Network_OnDisconnected);
            
            //client.Parcels.OnSimParcelsDownloaded += new ParcelManager.SimParcelsDownloaded(Parcel_OnSimParcelsDownloaded);
            //client.Parcels.RequestAllSimParcels(client.Network.CurrentSim);
            //client.Parcels.
        }

        private void GetParcels()
        {
            ParcelManager.SimParcelsDownloaded del = delegate(Simulator simulator, InternalDictionary<int, Parcel> simParcels, int[,] parcelMap)
            {
                ParcelsDownloaded.Set();
            };

            ParcelsDownloaded.Reset();
            client.Parcels.OnSimParcelsDownloaded += del;
            client.Parcels.RequestAllSimParcels(client.Network.CurrentSim);

            if (client.Network.CurrentSim.IsParcelMapFull())
                ParcelsDownloaded.Set();

            if (ParcelsDownloaded.WaitOne(30000, false) && client.Network.Connected)
            {
                //sb.AppendFormat("Downloaded {0} Parcels in {1} " + System.Environment.NewLine,
                    //.Network.CurrentSim.Parcels.Count, client.Network.CurrentSim.Name);

                int iParcels = 0;

                client.Network.CurrentSim.Parcels.ForEach(delegate(Parcel parcel)
                {
                    if ((parcel.Flags & Parcel.ParcelFlags.ForSale) == Parcel.ParcelFlags.ForSale)
                    {
                        try
                        {
                            BeginInvoke(new MethodInvoker(delegate()
                            {
                                if (!LandResults.ContainsKey(iParcels))
                                {
                                    LandResults.Add(iParcels, parcel);
                                    iParcels += 1;
                                }
                            }));

                            BeginInvoke(new MethodInvoker(EnableTimer));
                        }
                        catch (Exception exc)
                        {
                            // do nothing
                        }
                    }
                });

                PopulateList();
                progressBar1.Visible = false;
                client.Parcels.OnParcelProperties += new ParcelManager.ParcelPropertiesCallback(Parcels_OnParcelProperties);  
            }
            else
                //result = "Failed to retrieve information on all the simulator parcels";

            client.Parcels.OnSimParcelsDownloaded -= del;
            //return result;
        }

        private void Parcels_OnParcelProperties(Simulator simulator, Parcel parcel, ParcelResult result, int selectedPrims, int sequenceID, bool snapSelection)
        {
            string pcl = parcel.Name; 
            //this.parcel = parcel;
            //BeginInvoke(new MethodInvoker(UpdateLand));
        }

        // Seperate thread
        //private void Parcel_OnSimParcelsDownloaded(Simulator simulator, InternalDictionary<int, Parcel> simParcels, int[,] parcelMap)
        //{
        //    int iParcels = 0;
        //    //string pCount = "Total parcels: " + simParcels.Count.ToString();

        //    client.Network.CurrentSim.Parcels.ForEach(delegate(Parcel parcel)
        //    {
        //        if ((parcel.Flags & Parcel.ParcelFlags.ForSale) == Parcel.ParcelFlags.ForSale)
        //        {
        //            try
        //            {
        //                BeginInvoke(new MethodInvoker(delegate()
        //                {
        //                    if (!LandResults.ContainsKey(iParcels))
        //                    {
        //                        LandResults.Add(iParcels, parcel);
        //                        iParcels += 1;
        //                    }
        //                }));

        //                BeginInvoke(new MethodInvoker(EnableTimer));
        //            }
        //            catch (Exception exc)
        //            {
        //                // do nothing
        //            }
        //        }
        //    });
        //} 

        private void EnableTimer()
        {
            //timer1.Enabled = true;
        }


        private void Network_OnDisconnected(NetworkManager.DisconnectType reason, string message)
        {
            ResetObjects();
            ParcelsDownloaded.Set();
        }

        private void ResetObjects()
        {
            lvwFindLand.Items.Clear();
            LandResults.Clear();
            label1.Text = "";
            label2.Text = "";
            label3.Text = "";
            label4.Text = "";
            PopulateList();
        }

        private void frmLand_Load(object sender, EventArgs e)
        {
            label4.Text = "Retreiving parcels...";
            label1.Text = "SIM: " + client.Network.CurrentSim.Name.ToString();
            progressBar1.Visible = true;
            //PopulateList();
            GetParcels();
        }

        private void ShowDets()
        {
            int iDx = this.SelectedIndex;
            Parcel sPr;

            LandResults.TryGetValue(iDx, out sPr);

            string sLand = sPr.Name;
            Boolean isGroup = sPr.IsGroupOwned;
            int parcelid = sPr.LocalID;
            int iprims = sPr.MaxPrims;
            UUID iowner = sPr.OwnerID;
            string aid = sPr.AuctionID.ToString();


            label5.Text = "Owner UUID: " + iowner.ToString();
            label6.Text = "Group owned: " + isGroup.ToString();
            label7.Text = "max prims: " + iprims.ToString();
            label8.Text = "Description: " + sPr.Desc;
            label9.Text = "Auction ID: " + aid;

            if (sPr.AuthBuyerID.ToString() != "00000000-0000-0000-0000-000000000000")
                label10.Text = "On sale to: " + sPr.AuthBuyerID.ToString();
    
        }

        private void lvwFindLand_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedIndexChanged(e);
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            label9.Text = "";

            ShowDets();
        }

        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndexChanged != null) SelectedIndexChanged(this, e);
            
        }

        public Dictionary<int, Parcel> PARCELs
        {
            get { return LandResults; }
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

        public string SelectedLand
        {
            get
            {
                if (lvwFindLand.SelectedItems == null) return string.Empty;
                if (lvwFindLand.SelectedItems.Count == 0) return string.Empty;

                return lvwFindLand.SelectedItems[0].Text;
            }
        }

        private void PopulateList()
        {
            if (LandResults.Count > 0)
            {
                //if (LandResults.Count > 0)
                //{
                    label2.Text = "Total parcels on SIM: " + client.Network.CurrentSim.Parcels.Count.ToString();
                    label3.Text = "Parcels for sale: " + LandResults.Count.ToString();

                    Parcel sPr;
                    int i = 0;

                    lvwFindLand.BeginUpdate();

                    try
                    {

                        for (i = 0; i < LandResults.Count; i++)   //Iterate the dictionary.
                        {
                            LandResults.TryGetValue(i, out sPr);

                            string sstr = sPr.Name; // +" / " + sPr.Desc;

                            //if (sstr == "/")
                            //    sstr = "No information found";

                            ListViewItem item = lvwFindLand.Items.Add(sstr);
                            item.SubItems.Add(sPr.Area.ToString());
                            string sPrice = "L$" + sPr.SalePrice.ToString();
                            item.SubItems.Add(sPrice, Color.Red, Color.LightYellow, null);
                        }
                        //}
                    }
                    catch (Exception exc)
                    { 
                        // do nothing
                    }
            }
            else
            {
                if (client.Network.CurrentSim.Parcels.Count > 1)
                {
                    timer1.Enabled = false;

                    label2.Text = "Total parcels on SIM: " + client.Network.CurrentSim.Parcels.Count.ToString();

                    //Parcel sPr;
                    int i = 0;

                    lvwFindLand.BeginUpdate();

                    try
                    {

                    client.Network.CurrentSim.Parcels.ForEach(delegate(Parcel parcel)   //Iterate the parcels.
                    {
                        if ((parcel.Flags & Parcel.ParcelFlags.ForSale) == Parcel.ParcelFlags.ForSale)
                        {
                            if (!LandResults.ContainsKey(i))
                            {
                                string sstr = parcel.Name; // +" / " + parcel.Desc;

                                //if (sstr == "/")
                                //    sstr = "No information found"; 

                                ListViewItem item = lvwFindLand.Items.Add(sstr);
                                item.SubItems.Add(parcel.Area.ToString());
                                string sPrice = "L$" + parcel.SalePrice.ToString();
                                item.SubItems.Add(sPrice, Color.Red, Color.LightYellow, null);

                                LandResults.Add(i, parcel);

                                i += 1;
                            }
                        }
                    });
                    }
                    catch (Exception exc)
                    {
                        // do nothing
                    }

                    label3.Text = "Parcels for sale: " + i.ToString();
                    progressBar1.Visible = false;
                    label4.Text = "";
                } 
            }

            lvwFindLand.Sort();
            lvwFindLand.EndUpdate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //label4.Text = "";

            //PopulateList();
            //if (lvwFindLand.Items.Count == 0)
            //{
            //    label4.Text = "Data coming in too slow. Close down this screen & re-enter.";
            //}

            //progressBar1.Visible = false;
            //timer1.Enabled = false;
        }

        private void frmLand_FormClosing(object sender, FormClosingEventArgs e)
        {
            lvwFindLand.Clear();
        }

        private void lvwFindLand_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.lvwFindLand.ListViewItemSorter = new ListViewItemComparer(e.Column);
            // Call the sort method to manually sort.
            lvwFindLand.Sort();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
