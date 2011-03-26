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

namespace METAbolt
{
    public partial class frmMap : Form
    {
        private METAboltInstance instance;
        private GridClient client; // = new GridClient();
        private SLNetCom netcom;

        private UUID _MapImageID;
        private Image _MapLayer;
        private Image _LandLayer;
        private UUID _MapImageID1;
        private Image _MapLayer1;
        private Image _LandLayer1;
        private UUID _MapImageID2;
        private Image _MapLayer2;
        private Image _LandLayer2;
        private UUID _MapImageID3;
        private Image _MapLayer3;
        private Image _LandLayer3;
        private UUID _MapImageID4;
        private Image _MapLayer4;
        private Image _LandLayer4;
        private UUID _MapImageID5;
        private Image _MapLayer5;
        private Image _LandLayer5;
        private UUID _MapImageID6;
        private Image _MapLayer6;
        private Image _LandLayer6;
        private UUID _MapImageID7;
        private Image _MapLayer7;
        private Image _LandLayer7;
        private UUID _MapImageID8;
        private Image _MapLayer8;
        private Image _LandLayer8;
        private UUID _MapImageID9;
        private Image _MapLayer9;
        private Image _LandLayer9;
        private UUID _MapImageID10;
        private Image _MapLayer10;
        private Image _LandLayer10;

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

        public frmMap(METAboltInstance instance)
        {
            InitializeComponent();

            this.instance = instance;
            netcom = this.instance.Netcom;

            client = this.instance.Client;
            sim = client.Network.CurrentSim;
            client.Grid.CoarseLocationUpdate += new EventHandler<CoarseLocationUpdateEventArgs>(Grid_OnCoarseLocationUpdate);
            client.Network.SimChanged += new EventHandler<SimChangedEventArgs>(Network_OnCurrentSimChanged);

            string msg1 = "Yellow dot with red border = your avatar \nGreen dots = avs at your altitude\nRed squares = avs 20m+ below you\nBlue squares = avs 20m+ above you\n\n Click on map area to get TP position.";
            toolTip = new Popup(customToolTip = new CustomToolTip(instance, msg1));
            toolTip.AutoClose = false;
            toolTip.FocusOnOpen = false;
            toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;

            //List<AvLocation> avlocations = new List<AvLocation>();

            world4.Cursor = Cursors.Cross;
        }

        private void Assets_OnImageReceived(TextureRequestState image, AssetTexture texture)
        {
            if (texture.AssetID == _MapImageID)
            {
                ManagedImage nullImage;
                OpenJPEG.DecodeToImage(texture.AssetData, out nullImage, out _MapLayer);
            }

            if (texture.AssetID == _MapImageID1)
            {
                ManagedImage nullImage;
                OpenJPEG.DecodeToImage(texture.AssetData, out nullImage, out _MapLayer1);
            }

            if (texture.AssetID == _MapImageID2)
            {
                ManagedImage nullImage;
                OpenJPEG.DecodeToImage(texture.AssetData, out nullImage, out _MapLayer2);
            }

            if (texture.AssetID == _MapImageID3)
            {
                ManagedImage nullImage;
                OpenJPEG.DecodeToImage(texture.AssetData, out nullImage, out _MapLayer3);
            }

            if (texture.AssetID == _MapImageID4)
            {
                ManagedImage nullImage;
                OpenJPEG.DecodeToImage(texture.AssetData, out nullImage, out _MapLayer4);
            }

            if (texture.AssetID == _MapImageID5)
            {
                ManagedImage nullImage;
                OpenJPEG.DecodeToImage(texture.AssetData, out nullImage, out _MapLayer5);
            }

            if (texture.AssetID == _MapImageID6)
            {
                ManagedImage nullImage;
                OpenJPEG.DecodeToImage(texture.AssetData, out nullImage, out _MapLayer6);
            }

            if (texture.AssetID == _MapImageID7)
            {
                ManagedImage nullImage;
                OpenJPEG.DecodeToImage(texture.AssetData, out nullImage, out _MapLayer7);
            }

            if (texture.AssetID == _MapImageID8)
            {
                ManagedImage nullImage;
                OpenJPEG.DecodeToImage(texture.AssetData, out nullImage, out _MapLayer8);
            }

            if (texture.AssetID == _MapImageID9)
            {
                ManagedImage nullImage;
                OpenJPEG.DecodeToImage(texture.AssetData, out nullImage, out _MapLayer9);
            }

            //if (texture.AssetID == _MapImageID10)
            //{
            //    ManagedImage nullImage;
            //    OpenJPEG.DecodeToImage(texture.AssetData, out nullImage, out _MapLayer10);
            //}
        }

        private void Network_OnCurrentSimChanged(object sender, SimChangedEventArgs e)
        {
            //GetMap();

            if (chkForSale.Checked)
            {
                chkForSale.Checked = false;
            }

            _LandLayer = null;
            _MapLayer = null;
            _LandLayer1 = null;
            _MapLayer1 = null;
            _LandLayer2 = null;
            _MapLayer2 = null;
            _LandLayer3 = null;
            _MapLayer3 = null;
            _LandLayer4 = null;
            _MapLayer4 = null;
            _LandLayer5 = null;
            _MapLayer5 = null;
            _LandLayer6 = null;
            _MapLayer6 = null;
            _LandLayer7 = null;
            _MapLayer7 = null;
            _LandLayer8 = null;
            _MapLayer8 = null;
            _LandLayer9 = null;
            _MapLayer9 = null;
            _LandLayer10 = null;
            _MapLayer10 = null;

            client.Grid.RequestMapRegion(client.Network.CurrentSim.Name, GridLayerType.Objects);

            if (client.Network.Simulators.Count > 1)
            {
                try
                {
                    client.Grid.RequestMapRegion(client.Network.Simulators[1].Name, GridLayerType.Objects);
                    client.Grid.RequestMapRegion(client.Network.Simulators[2].Name, GridLayerType.Objects);
                    client.Grid.RequestMapRegion(client.Network.Simulators[3].Name, GridLayerType.Objects);
                    client.Grid.RequestMapRegion(client.Network.Simulators[4].Name, GridLayerType.Objects);
                    client.Grid.RequestMapRegion(client.Network.Simulators[5].Name, GridLayerType.Objects);
                    client.Grid.RequestMapRegion(client.Network.Simulators[6].Name, GridLayerType.Objects);
                    client.Grid.RequestMapRegion(client.Network.Simulators[7].Name, GridLayerType.Objects);
                    client.Grid.RequestMapRegion(client.Network.Simulators[8].Name, GridLayerType.Objects);
                    client.Grid.RequestMapRegion(client.Network.Simulators[9].Name, GridLayerType.Objects);
                    //client.Grid.RequestMapRegion(client.Network.Simulators[10].Name, GridLayerType.Objects);
                }
                catch { ; }
            }

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
                //TabCont.TabPages[0].Text = client.Network.CurrentSim.Name;

                if (!chkForSale.Checked)
                {
                    if (client.Grid.GetGridRegion(client.Network.CurrentSim.Name, GridLayerType.Objects, out region))
                    {
                        _MapImageID = region.MapImageID;
                        client.Assets.RequestImage(_MapImageID, ImageType.Baked, Assets_OnImageReceived);
                    }

                    if (client.Network.Simulators.Count > 1)
                    {
                        try
                        {
                            if (client.Grid.GetGridRegion(client.Network.Simulators[1].Name, GridLayerType.Objects, out region))
                            {
                                _MapImageID1 = region.MapImageID;
                                client.Assets.RequestImage(_MapImageID1, ImageType.Baked, Assets_OnImageReceived);
                            }

                            if (client.Grid.GetGridRegion(client.Network.Simulators[2].Name, GridLayerType.Objects, out region))
                            {
                                _MapImageID2 = region.MapImageID;
                                client.Assets.RequestImage(_MapImageID2, ImageType.Baked, Assets_OnImageReceived);
                            }

                            if (client.Grid.GetGridRegion(client.Network.Simulators[3].Name, GridLayerType.Objects, out region))
                            {
                                _MapImageID3 = region.MapImageID;
                                client.Assets.RequestImage(_MapImageID3, ImageType.Baked, Assets_OnImageReceived);
                            }

                            if (client.Grid.GetGridRegion(client.Network.Simulators[4].Name, GridLayerType.Objects, out region))
                            {
                                _MapImageID4 = region.MapImageID;
                                client.Assets.RequestImage(_MapImageID4, ImageType.Baked, Assets_OnImageReceived);
                            }

                            if (client.Grid.GetGridRegion(client.Network.Simulators[5].Name, GridLayerType.Objects, out region))
                            {
                                _MapImageID5 = region.MapImageID;
                                client.Assets.RequestImage(_MapImageID5, ImageType.Baked, Assets_OnImageReceived);
                            }

                            if (client.Grid.GetGridRegion(client.Network.Simulators[6].Name, GridLayerType.Objects, out region))
                            {
                                _MapImageID6 = region.MapImageID;
                                client.Assets.RequestImage(_MapImageID6, ImageType.Baked, Assets_OnImageReceived);
                            }

                            if (client.Grid.GetGridRegion(client.Network.Simulators[7].Name, GridLayerType.Objects, out region))
                            {
                                _MapImageID7 = region.MapImageID;
                                client.Assets.RequestImage(_MapImageID7, ImageType.Baked, Assets_OnImageReceived);
                            }

                            if (client.Grid.GetGridRegion(client.Network.Simulators[8].Name, GridLayerType.Objects, out region))
                            {
                                _MapImageID8 = region.MapImageID;
                                client.Assets.RequestImage(_MapImageID8, ImageType.Baked, Assets_OnImageReceived);
                            }

                            if (client.Grid.GetGridRegion(client.Network.Simulators[9].Name, GridLayerType.Objects, out region))
                            {
                                _MapImageID9 = region.MapImageID;
                                client.Assets.RequestImage(_MapImageID9, ImageType.Baked, Assets_OnImageReceived);
                            }

                            //if (client.Grid.GetGridRegion(client.Network.Simulators[10].Name, GridLayerType.Objects, out region))
                            //{
                            //    _MapImageID10 = region.MapImageID;
                            //    client.Assets.RequestImage(_MapImageID10, ImageType.Baked, Assets_OnImageReceived);
                            //}
                        }
                        catch { ; }
                    }
                }
                else
                {
                    if (client.Grid.GetGridRegion(client.Network.CurrentSim.Name, GridLayerType.LandForSale, out region))
                    {
                        _MapImageID = region.MapImageID;
                        client.Assets.RequestImage(_MapImageID, ImageType.Baked, Assets_OnImageReceived);
                    }

                    if (client.Network.Simulators.Count > 1)
                    {
                        try
                        {
                            if (client.Grid.GetGridRegion(client.Network.Simulators[1].Name, GridLayerType.LandForSale, out region))
                            {
                                _MapImageID1 = region.MapImageID;
                                client.Assets.RequestImage(_MapImageID1, ImageType.Baked, Assets_OnImageReceived);
                            }

                            if (client.Grid.GetGridRegion(client.Network.Simulators[2].Name, GridLayerType.LandForSale, out region))
                            {
                                _MapImageID2 = region.MapImageID;
                                client.Assets.RequestImage(_MapImageID2, ImageType.Baked, Assets_OnImageReceived);
                            }

                            if (client.Grid.GetGridRegion(client.Network.Simulators[3].Name, GridLayerType.LandForSale, out region))
                            {
                                _MapImageID3 = region.MapImageID;
                                client.Assets.RequestImage(_MapImageID3, ImageType.Baked, Assets_OnImageReceived);
                            }

                            if (client.Grid.GetGridRegion(client.Network.Simulators[4].Name, GridLayerType.LandForSale, out region))
                            {
                                _MapImageID4 = region.MapImageID;
                                client.Assets.RequestImage(_MapImageID4, ImageType.Baked, Assets_OnImageReceived);
                            }

                            if (client.Grid.GetGridRegion(client.Network.Simulators[5].Name, GridLayerType.LandForSale, out region))
                            {
                                _MapImageID5 = region.MapImageID;
                                client.Assets.RequestImage(_MapImageID5, ImageType.Baked, Assets_OnImageReceived);
                            }

                            if (client.Grid.GetGridRegion(client.Network.Simulators[6].Name, GridLayerType.LandForSale, out region))
                            {
                                _MapImageID6 = region.MapImageID;
                                client.Assets.RequestImage(_MapImageID6, ImageType.Baked, Assets_OnImageReceived);
                            }

                            if (client.Grid.GetGridRegion(client.Network.Simulators[7].Name, GridLayerType.LandForSale, out region))
                            {
                                _MapImageID7 = region.MapImageID;
                                client.Assets.RequestImage(_MapImageID7, ImageType.Baked, Assets_OnImageReceived);
                            }

                            if (client.Grid.GetGridRegion(client.Network.Simulators[8].Name, GridLayerType.LandForSale, out region))
                            {
                                _MapImageID8 = region.MapImageID;
                                client.Assets.RequestImage(_MapImageID8, ImageType.Baked, Assets_OnImageReceived);
                            }

                            if (client.Grid.GetGridRegion(client.Network.Simulators[9].Name, GridLayerType.LandForSale, out region))
                            {
                                _MapImageID9 = region.MapImageID;
                                client.Assets.RequestImage(_MapImageID9, ImageType.Baked, Assets_OnImageReceived);
                            }

                            //if (client.Grid.GetGridRegion(client.Network.Simulators[10].Name, GridLayerType.LandForSale, out region))
                            //{
                            //    _MapImageID10 = region.MapImageID;
                            //    client.Assets.RequestImage(_MapImageID10, ImageType.Baked, Assets_OnImageReceived);
                            //}
                        }
                        catch { ; }
                    }
                }
            }
            else
            {
                //UpdateMiniMap(sim);
                BeginInvoke(new OnUpdateMiniMap(UpdateMiniMap), new object[] { sim });
            }
        }

        private delegate void OnUpdateMiniMap(Simulator sim);
        private void UpdateMiniMap(Simulator sim)
        {
            if (this.InvokeRequired) this.BeginInvoke((MethodInvoker)delegate { UpdateMiniMap(sim); });
            else
            {
                if (sim != client.Network.CurrentSim) return;

                Bitmap bmp = _MapLayer == null ? new Bitmap(256, 256) : (Bitmap)_MapLayer.Clone();

                Graphics g = Graphics.FromImage(bmp);

                if (_MapLayer == null)
                {
                    g.Clear(this.BackColor);
                    g.FillRectangle(Brushes.White, 0f, 0f, 256f, 256f);
                    //label6.Visible = true;
                }
                else
                {
                    //label6.Visible = false;
                }

                if (_LandLayer != null)
                {
                    bmp = _LandLayer == null ? new Bitmap(256, 256) : (Bitmap)_LandLayer.Clone();

                    g = Graphics.FromImage(bmp);

                    if (_MapLayer != null)
                    {
                        g.DrawImage(_MapLayer, new Rectangle(0, 0, _MapLayer.Width, _MapLayer.Height), 0, 0, _MapLayer.Width, _MapLayer.Height, GraphicsUnit.Pixel);   //, ia);
                    }
                }

                // Draw compass points
                StringFormat strFormat = new StringFormat();
                strFormat.Alignment = StringAlignment.Near;

                g.DrawString(sim.Name, new Font("Arial", 8, FontStyle.Bold), Brushes.White, new RectangleF(0, 2, bmp.Width, bmp.Height), strFormat);

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

                world4.Image = bmp;

                g.Dispose();

                string strInfo = string.Format("Total Avatars: {0}", client.Network.CurrentSim.AvatarPositions.Count);
                lblSimData.Text = strInfo;

                strInfo = string.Format("{0}/{1}/{2}/{3}", client.Network.CurrentSim.Name,
                                                                            Math.Round(myPos.X, 0),
                                                                            Math.Round(myPos.Y, 0),
                                                                            Math.Round(myPos.Z, 0));
                label2.Text = "http://slurl.com/secondlife/" + strInfo;

                List<Simulator>sims = client.Network.Simulators;

                int scnt = 0;

                foreach (Simulator nsim in sims)
                {
                    DrawMaps(nsim, myPos, scnt);
                    scnt += 1;
                }
            }
        }

        private void DrawMaps(Simulator sim, Vector3 myPos, int scnt)
        {
            if (this.InvokeRequired) this.BeginInvoke((MethodInvoker)delegate { DrawMaps(sim, myPos, scnt); });
            else
            {
                if (sim == client.Network.CurrentSim) return;

                Image _Maplayertemp = null;
                Image _LandLayertemp = null;

                switch (scnt)
                {
                    case 1:
                        _Maplayertemp = _MapLayer1;
                        _LandLayertemp = _LandLayer1;
                        break;
                    case 2:
                        _Maplayertemp = _MapLayer2;
                        _LandLayertemp = _LandLayer2;
                        break;
                    case 3:
                        _Maplayertemp = _MapLayer3;
                        _LandLayertemp = _LandLayer3;
                        break;
                    case 4:
                        _Maplayertemp = _MapLayer4;
                        _LandLayertemp = _LandLayer4;
                        break;
                    case 5:
                        _Maplayertemp = _MapLayer5;
                        _LandLayertemp = _LandLayer5;
                        break;
                    case 6:
                        _Maplayertemp = _MapLayer6;
                        _LandLayertemp = _LandLayer6;
                        break;
                    case 7:
                        _Maplayertemp = _MapLayer7;
                        _LandLayertemp = _LandLayer7;
                        break;
                    case 8:
                        _Maplayertemp = _MapLayer8;
                        _LandLayertemp = _LandLayer8;
                        break;
                    case 9:
                        _Maplayertemp = _MapLayer9;
                        _LandLayertemp = _LandLayer9;
                        break;
                    case 10:
                        _Maplayertemp = _MapLayer10;
                        _LandLayertemp = _LandLayer10;
                        break;
                }

                Bitmap bmp = _Maplayertemp == null ? new Bitmap(256, 256) : (Bitmap)_Maplayertemp.Clone();

                Graphics g = Graphics.FromImage(bmp);

                if (_Maplayertemp == null)
                {
                    g.Clear(this.BackColor);
                    g.FillRectangle(Brushes.White, 0f, 0f, 256f, 256f);
                    //label6.Visible = true;
                }
                else
                {
                    //label6.Visible = false;
                }

                if (_LandLayertemp != null)
                {
                    bmp = _LandLayertemp == null ? new Bitmap(256, 256) : (Bitmap)_LandLayertemp.Clone();

                    g = Graphics.FromImage(bmp);

                    if (_Maplayertemp != null)
                    {
                        g.DrawImage(_Maplayertemp, new Rectangle(0, 0, _Maplayertemp.Width, _Maplayertemp.Height), 0, 0, _Maplayertemp.Width, _Maplayertemp.Height, GraphicsUnit.Pixel);   //, ia);
                    }
                }

                // Draw compass points
                StringFormat strFormat = new StringFormat();
                strFormat.Alignment = StringAlignment.Near;

                g.DrawString(sim.Name, new Font("Arial", 8, FontStyle.Bold), Brushes.White, new RectangleF(0, 2, bmp.Width, bmp.Height), strFormat);

                if (chkResident.Checked)
                {
                    int i = 0;
                    Rectangle rect = new Rectangle();

                    sim.AvatarPositions.ForEach(
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

                if (clickedx != 0 && clickedy != 0)
                {
                    //PlotSelected(clickedx, clickedy);
                    Rectangle selectedrect = new Rectangle(clickedx - 2, clickedy - 2, 10, 10);
                    g.DrawEllipse(new Pen(Brushes.Red, 2), selectedrect);
                }

                g.DrawImage(bmp, 0, 0);

                switch (scnt)
                {
                    case 1:
                        world5.Image = bmp;
                        break;
                    case 2:
                        world1.Image = bmp;
                        break;
                    case 3:
                        world3.Image = bmp;
                        break;
                    case 4:
                        world7.Image = bmp;
                        break;
                    case 5:
                        world6.Image = bmp;
                        break;
                    case 6:
                        world2.Image = bmp;
                        break;
                    case 7:
                        world8.Image = bmp;
                        break;
                    //case 10:
                    //    world6.Image = bmp;
                    //    break;
                    case 9:
                        world.Image = bmp;
                        break;
                }

                g.Dispose();
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

        private void frmMap_Load(object sender, EventArgs e)
        {
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

        private void chkForSale_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkForSale.Checked)
            {
                _LandLayer = null;
                _MapLayer = null;
                _LandLayer1 = null;
                _MapLayer1 = null;
                _LandLayer2 = null;
                _MapLayer2 = null;
                _LandLayer3 = null;
                _MapLayer3 = null;
                _LandLayer4 = null;
                _MapLayer4 = null;
                _LandLayer5 = null;
                _MapLayer5 = null;
                _LandLayer6 = null;
                _MapLayer6 = null;
                _LandLayer7 = null;
                _MapLayer7 = null;
                _LandLayer8 = null;
                _MapLayer8 = null;
                _LandLayer9 = null;
                _MapLayer9 = null;
                _LandLayer10 = null;
                _MapLayer10 = null;

                client.Grid.RequestMapRegion(client.Network.CurrentSim.Name, GridLayerType.LandForSale);

                if (client.Network.Simulators.Count > 1)
                {
                    try
                    {
                        client.Grid.RequestMapRegion(client.Network.Simulators[1].Name, GridLayerType.LandForSale);
                        client.Grid.RequestMapRegion(client.Network.Simulators[2].Name, GridLayerType.LandForSale);
                        client.Grid.RequestMapRegion(client.Network.Simulators[3].Name, GridLayerType.LandForSale);
                        client.Grid.RequestMapRegion(client.Network.Simulators[4].Name, GridLayerType.LandForSale);
                        client.Grid.RequestMapRegion(client.Network.Simulators[5].Name, GridLayerType.LandForSale);
                        client.Grid.RequestMapRegion(client.Network.Simulators[6].Name, GridLayerType.LandForSale);
                        client.Grid.RequestMapRegion(client.Network.Simulators[7].Name, GridLayerType.LandForSale);
                        client.Grid.RequestMapRegion(client.Network.Simulators[8].Name, GridLayerType.LandForSale);
                        client.Grid.RequestMapRegion(client.Network.Simulators[9].Name, GridLayerType.LandForSale);
                        //client.Grid.RequestMapRegion(client.Network.Simulators[10].Name, GridLayerType.LandForSale);
                    }
                    catch { ; }
                }
            }
            else
            {
                _LandLayer = _MapLayer;
                _MapLayer = null;
                _LandLayer1 = _MapLayer1;
                _MapLayer1 = null;
                _LandLayer2 = _MapLayer2;
                _MapLayer2 = null;
                _LandLayer3 = _MapLayer3;
                _MapLayer3 = null;
                _LandLayer4 = _MapLayer4;
                _MapLayer4 = null;
                _LandLayer5 = _MapLayer5;
                _MapLayer5 = null;
                _LandLayer6 = _MapLayer6;
                _MapLayer6 = null;
                _LandLayer7 = _MapLayer7;
                _MapLayer7 = null;
                _LandLayer8 = _MapLayer8;
                _MapLayer8 = null;
                _LandLayer9 = _MapLayer9;
                _MapLayer9 = null;
                _LandLayer10 = _MapLayer10;
                _MapLayer10 = null;

                client.Grid.RequestMapRegion(client.Network.CurrentSim.Name, GridLayerType.Objects);

                if (client.Network.Simulators.Count > 1)
                {
                    try
                    {
                        client.Grid.RequestMapRegion(client.Network.Simulators[1].Name, GridLayerType.Objects);
                        client.Grid.RequestMapRegion(client.Network.Simulators[2].Name, GridLayerType.Objects);
                        client.Grid.RequestMapRegion(client.Network.Simulators[3].Name, GridLayerType.Objects);
                        client.Grid.RequestMapRegion(client.Network.Simulators[4].Name, GridLayerType.Objects);
                        client.Grid.RequestMapRegion(client.Network.Simulators[5].Name, GridLayerType.Objects);
                        client.Grid.RequestMapRegion(client.Network.Simulators[6].Name, GridLayerType.Objects);
                        client.Grid.RequestMapRegion(client.Network.Simulators[7].Name, GridLayerType.Objects);
                        client.Grid.RequestMapRegion(client.Network.Simulators[8].Name, GridLayerType.Objects);
                        client.Grid.RequestMapRegion(client.Network.Simulators[9].Name, GridLayerType.Objects);
                        //client.Grid.RequestMapRegion(client.Network.Simulators[10].Name, GridLayerType.Objects);
                    }
                    catch { ; }
                }
            }

            GetMap();
        }

        private void frmMap_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Grid.CoarseLocationUpdate -= new EventHandler<CoarseLocationUpdateEventArgs>(Grid_OnCoarseLocationUpdate);
            client.Network.SimChanged -= new EventHandler<SimChangedEventArgs>(Network_OnCurrentSimChanged);
        }
    }
}
