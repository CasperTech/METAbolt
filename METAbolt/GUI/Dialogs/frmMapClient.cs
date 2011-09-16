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
using System.Reflection;
using OpenMetaverse;
using System.Drawing.Imaging;
using System.Net;
using System.Timers;
using System.Diagnostics;
using SLNetworkComm;
using OpenMetaverse.Imaging;
using PopupControl;
using OpenMetaverse.Assets;
using System.Threading;
using ExceptionReporting; 

/* Some of this code has been borrowed from the libsecondlife GUI */

namespace METAbolt
{
    public partial class frmMapClient : Form
    {
        private METAboltInstance instance;
        private GridClient client; // = new GridClient();
        private SLNetCom netcom;
        private UUID _MapImageID;
        private Image _MapLayer;
        private Image _LandLayer;
        private int px = 128;
        private int py = 128;
        private Simulator sim;

        private Popup toolTip;
        private CustomToolTip customToolTip;
        private bool showing = false;
        private UUID avuuid = UUID.Zero;
        private string avname = string.Empty;
        private ManualResetEvent TPEvent = new ManualResetEvent(false);
        private int clickedx = 0;
        private int clickedy = 0;
        private GridRegion selregion;
        private Image selectedmap;
        private bool mloaded = false;
        private Image orgmap;

        private ExceptionReporter reporter = new ExceptionReporter();

        internal class ThreadExceptionHandler
        {
            public void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
            {
                ExceptionReporter reporter = new ExceptionReporter();
                reporter.Show(e.Exception);
            }
        }

        public frmMapClient(METAboltInstance instance)
        {
            InitializeComponent();

            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.instance = instance;
            netcom = this.instance.Netcom;

            client = this.instance.Client;
            sim = client.Network.CurrentSim;

            client.Grid.CoarseLocationUpdate += new EventHandler<CoarseLocationUpdateEventArgs>(Grid_OnCoarseLocationUpdate);
            client.Network.SimChanged += new EventHandler<SimChangedEventArgs>(Network_OnCurrentSimChanged);

            client.Grid.GridRegion += new EventHandler<GridRegionEventArgs>(Grid_OnGridRegion);
            netcom.Teleporting += new EventHandler<TeleportingEventArgs>(netcom_Teleporting);
            netcom.TeleportStatusChanged += new EventHandler<TeleportEventArgs>(netcom_TeleportStatusChanged);

            string msg1 = "Yellow dot with red border = your avatar \nGreen dots = avs at your altitude\nRed squares = avs 20m+ below you\nBlue squares = avs 20m+ above you\n\n Click on map area to get TP position.";
            toolTip = new Popup(customToolTip = new CustomToolTip(instance, msg1));
            toolTip.AutoClose = false;
            toolTip.FocusOnOpen = false;
            toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;

            //List<AvLocation> avlocations = new List<AvLocation>();

            world.Cursor = Cursors.Cross;
            pictureBox2.Cursor = Cursors.Cross;
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

        private void Assets_OnImageReceived(TextureRequestState image, AssetTexture texture)
        {
            if (texture.AssetID == _MapImageID)
            {
                ManagedImage nullImage;
                OpenJPEG.DecodeToImage(texture.AssetData, out nullImage, out _MapLayer);

                //UpdateMiniMap(sim);
                BeginInvoke((MethodInvoker)delegate { UpdateMiniMap(sim); });
            }
        }

        private void Network_OnCurrentSimChanged(object sender, SimChangedEventArgs e)
        {
            //GetMap();

            this.BeginInvoke(new MethodInvoker(delegate()
            {
                if (chkForSale.Checked)
                {
                    chkForSale.Checked = false;
                }
            }));

            //_LandLayer = null;
            //_MapLayer = null;
            //client.Grid.RequestMapRegion(client.Network.CurrentSim.Name, GridLayerType.Objects);

            BeginInvoke((MethodInvoker)delegate { GetMap(); });
        }

        private void GetMap()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    GetMap();
                }));

                //BeginInvoke((MethodInvoker)delegate { GetMap(); });
                return;
            }

            GridRegion region;
            //List<Simulator> connectedsims = client.Network.Simulators;

            if (_MapLayer == null || sim != client.Network.CurrentSim)
            {
                sim = client.Network.CurrentSim;
                TabCont.TabPages[0].Text = client.Network.CurrentSim.Name;

                if (!chkForSale.Checked)
                {
                    if (client.Grid.GetGridRegion(client.Network.CurrentSim.Name, GridLayerType.Objects, out region))
                    {
                        _MapImageID = region.MapImageID;
                        client.Assets.RequestImage(_MapImageID, ImageType.Baked, Assets_OnImageReceived);
                    }
                }
                else
                {
                    if (client.Grid.GetGridRegion(client.Network.CurrentSim.Name, GridLayerType.LandForSale, out region))
                    {
                        _MapImageID = region.MapImageID;
                        client.Assets.RequestImage(_MapImageID, ImageType.Baked, Assets_OnImageReceived);
                    }
                }
            }
            else
            {
                //UpdateMiniMap(sim);
                BeginInvoke(new OnUpdateMiniMap(UpdateMiniMap), new object[] { sim });
            }
        }

        private delegate void OnUpdateMiniMap(Simulator ssim);
        private void UpdateMiniMap(Simulator ssim)
        {
            if (this.InvokeRequired) this.BeginInvoke((MethodInvoker)delegate { UpdateMiniMap(ssim); });
            else
            {
                sim = ssim;

                if (sim != client.Network.CurrentSim) return;

                //Bitmap nbmp = new Bitmap(256, 256);

                Bitmap bmp = _MapLayer == null ? new Bitmap(256, 256) : (Bitmap)_MapLayer.Clone();

                Graphics g = Graphics.FromImage(bmp);

                //nbmp.Dispose(); 

                if (_MapLayer == null)
                {
                    g.Clear(this.BackColor);
                    g.FillRectangle(Brushes.White, 0f, 0f, 256f, 256f);
                    label6.Visible = true;
                }
                else
                {
                    label6.Visible = false;
                }

                if (_LandLayer != null)
                {
                    //nbmp = new Bitmap(256, 256);

                    bmp = _LandLayer == null ? new Bitmap(256, 256) : (Bitmap)_LandLayer.Clone();
                    //g = Graphics.FromImage((Bitmap)_LandLayer.Clone());

                    g = Graphics.FromImage(bmp);

                    //nbmp.Dispose(); 

                    //ColorMatrix cm = new ColorMatrix();
                    //cm.Matrix00 = cm.Matrix11 = cm.Matrix22 = cm.Matrix44 = 1f;
                    //cm.Matrix33 = 1.0f;

                    //ImageAttributes ia = new ImageAttributes();
                    //ia.SetColorMatrix(cm);

                    if (_MapLayer != null)
                    {
                        g.DrawImage(_MapLayer, new Rectangle(0, 0, _MapLayer.Width, _MapLayer.Height), 0, 0, _MapLayer.Width, _MapLayer.Height, GraphicsUnit.Pixel);   //, ia);
                    }
                }

                // Draw compass points
                StringFormat strFormat = new StringFormat();
                strFormat.Alignment = StringAlignment.Center;

                g.DrawString("N", new Font("Arial", 12), Brushes.Black, new RectangleF(0, 0, bmp.Width, bmp.Height), strFormat);
                g.DrawString("N", new Font("Arial", 9, FontStyle.Bold), Brushes.White, new RectangleF(0, 2, bmp.Width, bmp.Height), strFormat);

                strFormat.LineAlignment = StringAlignment.Center;
                strFormat.Alignment = StringAlignment.Near;

                g.DrawString("W", new Font("Arial", 12), Brushes.Black, new RectangleF(0, 0, bmp.Width, bmp.Height), strFormat);
                g.DrawString("W", new Font("Arial", 9, FontStyle.Bold), Brushes.White, new RectangleF(2, 0, bmp.Width, bmp.Height), strFormat);

                strFormat.LineAlignment = StringAlignment.Center;
                strFormat.Alignment = StringAlignment.Far;

                g.DrawString("E", new Font("Arial", 12), Brushes.Black, new RectangleF(0, 0, bmp.Width, bmp.Height), strFormat);
                g.DrawString("E", new Font("Arial", 9, FontStyle.Bold), Brushes.White, new RectangleF(-2, 0, bmp.Width, bmp.Height), strFormat);

                strFormat.LineAlignment = StringAlignment.Far;
                strFormat.Alignment = StringAlignment.Center;

                g.DrawString("S", new Font("Arial", 12), Brushes.Black, new RectangleF(0, 0, bmp.Width, bmp.Height), strFormat);
                g.DrawString("S", new Font("Arial", 9, FontStyle.Bold), Brushes.White, new RectangleF(0, 0, bmp.Width, bmp.Height), strFormat);

                // V0.9.8.0 changes for OpenSIM compatibility
                Vector3 myPos;

                // Rollback change from 9.2.1
                if (!sim.AvatarPositions.ContainsKey(client.Self.AgentID))
                {
                    myPos = instance.SIMsittingPos();
                }
                else
                {
                    myPos = sim.AvatarPositions[client.Self.AgentID];
                }

                if (chkResident.Checked)
                {
                    int i = 0;
                    Rectangle rect = new Rectangle();

                    client.Network.CurrentSim.AvatarPositions.ForEach(
                        delegate(KeyValuePair<UUID, Vector3> pos)
                        {
                            int x = (int)pos.Value.X - 2;
                            int y = 255 - (int)pos.Value.Y - 2;

                            rect = new Rectangle(x, y, 7, 7);

                            if (pos.Key != client.Self.AgentID)
                            {
                                if (myPos.Z - pos.Value.Z > 20)
                                {
                                    g.FillRectangle(Brushes.DarkRed, rect);
                                    g.DrawRectangle(new Pen(Brushes.Red, 1), rect);
                                }
                                else if (myPos.Z - pos.Value.Z > -21 && myPos.Z - pos.Value.Z < 21)
                                {
                                    g.FillEllipse(Brushes.LightGreen, rect);
                                    g.DrawEllipse(new Pen(Brushes.Green, 1), rect);
                                }
                                else
                                {
                                    g.FillRectangle(Brushes.MediumBlue, rect);
                                    g.DrawRectangle(new Pen(Brushes.Red, 1), rect);
                                }
                            }

                            i++;
                        }
                    );
                }
                

                // Draw self position
                Rectangle myrect = new Rectangle((int)Math.Round(myPos.X, 0) - 2, 255 - ((int)Math.Round(myPos.Y, 0) - 2), 7, 7);
                g.FillEllipse(new SolidBrush(Color.Yellow), myrect);
                g.DrawEllipse(new Pen(Brushes.Red, 3), myrect);

                if (clickedx != 0 && clickedy != 0)
                {
                    //PlotSelected(clickedx, clickedy);
                    Rectangle selectedrect = new Rectangle(clickedx - 2, clickedy - 2, 10, 10);
                    g.DrawEllipse(new Pen(Brushes.Red, 2), selectedrect);
                }

                g.DrawImage(bmp, 0, 0);

                world.Image = bmp;

                strFormat.Dispose(); 
                g.Dispose();

                string strInfo = string.Format("Total Avatars: {0}", client.Network.CurrentSim.AvatarPositions.Count);
                lblSimData.Text = strInfo;

                strInfo = string.Format("{0}/{1}/{2}/{3}", client.Network.CurrentSim.Name,
                                                                            Math.Round(myPos.X, 0),
                                                                            Math.Round(myPos.Y, 0),
                                                                            Math.Round(myPos.Z, 0));
                label2.Text = "http://slurl.com/secondlife/" + strInfo;
            }
        }

        private void Grid_OnCoarseLocationUpdate(object sender, CoarseLocationUpdateEventArgs e)
        {
            try
            {
                //UpdateMiniMap(sim);
                BeginInvoke((MethodInvoker)delegate { UpdateMiniMap(e.Simulator); });
            }
            catch { ; }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMapClient_Load(object sender, EventArgs e)
        {
            this.CenterToParent();

            GetMap();

            Vector3 apos = instance.SIMsittingPos();
            float aZ = apos.Z;

            //printMap();
            nuX.Value = 128;
            nuY.Value = 128;
            nuZ.Value = (decimal)aZ;

            chkForSale.Checked = true;
            chkForSale.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (instance.State.IsSitting)
            {
                client.Self.Stand();
                instance.State.SetStanding();
                TPEvent.WaitOne(2000, false);
            }

            clickedx = 0;
            clickedy = 0;

            button1.Enabled = false;

            try
            {
                //netcom.Teleport(client.Network.CurrentSim.Name, new Vector3((float)nuX.Value, (float)nuY.Value, (float)nuZ.Value));
                client.Self.Teleport(client.Network.CurrentSim.Name, new Vector3((float)nuX.Value, (float)nuY.Value, (float)nuZ.Value));
            }
            catch
            {
                MessageBox.Show("An error occured while Teleporting. \n Please re-try later.", "METAbolt");
                return;
            }
        }

        private void world_MouseUp(object sender, MouseEventArgs e)
        {
            px = e.X;
            py = 255 - e.Y;

            nuX.Value = (decimal)px;
            nuY.Value = (decimal)py;
            nuZ.Value = (decimal)10;

            clickedx = e.X;
            clickedy = e.Y; 

            PlotSelected(e.X, e.Y);

            Point mouse = new Point(e.X, e.Y);

            METAboltInstance.AvLocation CurrentLoc = null;

            button1.Enabled = true; 

            try
            {
                CurrentLoc = instance.avlocations.Find(delegate(METAboltInstance.AvLocation g) { return g.Rectangle.Contains(mouse) == true; });
            }
            catch { ; }

            if (CurrentLoc != null)
            {
                (new frmProfile(instance, avname, avuuid)).Show();
            }
        }

        private void PlotSelected(int x, int y)
        {
            if (world.Image == null) return;

            try
            {
                //UpdateMiniMap(sim);
                BeginInvoke(new OnUpdateMiniMap(UpdateMiniMap), new object[] { sim });

                Bitmap map = (Bitmap)world.Image;
                Graphics g = Graphics.FromImage(map);

                Rectangle selectedrect = new Rectangle(x - 2, y - 2, 10, 10);
                g.DrawEllipse(new Pen(Brushes.Red, 2), selectedrect);
                world.Image = map;

                g.Dispose();
            }
            catch
            {
                // do nothing for now
                return;
            }
        }

        private void frmMapClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Grid.CoarseLocationUpdate -= new EventHandler<CoarseLocationUpdateEventArgs>(Grid_OnCoarseLocationUpdate);
            client.Network.SimChanged -= new EventHandler<SimChangedEventArgs>(Network_OnCurrentSimChanged);

            client.Grid.GridRegion -= new EventHandler<GridRegionEventArgs>(Grid_OnGridRegion);
            netcom.Teleporting -= new EventHandler<TeleportingEventArgs>(netcom_Teleporting);
            netcom.TeleportStatusChanged -= new EventHandler<TeleportEventArgs>(netcom_TeleportStatusChanged);

            _LandLayer = _MapLayer;
            _MapLayer = null;
            client.Grid.RequestMapRegion(client.Network.CurrentSim.Name, GridLayerType.Objects);
        }

        private void frmMapClient_Enter(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            toolTip.Show(pictureBox1);
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Close();
        }

        private void nuZ_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nuY_ValueChanged(object sender, EventArgs e)
        {
            if (world.Image == null) return;

            clickedx = (int)nuX.Value;
            clickedy = (int)nuY.Value;
            PlotSelected(clickedx, clickedy);

            button1.Enabled = true; 
        }

        private void world_MouseMove(object sender, MouseEventArgs e)
        {
            Point mouse = new Point(e.X, e.Y);

            METAboltInstance.AvLocation CurrentLoc = null;

            try
            {
                CurrentLoc = instance.avlocations.Find(delegate(METAboltInstance.AvLocation g) { return g.Rectangle.Contains(mouse) == true; });
            }
            catch { ; }

            if (CurrentLoc != null)
            {
                if (!showing)
                {
                    UUID akey = (UUID)CurrentLoc.LocationName;
                    string apstn = "\nCoords.: " + CurrentLoc.Position.X.ToString() + "/" + CurrentLoc.Position.Y.ToString() + "/" + CurrentLoc.Position.Z.ToString();

                    world.Cursor = Cursors.Hand;
                    string anme = string.Empty;

                    lock (instance.avnames)
                    {
                        if (instance.avnames.ContainsKey(akey))
                        {
                            avname = instance.avnames[akey];

                            if (instance.avtags.ContainsKey(akey))
                            {
                                anme = "\nTag: " + instance.avtags[akey];
                            }

                            toolTip1.SetToolTip(world, avname + anme + apstn);
                            avuuid = akey;
                        }
                        else
                        {
                            toolTip1.SetToolTip(world, CurrentLoc.LocationName + apstn);
                        }
                    }

                    showing = true;
                }
            }
            else
            {
                world.Cursor = Cursors.Cross;
                toolTip1.RemoveAll();
                showing = false;
            }
        }

        private void world_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            clickedx = 0;
            clickedy = 0;

            button1.Enabled = false; 
        }

        private void chkForSale_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkForSale.Checked)
            {
                _LandLayer = null;
                _MapLayer = null;
                client.Grid.RequestMapRegion(client.Network.CurrentSim.Name, GridLayerType.LandForSale);
            }
            else
            {
                _LandLayer = _MapLayer;
                _MapLayer = null;
                client.Grid.RequestMapRegion(client.Network.CurrentSim.Name, GridLayerType.Objects);
            }

            GetMap(); 
        }

        private void TabCont_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabCont.SelectedIndex == 0)
            {
                this.Width = 289;
                return;
            }

            if (TabCont.SelectedIndex == 1)
            {
                this.Width = 592;
                txtSearchFor.Focus(); 
                return;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            StartRegionSearch();
            mloaded = true;
        }

        private void StartRegionSearch()
        {
            lbxRegionSearch.Items.Clear();

            client.Grid.RequestMapRegion(txtSearchFor.Text.Trim(), GridLayerType.Objects);
        }

        //Separate thread
        private void Grid_OnGridRegion(object sender, GridRegionEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => Grid_OnGridRegion(sender, e)));
                return;
            }

            BeginInvoke(new MethodInvoker(delegate()
            {
                RegionSearchResult(e.Region);
            }));
        }

        //UI thread
        private void RegionSearchResult(GridRegion region)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => RegionSearchResult(region)));
                return;
            }

            if (!mloaded)
            {
                return;
            }

            if (TabCont.SelectedIndex == 0) return;

            RegionSearchResultItem item = new RegionSearchResultItem(instance, region, lbxRegionSearch);
            int index = lbxRegionSearch.Items.Add(item);
            item.ListIndex = index;
            selregion = item.Region;
        }

        private void netcom_TeleportStatusChanged(object sender, TeleportEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => netcom_TeleportStatusChanged(sender, e)));
                return;
            }

            try
            {
                switch (e.Status)
                {
                    case TeleportStatus.Start:
                        RefreshControls();
                        pnlTeleporting.Visible = true;
                        lblTeleportStatus.Visible = true;
                        break;
                    case TeleportStatus.Progress:
                        lblTeleportStatus.Text = e.Message;
                        break;

                    case TeleportStatus.Failed:
                        RefreshControls();
                        pnlTeleporting.Visible = false;
                        MessageBox.Show(e.Message, "Teleport", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;

                    case TeleportStatus.Finished:
                        RefreshControls();
                        pnlTeleporting.Visible = false;
                        //lblTeleportStatus.Visible = false;
                        this.Close();
                        break;
                }
            }
            catch { ; }
        }

        private void RefreshControls()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    RefreshControls();
                    return;
                }));
            }

            try
            {
                if (netcom.IsTeleporting)
                {
                    pnlTeleportOptions.Enabled = false;
                    btnTeleport.Enabled = false;
                    pnlTeleporting.Visible = true;
                }
                else
                {
                    pnlTeleportOptions.Enabled = true;
                    btnTeleport.Enabled = true;
                    pnlTeleporting.Visible = false;
                }
            }
            catch
            {
                ;
            }
        }

        private void netcom_Teleporting(object sender, TeleportingEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => netcom_Teleporting(sender, e)));
                return;
            }

            try
            {
                RefreshControls();
            }
            catch
            {
                ;
            }
        }

        private void txtRegion_TextChanged(object sender, EventArgs e)
        {
            btnTeleport.Enabled = (txtRegion.Text.Trim().Length > 0);
        }

        private void btnTeleport_Click(object sender, EventArgs e)
        {
            if (instance.State.IsSitting)
            {
                client.Self.Stand();
                instance.State.SetStanding();
            }

            pnlTeleporting.Visible = true;

            if (selregion.RegionHandle == 0 && !string.IsNullOrEmpty(txtRegion.Text))
            {
                //RefreshControls();
                netcom.Teleport(txtRegion.Text.Trim(), new Vector3((float)nudX1.Value, (float)nudY1.Value, (float)nudZ1.Value));
            }
            else
            {
                client.Self.RequestTeleport(selregion.RegionHandle, new Vector3((float)nudX1.Value, (float)nudY1.Value, (float)nudZ1.Value));
            }
        }

        private void txtSearchFor_TextChanged(object sender, EventArgs e)
        {
            btnFind.Enabled = (txtSearchFor.Text.Trim().Length > 0);
        }

        private void lbxRegionSearch_DoubleClick(object sender, EventArgs e)
        {
            //if (lbxRegionSearch.SelectedItem == null) return;
            //RegionSearchResultItem item = (RegionSearchResultItem)lbxRegionSearch.SelectedItem;
            
            //selregion = item.Region;
            //txtRegion.Text = item.Region.Name;
            //nudX.Value = 128;
            //nudY.Value = 128;
            //nudZ.Value = 0;

            //pictureBox2.Image = item.MapImage; 
        }

        private void txtSearchFor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            if (!btnFind.Enabled) return;
            e.SuppressKeyPress = true;

            StartRegionSearch();
        }

        private void lbxRegionSearch_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index < 0) return;

            RegionSearchResultItem itemToDraw = (RegionSearchResultItem)lbxRegionSearch.Items[e.Index];
            Brush textBrush = null;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                textBrush = new SolidBrush(Color.FromKnownColor(KnownColor.HighlightText));
            else
                textBrush = new SolidBrush(Color.FromKnownColor(KnownColor.ControlText));

            Font newFont = new Font(e.Font, FontStyle.Bold);
            SizeF stringSize = e.Graphics.MeasureString(itemToDraw.Region.Name, newFont);

            float iconSize = (float)64;   // trkIconSize.Value;
            float leftTextMargin = e.Bounds.Left + iconSize + 6.0f;
            float topTextMargin = e.Bounds.Top + 4.0f;

            if (itemToDraw.IsImageDownloaded)
            {
                if (itemToDraw.MapImage != null)
                {
                    e.Graphics.DrawImage(itemToDraw.MapImage, new RectangleF(e.Bounds.Left + 4.0f, e.Bounds.Top + 4.0f, iconSize, iconSize));
                }
            }
            else
            {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(200, 200, 200)), e.Bounds.Left + 4.0f, e.Bounds.Top + 4.0f, iconSize, iconSize);

                if (!itemToDraw.IsImageDownloading)
                    itemToDraw.RequestMapImage(125000.0f);
            }


            e.Graphics.DrawString(itemToDraw.Region.Name, newFont, textBrush, new PointF(leftTextMargin, topTextMargin));

            if (itemToDraw.GotAgentCount)
            {
                string peeps = " person";

                if (itemToDraw.Region.Agents != 1)
                {
                    peeps = " people";
                }

                string s = System.Convert.ToString(itemToDraw.Region.Agents);

                e.Graphics.DrawString(s + peeps, e.Font, textBrush, new PointF(leftTextMargin + stringSize.Width + 6.0f, topTextMargin));
            }
            else
            {
                if (!itemToDraw.GettingAgentCount)
                {
                    itemToDraw.RequestAgentLocations();
                }
            }

            switch (itemToDraw.Region.Access)
            {
                case SimAccess.PG:
                    e.Graphics.DrawString("PG", e.Font, textBrush, new PointF(leftTextMargin, topTextMargin + stringSize.Height));
                    break;

                case SimAccess.Mature:
                    e.Graphics.DrawString("Mature", e.Font, textBrush, new PointF(leftTextMargin, topTextMargin + stringSize.Height));
                    break;

                case SimAccess.Adult:
                    e.Graphics.DrawString("Adult", e.Font, textBrush, new PointF(leftTextMargin, topTextMargin + stringSize.Height));
                    break;

                case SimAccess.Down:
                    e.Graphics.DrawString("Offline", e.Font, new SolidBrush(Color.Red), new PointF(leftTextMargin, topTextMargin + stringSize.Height));
                    break;
            }

            e.Graphics.DrawLine(new Pen(Color.FromArgb(200, 200, 200)), new Point(e.Bounds.Left, e.Bounds.Bottom - 1), new Point(e.Bounds.Right, e.Bounds.Bottom - 1));
            e.DrawFocusRectangle();

            textBrush.Dispose();
            newFont.Dispose();
            textBrush = null;
            newFont = null;

            lbxRegionSearch.ItemHeight = 73;   // trkIconSize.Value + 10;
        }

        private void txtSearchFor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void lbxRegionSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbxRegionSearch.SelectedItem == null) return;
                RegionSearchResultItem item = (RegionSearchResultItem)lbxRegionSearch.SelectedItem;

                selregion = item.Region;
                txtRegion.Text = item.Region.Name;
                nudX1.Value = 128;
                nudY1.Value = 128;
                nudZ1.Value = 0;

                orgmap = item.MapImage;
                pictureBox2.Image = selectedmap = item.MapImage;

                Bitmap bmp = new Bitmap(selectedmap, 256, 256);

                Graphics g = Graphics.FromImage(bmp);

                Rectangle rect = new Rectangle();

                foreach (MapItem itm in item.AgentLocations)
                {
                    // Draw avatar location icons
                    int x = (int)itm.LocalX + 7;
                    int y = 255 - (int)itm.LocalY - 16;

                    rect = new Rectangle(x, y, 7, 7);

                    g.FillEllipse(Brushes.LightGreen, rect);
                    g.DrawEllipse(new Pen(Brushes.Green, 1), rect);
                }

                g.DrawImage(bmp, 0, 0);
                pictureBox2.Image = bmp;

                g.Dispose();
            }
            catch { ; }
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            px = e.X;
            py = 255 - e.Y;

            nudX1.Value = (decimal)px;
            nudY1.Value = (decimal)py;
            nudZ1.Value = (decimal)10;

            //Bitmap map = (Bitmap)pictureBox2.Image;
            //Graphics g = Graphics.FromImage(map);

            //Rectangle selectedrect = new Rectangle(e.X - 2, e.Y - 2, 10, 10);
            //g.DrawEllipse(new Pen(Brushes.Red, 2), selectedrect);
            //pictureBox2.Image = map;

            //g.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            nudX1.Value = 128;
            nudY1.Value = 128;
            nudZ1.Value = 0;

            pictureBox2.Image = orgmap;
        }

        private void nuX_Scroll(object sender, ScrollEventArgs e)
        {
            if (world.Image == null) return;

            clickedx = (int)nuX.Value;
            clickedy = (int)nuY.Value;
            PlotSelected(clickedx, clickedy);
            button1.Enabled = true; 
        }

        private void nuX_ValueChanged(object sender, EventArgs e)
        {
            if (world.Image == null) return;

            clickedx = (int)nuX.Value;
            clickedy = (int)nuY.Value;
            PlotSelected(clickedx, clickedy);
            button1.Enabled = true; 
        }

        private void nuY_MouseUp(object sender, MouseEventArgs e)
        {
            if (world.Image == null) return;

            clickedx = (int)nuX.Value;
            clickedy = (int)nuY.Value;
            PlotSelected(clickedx, clickedy);
            button1.Enabled = true; 
        }
    }
}
