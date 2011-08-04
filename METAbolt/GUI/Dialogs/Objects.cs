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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
using SLNetworkComm;
using OpenMetaverse.Packets;
using PopupControl;
using ExceptionReporting;
using System.Threading;
using System.Timers;
using System.Globalization;


namespace METAbolt
{
    public partial class frmObjects : Form
    {
        private METAboltInstance instance;
        private GridClient client;
        private SLNetCom netcom;

        //Quaternion BodyRotation = Quaternion.Identity;
        //Quaternion HeadRotation = Quaternion.Identity;
        //private Quaternion LastBodyRotation;
        //private Quaternion LastHeadRotation;
        public AgentFlags Flags = AgentFlags.None;
        public AgentState State = AgentState.None;
        //private int duplicateCount;
        private bool sloading;
        private float range = 20;
        private float newrange = 20;

        private SafeDictionary<uint, ObjectsListItem> listItems = new SafeDictionary<uint, ObjectsListItem>();
        private SafeDictionary<uint, ObjectsListItem> ItemsProps = new SafeDictionary<uint, ObjectsListItem>();
        private SafeDictionary<uint, ObjectsListItem> childItems = new SafeDictionary<uint, ObjectsListItem>();
        private List<uint> objs = new List<uint>();
        private SafeDictionary<UUID, string> avatars = new SafeDictionary<UUID, string>();

        private Popup toolTip;
        private Popup toolTip1;
        private CustomToolTip customToolTip;
        private bool eventsremoved = false;
        private ExceptionReporter reporter = new ExceptionReporter();
        //private System.Timers.Timer sittimer;


        internal class ThreadExceptionHandler
        {
            public void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
            {
                ExceptionReporter reporter = new ExceptionReporter();
                reporter.Show(e.Exception);
            }
        }

        public frmObjects(METAboltInstance instance)
        {
            InitializeComponent();

            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.instance = instance;
            client = this.instance.Client;
            netcom = this.instance.Netcom;

            client.Network.Disconnected += new EventHandler<DisconnectedEventArgs>(Network_OnDisconnected);
            client.Avatars.UUIDNameReply += new EventHandler<UUIDNameReplyEventArgs>(Avatars_OnAvatarNames);
            netcom.ClientLoggedOut += new EventHandler(netcom_ClientLoggedOut);
            netcom.ClientDisconnected += new EventHandler<DisconnectedEventArgs>(netcom_ClientDisconnected);
            client.Self.AvatarSitResponse += new EventHandler<AvatarSitResponseEventArgs>(Self_AvatarSitResponse);
            client.Network.SimChanged += new EventHandler<SimChangedEventArgs>(SIM_OnSimChanged);

            range = instance.Config.CurrentConfig.ObjectRange;
            newrange = range;
            //numericUpDown1.Maximum = instance.Config.CurrentConfig.RadarRange;
            numericUpDown1.Value = Convert.ToDecimal(range);

            string msg1 = "Click for online help on how to use the Object Manager";
            toolTip1 = new Popup(customToolTip = new CustomToolTip(instance, msg1));
            toolTip1.AutoClose = false;
            toolTip1.FocusOnOpen = false;
            toolTip1.ShowingAnimation = toolTip1.HidingAnimation = PopupAnimations.Blend;

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

        // separate thread
        private void Avatars_OnAvatarNames(object sender, UUIDNameReplyEventArgs names)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                OwnerReceived(sender, names);
            }));
        }

        //runs on the GUI thread
        private void OwnerReceived(object sender, UUIDNameReplyEventArgs names)
        {
            int iDx = lbxPrims.SelectedIndex;

            if (iDx == -1) return; 

            ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];
            Primitive sPr = item.Prim;

            foreach (KeyValuePair<UUID, string> av in names.Names)
            {
                if (av.Key == sPr.Properties.OwnerID)
                {
                    label9.Text = av.Value;  
                    pictureBox1.Enabled = true;
                    pictureBox1.Cursor = Cursors.Hand;
                }

                if (!instance.avnames.ContainsKey(av.Key))
                {
                    instance.avnames.Add(av.Key, av.Value);
                }
            }
        }

        private void AddObjectEvents()
        {
            client.Objects.ObjectUpdate += new EventHandler<PrimEventArgs>(Objects_OnNewPrim);
            client.Objects.KillObject += new EventHandler<KillObjectEventArgs>(Objects_OnObjectKilled);
            eventsremoved = false;
        }

        private void RemoveNetcomEvents()
        {
            client.Objects.ObjectUpdate -= new EventHandler<PrimEventArgs>(Objects_OnNewPrim);
            client.Objects.KillObject -= new EventHandler<KillObjectEventArgs>(Objects_OnObjectKilled);
            client.Avatars.UUIDNameReply -= new EventHandler<UUIDNameReplyEventArgs>(Avatars_OnAvatarNames);
            client.Network.Disconnected -= new EventHandler<DisconnectedEventArgs>(Network_OnDisconnected);
            netcom.ClientLoggedOut -= new EventHandler(netcom_ClientLoggedOut);
            netcom.ClientDisconnected -= new EventHandler<DisconnectedEventArgs>(netcom_ClientDisconnected);
            client.Network.SimChanged -= new EventHandler<SimChangedEventArgs>(SIM_OnSimChanged);
        }

        private void RemoveObjectEvents()
        {
            client.Objects.ObjectUpdate -= new EventHandler<PrimEventArgs>(Objects_OnNewPrim);
            client.Objects.KillObject -= new EventHandler<KillObjectEventArgs>(Objects_OnObjectKilled);
            eventsremoved = true;
        }

        private void netcom_ClientLoggedOut(object sender, EventArgs e)
        {
            try
            {
                RemoveNetcomEvents();
                this.Dispose();
            }
            catch
            {
                // do nothing
            }
        }

        private void netcom_ClientDisconnected(object sender, DisconnectedEventArgs e)
        {
            try
            {
                RemoveNetcomEvents();
                this.Dispose();
            }
            catch
            {
                // do nothing
            }
        }

        private void SIM_OnSimChanged(object sender, SimChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    SIM_OnSimChanged(sender, e);
                }));
            }

            if (!this.IsHandleCreated) return;

            BeginInvoke(new MethodInvoker(delegate()
            {
                //ClearLists();
                lbxPrims.Items.Clear();  

                //AddAllObjects();
            }));
        }

        private void ClearLists()
        {
            listItems.Clear();
            ItemsProps.Clear();
            childItems.Clear();
            objs.Clear();
            avatars.Clear();
        }

        private void Network_OnDisconnected(object sender, DisconnectedEventArgs e)
        {
            ClearLists();  
        }

        private void lbxPrims_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index < 0) return;

            ObjectsListItem itemToDraw = (ObjectsListItem)lbxPrims.Items[e.Index];

            Brush textBrush = null;
            Font boldFont = new Font(e.Font, FontStyle.Bold);
            Font regularFont = new Font(e.Font, FontStyle.Regular);

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                textBrush = new SolidBrush(Color.FromKnownColor(KnownColor.HighlightText));
            }
            else
            {
                textBrush = new SolidBrush(Color.FromKnownColor(KnownColor.ControlText));
            }

            string name = string.Empty;
            string description = string.Empty;
            string distance = string.Empty;  

            try
            {
                if (itemToDraw.Prim.Properties == null)
                //if (itemToDraw.Prim == null)
                {
                    name = "...";
                    description = "...";
                }
                else
                {
                    Vector3 location = instance.SIMsittingPos();
                    Vector3 pos = itemToDraw.Prim.Position;
                    double dist = Math.Round(Vector3.Distance(pos, location), MidpointRounding.ToEven);

                    distance = " [" + dist.ToString(CultureInfo.CurrentCulture) + "m]";

                    name = itemToDraw.Prim.Properties.Name;
                    description = itemToDraw.Prim.Properties.Description + distance;
                }
            }
            catch (Exception ex)
            {
                name = "...";
                description = "...";
                Logger.Log(ex.Message, Helpers.LogLevel.Debug, ex);     
            }

            SizeF nameSize = e.Graphics.MeasureString(name, boldFont);
            float nameX = e.Bounds.Left + 4;
            float nameY = e.Bounds.Top + 2;

            e.Graphics.DrawString(name, boldFont, textBrush, nameX, nameY);
            e.Graphics.DrawString(description, regularFont, textBrush, nameX + nameSize.Width + 8, nameY);

            e.DrawFocusRectangle();

            boldFont.Dispose();
            regularFont.Dispose();
            textBrush.Dispose();
            boldFont = null;
            regularFont = null;
            textBrush = null;
        }

        private void AddAllObjects()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    AddAllObjects();
                }));
            }

            Cursor.Current = Cursors.WaitCursor;

            pB1.Visible = true;
            bool inmem = false;

            lbxPrims.location = instance.SIMsittingPos();
            lbxPrims.SortByName = false;
            pBar3.Visible = true;
            lbxPrims.SortList();
            pBar3.Visible = false;

            try
            {
                Vector3 location = instance.SIMsittingPos();

                // *** find all objects in radius ***
                List<Primitive> results = client.Network.CurrentSim.ObjectsPrimitives.FindAll(
                    delegate(Primitive prim)
                    {
                        Vector3 pos = prim.Position;
                        return ((pos != Vector3.Zero) && (Vector3.Distance(pos, location) < range));
                    }
                );

                pB1.Maximum = results.Count;

                lock (listItems)
                {
                    foreach (Primitive prim in results)
                    {
                        try
                        {
                            if (prim.ParentID == 0) //root prims only
                            {
                                ObjectsListItem item = new ObjectsListItem(prim, client, lbxPrims);

                                if (!listItems.ContainsKey(prim.LocalID))
                                {
                                    listItems.Add(prim.LocalID, item);

                                    item.PropertiesReceived += new EventHandler(iitem_PropertiesReceived);
                                    item.RequestProperties();
                                    inmem = true;
                                }
                                else
                                {
                                    if (pB1.Value < results.Count) pB1.Value += 1;
                                    lbxPrims.BeginUpdate();
                                    lbxPrims.Items.Add(item);
                                    lbxPrims.EndUpdate();
                                    inmem = true;
                                }
                            }
                            else
                            {
                                ObjectsListItem citem = new ObjectsListItem(prim, client, lbxChildren);

                                if (!childItems.ContainsKey(prim.LocalID))
                                {
                                    childItems.Add(prim.LocalID, citem);
                                }
                            }

                        }
                        catch
                        {
                            ;
                        }
                    }
                }

                if (!inmem)
                {
                    pB1.Value = 0;
                    pB1.Visible = false;
                    groupBox2.Enabled = true;
                    groupBox3.Enabled = true;
                    //lbxPrims.SortList();
                }

                lblStatus.Visible = false;
                lbxPrims.Visible = true;
                lbxChildren.Visible = true;
                txtSearch.Enabled = true;

                //tlbStatus.Text = listItems.Count.ToString() + " objects";
                tlbDisplay.Text = lbxPrims.Items.Count.ToString(CultureInfo.CurrentCulture) + " objects";
            }
            catch (Exception ex)
            {
                //string exp = exc.Message;
                reporter.Show(ex);
            }

            Cursor.Current = Cursors.Default;
        }

        private void DisplayObjects()
        {
            if (eventsremoved) AddObjectEvents();
 
            lbxPrims.Items.Clear();

            try
            {
                Vector3 location = instance.SIMsittingPos();
                pBar3.Visible = true;

                lock (ItemsProps)
                {
                    //Vector3 location = instance.SIMsittingPos();

                    foreach (KeyValuePair<uint, ObjectsListItem> entry in ItemsProps)
                    {
                        ObjectsListItem item = entry.Value;

                        if (item.Prim.ParentID == 0) //root prims only
                        {
                            Vector3 pos = item.Prim.Position;

                            if (Vector3.Distance(pos, location) < range)
                            {
                                lbxPrims.BeginUpdate();
                                lbxPrims.Items.Add(item);
                                lbxPrims.EndUpdate();
                            }
                        }

                        if (pB1.Value < ItemsProps.Count) pB1.Value += 1;
                    }
                }

                pB1.Visible = false;
                pBar3.Visible = false;

                //lblStatus.Visible = true;
                //lbxPrims.SortList();
                //lblStatus.Visible = false;

                //lbxPrims.Visible = true;
                //lbxChildren.Visible = true;
                txtSearch.Enabled = true;

                //tlbStatus.Text = listItems.Count.ToString() + " objects";
                tlbDisplay.Text = lbxPrims.Items.Count.ToString(CultureInfo.CurrentCulture) + " objects";
            }
            catch (Exception ex)
            {
                //string exp = exc.Message;
                reporter.Show(ex);
            }
        }

        private void SearchFor(string text)
        {
            RemoveObjectEvents();

            lbxPrims.Items.Clear();
            pB1.Visible = true;

            string query = text.ToLower(CultureInfo.CurrentCulture);
            bool inmem = false;

            List<Primitive> results =
                client.Network.CurrentSim.ObjectsPrimitives.FindAll(
                new Predicate<Primitive>(delegate(Primitive prim)
                {
                    try
                    {
                        //evil comparison of death!
                        return (prim.ParentID == 0 && prim.Properties != null) &&
                            (prim.Properties.Name.ToLower(CultureInfo.CurrentCulture).Contains(query) ||
                            prim.Properties.Description.ToLower(CultureInfo.CurrentCulture).Contains(query) ||
                            prim.Properties.OwnerID.ToString().ToLower(CultureInfo.CurrentCulture).Contains(query) ||
                            prim.Text.ToLower(CultureInfo.CurrentCulture).Contains(query) ||
                            prim.ID.ToString().ToLower(CultureInfo.CurrentCulture).Contains(query));
                    }
                    catch
                    {
                        return false;
                    }
                }));

            pB1.Maximum = results.Count;

            lock (listItems)
            {
                foreach (Primitive prim in results)
                {
                    try
                    {
                        ObjectsListItem item = new ObjectsListItem(prim, client, lbxPrims);

                        if (!listItems.ContainsKey(prim.LocalID))
                        {
                            listItems.Add(prim.LocalID, item);

                            item.PropertiesReceived += new EventHandler(item_PropertiesReceived);
                            item.RequestProperties();
                            inmem = true;
                        }
                        else
                        {
                            if (pB1.Value < results.Count) pB1.Value += 1;
                            lbxPrims.BeginUpdate();
                            lbxPrims.Items.Add(item);
                            lbxPrims.EndUpdate();
                        }
                    }
                    catch
                    {
                        ;
                    }
                }
            }

            if (!inmem)
            {
                pB1.Value = 0;
                pB1.Visible = false;
                //lbxPrims.SortList();
            }

            //tlbStatus.Text = listItems.Count.ToString() + " objects";
            tlbDisplay.Text = lbxPrims.Items.Count.ToString(CultureInfo.CurrentCulture) + " objects";
        }

        private void DisplayForSale()
        {
            lbxPrims.Items.Clear();

            try
            {
                Vector3 location = instance.SIMsittingPos();
                pB1.Maximum = ItemsProps.Count;

                pBar3.Visible = true;

                lock (ItemsProps)
                {
                    foreach (KeyValuePair<uint, ObjectsListItem> entry in ItemsProps)
                    {
                        ObjectsListItem item = entry.Value;
                        Vector3 pos = item.Prim.Position;

                        if ((item.Prim.ParentID == 0) && (item.Prim.Properties.SaleType != 0) && (pos != Vector3.Zero) && (Vector3.Distance(pos, location) < range))
                        {
                            if (item.Prim.ParentID == 0) //root prims only
                            {
                                lbxPrims.BeginUpdate();
                                lbxPrims.Items.Add(item);
                                lbxPrims.EndUpdate();
                            }
                        }

                        if (pB1.Value < ItemsProps.Count) pB1.Value += 1;
                    }
                }

                //lblStatus.Visible = true;
                //lbxPrims.SortList();
                //lblStatus.Visible = false;
                pB1.Visible = false;
                pBar3.Visible = false;

                //lblStatus.Visible = false;
                //lbxPrims.Visible = true;
                //lbxChildren.Visible = true;
                txtSearch.Enabled = true;

                //tlbStatus.Text = listItems.Count.ToString() + " objects";
                tlbDisplay.Text = lbxPrims.Items.Count.ToString(CultureInfo.CurrentCulture) + " objects";
            }
            catch (Exception ex)
            {
                //string exp = exc.Message;
                reporter.Show(ex);
            }
        }

        private void DisplayScriptedObjects()
        {
            lbxPrims.Items.Clear();

            try
            {
                Vector3 location = instance.SIMsittingPos();
                pB1.Maximum = ItemsProps.Count;
                pBar3.Visible = true;

                lock (ItemsProps)
                {
                    foreach (KeyValuePair<uint, ObjectsListItem> entry in ItemsProps)
                    {
                        ObjectsListItem item = entry.Value;
                        Vector3 pos = item.Prim.Position;

                        if ((item.Prim.ParentID == 0) && ((item.Prim.Flags & PrimFlags.Scripted) == PrimFlags.Scripted) && (pos != Vector3.Zero) && (Vector3.Distance(pos, location) < range))
                        {
                            if (item.Prim.ParentID == 0) //root prims only
                            {
                                lbxPrims.BeginUpdate();
                                lbxPrims.Items.Add(item);
                                lbxPrims.EndUpdate();
                            }
                        }

                        if (pB1.Value < ItemsProps.Count) pB1.Value += 1;
                    }
                }

                //lblStatus.Visible = true;
                //lbxPrims.SortList();
                //lblStatus.Visible = false;
                pB1.Visible = false;
                pBar3.Visible = false;

                //lblStatus.Visible = false;
                //lbxPrims.Visible = true;
                //lbxChildren.Visible = true;
                txtSearch.Enabled = true;

                //tlbStatus.Text = listItems.Count.ToString() + " objects";
                tlbDisplay.Text = lbxPrims.Items.Count.ToString(CultureInfo.CurrentCulture) + " objects";
            }
            catch (Exception ex)
            {
                //string exp = exc.Message;
                reporter.Show(ex);
            }
        }

        private void DisplayMyObjects()
        {
            lbxPrims.Items.Clear();

            try
            {
                Vector3 location = instance.SIMsittingPos();
                pB1.Maximum = ItemsProps.Count;
                pBar3.Visible = true;

                lock (ItemsProps)
                {
                    foreach (KeyValuePair<uint, ObjectsListItem> entry in ItemsProps)
                    {
                        ObjectsListItem item = entry.Value;
                        Vector3 pos = item.Prim.Position;

                        if ((item.Prim.ParentID == 0) && item.Prim.Properties.OwnerID.ToString().ToLower(CultureInfo.CurrentCulture).Contains(client.Self.AgentID.ToString()) && (pos != Vector3.Zero) && (Vector3.Distance(pos, location) < range))
                        {
                            if (item.Prim.ParentID == 0) //root prims only
                            {
                                lbxPrims.BeginUpdate();
                                lbxPrims.Items.Add(item);
                                lbxPrims.EndUpdate();
                            }
                        }

                        if (pB1.Value < ItemsProps.Count) pB1.Value += 1;
                    }
                }

                pB1.Visible = false;
                pBar3.Visible = false;

                txtSearch.Enabled = true;

                tlbDisplay.Text = lbxPrims.Items.Count.ToString(CultureInfo.CurrentCulture) + " objects";
            }
            catch (Exception ex)
            {
                //string exp = exc.Message;
                reporter.Show(ex);
            }
        }

        private void DisplayOthersObjects()
        {
            lbxPrims.Items.Clear();

            try
            {
                Vector3 location = instance.SIMsittingPos();
                pB1.Maximum = ItemsProps.Count;
                pBar3.Visible = true;

                lock (ItemsProps)
                {
                    foreach (KeyValuePair<uint, ObjectsListItem> entry in ItemsProps)
                    {
                        ObjectsListItem item = entry.Value;
                        Vector3 pos = item.Prim.Position;

                        if ((item.Prim.ParentID == 0) && !item.Prim.Properties.OwnerID.ToString().ToLower(CultureInfo.CurrentCulture).Contains(client.Self.AgentID.ToString()) && (pos != Vector3.Zero) && (Vector3.Distance(pos, location) < range))
                        {
                            if (item.Prim.ParentID == 0) //root prims only
                            {
                                lbxPrims.BeginUpdate();
                                lbxPrims.Items.Add(item);
                                lbxPrims.EndUpdate();
                            }
                        }

                        if (pB1.Value < ItemsProps.Count) pB1.Value += 1;
                    }
                }

                pB1.Visible = false;
                pBar3.Visible = false;

                txtSearch.Enabled = true;

                tlbDisplay.Text = lbxPrims.Items.Count.ToString(CultureInfo.CurrentCulture) + " objects";
            }
            catch (Exception ex)
            {
                //string exp = exc.Message;
                reporter.Show(ex);
            }
        }

        private void DisplayEmptyObjects()
        {
            lbxPrims.Items.Clear();

            try
            {
                Vector3 location = instance.SIMsittingPos();
                pB1.Maximum = ItemsProps.Count;
                pBar3.Visible = true;

                lock (ItemsProps)
                {
                    foreach (KeyValuePair<uint, ObjectsListItem> entry in ItemsProps)
                    {
                        ObjectsListItem item = entry.Value;
                        Vector3 pos = item.Prim.Position;

                        if ((item.Prim.ParentID == 0) && ((item.Prim.Flags & PrimFlags.InventoryEmpty) == PrimFlags.InventoryEmpty) && (pos != Vector3.Zero) && (Vector3.Distance(pos, location) < range))
                        {
                            if (item.Prim.ParentID == 0) //root prims only
                            {
                                lbxPrims.BeginUpdate();
                                lbxPrims.Items.Add(item);
                                lbxPrims.EndUpdate();
                            }
                        }

                        if (pB1.Value < ItemsProps.Count) pB1.Value += 1;
                    }
                }

                pB1.Visible = false;
                pBar3.Visible = false;

                txtSearch.Enabled = true;

                tlbDisplay.Text = lbxPrims.Items.Count.ToString(CultureInfo.CurrentCulture) + " objects";
            }
            catch (Exception ex)
            {
                //string exp = exc.Message;
                reporter.Show(ex);
            }
        }

        private void DisplayCreatedByMeObjects()
        {
            lbxPrims.Items.Clear();

            try
            {
                Vector3 location = instance.SIMsittingPos();
                pB1.Maximum = ItemsProps.Count;
                pBar3.Visible = true;

                lock (ItemsProps)
                {
                    foreach (KeyValuePair<uint, ObjectsListItem> entry in ItemsProps)
                    {
                        ObjectsListItem item = entry.Value;
                        Vector3 pos = item.Prim.Position;

                        if ((item.Prim.ParentID == 0) && item.Prim.Properties.CreatorID.ToString().Contains(client.Self.AgentID.ToString()) && (pos != Vector3.Zero) && (Vector3.Distance(pos, location) < range))
                        {
                            if (item.Prim.ParentID == 0) //root prims only
                            {
                                lbxPrims.BeginUpdate();
                                lbxPrims.Items.Add(item);
                                lbxPrims.EndUpdate();
                            }
                        }

                        if (pB1.Value < ItemsProps.Count) pB1.Value += 1;
                    }
                }

                pB1.Visible = false;
                pBar3.Visible = false;

                txtSearch.Enabled = true;

                tlbDisplay.Text = lbxPrims.Items.Count.ToString(CultureInfo.CurrentCulture) + " objects";
            }
            catch (Exception ex)
            {
                //string exp = exc.Message;
                reporter.Show(ex);
            }
        }

        private void DisplayFullModObjects()
        {
            lbxPrims.Items.Clear();

            try
            {
                Vector3 location = instance.SIMsittingPos();
                pB1.Maximum = ItemsProps.Count;
                pBar3.Visible = true;

                lock (ItemsProps)
                {
                    foreach (KeyValuePair<uint, ObjectsListItem> entry in ItemsProps)
                    {
                        ObjectsListItem item = entry.Value;
                        Vector3 pos = item.Prim.Position;

                        if ((item.Prim.ParentID == 0) && (pos != Vector3.Zero) && (Vector3.Distance(pos, location) < range))
                        {
                            if (item.Prim.ParentID == 0) //root prims only
                            {
                                PermissionMask sPerm = item.Prim.Properties.Permissions.NextOwnerMask;
                                //PermissionMask sOPerm = item.Prim.Properties.Permissions.OwnerMask;
                                string sEp = sPerm.ToString();
                                //string sOEp = sOPerm.ToString();

                                if (sEp.ToLower(CultureInfo.CurrentCulture).Contains("modify") && sEp.ToLower(CultureInfo.CurrentCulture).Contains("copy") & sEp.ToLower(CultureInfo.CurrentCulture).Contains("transfer"))
                                {
                                    lbxPrims.BeginUpdate();
                                    lbxPrims.Items.Add(item);
                                    lbxPrims.EndUpdate();
                                }
                            }
                        }

                        if (pB1.Value < ItemsProps.Count) pB1.Value += 1;
                    }
                }

                pB1.Visible = false;
                pBar3.Visible = false;

                txtSearch.Enabled = true;

                tlbDisplay.Text = lbxPrims.Items.Count.ToString(CultureInfo.CurrentCulture) + " objects";
            }
            catch (Exception ex)
            {
                //string exp = exc.Message;
                reporter.Show(ex);
            }
        }

        private void DisplayCFullModObjects()
        {
            lbxPrims.Items.Clear();

            try
            {
                Vector3 location = instance.SIMsittingPos();
                pB1.Maximum = ItemsProps.Count;
                pBar3.Visible = true;

                lock (ItemsProps)
                {
                    foreach (KeyValuePair<uint, ObjectsListItem> entry in ItemsProps)
                    {
                        ObjectsListItem item = entry.Value;
                        Vector3 pos = item.Prim.Position;

                        if ((item.Prim.ParentID == 0) && (pos != Vector3.Zero) && (Vector3.Distance(pos, location) < range))
                        {
                            if (item.Prim.ParentID == 0) //root prims only
                            {
                                PermissionMask sPerm = item.Prim.Properties.Permissions.OwnerMask;
                                //PermissionMask sOPerm = item.Prim.Properties.Permissions.OwnerMask;
                                string sEp = sPerm.ToString();
                                //string sOEp = sOPerm.ToString();

                                if (sEp.ToLower(CultureInfo.CurrentCulture).Contains("modify") && sEp.ToLower(CultureInfo.CurrentCulture).Contains("copy") & sEp.ToLower(CultureInfo.CurrentCulture).Contains("transfer"))
                                {
                                    lbxPrims.BeginUpdate();
                                    lbxPrims.Items.Add(item);
                                    lbxPrims.EndUpdate();
                                }
                            }
                        }

                        if (pB1.Value < ItemsProps.Count) pB1.Value += 1;
                    }
                }

                pB1.Visible = false;
                pBar3.Visible = false;

                txtSearch.Enabled = true;

                tlbDisplay.Text = lbxPrims.Items.Count.ToString(CultureInfo.CurrentCulture) + " objects";
            }
            catch (Exception ex)
            {
                //string exp = exc.Message;
                reporter.Show(ex);
            }
        }

        private void item_PropertiesReceived(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    item_PropertiesReceived(sender, e);
                    
                }));
                return;
            }

            ObjectsListItem item = (ObjectsListItem)sender;

            BeginInvoke(new MethodInvoker(delegate()
            {
                Vector3 location = instance.SIMsittingPos();
                Vector3 pos = item.Prim.Position;

                if (Vector3.Distance(pos, location) < range)
                {
                    lbxPrims.BeginUpdate();
                    lbxPrims.Items.Add(sender);
                    lbxPrims.EndUpdate();

                    if (!ItemsProps.ContainsKey(item.Prim.LocalID))
                    {
                        ItemsProps.Add(item.Prim.LocalID, item);
                    }                
                }

                tlbDisplay.Text = lbxPrims.Items.Count.ToString(CultureInfo.CurrentCulture) + " objects";

                if (pB1.Value + 1 <= pB1.Maximum)
                    pB1.Value += 1;

                if (pB1.Value >= pB1.Maximum)
                {
                    pB1.Value = 0;
                    pB1.Visible = false;

                    //lblStatus.Visible = true;
                    //lbxPrims.SortList();
                    //lblStatus.Visible = false;
                }

                //pB1.Visible = false;

                //lbxPrims.SortList();
            }));

            item.PropertiesReceived -= new EventHandler(item_PropertiesReceived);
        }

        private void iitem_PropertiesReceived(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    iitem_PropertiesReceived(sender, e);
                    
                }));
                return;
            }

            ObjectsListItem item = (ObjectsListItem)sender;

            BeginInvoke(new MethodInvoker(delegate()
            {
                Vector3 location = instance.SIMsittingPos();
                Vector3 pos = item.Prim.Position;

                if (Vector3.Distance(pos, location) < range)
                {
                    lbxPrims.BeginUpdate();
                    lbxPrims.Items.Add(sender);
                    lbxPrims.EndUpdate();

                    if (!ItemsProps.ContainsKey(item.Prim.LocalID))
                    {
                        ItemsProps.Add(item.Prim.LocalID, item);
                    }  
                }

                //tlbStatus.Text = listItems.Count.ToString() + " objects";
                tlbDisplay.Text = lbxPrims.Items.Count.ToString(CultureInfo.CurrentCulture) + " objects";

                if (pB1.Value + 1 <= pB1.Maximum)
                    pB1.Value += 1;

                if (pB1.Value >= pB1.Maximum)
                {
                    pB1.Value = 0;
                    pB1.Visible = false;
                    groupBox2.Enabled = true;
                    groupBox3.Enabled = true;

                    //lblStatus.Visible = true;
                    pBar3.Visible = true;
                    lbxPrims.SortList();
                    pBar3.Visible = false;
                    //lblStatus.Visible = false;
                }
            }));

            //lbxPrims.SortList();
            item.PropertiesReceived -= new EventHandler(iitem_PropertiesReceived);
        }

        private void ResetObjects()
        {
            lbxPrims.Items.Clear();
            lbxChildren.Items.Clear();
            lbxTask.Items.Clear();
            listItems.Clear();
            childItems.Clear();
            DisplayObjects();
            button3.Visible = button7.Visible = false;
        }

        private void frmObjects_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            
            this.Text = "Object Manager [" + client.Self.FirstName.ToString() + " " + client.Self.LastName.ToString() + "]";

            //numericUpDown1.Maximum = instance.Config.CurrentConfig.RadarRange;

            AddObjectEvents();
            
            if (instance.Config.CurrentConfig.SortByDistance)
            {
                radioButton2.Checked = true;
            }
            else
            {
                radioButton1.Checked = true;
            }

            AddAllObjects();
        }

        //Separate thread
        private void Objects_OnNewPrim(object sender, PrimEventArgs e)
        {
            if (!this.IsHandleCreated) return;

            try
            {
                if (e.Prim.ParentID != 0)
                {
                    lock (childItems)
                    {
                        ObjectsListItem citem = new ObjectsListItem(e.Prim, client, lbxChildren);

                        if (!childItems.ContainsKey(e.Prim.LocalID))
                        {
                            try
                            {
                                childItems.Add(e.Prim.LocalID, citem);
                            }
                            catch
                            {
                                ;
                            }
                        }
                    }
                }
                else
                {
                    lock (listItems)
                    {
                        try
                        {
                            if (listItems.ContainsKey(e.Prim.LocalID)) return;
                        }
                        catch
                        { 
                            ; 
                        }

                        try
                        {
                            BeginInvoke(new MethodInvoker(delegate()
                            {
                                ObjectsListItem item = new ObjectsListItem(e.Prim, client, lbxPrims);

                                try
                                {
                                    if (listItems.ContainsKey(e.Prim.LocalID)) return;
                                }
                                catch { ; }

                                try
                                {
                                    listItems.Add(e.Prim.LocalID, item);

                                    BeginInvoke(new MethodInvoker(delegate()
                                    {
                                        pB1.Maximum += 1;
                                    }));

                                    item.PropertiesReceived += new EventHandler(iitem_PropertiesReceived);
                                    item.RequestProperties();
                                }
                                catch
                                {
                                    ;
                                }
                            }));
                        }
                        catch
                        {
                            ;
                        }
                    }
                }
            }
            catch
            {
                ;  
            }
        }

        //Separate thread
        private void Objects_OnObjectKilled(object sender, KillObjectEventArgs e)
        {
            if (!this.IsHandleCreated) return;

            ObjectsListItem item;

            uint objectID = e.ObjectLocalID; 

            try
            {
                if (listItems.ContainsKey(objectID))
                {
                    item = listItems[objectID];
                }
                else
                {
                    return;
                }

                if (item.Prim.ParentID != 0)
                {
                    lock (childItems)
                    {
                        if (!childItems.ContainsKey(objectID)) return;

                        try
                        {
                            BeginInvoke(new MethodInvoker(delegate()
                            {
                                item = childItems[objectID];

                                if (lbxChildren.Items.Contains(item))
                                {
                                    lbxChildren.Items.Remove(item);
                                }

                                try
                                {
                                    childItems.Remove(objectID);
                                }
                                catch
                                {
                                    ;
                                }
                            }));
                        }
                        catch
                        {
                            ;
                        }
                    }
                }
                else
                {
                    lock (listItems)
                    {
                        if (!listItems.ContainsKey(objectID)) return;

                        try
                        {
                            BeginInvoke(new MethodInvoker(delegate()
                            {
                                item = listItems[objectID];

                                if (lbxPrims.Items.Contains(item))
                                {
                                    lbxPrims.Items.Remove(item);
                                }

                                pB1.Maximum -= 1;

                                try
                                {
                                    listItems.Remove(objectID);
                                }
                                catch
                                {
                                    ;
                                }

                                //tlbStatus.Text = listItems.Count.ToString() + " objects";
                                tlbDisplay.Text = lbxPrims.Items.Count.ToString(CultureInfo.CurrentCulture) + " objects";
                            }));
                        }
                        catch
                        {
                            ;
                        }
                    }
                }
            }
            catch
            {
                // passed key wasn't available
                return;
            }
        }

        private void lbxPrims_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                sloading = true;

                lbxTask.Items.Clear();

                button6.Enabled = groupBox1.Enabled = gbxInworld.Enabled = (lbxPrims.SelectedItem != null);

                int iDx = lbxPrims.SelectedIndex;

                if (iDx < 0)
                    return;

                ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];

                Primitive sPr = item.Prim;

                lblOwner.Text = sPr.Properties.OwnerID.ToString();
                lblUUID.Text = sPr.Properties.ObjectID.ToString();

                if (instance.State.SitPrim != UUID.Zero)
                {
                    if (sPr.ID == instance.State.SitPrim)
                    {
                        btnSitOn.Text = "&Stand";
                    }
                    else
                    {
                        btnSitOn.Text = "&Sit";
                    }
                }
                else
                {
                    btnSitOn.Text = "&Sit";
                }

                // Get the owner name
                UUID lookup = sPr.Properties.OwnerID;
                if (!instance.avnames.ContainsKey(lookup))
                {
                    client.Avatars.RequestAvatarName(lookup);
                    pictureBox1.Cursor = Cursors.Default;
                }
                else
                {
                    label9.Text = instance.avnames[lookup];
                    pictureBox1.Enabled = true;
                    pictureBox1.Cursor = Cursors.Hand;   
                }

                //if (lookup == client.Self.AgentID)
                //{
                //    btnReturn.Enabled =  btnTake.Enabled = true;
                //}
                //else
                //{
                //    btnReturn.Enabled = btnTake.Enabled = false;
                //}

                btnReturn.Enabled = btnTake.Enabled = true;

                PermissionMask sPerm = sPr.Properties.Permissions.NextOwnerMask;
                PermissionMask sOPerm = sPr.Properties.Permissions.OwnerMask;
                string sEp = sPerm.ToString();
                string sOEp = sOPerm.ToString();

                if (sPr.Properties.SaleType != 0)
                {
                    label3.Text = "L$" + sPr.Properties.SalePrice.ToString(CultureInfo.CurrentCulture);
                }
                else
                {
                    label3.Text = "Not for sale";
                }

                label15.Text = sPr.Properties.Description;
                label11.Text = sPr.Text;

                Vector3 primpos = sPr.Position;
                //// Calculate the distance here in metres
                //int pX = (int)primpos.X;
                //int pY = (int)primpos.Y;
                //int pZ = (int)primpos.Z;

                //int sX = (int)client.Self.SimPosition.X;
                //int sY = (int)client.Self.SimPosition.Y;
                //int sZ = (int)client.Self.SimPosition.Z;

                int vZ = (int)primpos.Z - (int)instance.SIMsittingPos().Z;

                //int vX = sX - pX;
                //int vY = sY - pY;

                //int pX2 = vX * vX;
                //int pY2 = vY * vY;
                //int h2 = pX2 + pY2;

                //int hyp1 = (int)Math.Sqrt(h2);
                //int hyp = instance.Distance3D(sX, sY, sZ, pX, pY, pZ);

                double dist = Math.Round(Vector3.Distance(primpos, instance.SIMsittingPos()), MidpointRounding.ToEven);

                label13.Text = " " + dist.ToString(CultureInfo.CurrentCulture) + "m - [ Elev.:" + vZ.ToString(CultureInfo.CurrentCulture) + "m]";

                label5.Text = "L$" + sPr.Properties.OwnershipCost.ToString(CultureInfo.CurrentCulture);
                //label3.Text = sPr.Properties.SaleType.ToString(); 

                // Owner perms
                if (sOEp.Contains("Modify"))
                {
                    checkBox6.Checked = true;
                }
                else
                {
                    checkBox6.Checked = false;
                }

                if (sOEp.Contains("Copy"))
                {
                    checkBox5.Checked = true;
                }
                else
                {
                    checkBox5.Checked = false;
                }

                if (sOEp.Contains("Transfer"))
                {
                    checkBox4.Checked = true;
                }
                else
                {
                    checkBox4.Checked = false;
                }

                // Next Owner perms
                if (sEp.Contains("Modify"))
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }

                if (sEp.Contains("Copy"))
                {
                    checkBox2.Checked = true;
                }
                else
                {
                    checkBox2.Checked = false;
                }

                if (sEp.Contains("Transfer"))
                {
                    checkBox3.Checked = true;
                }
                else
                {
                    checkBox3.Checked = false;
                }

                if (btnTP.Enabled)
                    btnTP.Enabled = false; lkLocation.Text = "";

                //sPr.Flags = LLObject.ObjectFlags.Scripted;
                //client.Objects.RequestObject("", sPr.LocalID);
                //client.Objects.SelectObject();

                pBar1.Visible = true;
                pBar1.Refresh();
                // Populate child items here
                lbxChildren.BeginUpdate();
                lbxChildren.Items.Clear();

                button3.Visible = button7.Visible = false;

                foreach (KeyValuePair<uint, ObjectsListItem> kvp in childItems)
                {
                    if (sPr.LocalID == kvp.Value.Prim.ParentID)
                    {
                        //ObjectsListItem citem = new ObjectsListItem(kvp.Value.Prim, client, lbxChildren);
                        //sPr.
                        //citem.PropertiesReceived += new EventHandler(citem_PropertiesReceived);
                        //citem.RequestProperties();
                        lbxChildren.Items.Add(kvp.Value);
                    }
                }

                lbxChildren.EndUpdate();
                pBar1.Visible = false;

                SetPerm(sPr);

                if (sPr.Properties.OwnerID != client.Self.AgentID)
                {
                    checkBox1.Enabled = checkBox2.Enabled = checkBox3.Enabled = false;
                }
                else
                {
                    checkBox1.Enabled = checkBox2.Enabled = checkBox3.Enabled = true;
                }

                sloading = false;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, Helpers.LogLevel.Error);
            }

            string msg1 = label11.Text;
            toolTip = new Popup(customToolTip = new CustomToolTip(instance, msg1));
            toolTip.AutoClose = false;
            toolTip.FocusOnOpen = false;
            toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;
        }

        private void SetPerm(Primitive sPr)
        {
            // Set permission checboxes  
            if ((sPr.Properties.Permissions.OwnerMask & PermissionMask.Modify) == PermissionMask.Modify)
            {
                checkBox1.Enabled = true;
            }
            else
            {
                checkBox1.Enabled = false;
            }

            if ((sPr.Properties.Permissions.OwnerMask & PermissionMask.Copy) == PermissionMask.Copy)
            {
                checkBox2.Enabled = true;
            }
            else
            {
                checkBox2.Enabled = false;
            }

            if ((sPr.Properties.Permissions.OwnerMask & PermissionMask.Transfer) == PermissionMask.Transfer)
            {
                checkBox3.Enabled = true;
            }
            else
            {
                checkBox3.Enabled = false;
            }

            if ((sPr.Properties.Permissions.OwnerMask & PermissionMask.Modify) != PermissionMask.Modify)
            {
                checkBox1.Enabled = checkBox2.Enabled = checkBox3.Enabled = false;
            }
        }

        private void SetPerms(Primitive oPrm)
        {
            Dictionary<UUID, Primitive> Objects = new Dictionary<UUID, Primitive>();
            PermissionMask Perms = PermissionMask.None;
            List<Primitive> childPrims;
            List<uint> localIDs = new List<uint>();
            UUID rootID = oPrm.ID;

            Primitive rootPrim = client.Network.CurrentSim.ObjectsPrimitives.Find(
            delegate(Primitive prim)
            {
                return prim.ID == oPrm.ID;
            }
            );

            if (checkBox1.Checked)
            {
                Perms |= PermissionMask.Modify;
            }

            if (checkBox2.Checked)
            {
                Perms |= PermissionMask.Copy;
            }

            if (checkBox3.Checked)
            {
                Perms |= PermissionMask.Transfer;
            }


            rootPrim = client.Network.CurrentSim.ObjectsPrimitives.Find(delegate(Primitive prim) { return prim.ID == rootID; });

            if (rootPrim == null)
                return;
            else
                Logger.DebugLog("Found requested prim " + rootPrim.ID.ToString(), client);

            if (rootPrim.ParentID != 0)
            {
                // This is not actually a root prim, find the root
                if (!client.Network.CurrentSim.ObjectsPrimitives.TryGetValue(rootPrim.ParentID, out rootPrim))
                    return;
            }

            // Save the description
            client.Objects.SetDescription(client.Network.CurrentSim, rootPrim.LocalID , label15.Text);
            client.Objects.SetName(client.Network.CurrentSim, rootPrim.LocalID, label11.Text);         

            // Find all of the child objects linked to this root
            childPrims = client.Network.CurrentSim.ObjectsPrimitives.FindAll(delegate(Primitive prim) { return prim.ParentID == rootPrim.LocalID; });

            // Build a dictionary of primitives for referencing later
            Objects[rootPrim.ID] = rootPrim;
            for (int i = 0; i < childPrims.Count; i++)
                Objects[childPrims[i].ID] = childPrims[i];

            // Build a list of all the localIDs to set permissions for
            localIDs.Add(rootPrim.LocalID);
            for (int i = 0; i < childPrims.Count; i++)
                localIDs.Add(childPrims[i].LocalID);

            if ((Perms & PermissionMask.Modify) == PermissionMask.Modify)
                client.Objects.SetPermissions(client.Network.CurrentSim, localIDs, PermissionWho.NextOwner, PermissionMask.Modify, true);
            else
                client.Objects.SetPermissions(client.Network.CurrentSim, localIDs, PermissionWho.NextOwner, PermissionMask.Modify, false);

            if ((Perms & PermissionMask.Copy) == PermissionMask.Copy)
                client.Objects.SetPermissions(client.Network.CurrentSim, localIDs, PermissionWho.NextOwner, PermissionMask.Copy, true);
            else
                client.Objects.SetPermissions(client.Network.CurrentSim, localIDs, PermissionWho.NextOwner, PermissionMask.Copy, false);

            if ((Perms & PermissionMask.Transfer) == PermissionMask.Transfer)
                client.Objects.SetPermissions(client.Network.CurrentSim, localIDs, PermissionWho.NextOwner, PermissionMask.Transfer, true);
            else
                client.Objects.SetPermissions(client.Network.CurrentSim, localIDs, PermissionWho.NextOwner, PermissionMask.Transfer, false);

        
            //// Check each prim for task inventory and set permissions on the task inventory
            //int taskItems = 0;
            //foreach (Primitive prim in Objects.Values)
            //{
            //    if ((prim.Flags & PrimFlags.InventoryEmpty) == 0)
            //    {
            //        List<InventoryBase> items = client.Inventory.GetTaskInventory(prim.ID, prim.LocalID, 1000 * 30);

            //        if (items != null)
            //        {
            //            for (int i = 0; i < items.Count; i++)
            //            {
            //                if (!(items[i] is InventoryFolder))
            //                {
            //                    InventoryItem itemf = (InventoryItem)items[i];
            //                    itemf.Permissions.NextOwnerMask = Perms;

            //                    client.Inventory.UpdateTaskInventory(prim.LocalID, itemf);
            //                    ++taskItems;
            //                }
            //            }
            //        }
            //    }
            //}
        }

        private void btnSitOn_Click(object sender, EventArgs e)
        {
            int iDx = lbxPrims.SelectedIndex;
            ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];

            if (item == null) return;

            if (btnSitOn.Text == "&Sit")
            {
                instance.State.SetSitting(true, item.Prim.ID);

                //// start the timer
                //sittimer = new System.Timers.Timer();
                //sittimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                //// Set the Interval to 10 seconds.
                //sittimer.Interval = 5000;
                //sittimer.Enabled = true;
                //sittimer.Start();  
            }
            else if (btnSitOn.Text == "&Stand")
            {
                instance.State.SetSitting(false, item.Prim.ID);
                btnSitOn.Text = "&Sit";
            }
        }

        void Self_AvatarSitResponse(object sender, AvatarSitResponseEventArgs e)
        {
            instance.State.SitPrim = e.ObjectID;
            instance.State.IsSitting = true;

            BeginInvoke(new MethodInvoker(delegate()
            {
                btnSitOn.Text = "&Stand";
            }));
        }

        //private void OnTimedEvent(object sender, ElapsedEventArgs e)
        //{
        //    //PrimEvent.WaitOne(4000, false);

        //    if (client.Self.SittingOn == 0)
        //    {
        //        instance.State.SetSitting(false, instance.State.SitPrim);
        //        btnSitOn.Text = "&Sit";
        //    }

        //    sittimer.Stop();
        //    sittimer.Enabled = false; 
        //}

        private void btnTouch_Click(object sender, EventArgs e)
        {
            int iDx = lbxPrims.SelectedIndex;
            ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];

            if (item == null) return;

            if ((item.Prim.Flags & PrimFlags.Touch) != 0)
            {
                Vector3 pos = item.Prim.Position;

                uint regionX, regionY;
                Utils.LongToUInts(client.Network.CurrentSim.Handle, out regionX, out regionY);
                Vector3d objpos;

                objpos.X = (double)pos.X + (double)regionX;
                objpos.Y = (double)pos.Y + (double)regionY;
                objpos.Z = pos.Z;   // -2f;

                instance.State.SetPointingTouch(true, item.Prim.ID, objpos, pos);
                instance.State.LookAtObject(true, item.Prim.ID);

                client.Self.Touch(item.Prim.LocalID);
                System.Threading.Thread.Sleep(800);

                instance.State.SetPointingTouch(false, item.Prim.ID, objpos, pos);
                instance.State.LookAtObject(false, item.Prim.ID);
            }
        }

        private void frmObjects_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClearLists();
            lbxPrims.Items.Clear();  

            RemoveObjectEvents();
            RemoveNetcomEvents(); 
            client.Avatars.UUIDNameReply -= new EventHandler<UUIDNameReplyEventArgs>(Avatars_OnAvatarNames);
            client.Self.AvatarSitResponse -= new EventHandler<AvatarSitResponseEventArgs>(Self_AvatarSitResponse);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLocation_Click(object sender, EventArgs e)
        {
            int iDx = lbxPrims.SelectedIndex;
            ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];

            if (item == null) return;

            //"http://slurl.com/GridClient/" + 
            string sPos = client.Network.CurrentSim.Name + "/" + item.Prim.Position.X + "/" + item.Prim.Position.Y + "/" + item.Prim.Position.Z;
            lkLocation.Text = sPos;
            btnTP.Enabled = true;
        }

        private void btnTP_Click(object sender, EventArgs e)
        {
            int iDx = lbxPrims.SelectedIndex;
            ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];

            if (item == null) return;

            (new frmTeleport(instance, client.Network.CurrentSim.Name, item.Prim.Position.X, item.Prim.Position.Y, item.Prim.Position.Z)).ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int iDx = lbxPrims.SelectedIndex;
            ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];

            if (item == null) return;

            Vector3 target = item.Prim.Position; // the object to look at
            client.Self.Movement.TurnToward(target);
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        //private int GetDistance(Vector3 primpos)
        //{
        //    // Calculate the distance here in metres
        //    int pX = (int)primpos.X;
        //    int pY = (int)primpos.Y;
        //    int pZ = (int)primpos.Z;

        //    int sX = (int)client.Self.SimPosition.X;
        //    int sY = (int)client.Self.SimPosition.Y;
        //    int sZ = (int)client.Self.SimPosition.Z;

        //    int vX = sX - pX;
        //    int vY = sY - pY;

        //    int pX2 = vX * vX;
        //    int pY2 = vY * vY;
        //    int h2 = pX2 + pY2;

        //    int vZ = pZ - sZ;

        //    int hyp = (int)Math.Sqrt(h2);

        //    return hyp;
        //}

        private void btnPay_Click_1(object sender, EventArgs e)
        {
            int iDx = lbxPrims.SelectedIndex;
            ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];

            if (item == null) return;

            Primitive sPr = item.Prim;
            SaleType styp = sPr.Properties.SaleType;

            int sprice = sPr.Properties.SalePrice;

            //if (sprice != 0)
            if (styp != SaleType.Not)
            {
                (new frmPay(instance, item.Prim.ID, sPr.Properties.Name, sprice, sPr)).ShowDialog();
            }
            else
            {
                (new frmPay(instance, item.Prim.ID, sPr.Properties.Name)).ShowDialog();
            }
        }

        private void btnPointAt_Click_1(object sender, EventArgs e)
        {
            int iDx = lbxPrims.SelectedIndex;
            ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];

            if (item == null) return;

            uint regionX, regionY;
            Utils.LongToUInts(client.Network.CurrentSim.Handle, out regionX, out regionY);
            Vector3 pos = item.Prim.Position;

            Vector3d objpos;
 
            objpos.X = (double)pos.X + (double)regionX;
            objpos.Y = (double)pos.Y + (double)regionY;
            objpos.Z  = pos.Z;   // -2f;

            if (btnPointAt.Text == "Po&int At")
            {
                client.Self.AnimationStart(Animations.TURNLEFT, false);
                client.Self.Movement.TurnToward(item.Prim.Position);
                client.Self.Movement.FinishAnim = true;
                System.Threading.Thread.Sleep(200);
                client.Self.AnimationStop(Animations.TURNLEFT, false);

                instance.State.SetPointing(true, item.Prim.ID, objpos, pos);
                instance.State.LookAtObject(true, item.Prim.ID);
                btnPointAt.Text = "Unpo&int";
            }
            else if (btnPointAt.Text == "Unpo&int")
            {
                instance.State.SetPointing(false, item.Prim.ID, objpos, pos);
                instance.State.LookAtObject(false, item.Prim.ID);
                btnPointAt.Text = "Po&int At";
            }
        }

        private void lbxChildren_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index < 0) return;

            ObjectsListItem itemToDraw = (ObjectsListItem)lbxChildren.Items[e.Index];

            Brush textBrush = null;
            Font boldFont = new Font(e.Font, FontStyle.Bold);
            Font regularFont = new Font(e.Font, FontStyle.Regular);

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                textBrush = new SolidBrush(Color.FromKnownColor(KnownColor.HighlightText));
            }
            else
            {
                textBrush = new SolidBrush(Color.FromKnownColor(KnownColor.ControlText));
            }

            string name;
            string description;

            ////GM: protected from null properties
            //if (itemToDraw.Prim.Properties == null)
            //{
            //    name = description = "Null";
            //}
            if (itemToDraw.Prim.Properties == null)
            {
                if ((itemToDraw.Prim.Flags & PrimFlags.Scripted) != 0)
                {
                    if ((itemToDraw.Prim.Flags & PrimFlags.Touch) != 0)
                    {
                        name = "Child Object (scripted/touch)";
                    }
                    else
                    {
                        name = "Child Object (scripted)";
                    }
                }
                else
                {
                    name = "Child Object";
                }

                description = itemToDraw.Prim.ID.ToString();
            }
            else
            {
                name = itemToDraw.Prim.Properties.Name;
                description = itemToDraw.Prim.Properties.Description;
            }

            SizeF nameSize = e.Graphics.MeasureString(name, boldFont);
            float nameX = e.Bounds.Left + 4;
            float nameY = e.Bounds.Top + 2;

            e.Graphics.DrawString(name, boldFont, textBrush, nameX, nameY);
            e.Graphics.DrawString(description, regularFont, textBrush, nameX + nameSize.Width + 8, nameY);

            e.DrawFocusRectangle();

            boldFont.Dispose();
            regularFont.Dispose();
            textBrush.Dispose();
            boldFont = null;
            regularFont = null;
            textBrush = null;
        }

        private void lbxChildren_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnTask.Enabled = button7.Visible = button3.Visible = (lbxChildren.SelectedItem != null);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int iDx = lbxChildren.SelectedIndex;
            ObjectsListItem item = (ObjectsListItem)lbxChildren.Items[iDx];

            if (item == null) return;

            client.Self.Touch(item.Prim.LocalID);
        }

        private void btnWalk1_Click(object sender, EventArgs e)
        {
            if (btnWalk1.Text == "Walk to")
            {
                int iDx = lbxPrims.SelectedIndex;
                ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];

                if (item == null) return;

                Primitive prim = item.Prim;
                Vector3 pos = prim.Position;
                ulong regionHandle = client.Network.CurrentSim.Handle;

                int followRegionX = (int)(regionHandle >> 32);
                int followRegionY = (int)(regionHandle & 0xFFFFFFFF);
                ulong x = (ulong)(pos.X + followRegionX);
                ulong y = (ulong)(pos.Y + followRegionY);

                //string sPos = client.Network.CurrentSim.Name + "/" + item.Prim.Position.X + "/" + item.Prim.Position.Y + "/" + item.Prim.Position.Z;

                btnWalk1.Text = "Stop";
                client.Self.AutoPilotCancel();
                client.Self.AutoPilot(x, y, pos.Z);
            }
            else
            {
                client.Self.AutoPilotCancel();
                btnWalk1.Text = "Walk to";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int iDx = lbxPrims.SelectedIndex;
            ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];

            if (item == null) return;

            Primitive sPr = item.Prim;

            if (sPr.Properties.OwnerID == client.Self.AgentID)
            {
                Vector3 pos = sPr.Position;

                UUID pointID = UUID.Random();
                UUID beamID = UUID.Random();

                client.Self.PointAtEffect(client.Self.AgentID, item.Prim.ID, Vector3d.Zero, PointAtType.Select, pointID);
                client.Self.BeamEffect(client.Self.AgentID, item.Prim.ID, Vector3d.Zero, new Color4(0, 0, 255, 0), 250.0f, beamID);

                client.Self.Movement.TurnToward(pos);

                client.Inventory.RequestDeRezToInventory(item.Prim.LocalID);

                client.Self.PointAtEffect(client.Self.AgentID, UUID.Zero, Vector3d.Zero, PointAtType.None, pointID);
                client.Self.BeamEffect(client.Self.AgentID, UUID.Zero, Vector3d.Zero, new Color4(0, 0, 255, 0), 0, beamID);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (sloading) return;

            int iDx = lbxPrims.SelectedIndex;

            if (iDx < 0)
                return;

            ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];

            Primitive sPr = item.Prim;

            SetPerms(sPr);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (sloading) return;

            int iDx = lbxPrims.SelectedIndex;

            if (iDx < 0)
                return;

            ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];

            Primitive sPr = item.Prim;

            SetPerms(sPr);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (sloading) return;

            int iDx = lbxPrims.SelectedIndex;

            if (iDx < 0)
                return;

            ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];

            Primitive sPr = item.Prim;

            SetPerms(sPr);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            newrange = (float)numericUpDown1.Value;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string query = txtSearch.Text.Trim();

            lbxChildren.Items.Clear();
            lbxTask.Items.Clear();
            button3.Visible = button7.Visible = false;

            if (query.Length == 0)
            {
                btnClear.Enabled = false;
            }
            else
            {
                //SearchFor(query);
                btnClear.Enabled = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //txtSearch.Clear();
            //txtSearch.Select();

            if (txtSearch.Text.Length == 0) return;

            lbxPrims.Items.Clear();
            lbxChildren.Items.Clear();
            lbxTask.Items.Clear();

            button3.Visible = button7.Visible = false;

            string query = txtSearch.Text.Trim();

            SearchFor(query);
        }

        private void cboDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetFields();

            if (range != newrange)
            {
                range = newrange;
                //cboDisplay.SelectedIndex = 0;
                lbxPrims.Items.Clear();
                AddAllObjects();
            }
            else
            {
                if (cboDisplay.SelectedIndex == 0)
                {
                    DisplayObjects();
                }
                else if (cboDisplay.SelectedIndex == 1)
                {
                    DisplayForSale();
                }
                else if (cboDisplay.SelectedIndex == 2)
                {
                    DisplayScriptedObjects();
                }
                else if (cboDisplay.SelectedIndex == 3)
                {
                    DisplayMyObjects();
                }
                else if (cboDisplay.SelectedIndex == 4)
                {
                    DisplayOthersObjects();
                }
                else if (cboDisplay.SelectedIndex == 5)
                {
                    DisplayEmptyObjects();
                }
                else if (cboDisplay.SelectedIndex == 6)
                {
                    DisplayCreatedByMeObjects();
                }
                else if (cboDisplay.SelectedIndex == 7)
                {
                    DisplayFullModObjects();
                }
                else if (cboDisplay.SelectedIndex == 8)
                {
                    DisplayCFullModObjects();
                }

                lbxPrims.SortList();
            }
        }

        private void ResetFields()
        {
            lbxChildren.Items.Clear();
            lbxTask.Items.Clear();
            lbxPrims.Enabled = true;

            //btnClear.PerformClick();
            txtSearch.Text = string.Empty;   

            label11.Text = string.Empty;
            label15.Text = string.Empty;
            label13.Text = string.Empty;
            label9.Text = string.Empty;
            lblOwner.Text = string.Empty;
            lblUUID.Text = string.Empty;
            label3.Text = string.Empty;
            label5.Text = string.Empty;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            //ResetFields();

            if (range != newrange)
            {
                //range = newrange;
                if (cboDisplay.SelectedIndex == 0)
                {
                    range = newrange;
                    lbxPrims.Items.Clear();
                    AddAllObjects();
                }
                else
                {
                    cboDisplay.SelectedIndex = 0;
                }
            }
            //else
            //{
            //    if (cboDisplay.SelectedIndex == -1)
            //    {
            //        MessageBox.Show("Select a 'Display' option from above first.", "Object Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }

            //    if (cboDisplay.SelectedIndex == 0)
            //    {
            //        DisplayObjects();
            //    }
            //    else if (cboDisplay.SelectedIndex == 1)
            //    {
            //        DisplayForSale();
            //    }
            //    else if (cboDisplay.SelectedIndex == 2)
            //    {
            //        DisplayScriptedObjects();
            //    }

            //    lbxPrims.SortList();
            //}
        }

        private void GetTaskInventory(UUID objID, uint localID)
        {
            lbxTask.Items.Clear();

            List<InventoryBase> items = client.Inventory.GetTaskInventory(objID, localID, 1000 * 30);

            if (items != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] is InventoryFolder)
                    {
                        //ListViewItem fitem = lbxTask.Items.Add(items[i].Name, "- " + items[i].Name + " folder", string.Empty);
                        //fitem.Tag = fitem;
                    }
                    else
                    {
                        InventoryItem sitem = (InventoryItem)items[i];
                        string perms = string.Empty;

                        if (sitem.OwnerID == client.Self.AgentID)
                        {
                            if ((sitem.Permissions.OwnerMask & PermissionMask.Modify) != PermissionMask.Modify)
                            {
                                perms = "(no modify)";
                            }

                            if ((sitem.Permissions.OwnerMask & PermissionMask.Copy) != PermissionMask.Copy)
                            {
                                perms += " (no copy)";
                            }

                            if ((sitem.Permissions.OwnerMask & PermissionMask.Transfer) != PermissionMask.Transfer)
                            {
                                perms += " (no transfer)";
                            }
                        }
                        else
                        {
                            if ((sitem.Permissions.EveryoneMask & PermissionMask.Modify) != PermissionMask.Modify)
                            {
                                perms += "(no modify)";
                            }

                            if ((sitem.Permissions.EveryoneMask & PermissionMask.Copy) != PermissionMask.Copy)
                            {
                                perms += " (no copy)";
                            }

                            if ((sitem.Permissions.EveryoneMask & PermissionMask.Transfer) != PermissionMask.Transfer)
                            {
                                perms += " (no transfer)";
                            }
                        }

                        ListViewItem litem = lbxTask.Items.Add(sitem.Name, "   - [" + sitem.AssetType + "] " + sitem.Name + " " + perms, string.Empty);
                        litem.Tag = sitem;
                    }
                }
            }

            pBar2.Visible = false;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lbxTask_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label11_MouseHover(object sender, EventArgs e)
        {
            if (label11.Text.Length > 0)
            {
                toolTip.Show(label11);
            }
        }

        private void label11_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Close();
        }

        private void lbxPrims_Leave(object sender, EventArgs e)
        {
            gbxInworld.Enabled = (lbxPrims.SelectedItem != null);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int iDx = lbxPrims.SelectedIndex;

            if (iDx < 0)
                return;

            pBar2.Visible = true;
            pBar2.Refresh();

            ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];
            Primitive sPr = item.Prim;

            GetTaskInventory(sPr.ID, sPr.LocalID);
        }

        private void btnTask_Click(object sender, EventArgs e)
        {
            int iDx = lbxChildren.SelectedIndex;

            if (iDx < 0)
                return;

            pBar2.Visible = true;
            pBar2.Refresh();

            ObjectsListItem item = (ObjectsListItem)lbxChildren.Items[iDx];
            Primitive sPr = item.Prim;

            GetTaskInventory(sPr.ID, sPr.LocalID);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int iDx = lbxPrims.SelectedIndex;
            ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];

            if (item == null) return;

            if (instance.MuteList.Rows.Contains(item.Prim.ID.ToString()))
            {
                MessageBox.Show(item.Prim.Properties.Name + " is already in your mute list.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataRow dr = instance.MuteList.NewRow();
            dr["uuid"] = item.Prim.ID;
            dr["mute_name"] = item.Prim.Properties.Name;
            instance.MuteList.Rows.Add(dr);

            MessageBox.Show(item.Prim.Properties.Name + " is now muted.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int iDx = lbxChildren.SelectedIndex;
            ObjectsListItem item = (ObjectsListItem)lbxChildren.Items[iDx];

            if (item == null) return;

            if (button7.Text == "Sit On")
            {
                instance.State.SetSitting(true, item.Prim.ID);
                button7.Text = "Stand Up";
            }
            else if (button7.Text == "Stand Up")
            {
                instance.State.SetSitting(false, item.Prim.ID);
                button7.Text = "Sit On";
            }
        }

        private void label11_Leave(object sender, EventArgs e)
        {
            if (sloading) return;

            int iDx = lbxPrims.SelectedIndex;

            if (iDx < 0)
                return;

            ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];

            Primitive sPr = item.Prim;

            SetPerms(sPr);
        }

        private void label15_Leave(object sender, EventArgs e)
        {
            if (sloading) return;

            int iDx = lbxPrims.SelectedIndex;

            if (iDx < 0)
                return;

            ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];

            Primitive sPr = item.Prim;

            SetPerms(sPr);
        }

        private void lvwRadar_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pBar2_Click(object sender, EventArgs e)
        {

        }

        private void lbxTask_DoubleClick(object sender, EventArgs e)
        {
            if (lbxTask.SelectedItems.Count == 1)
            {
                int iDx = lbxPrims.SelectedIndex;

                if (iDx < 0) return;

                ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];
                Primitive sPr = item.Prim;

                if (sPr.Properties.OwnerID != client.Self.AgentID)
                {
                    return;
                }

                InventoryItem llitem = ((ListViewItem)lbxTask.SelectedItems[0]).Tag as InventoryItem;
                ListViewItem selitem = lbxTask.SelectedItems[0];

                if (llitem.InventoryType == InventoryType.LSL)
                {
                    // TPV precaution just in case SL messes up and allows it
                    if ((llitem.Permissions.OwnerMask & PermissionMask.Modify) != PermissionMask.Modify)
                    {
                        return;
                    }

                    InventoryLSL sobj = (InventoryLSL)selitem.Tag;

                    if (sobj == null) return;

                    if (sobj is InventoryLSL)
                    {
                        // open the Script Manager
                        (new frmScriptEditor(instance, sobj, sPr)).Show();
                        return;
                    }
                }
                else if (llitem.InventoryType == InventoryType.Notecard)
                {
                    // TPV precaution just in case SL messes up and allows it
                    if ((llitem.Permissions.OwnerMask & PermissionMask.Modify) != PermissionMask.Modify)
                    {
                        return;
                    }

                    InventoryNotecard nobj = (InventoryNotecard)selitem.Tag;
                    if (nobj == null) return;

                    if (nobj is InventoryNotecard)
                    {
                        (new frmNotecardEditor(instance, nobj, sPr)).Show();
                    }
                }
            }
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode node = e.Data.GetData(typeof(TreeNode)) as TreeNode;

            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                int iDx = lbxPrims.SelectedIndex;

                if (iDx < 0)
                {
                    MessageBox.Show("You must first select an object before yo ucan drop an item.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);     
                    return;
                }

                ObjectsListItem tiobject;

                int iDx2 = lbxChildren.SelectedIndex;

                if (iDx2 < 0)
                {

                    tiobject = (ObjectsListItem)lbxPrims.Items[iDx];
                }
                else
                {
                    tiobject = (ObjectsListItem)lbxChildren.Items[iDx2];
                }

                Primitive sPr = tiobject.Prim;

                InventoryItem item = node.Tag as InventoryItem;

                if ((item.Permissions.OwnerMask & PermissionMask.Copy) != PermissionMask.Copy)
                {
                    DialogResult res = MessageBox.Show("This is a 'no copy' item and you will lose ownership if you continue.", "Warning", MessageBoxButtons.OKCancel);

                    if (res == DialogResult.Cancel) return;
                }

                if (item.InventoryType == InventoryType.LSL)
                {
                    client.Inventory.CopyScriptToTask(sPr.LocalID, item, true);
                }
                else
                {
                    client.Inventory.UpdateTaskInventory(sPr.LocalID, item);
                }

                //client.Inventory.TaskInventoryReply += new EventHandler<TaskInventoryReplyEventArgs>(Inventory_TaskInventoryReply); 

                button4.PerformClick();  
                button6.PerformClick();  
            }
        }

        //void Inventory_TaskInventoryReply(object sender, TaskInventoryReplyEventArgs e)
        //{
        //    e.AssetFilename
        //}

        private void Item_CopiedCallback(InventoryBase item)
        {
            //string citem = item.Name;
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
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

        private void textBox1_DragOver(object sender, DragEventArgs e)
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

        private void lbxTask_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete) return;

            DeleteTaskItem();
        }

        private void DeleteTaskItem()
        {
            int iDx = lbxPrims.SelectedIndex;

            if (iDx < 0) return;

            ObjectsListItem item;

            int iDx2 = lbxChildren.SelectedIndex;

            if (iDx2 < 0)
            {
                item = (ObjectsListItem)lbxPrims.Items[iDx];
            }
            else
            {
                item = (ObjectsListItem)lbxChildren.Items[iDx2];
            }

            Primitive sPr = item.Prim;

            InventoryItem llitem = ((ListViewItem)lbxTask.SelectedItems[0]).Tag as InventoryItem;
            //ListViewItem selitem = lbxTask.SelectedItems[0];

            client.Inventory.RemoveTaskInventory(sPr.LocalID, llitem.UUID, client.Network.CurrentSim);

            button4.PerformClick();
            button6.PerformClick();  
        }

        private void CopyTaskItem()
        {
            int iDx = lbxPrims.SelectedIndex;

            if (iDx < 0) return;

            ObjectsListItem item;

            int iDx2 = lbxChildren.SelectedIndex;

            if (iDx2 < 0)
            {
                item = (ObjectsListItem)lbxPrims.Items[iDx];
            }
            else
            {
                item = (ObjectsListItem)lbxChildren.Items[iDx2];
            }

            Primitive sPr = item.Prim;

            InventoryItem llitem = ((ListViewItem)lbxTask.SelectedItems[0]).Tag as InventoryItem;
            //ListViewItem selitem = lbxTask.SelectedItems[0];


            InventoryFolder ifolders = client.Inventory.Store.RootFolder;
            List<InventoryBase> foundfolders = client.Inventory.Store.GetContents(ifolders);
            
            bool folderfound = false;
            UUID newfolder = UUID.Zero;

            foreach (InventoryBase o in foundfolders)
            {
                if (o.Name.ToLower(CultureInfo.CurrentCulture) == llitem.Name.ToLower(CultureInfo.CurrentCulture))
                {
                    if (o is InventoryFolder)
                    {
                        folderfound = true;
                        newfolder = o.UUID; 
                    }
                }
            }

            if (!folderfound)
            {
                newfolder = client.Inventory.CreateFolder(client.Inventory.Store.RootFolder.UUID, llitem.Name);
            }

            client.Inventory.MoveTaskInventory(sPr.LocalID, llitem.UUID, newfolder, client.Network.CurrentSim);

            button4.PerformClick();
            button6.PerformClick();
        }

        private void lbxTask_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                mnuTask.Show(this.lbxTask, new Point(e.X, e.Y));     
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            DeleteTaskItem();
        }

        private void mnuTask_Opening(object sender, CancelEventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                lbxPrims.SortByName = true;
                lbxPrims.SortList();
                instance.Config.CurrentConfig.SortByDistance = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                lbxPrims.location = instance.SIMsittingPos();
                lbxPrims.SortByName = false;
                lbxPrims.SortList();
                instance.Config.CurrentConfig.SortByDistance = true;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            UUID aID = (UUID)lblOwner.Text;

            (new frmProfile(instance, label9.Text, aID)).Show();
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void pB1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int iDx = lbxPrims.SelectedIndex;
            ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];

            if (item == null) return;

            Primitive sPr = item.Prim;

            //if (sPr.Properties.OwnerID != client.Self.AgentID) return;

            client.Inventory.RequestDeRezToInventory(sPr.LocalID, DeRezDestination.ReturnToOwner, UUID.Zero, UUID.Random());
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int iDx = lbxPrims.SelectedIndex;
            ObjectsListItem item = (ObjectsListItem)lbxPrims.Items[iDx];

            if (item == null) return;

            //(new META3D(instance, item.Prim.LocalID)).Show();
            (new META3D(instance, item)).Show();
        }

        private void picAutoSit_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://www.metabolt.net/metawiki/ObjectManager.ashx");
        }

        private void picAutoSit_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show(picAutoSit);
        }

        private void picAutoSit_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Close();  
        }
    }
}