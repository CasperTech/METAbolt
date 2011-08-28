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
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
using OpenMetaverse.Packets;
//using SLNetworkComm;
using OpenMetaverse.Assets;

namespace METAbolt
{
    public class RegionSearchResultItem
    {
        private METAboltInstance instance;
        //private SLNetCom netcom;
        private GridClient client;

        private GridRegion region;
        private System.Drawing.Image mapImage;

        private ListBox listBox;
        private int listIndex;

        private bool imageDownloading = false;
        private bool imageDownloaded = false;

        private bool gettingAgentCount = false;
        private bool gotAgentCount = false;
        private BackgroundWorker agentCountWorker;
        private List<MapItem> agentlocations = new List<MapItem>();

        public RegionSearchResultItem(METAboltInstance instance, GridRegion region, ListBox listBox)
        {
            this.instance = instance;
            //netcom = this.instance.Netcom;
            client = this.instance.Client;
            this.region = region;
            this.listBox = listBox;

            agentCountWorker = new BackgroundWorker();
            agentCountWorker.DoWork += new DoWorkEventHandler(agentCountWorker_DoWork);
            agentCountWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(agentCountWorker_RunWorkerCompleted);

            AddClientEvents();
        }

        private void agentCountWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<OpenMetaverse.MapItem> items =
                client.Grid.MapItems(
                    region.RegionHandle,
                    OpenMetaverse.GridItemType.AgentLocations,
                    GridLayerType.Terrain, 500);

            if (items != null)
            {
                e.Result = (byte)items.Count;
                agentlocations = items; 
            }
        }

        private void agentCountWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            gettingAgentCount = false;
            gotAgentCount = true;

            if (e.Result != null)
            {
                region.Agents = (byte)e.Result;
            }

            RefreshListBox();
        }

        private void AddClientEvents()
        {
            //client.Assets.OnImageReceived += new AssetManager.ImageReceivedCallback(Assets_OnImageReceived);
        }

        //Separate thread
        private void Assets_OnImageReceived(TextureRequestState image, AssetTexture texture)
        {
            if (texture.AssetID != region.MapImageID) return;
            if (texture.AssetData == null) return;

            mapImage = ImageHelper.Decode(texture.AssetData);
            if (mapImage == null) return;

            instance.ImageCache.AddImage(texture.AssetID, mapImage);

            imageDownloading = false;
            imageDownloaded = true;
            listBox.BeginInvoke(new MethodInvoker(RefreshListBox));
            listBox.BeginInvoke(new OnMapImageRaise(OnMapImageDownloaded), new object[] { EventArgs.Empty });
        }

        //UI thread
        private void RefreshListBox()
        {
            listBox.Refresh();
        }

        public void RequestMapImage(float priority)
        {
            if (region.MapImageID == UUID.Zero || region.MapImageID == null)
            {
                imageDownloaded = true;
                OnMapImageDownloaded(EventArgs.Empty);
                return;
            }

            if (instance.ImageCache.ContainsImage(region.MapImageID))
            {
                mapImage = instance.ImageCache.GetImage(region.MapImageID);
                imageDownloaded = true;
                OnMapImageDownloaded(EventArgs.Empty);
                RefreshListBox();
            }
            else
            {
                client.Assets.RequestImage(region.MapImageID, ImageType.Normal, Assets_OnImageReceived); //, priority, 0);
                imageDownloading = true;
            }
        }

        public void RequestAgentLocations()
        {
            gettingAgentCount = true;
            agentCountWorker.RunWorkerAsync();
        }

        public override string ToString()
        {
            return region.Name;
        }

        public event EventHandler MapImageDownloaded;
        private delegate void OnMapImageRaise(EventArgs e);
        protected virtual void OnMapImageDownloaded(EventArgs e)
        {
            if (MapImageDownloaded != null) MapImageDownloaded(this, e);
        }

        public GridRegion Region
        {
            get { return region; }
        }

        public System.Drawing.Image MapImage
        {
            get { return mapImage; }
        }

        public bool IsImageDownloaded
        {
            get { return imageDownloaded; }
        }

        public bool IsImageDownloading
        {
            get { return imageDownloading; }
        }

        public bool GettingAgentCount
        {
            get { return gettingAgentCount; }
        }

        public bool GotAgentCount
        {
            get { return gotAgentCount; }
        }

        public int ListIndex
        {
            get { return listIndex; }
            set { listIndex = value; }
        }

        public List<MapItem> AgentLocations
        {
            get { return agentlocations; }
        }
    }
}
