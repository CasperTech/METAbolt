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
using SLNetworkComm;
using OpenMetaverse;
using Khendys.Controls;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using System.Drawing.Drawing2D;
using GoogleTranslationUtils;
using OpenMetaverse.Imaging;
using OpenMetaverse.Assets;
using System.Drawing.Imaging;
using MD5library;
using System.Timers;
using ExceptionReporting;
using System.Threading;
using System.Linq;
using OpenMetaverse.Utilities;
using OpenMetaverse.Voice;
using System.Globalization;
using PopupControl;


namespace METAbolt
{
    public partial class ChatConsole : UserControl
    {

        private METAboltInstance instance;
        private SLNetCom netcom;
        private GridClient client;
        private ChatTextManager chatManager;
        private TabsConsole tabConsole;
        private int previousChannel = 0;
        private double ahead = 0.0;
        private bool flying = false;
        private bool sayopen = false;
        private bool saveopen = false;
        private UUID _MapImageID;
        private Image _MapLayer;
        private int px = 0;
        private int py =0;
        private Simulator sim;
        private Rectangle rect;
        private bool move = false;
        private string selectedname = string.Empty;
        private Vector3 vDir;
        private string clickedurl = string.Empty;
        private bool avrezzed = false;
        private bool pasted = false;
        private uint[] localids;
        private int newsize = 140;
        private bool listnerdisposed = true;
        private System.Timers.Timer sitTimer;
        private bool showing = false;
        private UUID avuuid = UUID.Zero;
        private string avname = string.Empty;
        //private bool removead = false;
        private ExceptionReporter reporter = new ExceptionReporter();
        private SafeDictionary<uint, Avatar> sfavatar = new SafeDictionary<uint,Avatar>();
        private List<string> avtyping = new List<string>();
        private int start = 0;
        private int indexOfSearchText = 0;
        private string prevsearchtxt = string.Empty;
        //private bool voiceon = false;

        //static AutoResetEvent ParcelVoiceInfoEvent = new AutoResetEvent(false);
        //static AutoResetEvent ProvisionEvent = new AutoResetEvent(false);
        //static string VoiceAccount = String.Empty;
        //static string VoicePassword = String.Empty;
        //static string VoiceRegionName = String.Empty;
        //static int VoiceLocalID = 0;
        //static string VoiceChannelURI = String.Empty;
        //private VoiceManager voice = null;
        private VoiceGateway vgate = null;
        //List<string> mics;
        //List<string> speakers;
        private Popup toolTip;
        private CustomToolTip customToolTip;
        private bool formloading = false;


        internal class ThreadExceptionHandler
        {
            public void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
            {
                ExceptionReporter reporter = new ExceptionReporter();
                reporter.Show(e.Exception);
            }
        }

        public ChatConsole(METAboltInstance instance)
        {
            InitializeComponent();
            Disposed += new EventHandler(ChatConsole_Disposed);

            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.instance = instance;
            netcom = this.instance.Netcom;
            client = this.instance.Client;

            AddNetcomEvents();
            AddClientEvents();

            chatManager = new ChatTextManager(instance, new RichTextBoxPrinter(instance, rtbChat));
            chatManager.PrintStartupMessage();

            this.instance.MainForm.Load += new EventHandler(MainForm_Load);
            
            this.instance.Config.ConfigApplied += new EventHandler<ConfigAppliedEventArgs>(Config_ConfigApplied);
            ApplyConfig(this.instance.Config.CurrentConfig);

            client.Avatars.UUIDNameReply += new EventHandler<UUIDNameReplyEventArgs>(Avatars_OnAvatarNames);
            client.Objects.ObjectProperties += new EventHandler<ObjectPropertiesEventArgs>(Objects_OnObjectProperties);

            CreateSmileys();
            AddLanguages();

            //client.Appearance.OnAppearanceUpdated += new AppearanceManager.AppearanceUpdatedCallback(Appearance_OnAppearanceUpdated);
            client.Appearance.AppearanceSet += new EventHandler<AppearanceSetEventArgs>(Appearance_OnAppearanceSet);
            client.Parcels.ParcelDwellReply += new EventHandler<ParcelDwellReplyEventArgs>(Parcels_OnParcelDwell);
            netcom.TeleportStatusChanged += new EventHandler<TeleportEventArgs>(netcom_TeleportStatusChanged);
            client.Self.AvatarSitResponse += new EventHandler<AvatarSitResponseEventArgs>(Self_AvatarSitResponse);

            lvwRadar.ListViewItemSorter = new RadarSorter();

            sim = client.Network.CurrentSim;

            world.Cursor = Cursors.NoMove2D;

            timer2.Enabled = true;
            timer2.Start();

            string msg1 = "Yellow dot with red border = your avatar \nGreen dots = avs at your altitude\nRed squares = avs 20m+ below you\nBlue squares = avs 20m+ above you\n[Hover mouse on avatar icons for info. Click for profile]";
            toolTip = new Popup(customToolTip = new CustomToolTip(instance, msg1));
            toolTip.AutoClose = false;
            toolTip.FocusOnOpen = false;
            toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;
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

        ////private void Appearance_OnAppearanceUpdated(Primitive.TextureEntry te)
        ////{
        ////    //int wcnt = client.Appearance.Wearables.Count;

        ////    //try
        ////    //{
        ////    //    BeginInvoke(new MethodInvoker(delegate()
        ////    //    {
        ////    //        chatManager.PrintAlertMessage("Total number of wearables found: " + wcnt.ToString());
        ////    //        CheckWearables();
        ////    //    }));
        ////    //}
        ////    //catch (Exception ex)
        ////    //{

        ////    //}
        ////}

        private void Avatars_OnAvatarNames(object sender, UUIDNameReplyEventArgs names)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    Avatars_OnAvatarNames(sender, names);
                }));

                return;
            }

            //lock (instance.avnames)
            //{
                foreach (KeyValuePair<UUID, string> av in names.Names)
                {
                    if (!instance.avnames.ContainsKey(av.Key))
                    {
                        instance.avnames.Add(av.Key, av.Value);
                    }
                }

            //    if (instance.avlocations.Contains(  
            //client.Network.CurrentSim.ObjectsAvatars.
            //}
        }

        private void netcom_TeleportStatusChanged(object sender, TeleportEventArgs e)
        {
            try
            {
                //evs.OnTeleportStatusChanged(e); 

                switch (e.Status)
                {
                    case TeleportStatus.Start:
                        break;
                    case TeleportStatus.Progress:
                        break;

                    case TeleportStatus.Failed:
                        //MessageBox.Show(e.Message, "Teleport", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;

                    case TeleportStatus.Finished: 
                        if (instance.Config.CurrentConfig.AutoSit)
                        {
                            Logger.Log("AUTOSIT: Initialising...", Helpers.LogLevel.Info);

                            sitTimer = new System.Timers.Timer();
                            sitTimer.Interval = 61000;
                            sitTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                            sitTimer.Enabled = true;
                            sitTimer.Start();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                reporter.Show(ex);
            }
        }

        void ChatConsole_Disposed(object sender, EventArgs e)
        {
            this.instance.Config.ConfigApplied -= new EventHandler<ConfigAppliedEventArgs>(Config_ConfigApplied);
            client.Objects.ObjectProperties -= new EventHandler<ObjectPropertiesEventArgs>(Objects_OnObjectProperties);
            client.Appearance.AppearanceSet -= new EventHandler<AppearanceSetEventArgs>(Appearance_OnAppearanceSet);
            client.Parcels.ParcelDwellReply -= new EventHandler<ParcelDwellReplyEventArgs>(Parcels_OnParcelDwell);
            client.Avatars.UUIDNameReply -= new EventHandler<UUIDNameReplyEventArgs>(Avatars_OnAvatarNames);

            RemoveEvents();
            chatManager.Dispose();
        }

        private void Appearance_OnAppearanceSet(object sender, AppearanceSetEventArgs e)
        {
            string rmsg = string.Empty;

            if (avrezzed) return;

            rmsg = " Avatar has rezzed. ";
            avrezzed = true;
  
            //if (e.Success)
            //{
            //    rmsg = " Avatar has rezzed. ";
            //    avrezzed = true; 
            //}
            //else
            //{
            //    rmsg = " Avatar has not rezzed as expected. ";
            //}

            try
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    chatManager.PrintAlertMessage(rmsg);
                }));
            }
            catch { ; }

            if (instance.Config.CurrentConfig.AutoSit)
            {
                if (!instance.State.IsSitting)
                {
                    Logger.Log("AUTOSIT: Initialising...", Helpers.LogLevel.Info);

                    sitTimer = new System.Timers.Timer();
                    sitTimer.Interval = 61000;
                    sitTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                    sitTimer.Enabled = true;
                    sitTimer.Start();
                }
            }

            try
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    CheckWearables();
                    CheckLocation();

                    client.Appearance.AppearanceSet -= new EventHandler<AppearanceSetEventArgs>(Appearance_OnAppearanceSet);
                }));
            }
            catch { ; }
        }

        private void CheckLocation()
        {
            // The land does not tie up with the location coords
            // not sure if this is an SL bug as at SIM V 1.40 (20/07/2010) or libopenmv bug
            // below is a work around and I beleive it should remain
            // permanently as a safeguard

            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    CheckLocation();
                }));

                return;
            }

            try
            {
                Vector3 apos = client.Self.SimPosition;

                float f1 = 64.0f * (apos.Y / 256.0f);
                float f2 = 64.0f * (apos.X / 256.0f);
                int posY = Convert.ToInt32(f1);
                int posX = Convert.ToInt32(f2);

                int parcelid = client.Network.CurrentSim.ParcelMap[posY, posX];

                if (parcelid == 0)
                {
                    client.Self.Teleport(client.Network.CurrentSim.Name, apos);
                    return;
                }

                if ((posX == 0 && posY == 0) || (posX == -1 && posY == -1) || (posX == -1 && posY == 0) || (posX == 0 && posY == -1))
                {
                    client.Self.GoHome();
                    return;
                }
            }
            catch { ; }
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            CheckAutoSit();
        }

        private void Parcels_OnParcelDwell(object sender, ParcelDwellReplyEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                UpdateMedia();
            }));           
        }

        private void UpdateMedia()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => UpdateMedia()));
                return;
            }

            try
            {
                Parcel parcel;
                Vector3 apos = instance.SIMsittingPos();

                float f1 = 64.0f * (apos.Y / 256.0f);
                float f2 = 64.0f * (apos.X / 256.0f);
                int posY = Convert.ToInt32(f1);
                int posX = Convert.ToInt32(f2);

                int parcelid = client.Network.CurrentSim.ParcelMap[posY, posX];

                if (parcelid == 0)
                {
                    Logger.Log("Chat Console: land media could not be updated ", Helpers.LogLevel.Info); 
                    return;
                }

                if (!client.Network.CurrentSim.Parcels.TryGetValue(parcelid, out parcel))
                    return;

                ParcelMedia med = parcel.Media;
                this.instance.Config.CurrentConfig.mURL = @med.MediaURL;
            }
            catch (Exception ex)
            {
                Logger.Log("Chat Console Error updating Land Media: " + ex.Message, Helpers.LogLevel.Error);   
            }

            if (string.IsNullOrEmpty(this.instance.Config.CurrentConfig.mURL))
            {
                tsMovie.Enabled = false;
            }
            else
            {
                tsMovie.Enabled = true;
            }

            if (string.IsNullOrEmpty(this.instance.Config.CurrentConfig.pURL))
            {
                tsMusic.Enabled = false;
            }
            else
            {
                tsMusic.Enabled = true;
            }
        }

        public void RemoveEvents()
        {
            netcom.ClientLoginStatus -= new EventHandler<LoginProgressEventArgs>(netcom_ClientLoginStatus);
            netcom.ClientLoggedOut -= new EventHandler(netcom_ClientLoggedOut);
            netcom.ChatReceived -= new EventHandler<ChatEventArgs>(netcom_ChatReceived);

            client.Objects.AvatarUpdate -= new EventHandler<AvatarUpdateEventArgs>(Objects_OnNewAvatar);
            client.Objects.KillObject -= new EventHandler<KillObjectEventArgs>(Objects_OnObjectKilled);
            client.Grid.CoarseLocationUpdate -= new EventHandler<CoarseLocationUpdateEventArgs>(Grid_OnCoarseLocationUpdate);
            client.Network.SimChanged -= new EventHandler<SimChangedEventArgs>(Network_OnCurrentSimChanged);
            client.Self.MeanCollision -= new EventHandler<MeanCollisionEventArgs>(Self_Collision);
            client.Objects.TerseObjectUpdate -= new EventHandler<TerseObjectUpdateEventArgs>(Objects_OnObjectUpdated);

            //if (instance.Config.CurrentConfig.iRadar)
            //{
            //    //client.Objects.TerseObjectUpdate -= new EventHandler<TerseObjectUpdateEventArgs>(Objects_OnObjectUpdated);
            //    client.Self.MeanCollision -= new EventHandler<MeanCollisionEventArgs>(Self_Collision);
            //}
        }

        private void CheckWearables()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    CheckWearables();
                }));
                
                //BeginInvoke(new MethodInvoker(() => CheckWearables()));
                return;
            }

            int loopbreaker = 0;

            try
            {
                UUID shape = client.Appearance.GetWearableAsset(WearableType.Shape);
                UUID skin = client.Appearance.GetWearableAsset(WearableType.Skin);
                UUID hair = client.Appearance.GetWearableAsset(WearableType.Hair);

                UUID pants = client.Appearance.GetWearableAsset(WearableType.Pants);
                UUID skirt = client.Appearance.GetWearableAsset(WearableType.Skirt);

                // shoes always seem to be missing!!! So I am taking it out
                //UUID shoes = client.Appearance.GetWearableAsset(WearableType.Shoes);

                UUID shirt = client.Appearance.GetWearableAsset(WearableType.Shirt);
                UUID jacket = client.Appearance.GetWearableAsset(WearableType.Jacket);
                UUID eyes = client.Appearance.GetWearableAsset(WearableType.Eyes);
                UUID underpants = client.Appearance.GetWearableAsset(WearableType.Underpants);
                UUID undershirt = client.Appearance.GetWearableAsset(WearableType.Undershirt);

                if (shape == UUID.Zero || skin == UUID.Zero || hair == UUID.Zero || shirt == UUID.Zero
                                        || pants == UUID.Zero || skirt == UUID.Zero || jacket == UUID.Zero 
                                        || eyes == UUID.Zero )
                {
                    string missing = string.Empty;

                    if (shape == UUID.Zero)
                    {
                        missing = "shape, ";
                    }

                    if (skin == UUID.Zero)
                    {
                        missing += "skin, ";
                    }

                    if (hair == UUID.Zero)
                    {
                        missing += "hair, ";
                    }

                    if (eyes == UUID.Zero)
                    {
                        missing += "eyes, ";
                    }

                    if (shirt == UUID.Zero && jacket == UUID.Zero)
                    {
                        if (undershirt == UUID.Zero)
                        {
                            missing += "shirt/jacket & undershirt, ";
                        }
                    }

                    if (skirt == UUID.Zero && pants == UUID.Zero)
                    {
                        if (underpants == UUID.Zero)
                        {
                            // The avatar is likely to be naked so let's try
                            // to get it dressed but we must avoid ending up in a loop
                            if (loopbreaker == 0)
                            {
                                client.Appearance.RequestSetAppearance(true);
                                loopbreaker += 1;
                                return;
                            }
                        }

                        missing += "pants/skirt & underpants, ";
                    }

                    if (!string.IsNullOrEmpty(missing))
                    {
                        if (missing.EndsWith(", ", StringComparison.CurrentCulture))
                        {
                            missing = missing.Remove(missing.Length - 2);   
                        }

                        chatManager.PrintAlertMessage("Wearables missing: " + missing);
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.Log(ex.Message, Helpers.LogLevel.Error);
                reporter.Show(ex);
            }

            client.Appearance.AppearanceSet -= new EventHandler<AppearanceSetEventArgs>(Appearance_OnAppearanceSet);
        }

        private void CheckAutoSit()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    CheckAutoSit();
                }));

                return;
            }

            if (!sitTimer.Enabled) return;

            sitTimer.Stop();
            sitTimer.Enabled = false;
            sitTimer.Dispose();
            sitTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent);

            Logger.Log("AUTOSIT: Searching for sit object", Helpers.LogLevel.Info);
            
            Vector3 location = client.Self.SimPosition;
            float radius = 21;

            // *** find all objects in radius ***
            List<Primitive> prims = client.Network.CurrentSim.ObjectsPrimitives.FindAll(
                delegate(Primitive prim)
                {
                    Vector3 pos = prim.Position;
                    return ((prim.ParentID == 0) && (pos != Vector3.Zero) && (Vector3.Distance(pos, location) < radius));
                }
            );

            localids = new uint[prims.Count];
            int i = 0;

            if (listnerdisposed)
            {
                client.Objects.ObjectProperties += new EventHandler<ObjectPropertiesEventArgs>(Objects_OnObjectProperties);
                listnerdisposed = false;
            }

            foreach (Primitive prim in prims)
            {
                try
                {
                    if (prim.ParentID == 0) //root prims only
                    {
                        localids[i] = prims[i].LocalID;
                        i += 1;
                    }

                }
                catch (Exception ex)
                {
                    //string exp = exc.Message;
                    reporter.Show(ex);
                }
            }

            client.Objects.SelectObjects(client.Network.CurrentSim, localids);
        }

        private void Objects_OnObjectProperties(object sender, ObjectPropertiesEventArgs e)
        {
            if (e.Properties.Description.Trim() == client.Self.AgentID.ToString().Trim())
            {
                instance.State.SetSitting(true, e.Properties.ObjectID);
                client.Objects.ObjectProperties -= new EventHandler<ObjectPropertiesEventArgs>(Objects_OnObjectProperties);
                localids = null;
                listnerdisposed = true;
                Logger.Log("AUTOSIT: Found sit object and sitting", Helpers.LogLevel.Info);
            }
        }

        void Self_AvatarSitResponse(object sender, AvatarSitResponseEventArgs e)
        {
            instance.State.SitPrim = e.ObjectID;
            instance.State.IsSitting = true;

            client.Self.AvatarSitResponse -= new EventHandler<AvatarSitResponseEventArgs>(Self_AvatarSitResponse);
        }

        private void AddLanguages()
        {
            // TODO: This should be converted into a language combobox component at
            // some stage

            cboLanguage.Items.Clear();
            //cboLanguage.Items.Add("Select...");
            cboLanguage.Items.Add(new ComboEx.ICItem("Select...", -1));

            cboLanguage.Items.Add(new ComboEx.ICItem("English/Arabic en|ar", 1));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Chineese(simp) en|zh-CN", 2));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Chineese(trad) en|zh-TW", 3));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Croatian en|hr", 4));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Czech en|cs", 5));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Danish en|da", 6));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Dutch en|nl", 7));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Filipino en|tl", 9));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Finnish en|fi", 10));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/French en|fr", 11));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/German en|de", 12));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Greek en|el", 13));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Hebrew en|iw", 14));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Hindi en|hi", 15));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Hungarian en|hu", 16));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Indonesian en|id", 17));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Italian en|it", 18));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Japanese en|ja", 19));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Korean en|ko", 20));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Lithuanian en|lt", 21));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Norwegian en|no", 22));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Polish en|pl", 23));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Portuguese en|p", 24));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Romanian en|ro", 25));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Russian en|ru", 26));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Slovenian en|sl", 27));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Spanish en|es", 28));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Swedish en|sv", 29));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Thai en|th", 30));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Turkish en|tr", 31));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Ukrainian en|uk", 32));

            cboLanguage.Items.Add("Arabic/English ar|en");
            cboLanguage.Items.Add("Chineese(simp)/English zh-CN|en");
            cboLanguage.Items.Add("Chineese(trad)/English zh-TW|en");
            cboLanguage.Items.Add("Croatian/English hr|en");
            cboLanguage.Items.Add("Czech/English cs|en");
            cboLanguage.Items.Add("Danish/English da|en");
            cboLanguage.Items.Add("Dutch/English nl|en");
            cboLanguage.Items.Add("Finnish/English fi|en");
            cboLanguage.Items.Add("Filipino/English tl|en");
            cboLanguage.Items.Add("French/English fr|en");
            cboLanguage.Items.Add("German/English de|en");
            cboLanguage.Items.Add("Greek/English el|en");
            cboLanguage.Items.Add("Hebrew/English iw|en");
            cboLanguage.Items.Add("Hindi/English hi|en");
            cboLanguage.Items.Add("Hungarian/English hu|en");
            cboLanguage.Items.Add("Indonesian/English id|en");
            cboLanguage.Items.Add("Italian/English it|en");
            cboLanguage.Items.Add("Japanese/English ja|en");
            cboLanguage.Items.Add("Korean/English ko|en");
            cboLanguage.Items.Add("Lithuanian/English lt|en");
            cboLanguage.Items.Add("Norwegian/English no|en");
            cboLanguage.Items.Add("Polish/English pl|en");
            cboLanguage.Items.Add("Portuguese/English pt|en");
            cboLanguage.Items.Add("Russian/English ru|en");
            cboLanguage.Items.Add("Romanian/English ro|en");
            cboLanguage.Items.Add("Slovenian/English sl|en");
            cboLanguage.Items.Add("Spanish/English es|en");
            cboLanguage.Items.Add("Swedish/English sv|en");
            cboLanguage.Items.Add("Thai/English th|en");
            cboLanguage.Items.Add("Turkish/English tr|en");
            cboLanguage.Items.Add("Ukrainian/English uk|en");

            cboLanguage.Items.Add("German/French de|fr");
            cboLanguage.Items.Add("Spanish/French es|fr");
            cboLanguage.Items.Add("French/German fr|de");
            cboLanguage.Items.Add("French/Spanish fr|es");
            cboLanguage.SelectedIndex = 0;
        }

        private void CreateSmileys()
        {
            // TODO: This should be converted into a smiley menu component at
            // some stage

            EmoticonMenuItem _menuItem;

            _menuItem = new EmoticonMenuItem(Smileys.AngelSmile);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[0].Tag = (object)"angelsmile;";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.AngrySmile);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[1].Tag = "angry;";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.Beer);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[2].Tag = "beer;";
            _menuItem = null;

            //_menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.BrokenHeart);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[3].Tag = "brokenheart;";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.bye);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[4].Tag = "bye";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.clap);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[5].Tag = "clap;";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.ConfusedSmile);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[6].Tag = ":S";

            _menuItem.BarBreak = true;
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.cool);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[7].Tag = "cool;";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.CrySmile);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[8].Tag = "cry;";
            _menuItem = null;

            //_menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.DevilSmile);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[9].Tag = "devil;";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.duh);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[10].Tag = "duh;";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.EmbarassedSmile);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[11].Tag = "embarassed;";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.happy0037);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[12].Tag = ":)";

            _menuItem.BarBreak = true;
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.heart);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[13].Tag = "heart;";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.kiss);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[14].Tag = "muah;";
            _menuItem = null;

            //_menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.help);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[15].Tag = "help ";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.liar);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[16].Tag = "liar;";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.lol);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[17].Tag = "lol";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.oops);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[18].Tag = "oops";

            _menuItem.BarBreak = true;
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.sad);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[19].Tag = ":(";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.shhh);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[20].Tag = "shhh";
            _menuItem = null;

            //_menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.sigh);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[21].Tag = "sigh ";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.silenced);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[22].Tag = ":X";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.think);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[23].Tag = "thinking;";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.ThumbsUp);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[24].Tag = "thumbsup;";

            _menuItem.BarBreak = true;
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.whistle);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[25].Tag = "whistle;";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.wink);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[26].Tag = ";)";
            _menuItem = null;

            //_menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.wow);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[27].Tag = "wow ";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.yawn);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[28].Tag = "yawn;";
            _menuItem = null;

            _menuItem = new EmoticonMenuItem(Smileys.zzzzz);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[29].Tag = "zzz";

            _menuItem.Dispose(); 
        }

        // When an emoticon is clicked, insert its image into RTF
        private void cmenu_Emoticons_Click(object _sender, EventArgs _args)
        {
            // Write the code here
            EmoticonMenuItem _item = (EmoticonMenuItem)_sender;

            cbxInput.Text += _item.Tag.ToString();
            cbxInput.Select(cbxInput.Text.Length, 0);
            //cbxInput.Focus();

            _item.Dispose(); 
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            tabConsole = instance.TabConsole;
        }

        private void Config_ConfigApplied(object sender, ConfigAppliedEventArgs e)
        {
            ApplyConfig(e.AppliedConfig);
        }

        private void ApplyConfig(Config config)
        {
            if (config.InterfaceStyle == 0) //System
            {
                toolStrip1.RenderMode = ToolStripRenderMode.System;
            }
            else if (config.InterfaceStyle == 1) //Office 2003
            {
                toolStrip1.RenderMode = ToolStripRenderMode.ManagerRenderMode;
            }

            //rtbChat.BackColor = instance.Config.CurrentConfig.BgColour; 
        }

        private void AddNetcomEvents()
        {
            netcom.ClientLoginStatus += new EventHandler<LoginProgressEventArgs>(netcom_ClientLoginStatus);
            netcom.ClientLoggedOut += new EventHandler(netcom_ClientLoggedOut);
            netcom.ChatReceived += new EventHandler<ChatEventArgs>(netcom_ChatReceived);
        }

        private void AddClientEvents()
        {
            client.Objects.AvatarUpdate += new EventHandler<AvatarUpdateEventArgs>(Objects_OnNewAvatar);
            client.Objects.KillObject += new EventHandler<KillObjectEventArgs>(Objects_OnObjectKilled);
            client.Grid.CoarseLocationUpdate += new EventHandler<CoarseLocationUpdateEventArgs>(Grid_OnCoarseLocationUpdate);
            client.Network.SimChanged += new EventHandler<SimChangedEventArgs>(Network_OnCurrentSimChanged);

            client.Self.MeanCollision += new EventHandler<MeanCollisionEventArgs>(Self_Collision);
            client.Objects.TerseObjectUpdate += new EventHandler<TerseObjectUpdateEventArgs>(Objects_OnObjectUpdated);

            //if (instance.Config.CurrentConfig.iRadar)
            //{
            //    //client.Objects.TerseObjectUpdate += new EventHandler<TerseObjectUpdateEventArgs>(Objects_OnObjectUpdated);
                
            //}
        }

        // Seperate thread
        private void Self_Collision(object sender, MeanCollisionEventArgs e)
        {
            // The av has collided with an object or avatar

            string cty = string.Empty;

            try
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    if (e.Type == MeanCollisionType.Bump)
                    {
                        cty = "Bumped in by: (" + e.Time.ToString(CultureInfo.CurrentCulture) + " - " + e.Magnitude.ToString(CultureInfo.CurrentCulture) + "): ";
                    }
                    else if (e.Type == MeanCollisionType.LLPushObject)
                    {
                        cty = "Pushed by: (" + e.Time.ToString(CultureInfo.CurrentCulture) + " - " + e.Magnitude.ToString(CultureInfo.CurrentCulture) + "): ";
                    }
                    else if (e.Type == MeanCollisionType.PhysicalObjectCollide)
                    {
                        cty = "Physical object collided (" + e.Time.ToString(CultureInfo.CurrentCulture) + " - " + e.Magnitude.ToString(CultureInfo.CurrentCulture) + "): ";
                    }

                    chatManager.PrintAlertMessage(cty + e.Aggressor.ToString());
                }));
            }
            catch
            {
               ;
            }
        }

        //Separate thread
        private void Objects_OnObjectKilled(object sender, KillObjectEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    Objects_OnObjectKilled(sender, e);
                }));

                return;
            }

            if (e.Simulator != client.Network.CurrentSim) return;
            if (sfavatar == null) return;
            if (!sfavatar.ContainsKey(e.ObjectLocalID)) return;

            foreach (ListViewItem litem in lvwRadar.Items)
            {

                if (litem.Tag.ToString() == sfavatar[e.ObjectLocalID].ID.ToString())
                {
                    lvwRadar.BeginUpdate();
                    lvwRadar.Items.RemoveByKey(sfavatar[e.ObjectLocalID].Name);
                    lvwRadar.EndUpdate();
                }
            }

            try
            {
                lock (sfavatar)
                {
                    sfavatar.Remove(e.ObjectLocalID);
                }
            }
            catch { ; }
        }

        //Separate thread
        private void Objects_OnNewAvatar(object sender, AvatarUpdateEventArgs e)
        {
            if (InvokeRequired)
            {

                BeginInvoke(new MethodInvoker(delegate()
                {
                    Objects_OnNewAvatar(sender, e);
                }));

                return;
            }

            if (e.Simulator != client.Network.CurrentSim) return;
            if (sfavatar.ContainsKey(e.Avatar.LocalID)) return;

            try
            {
                lock (sfavatar)
                {
                    sfavatar.Add(e.Avatar.LocalID, e.Avatar);
                }
            }
            catch { ; }
        }

        private void Objects_OnObjectUpdated(object sender, TerseObjectUpdateEventArgs e)
        {
            if (e.Simulator != client.Network.CurrentSim) return;
            if (!e.Update.Avatar) return;

            if (!sfavatar.ContainsKey(e.Prim.LocalID))
            {
                Avatar av;
                client.Network.CurrentSim.ObjectsAvatars.TryGetValue(e.Update.LocalID, out av);

                if (av == null) return;

                if (!sfavatar.ContainsKey(av.LocalID))
                {
                    try
                    {
                        lock (sfavatar)
                        {
                            sfavatar.Add(av.LocalID, av);
                        }
                    }
                    catch { ; }
                }
            }
        }

        private delegate void OnAddSIMAvatar(string av, UUID key, Vector3 avpos);
        public void AddSIMAvatar(string av, UUID key, Vector3 avpos)
        {
            if (InvokeRequired)
            {

                BeginInvoke(new MethodInvoker(delegate()
                {
                    AddSIMAvatar(av, key, avpos);
                }));

                return;
            }

            if (!string.IsNullOrEmpty(selectedname)) return;

            if (av == null) return;
            string name = av;

            if (string.IsNullOrEmpty(name)) return;

            lvwRadar.BeginUpdate();
            if (lvwRadar.Items.ContainsKey(name))
            {
                lvwRadar.Items.RemoveByKey(name);  
            }
            lvwRadar.EndUpdate();

            string sDist;
            
            try
            {
                Vector3 selfpos = client.Self.SimPosition;

                if (selfpos.Z == 0)
                {
                    selfpos.Z = 1024f; // Convert.ToSingle(client.Self.GlobalPosition.Z);  
                }

                double dist = Math.Round(Vector3d.Distance(ConverToGLobal(avpos), ConverToGLobal(selfpos)), MidpointRounding.ToEven);

                if ((int)dist > instance.Config.CurrentConfig.RadarRange) return;

                if (avpos.Z < 0.1f)
                {
                    avpos.Z = 1024f;
                    dist = Math.Round(Vector3d.Distance(ConverToGLobal(avpos), ConverToGLobal(selfpos)), MidpointRounding.ToEven);
                    sDist = " >[" + Convert.ToInt32(dist, CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture) + "m]";
                }
                else
                {
                    sDist = " [" + Convert.ToInt32(dist, CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture) + "m]";
                }

                string rentry = name + sDist;

                lvwRadar.BeginUpdate();

                if (name != client.Self.Name)
                {
                    ListViewItem item = lvwRadar.Items.Add(name, rentry, string.Empty);
                    item.ForeColor = Color.DarkBlue; 
                    item.Tag = key;

                    if (avtyping.Contains(name))
                    {
                        int index = lvwRadar.Items.IndexOfKey(name);
                        if (index != -1)
                        {
                            lvwRadar.Items[index].ForeColor = Color.Red;
                        }
                    }
                }
                //else
                //{
                //    ListViewItem item = lvwRadar.Items.Add(name, name, string.Empty);
                //    item.Font = new Font(item.Font, FontStyle.Bold);
                //    item.Tag = key;
                //}

                if (!lvwRadar.Items.ContainsKey(client.Self.Name))
                {
                    ListViewItem item = lvwRadar.Items.Add(client.Self.Name, client.Self.Name, string.Empty);
                    item.Font = new Font(item.Font, FontStyle.Bold);
                    item.Tag = key;
                }
                
                lvwRadar.EndUpdate();
            }
            catch (Exception ex)
            {
                Logger.Log("Radar update: " + ex.Message, Helpers.LogLevel.Warning);
            }
        }

        private Vector3d ConverToGLobal(Vector3 pos)
        {
            uint regionX, regionY;
            Utils.LongToUInts(client.Network.CurrentSim.Handle, out regionX, out regionY);
            Vector3d objpos;

            objpos.X = (double)pos.X + (double)regionX;
            objpos.Y = (double)pos.Y + (double)regionY;
            objpos.Z = pos.Z;   // -2f;

            return objpos; 
        }

        private delegate void OnAddAvatar(Avatar av);
        public void AddAvatar(Avatar av)
        {
            if (InvokeRequired)
            {

                BeginInvoke(new MethodInvoker(delegate()
                {
                    AddAvatar(av);
                }));

                return;
            }

            if (!string.IsNullOrEmpty(selectedname)) return;

            if (av == null) return;
            string name = av.Name;

            if (string.IsNullOrEmpty(name)) return;

            BeginInvoke(new MethodInvoker(delegate()
            {
                lvwRadar.BeginUpdate();
                if (lvwRadar.Items.ContainsKey(name))
                {
                    lvwRadar.Items.RemoveByKey(name);
                }

                lvwRadar.EndUpdate();
            }));

            string sDist;

            Vector3 avpos = av.Position;

            uint oID = av.ParentID;
            string astate = string.Empty;

            if (!instance.avtags.ContainsKey(av.ID))
            {
                instance.avtags.Add(av.ID, av.GroupName);
            }

            if (oID != 0)
            {
                // the av is sitting
                Primitive prim;

                try
                {
                    //// Stop following if in following mode
                    //if (instance.State.IsFollowing)
                    //{
                    //    instance.State.Follow(string.Empty);
                    //    tbtnFollow.ToolTipText = "Follow";
                    //}

                    client.Network.CurrentSim.ObjectsPrimitives.TryGetValue(oID, out prim);

                    if (prim == null) return;

                    avpos = prim.Position + avpos;
                    astate = " (SITTING)";
                }
                catch (Exception ex)
                {
                    Logger.Log("Chat console: (add avatar) when adding " + av.FirstName + " " + av.LastName + " - " + ex.Message, Helpers.LogLevel.Error, ex);
                    //reporter.Show(ex);
                }
            }
            else
            {
                astate = string.Empty;
            }

            try
            {
                Vector3 selfpos = client.Self.SimPosition;

                double dist = Math.Round(Vector3d.Distance(ConverToGLobal(avpos), ConverToGLobal(selfpos)), MidpointRounding.ToEven);

                if ((int)dist > instance.Config.CurrentConfig.RadarRange) return;

                if (avpos.Z < 0.1f)
                {
                    avpos.Z = 1024f;
                    dist = Math.Round(Vector3d.Distance(ConverToGLobal(avpos), ConverToGLobal(selfpos)), MidpointRounding.ToEven);
                    sDist = " >[" + Convert.ToInt32(dist, CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture) + "m]";
                }
                else
                {
                    sDist = " [" + Convert.ToInt32(dist, CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture) + "m]";
                }

                string rentry = name + sDist + astate;

                BeginInvoke(new MethodInvoker(delegate()
                { 
                    lvwRadar.BeginUpdate();

                    if (name != client.Self.Name)
                    {
                        ListViewItem item = lvwRadar.Items.Add(name, rentry, string.Empty);
                        item.Tag = av.ID;

                        if (avtyping.Contains(name))
                        {
                            int index = lvwRadar.Items.IndexOfKey(name);
                            if (index != -1)
                            {
                                lvwRadar.Items[index].ForeColor = Color.Red;
                            }
                        }
                    }
                    //else
                    //{
                    //    ListViewItem item = lvwRadar.Items.Add(name, name, string.Empty);
                    //    item.Font = new Font(item.Font, FontStyle.Bold);
                    //    item.Tag = av.ID;
                    //}

                    if (!lvwRadar.Items.ContainsKey(client.Self.Name))
                    {
                        ListViewItem item = lvwRadar.Items.Add(client.Self.Name, client.Self.Name, string.Empty);
                        item.Font = new Font(item.Font, FontStyle.Bold);
                        item.Tag = av.ID;
                    }

                    lvwRadar.EndUpdate();
                }));
            }
            catch (Exception ex)
            {
                Logger.Log("Radar update: " + ex.Message, Helpers.LogLevel.Warning);
            }
        }

        private void GetCompass()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    GetCompass();
                }));

                return;
            }

            //string heading = "~";

            Quaternion avRot = client.Self.RelativeRotation;
            
            Vector3 vdir = new Vector3(Vector3.Zero);
            vdir.X = 0.0f;
            vdir.Y = 1.0f;
            vdir.Z = 0.0f;

            Matrix4 m = Matrix4.CreateFromQuaternion(avRot);

            vDir = new Vector3(Vector3.Zero);
            vDir.X = m.M11;
            vDir.Y = m.M21;
            vDir.Z = m.M31;

            int x = Convert.ToInt32(vDir.X);
            int y = Convert.ToInt32(vDir.Y);

            if ((Math.Abs(x) > Math.Abs(y)) && (x > 0))
            {
                //heading = "E";
                picCompass.Image = Properties.Resources.c_e;
            }
            else if ((Math.Abs(x) > Math.Abs(y)) && (x < 0))
            {
                //heading = "W";
                picCompass.Image = Properties.Resources.c_w;
            }
            else if ((Math.Abs(y) > Math.Abs(x)) && (y > 0))
            {
                //heading = "S";
                picCompass.Image = Properties.Resources.c_s;
            }
            else if ((Math.Abs(y) > Math.Abs(x)) && (y < 0))
            {
                //heading = "N";
                picCompass.Image = Properties.Resources.c_n;
            }
            else if ((Math.Abs(y) == Math.Abs(x)) && (x > 0 && y > 0))
            {
                //heading = "SE";
                picCompass.Image = Properties.Resources.c_se;
            }
            else if ((Math.Abs(y) == Math.Abs(x)) && (x < 0 && y > 0))
            {
                //heading = "SW";
                picCompass.Image = Properties.Resources.c_sw;
            }
            else if ((Math.Abs(y) == Math.Abs(x)) && (x < 0 && y < 0))
            {
                //heading = "NW";
                picCompass.Image = Properties.Resources.c_nw;
            }
            else if ((Math.Abs(y) == Math.Abs(x)) && (x > 0 && y < 0))
            {
                //heading = "NE";
                picCompass.Image = Properties.Resources.c_ne;
            }

            //textBox1.Text = heading;
            //textBox1.Refresh();
        }

        private void netcom_ClientLoginStatus(object sender, LoginProgressEventArgs e)
        {
            if (e.Status != LoginStatus.Success) return;

            cbxInput.Enabled = true;
        }

        private void netcom_ClientLoggedOut(object sender, EventArgs e)
        {
            cbxInput.Enabled = false;
            tbSay.Enabled = false;

            lvwRadar.Items.Clear();
        }

        public void PrintAvUUID()
        {
            //chatManager = new ChatTextManager(instance, new RichTextBoxPrinter(instance, rtbChat));
            chatManager.PrintUUID();
        }

        private void netcom_ChatReceived(object sender, ChatEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    netcom_ChatReceived(sender, e);
                }));

                return;
            }

            if (e.SourceType != ChatSourceType.Agent)
            {
                return;
            }

            if (e.FromName.ToLower(CultureInfo.CurrentCulture) == netcom.LoginOptions.FullName.ToLower(CultureInfo.CurrentCulture))
            {
                return;
            }

            int index = lvwRadar.Items.IndexOfKey(e.FromName);
            if (index == -1) return;

            if (e.Type == ChatType.StartTyping)
            {
                lvwRadar.Items[index].ForeColor = Color.Red;

                if (!avtyping.Contains(e.FromName))
                {
                    avtyping.Add(e.FromName);
                }

                instance.State.LookAt(true, e.OwnerID);
            }
            else
            {
                lvwRadar.Items[index].ForeColor = Color.FromKnownColor(KnownColor.ControlText);

                if (avtyping.Contains(e.FromName))
                {
                    avtyping.Remove(e.FromName);
                }

                instance.State.LookAt(false, e.OwnerID);
            }

            if (instance.DetectLang)
            {
                if (!string.IsNullOrEmpty(e.Message))
                {
                    GoogleTranslationUtils.DetectLanguage detect = new GoogleTranslationUtils.DetectLanguage(e.Message);

                    int sindex = detect.LanguageIndex;

                    if (sindex == 33)
                        sindex = 0;

                    this.instance.MainForm.SetFlag(imgFlags.Images[sindex], detect.SpokenLanguage);

                    // select the language pair fro mthe combo
                    if (sindex != 0 && sindex != 8)
                    {
                        // English does not exist in the combo so adjust
                        if (sindex > 7)
                        {
                            sindex -= 1;
                        }

                        cboLanguage.SelectedIndex = sindex;
                    }
                }
            }
        }

        private void ProcessChatInput(string input, ChatType type)
        {
            if (string.IsNullOrEmpty(input)) return;

            if (chkTranslate.Checked == true)
            {
                if (cboLanguage.SelectedIndex != 0)
                {
                    // Call translation here
                    string oinp = input;
                    string tinput = GetTranslation(input);

                    if (tinput != null)
                    {
                        tinput = HttpUtility.HtmlDecode(tinput);
                        input = tinput + " (" + oinp + ")";
                    }
                }
            }

            string[] inputArgs = input.Split(' ');

            if (inputArgs[0].StartsWith("//", StringComparison.CurrentCulture)) //Chat on previously used channel
            {
                string message = string.Join(" ", inputArgs).Substring(2);
                netcom.ChatOut(message, type, previousChannel);
            }
            else if (inputArgs[0].StartsWith("/", StringComparison.CurrentCulture)) //Chat on specific channel
            {
                string message = string.Empty;
                string number = inputArgs[0].Substring(1);

                int channel = 0;
                int.TryParse(number, out channel);
                if (channel < 0) channel = 0;

                // VidaOrenstein on METAforum fix
                //string message = string.Join(" ", inputArgs, 1, inputArgs.GetUpperBound(0) - 1);

                if (input.StartsWith("/me ", StringComparison.CurrentCulture))
                {
                    message = input;
                }
                else
                {
                    message = string.Join(" ", inputArgs, 1, inputArgs.GetUpperBound(0));
                }

                netcom.ChatOut(message, type, channel);

                previousChannel = channel;
            }
            else //Chat on channel 0 (public chat)
            {
                netcom.ChatOut(input, type, 0);
            }

            ClearChatInput();
        }

        private string GetTranslation(string sTrStr)
        {
            string sPair = GetLangPair(cboLanguage.Text);

            GoogleTranslationUtils.Translate trans = new GoogleTranslationUtils.Translate(sTrStr, sPair);

            return trans.Translation;
        }

        private string GetLangPair(string sPair)
        {
            string[] inputArgs = sPair.Split(' ');

            return inputArgs[1].ToString();
        }

        private void ClearChatInput()
        {
            cbxInput.Items.Add(cbxInput.Text);
            cbxInput.Text = string.Empty;
        }

        private void cbxInput_TextChanged(object sender, EventArgs e)
        {
            if (cbxInput.Text.Length > 0)
            {
                tbSay.Enabled = true;

                if (!cbxInput.Text.StartsWith("/", StringComparison.CurrentCulture))
                {
                    if (!instance.State.IsTyping)
                        instance.State.SetTyping(true);
                }
            }
            else
            {
                tbSay.Enabled = false;
                instance.State.SetTyping(false);
            }
        }

        public ChatTextManager ChatManager
        {
            get { return chatManager; }
        }

        private void tbtnStartIM_Click(object sender, EventArgs e)
        {
            UUID av = (UUID)lvwRadar.SelectedItems[0].Tag;

            if (av == UUID.Zero || av == null) return;

            string name = instance.avnames[av];

            if (tabConsole.TabExists(name))
            {
                tabConsole.SelectTab(name);
                return;
            }

            tabConsole.AddIMTab(av, client.Self.AgentID ^ av, name);
            tabConsole.SelectTab(name);
        }

        private void tbtnFollow_Click(object sender, EventArgs e)
        {
            //Avatar av = ((ListViewItem)lvwRadar.SelectedItems[0]).Tag as Avatar;
            //if (av == null) return;

            UUID av = (UUID)lvwRadar.SelectedItems[0].Tag;

            if (av == UUID.Zero || av == null) return;

            string name = instance.avnames[av];

            if (instance.State.FollowName != name)
            {
                instance.State.Follow(name);
                tbtnFollow.ToolTipText = "Stop Following";
            }
            else
            {
                instance.State.Follow(string.Empty);
                tbtnFollow.ToolTipText = "Follow";
            }
        }

        private void tbtnProfile_Click(object sender, EventArgs e)
        {
            //Avatar av = ((ListViewItem)lvwRadar.SelectedItems[0]).Tag as Avatar;
            //if (av == null) return;

            UUID av = (UUID)lvwRadar.SelectedItems[0].Tag;

            if (av == UUID.Zero || av == null) return;

            string name = instance.avnames[av];

            (new frmProfile(instance, name, av)).Show();
        }

        private void cbxInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) e.SuppressKeyPress = true;

            if (e.Control && e.KeyCode == Keys.V)
            {
                ClipboardAsync Clipboard2 = new ClipboardAsync();
                cbxInput.Text += Clipboard2.GetText(TextDataFormat.UnicodeText).Replace(Environment.NewLine, "\r\n");

                // This is a fix for a silly bug which I can't find
                // what's causing it. There is a REWARD for the first that can :)
                // The bug is that the 1st copied line is pasted twice!!!

                pasted = true; 
            }
        }

        private void cbxInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (pasted)
            {
                int pos = cbxInput.SelectionStart;
                cbxInput.SelectionLength = cbxInput.Text.Length - pos;
                cbxInput.Text = cbxInput.SelectedText;
                pasted = false;
            }

            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;

            if (e.Control && e.Shift)
                ProcessChatInput(cbxInput.Text, ChatType.Whisper);
            else if (e.Control)
                ProcessChatInput(cbxInput.Text, ChatType.Shout);
            else
                ProcessChatInput(cbxInput.Text, ChatType.Normal);
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void tbtnAddFriend_Click(object sender, EventArgs e)
        {
            //Avatar av = ((ListViewItem)lvwRadar.SelectedItems[0]).Tag as Avatar;
            //if (av == null) return;

            UUID av = (UUID)lvwRadar.SelectedItems[0].Tag;

            if (av == UUID.Zero || av == null) return;

            string name = instance.avnames[av];

            Boolean fFound = true;

            client.Friends.FriendList.ForEach(delegate(FriendInfo friend)
            {
                if (friend.Name == name)
                {
                    fFound = false;
                }
            });

            if (fFound)
            {
                client.Friends.OfferFriendship(av);
            }
        }

        private void tbtnFreeze_Click(object sender, EventArgs e)
        {
            //Avatar av = ((ListViewItem)lvwRadar.SelectedItems[0]).Tag as Avatar;
            //if (av == null) return;

            UUID av = (UUID)lvwRadar.SelectedItems[0].Tag;

            client.Parcels.FreezeUser(av, true);
        }

        private void tbtnBan_Click(object sender, EventArgs e)
        {
            //Avatar av = ((ListViewItem)lvwRadar.SelectedItems[0]).Tag as Avatar;
            //if (av == null) return;

            UUID av = (UUID)lvwRadar.SelectedItems[0].Tag;

            client.Parcels.EjectUser(av, true);
        }

        private void tbtnEject_Click_1(object sender, EventArgs e)
        {
            //Avatar av = ((ListViewItem)lvwRadar.SelectedItems[0]).Tag as Avatar;
            //if (av == null) return;

            UUID av = (UUID)lvwRadar.SelectedItems[0].Tag;

            client.Parcels.EjectUser(av, false);
        }

        private void rtbChat_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void rtbChat_TextChanged_1(object sender, EventArgs e)
        {
            //int i = rtbChat.Lines.Length;

            //if (i > 10)
            //{
            //    int lineno = i-10;
            //    int chars = rtbChat.GetFirstCharIndexFromLine(lineno);
            //    rtbChat.SelectionStart = 0;
            //    rtbChat.SelectionLength = chars; // rtbChat.Text.IndexOf("\n", 0) + 1;
            //    rtbChat.SelectedText = "*** " + lineno.ToString() + "lines purged\n";
            //}
            //else
            //{
            //    return;
            //}
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void tbar_SendMessage_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {

        }

        private void tbar_SendMessage_ButtonClick_1(object sender, ToolBarButtonClickEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkTranslate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTranslate.Checked == true)
            {
                MessageBox.Show("~ METAtranslate ~ \n \n You must now select a language pair \n from the dropdown box. \n \n Anything you say will be auto translated to that language.", "METAtranslate");
                cboLanguage.Enabled = true;
            }
            else
            {
                cboLanguage.Enabled = false;
                cboLanguage.SelectedIndex = 0;
            }
        }

        //private void checkBox2_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBox2.Checked == true)
        //    {
        //        (new frmTranslate(instance)).Show();
        //    }
        //    else
        //    {
        //        (new frmTranslate(instance)).Close();
        //    }
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            (new frmTranslate(instance)).ShowDialog();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        public string _textBox
        {
            set { cbxInput.Text = value; }

        }

        //public bool _Search
        //{
        //    set { panel7.Visible = value; }
        //}

        private void tbtnGoto_Click(object sender, EventArgs e)
        {
            client.Self.AutoPilotCancel();

            //Avatar av = ((ListViewItem)lvwRadar.SelectedItems[0]).Tag as Avatar;
            //if (av == null) return;

            UUID av = (UUID)lvwRadar.SelectedItems[0].Tag;

            if (av == UUID.Zero || av == null) return;

            //string name = instance.avnames[av];

            Avatar sav = client.Network.CurrentSim.ObjectsAvatars.Find(delegate(Avatar fa)
            {
                return fa.ID == av;
            }
            );

            if (sav != null)
            {
                Vector3 pos = sav.Position;
                ulong regionHandle = client.Network.CurrentSim.Handle;

                //int followRegionX = (int)(regionHandle >> 32);
                //int followRegionY = (int)(regionHandle & 0xFFFFFFFF);
                //ulong x = (ulong)pos.X + (ulong)followRegionX;
                //ulong y = (ulong)pos.Y + (ulong)followRegionY;

                ulong followRegionX = regionHandle >> 32;
                ulong followRegionY = regionHandle & (ulong)0xFFFFFFFF;

                ulong x = (ulong)pos.X + followRegionX;
                ulong y = (ulong)pos.Y + followRegionY;
                float z = pos.Z - 1f;

                client.Self.AutoPilot(x, y, z);
            }
            else
            {
                MessageBox.Show("Avatar is out of range for this function.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tbtnTurn_Click(object sender, EventArgs e)
        {
            //Avatar av = ((ListViewItem)lvwRadar.SelectedItems[0]).Tag as Avatar;
            //if (av == null) return;

            UUID av = (UUID)lvwRadar.SelectedItems[0].Tag;

            if (av == UUID.Zero || av == null) return;

            //string name = instance.avnames[av];

            Avatar sav = client.Network.CurrentSim.ObjectsAvatars.Find(delegate(Avatar fa)
            {
                return fa.ID == av;
            }
            );

            if (sav != null)
            {
                client.Self.AnimationStart(Animations.TURNLEFT, false);

                Vector3 pos = sav.Position;

                client.Self.Movement.TurnToward(pos);

                client.Self.Movement.FinishAnim = true;
                System.Threading.Thread.Sleep(200);
                client.Self.AnimationStop(Animations.TURNLEFT, false);
            }
            else
            {
                MessageBox.Show("Avatar is out of range for this function.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void rtbChat_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (e.LinkText.StartsWith("http://slurl.", StringComparison.CurrentCulture))
            {
                try
                {
                    // Open up the TP form here
                    string[] split = e.LinkText.Split(new Char[] { '/' });
                    string simr = split[4].ToString();
                    double x = Convert.ToDouble(split[5].ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
                    double y = Convert.ToDouble(split[6].ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
                    double z = Convert.ToDouble(split[7].ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);

                    (new frmTeleport(instance, simr, (float)x, (float)y, (float)z)).ShowDialog();
                }
                catch { ; }

            }
            if (e.LinkText.StartsWith("http://maps.secondlife", StringComparison.CurrentCulture))
            {
                try
                {
                    // Open up the TP form here
                    string[] split = e.LinkText.Split(new Char[] { '/' });
                    string simr = split[4].ToString();
                    double x = Convert.ToDouble(split[5].ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
                    double y = Convert.ToDouble(split[6].ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
                    double z = Convert.ToDouble(split[7].ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);

                    (new frmTeleport(instance, simr, (float)x, (float)y, (float)z)).ShowDialog();
                }
                catch { ; }

            }
            else if (e.LinkText.Contains("http://mbprofile:"))
            {
                try
                {
                    string[] split = e.LinkText.Split(new Char[] { '#' });
                    string aavname = split[0].ToString();
                    split = e.LinkText.Split(new Char[] { ':' });
                    string elink = split[2].ToString();
                    split = elink.Split(new Char[] { '&' });

                    UUID avid = (UUID)split[0].ToString();

                    (new frmProfile(instance, aavname, avid)).Show();
                }
                catch { ; }
            }
            //else if (e.LinkText.Contains("secondlife:///"))
            //{
            //    // Open up the Group Info form here
            //    //string[] split = e.LinkText.Split(new Char[] { '/' });
            //    //UUID uuid = (UUID)split[4].ToString();
                
            //    //frmGroupInfo frm = new frmGroupInfo(uuid, instance);
            //    //frm.Show();
            //}
            else if (e.LinkText.StartsWith("http://", StringComparison.CurrentCulture) || e.LinkText.StartsWith("ftp://", StringComparison.CurrentCulture) || e.LinkText.StartsWith("https://", StringComparison.CurrentCulture))
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            else
            {
                System.Diagnostics.Process.Start("http://" + e.LinkText);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lft();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rgt();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            fwd();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bck();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        protected override bool ProcessKeyPreview(ref System.Windows.Forms.Message m)
        {
            int key = m.WParam.ToInt32();

            // Fix submitted by METAforums user Spirit 25/09/2009
            // TO DO: This should be a setting in preferences so that
            // it works both ways...
            if (cbxInput.Focused) return false;

           
            if (instance.State.IsFlying)
            {
                switch (key)
                {
                    case 33: // <--- page up.
                        up();
                        break;
                    case 34: // <--- page down.
                        dwn();
                        break;
                    case 37: // <--- left arrow.
                        lft();
                        break;
                    case 38: // <--- up arrow.
                        fwd();
                        break;
                    case 39: // <--- right arrow.
                        rgt();
                        break;
                    case 40: // <--- down arrow.
                        bck();
                        break;
                }
            }
            else
            {
                switch (key)
                {
                    case 33: // <--- page up.
                        up();
                        break;
                    case 34: // <--- page down.
                        dwn();
                        break;
                    case 37: // <--- left arrow.
                        lft();
                        break;
                    case 38: // <--- up arrow.
                        fwd();
                        break;
                    case 39: // <--- right arrow.
                        rgt();
                        break;
                    case 40: // <--- down arrow.
                        bck();
                        break;
                }
            }

            return false;
        }

        private void up()
        {
            client.Self.Movement.SendManualUpdate(AgentManager.ControlFlags.AGENT_CONTROL_UP_POS, client.Self.Movement.Camera.Position,
                    client.Self.Movement.Camera.AtAxis, client.Self.Movement.Camera.LeftAxis, client.Self.Movement.Camera.UpAxis,
                    client.Self.Movement.BodyRotation, client.Self.Movement.HeadRotation, client.Self.Movement.Camera.Far, AgentFlags.None,
                    AgentState.None, true);
        }

        private void dwn()
        {
            client.Self.Movement.SendManualUpdate(AgentManager.ControlFlags.AGENT_CONTROL_UP_NEG, client.Self.Movement.Camera.Position,
                    client.Self.Movement.Camera.AtAxis, client.Self.Movement.Camera.LeftAxis, client.Self.Movement.Camera.UpAxis,
                    client.Self.Movement.BodyRotation, client.Self.Movement.HeadRotation, client.Self.Movement.Camera.Far, AgentFlags.None,
                    AgentState.None, true);
        }

        private void ffwd()
        {
            client.Self.Movement.SendManualUpdate(AgentManager.ControlFlags.AGENT_CONTROL_PITCH_POS, client.Self.Movement.Camera.Position,
                    client.Self.Movement.Camera.AtAxis, client.Self.Movement.Camera.LeftAxis, client.Self.Movement.Camera.UpAxis,
                    client.Self.Movement.BodyRotation, client.Self.Movement.HeadRotation, client.Self.Movement.Camera.Far, AgentFlags.None,
                    AgentState.None, true);
        }

        private void fwd()
        {
            //client.Self.Movement.SendManualUpdate(AgentManager.ControlFlags.AGENT_CONTROL_AT_POS, client.Self.Movement.Camera.Position,
            //        client.Self.Movement.Camera.AtAxis, client.Self.Movement.Camera.LeftAxis, client.Self.Movement.Camera.UpAxis,
            //        client.Self.Movement.BodyRotation, client.Self.Movement.HeadRotation, client.Self.Movement.Camera.Far, AgentFlags.None,
            //        AgentState.None, true);

            client.Self.Movement.AutoResetControls = false;
            client.Self.Movement.AtPos = true;
            client.Self.Movement.SendUpdate();
        }

        private void bck()
        {
            //client.Self.Movement.SendManualUpdate(AgentManager.ControlFlags.AGENT_CONTROL_AT_NEG, client.Self.Movement.Camera.Position,
            //        client.Self.Movement.Camera.AtAxis, client.Self.Movement.Camera.LeftAxis, client.Self.Movement.Camera.UpAxis,
            //        client.Self.Movement.BodyRotation, client.Self.Movement.HeadRotation, client.Self.Movement.Camera.Far, AgentFlags.None,
            //        AgentState.None, true);

            client.Self.Movement.AutoResetControls = false;
            client.Self.Movement.AtNeg = true;
            client.Self.Movement.SendUpdate();
        }

        private void lft()
        {   
            client.Self.Movement.SendManualUpdate(AgentManager.ControlFlags.AGENT_CONTROL_LEFT_POS, client.Self.Movement.Camera.Position,
                    client.Self.Movement.Camera.AtAxis, client.Self.Movement.Camera.LeftAxis, client.Self.Movement.Camera.UpAxis,
                    client.Self.Movement.BodyRotation, client.Self.Movement.HeadRotation, client.Self.Movement.Camera.Far, AgentFlags.None,
                    AgentState.None, true);
        }

        private void rgt()
        {
            client.Self.Movement.SendManualUpdate(AgentManager.ControlFlags.AGENT_CONTROL_LEFT_NEG, client.Self.Movement.Camera.Position,
                    client.Self.Movement.Camera.AtAxis, client.Self.Movement.Camera.LeftAxis, client.Self.Movement.Camera.UpAxis,
                    client.Self.Movement.BodyRotation, client.Self.Movement.HeadRotation, client.Self.Movement.Camera.Far, AgentFlags.None,
                    AgentState.None, true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            client.Self.AnimationStart(Animations.TURNLEFT, false); 

            ahead += 45.0;
            if (ahead > 360) ahead = 135.0;

            client.Self.Movement.UpdateFromHeading(ahead, true);

            client.Self.Movement.FinishAnim = true;
            System.Threading.Thread.Sleep(200);    
            client.Self.AnimationStop(Animations.TURNLEFT, false);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (instance.State.IsFlying)
            {
                flying = false;
            }
            else
            {
                flying = true;
            }

            instance.State.SetFlying(flying);
        }

        //private void timer2_Tick(object sender, EventArgs e)
        //{
        //    CheckAutoSit();
        //}

        private void button8_Click(object sender, EventArgs e)
        {
            client.Self.AnimationStart(Animations.TURNRIGHT, false);

            ahead += -45.0;
            if (ahead > 360) ahead = 135.0;


            client.Self.Movement.UpdateFromHeading(ahead, true);

            client.Self.Movement.FinishAnim = true;
            System.Threading.Thread.Sleep(200);
            client.Self.AnimationStop(Animations.TURNRIGHT, false);
        }

        private void shoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessChatInput(cbxInput.Text, ChatType.Shout);
        }

        private void whisperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessChatInput(cbxInput.Text, ChatType.Whisper);
        }

        private void clearChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbChat.Clear();
            chatManager.ClearInternalBuffer();
        }

        private void tbSay_DropDownOpening(object sender, EventArgs e)
        {
            sayopen = true;
        }

        private void tbSay_DropDownClosed(object sender, EventArgs e)
        {
            sayopen = false;
        }

        private void sayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessChatInput(cbxInput.Text, ChatType.Normal);
        }

        private void SaveChat()
        {
            // Create a SaveFileDialog to request a path and file name to save to.
            SaveFileDialog saveFile1 = new SaveFileDialog();

            string logdir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt";
            logdir += "\\Logs\\";

            saveFile1.InitialDirectory = logdir;

            // Initialize the SaveFileDialog to specify the RTF extension for the file.
            saveFile1.DefaultExt = "*.rtf";
            saveFile1.Filter = "txt files (*.txt)|*.txt|RTF Files (*.rtf)|*.rtf";  //"RTF Files|*.rtf";
            saveFile1.Title = "Save chat contents to hard disk...";

            // Determine if the user selected a file name from the saveFileDialog.
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
               saveFile1.FileName.Length > 0)
            {
                if (saveFile1.FileName.Substring(saveFile1.FileName.Length - 3) == "rtf")
                {
                    // Save the contents of the RichTextBox into the file.
                    rtbChat.SaveFile(saveFile1.FileName, RichTextBoxStreamType.RichText);
                }
                else
                {
                    rtbChat.SaveFile(saveFile1.FileName, RichTextBoxStreamType.PlainText);
                }
            }

            saveFile1.Dispose();  
        }

        private void saveChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveChat();
        }

        private void tbChat_Click(object sender, EventArgs e)
        {
            if (!saveopen)
            {
                SaveChat();
            }
        }

        private void tbSay_Click(object sender, EventArgs e)
        {
            if (!sayopen)
            {
                ProcessChatInput(cbxInput.Text, ChatType.Normal);
            }
        }

        private void tbChat_DropDownOpening(object sender, EventArgs e)
        {
            saveopen = true;
        }

        private void tbChat_DropDownClosed(object sender, EventArgs e)
        {
            saveopen = false;
        }

        private void cboLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tsMovie_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@instance.Config.CurrentConfig.mURL);
        }

        private void tbar_SendMessage_ButtonClick_2(object sender, ToolBarButtonClickEventArgs e)
        {

        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            (new frmPlayer(instance)).Show();
        }

        #region Minimap
        // Seperate thread
        private void Grid_OnCoarseLocationUpdate(object sender, CoarseLocationUpdateEventArgs e)
        {
            if (e.Simulator != client.Network.CurrentSim) return;

            if (InvokeRequired)
            {

                BeginInvoke(new MethodInvoker(delegate()
                {
                    Grid_OnCoarseLocationUpdate(sender, e);
                }));
                
                return;
            }

            List<UUID> tremove = e.RemovedEntries;

            foreach (UUID id in tremove)
            {
                foreach (ListViewItem litem in lvwRadar.Items)
                {
                    if (litem.Tag.ToString() == id.ToString())
                    {
                        lvwRadar.BeginUpdate();
                        lvwRadar.Items.RemoveAt(lvwRadar.Items.IndexOf(litem));
                        lvwRadar.EndUpdate();
                    }
                } 
            }

            try
            {
                BeginInvoke((MethodInvoker)delegate { UpdateMiniMap(e.Simulator); });
                BeginInvoke((MethodInvoker)delegate { GetCompass(); });
            }
            catch { ; } 
        }

        // Seperate thread
        void Assets_OnImageReceived(TextureRequestState image, AssetTexture texture)
        {
            if (texture.AssetID == _MapImageID)
            {
                ManagedImage nullImage;
                OpenJPEG.DecodeToImage(texture.AssetData, out nullImage, out _MapLayer);

                BeginInvoke((MethodInvoker)delegate { UpdateMiniMap(sim); });
            }
        }

        // Seperate thread
        void Network_OnCurrentSimChanged(object sender, SimChangedEventArgs e)
        {
            lock (sfavatar)
            {
                sfavatar.Clear();
            }

            BeginInvoke(new MethodInvoker(delegate()
            {
                lvwRadar.Clear();
            }));

            //GetMap();
            BeginInvoke((MethodInvoker)delegate { GetMap(); });
        }

        private void GetMap()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    GetMap();
                }));

                return;
            }

            GridRegion region;

            if (_MapLayer == null || sim != client.Network.CurrentSim)
            {
                sim = client.Network.CurrentSim;
                label8.Text = client.Network.CurrentSim.Name;

                if (client.Grid.GetGridRegion(client.Network.CurrentSim.Name, GridLayerType.Objects, out region))
                {
                    if (region.MapImageID != UUID.Zero)
                    {
                        _MapImageID = region.MapImageID;
                        client.Assets.RequestImage(_MapImageID, ImageType.Baked, Assets_OnImageReceived);
                    }
                    else
                    {
                        if (client.Grid.GetGridRegion(client.Network.CurrentSim.Name, GridLayerType.Terrain, out region))
                        {
                            if (region.MapImageID != UUID.Zero)
                            {
                                _MapImageID = region.MapImageID;
                                client.Assets.RequestImage(_MapImageID, ImageType.Baked, Assets_OnImageReceived);
                            }
                        }
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
            if (this.InvokeRequired) this.BeginInvoke((MethodInvoker)delegate { UpdateMiniMap(sim); });
            else
            {
                try
                {
                    if (ssim != client.Network.CurrentSim) return;

                    // SIM stats
                    Simulator sims = sim = ssim;   // client.Network.Simulators[0];

                    Bitmap newbmp = new Bitmap(256, 256);
                    Bitmap bmp = _MapLayer == null ? newbmp : (Bitmap)_MapLayer.Clone();
                    Graphics g = Graphics.FromImage(bmp);

                    newbmp.Dispose(); 

                    if (_MapLayer == null)
                    {
                        g.Clear(this.BackColor);
                        g.FillRectangle(Brushes.White, 0f, 0f, 256f, 256f);
                        label11.Visible = true;
                    }
                    else
                    {
                        label11.Visible = false;
                    }                    

                    try
                    {
                        //sims = client.Network.Simulators[0];

                        label4.Text = "Ttl objects: " + sims.Stats.Objects.ToString(CultureInfo.CurrentCulture);
                        label5.Text = "Scripted objects: " + sims.Stats.ScriptedObjects.ToString(CultureInfo.CurrentCulture);
                        label8.Text = client.Network.CurrentSim.Name;

                        Simulator csim = client.Network.CurrentSim;

                        label9.Text = "FPS: " + csim.Stats.FPS.ToString(CultureInfo.CurrentCulture);

                        // Maximum value changes for OSDGrid compatibility V 0.9.32.0

                        if (csim.Stats.FPS > progressBar7.Maximum)
                        {
                            progressBar7.Maximum = csim.Stats.FPS;
                        }

                        progressBar7.Value = csim.Stats.FPS;

                        label15.Text = "Dilation: " + csim.Stats.Dilation.ToString(CultureInfo.CurrentCulture);

                        if ((int)(csim.Stats.Dilation * 10) > progressBar1.Maximum)
                        {
                            progressBar1.Maximum = (int)(csim.Stats.Dilation * 10);
                        }

                        progressBar1.Value = (int)(csim.Stats.Dilation * 10);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Chatconsole MiniMap - Stats: " + ex.Message, Helpers.LogLevel.Error);
                    }

                    // V0.9.8.0 changes for OpenSIM compatibility
                    Vector3 myPos = new Vector3(0,0,0);
                    string strInfo = string.Empty;

                    myPos = instance.SIMsittingPos();

                    if (!sim.AvatarPositions.ContainsKey(client.Self.AgentID))
                    {
                        label12.Text = sims.SimVersion;
                        strInfo = string.Format(CultureInfo.CurrentCulture,"Ttl Avatars: {0}", client.Network.CurrentSim.AvatarPositions.Count + 1);
                    }
                    else
                    {
                        try
                        {
                            //myPos = sim.AvatarPositions[client.Self.AgentID];

                            try
                            {
                                string[] svers = sims.SimVersion.Split(' ');
                                var e = from s in svers
                                        select s;

                                int cnt = e.Count() - 1;

                                //label12.Text = sims.SimVersion.Remove(0, 18);
                                label12.Text = svers[cnt]; 
                            }
                            catch
                            {
                                label12.Text = "na";
                            }

                            strInfo = string.Format(CultureInfo.CurrentCulture,"Ttl Avatars: {0}", client.Network.CurrentSim.AvatarPositions.Count);
                        }
                        catch { ; }
                    }

                    label6.Text = strInfo;

                    int i = 0;

                    instance.avlocations.Clear();

                    client.Network.CurrentSim.AvatarPositions.ForEach(
                    delegate(KeyValuePair<UUID, Vector3> pos)
                    {
                        if (!instance.avnames.ContainsKey(pos.Key))
                        {
                            client.Avatars.RequestAvatarName(pos.Key);
                        }

                        int x = (int)pos.Value.X - 2;
                        int y = 255 - (int)pos.Value.Y - 2;

                        rect = new Rectangle(x, y, 7, 7);

                        if (pos.Key != client.Self.AgentID)
                        {
                            if (pos.Value.Z < 0.1f)
                            {
                                g.FillRectangle(Brushes.MediumBlue, rect);
                                g.DrawRectangle(new Pen(Brushes.Red, 1), rect);
                            }
                            else
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

                            Point mouse = new Point(x, y);
                            string anme = string.Empty; 

                            try
                            {
                                if (instance.avtags.ContainsKey(pos.Key))
                                {
                                    anme = instance.avtags[pos.Key];
                                }
                                
                                instance.avlocations.Add(new METAboltInstance.AvLocation(mouse, rect.Size, pos.Key.ToString(), anme, pos.Value));

                                Avatar fav = sim.ObjectsAvatars.Find((Avatar av) => { return av.ID == pos.Key; });

                                if (fav == null)
                                {
                                    if (instance.avnames.ContainsKey(pos.Key))
                                    {
                                        string name = instance.avnames[pos.Key];

                                        BeginInvoke(new OnAddSIMAvatar(AddSIMAvatar), new object[] { name, pos.Key, pos.Value });
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Log("UpdateMiniMap: " + ex.Message, Helpers.LogLevel.Warning);   
                                //reporter.Show(ex);
                            }
                        }

                        i++;
                    }
                    );

                    try
                    {
                        lock (sfavatar)
                        {
                            foreach (KeyValuePair<uint, Avatar> av in sfavatar)
                            {
                                Avatar sav = av.Value;

                                if (!sim.AvatarPositions.ContainsKey(sav.ID))
                                {
                                    sfavatar.Remove(sav.LocalID);

                                    if (lvwRadar.Items.ContainsKey(sav.Name))
                                    {
                                        lvwRadar.BeginUpdate();
                                        lvwRadar.Items.RemoveByKey(sav.Name);
                                        lvwRadar.EndUpdate();
                                    }

                                    if (instance.avtags.ContainsKey(sav.ID))
                                    {
                                        instance.avtags.Remove(sav.ID);
                                    }
                                }
                                else
                                {
                                    BeginInvoke(new OnAddAvatar(AddAvatar), new object[] { sav });
                                }
                            }
                        }
                    }
                    catch { ; }

                    // Draw self position
                    Rectangle myrect = new Rectangle((int)Math.Round(myPos.X, 0) - 2, 255 - ((int)Math.Round(myPos.Y, 0) - 2), 7, 7);

                    SolidBrush sb = new SolidBrush(Color.Yellow);
                    Pen pen = new Pen(Brushes.Red, 3);

                    g.FillEllipse(sb, myrect);
                    g.DrawEllipse(pen, myrect);

                    g.DrawImage(bmp, 0, 0);
                    sb.Dispose();
                    pen.Dispose();  

                    // Draw compass points
                    StringFormat strFormat = new StringFormat();
                    strFormat.Alignment = StringAlignment.Center;

                    //myrect = new Rectangle((bmp.Height / 2) - 10, 0, 20, 20);
                    //g.FillEllipse(new SolidBrush(Color.Black), myrect);
                    //g.DrawEllipse(new Pen(Brushes.Red, 2), myrect);

                    Font font1 = new Font("Arial", 13);
                    Font font2 = new Font("Arial", 10, FontStyle.Bold);

                    g.DrawString("N", font1, Brushes.Black, new RectangleF(0, 2, bmp.Width, bmp.Height), strFormat);
                    g.DrawString("N", font2, Brushes.White, new RectangleF(0, 2, bmp.Width, bmp.Height), strFormat);

                    strFormat.LineAlignment = StringAlignment.Center;
                    strFormat.Alignment = StringAlignment.Near;

                    g.DrawString("W", font1, Brushes.Black, new RectangleF(0, 0, bmp.Width, bmp.Height), strFormat);
                    g.DrawString("W", font2, Brushes.White, new RectangleF(2, 0, bmp.Width, bmp.Height), strFormat);

                    strFormat.LineAlignment = StringAlignment.Center;
                    strFormat.Alignment = StringAlignment.Far;

                    g.DrawString("E", font1, Brushes.Black, new RectangleF(0, 0, bmp.Width, bmp.Height), strFormat);
                    g.DrawString("E", font2, Brushes.White, new RectangleF(-2, 0, bmp.Width, bmp.Height), strFormat);

                    strFormat.LineAlignment = StringAlignment.Far;
                    strFormat.Alignment = StringAlignment.Center;

                    g.DrawString("S", font1, Brushes.Black, new RectangleF(0, 0, bmp.Width, bmp.Height), strFormat);
                    g.DrawString("S", font2, Brushes.White, new RectangleF(0, 0, bmp.Width, bmp.Height), strFormat);

                    world.Image = bmp;
                    //world.Cursor = Cursors.NoMove2D;

                    font1.Dispose();
                    font2.Dispose();  
                    g.Dispose();

                    GetCompass();
                }
                catch (Exception ex)
                {
                    Logger.Log("Chatconsole MiniMap: " + ex.Message, Helpers.LogLevel.Error);
                    return;
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                if (!instance.LoggedIn) return;

                label6.Text = string.Empty;
                label4.Text = string.Empty;
                label5.Text = string.Empty;
                label8.Text = string.Empty;
                label9.Text = string.Empty;
                label15.Text = string.Empty;

                client.Grid.RequestMapRegion(client.Network.CurrentSim.Name, GridLayerType.Objects);

                //GetMap();
                BeginInvoke((MethodInvoker)delegate { GetMap(); });
                toolStrip1.Visible = false;
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                toolStrip1.Visible = false;
            }
            else
            {
                toolStrip1.Visible = true;
            }
        }
        #endregion

        private void world_MouseDown(object sender, MouseEventArgs e)
        {
            if (instance.MainForm.WindowState == FormWindowState.Maximized) return;
           
            if (e.Button != MouseButtons.Left)
                return;
            px = world.Top;
            py = world.Left;

            world.Width = newsize * 2;
            world.Height = newsize * 2; 

            move = true;
            rect.X = e.X;
            rect.Y = e.Y;
        }

        private int NormaliseSize(int number)
        {
            decimal ssize = (decimal)256 / (decimal)panel6.Width;

            int pos = Convert.ToInt32(Math.Round(number * ssize));

            return pos;
        }

        private void world_MouseUp(object sender, MouseEventArgs e)
        {
            //decimal ssize = (decimal)256 / (decimal)panel6.Width;

            int posX = NormaliseSize(e.X);   // Convert.ToInt32(Math.Round(e.X * ssize));
            int posY = NormaliseSize(e.Y);   // Convert.ToInt32(Math.Round(e.Y * ssize));

            Point mouse = new Point(posX, posY);

            METAboltInstance.AvLocation CurrentLoc = null;

            try
            {
                CurrentLoc = instance.avlocations.Find(delegate(METAboltInstance.AvLocation g) { return g.Rectangle.Contains(mouse) == true; });
            }
            catch { ; }

            if (CurrentLoc != null)
            {
                (new frmProfile(instance, avname, avuuid)).Show();
            }

            if (instance.MainForm.WindowState == FormWindowState.Maximized) return;

            move = false;

            world.Width = newsize;
            world.Height = newsize;

            world.Top = px;
            world.Left = py;
        }

        private void world_MouseMove(object sender, MouseEventArgs e)
        {
            int posX = NormaliseSize(e.X);
            int posY = NormaliseSize(e.Y);

            Point mouse = new Point(posX, posY);

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

                    string apstn = "\nCoords.: " + CurrentLoc.Position.X.ToString(CultureInfo.CurrentCulture) + "/" + CurrentLoc.Position.Y.ToString(CultureInfo.CurrentCulture) + "/" + CurrentLoc.Position.Z.ToString(CultureInfo.CurrentCulture);

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
                world.Cursor = Cursors.NoMove2D;
                toolTip1.RemoveAll();
                showing = false;
            }


            if (instance.MainForm.WindowState == FormWindowState.Maximized) return;

            if (e.Button != MouseButtons.Left)
                return;

            if (move)
            {
                world.Left += e.X - rect.X;
                world.Top += e.Y - rect.Y;
            }
        }

        private void tbtnAttachments_Click(object sender, EventArgs e)
        {
            //Avatar av = ((ListViewItem)lvwRadar.SelectedItems[0]).Tag as Avatar;
            //if (av == null) return;

            UUID av = (UUID)lvwRadar.SelectedItems[0].Tag;

            if (av == UUID.Zero || av == null) return;

            //string name = instance.avnames[av];

            Avatar sav = client.Network.CurrentSim.ObjectsAvatars.Find(delegate(Avatar fa)
            {
                return fa.ID == av;
            }
            );

            if (sav != null)
            {
                (new WornAttachments(instance, sav)).Show(this);
            }
            else
            {
                MessageBox.Show("Avatar is out of range for this function.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);  
            }
        }

        private void lvwRadar_DoubleClick(object sender, EventArgs e)
        {
            if (lvwRadar.SelectedItems.Count == 1)
            {
                tbtnStartIM.PerformClick();
            }
        }

        private void lvwRadar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwRadar.SelectedItems.Count == 0)
            {
                tbtnTurn.Enabled =
                tbtnFollow.Enabled =
                tbtnStartIM.Enabled =
                tbtnGoto.Enabled =
                tbtnAddFriend.Enabled =
                tbtnFreeze.Enabled =
                tbtnBan.Enabled =
                tbtnEject.Enabled =
                tbtnAttachments.Enabled =
                tbtnProfile.Enabled = false;

                selectedname = string.Empty;
            }
            else
            {
                tbtnAttachments.Enabled = tbtnProfile.Enabled = true;

                tbtnTurn.Enabled =
                tbtnFollow.Enabled =
                tbtnStartIM.Enabled =
                tbtnGoto.Enabled =
                tbtnAddFriend.Enabled =
                tbtnFreeze.Enabled =
                tbtnBan.Enabled =
                tbtnEject.Enabled =

                (lvwRadar.SelectedItems[0].Name != client.Self.Name);

                selectedname = lvwRadar.SelectedItems[0].Name;
            }
        }

        private void lvwRadar_Leave(object sender, EventArgs e)
        {
            lvwRadar.SelectedItems.Clear();

            tbtnTurn.Enabled =
                tbtnFollow.Enabled =
                tbtnStartIM.Enabled =
                tbtnGoto.Enabled =
                tbtnAddFriend.Enabled =
                tbtnFreeze.Enabled =
                tbtnBan.Enabled =
                tbtnEject.Enabled =
                tbtnAttachments.Enabled =
                tbtnProfile.Enabled = false;

            //if (!instance.Config.CurrentConfig.iRadar)
            //{
            //    UpdateRadar();
            //}
        }

        private void world_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        //private void LinkClicked(object sender, EventArgs e)
        //{
        //    HtmlElement link = webBrowser1.Document.ActiveElement;
        //    clickedurl = link.GetAttribute("href");
        //}

        //private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(clickedurl))
        //    {
        //        HtmlElement link = webBrowser1.Document.ActiveElement;
        //        clickedurl = link.GetAttribute("href");
        //    }

        //    e.Cancel = true;

        //    if (clickedurl.StartsWith("http://slurl.", StringComparison.CurrentCulture))
        //    {
        //        // Open up the TP form here
        //        string[] split = clickedurl.Split(new Char[] { '/' });
        //        string ssim = split[4].ToString();
        //        double x = Convert.ToDouble(split[5].ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        //        double y = Convert.ToDouble(split[6].ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        //        double z = Convert.ToDouble(split[7].ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);

        //        (new frmTeleport(instance, ssim, (float)x, (float)y, (float)z)).ShowDialog();
        //        clickedurl = string.Empty;
        //        return;
        //    }
        //    if (clickedurl.StartsWith("http://maps.secondlife", StringComparison.CurrentCulture))
        //    {
        //        // Open up the TP form here
        //        string[] split = clickedurl.Split(new Char[] { '/' });
        //        string ssim = split[4].ToString();
        //        double x = Convert.ToDouble(split[5].ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        //        double y = Convert.ToDouble(split[6].ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        //        double z = Convert.ToDouble(split[7].ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);

        //        (new frmTeleport(instance, ssim, (float)x, (float)y, (float)z)).ShowDialog();
        //        clickedurl = string.Empty;
        //        return;
        //    }

        //    System.Diagnostics.Process.Start(clickedurl);
        //    clickedurl = string.Empty;  
        //}

        private void cbxInput_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void rtbChat_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter) e.SuppressKeyPress = true;

            //if (rtbChat.SelectedText == string.Empty || rtbChat.SelectedText == null) return; 

            //if (e.Control && e.KeyCode == Keys.C)
            //{
            //    Clipboard.Clear();
            //    Clipboard.SetText(rtbChat.SelectedText, TextDataFormat.UnicodeText); 
            //}
        }

        private void ChatConsole_SizeChanged(object sender, EventArgs e)
        {
            if (!formloading)
            {
                formloading = true;
                return;
            }            

            newsize = tabPage2.Width - 30;

            px = world.Top;
            py = world.Left;

            System.Drawing.Size sz = new Size();
            sz.Height = newsize;
            sz.Width = newsize;

            panel6.Size = sz;

            //if (instance.MainForm.WindowState == FormWindowState.Maximized)
            //{
            //    px = world.Top;
            //    py = world.Left;

            //    System.Drawing.Size sz = new Size();
            //    sz.Height = 256;
            //    sz.Width = 256;

            //    panel6.Size = sz;
            //}
            //else
            //{
            //    System.Drawing.Size sz = new Size();
            //    sz.Height = 140;
            //    sz.Width = 140;

            //    panel6.Size = sz;

            //    world.Top = px;
            //    world.Left = py;
            //}
        }

        private void lvwRadar_KeyUp(object sender, KeyEventArgs e)
        {
            if (lvwRadar.SelectedItems.Count < 1) return;
 
            if (e.Control && e.Alt && e.KeyCode == Keys.I)
            {
                tbtnStartIM.PerformClick();
            }

            if (e.Control && e.Alt && e.KeyCode == Keys.P)
            {
                tbtnProfile.PerformClick();
            }

            if (e.Control && e.Alt && e.KeyCode == Keys.A)
            {
                tbtnAttachments.PerformClick();
            }

            if (e.Control && e.Alt && e.KeyCode == Keys.F)
            {
                tbtnAddFriend.PerformClick();
            }

            if (e.Control && e.Alt && e.KeyCode == Keys.T)
            {
                tbtnTurn.PerformClick();
            }

            if (e.Control && e.Alt && e.KeyCode == Keys.W)
            {
                tbtnFollow.PerformClick();
            }

            if (e.Control && e.Alt && e.KeyCode == Keys.G)
            {
                tbtnGoto.PerformClick();
            }

            if (e.Control && e.Alt && e.KeyCode == Keys.E)
            {
                tbtnFreeze.PerformClick();
            }

            if (e.Control && e.Alt && e.KeyCode == Keys.J)
            {
                tbtnEject.PerformClick();
            }

            if (e.Control && e.Alt && e.KeyCode == Keys.B)
            {
                tbtnBan.PerformClick();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // All this could go into the extended rtb component in the future

            int startindex = 0;

            if (!string.IsNullOrEmpty(prevsearchtxt))
            {
                if (prevsearchtxt != tsFindText.Text.Trim())
                {
                    startindex = 0;
                    start = 0;
                    indexOfSearchText = 0;
                }
            }

            prevsearchtxt = tsFindText.Text.Trim();

            //int linenumber = rtbScript.GetLineFromCharIndex(rtbScript.SelectionStart) + 1;
            //Point pnt = rtbScript.GetPositionFromCharIndex(rtbScript.SelectionStart);

            if (tsFindText.Text.Length > 0)
                startindex = FindNext(tsFindText.Text.Trim(), start, rtbChat.Text.Length);

            // If string was found in the RichTextBox, highlight it
            if (startindex > 0)
            {
                // Set the highlight color as red
                rtbChat.SelectionColor = Color.LightBlue;
                // Find the end index. End Index = number of characters in textbox
                int endindex = tsFindText.Text.Length;
                // Highlight the search string
                rtbChat.Select(startindex, endindex);
                // mark the start position after the position of 
                // last search string
                start = startindex + endindex;

                if (start == rtbChat.TextLength || start > rtbChat.TextLength)
                {
                    startindex = 0;
                    start = 0;
                    indexOfSearchText = 0;
                }
            }
            else if (startindex == -1)
            {
                startindex = 0;
                start = 0;
                indexOfSearchText = 0;
            }
        }

        public int FindNext(string txtToSearch, int searchStart, int searchEnd)
        {
            // Unselect the previously searched string
            if (searchStart > 0 && searchEnd > 0 && indexOfSearchText >= 0)
            {
                rtbChat.Undo();
            }

            // Set the return value to -1 by default.
            int retVal = -1;

            // A valid starting index should be specified.
            // if indexOfSearchText = -1, the end of search
            if (searchStart >= 0 && indexOfSearchText >= 0)
            {
                // A valid ending index 
                if (searchEnd > searchStart || searchEnd == -1)
                {
                    // Determine if it's a match case or what
                    RichTextBoxFinds mcase = RichTextBoxFinds.None;

                    if (checkBox1.Checked)
                    {
                        mcase = RichTextBoxFinds.MatchCase;
                    }


                    if (checkBox2.Checked)
                    {
                        mcase |= RichTextBoxFinds.WholeWord;
                    }

                    // Find the position of search string in RichTextBox
                    indexOfSearchText = rtbChat.Find(txtToSearch, searchStart, searchEnd, mcase);
                    // Determine whether the text was found in richTextBox1.
                    if (indexOfSearchText != -1)
                    {
                        // Return the index to the specified search text.
                        retVal = indexOfSearchText;
                    }
                }
            }

            return retVal;
        }

        private void toolStripButton1_Click_2(object sender, EventArgs e)
        {
            if (panel7.Visible)
            {
                panel7.Visible = false;
                tsbSearch.ToolTipText = "Show chat search";
                rtbChat.Height += 23; 
            }
            else
            {
                panel7.Visible = true;
                tsbSearch.ToolTipText = "Hide chat search";
                rtbChat.Height -= 23;
            }
        }

        private void tsFindText_Click(object sender, EventArgs e)
        {
            tsFindText.SelectionStart = 0;
            tsFindText.SelectionLength = tsFindText.Text.Length;
        }

        private void vgate_OnVoiceConnectionChange(VoiceGateway.ConnectionState state)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                string s = string.Empty;
  
                if (state == VoiceGateway.ConnectionState.AccountLogin)
                {
                    s = "Logging In";
                }
                else if (state == VoiceGateway.ConnectionState.ConnectorConnected)
                {
                    s = "Connected";
                }
                else if (state == VoiceGateway.ConnectionState.DaemonConnected)
                {
                    s = "Daemon Connected";
                }
                else if (state == VoiceGateway.ConnectionState.DaemonStarted)
                {
                    s = "Daemon Started";
                }
                else if (state == VoiceGateway.ConnectionState.SessionRunning)
                {
                    s = "Session Started";
                }

                label18.Text = s; 
            }));
        }

        private void vgate_OnAuxGetCaptureDevicesResponse(object sender, VoiceGateway.VoiceDevicesEventArgs e)
        {
            BeginInvoke(new MethodInvoker(() => LoadMics(e.Devices)));
        }

        private void vgate_OnAuxGetRenderDevicesResponse(object sender, VoiceGateway.VoiceDevicesEventArgs e)
        {
            BeginInvoke(new MethodInvoker(() => LoadSpeakers(e.Devices)));
        }

        private void vgate_OnSessionCreate(object sender, EventArgs e)
        {
            vgate.AuxGetCaptureDevices();
            vgate.AuxGetRenderDevices();

            BeginInvoke(new MethodInvoker(delegate()
            {
                vgate.MicMute = true;
                vgate.SpkrMute = false;
                vgate.SpkrLevel = 70;
                vgate.MicLevel = 70;
                checkBox5.ForeColor = Color.Red;
                label18.Text = "Session started";
                EnableVoice(true);

                if (!checkBox3.Checked)
                {
                    checkBox3.Checked = true;
                }
            }));
        }

        private void LoadMics(List<string> list)
        {
            try 
            {
                cboCapture.Items.Clear();

                foreach (string dev in list)
                {
                    cboCapture.Items.Add(dev);  
                }
            }
            catch { ; }

            string cmic = vgate.CurrentCaptureDevice;

            if (string.IsNullOrEmpty(cmic))
            {
                cboCapture.SelectedItem = cmic;    //cmic = mics[0];
            }
            else
            {
                cboCapture.Text = cmic;
            }

            vgate.MicMute = true;
            vgate.MicLevel = 70;
            cmic = vgate.CurrentCaptureDevice;
        }

        private void LoadSpeakers(List<string> list)
        {
            try
            {
                cboRender.Items.Clear();
   
                foreach (string dev in list)
                {
                    cboRender.Items.Add(dev);   
                }
            }
            catch { ; }

            string cspk = vgate.PlaybackDevice;

            if (string.IsNullOrEmpty(cspk))
            {
                cboRender.SelectedItem = cspk; //speakers[0];
            }
            else
            {
                cboRender.Text = cspk;
            }

            vgate.SpkrMute = false;
            vgate.SpkrLevel = 70;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            vgate.MicMute = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            vgate.SpkrMute = checkBox4.Checked;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            
        }

        private bool CheckVoiceSetupFile(string filename)
        {
            if (!System.IO.File.Exists(Application.StartupPath.ToString() + "\\" + filename))
            {
                MessageBox.Show("The required '" + filename + "' file was not found.\nPlease read the wiki page below\nfor instructions:\n\nhttp://www.metabolt.net/METAwiki/How-to-enable-VOICE.ashx?NoRedirect=1");
                return(false);
            }
            return (true);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                if (!this.CheckVoiceSetupFile("SLVoice.exe")) return;
                if (!this.CheckVoiceSetupFile("alut.dll")) return;
                if (!this.CheckVoiceSetupFile("ortp.dll")) return;
                if (!this.CheckVoiceSetupFile("vivoxsdk.dll")) return;
                if (!this.CheckVoiceSetupFile("wrap_oal.dll")) return;

                vgate = new VoiceGateway(client);
                vgate.OnVoiceConnectionChange += new VoiceGateway.VoiceConnectionChangeCallback(vgate_OnVoiceConnectionChange);
                vgate.OnAuxGetCaptureDevicesResponse += new EventHandler<VoiceGateway.VoiceDevicesEventArgs>(vgate_OnAuxGetCaptureDevicesResponse);
                vgate.OnAuxGetRenderDevicesResponse += new EventHandler<VoiceGateway.VoiceDevicesEventArgs>(vgate_OnAuxGetRenderDevicesResponse);
                vgate.OnSessionCreate += new EventHandler(vgate_OnSessionCreate);

                vgate.Start();
                //voiceon = true;
            }
            else
            {
                try
                {
                    vgate.MicMute = true;
                    vgate.Stop();
                    EnableVoice(false);
                    cboRender.Items.Clear();
                    cboCapture.Items.Clear();   

                    vgate.OnVoiceConnectionChange -= new VoiceGateway.VoiceConnectionChangeCallback(vgate_OnVoiceConnectionChange);
                    vgate.OnAuxGetCaptureDevicesResponse -= new EventHandler<VoiceGateway.VoiceDevicesEventArgs>(vgate_OnAuxGetCaptureDevicesResponse);
                    vgate.OnAuxGetRenderDevicesResponse -= new EventHandler<VoiceGateway.VoiceDevicesEventArgs>(vgate_OnAuxGetRenderDevicesResponse);
                    vgate.OnSessionCreate -= new EventHandler(vgate_OnSessionCreate);

                    
                    //voiceon = false;

                    if (!checkBox3.Checked)
                    {
                        checkBox3.Checked = true;
                    }

                    checkBox5.ForeColor = Color.Black;
                    label18.Text = string.Empty;
                }
                catch
                {
                    ;
                }
            }
        }

        private void EnableVoice(bool ebl)
        {
            cboCapture.Enabled = ebl;
            cboRender.Enabled = ebl;
            trackBar1.Enabled = ebl;
            trackBar2.Enabled = ebl;
            checkBox3.Enabled = ebl;
            checkBox4.Enabled = ebl; 
        }

        private void cboCapture_SelectedIndexChanged(object sender, EventArgs e)
        {
            vgate.CurrentCaptureDevice = cboCapture.SelectedItem.ToString();  
        }

        private void cboRender_SelectedIndexChanged(object sender, EventArgs e)
        {
            vgate.PlaybackDevice = cboRender.SelectedItem.ToString(); 
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            vgate.MicLevel = trackBar1.Value;
            toolTip1.SetToolTip(trackBar1, "Volume: " + trackBar1.Value.ToString(CultureInfo.CurrentCulture)); 
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            vgate.SpkrLevel = trackBar2.Value;
            toolTip1.SetToolTip(trackBar2, "Volume: " + trackBar2.Value.ToString(CultureInfo.CurrentCulture));
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (!avrezzed && netcom.IsLoggedIn)
            {
                client.Appearance.RequestSetAppearance(false);
                timer2.Enabled = false;
                timer2.Stop();
            }  
        }

        private void toolStripButton1_Click_3(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://www.metabolt.net/metawiki/Quick.aspx");
        }

        private void picAutoSit_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://www.metabolt.net/metawiki/How-to-enable-VOICE.ashx");
        }

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            toolTip.Show(pictureBox1);
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Close();
        }

        private void ChatConsole_Load(object sender, EventArgs e)
        {
            
        }
    }
}
