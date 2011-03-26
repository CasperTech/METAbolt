using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
//using SLNetworkComm;
using libsecondlife;
using libsecondlife.Packets;

namespace METAbolt
{
    public partial class frmBots : Form
    {
        //private SLNetCom netcom;
        private SecondLife client;
        private METAboltInstance instance;
        public Dictionary<LLUUID, AvatarAppearancePacket> Appearances = new Dictionary<LLUUID, AvatarAppearancePacket>();

        public frmBots(METAboltInstance instance)
        {
            InitializeComponent();

            this.instance = instance;
            client = this.instance.Client;
            //netcom = this.instance.Netcom;
            //netcom.NetcomSync = this;

            //client.Settings.DEBUG = true;
            //client.Settings.LOG_RESENDS = false;
            //client.Settings.SEND_AGENT_UPDATES = true;
            //client.Settings.USE_TEXTURE_CACHE = true;

            //client.Network.RegisterCallback(PacketType.AvatarAppearance, new NetworkManager.PacketCallback(AvatarAppearanceHandler));
            //PacketType.AvatarAppearance += new NetworkManager.PacketCallback(AvatarAppearanceHandler);

            

            //Console.WriteLine("setting " + currentGroup.Name + " as active group");
            //Client.Groups.ActivateGroup(currentGroup.ID);
            //GroupsEvent.WaitOne(30000, false);

            //Client.Network.UnregisterCallback(PacketType.AgentDataUpdate, pcallback);
        }

        //private void AvatarAppearanceHandler(Packet packet, Simulator simulator)
        //{
        //    AvatarAppearancePacket appearance = (AvatarAppearancePacket)packet;

        //    lock (Appearances) Appearances[appearance.Sender.ID] = appearance;
        //}

        private void AvatarAppearanceHandler(Packet packet, Simulator simulator)
        {
            AvatarAppearancePacket appearance = (AvatarAppearancePacket)packet;

            lock (Appearances) Appearances[appearance.Sender.ID] = appearance;

            LLObject.TextureEntry te = new LLObject.TextureEntry(appearance.ObjectData.TextureEntry, 0,
                appearance.ObjectData.TextureEntry.Length);

            if (IsNullOrZero(te.FaceTextures[(int)AppearanceManager.TextureIndex.EyesBaked]) &&
                IsNullOrZero(te.FaceTextures[(int)AppearanceManager.TextureIndex.HeadBaked]) &&
                IsNullOrZero(te.FaceTextures[(int)AppearanceManager.TextureIndex.LowerBaked]) &&
                IsNullOrZero(te.FaceTextures[(int)AppearanceManager.TextureIndex.SkirtBaked]) &&
                IsNullOrZero(te.FaceTextures[(int)AppearanceManager.TextureIndex.UpperBaked]))
            {
                listBox1.Items.Add(appearance.Sender.ID.ToString());
                //Console.WriteLine("Avatar " + appearance.Sender.ID.ToString() + " may be a bot");
            }
        }

        private bool IsNullOrZero(LLObject.TextureEntryFace face)
        {
            return (face == null || face.TextureID == LLUUID.Zero);
        }

        private void frmBots_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void frmBots_FormClosing(object sender, FormClosingEventArgs e)
        {
            listBox1.Items.Clear();
            //client.Network.UnregisterCallback(PacketType.AvatarAppearance, pcallback);
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            client.Settings.DEBUG = true;
            client.Settings.LOG_RESENDS = false;
            client.Settings.SEND_AGENT_UPDATES = true;
            client.Settings.USE_TEXTURE_CACHE = true;

            NetworkManager.PacketCallback pcallback = new NetworkManager.PacketCallback(AvatarAppearanceHandler);
            client.Network.RegisterCallback(PacketType.AvatarAppearance, pcallback);
            //client.Network.RegisterCallback(PacketType.Default, pcallback);

            //System.Threading.Thread.Sleep(100);

            //client.Network.UnregisterCallback(PacketType.Default, pcallback);
        }
    }
}
