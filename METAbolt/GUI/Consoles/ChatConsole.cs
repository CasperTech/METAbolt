//  Copyright (c) 2008 - 2013, www.metabolt.net (METAbolt)
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
//using GoogleTranslationUtils;
//using MB_Translation_Utils;
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
using PopupControl;
using NHunspell;
//using System.Runtime.InteropServices;


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
        //private double ahead = 0.0;
        private bool flying = false;
        private bool sayopen = false;
        private bool saveopen = false;
        private UUID _MapImageID;
        private Image _MapLayer;
        private int px = 0;
        private int py =0;
        public Simulator sim;
        private Rectangle rect;
        private bool move = false;
        private string selectedname = string.Empty;
        //private Vector3 vDir;
        //private string clickedurl = string.Empty;
        private bool avrezzed = false;
        //private bool pasted = false;
        private uint[] localids;
        private int newsize = 140;
        //private bool listnerdisposed = true;
        private System.Timers.Timer sitTimer;
        //private System.Timers.Timer tpTimer;
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
        private VoiceGateway vgate = null;

        private const int WM_KEYUP = 0x101;
        private const int WM_KEYDOWN = 0x100;

        private Popup tTip;
        private Popup tTip1;
        private CustomToolTip customToolTip;

        private Hunspell hunspell = new Hunspell();
        private string afffile = string.Empty;
        private string dicfile = string.Empty;
        private string dic = string.Empty;
        private string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt\\Spelling\\";

        private ToolTip toolTip = new ToolTip();
        private string tooltiptext = string.Empty;
        private Simulator CurrentSIM;
        private Vector3 lastPos = new Vector3(0, 0, 0);
        private TabPage tp1 = new TabPage();
        private TabPage tp2 = new TabPage();
        private TabPage tp3 = new TabPage();
        private TabPage tp4 = new TabPage();
        private Form tpf;


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
            try
            {
                InitializeComponent();
            }
            catch { ; }

            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.instance = instance;
            netcom = this.instance.Netcom;
            client = this.instance.Client;

            AddNetcomEvents();

            chatManager = new ChatTextManager(instance, new RichTextBoxPrinter(instance, rtbChat));
            chatManager.PrintStartupMessage();

            afffile = this.instance.AffFile = instance.Config.CurrentConfig.SpellLanguage + ".aff";   // "en_GB.aff";
            dicfile = this.instance.DictionaryFile = instance.Config.CurrentConfig.SpellLanguage + ".dic";   // "en_GB.dic";

            this.instance.MainForm.Load += new EventHandler(MainForm_Load);

            ApplyConfig(this.instance.Config.CurrentConfig);
            this.instance.Config.ConfigApplied += new EventHandler<ConfigAppliedEventArgs>(Config_ConfigApplied);

            CreateSmileys();
            //AddLanguages();

            Disposed += new EventHandler(ChatConsole_Disposed);

            lvwRadar.ListViewItemSorter = new RadarSorter();

            //sim = client.Network.CurrentSim;

            world.Cursor = Cursors.NoMove2D;
            //pictureBox1.Cursor = Cursors.Hand; 

            string msg1 = "Click for help on how to use/setup the Voice feature.";
            tTip = new Popup(customToolTip = new CustomToolTip(instance, msg1));
            tTip.AutoClose = false;
            tTip.FocusOnOpen = false;
            tTip.ShowingAnimation = tTip.HidingAnimation = PopupAnimations.Blend;

            string msg2 = ">Hover mouse on avatar icon for info\n>Click on avatar icon for Profile\n>Left click on map and drag to zoom\n>Double click on map to open large map";
            tTip1 = new Popup(customToolTip = new CustomToolTip(instance, msg2));
            tTip1.AutoClose = false;
            tTip1.FocusOnOpen = false;
            tTip1.ShowingAnimation = tTip1.HidingAnimation = PopupAnimations.Blend;

            toolTip.AutoPopDelay = 7000;
            toolTip.InitialDelay = 450;
            toolTip.ReshowDelay = 450;
            toolTip.IsBalloon = false;
            //toolTip.ToolTipIcon = ToolTipIcon.Info;

            toolTip.OwnerDraw = true;
            toolTip.BackColor = Color.RoyalBlue;
            toolTip.ForeColor = Color.White;
            toolTip.Draw += new DrawToolTipEventHandler(toolTip_Draw);

            tp1 = tabPage1;
            tp2 = tabPage2;
            tp3 = tabPage3;
            tp4 = tabPage4;

            //pTP.BackColor = Color.FromArgb(170, 64, 64, 64);  //Color.FromArgb(25, Color.DimGray);
        }

        private void toolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.DrawBackground();
            e.DrawBorder();
            e.DrawText();
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

            lock (instance.avnames)
            {
                foreach (KeyValuePair<UUID, string> av in names.Names)
                {
                    if (!instance.avnames.ContainsKey(av.Key))
                    {
                        instance.avnames.Add(av.Key, av.Value);
                    }
                }
            }
        }

        private void netcom_TeleportStatusChanged(object sender, TeleportEventArgs e)
        {
            try
            {
                //evs.OnTeleportStatusChanged(e); 

                switch (e.Status)
                {
                    case TeleportStatus.Start:
                        pTP.Visible = true;
                        label13.Text = "Teleporting";

                        break;
                    case TeleportStatus.Progress:
                        BeginInvoke(new MethodInvoker(delegate()
                        {
                            pTP.Visible = true;
                            label13.Text = e.Message;
                            //label13.Refresh(); 
                        }));


                        //if (e.Message.ToLower() == "resolving")
                        //{
                        //    if (tpf == null)
                        //    {
                        //        tpf = new frmTPdialogue(instance);
                        //        tpf.ShowDialog(instance.MainForm);
                        //    }
                        //    else
                        //    {
                        //        tpf.Dispose();

                        //        tpf = new frmTPdialogue(instance);
                        //        tpf.ShowDialog(instance.MainForm);
                        //    }
                        //}

                        break;

                    case TeleportStatus.Failed:
                        //MessageBox.Show(e.Message, "Teleport", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //if (tpf == null)
                        //{
                        //    tpf = new frmTPdialogue(instance, "Teleport Failed");
                        //    tpf.ShowDialog(instance.MainForm);
                        //}
                        //else
                        //{
                        //    tpf.Dispose();

                        //    tpf = new frmTPdialogue(instance, "Teleport Failed");
                        //    tpf.ShowDialog(instance.MainForm);
                        //}

                        BeginInvoke(new MethodInvoker(delegate()
                        {
                            label13.Text = "Teleport Failed";
                            pTP.Visible = true;

                            TPtimer.Enabled = true;
                            TPtimer.Start();  
                        }));

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

                        //if (tpf == null)
                        //{
                        //    tpf = new frmTPdialogue(instance, "Teleport Succeded");
                        //    tpf.ShowDialog(instance.MainForm);
                        //}
                        //else
                        //{
                        //    tpf.Dispose();

                        //    tpf = new frmTPdialogue(instance, "Teleport Succeded");
                        //    tpf.ShowDialog(instance.MainForm);
                        //}

                        BeginInvoke(new MethodInvoker(delegate()
                        {
                            label13.Text = "Teleport Succeded";
                            pTP.Visible = true;

                            TPtimer.Enabled = true;
                            TPtimer.Start(); 
                        }));

                        break;
                }
            }
            catch (Exception ex)
            {
                reporter.Show(ex);
            }
        }

        //void StartTPTimer()
        //{
        //    tpTimer = new System.Timers.Timer();
        //    tpTimer.Interval = 3000;
        //    tpTimer.Elapsed += new ElapsedEventHandler(OnTimedTPEvent);
        //    tpTimer.Enabled = true;
        //    tpTimer.Start();
        //}

        //private void OnTimedTPEvent(object sender, ElapsedEventArgs e)
        //{
        //    pTP.Visible = false;
        //    label13.Text = "Teleporting..."; 
        //    StopTPTimer();
        //}

        //void StopTPTimer()
        //{
        //    tpTimer.Stop();
        //    tpTimer.Enabled = false;
            
        //    tpTimer.Elapsed -= new ElapsedEventHandler(OnTimedTPEvent);
        //    //tpTimer.Dispose();
        //}

        void ChatConsole_Disposed(object sender, EventArgs e)
        {
            this.instance.Config.ConfigApplied -= new EventHandler<ConfigAppliedEventArgs>(Config_ConfigApplied);
            client.Objects.ObjectProperties -= new EventHandler<ObjectPropertiesEventArgs>(Objects_OnObjectProperties);
            client.Appearance.AppearanceSet -= new EventHandler<AppearanceSetEventArgs>(Appearance_OnAppearanceSet);
            client.Parcels.ParcelDwellReply -= new EventHandler<ParcelDwellReplyEventArgs>(Parcels_OnParcelDwell);
            client.Avatars.UUIDNameReply -= new EventHandler<UUIDNameReplyEventArgs>(Avatars_OnAvatarNames);

            RemoveEvents();
            chatManager = null;
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
                    //CheckWearables();
                    //CheckLocation();
                }));
            }
            catch { ; }

            checkBox5.Enabled = instance.AllowVoice;

            if (checkBox5.Enabled)
            {
                label18.Text = "Check 'Voice ON' box below. Then on 'Session start' unmute MIC to talk";
            }
            else
            {
                label18.Text = "Voice is disabled on this parcel";
            }

            client.Appearance.AppearanceSet -= new EventHandler<AppearanceSetEventArgs>(Appearance_OnAppearanceSet);
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
                Vector3 apos = new Vector3(Vector3.Zero);
                apos = client.Self.SimPosition;

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
            if (this.instance.MainForm.parcel != null)
            {
                if (this.instance.MainForm.parcel.LocalID != e.LocalID) return;
            }

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
                Vector3 apos = new Vector3(Vector3.Zero); 
                apos = instance.SIMsittingPos();

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
                        if (missing.EndsWith(", "))
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

            instance.State.ResetCamera();   

            sitTimer.Stop();
            sitTimer.Enabled = false;
            sitTimer.Dispose();
            sitTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent);

            Logger.Log("AUTOSIT: Searching for sit object", Helpers.LogLevel.Info);

            Vector3 location = new Vector3(Vector3.Zero); 
            location = client.Self.SimPosition;
            float radius = 21;

            // *** find all objects in radius ***
            List<Primitive> prims = client.Network.CurrentSim.ObjectsPrimitives.FindAll(
                delegate(Primitive prim)
                {
                    Vector3 pos = new Vector3(Vector3.Zero); 
                    pos = prim.Position;
                    return ((prim.ParentID == 0) && (pos != Vector3.Zero) && (Vector3.Distance(location, pos) < radius));
                }
            );

            if (prims == null)
            {
                //
            }

            localids = new uint[prims.Count];
            int i = 0;

            //if (listnerdisposed)
            //{
                client.Objects.ObjectProperties += new EventHandler<ObjectPropertiesEventArgs>(Objects_OnObjectProperties);
                //listnerdisposed = false;
            //}

            foreach (Primitive prim in prims)
            {
                try
                {
                    if (prim.ParentID == 0) //root prims only
                    {
                        //localids[i] = prim.LocalID;

                        //client.Objects.RequestObject(client.Network.CurrentSim, localids[i]);
                        client.Objects.SelectObject(client.Network.CurrentSim, prim.LocalID, true);

                        i += 1;
                    }
                }
                catch (Exception ex)
                {
                    //string exp = exc.Message;
                    reporter.Show(ex);
                }
            }

            //client.Objects.SelectObjects(client.Network.CurrentSim, localids);
        }

        private void Objects_OnObjectProperties(object sender, ObjectPropertiesEventArgs e)
        {
            if (e.Properties.Description.Trim() == client.Self.AgentID.ToString().Trim())
            {
                client.Objects.ObjectProperties -= new EventHandler<ObjectPropertiesEventArgs>(Objects_OnObjectProperties);
                //client.Self.AvatarSitResponse += new EventHandler<AvatarSitResponseEventArgs>(Self_AvatarSitResponse);
                //listnerdisposed = true;

                if (instance.Config.CurrentConfig.AutoSit)
                {
                    if (!instance.State.IsSitting)
                    {
                        instance.State.SetSitting(true, e.Properties.ObjectID);
                        localids = null;
                        //listnerdisposed = true;

                        Logger.Log("AUTOSIT: Found sit object and sitting", Helpers.LogLevel.Info);
                    }
                }
            }
        }

        //void Self_AvatarSitResponse(object sender, AvatarSitResponseEventArgs e)
        //{
        //    //client.Self.AvatarSitResponse -= new EventHandler<AvatarSitResponseEventArgs>(Self_AvatarSitResponse);

        //    instance.State.SitPrim = e.ObjectID;
        //    instance.State.IsSitting = true;
        //}

        private void AddLanguages()
        {
            // TODO: This should be converted into a language combobox component at
            // some stage

            //cboLanguage.Items.Clear();
            ////cboLanguage.Items.Add("Select...");
            //cboLanguage.Items.Add(new ComboEx.ICItem("Select...", -1));

            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Arabic en|ar", 1));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Chineese(simp) en|zh-CN", 2));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Chineese(trad) en|zh-TW", 3));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Croatian en|hr", 4));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Czech en|cs", 5));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Danish en|da", 6));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Dutch en|nl", 7));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Filipino en|tl", 9));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Finnish en|fi", 10));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/French en|fr", 11));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/German en|de", 12));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Greek en|el", 13));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Hebrew en|iw", 14));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Hindi en|hi", 15));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Hungarian en|hu", 16));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Indonesian en|id", 17));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Italian en|it", 18));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Japanese en|ja", 19));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Korean en|ko", 20));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Lithuanian en|lt", 21));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Norwegian en|no", 22));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Polish en|pl", 23));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Portuguese en|p", 24));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Romanian en|ro", 25));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Russian en|ru", 26));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Slovenian en|sl", 27));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Spanish en|es", 28));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Swedish en|sv", 29));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Thai en|th", 30));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Turkish en|tr", 31));
            //cboLanguage.Items.Add(new ComboEx.ICItem("English/Ukrainian en|uk", 32));

            //cboLanguage.Items.Add("Arabic/English ar|en");
            //cboLanguage.Items.Add("Chineese(simp)/English zh-CN|en");
            //cboLanguage.Items.Add("Chineese(trad)/English zh-TW|en");
            //cboLanguage.Items.Add("Croatian/English hr|en");
            //cboLanguage.Items.Add("Czech/English cs|en");
            //cboLanguage.Items.Add("Danish/English da|en");
            //cboLanguage.Items.Add("Dutch/English nl|en");
            //cboLanguage.Items.Add("Finnish/English fi|en");
            //cboLanguage.Items.Add("Filipino/English tl|en");
            //cboLanguage.Items.Add("French/English fr|en");
            //cboLanguage.Items.Add("German/English de|en");
            //cboLanguage.Items.Add("Greek/English el|en");
            //cboLanguage.Items.Add("Hebrew/English iw|en");
            //cboLanguage.Items.Add("Hindi/English hi|en");
            //cboLanguage.Items.Add("Hungarian/English hu|en");
            //cboLanguage.Items.Add("Indonesian/English id|en");
            //cboLanguage.Items.Add("Italian/English it|en");
            //cboLanguage.Items.Add("Japanese/English ja|en");
            //cboLanguage.Items.Add("Korean/English ko|en");
            //cboLanguage.Items.Add("Lithuanian/English lt|en");
            //cboLanguage.Items.Add("Norwegian/English no|en");
            //cboLanguage.Items.Add("Polish/English pl|en");
            //cboLanguage.Items.Add("Portuguese/English pt|en");
            //cboLanguage.Items.Add("Russian/English ru|en");
            //cboLanguage.Items.Add("Romanian/English ro|en");
            //cboLanguage.Items.Add("Slovenian/English sl|en");
            //cboLanguage.Items.Add("Spanish/English es|en");
            //cboLanguage.Items.Add("Swedish/English sv|en");
            //cboLanguage.Items.Add("Thai/English th|en");
            //cboLanguage.Items.Add("Turkish/English tr|en");
            //cboLanguage.Items.Add("Ukrainian/English uk|en");

            //cboLanguage.Items.Add("German/French de|fr");
            //cboLanguage.Items.Add("Spanish/French es|fr");
            //cboLanguage.Items.Add("French/German fr|de");
            //cboLanguage.Items.Add("French/Spanish fr|es");
            //cboLanguage.SelectedIndex = 0;
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

            _menuItem = new EmoticonMenuItem(Smileys.AngrySmile);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[1].Tag = "angry;";

            _menuItem = new EmoticonMenuItem(Smileys.Beer);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[2].Tag = "beer;";

            //_menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.BrokenHeart);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[3].Tag = "brokenheart;";

            _menuItem = new EmoticonMenuItem(Smileys.bye);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[4].Tag = "bye";

            _menuItem = new EmoticonMenuItem(Smileys.clap);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[5].Tag = "clap;";

            _menuItem = new EmoticonMenuItem(Smileys.ConfusedSmile);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[6].Tag = ":S";

            _menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.cool);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[7].Tag = "cool;";

            _menuItem = new EmoticonMenuItem(Smileys.CrySmile);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[8].Tag = "cry;";

            //_menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.DevilSmile);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[9].Tag = "devil;";

            _menuItem = new EmoticonMenuItem(Smileys.duh);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[10].Tag = "duh;";

            _menuItem = new EmoticonMenuItem(Smileys.EmbarassedSmile);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[11].Tag = "embarassed;";

            _menuItem = new EmoticonMenuItem(Smileys.happy0037);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[12].Tag = ":)";

            _menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.heart);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[13].Tag = "heart;";

            _menuItem = new EmoticonMenuItem(Smileys.kiss);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[14].Tag = "muah;";

            //_menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.help);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[15].Tag = "help ";

            _menuItem = new EmoticonMenuItem(Smileys.liar);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[16].Tag = "liar;";

            _menuItem = new EmoticonMenuItem(Smileys.lol);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[17].Tag = "lol";

            _menuItem = new EmoticonMenuItem(Smileys.oops);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[18].Tag = "oops";

            _menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.sad);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[19].Tag = ":(";

            _menuItem = new EmoticonMenuItem(Smileys.shhh);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[20].Tag = "shhh";

            //_menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.sigh);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[21].Tag = "sigh ";

            _menuItem = new EmoticonMenuItem(Smileys.silenced);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[22].Tag = ":X";

            _menuItem = new EmoticonMenuItem(Smileys.think);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[23].Tag = "thinking;";

            _menuItem = new EmoticonMenuItem(Smileys.ThumbsUp);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[24].Tag = "thumbsup;";

            _menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.whistle);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[25].Tag = "whistle;";

            _menuItem = new EmoticonMenuItem(Smileys.wink);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[26].Tag = ";)";

            //_menuItem.BarBreak = true;

            _menuItem = new EmoticonMenuItem(Smileys.wow);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[27].Tag = "wow ";

            _menuItem = new EmoticonMenuItem(Smileys.yawn);
            _menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
            cmenu_Emoticons.MenuItems.Add(_menuItem);
            cmenu_Emoticons.MenuItems[28].Tag = "yawn;";

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
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            tabConsole = instance.TabConsole;
        }

        private void Config_ConfigApplied(object sender, ConfigAppliedEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
                {
                    ApplyConfig(e.AppliedConfig);
                }));
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

            if (instance.Config.CurrentConfig.EnableSpelling)
            {
                dicfile = instance.Config.CurrentConfig.SpellLanguage;   // "en_GB.dic";

                this.instance.AffFile = afffile = dicfile + ".aff";
                this.instance.DictionaryFile = dicfile + ".dic";

                dic = dir + dicfile;

                dicfile += ".dic";

                if (!System.IO.File.Exists(dic + ".csv"))
                {
                    //System.IO.File.Create(dic + ".csv");

                    using (StreamWriter sw = File.CreateText(dic + ".csv"))
                    {
                        sw.Dispose();
                    }
                }

                hunspell.Dispose();
                hunspell = new Hunspell();

                hunspell.Load(dir + afffile, dir + dicfile);
                ReadWords();
            }
            else
            {
                hunspell.Dispose();
            }

            if (instance.Config.CurrentConfig.DisableRadar)
            {
                toolStrip1.Visible = false;
                tabControl1.TabPages.Remove(tabPage1);
                tabControl1.TabPages.Remove(tabPage2);

                picCompass.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                label19.Visible = false;
                label20.Visible = false;

                client.Grid.CoarseLocationUpdate -= new EventHandler<CoarseLocationUpdateEventArgs>(Grid_OnCoarseLocationUpdate);
            }
            else
            {
                if (!tabControl1.TabPages.Contains(tabPage1))
                {
                    toolStrip1.Visible = true;
                    tabControl1.TabPages.Remove(tabPage3);
                    tabControl1.TabPages.Remove(tabPage4);

                    tabPage1 = tp1;
                    tabControl1.TabPages.Add(tabPage1);
                    tabPage2 = tp2;
                    tabControl1.TabPages.Add(tabPage2);

                    picCompass.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label19.Visible = true;
                    label20.Visible = true;

                    client.Grid.CoarseLocationUpdate += new EventHandler<CoarseLocationUpdateEventArgs>(Grid_OnCoarseLocationUpdate);
                }
            }

            if (instance.Config.CurrentConfig.DisableVoice)
            {
                tabControl1.TabPages.Remove(tabPage3);
            }
            else
            {
                if (!tabControl1.TabPages.Contains(tabPage3))
                {
                    tabControl1.TabPages.Remove(tabPage4);
                    tabPage3 = tp3;
                    tabControl1.TabPages.Add(tabPage3);
                }
            }

            if (instance.Config.CurrentConfig.DisableFavs)
            {
                tabControl1.TabPages.Remove(tabPage4);
            }
            else
            {
                if (!tabControl1.TabPages.Contains(tabPage4))
                {
                    tabPage4 = tp4;
                    tabControl1.TabPages.Add(tabPage4);
                }
            }

            if (instance.Config.CurrentConfig.DisableRadar && instance.Config.CurrentConfig.DisableFavs && instance.Config.CurrentConfig.DisableFavs)
            {
                splitContainer1.SplitterDistance = splitContainer1.Width;   //513
                panel5.Visible = false;
                tabControl1.Visible = false;
            }
            else
            {
                splitContainer1.SplitterDistance = 513;
                panel5.Visible = true;
                tabControl1.Visible = true;
                //tabControl1.TabPages.Add(tabPage1);
                //tabControl1.TabPages.Add(tabPage2);
                //toolStrip1.Visible = true;
                //client.Grid.CoarseLocationUpdate += new EventHandler<CoarseLocationUpdateEventArgs>(Grid_OnCoarseLocationUpdate);
            }

            textBox1.Text = "Range: " + instance.Config.CurrentConfig.RadarRange.ToString() + "m"; 
        }

        public void RemoveEvents()
        {
            netcom.ClientLoginStatus -= new EventHandler<LoginProgressEventArgs>(netcom_ClientLoginStatus);
            netcom.ClientLoggedOut -= new EventHandler(netcom_ClientLoggedOut);
            netcom.ChatReceived -= new EventHandler<ChatEventArgs>(netcom_ChatReceived);
            netcom.TeleportStatusChanged -= new EventHandler<TeleportEventArgs>(netcom_TeleportStatusChanged);

            //client.Objects.AvatarUpdate -= new EventHandler<AvatarUpdateEventArgs>(Objects_OnNewAvatar);
            //client.Objects.KillObject -= new EventHandler<KillObjectEventArgs>(Objects_OnObjectKilled);

            client.Grid.CoarseLocationUpdate -= new EventHandler<CoarseLocationUpdateEventArgs>(Grid_OnCoarseLocationUpdate);
            client.Network.SimChanged -= new EventHandler<SimChangedEventArgs>(Network_OnCurrentSimChanged);
            client.Self.MeanCollision -= new EventHandler<MeanCollisionEventArgs>(Self_Collision);
            client.Objects.TerseObjectUpdate -= new EventHandler<TerseObjectUpdateEventArgs>(Objects_OnObjectUpdated);
            client.Avatars.UUIDNameReply -= new EventHandler<UUIDNameReplyEventArgs>(Avatars_OnAvatarNames);

            //if (instance.Config.CurrentConfig.iRadar)
            //{
            //    //client.Objects.TerseObjectUpdate -= new EventHandler<TerseObjectUpdateEventArgs>(Objects_OnObjectUpdated);
            //    client.Self.MeanCollision -= new EventHandler<MeanCollisionEventArgs>(Self_Collision);
            //}
        }

        private void AddNetcomEvents()
        {
            netcom.ClientLoginStatus += new EventHandler<LoginProgressEventArgs>(netcom_ClientLoginStatus);
            netcom.ClientLoggedOut += new EventHandler(netcom_ClientLoggedOut);
        }

        private void AddClientEvents()
        {
            netcom.ChatReceived += new EventHandler<ChatEventArgs>(netcom_ChatReceived);
            netcom.TeleportStatusChanged += new EventHandler<TeleportEventArgs>(netcom_TeleportStatusChanged);

            //client.Objects.AvatarUpdate += new EventHandler<AvatarUpdateEventArgs>(Objects_OnNewAvatar);
            //client.Objects.KillObject += new EventHandler<KillObjectEventArgs>(Objects_OnObjectKilled);

            if (!instance.Config.CurrentConfig.DisableRadar)
            {
                client.Grid.CoarseLocationUpdate += new EventHandler<CoarseLocationUpdateEventArgs>(Grid_OnCoarseLocationUpdate);
            }

            client.Network.SimChanged += new EventHandler<SimChangedEventArgs>(Network_OnCurrentSimChanged);

            client.Self.MeanCollision += new EventHandler<MeanCollisionEventArgs>(Self_Collision);
            client.Objects.TerseObjectUpdate += new EventHandler<TerseObjectUpdateEventArgs>(Objects_OnObjectUpdated);

            //client.Appearance.OnAppearanceUpdated += new AppearanceManager.AppearanceUpdatedCallback(Appearance_OnAppearanceUpdated);
            client.Appearance.AppearanceSet += new EventHandler<AppearanceSetEventArgs>(Appearance_OnAppearanceSet);
            client.Parcels.ParcelDwellReply += new EventHandler<ParcelDwellReplyEventArgs>(Parcels_OnParcelDwell);
            
            //client.Self.AvatarSitResponse += new EventHandler<AvatarSitResponseEventArgs>(Self_AvatarSitResponse);

            client.Avatars.UUIDNameReply += new EventHandler<UUIDNameReplyEventArgs>(Avatars_OnAvatarNames);
            //client.Objects.ObjectProperties += new EventHandler<ObjectPropertiesEventArgs>(Objects_OnObjectProperties);

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
                        cty = "Bumped in by: (" + e.Time.ToString() + " - " + e.Magnitude.ToString() + "): ";
                    }
                    else if (e.Type == MeanCollisionType.LLPushObject)
                    {
                        cty = "Pushed by: (" + e.Time.ToString() + " - " + e.Magnitude.ToString() + "): ";
                    }
                    else if (e.Type == MeanCollisionType.PhysicalObjectCollide)
                    {
                        cty = "Physical object collided (" + e.Time.ToString() + " - " + e.Magnitude.ToString() + "): ";
                    }

                    chatManager.PrintAlertMessage(cty + e.Aggressor.ToString());
                }));
            }
            catch
            {
               ;
            }
        }

        ////Separate thread
        //private void Objects_OnObjectKilled(object sender, KillObjectEventArgs e)
        //{
        //    if (InvokeRequired)
        //    {
        //        BeginInvoke(new MethodInvoker(delegate()
        //        {
        //            Objects_OnObjectKilled(sender, e);
        //        }));

        //        return;
        //    }

        //    if (e.Simulator != client.Network.CurrentSim) return;
        //    if (sfavatar == null) return;
        //    if (!sfavatar.ContainsKey(e.ObjectLocalID)) return;

        //    foreach (ListViewItem litem in lvwRadar.Items)
        //    {

        //        if (litem.Tag.ToString() == sfavatar[e.ObjectLocalID].ID.ToString())
        //        {
        //            lvwRadar.BeginUpdate();
        //            lvwRadar.Items.RemoveByKey(sfavatar[e.ObjectLocalID].Name);
        //            lvwRadar.EndUpdate();
        //        }
        //    }

        //    try
        //    {
        //        lock (sfavatar)
        //        {
        //            sfavatar.Remove(e.ObjectLocalID);
        //        }
        //    }
        //    catch { ; }
        //}

        ////Separate thread
        //private void Objects_OnNewAvatar(object sender, AvatarUpdateEventArgs e)
        //{
        //    if (InvokeRequired)
        //    {

        //        BeginInvoke(new MethodInvoker(delegate()
        //        {
        //            Objects_OnNewAvatar(sender, e);
        //        }));

        //        return;
        //    }

        //    if (e.Simulator != client.Network.CurrentSim) return;
        //    if (sfavatar.ContainsKey(e.Avatar.LocalID)) return;

        //    try
        //    {
        //        lock (sfavatar)
        //        {
        //            sfavatar.Add(e.Avatar.LocalID, e.Avatar);
        //        }
        //    }
        //    catch { ; }
        //}

        private void Objects_OnObjectUpdated(object sender, TerseObjectUpdateEventArgs e)
        {
            if (e.Simulator != client.Network.CurrentSim) return;
            if (!e.Update.Avatar) return;

            //Avatar av = new Avatar();
            //client.Network.CurrentSim.ObjectsAvatars.TryGetValue(e.Update.LocalID, out av);
            ////client.Network.CurrentSim.ObjectsAvatars.TryGetValue(e.Prim.LocalID, out av);

            //if (av == null) return;

            //if (!sfavatar.ContainsKey(av.LocalID))
            //{
            //    //Avatar av = new Avatar();
            //    //client.Network.CurrentSim.ObjectsAvatars.TryGetValue(e.Update.LocalID, out av);

            //    try
            //    {
            //        lock (sfavatar)
            //        {
            //            sfavatar.Add(av.LocalID, av);
            //        }
            //    }
            //    catch { ; }
            //}
            //else
            //{
            //    lock (sfavatar)
            //    {
            //        sfavatar.Remove(av.LocalID);
            //        sfavatar.Add(av.LocalID, av);
            //    }
            //}

            if (e.Prim.ID == client.Self.AgentID)
            {
                instance.State.ResetCamera();

                BeginInvoke(new MethodInvoker(delegate()
                {
                    try
                    {
                        if (!lvwRadar.Items.ContainsKey(client.Self.Name))
                        {
                            ListViewItem item = lvwRadar.Items.Add(client.Self.Name, client.Self.Name, string.Empty);
                            item.Font = new Font(item.Font, FontStyle.Bold);
                            item.Tag = client.Self.AgentID;
                            item.BackColor = Color.WhiteSmoke;
                            item.ForeColor = Color.Black;

                            item.SubItems.Add(string.Empty);
                            //item.SubItems.Add(string.Empty);
                        }
                    }
                    catch { ; }
                }));

                return;
            }
        }

        private delegate void OnAddSIMAvatar(string av, UUID key, Vector3 avpos, Color clr, string state);
        public void AddSIMAvatar(string av, UUID key, Vector3 avpos, Color clr, string state)
        {
            if (InvokeRequired)
            {

                BeginInvoke(new MethodInvoker(delegate()
                {
                    AddSIMAvatar(av, key, avpos, clr, state);
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

            string sDist = string.Empty;

            try
            {
                Vector3 selfpos = new Vector3(Vector3.Zero); 
                selfpos = client.Self.SimPosition;

                if (selfpos.Z < 0.1f)
                {
                    selfpos.Z = 1024f;
                }

                if (avpos.Z < 0.1f)
                {
                    avpos.Z = 1024f;
                }

                double dist = Math.Round(Vector3d.Distance(ConverToGLobal(selfpos), ConverToGLobal(avpos)), MidpointRounding.ToEven);

                string sym = string.Empty;
                
                sym = "(Alt.: " + avpos.Z.ToString("#0") + "m)";

                if (clr == Color.RoyalBlue)
                {
                    if (avpos.Z > 1019f)
                    {
                        //sDist = "[???m] ";
                        sDist = ">[" + Convert.ToInt32(dist).ToString() + "m] ";
                    }
                    else
                    {
                        sDist = "[" + Convert.ToInt32(dist).ToString() + "m] ";
                    }
                }
                else
                {
                    sDist = "[" + Convert.ToInt32(dist).ToString() + "m] ";
                }

                string rentry = " " + sym + state;

                lvwRadar.BeginUpdate();

                if (name != client.Self.Name)
                {
                    ListViewItem item = lvwRadar.Items.Add(name, sDist + name, string.Empty);
                    item.ForeColor = clr;
                    item.Tag = key;

                    rentry = rentry.Replace("*", " (Sitting)");  
                    item.ToolTipText = sDist + name + rentry;
                    //item.BackColor = rowclr;

                    //item.SubItems.Add(state);
                    //item.SubItems.Add(state);

                    //string[] str = name.Split(' ');
                    //string url = "https://my-secondlife.s3.amazonaws.com/users/" + str[0].ToLower() + "." + str[1].ToLower() + "/sl_image.png?" + key.ToString().Replace("-", "");
                    //Stream ImageStream = new WebClient().OpenRead(url);
                    //Image img = Image.FromStream(ImageStream);

                    //Bitmap bmp = new Bitmap(img, 25, 20);
                    //bmp.Tag = key.ToString();

                    if (avtyping.Contains(name))
                    {
                        int index = lvwRadar.Items.IndexOfKey(name);
                        if (index != -1)
                        {
                            lvwRadar.Items[index].ForeColor = Color.Red;
                        }
                    }
                }

                string avsnem = client.Self.Name;

                if (!lvwRadar.Items.ContainsKey(avsnem))
                {
                    ListViewItem item = lvwRadar.Items.Add(avsnem, avsnem, string.Empty);
                    item.Font = new Font(item.Font, FontStyle.Bold);
                    item.Tag = client.Self.AgentID;
                    item.BackColor = Color.WhiteSmoke;
                    item.ForeColor = Color.Black;  
                    
                    item.SubItems.Add(string.Empty);
                }

                recolorListItems(lvwRadar);
                lvwRadar.EndUpdate();
            }
            catch (Exception ex)
            {
                Logger.Log("Radar update: " + ex.Message, Helpers.LogLevel.Warning);
            }
        }

        private static void recolorListItems(ListView lv)
        {
            for (int ix = 0; ix < lv.Items.Count; ++ix)
            {
                var item = lv.Items[ix];
                item.BackColor = (ix % 2 == 0) ? Color.WhiteSmoke : Color.White;
            }
        }

        private Vector3d ConverToGLobal(Vector3 pos)
        {
            uint regionX, regionY;
            OpenMetaverse.Utils.LongToUInts(client.Network.CurrentSim.Handle, out regionX, out regionY);
            Vector3d objpos;

            objpos.X = (double)pos.X + (double)regionX;
            objpos.Y = (double)pos.Y + (double)regionY;
            objpos.Z = pos.Z;   // -2f;

            return objpos; 
        }

        //private delegate void OnAddAvatar(Avatar av);
        //public void AddAvatar(Avatar av)
        //{
        //    if (InvokeRequired)
        //    {

        //        BeginInvoke(new MethodInvoker(delegate()
        //        {
        //            AddAvatar(av);
        //        }));

        //        return;
        //    }

        //    if (!string.IsNullOrEmpty(selectedname)) return;

        //    if (av == null) return;
        //    string name = av.Name;

        //    if (string.IsNullOrEmpty(name)) return;

        //    BeginInvoke(new MethodInvoker(delegate()
        //    {
        //        lvwRadar.BeginUpdate();
        //        if (lvwRadar.Items.ContainsKey(name))
        //        {
        //            lvwRadar.Items.RemoveByKey(name);
        //        }

        //        lvwRadar.EndUpdate();
        //    }));

        //    string sDist;

        //    Vector3 avpos = new Vector3(Vector3.Zero); 
        //    avpos = av.Position;

        //    uint oID = av.ParentID;
        //    string astate = string.Empty;

        //    if (!instance.avtags.ContainsKey(av.ID))
        //    {
        //        instance.avtags.Add(av.ID, av.GroupName);
        //    }

        //    bool avissit = false;

        //    if (oID != 0)
        //    {
        //        // the av is sitting
        //        Primitive prim = new Primitive();

        //        try
        //        {
        //            client.Network.CurrentSim.ObjectsPrimitives.TryGetValue(oID, out prim);

        //            if (prim == null)
        //            {
        //                // do nothing
        //                avissit = true;
        //            }
        //            else
        //            {
        //                avpos += prim.Position;
        //            }

        //            astate = " (SIT.)";
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.Log("Chat console: (add avatar) when adding " + av.FirstName + " " + av.LastName + " - " + ex.Message, Helpers.LogLevel.Error, ex);
        //            //reporter.Show(ex);
        //        }
        //    }
        //    else
        //    {
        //        astate = string.Empty;
        //    }

        //    try
        //    {
        //        Vector3 selfpos = new Vector3(Vector3.Zero); 
        //        selfpos = client.Self.SimPosition;

        //        double dist = Math.Round(Vector3d.Distance(ConverToGLobal(selfpos),ConverToGLobal(avpos)), MidpointRounding.ToEven);

        //        //if ((int)dist > instance.Config.CurrentConfig.RadarRange) return;

        //        if (avpos.Z < 0.1f)
        //        {
        //            avpos.Z = 1024f;
        //            //dist = Math.Round(Vector3d.Distance(ConverToGLobal(selfpos),ConverToGLobal(avpos)), MidpointRounding.ToEven);
        //            sDist = "  >[" + Convert.ToInt32(dist).ToString() + "m]  ";
        //        }
        //        else
        //        {
        //            sDist = "  [" + Convert.ToInt32(dist).ToString() + "m]  ";
        //        }

        //        if (avissit)
        //        {
        //            sDist = "  [???m]  ";
        //        }

        //        //if (av.Name != client.Self.Name)
        //        //{
        //        //    Vector3 dirv = new Vector3(Vector3.Zero); 
        //        //    dirv = Vector3.Normalize(avpos - selfpos);
        //        //    dirv.Normalize();

        //        //    Quaternion avRot = client.Self.RelativeRotation;

        //        //    Matrix4 m = Matrix4.CreateFromQuaternion(avRot);

        //        //    Vector3 myrot = new Vector3(Vector3.Zero);
        //        //    myrot.X = m.M11;
        //        //    myrot.Y = m.M21;
        //        //    myrot.Z = m.M31;

        //        //    float vs = Vector3.Dot(myrot, dirv);

        //        //    bool isonfront = Vector3.Dot(myrot, dirv) > 0f; // less than 90 degrees

        //        //    Vector3 v1 = new Vector3(0, 0, 0);
        //        //    Vector3 v2 = new Vector3(0, 0, 0);

        //        //    v1 = selfpos;
        //        //    v2 = avpos;

        //        //    v1.Normalize();
        //        //    v2.Normalize();

        //        //    double angle = (float)Math.Acos(Vector3.Dot(v1, v2));
        //        //    //double angle = (float)Math.Acos(Vector3.Dot(myrot, dirv));

        //        //    double degrees = angle * 180 / Math.PI;
        //        //}

        //        string rentry = sDist + name + astate;
        //        //string rentry = name;

        //        BeginInvoke(new MethodInvoker(delegate()
        //        { 
        //            lvwRadar.BeginUpdate();

        //            if (name != client.Self.Name)
        //            {
        //                ListViewItem item = lvwRadar.Items.Add(name, rentry, string.Empty);
        //                item.Tag = av.ID;
        //                item.ToolTipText = name + "  " + astate;
        //                //item.ToolTipText = "test";

        //                //ListViewItem lvi = new ListViewItem(sDist + astate);
        //                //lvi.UseItemStyleForSubItems = false;
        //                //lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
        //                //    "subitem", Color.White, Color.Blue, lvi.Font));

        //                if (avtyping.Contains(name))
        //                {
        //                    int index = lvwRadar.Items.IndexOfKey(name);
        //                    if (index != -1)
        //                    {
        //                        lvwRadar.Items[index].ForeColor = Color.Red;
        //                    }
        //                }
        //            }
        //            //else
        //            //{
        //            //    ListViewItem item = lvwRadar.Items.Add(name, name, string.Empty);
        //            //    item.Font = new Font(item.Font, FontStyle.Bold);
        //            //    item.Tag = av.ID;
        //            //}

        //            string avsnem = client.Self.Name;

        //            if (!lvwRadar.Items.ContainsKey(avsnem))
        //            {
        //                ListViewItem item = lvwRadar.Items.Add(avsnem, avsnem, string.Empty);
        //                item.Font = new Font(item.Font, FontStyle.Bold);
        //                item.Tag = av.ID;
        //            }

        //            lvwRadar.EndUpdate();
        //        }));
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log("Radar update: " + ex.Message, Helpers.LogLevel.Warning);
        //    }
        //}

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

            //Vector3 vdir = new Vector3(Vector3.Zero);
            //vdir.X = 0.0f;
            //vdir.Y = 1.0f;
            //vdir.Z = 0.0f;

            Matrix4 m = Matrix4.CreateFromQuaternion(avRot);

            Vector3 vDir = new Vector3(Vector3.Zero);
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
        }

        public Vector3 QuaternionToEuler(Quaternion q)
        {
            Vector3 v = new Vector3(Vector3.Zero); 
            v = Vector3.Zero;

            v.X = (float)Math.Atan2
            (
                2 * q.Y * q.W - 2 * q.X * q.Z,
                   1 - 2 * Math.Pow(q.Y, 2) - 2 * Math.Pow(q.Z, 2)
            );

            v.Z = (float)Math.Asin
            (
                2 * q.X * q.Y + 2 * q.Z * q.W
            );

            v.Y = (float)Math.Atan2
            (
                2 * q.X * q.W - 2 * q.Y * q.Z,
                1 - 2 * Math.Pow(q.X, 2) - 2 * Math.Pow(q.Z, 2)
            );

            if (q.X * q.Y + q.Z * q.W == 0.5)
            {
                v.X = (float)(2 * Math.Atan2(q.X, q.W));
                v.Y = 0;
            }

            else if (q.X * q.Y + q.Z * q.W == -0.5)
            {
                v.X = (float)(-2 * Math.Atan2(q.X, q.W));
                v.Y = 0;
            }

            return v;
        }

        private void netcom_ClientLoginStatus(object sender, LoginProgressEventArgs e)
        {
            if (e.Status != LoginStatus.Success) return;

            cbxInput.Enabled = true;
            sim = client.Network.CurrentSim;

            AddClientEvents();
            timer2.Enabled = true;
            timer2.Start();
        }

        private void netcom_ClientLoggedOut(object sender, EventArgs e)
        {
            cbxInput.Enabled = false;
            tbSay.Enabled = false;

            lvwRadar.Items.Clear();
        }

        public void PrintAvUUID()
        {
            chatManager = new ChatTextManager(instance, new RichTextBoxPrinter(instance, rtbChat));
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

            if (e.FromName.ToLower() == netcom.LoginOptions.FullName.ToLower())
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

            //if (instance.DetectLang)
            //{
            //    if (!string.IsNullOrEmpty(e.Message))
            //    {
            //        //GoogleTranslationUtils.DetectLanguage detect = new GoogleTranslationUtils.DetectLanguage(e.Message);

            //        MB_Translation_Utils.Utils trans = new MB_Translation_Utils.Utils();
            //        string dland = trans.DetectLanguageFullName(e.Message);

            //        int sindex = trans.GetLangIndex(dland);

            //        if (sindex == 33)
            //            sindex = 0;

            //        this.instance.MainForm.SetFlag(imgFlags.Images[sindex], dland);

            //        // select the language pair fro mthe combo
            //        if (sindex != 0 && sindex != 8)
            //        {
            //            // English does not exist in the combo so adjust
            //            if (sindex > 7)
            //            {
            //                sindex -= 1;
            //            }

            //            //cboLanguage.SelectedIndex = sindex;
            //        }
            //    }
            //}
        }        

        public void ProcessChatInput(string input, ChatType type)
        {
            if (string.IsNullOrEmpty(input)) return;

            //if (chkTranslate.Checked == true)
            //{
            //    if (cboLanguage.SelectedIndex != 0)
            //    {
            //        // Call translation here
            //        string oinp = input;
            //        string tinput = GetTranslation(input);

            //        if (tinput != null)
            //        {
            //            tinput = HttpUtility.HtmlDecode(tinput);
            //            input = tinput + " (" + oinp + ")";
            //        }
            //    }
            //}
            //else
            //{

            input = input.Replace("http://secondlife:///", "secondlife:///");
            input = input.Replace("http://secondlife://", "secondlife:///");
 
                if (instance.Config.CurrentConfig.EnableSpelling)
                {
                    // put preference check here
                    //string cword = Regex.Replace(cbxInput.Text, @"[^a-zA-Z0-9]", "");
                    //string[] swords = cword.Split(' ');
                    string[] swords = input.Split(' ');
                    bool hasmistake = false;
                    bool correct = true;

                    foreach (string word in swords)
                    {
                        string cword = Regex.Replace(word, @"[^a-zA-Z0-9]", "");

                        try
                        {
                            correct = hunspell.Spell(cword);
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Dictionary is not loaded")
                            {
                                instance.Config.ApplyCurrentConfig();
                                //correct = hunspell.Spell(cword);
                            }
                            else
                            {
                                Logger.Log("Spellcheck error chat: " + ex.Message, Helpers.LogLevel.Error);
                            }
                        }

                        if (!correct)
                        {
                            hasmistake = true;
                        }
                    }

                    if (hasmistake)
                    {
                        (new frmSpelling(instance, cbxInput.Text, swords, type)).Show();
                        hasmistake = false;
                        return;
                    }
                }
            //}

            SendChat(input, type);
        }

        public void SendChat(string input, ChatType type)
        {
            string[] inputArgs = input.Split(' ');

            if (inputArgs[0].StartsWith("//")) //Chat on previously used channel
            {
                string message = string.Join(" ", inputArgs).Substring(2);
                netcom.ChatOut(message, type, previousChannel);
            }
            else if (inputArgs[0].StartsWith("/")) //Chat on specific channel
            {
                string message = string.Empty;
                string number = inputArgs[0].Substring(1);

                int channel = 0;
                int.TryParse(number, out channel);
                if (channel < 0) channel = 0;

                // VidaOrenstein on METAforum fix
                //string message = string.Join(" ", inputArgs, 1, inputArgs.GetUpperBound(0) - 1);

                if (input.StartsWith("/me "))
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

        private void ReadWords()
        {
            using (CsvFileReader reader = new CsvFileReader(dic + ".csv"))
            {
                CsvRow row = new CsvRow();

                while (reader.ReadRow(row))
                {
                    foreach (string s in row)
                    {
                        hunspell.Add(s);
                    }
                }

                reader.Dispose(); 
            }
        }

        private string GetTranslation(string sTrStr)
        {
            ////string sPair = GetLangPair(cboLanguage.Text);

            ////GoogleTranslationUtils.Translate trans = new GoogleTranslationUtils.Translate(sTrStr, sPair);

            ////return trans.Translation;

            //string sPair = GetLangPair(cboLanguage.Text);

            ////GoogleTranslationUtils.Translate trans = new GoogleTranslationUtils.Translate(sTrStr, sPair);
            ////return trans.Translation;

            ////string sPair

            //MB_Translation_Utils.Utils trans = new MB_Translation_Utils.Utils();

            //string tres = trans.Translate(sTrStr, sPair);

            //return tres;

            return string.Empty;  
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

                if (!cbxInput.Text.StartsWith("/"))
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
            client.Self.AutoPilotCancel();

            UUID av = (UUID)lvwRadar.SelectedItems[0].Tag;

            if (av == UUID.Zero || av == null) return;

            string name = instance.avnames[av];

            Avatar sav = new Avatar();
            sav = CurrentSIM.ObjectsAvatars.Find(delegate(Avatar fa)
            {
                return fa.ID == av;
            }
            );

            if (sav == null)
            {
                MessageBox.Show("Avatar is out of range for this function.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (instance.State.FollowName != name)
            {
                //instance.State.GoTo(string.Empty, UUID.Zero);

                instance.State.Follow(string.Empty, UUID.Zero);
                instance.State.Follow(name, av);
                tbtnFollow.ToolTipText = "Stop Following";
            }
            else
            {
                instance.State.Follow(string.Empty, UUID.Zero);
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
        }

        private void cbxInput_KeyUp(object sender, KeyEventArgs e)
        {
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

        private void rtbChat_TextChanged_1(object sender, EventArgs e)
        {
            ////int i = rtbChat.Lines.Length;

            ////if (i > 10)
            ////{
            ////    int lineno = i-10;
            ////    int chars = rtbChat.GetFirstCharIndexFromLine(lineno);
            ////    rtbChat.SelectionStart = 0;
            ////    rtbChat.SelectionLength = chars; // rtbChat.Text.IndexOf("\n", 0) + 1;
            ////    rtbChat.SelectedText = "*** " + lineno.ToString() + "lines purged\n";
            ////}
            ////else
            ////{
            ////    return;
            ////}

            ////int lncnt = Convert.ToInt32(rtbChat.Lines.LongLength);

            ////if (lncnt > this.instance.Config.CurrentConfig.lineMax)
            ////{
            ////    int numOfLines = 1;
            ////    var lines = rtbChat.Lines;
            ////    var newLines = lines.Skip(numOfLines);

            ////    rtbChat.Lines = newLines.ToArray();

            ////    chatManager.ReprintAllText();  
            ////}

            //int lncnt = Convert.ToInt32(rtbChat.Lines.LongLength);
            //int maxlines = this.instance.Config.CurrentConfig.lineMax;

            //if (lncnt > maxlines)
            //{
            //    int numOfLines = 1;
            //    var lines = rtbChat.Lines;
            //    var newLines = lines.Skip(numOfLines);

            //    rtbChat.Lines = newLines.ToArray();
            //}

            //bool focused = rtbChat.Focused;
            ////backup initial selection
            //int selection = rtbChat.SelectionStart;
            //int length = rtbChat.SelectionLength;
            ////allow autoscroll if selection is at end of text
            //bool autoscroll = (selection == rtbChat.Text.Length);

            //if (!autoscroll)
            //{
            //    //shift focus from RichTextBox to some other control
            //    if (focused) cbxInput.Focus();
            //    //hide selection
            //    SendMessage(rtbChat.Handle, EM_HIDESELECTION, 1, 0);
            //}
            //else
            //{
            //    SendMessage(rtbChat.Handle, EM_HIDESELECTION, 0, 0);
            //    //restore focus to RichTextBox
            //    if (focused) rtbChat.Focus();
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
            //if (chkTranslate.Checked == true)
            //{
            //    MessageBox.Show("~ METAtranslate ~ \n \n You must now select a language pair \n from the dropdown box. \n \n Anything you say will be auto translated to that language.", "METAtranslate");
            //    cboLanguage.Enabled = true;
            //}
            //else
            //{
            //    cboLanguage.Enabled = false;
            //    cboLanguage.SelectedIndex = 0;
            //}
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
            //(new frmTranslate(instance)).Show();
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

            ////Avatar av = ((ListViewItem)lvwRadar.SelectedItems[0]).Tag as Avatar;
            ////if (av == null) return;

            UUID av = (UUID)lvwRadar.SelectedItems[0].Tag;

            if (av == UUID.Zero || av == null) return;

            string name = instance.avnames[av];

            Avatar sav = new Avatar();
            sav = CurrentSIM.ObjectsAvatars.Find(delegate(Avatar fa)
            {
                return fa.ID == av;
            }
            );

            if (sav == null)
            {
                MessageBox.Show("Avatar is out of range for this function.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Vector3 pos = new Vector3(Vector3.Zero);
            pos = sav.Position;

            // Is the avatar sitting
            uint oID = sav.ParentID;

            if (oID != 0)
            {
                // the av is sitting
                Primitive prim = new Primitive();

                try
                {
                    client.Network.CurrentSim.ObjectsPrimitives.TryGetValue(oID, out prim);

                    if (prim == null)
                    {
                        // do nothing
                        client.Self.AutoPilotCancel();
                        Logger.Log("GoTo cancelled. Could find the object the target avatar is sitting on.", Helpers.LogLevel.Warning);
                        return;
                    }
                    else
                    {
                        pos += prim.Position;
                    }
                }
                catch
                {
                    ;
                    //reporter.Show(ex);
                }
            }

            ulong regionHandle = client.Network.CurrentSim.Handle;

            ulong followRegionX = regionHandle >> 32;
            ulong followRegionY = regionHandle & (ulong)0xFFFFFFFF;

            ulong x = (ulong)pos.X + followRegionX;
            ulong y = (ulong)pos.Y + followRegionY;
            float z = pos.Z - 1f;

            //if (instance.State.GoName != name)
            //{
            //    instance.State.Follow(string.Empty, UUID.Zero);

            //    instance.State.GoTo(string.Empty, UUID.Zero);
            //    instance.State.GoTo(name, av);
            //    //tbtnGoto.ToolTipText = "Stop Go to";
            //}
            //else
            //{
            //    instance.State.GoTo(string.Empty, UUID.Zero);
            //    //tbtnFollow.ToolTipText = "Go to";
            //}

            client.Self.AutoPilot(x, y, z);
        }

        private void tbtnTurn_Click(object sender, EventArgs e)
        {
            //Avatar av = ((ListViewItem)lvwRadar.SelectedItems[0]).Tag as Avatar;
            //if (av == null) return;

            UUID av = (UUID)lvwRadar.SelectedItems[0].Tag;

            if (av == UUID.Zero || av == null) return;

            //string name = instance.avnames[av];

            Avatar sav = new Avatar();
            sav = CurrentSIM.ObjectsAvatars.Find(delegate(Avatar fa)
             {
                 return fa.ID == av;
             }
             );

            if (sav != null)
            {
                client.Self.AnimationStart(Animations.TURNLEFT, false);

                Vector3 pos = new Vector3(Vector3.Zero); 
                pos = sav.Position;

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
            if (e.LinkText.StartsWith("http://slurl."))
            {
                try
                {
                    // Open up the TP form here
                    string encoded = HttpUtility.UrlDecode(e.LinkText);
                    string[] split = encoded.Split(new Char[] { '/' });
                    //string[] split = e.LinkText.Split(new Char[] { '/' });
                    string simr = split[4].ToString();
                    double x = Convert.ToDouble(split[5].ToString());
                    double y = Convert.ToDouble(split[6].ToString());
                    double z = Convert.ToDouble(split[7].ToString());

                    (new frmTeleport(instance, simr, (float)x, (float)y, (float)z)).Show();
                }
                catch { ; }

            }
            if (e.LinkText.StartsWith("http://maps.secondlife"))
            {
                try
                {
                    // Open up the TP form here
                    string encoded = HttpUtility.UrlDecode(e.LinkText);
                    string[] split = encoded.Split(new Char[] { '/' });
                    //string[] split = e.LinkText.Split(new Char[] { '/' });
                    string simr = split[4].ToString();
                    double x = Convert.ToDouble(split[5].ToString());
                    double y = Convert.ToDouble(split[6].ToString());
                    double z = Convert.ToDouble(split[7].ToString());

                    (new frmTeleport(instance, simr, (float)x, (float)y, (float)z)).Show();
                }
                catch { ; }

            }
            else if (e.LinkText.Contains("http://mbprofile:"))
            {
                try
                {
                    string encoded = HttpUtility.UrlDecode(e.LinkText);
                    string[] split = encoded.Split(new Char[] { '/' });
                    //string[] split = e.LinkText.Split(new Char[] { '#' });
                    string aavname = split[0].ToString();
                    string[] avnamesplit = aavname.Split(new Char[] { '#' });
                    aavname = avnamesplit[0].ToString();

                    split = e.LinkText.Split(new Char[] { ':' });
                    string elink = split[2].ToString();
                    split = elink.Split(new Char[] { '&' });

                    UUID avid = (UUID)split[0].ToString();

                    (new frmProfile(instance, aavname, avid)).Show();
                }
                catch { ; }
            }
            else if (e.LinkText.Contains("http://secondlife:///"))
            {
                // Open up the Group Info form here
                string encoded = HttpUtility.UrlDecode(e.LinkText);
                string[] split = encoded.Split(new Char[] { '/' });
                UUID uuid = (UUID)split[7].ToString();

                if (uuid != UUID.Zero && split[6].ToString().ToLower() == "group")
                {
                    frmGroupInfo frm = new frmGroupInfo(uuid, instance);
                    frm.Show();
                }
                else if (uuid != UUID.Zero && split[6].ToString().ToLower() == "agent")
                {
                    (new frmProfile(instance, string.Empty, uuid)).Show();
                }
            }
            else if (e.LinkText.StartsWith("http://") || e.LinkText.StartsWith("ftp://") || e.LinkText.StartsWith("https://"))
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
            const int WM_SYSKEYDOWN = 0x104;

            // Fix submitted by METAforums user Spirit 25/09/2009
            // TO DO: This should be a setting in preferences so that
            // it works both ways...
            if (cbxInput.Focused) return false;

            switch (key)
            {
                case 33: // <--- page up.
                    if ((m.Msg == WM_KEYDOWN) || (m.Msg == WM_SYSKEYDOWN))
                    {
                        up(true);
                    }
                    else
                    {
                        up(false);
                    }
                    break;
                case 34: // <--- page down.
                    if ((m.Msg == WM_KEYDOWN) || (m.Msg == WM_SYSKEYDOWN))
                    {
                        dwn(true);
                    }
                    else
                    {
                        dwn(false);
                    }
                    break;
                case 37: // <--- left arrow.
                    lft();
                    break;
                case 38: // <--- up arrow.
                    if ((m.Msg == WM_KEYDOWN) || (m.Msg == WM_SYSKEYDOWN))
                    {
                        fwd(true);
                    }
                    else
                    {
                        fwd(false);
                    }
                    break;
                case 39: // <--- right arrow.
                    rgt();
                    break;
                case 40: // <--- down arrow.
                    if ((m.Msg == WM_KEYDOWN) || (m.Msg == WM_SYSKEYDOWN))
                    {
                        bck(true);
                    }
                    else
                    {
                        bck(false);
                    }
                    break;
                case 114: // <--- F3 Key
                    if (!checkBox5.Checked) return false; 
  
                    if (m.Msg == WM_KEYDOWN)
                    {
                        if (!checkBox3.Checked) break;

                        vgate.MicMute = checkBox3.Checked = false;
                    }
                    else
                    {
                        vgate.MicMute = checkBox3.Checked = true;
                    }

                    break;
            }

            return false;
        }

        private void up(bool goup)
        {
            //client.Self.Movement.SendManualUpdate(AgentManager.ControlFlags.AGENT_CONTROL_UP_POS, client.Self.Movement.Camera.Position,
            //        client.Self.Movement.Camera.AtAxis, client.Self.Movement.Camera.LeftAxis, client.Self.Movement.Camera.UpAxis,
            //        client.Self.Movement.BodyRotation, client.Self.Movement.HeadRotation, client.Self.Movement.Camera.Far, AgentFlags.None,
            //        AgentState.None, true);

            if (goup)
            {
                client.Self.Movement.AutoResetControls = false;
                client.Self.Movement.UpPos = true;
                client.Self.Movement.SendUpdate();
            }
            else
            {
                client.Self.Movement.UpPos = false;
                client.Self.Movement.SendUpdate();
                client.Self.Movement.AutoResetControls = true;
            }
        }

        private void dwn(bool godown)
        {
            //client.Self.Movement.SendManualUpdate(AgentManager.ControlFlags.AGENT_CONTROL_UP_NEG, client.Self.Movement.Camera.Position,
            //        client.Self.Movement.Camera.AtAxis, client.Self.Movement.Camera.LeftAxis, client.Self.Movement.Camera.UpAxis,
            //        client.Self.Movement.BodyRotation, client.Self.Movement.HeadRotation, client.Self.Movement.Camera.Far, AgentFlags.None,
            //        AgentState.None, true);

            if (godown)
            {
                client.Self.Movement.AutoResetControls = false;
                client.Self.Movement.UpNeg = true;
                client.Self.Movement.SendUpdate();
            }
            else
            {
                client.Self.Movement.UpNeg = false;
                client.Self.Movement.SendUpdate();
                client.Self.Movement.AutoResetControls = true;
            }
        }

        private void fwd(bool goforward)
        {
            //client.Self.Movement.SendManualUpdate(AgentManager.ControlFlags.AGENT_CONTROL_AT_POS, client.Self.Movement.Camera.Position,
            //        client.Self.Movement.Camera.AtAxis, client.Self.Movement.Camera.LeftAxis, client.Self.Movement.Camera.UpAxis,
            //        client.Self.Movement.BodyRotation, client.Self.Movement.HeadRotation, client.Self.Movement.Camera.Far, AgentFlags.None,
            //        AgentState.None, true);

            if (goforward)
            {
                client.Self.Movement.AutoResetControls = false;
                client.Self.Movement.AtPos = true;
                client.Self.Movement.SendUpdate();
            }
            else
            {
                client.Self.Movement.AtPos = false;
                client.Self.Movement.SendUpdate();
                client.Self.Movement.AutoResetControls = true;
            }
        }

        private void bck(bool goback)
        {
            //client.Self.Movement.SendManualUpdate(AgentManager.ControlFlags.AGENT_CONTROL_AT_NEG, client.Self.Movement.Camera.Position,
            //        client.Self.Movement.Camera.AtAxis, client.Self.Movement.Camera.LeftAxis, client.Self.Movement.Camera.UpAxis,
            //        client.Self.Movement.BodyRotation, client.Self.Movement.HeadRotation, client.Self.Movement.Camera.Far, AgentFlags.None,
            //        AgentState.None, true);

            if (goback)
            {
                client.Self.Movement.AutoResetControls = false;
                client.Self.Movement.AtNeg = true;
                client.Self.Movement.SendUpdate();
            }
            else
            {
                client.Self.Movement.AtNeg = false;
                client.Self.Movement.SendUpdate();
                client.Self.Movement.AutoResetControls = true;
            }
        }

        private void lft()
        {
            //client.Self.Movement.SendManualUpdate(AgentManager.ControlFlags.AGENT_CONTROL_LEFT_POS, client.Self.Movement.Camera.Position,
            //        client.Self.Movement.Camera.AtAxis, client.Self.Movement.Camera.LeftAxis, client.Self.Movement.Camera.UpAxis,
            //        client.Self.Movement.BodyRotation, client.Self.Movement.HeadRotation, client.Self.Movement.Camera.Far, AgentFlags.None,
            //        AgentState.None, true);

            //// turn left

            //client.Self.AnimationStart(Animations.TURNLEFT, false);

            //ahead += 45.0;
            //if (ahead > 360) ahead = 135.0;

            //client.Self.Movement.TurnRight = false;
            //client.Self.Movement.TurnLeft = true;

            //client.Self.Movement.UpdateFromHeading(ahead, true);

            //client.Self.Movement.FinishAnim = true;
            //System.Threading.Thread.Sleep(200);
            //client.Self.AnimationStop(Animations.TURNLEFT, false);

            client.Self.Movement.TurnRight = false;
            client.Self.Movement.TurnLeft = true;
            client.Self.Movement.BodyRotation = client.Self.Movement.BodyRotation * Quaternion.CreateFromAxisAngle(Vector3.UnitZ, 45f);
            client.Self.Movement.SendUpdate(true);
            System.Threading.Thread.Sleep(500);
            client.Self.Movement.TurnLeft = false;
            client.Self.Movement.SendUpdate(true);
        }

        private void rgt()
        {
            //client.Self.Movement.SendManualUpdate(AgentManager.ControlFlags.AGENT_CONTROL_LEFT_NEG, client.Self.Movement.Camera.Position,
            //        client.Self.Movement.Camera.AtAxis, client.Self.Movement.Camera.LeftAxis, client.Self.Movement.Camera.UpAxis,
            //        client.Self.Movement.BodyRotation, client.Self.Movement.HeadRotation, client.Self.Movement.Camera.Far, AgentFlags.None,
            //        AgentState.None, true);


            //// turn right

            //client.Self.AnimationStart(Animations.TURNRIGHT, false);

            //ahead += -45.0;
            //if (ahead > 360) ahead = 135.0;

            //client.Self.Movement.UpdateFromHeading(ahead, true);

            //client.Self.Movement.FinishAnim = true;
            //System.Threading.Thread.Sleep(200);
            //client.Self.AnimationStop(Animations.TURNRIGHT, false);

            client.Self.Movement.TurnLeft = false;
            client.Self.Movement.TurnRight = true;
            client.Self.Movement.BodyRotation = client.Self.Movement.BodyRotation * Quaternion.CreateFromAxisAngle(Vector3.UnitZ, -45f);
            client.Self.Movement.SendUpdate(true);
            System.Threading.Thread.Sleep(500);
            client.Self.Movement.TurnRight = false;
            client.Self.Movement.SendUpdate(true);
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

            CurrentSIM = e.Simulator;

            if (InvokeRequired)
            {

                BeginInvoke(new MethodInvoker(delegate()
                {
                    try
                    {
                        Grid_OnCoarseLocationUpdate(sender, e);
                    }
                    catch { ; }
                }));
                
                return;
            }

            List<UUID> tremove = new List<UUID>();
            tremove = e.RemovedEntries;

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

                lock (instance.avnames)
                {
                    instance.avnames.Remove(id);
                }

                if (instance.State.IsFollowing)
                {
                    if (id == instance.State.FollowID)
                    {
                        instance.State.Follow(string.Empty, UUID.Zero);
                        tbtnFollow.ToolTipText = "Follow";
                    }
                }
            }

            e.Simulator.AvatarPositions.ForEach(delegate(KeyValuePair<UUID, Vector3> favpos)
                    {
                        if (!instance.avnames.ContainsKey(favpos.Key))
                        {
                            client.Avatars.RequestAvatarName(favpos.Key);
                        }
                    });

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
                lvwRadar.Items.Clear();
            }));

            _MapLayer = null;

            //GetMap();
            BeginInvoke((MethodInvoker)delegate { GetMap(); });
        }

        private void GetMap()
        {
            if (instance.Config.CurrentConfig.DisableRadar) return;

            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    GetMap();
                }));

                return;
            }

            GridRegion region;

            label11.Text = "Map downloading...";

            if (_MapLayer == null || sim != client.Network.CurrentSim)
            {
                world.Image = null; 
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
                            else
                            {
                                label11.Text = "Map unavailable"; 
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
            if (this.InvokeRequired) this.BeginInvoke((MethodInvoker)delegate { UpdateMiniMap(ssim); });
            else
            {
                try
                {
                    if (instance.Config.CurrentConfig.DisableRadar) return;

                    if (ssim != client.Network.CurrentSim) return;

                    //Bitmap nbmp = new Bitmap(256, 256);

                    Bitmap bmp = _MapLayer == null ? new Bitmap(256, 256) : (Bitmap)_MapLayer.Clone();
                    Graphics g = Graphics.FromImage(bmp);

                    //nbmp.Dispose(); 

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
                        //ssim = client.Network.Simulators[0];

                        label4.Text = "Ttl objects: " + ssim.Stats.Objects.ToString();
                        label5.Text = "Scripted objects: " + ssim.Stats.ScriptedObjects.ToString();
                        label8.Text = client.Network.CurrentSim.Name;

                        Simulator csim = client.Network.CurrentSim;

                        label9.Text = "FPS: " + csim.Stats.FPS.ToString();

                        // Maximum value changes for OSDGrid compatibility V 0.9.32.0

                        if (csim.Stats.FPS > progressBar7.Maximum)
                        {
                            progressBar7.Maximum = csim.Stats.FPS;
                        }

                        progressBar7.Value = csim.Stats.FPS;

                        label15.Text = "Dilation: " + csim.Stats.Dilation.ToString();

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

                    try
                    {
                        string[] svers = ssim.SimVersion.Split(' ');
                        var e = from s in svers
                                select s;

                        int cnt = e.Count() - 1;

                        try
                        {
                            label3.Text = svers[0] + " " + svers[1] + " " + svers[2] + " " + svers[3];
                            label12.Text = svers[cnt];
                        }
                        catch
                        {
                            ;
                        }
                    }
                    catch
                    {
                        label12.Text = "na";
                    }

                    strInfo = string.Format("Ttl Avatars: {0}", ssim.AvatarPositions.Count);
                    label6.Text = strInfo;

                    int i = 0;

                    instance.avlocations.Clear();

                    if (myPos.Z < 0.1f)
                    {
                        myPos.Z = 1024f;
                    }

                    // Draw self position

                    if (!instance.Config.CurrentConfig.DisableRadarImageMiniMap)
                    {
                        int rg = instance.Config.CurrentConfig.RadarRange;

                        Rectangle myrect;

                        if (rg < 150)
                        {
                            rg *= 2;

                            myrect = new Rectangle(((int)Math.Round(myPos.X, 0)) - (rg / 2), (255 - ((int)Math.Round(myPos.Y, 0))) - (rg / 2 - 4), rg + 2, rg + 2);
                            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 255));
                            g.CompositingQuality = CompositingQuality.GammaCorrected;
                            g.FillEllipse(semiTransBrush, myrect);

                            myrect = new Rectangle(((int)Math.Round(myPos.X, 0)) - (rg / 4), (255 - ((int)Math.Round(myPos.Y, 0))) - (rg / 4 - 4), rg / 2 + 2, rg / 2 + 2);
                            //semiTransBrush = new SolidBrush(Color.FromArgb(128, 0, 245, 225));
                            g.DrawEllipse(new Pen(Color.Blue, 1), myrect);
                        }
                    }

                    ssim.AvatarPositions.ForEach(
                    delegate(KeyValuePair<UUID, Vector3> pos)
                    {
                        if (pos.Key != client.Self.AgentID)
                        {
                            bool restrict = false;

                            if (!instance.avnames.ContainsKey(pos.Key))
                            {
                                client.Avatars.RequestAvatarName(pos.Key);
                            }

                            Vector3 oavPos = new Vector3(0, 0, 0);
                            oavPos.X = pos.Value.X;
                            oavPos.Y = pos.Value.Y;
                            oavPos.Z = pos.Value.Z;

                            if (oavPos.Z < 0.1f)
                            {
                                oavPos.Z = 1024f;
                            }

                            Avatar fav = new Avatar();
                            fav = ssim.ObjectsAvatars.Find((Avatar av) => { return av.ID == pos.Key; });

                            string st = string.Empty;

                            if (fav != null)
                            {
                                oavPos = fav.Position;
                                uint sobj = fav.ParentID;

                                if (sobj != 0)
                                {
                                    st = "*";

                                    Primitive prim;
                                    client.Network.CurrentSim.ObjectsPrimitives.TryGetValue(sobj, out prim);

                                    if (prim != null)
                                    {
                                        oavPos = prim.Position + oavPos;
                                    }
                                }
                            }

                            if (instance.Config.CurrentConfig.RestrictRadar)
                            {
                                double dist = Math.Round(Vector3d.Distance(ConverToGLobal(myPos), ConverToGLobal(oavPos)), MidpointRounding.ToEven);

                                if (instance.Config.CurrentConfig.RadarRange < Convert.ToInt32(dist))
                                {
                                    restrict = true;

                                    if (instance.avnames.ContainsKey(pos.Key))
                                    {
                                        string name = instance.avnames[pos.Key];

                                        lvwRadar.BeginUpdate();
                                        if (lvwRadar.Items.ContainsKey(name))
                                        {
                                            lvwRadar.Items.RemoveByKey(name);
                                        }
                                        lvwRadar.EndUpdate();
                                    }
                                }
                            }

                            if (!restrict)
                            {
                                int x = (int)oavPos.X - 2;
                                int y = 255 - (int)oavPos.Y - 2;

                                rect = new Rectangle(x, y, 6, 6);

                                if (myPos.Z - oavPos.Z > 20)
                                {
                                    g.FillRectangle(Brushes.DarkRed, rect);
                                    g.DrawRectangle(new Pen(Brushes.Red, 1), rect);
                                }
                                else if (myPos.Z - oavPos.Z > -11 && myPos.Z - oavPos.Z < 11)
                                {
                                    g.FillEllipse(Brushes.LightGreen, rect);
                                    g.DrawEllipse(new Pen(Brushes.Green, 1), rect);
                                }
                                else
                                {
                                    g.FillRectangle(Brushes.MediumBlue, rect);
                                    g.DrawRectangle(new Pen(Brushes.Red, 1), rect);
                                }

                                Point mouse = new Point(x, y);

                                instance.avlocations.Add(new METAboltInstance.AvLocation(mouse, rect.Size, pos.Key.ToString(), string.Empty, oavPos));

                                try
                                {
                                    Color aclr = Color.Black;                                    

                                    if (fav == null)
                                    {
                                        aclr = Color.RoyalBlue;
                                    }
                                    else
                                    {
                                        if (!instance.avtags.ContainsKey(fav.ID))
                                        {
                                            instance.avtags.Add(fav.ID, fav.GroupName);
                                        }
                                    }

                                    if (instance.avnames.ContainsKey(pos.Key))
                                    {
                                        string name = instance.avnames[pos.Key];
                                        BeginInvoke(new OnAddSIMAvatar(AddSIMAvatar), new object[] { name, pos.Key, oavPos, aclr, st });
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.Log("UpdateMiniMap: " + ex.Message, Helpers.LogLevel.Warning);
                                }
                            }
                        }

                        i++;
                    }
                    );

                    g.DrawImage(bmp, 0, 0);

                    myrect = new Rectangle((int)Math.Round(myPos.X, 0) - 2, 255 - ((int)Math.Round(myPos.Y, 0) - 2), 6, 6);
                    g.FillEllipse(new SolidBrush(Color.Yellow), myrect);
                    g.DrawEllipse(new Pen(Brushes.Red, 2), myrect);

                    // Draw compass points
                    StringFormat strFormat = new StringFormat();
                    strFormat.Alignment = StringAlignment.Center;

                    g.DrawString("N", new Font("Arial", 13), Brushes.Black, new RectangleF(0, 2, bmp.Width, bmp.Height), strFormat);
                    g.DrawString("N", new Font("Arial", 10, FontStyle.Bold), Brushes.White, new RectangleF(0, 2, bmp.Width, bmp.Height), strFormat);

                    strFormat.LineAlignment = StringAlignment.Center;
                    strFormat.Alignment = StringAlignment.Near;

                    g.DrawString("W", new Font("Arial", 13), Brushes.Black, new RectangleF(0, 0, bmp.Width, bmp.Height), strFormat);
                    g.DrawString("W", new Font("Arial", 10, FontStyle.Bold), Brushes.White, new RectangleF(2, 0, bmp.Width, bmp.Height), strFormat);

                    strFormat.LineAlignment = StringAlignment.Center;
                    strFormat.Alignment = StringAlignment.Far;

                    g.DrawString("E", new Font("Arial", 13), Brushes.Black, new RectangleF(0, 0, bmp.Width, bmp.Height), strFormat);
                    g.DrawString("E", new Font("Arial", 10, FontStyle.Bold), Brushes.White, new RectangleF(-2, 0, bmp.Width, bmp.Height), strFormat);

                    strFormat.LineAlignment = StringAlignment.Far;
                    strFormat.Alignment = StringAlignment.Center;

                    g.DrawString("S", new Font("Arial", 13), Brushes.Black, new RectangleF(0, 0, bmp.Width, bmp.Height), strFormat);
                    g.DrawString("S", new Font("Arial", 10, FontStyle.Bold), Brushes.White, new RectangleF(0, 0, bmp.Width, bmp.Height), strFormat);

                    world.Image = bmp;
                    //world.Cursor = Cursors.NoMove2D;

                    strFormat.Dispose(); 
                    g.Dispose();

                    if (lastPos != myPos)
                    {
                        lastPos = myPos; 
                        GetCompass();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log("Chatconsole MiniMap: " + ex.Message, Helpers.LogLevel.Error);
                    //return;
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                if (!instance.LoggedIn) return;

                toolStrip1.Visible = true;

                label6.Text = string.Empty;
                label4.Text = string.Empty;
                label5.Text = string.Empty;
                label8.Text = string.Empty;
                label9.Text = string.Empty;
                label15.Text = string.Empty;
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                client.Grid.RequestMapRegion(client.Network.CurrentSim.Name, GridLayerType.Objects);

                //GetMap();
                BeginInvoke((MethodInvoker)delegate { GetMap(); });

                toolStrip1.Visible = false;
            }
            else if (tabControl1.SelectedTab == tabPage3)
            {
                toolStrip1.Visible = false;
            }
            else
            {
                toolStrip1.Visible = false;
                List<InventoryBase> invroot = client.Inventory.Store.GetContents(client.Inventory.Store.RootFolder.UUID);

                foreach (InventoryBase o in invroot)
                {
                    if (o.Name.ToLower() == "favorites" || o.Name.ToLower() == "my favorites")
                    {
                        if (o is InventoryFolder)
                        {
                            client.Inventory.RequestFolderContents(o.UUID, client.Self.AgentID, true, true, InventorySortOrder.ByDate);

                            break;
                        }
                    }
                }
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

        private Avatar GetAvID()
        {
            Avatar nav = new Avatar();
            //nav = e.Simulator.ObjectsAvatars.Find((Avatar fnav) => { return fnav.ID == favpos.Key; });

            UUID avid = (UUID)lvwRadar.SelectedItems[0].Tag;

            //nav = client.Network.CurrentSim.ObjectsAvatars.Find(delegate(Avatar avr)
            //{
            //    return avr.ID == avid;
            //});

            nav = CurrentSIM.ObjectsAvatars.Find((Avatar av) => { return av.ID == avid; });

            return nav;
        }

        private void tbtnAttachments_Click(object sender, EventArgs e)
        {
            ////Avatar av = ((ListViewItem)lvwRadar.SelectedItems[0]).Tag as Avatar;
            ////if (av == null) return;

            UUID av = (UUID)lvwRadar.SelectedItems[0].Tag;

            if (av == UUID.Zero || av == null) return;

            //string name = instance.avnames[av];

            Avatar sav = new Avatar();
            sav = GetAvID(); 

            //sav = client.Network.CurrentSim.ObjectsAvatars.Find(delegate(Avatar fa)
            // {
            //     return fa.ID == av;
            // }
            // );

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
            //newsize = tabPage2.Width - 40;

            //px = world.Top;
            //py = world.Left;

            //System.Drawing.Size sz = new Size();
            //sz.Height = newsize;
            //sz.Width = newsize;

            //panel6.Size = sz;

            //lvwRadar.Columns[0].Width = lvwRadar.Width - 3;

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
                rtbChat.Height += 28; 
            }
            else
            {
                panel7.Visible = true;
                tsbSearch.ToolTipText = "Hide chat search";
                rtbChat.Height -= 28;
            }
        }

        private void tsFindText_Click(object sender, EventArgs e)
        {
            tsFindText.SelectionStart = 0;
            tsFindText.SelectionLength = tsFindText.Text.Length;
        }

        private void vgate_OnVoiceConnectionChange(VoiceGateway.ConnectionState state)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    vgate_OnVoiceConnectionChange(state);
                }));

                return;
            }

            try
            {
                string s = string.Empty;

                if (state == VoiceGateway.ConnectionState.AccountLogin)
                {
                    s = "Logging In...";
                }
                else if (state == VoiceGateway.ConnectionState.ConnectorConnected)
                {
                    s = "Connected...";
                }
                else if (state == VoiceGateway.ConnectionState.DaemonConnected)
                {
                    s = "Daemon Connected. Starting...";
                }
                else if (state == VoiceGateway.ConnectionState.DaemonStarted)
                {
                    s = "Daemon Started. Please wait...";
                }
                else if (state == VoiceGateway.ConnectionState.SessionRunning)
                {
                    s = "Session Started & Ready";
                }

                label18.Text = s;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "METAbolt");
            }
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
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    vgate_OnSessionCreate(sender, e);
                }));

                return;
            }

            try
            {
                vgate.AuxGetCaptureDevices();
                vgate.AuxGetRenderDevices();

                vgate.MicMute = true;
                vgate.SpkrMute = false;
                vgate.SpkrLevel = 70;
                vgate.MicLevel = 70;
                checkBox5.ForeColor = Color.Red;
                label18.Text = "Session Started & Ready";
                EnableVoice(true);

                if (!checkBox3.Checked)
                {
                    checkBox3.Checked = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "METAbolt");
            }
        }

        private void LoadMics(List<string> list)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    LoadMics(list);
                }));

                return;
            }

            try 
            {
                cboCapture.Items.Clear();

                foreach (string dev in list)
                {
                    cboCapture.Items.Add(dev);  
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "METAbolt"); }

            try
            {
                string cmic = vgate.CurrentCaptureDevice;

                if (string.IsNullOrEmpty(cmic))
                {
                    cboCapture.SelectedItem = cmic;    //cmic = mics[0];
                }
                else
                {
                    cboCapture.Text = cmic;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "METAbolt"); }

            vgate.MicMute = true;
            vgate.MicLevel = 70;
        }

        private void LoadSpeakers(List<string> list)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    LoadSpeakers(list);
                }));

                return;
            }

            try
            {
                cboRender.Items.Clear();
   
                foreach (string dev in list)
                {
                    cboRender.Items.Add(dev);   
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "METAbolt"); }

            try
            {
                string cspk = vgate.PlaybackDevice;

                if (string.IsNullOrEmpty(cspk))
                {
                    cboRender.SelectedItem = cspk; //speakers[0];
                }
                else
                {
                    cboRender.Text = cspk;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "METAbolt"); }

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

        private void button10_Click(object sender, EventArgs e)
        {
            
        }

        private bool CheckVoiceSetupFile(string filename)
        {
            if (!System.IO.File.Exists(Application.StartupPath.ToString() + "\\" + filename))
            {
                MessageBox.Show("The required '" + filename + "' file was not found.\nPlease read the wiki page below for instructions:\n\nhttp://www.metabolt.net/METAwiki/How-to-enable-VOICE.ashx?NoRedirect=1");
                return(false);
            }
            return (true);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.CheckVoiceSetupFile("SLVoice.exe")) return;
            if (!this.CheckVoiceSetupFile("alut.dll")) return;
            //if (!this.CheckVoiceSetupFile("openal32.dll")) return;
            if (!this.CheckVoiceSetupFile("ortp.dll")) return;
            if (!this.CheckVoiceSetupFile("vivoxsdk.dll")) return;
            if (!this.CheckVoiceSetupFile("wrap_oal.dll")) return;

            if (checkBox5.Checked)
            {
                if (!instance.AllowVoice)
                {
                    label18.Text = "Voice is disabled on this parcel";
                    
                    return;
                }

                try
                {
                    vgate = new VoiceGateway(client);
                    vgate.OnVoiceConnectionChange += new VoiceGateway.VoiceConnectionChangeCallback(vgate_OnVoiceConnectionChange);
                    vgate.OnAuxGetCaptureDevicesResponse += new EventHandler<VoiceGateway.VoiceDevicesEventArgs>(vgate_OnAuxGetCaptureDevicesResponse);
                    vgate.OnAuxGetRenderDevicesResponse += new EventHandler<VoiceGateway.VoiceDevicesEventArgs>(vgate_OnAuxGetRenderDevicesResponse);
                    vgate.OnSessionCreate += new EventHandler(vgate_OnSessionCreate);

                    vgate.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "METAbolt");
                }
            }
            else
            {
                if (!instance.AllowVoice)
                {
                    label18.Text = "Voice is disabled on this parcel";

                    return;
                }

                try
                {
                    vgate.MicMute = true;
                    vgate.Stop();
                    vgate.Dispose();

                    EnableVoice(false);
                    cboRender.Items.Clear();
                    cboCapture.Items.Clear();   

                    vgate.OnVoiceConnectionChange -= new VoiceGateway.VoiceConnectionChangeCallback(vgate_OnVoiceConnectionChange);
                    vgate.OnAuxGetCaptureDevicesResponse -= new EventHandler<VoiceGateway.VoiceDevicesEventArgs>(vgate_OnAuxGetCaptureDevicesResponse);
                    vgate.OnAuxGetRenderDevicesResponse -= new EventHandler<VoiceGateway.VoiceDevicesEventArgs>(vgate_OnAuxGetRenderDevicesResponse);
                    vgate.OnSessionCreate -= new EventHandler(vgate_OnSessionCreate);

                    if (!checkBox3.Checked)
                    {
                        checkBox3.Checked = true;
                    }

                    checkBox5.ForeColor = Color.Black;
                    label18.Text = "Check 'Voice ON' box below. Then on 'Session start' unmute MIC to talk";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "METAbolt");
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
            toolTip1.SetToolTip(trackBar1, "Volume: " + trackBar1.Value.ToString()); 
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            vgate.SpkrLevel = trackBar2.Value;
            toolTip1.SetToolTip(trackBar2, "Volume: " + trackBar2.Value.ToString());
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (!avrezzed && netcom.IsLoggedIn)
            {
                client.Appearance.RequestSetAppearance(true);
                timer2.Enabled = false;
                timer2.Stop();
            }  
        }

        private void toolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {

        }

        private void world_Click(object sender, EventArgs e)
        {

        }

        private void tbtnHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://www.metabolt.net/metawiki/Quick.aspx");
        }

        private void picAutoSit_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://www.metabolt.net/metawiki/How-to-enable-VOICE.ashx");
        }

        private void button5_KeyDown(object sender, KeyEventArgs e)
        {
            fwd(true);
        }

        private void button5_KeyUp(object sender, KeyEventArgs e)
        {
            fwd(false);
        }

        private void button4_KeyDown(object sender, KeyEventArgs e)
        {
            bck(true);
        }

        private void button4_KeyUp(object sender, KeyEventArgs e)
        {
            bck(false);
        }

        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            fwd(true);
        }

        private void button5_MouseUp(object sender, MouseEventArgs e)
        {
            fwd(false);
        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            bck(true);
        }

        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            bck(false);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //WalkRight();
        }

        private void WalkRight()
        {
            client.Self.Movement.SendManualUpdate(AgentManager.ControlFlags.AGENT_CONTROL_LEFT_NEG, client.Self.Movement.Camera.Position,
                    client.Self.Movement.Camera.AtAxis, client.Self.Movement.Camera.LeftAxis, client.Self.Movement.Camera.UpAxis,
                    client.Self.Movement.BodyRotation, client.Self.Movement.HeadRotation, client.Self.Movement.Camera.Far, AgentFlags.None,
                    AgentState.None, true);
        }

        private void button6_MouseDown(object sender, MouseEventArgs e)
        {
            WalkRight();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //WalkLeft();
        }

        private void WalkLeft()
        {
            client.Self.Movement.SendManualUpdate(AgentManager.ControlFlags.AGENT_CONTROL_LEFT_POS, client.Self.Movement.Camera.Position,
                    client.Self.Movement.Camera.AtAxis, client.Self.Movement.Camera.LeftAxis, client.Self.Movement.Camera.UpAxis,
                    client.Self.Movement.BodyRotation, client.Self.Movement.HeadRotation, client.Self.Movement.Camera.Far, AgentFlags.None,
                    AgentState.None, true);
        }

        private void button8_MouseDown(object sender, MouseEventArgs e)
        {
            WalkLeft();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void picVoice_MouseHover(object sender, EventArgs e)
        {
            tTip.Show(picVoice);
        }

        private void picVoice_MouseLeave(object sender, EventArgs e)
        {
            tTip.Close(); 
        }

        private void picMap_MouseHover(object sender, EventArgs e)
        {
            tTip1.Show(picMap);
        }

        private void picMap_MouseLeave(object sender, EventArgs e)
        {
            tTip1.Close(); 
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton1_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://www.bing.com/");
        }

        private void rtbChat_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void lvwRadar_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            ////e.DrawBackground();

            ////if (e.ItemIndex < 0) return;

            ////ListViewItem itemToDraw = lvwRadar.Items[e.ItemIndex];

            ////if ((e.State & ListViewItemStates.Selected) != 0)
            ////{
            ////    // Draw the background and focus rectangle for a selected item.
            ////    e.Graphics.FillRectangle(Brushes.Maroon, e.Bounds);
            ////    e.DrawFocusRectangle();
            ////}
            ////else
            ////{
            ////    // Draw the background for an unselected item. 
            ////    using (LinearGradientBrush brush =
            ////        new LinearGradientBrush(e.Bounds, Color.Orange,
            ////        Color.Maroon, LinearGradientMode.Horizontal))
            ////    {
            ////        e.Graphics.FillRectangle(brush, e.Bounds);
            ////    }
            ////}

            ////// Draw the item text for views other than the Details view.
            ////if (lvwRadar.View != View.Details)
            ////{
            ////    e.DrawText();
            ////}

            e.DrawBackground();
            

            ////TextFormatFlags flags = TextFormatFlags.Left;

            //ListViewItem itemToDraw = lvwRadar.Items[e.ItemIndex];

            ////Brush textBrush = null;
            ////Brush dBrush = null;
            ////Brush rBrush = null;
            ////Font boldFont = new Font("Arial", 8, FontStyle.Bold);
            ////Font regularFont = new Font("Arial", 8, FontStyle.Regular);
            ////Font italicFont = new Font("Arial", 7, FontStyle.Italic);

            //if ((e.State & ListViewItemStates.Selected) != 0)
            //{
            //    //textBrush = new SolidBrush(Color.FromKnownColor(KnownColor.HighlightText));
            //    //dBrush = new SolidBrush(Color.Yellow);
            //    e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            //    e.DrawFocusRectangle();
            //}
            //else
            //{
            //    //textBrush = new SolidBrush(Color.FromKnownColor(KnownColor.ControlText));
            //    //dBrush = new SolidBrush(Color.RoyalBlue);
            //    e.Graphics.FillRectangle(Brushes.RoyalBlue, e.Bounds);
            //}

            ////float nameX = e.Bounds.Location.X;
            ////float nameY = e.Bounds.Location.Y;

            //string name = string.Empty;
            ////string description = string.Empty;
            ////string distance = string.Empty;

            //name = itemToDraw.Name;

            //if (avtyping.Contains(name))
            //{
            //    //rBrush = new SolidBrush(Color.Red);
            //    //e.Graphics.DrawString(name, regularFont, rBrush, nameX, nameY);
            //}
            //else if (client.Self.AgentID == (UUID)itemToDraw.Tag)
            //{
            //    //e.Graphics.DrawString(name, boldFont, textBrush, nameX, nameY);
            //    e.Graphics.DrawImage(Properties.Resources.green_orb, e.Bounds.Location);
            //}
            //else
            //{
            //    //e.Graphics.DrawString(name, regularFont, textBrush, nameX, nameY);
            //}

            ////e.Graphics.DrawLine(new Pen(Color.FromArgb(200, 200, 200)), new Point(e.Bounds.Left, e.Bounds.Bottom - 1), new Point(e.Bounds.Right, e.Bounds.Bottom - 1));
            //////e.DrawFocusRectangle();

            ////boldFont.Dispose();
            ////regularFont.Dispose();
            ////textBrush.Dispose();
            ////boldFont = null;
            ////regularFont = null;
            ////textBrush = null;
            ////dBrush = null;
            ////rBrush = null;

            ////e.DrawText(flags);

            //// Draw the item text for views other than the Details view.
            //if (lvwRadar.View != View.Details)
            //{
                e.DrawText();
            //}
        }

        public void UpdateFavourites(List<InventoryBase> foundfolders)
        {
            if (foundfolders == null) return;

            if (foundfolders.Count > 0)
            {
                tsFavs.Visible = true;
                tsFavs.Items.Clear();

                foreach (InventoryBase oitem in foundfolders)
                {
                    InventoryItem item = (InventoryItem)oitem;

                    if (item.InventoryType == InventoryType.Landmark)
                    {
                        string iname = item.Name;
                        string desc = item.Description;

                        //int twh = tabPage4.Width; 

                        if (iname.Length > 48)
                        {
                            iname = iname.Substring(0, 48) + "...";
                        }

                        ToolStripButton btn = new ToolStripButton(iname, null, FavsToolStripMenuItem_Click, item.AssetUUID.ToString());

                        //if (!tsFavs.Items.Contains(btn))
                        //{
                        btn.TextAlign = ContentAlignment.MiddleLeft;
                        btn.ToolTipText = desc;
                        tsFavs.Items.Add(btn);

                        ToolStripSeparator sep = new ToolStripSeparator();
                        tsFavs.Items.Add(sep);
                        //}
                    }
                }
            }
        }

        private void FavsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cbtn = sender.ToString();

            ToolStripButton btn = (ToolStripButton)sender;
            UUID landmark = new UUID();

            if (!UUID.TryParse(btn.Name, out landmark))
            {
                MessageBox.Show("Invalid Landmark", "Teleport");
                return;
            }

            client.Self.Teleport(landmark);

            //if (client.Self.Teleport(landmark))
            //{
            //    MessageBox.Show("Teleport Succesful", "Teleport");
            //}
            //else
            //{
            //    MessageBox.Show("Teleport Failed", "Teleport");
            //}
        }

        private void rtbChat_Click(object sender, EventArgs e)
        {
            //rtbChat.HideSelection = true;
            //SendMessage(rtbChat.Handle, EM_HIDESELECTION, 1, 0);
            //cpos = rtbChat.SelectionStart;

            //LockWindow(this.Handle);
        }

        private void rtbChat_Leave(object sender, EventArgs e)
        {
            //rtbChat.HideSelection = false;

            //SendMessage(rtbChat.Handle, EM_HIDESELECTION, 0, 0);

            //LockWindow(IntPtr.Zero);
        }

        private void rtbChat_Enter(object sender, EventArgs e)
        {
            //rtbChat.HideSelection = true;
            //SendMessage(rtbChat.Handle, EM_HIDESELECTION, 1, 0);
        }

        private void world_DoubleClick(object sender, EventArgs e)
        {
            (new frmMapClient(instance)).Show();
        }

        private void lvwRadar_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                ListViewItem item = lvwRadar.GetItemAt(e.X, e.Y);
                ListViewHitTestInfo info = lvwRadar.HitTest(e.X, e.Y);

                if (item != null)
                {
                    if (tooltiptext != info.Item.ToolTipText)
                    {
                        tooltiptext = info.Item.ToolTipText;
                        toolTip.SetToolTip(lvwRadar, info.Item.ToolTipText);
                    }
                }
                else
                {
                    tooltiptext = string.Empty;
                    toolTip.SetToolTip(lvwRadar, null);
                }
            }
            catch
            {
                try
                {
                    tooltiptext = string.Empty;
                    toolTip.SetToolTip(lvwRadar, null);
                }
                catch { ; }
            }
        }

        private void lvwRadar_SizeChanged(object sender, EventArgs e)
        {
            lvwRadar.Columns[0].Width = lvwRadar.Width - 3;
        }

        private void tabPage2_SizeChanged(object sender, EventArgs e)
        {
            newsize = tabPage2.Width - 40;

            px = world.Top;
            py = world.Left;

            System.Drawing.Size sz = new Size();
            sz.Height = newsize;
            sz.Width = newsize;

            panel6.Size = sz;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            label13.Text = "Teleporting...";

            TPtimer.Stop();
            TPtimer.Enabled = false;

            pTP.Visible = false; 
        }

        private void TPtimer_Tick(object sender, EventArgs e)
        {
            pTP.Visible = false;
            label13.Text = "Teleporting...";

            TPtimer.Stop();
            TPtimer.Enabled = false;
        }

        private void rtbChat_SizeChanged(object sender, EventArgs e)
        {
            pTP.Location = new Point(
            rtbChat.Width / 2 - pTP.Size.Width / 2,
            rtbChat.Height / 2 - pTP.Size.Height / 2);
            pTP.Anchor = AnchorStyles.None;
        }

        //private void lvwRadar_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        //{
        //    if (e.ColumnIndex == 1)
        //    {
        //        e.Graphics.DrawImage(Properties.Resources.green_orb, e.SubItem.Bounds.Location);
        //        e.Item.UseItemStyleForSubItems = false;
        //    }
        //}

        //private void lvwRadar_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        //{
        //    e.DrawDefault = true;
        //}
    }
}
