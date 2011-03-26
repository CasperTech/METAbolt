using System;
using System.ComponentModel;
using System.Data;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
using System.Net;
using System.Timers;
using System.Diagnostics;
using SLNetworkComm;

namespace METAbolt
{
    public class MinimapInterface : Interface
    {
        private METAboltInstance instance;
        private GridClient client;
        private SLNetCom netcom;

        //A magic number to calculate index sim y coord from actual coord
        private const int GRID_Y_OFFSET = 1279;
        //Base URL for web map api sim images
        private const String MAP_IMG_URL = "http://secondlife.com/apps/mapapi/grid/map_image/";
        private PictureBox world = new PictureBox();
        private Button cmdRefresh = new Button();
        //private Timer mTimer = new Timer();
        //private System.Timers.Timer _timer = null;
        private Label lblSimData = new Label();
        private Label label1 = new Label();
        private TextBox label2 = new TextBox();
        private NumericUpDown nuX = new NumericUpDown();
        private NumericUpDown nuY = new NumericUpDown();
        private NumericUpDown nuZ = new NumericUpDown();
        private Button cmdTP = new Button();
        private System.Drawing.Image mMapImage = null;
        private string oldSim = String.Empty;
        int px = 128;
        int py = 128;

        public MinimapInterface(METAboltInstance instance, frmMapClient mapClient)
        {
            this.instance = instance;
            netcom = this.instance.Netcom;
            client = this.instance.Client;

            //Name = client.Network.CurrentSim.Name; // "Minimap";
            //Description = "Current SIM";

            //client.Grid.OnGridItems += new GridManager.GridItemsCallback(Grid_OnGridItems);
            //client.Grid.RequestMapItems(client.Network.CurrentSim.Handle, OpenMetaverse.GridItemType.Telehub, GridLayerType.Terrain);

            client.Grid.OnCoarseLocationUpdate += new GridManager.CoarseLocationUpdateCallback(Grid_OnCoarseLocationUpdate);
            client.Network.OnCurrentSimChanged += new NetworkManager.CurrentSimChangedCallback(Network_OnCurrentSimChanged);

            printMap();

            //client.Grid.RequestMapItems(client.Network.CurrentSim.Handle, OpenMetaverse.GridItemType.AgentLocations,OpenMetaverse.GridLayer  );
        }

        private void Network_OnCurrentSimChanged(Simulator PreviousSimulator)
        {
            Name = client.Network.CurrentSim.Name;
            printMap();
        }

        private void Grid_OnCoarseLocationUpdate(Simulator sim)
        {
            Name = client.Network.CurrentSim.Name;
            printMap();
        }


        //private void Grid_OnGridItems(OpenMetaverse.GridItemType stype, List<OpenMetaverse.GridItem> sitems)
        //{
        //    if (stype == OpenMetaverse.GridItemType.AgentLocations)
        //    {
        //        printMap();
        //    }


        //    // TODO
        //    // put these in an plot the locations at some stage
        //    //OpenMetaverse.GridItemType.LandForSale;
        //    //OpenMetaverse.GridItemType.Telehub;
        //    //OpenMetaverse.GridItemType.Popular;

        //    //OpenMetaverse.GridItem stm;
        //    //stm = sitems[0];
        //}
        
        private void map_onclick(object sender, System.EventArgs e)
        {
            // do nothing;
        }
        
        private void cmdRefresh_onclick(object sender, System.EventArgs e)
        {
            printMap();

            cmdTP.Enabled = false;

            nuX.Value = (decimal)Math.Round(client.Self.SimPosition.X, 0);
            nuY.Value = (decimal)Math.Round(client.Self.SimPosition.Y, 0);
            nuZ.Value = (decimal)Math.Round(client.Self.SimPosition.Z, 0);
        }

        private void cmdTP_onclick(object sender, System.EventArgs e)
        {
            try
            {
                netcom.Teleport(client.Network.CurrentSim.Name, new Vector3((float)nuX.Value, (float)nuY.Value, (float)nuY.Value));
            }

            catch (Exception exp)
            {
                MessageBox.Show("An error occured while Teleporting. \n Please re-try later.","METAbolt");  
            }
        }

        private void PlotAvs()
        {
            if (client.Network.CurrentSim.ObjectsAvatars.Count > 0)
            {
                client.Network.CurrentSim.ObjectsAvatars.ForEach(delegate(Avatar avatar)
                {
                    Vector3 apos = avatar.Position;
                    string spos = apos.X.ToString() + "," + apos.Y.ToString() + "," + apos.Z.ToString();
                });
            }
        }

        public void printMap()
        {
            Bitmap map      = new Bitmap(256, 256, PixelFormat.Format32bppRgb);
            Font font       = new Font("Tahoma", 8, FontStyle.Bold);
            Pen mAvPen = new Pen(Brushes.Green, 2);    //new Pen(Brushes.GreenYellow, 1);
            Brush mAvBrush = new SolidBrush(Color.LightGreen);

            //Pen mAvPen1 = new Pen(Brushes.AntiqueWhite, 1);
            //Brush mAvBrush1 = new SolidBrush(Color.White);

            String strInfo  = String.Empty;

            // Get new background map if necessary
            if (mMapImage == null || oldSim != client.Network.CurrentSim.Name)
            {
                oldSim = client.Network.CurrentSim.Name;
                mMapImage = DownloadWebMapImage();
            }

            // Create in memory bitmap   
            using (Graphics g = Graphics.FromImage(map))
            {
                if (mMapImage == null)
                {
                    MessageBox.Show("Error downloading map image from SL. \n Try again later.", "METAbolt");
                    return;
                }

                // Draw background map
                g.DrawImage(mMapImage, new Rectangle(0, 0, 256, 256), 0, 0, 256, 256, GraphicsUnit.Pixel);

                // Draw all avatars
                client.Network.CurrentSim.AvatarPositions.ForEach(
                    delegate(Vector3 pos)
                    { 
                        Rectangle rect = new Rectangle((int)Math.Round(pos.X, 0) - 2, 255 - ((int)Math.Round(pos.Y, 0) - 2), 6, 6);
                        g.FillEllipse(mAvBrush, rect);
                        g.DrawEllipse(mAvPen, rect);
                    }
                );

                // Draw self ;)
                Rectangle myrect = new Rectangle((int)Math.Round(client.Self.SimPosition.X, 0) - 2, 255 - ((int)Math.Round(client.Self.SimPosition.Y, 0) - 2), 6, 6);
                g.FillEllipse(new SolidBrush(Color.Yellow), myrect);
                g.DrawEllipse(new Pen(Brushes.Red, 3), myrect);

                // Draw region info
                strInfo = string.Format("{0}/{1}/{2}/{3} - Ttl Avatars: {4}", client.Network.CurrentSim.Name,
                                                                            Math.Round(client.Self.SimPosition.X, 0),
                                                                            Math.Round(client.Self.SimPosition.Y, 0),
                                                                            Math.Round(client.Self.SimPosition.Z, 0),
                                                                            client.Network.CurrentSim.AvatarPositions.Count);
                lblSimData.Text = "Coords: " + strInfo;

                strInfo = string.Format("{0}/{1}/{2}/{3}", client.Network.CurrentSim.Name,
                                                                            Math.Round(client.Self.SimPosition.X, 0),
                                                                            Math.Round(client.Self.SimPosition.Y, 0),
                                                                            Math.Round(client.Self.SimPosition.Z, 0));
                label2.Text = "SLURL: http://slurl.com/secondlife/" + strInfo; ;
            }

            // update picture box with new map bitmap
            world.BackgroundImage = map;
            world.Cursor = Cursors.Cross;   
        }

        private void map_MouseUp(object sender, MouseEventArgs e)
        {

            px = e.X;
            py = 255 - e.Y;

            nuX.Value = (decimal)px;
            nuY.Value = (decimal)py;
            nuZ.Value = (decimal)20;  
            
            //label2.Text = px.ToString() + " - " + py.ToString() + " - 20";

            PlotSeleted(e.X, e.Y); 

            cmdTP.Enabled = true; 
        }

        private void PlotSeleted(int x, int y)
        {
            Bitmap map = new Bitmap(256, 256, PixelFormat.Format32bppRgb);
            Font font = new Font("Tahoma", 8, FontStyle.Bold);
            Pen mAvPen = new Pen(Brushes.Black, 2);    //new Pen(Brushes.GreenYellow, 1);
            Brush mAvBrush = new SolidBrush(Color.LightGreen);

            String strInfo = String.Empty;

            // Get new background map if necessary
            if (mMapImage == null || oldSim != client.Network.CurrentSim.Name)
            {
                oldSim = client.Network.CurrentSim.Name;
                mMapImage = DownloadWebMapImage();
            }

            // Create in memory bitmap   
            using (Graphics g = Graphics.FromImage(map))
            {
                // Draw background map
                g.DrawImage(mMapImage, new Rectangle(0, 0, 256, 256), 0, 0, 256, 256, GraphicsUnit.Pixel);

                // Draw all avatars
                client.Network.CurrentSim.AvatarPositions.ForEach(
                    delegate(Vector3 pos)
                    {
                        Rectangle rect = new Rectangle((int)Math.Round(pos.X, 0) - 2, 255 - ((int)Math.Round(pos.Y, 0) - 2), 4, 4);
                        g.FillEllipse(mAvBrush, rect);
                        g.DrawEllipse(mAvPen, rect);
                    }
                );


                // Draw self ;)
                Rectangle myrect = new Rectangle((int)Math.Round(client.Self.SimPosition.X, 0) - 2, 255 - ((int)Math.Round(client.Self.SimPosition.Y, 0) - 2), 6, 6);
                g.FillEllipse(new SolidBrush(Color.Yellow), myrect);
                g.DrawEllipse(new Pen(Brushes.Red, 3), myrect);

                // Draw selected location
                //Rectangle selectedrect = new Rectangle(x, y, 6, 6);
                Rectangle selectedrect = new Rectangle(x - 2, y - 2, 10, 10);
                //g.FillEllipse(mAvBrush, rect);
                g.DrawEllipse(new Pen(Brushes.Red, 2), selectedrect);

                // Draw region info
                strInfo = string.Format("{0}/{1}/{2}/{3} - Ttl Avatars: {4}", client.Network.CurrentSim.Name,
                                                                            Math.Round(client.Self.SimPosition.X, 0),
                                                                            Math.Round(client.Self.SimPosition.Y, 0),
                                                                            Math.Round(client.Self.SimPosition.Z, 0),
                                                                            client.Network.CurrentSim.AvatarPositions.Count);

                lblSimData.Text = "Coords: " + strInfo;

                strInfo = string.Format("{0}/{1}/{2}/{3}", client.Network.CurrentSim.Name,
                                                                            Math.Round(client.Self.SimPosition.X, 0),
                                                                            Math.Round(client.Self.SimPosition.Y, 0),
                                                                            Math.Round(client.Self.SimPosition.Z, 0));
                label2.Text = "SLURL: http://slurl.com/secondlife/" + strInfo; ;
            }

            // update picture box with new map bitmap
            world.BackgroundImage = map;
        }

        public override void Initialize()
        {
            ((System.ComponentModel.ISupportInitialize)(world)).BeginInit();
            world.BorderStyle   = System.Windows.Forms.BorderStyle.FixedSingle;
            world.Size          = new System.Drawing.Size(256, 256);
            world.Visible       = true;
            world.Click         += new System.EventHandler(this.map_onclick);
            world.MouseUp       += new MouseEventHandler(this.map_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(world)).EndInit();

            //((System.ComponentModel.ISupportInitialize)(cmdRefresh)).BeginInit();
            cmdRefresh.Text     = "Refresh";
            cmdRefresh.Size     = new System.Drawing.Size(80, 24);
            cmdRefresh.Left     = world.Left + world.Width + 10;
            cmdRefresh.Click    += new System.EventHandler(this.cmdRefresh_onclick);
            cmdRefresh.Visible  = true;

            //mTimer.Interval     = 60000;
            //mTimer.Tick         += new System.EventHandler(this.mTimer_Tick);
            //mTimer.Enabled      = true;
            ////mTimer.Start;

            //_timer = new System.Timers.Timer(60000);
            //_timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
            //_timer.Start();

            lblSimData.AutoSize = true; 
            //lblSimData.Size     = new System.Drawing.Size(300, 24);
            lblSimData.Top      = world.Top + world.Height + 5;
            lblSimData.Visible  = true;

            label1.AutoSize = true;  
            //label1.Size     = new System.Drawing.Size(300, 24);
            label1.Left     = world.Left + world.Width + 10;
            label1.Top      = cmdRefresh.Top + cmdRefresh.Height + 50;
            label1.Text     = "Selected position:"; 
            label1.Visible  = true;

            nuX.Size        = new System.Drawing.Size(50, 24); 
            nuX.Left        = world.Left + world.Width + 10;
            nuX.Top         = label1.Top + label1.Height + 5;
            nuX.Minimum     = 0;
            nuX.Maximum     = 256;
            nuX.Value       = (decimal)Math.Round(client.Self.SimPosition.X, 0);
            nuX.Visible     = true;

            nuY.Size        = new System.Drawing.Size(50, 24);
            nuY.Left        = nuX.Left + nuX.Width + 5;
            nuY.Top         = label1.Top + label1.Height + 5;
            nuY.Minimum     = 0;
            nuY.Maximum     = 256;
            nuY.Value       = (decimal)Math.Round(client.Self.SimPosition.Y, 0);
            nuY.Visible     = true;

            nuZ.Size        = new System.Drawing.Size(50, 24);
            nuZ.AutoSize    = true;  
            nuZ.Left        = nuY.Left + nuY.Width + 5;
            nuZ.Top         = label1.Top + label1.Height + 5;
            nuZ.Minimum     = 0;
            nuZ.Maximum     = 10000;
            nuZ.Value       = (decimal)Math.Round(client.Self.SimPosition.Z, 0);
            nuZ.Visible     = true; 

            //label2.au = true;
            //label2.Left     = world.Left + world.Width + 40;
            label2.Size     = new System.Drawing.Size(350, 20);
            label2.Top      = lblSimData.Top + lblSimData.Height;
            label2.BorderStyle = BorderStyle.None;
            label2.ReadOnly = true;  
            //label2.Text     = "";
            label2.Visible  = true;

            cmdTP.Text      = "Teleport";
            cmdTP.Size      = new System.Drawing.Size(80, 24);
            cmdTP.Left      = world.Left + world.Width + 10;
            //cmdTP.Top       = world.Height - cmdTP.Height;   //label2.Top + label2.Height + 20;
            cmdTP.Top       = nuX.Top + nuX.Height + 5; 
            cmdTP.Click     += new System.EventHandler(this.cmdTP_onclick);
            cmdTP.Enabled   = false;  
            cmdTP.Visible   = true;


            
            //((System.ComponentModel.ISupportInitialize)(world)).EndInit();

            TabPage.Controls.Add(world);
            TabPage.Controls.Add(cmdRefresh);
            TabPage.Controls.Add(lblSimData);
            TabPage.Controls.Add(label1);
            TabPage.Controls.Add(label2);
            TabPage.Controls.Add(nuX);
            TabPage.Controls.Add(nuY);
            TabPage.Controls.Add(nuZ);
            TabPage.Controls.Add(cmdTP);
        }

        // Ripped from "Terrain Scultor" by Cadroe with minors changes
        // http://spinmass.blogspot.com/2007/08/terrain-sculptor-maps-sims-and-creates.html
        private System.Drawing.Image DownloadWebMapImage()
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            String imgURL = "";
            GridRegion currRegion;

            client.Grid.GetGridRegion(client.Network.CurrentSim.Name, GridLayerType.Terrain, out currRegion);

            try
            {
                //Form the URL using the sim coordinates
                imgURL = MAP_IMG_URL + currRegion.X.ToString() + "-" +
                        (GRID_Y_OFFSET - currRegion.Y).ToString() + "-1-0.jpg";
                //Make the http request
                request = (HttpWebRequest)HttpWebRequest.Create(imgURL);
                request.Timeout = 5000;
                request.ReadWriteTimeout = 20000;
                response = (HttpWebResponse)request.GetResponse();

                return System.Drawing.Image.FromStream(response.GetResponseStream());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Downloading Web Map Image from SL","METAbolt");
                return null;
            }
        }

        public override void Paint(object sender, PaintEventArgs e)
        {
            //;
        }

        protected void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //printMap();
        }
    }
}
