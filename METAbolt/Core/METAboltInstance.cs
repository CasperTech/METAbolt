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
using System.Text;
using System.Windows.Forms;
using SLNetworkComm;
using OpenMetaverse;
using System.Deployment.Application;
using System.Reflection;
using AIMLbot;
using System.Data;
using System.IO;
using METAxCommon;
using System.Drawing;
using ExceptionReporting;
using System.Threading;
using System.Globalization;


namespace METAbolt
{
    public class METAboltInstance
    {
        //public event Action<METAboltForm> METAboltFormCreated;
        //public virtual void OnMETAboltFormCreated(METAboltForm form)
        //{
        //    if (METAboltFormCreated != null) METAboltFormCreated(form);
        //}

        public class AvLocation
        {
            public Rectangle Rectangle { get; set; }
            public string LocationName { get; set; }
            public Vector3 Position { get; set; }
            public string Tag { get; set; }

            public AvLocation(Point location, Size size, string name, Vector3 pos)
            {
                LocationName = name;
                Position = pos;
                Rectangle = new Rectangle(location, size);
            }

            public AvLocation(Point location, Size size, string name, string tag, Vector3 pos)
            {
                LocationName = name;
                Position = pos;
                Tag = tag; 
                Rectangle = new Rectangle(location, size);
            }
        }

        private GridClient client;
        private SLNetCom netcom;

        private ImageCache imageCache;
        private StateManager state;
        private ConfigManager config;

        private frmMain mainForm;
        private TabsConsole tabsConsole;
        private IMbox imbox;

        private bool firstInstance;
        private bool otherInstancesOpen = false;

        private bool loggedin = false;
        private bool logoffclicked = false;
        private bool rebooted = false;

        private int dialogtimeout = 900000;
        private int dialogcount = 0;
        private int noticecount = 0;
        public AIMLbot.Bot myBot;
        //private DataTable mutelist = null;
        private DataTable tp = null;
        private bool detectlang = false;
        private UUID lookID = UUID.Zero; 
        private List<IExtension> elist;
        public SafeDictionary<UUID, string> avnames = new SafeDictionary<UUID, string>();
        public SafeDictionary<UUID, string> avtags = new SafeDictionary<UUID, string>();
        public List<AvLocation> avlocations = new List<AvLocation>();
        private ExceptionReporter reporter = new ExceptionReporter();
        public string appdir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt";
        public bool startfrombat = false;
        private DataTable giveritems = null;
        private bool readims = false;
        public InventoryConsole insconsole;
        private string afffile = string.Empty;
        private string dicfile = string.Empty;
        private bool allowvoice = true;
        private UUID favsfolder = UUID.Zero; 

        internal class ThreadExceptionHandler
        {
            public void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
            {
                ExceptionReporter reporter = new ExceptionReporter();
                reporter.Show(e.Exception);
            }
        }

        public METAboltInstance(bool firstInstance)
        {
            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);

            this.firstInstance = firstInstance;

            //LoadXMLFile(appdir + "\\MuteList.xml");
            LoadGiverItems(appdir + "\\METAgiverItems.xml");

            MakeTPTable();
            CreateLogDir();
            CreateNotesDir();
            CreateSpellingDir();

            client = new GridClient();

            config = new ConfigManager();
            config.ApplyDefault();

            SetSettings();

            netcom = new SLNetCom(client);
            InitializeConfig();

            state = new StateManager(this);

            imageCache = new ImageCache();

            mainForm = new frmMain(this);
            mainForm.InitializeControls();
            tabsConsole = mainForm.TabConsole;

            //CheckForShortcut();

            if (config.CurrentConfig.AIon)
            {
                //myBot = new AIMLbot.Bot();
                InitAI();
            }

            //InitAI();

            RandomPwd();
            //mutelist = LoadXMLFile("MuteList.xml");
            //mutelist.PrimaryKey = new DataColumn[] { mutelist.Columns["uuid"] };
        }

        public METAboltInstance(bool firstInstance, string[] args)
        {
            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);

            this.firstInstance = firstInstance;

            //LoadXMLFile(appdir + "\\MuteList.xml");
            LoadGiverItems(appdir + "\\METAgiverItems.xml");

            MakeTPTable();
            CreateLogDir();
            CreateNotesDir();
            CreateSpellingDir();

            client = new GridClient();

            //at this point we have been given: metabolt.exe [firstname] [lastname] [password]
            //so args[0] and args[1] have the necessary name parts
            if (args.Length > 1)
            {
                string full_name = args[0] + "_" + args[1];
                config = new ConfigManager(full_name);
                
            }
            else
            {
                config = new ConfigManager();
            }

            config.ApplyDefault();

            SetSettings();
            netcom = new SLNetCom(client);

            this.rebooted = true;
            startfrombat = true;

            config.CurrentConfig.FirstName = args[0].ToString();
            config.CurrentConfig.LastName = args[1].ToString();
            config.CurrentConfig.PasswordMD5 = args[2].ToString();

            InitializeConfig();

            imageCache = new ImageCache();
            state = new StateManager(this);

            mainForm = new frmMain(this);
            mainForm.InitializeControls();
            tabsConsole = mainForm.TabConsole;

            if (config.CurrentConfig.AIon)
            {
                //myBot = new AIMLbot.Bot();
                InitAI();
            }

            //InitAI();

            RandomPwd();
            //mutelist = LoadXMLFile("MuteList.xml");
            //mutelist.PrimaryKey = new DataColumn[] { mutelist.Columns["uuid"] };
        }

        public void ReapplyConfig(string full_name)
        {
            //config = new ConfigManager(full_name);
            ////config.ApplyDefault();
            //config.ApplyCurrentConfig();

            config.ChangeConfigFile(full_name);

            SetSettings();
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

        private void RandomPwd()
        {
            if (string.IsNullOrEmpty(config.CurrentConfig.GroupManPro))
            {
                // assign a random pwd
                config.CurrentConfig.GroupManPro = GetRandomPassword(15);
            }
        }

        public string GetRandomPassword(int length)
        {
            char[] chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&".ToCharArray();
            string password = string.Empty;
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int x = random.Next(1, chars.Length);
                //Don't Allow Repetition of Characters
                if (!password.Contains(chars.GetValue(x).ToString()))
                    password += chars.GetValue(x);
                else
                    i--;
            }

            return password;
        }

        public void InitAI()
        {
            string aimlDirectory = Application.StartupPath.ToString();
            //string confdir = aimlDirectory + "\\config\\";
            aimlDirectory += "\\aiml\\";

            bool direxists = DirExists(aimlDirectory);

            if (direxists)
            {
                myBot = null;
                //GC.Collect();  
                //myBot = new AIMLbot.Bot();
                ////string a1 = myBot.PathToAIML;
                ////string a2 = myBot.PathToConfigFiles;
                //myBot.isAcceptingUserInput = false;
                //myBot.loadSettings();
                //myBot.loadAIMLFromFiles();
                
                //myBot.isAcceptingUserInput = true;

                //AIMLbot.Utils.SettingsDictionary dic = myBot.Substitutions;

                myBot = new AIMLbot.Bot();
                myBot.loadSettings();
                myBot.loadAIMLFromFiles();
                myBot.isAcceptingUserInput = true;
            }
            else
            {
                Logger.Log("AI is enabled but AI libraries are not installed.", Helpers.LogLevel.Warning);    
            }
        }

        private void CreateNotesDir()
        {
            string logdir = appdir;   // Application.StartupPath.ToString();
            logdir += "\\Notes\\";

            bool direxists = DirExists(logdir);

            if (!direxists)
            {
                // Create the dir
                Directory.CreateDirectory(logdir);
            }
        }

        private void CreateSpellingDir()
        {
            string logdir = appdir;   // Application.StartupPath.ToString();
            logdir += "\\Spelling\\";

            bool direxists = DirExists(logdir);

            if (!direxists)
            {
                // Create the dir
                Directory.CreateDirectory(logdir);
            }
        }

        private void CreateLogDir()
        {
            string logdir = appdir;   // Application.StartupPath.ToString();
            logdir += "\\Logs\\";

            bool direxists = DirExists(logdir);

            if (!direxists)
            {
                // Create the dir
                Directory.CreateDirectory(logdir);
            }
            else
            {
                long size = GetDirectorySize(logdir);
                double fsize = ConvertBytesToMegabytes(size);

                if (fsize > 250)
                {
                    DialogResult res = MessageBox.Show("Your chat log folder is over 250MB in size.\n\nDo you want to delete log files older than 60 days?", "METAbolt", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (res == DialogResult.Yes)
                    {
                        DirectoryInfo dir = new DirectoryInfo(logdir);

                        FileInfo[] rgFiles = dir.GetFiles("*.txt");

                        foreach (FileInfo fi in rgFiles)
                        {
                            if (DateTime.Now.AddDays(-60) > fi.CreationTime)
                            {
                                fi.Delete();
                            }
                        }
                    }
                }
            }

            // Create the Extensions dir
            logdir = appdir;   // Application.StartupPath.ToString();
            logdir += "\\Extensions\\";

            direxists = DirExists(logdir);

            if (!direxists)
            {
                // Create the dir
                Directory.CreateDirectory(logdir);
            }
        }

        private bool DirExists(string sDirName)
        {
            try
            {
                return (System.IO.Directory.Exists(sDirName));    //Check for file
            }
            catch (Exception)
            {
                return (false);                                 //Exception occured, return False
            }
        }

        private long GetDirectorySize(string p)
        {
            string[] a = Directory.GetFiles(p, "*.*");

            long b = 0;
            foreach (string name in a)
            {
                FileInfo info = new FileInfo(name);
                b += info.Length;
            }

            return b;
        }

        private double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        public bool IsAvatarMuted(UUID avatar, string name)
        {
            //if (avatar == UUID.Zero) return false;
  
            //try
            //{
            //    DataRow dr = mutelist.Rows.Find(avatar.ToString());

            //    if (dr != null)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //catch { return false; }

            //MuteEntry mentry = client.Self.MuteList.Find(mle => mle.Type == mtyp && mle.ID == avatar);

            if (null != client.Self.MuteList.Find(mle => (mle.Type == MuteType.Object && mle.ID == avatar) // id 
                || (mle.Type == MuteType.ByName && mle.Name == name) // avatar/object name
                || (mle.Type == MuteType.Resident && mle.ID == avatar) // avatar
                ))
            {
                return true;
            }
            else
            {
                return false;
            }

            //if (mentry != null)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        public bool IsObjectMuted(UUID avatar, string name)
        {
            if (null != client.Self.MuteList.Find(mle => (mle.Type == MuteType.Object && mle.ID == avatar)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsGiveItem(string item, UUID avid)
        {
            if (string.IsNullOrEmpty(item)) return false;

            try
            {
                DataRow dr = giveritems.Rows.Find(item.ToString());

                if (dr != null)
                {
                    UUID iid = (UUID)dr.ItemArray[1].ToString();
                    string name = dr.ItemArray[2].ToString().ToString();

                    string astype = dr.ItemArray[3].ToString();

                    AssetType type;

                    // You can't cast a string or an object to an AssetType!
                    // TODO: Change this section when possible

                    switch (astype.ToLower(CultureInfo.CurrentCulture))
                    {
                        case "animation":
                            type = AssetType.Animation;
                            break;
                        case "bodypart":
                            type = AssetType.Bodypart;
                            break;
                        case "callingcard":
                            type = AssetType.CallingCard;
                            break;
                        case "clothing":
                            type = AssetType.Clothing;
                            break;
                        case "gesture":
                            type = AssetType.Gesture;
                            break;
                        case "landmark":
                            type = AssetType.Landmark;
                            break;
                        case "mesh":
                            type = AssetType.Mesh;
                            break;
                        case "notecard":
                            type = AssetType.Notecard;
                            break;
                        case "object":
                            type = AssetType.Object;
                            break;
                        case "lsltext":
                            type = AssetType.LSLText;
                            break;
                        case "sound":
                            type = AssetType.Sound;
                            break;
                        case "imagetga":
                            type = AssetType.ImageTGA;
                            break;
                        case "imagejpeg":
                            type = AssetType.ImageJPEG;
                            break;
                        case "texture":
                            type = AssetType.Texture;
                            break;
                        case "texturetga":
                            type = AssetType.TextureTGA;
                            break;
                        default:
                            type = AssetType.Unknown;
                            break;
                    }

                    client.Inventory.GiveItem(iid, name, type, avid, false);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Log("METAcourier: " + ex.Message, Helpers.LogLevel.Error);
                return false;
            }
        }

        //private void LoadXMLFile(string XmlFile)
        //{
        //    if (!System.IO.File.Exists(XmlFile))
        //    {
        //        DataTable tbl = MakeDataTable();

        //        mutelist = tbl;
        //        mutelist.PrimaryKey = new DataColumn[] { mutelist.Columns["uuid"] };
        //        return;
        //    }
 
        //    DataSet dset = new DataSet();
        //    FileStream fstr = null;

        //    try
        //    {
        //        fstr = new FileStream(XmlFile, FileMode.Open, FileAccess.Read);
        //        dset.ReadXml(fstr);
        //    }
        //    catch (Exception exp)
        //    {
        //        Logger.Log("Load mute list: " + exp.Message, Helpers.LogLevel.Warning);
        //    }

        //    try
        //    {
        //        if (dset.Tables.Count > 0)
        //        {
        //            DataTable dtbl = dset.Tables[0];

        //            fstr.Close();
        //            dset.Dispose();

        //            mutelist = dtbl;
        //            mutelist.PrimaryKey = new DataColumn[] { mutelist.Columns["uuid"] };
        //        }
        //        else
        //        {
        //            fstr.Close();
        //            dset.Dispose();

        //            DataTable tbl = MakeDataTable();

        //            mutelist = tbl;
        //            mutelist.PrimaryKey = new DataColumn[] { mutelist.Columns["uuid"] };
        //        }
        //    }
        //    catch
        //    {
        //        ;
        //    }
        //}

        //private void SaveXMLFile(string XmlFile)
        //{
        //    try
        //    {
        //        //int recs = mutelist.Rows.Count;  
        //        mutelist.WriteXml(XmlFile);
        //    }
        //    catch
        //    {
        //        ;
        //    }
        //}

        private void SaveGiverItems(string XmlFile)
        {
            try
            {
                //int recs = mutelist.Rows.Count;  
                giveritems.WriteXml(XmlFile);
            }
            catch
            {
                ;
            }
        }

        private void LoadGiverItems(string XmlFile)
        {
            if (!System.IO.File.Exists(XmlFile))
            {
                DataTable tbl = MakeGiverDataTable();

                giveritems = tbl;
                giveritems.PrimaryKey = new DataColumn[] { giveritems.Columns["Command"] };
                return;
            }

            DataSet dset = new DataSet();
            FileStream fstr = null;

            try
            {
                fstr = new FileStream(XmlFile, FileMode.Open, FileAccess.Read);
                dset.ReadXml(fstr);
            }
            catch (Exception exp)
            {
                Logger.Log("Load METAcourier items: " + exp.Message, Helpers.LogLevel.Warning);
            }

            try
            {
                if (dset.Tables.Count > 0)
                {
                    DataTable dtbl = dset.Tables[0];

                    fstr.Close();
                    dset.Dispose();

                    giveritems = dtbl;
                    giveritems.PrimaryKey = new DataColumn[] { giveritems.Columns["Command"] };
                }
                else
                {
                    fstr.Close();
                    dset.Dispose();

                    DataTable tbl = MakeGiverDataTable();

                    giveritems = tbl;
                    giveritems.PrimaryKey = new DataColumn[] { giveritems.Columns["Command"] };
                }
            }
            catch
            {
                ;
            }
        }

        //private DataTable MakeDataTable()
        //{
        //    DataColumn myColumn = new DataColumn();
        //    DataTable dtbl = new DataTable("list");

        //    myColumn.DataType = System.Type.GetType("System.String");
        //    myColumn.ColumnName = "mute_name";
        //    dtbl.Columns.Add(myColumn);

        //    myColumn = new DataColumn();
        //    myColumn.DataType = System.Type.GetType("System.String");
        //    myColumn.ColumnName = "uuid";
        //    dtbl.Columns.Add(myColumn);
        //    //mutelist.Columns.Add(myColumn);

        //    //dtbl.PrimaryKey = new DataColumn[] { dtbl.Columns["uuid"] };

        //    myColumn.Dispose();
 
        //    return dtbl;
        //}

        private DataTable MakeGiverDataTable()
        {
            DataTable dtbl = new DataTable("list");
            DataColumn myColumn = new DataColumn();

            myColumn.DataType = System.Type.GetType("System.String");
            myColumn.ColumnName = "Command";
            dtbl.Columns.Add(myColumn);

            //client.Inventory.GiveItem(iitem.UUID, iitem.Name, iitem.AssetType, avid, false);

            DataColumn myColumn1 = new DataColumn();
            myColumn1.DataType = System.Type.GetType("System.String");
            myColumn1.ColumnName = "UUID";
            dtbl.Columns.Add(myColumn1);

            DataColumn myColumn2 = new DataColumn();
            myColumn2 = new DataColumn();
            myColumn2.DataType = System.Type.GetType("System.String");
            myColumn2.ColumnName = "Name";
            dtbl.Columns.Add(myColumn2);

            DataColumn myColumn3 = new DataColumn();
            myColumn3.DataType = System.Type.GetType("System.String");
            myColumn3.ColumnName = "AssetType";
            dtbl.Columns.Add(myColumn3);

            DataColumn[] keys = new DataColumn[1];
            keys[0] = myColumn;

            dtbl.PrimaryKey = keys;

            return dtbl;
        }

        private void MakeTPTable()
        {
            DataColumn myColumn = new DataColumn();
            DataTable dtbl = new DataTable("history");

            myColumn.DataType = System.Type.GetType("System.String");
            myColumn.ColumnName = "time";
            dtbl.Columns.Add(myColumn);

            myColumn = new DataColumn();
            myColumn.DataType = System.Type.GetType("System.String");
            myColumn.ColumnName = "name";
            dtbl.Columns.Add(myColumn);

            myColumn = new DataColumn();
            myColumn.DataType = System.Type.GetType("System.String");
            myColumn.ColumnName = "slurl";
            dtbl.Columns.Add(myColumn);

            dtbl.PrimaryKey = new DataColumn[] { dtbl.Columns["time"] };

            tp = dtbl;

            myColumn.Dispose();
            dtbl.Dispose(); 
        }

        public void SetSettings()
        {
            // Timeouts and Intervals

            /// <summary>Number of milliseconds before a teleport attempt will time
            /// out</summary>
            //client.Settings.TELEPORT_TIMEOUT = 120 * 1000;
            /// <summary>Number of milliseconds before a CAPS call will time out 
            /// and try again</summary>
            /// <remarks>Setting this too low will cause web requests to repeatedly
            /// time out and retry</remarks>
            //client.Settings.CAPS_TIMEOUT = 120 * 1000;   //60 * 1000;
            client.Settings.SIMULATOR_TIMEOUT = 180 * 1000;    //90 * 1000;
            /// <summary>Number of milliseconds for xml-rpc to timeout</summary>
            //client.Settings.RESEND_TIMEOUT = 8 * 1000;   //4 * 1000;
            /// <summary>Milliseconds to wait for a simulator info request through
            /// the grid interface</summary>
            //client.Settings.MAP_REQUEST_TIMEOUT = 60 * 1000;  //5 * 1000;
            client.Settings.MAX_CONCURRENT_TEXTURE_DOWNLOADS = 20;
            client.Settings.USE_INTERPOLATION_TIMER = false;

            // Sizes

            /// <summary>Maximum number of queued ACKs to be sent before SendAcks()
            /// is forced</summary>
            //client.Settings.MAX_PENDING_ACKS = 1;
            /// <summary>Maximum number of ACKs to append to a packet</summary>
            //client.Settings.MAX_APPENDED_ACKS = 10;
            /// <summary>Network stats queue length (seconds)</summary>
            //client.Settings.STATS_QUEUE_SIZE = 5;


            // Configuration options (mostly booleans)

            

            /// <summary>Enable to process packets synchronously, where all of the
            /// callbacks for each packet must return before the next packet is
            /// processed</summary>
            /// <remarks>This is an experimental feature and is not completely
            /// reliable yet. Ideally it would reduce context switches and thread
            /// overhead, but several calls currently block for a long time and
            /// would need to be rewritten as asynchronous code before this is
            /// feasible</remarks>
            ///
 
            // For smoother bot movement
            //client.Settings.DISABLE_AGENT_UPDATE_DUPLICATE_CHECK = true;

            //client.Settings.USE_LLSD_LOGIN = true;

            //client.Settings.SEND_AGENT_APPEARANCE = true;
            //client.Settings.CLIENT_IDENTIFICATION_TAG = 

            //client.Settings.STORE_LAND_PATCHES = true;
            /// <summary>Enable/disable sending periodic camera updates</summary>
            client.Settings.SEND_AGENT_UPDATES = true;

            // Enable stats
            //client.Settings.TRACK_UTILIZATION = false;
            /// <summary>Enable/disable the sending of pings to monitor lag and 
            /// packet loss</summary>
            //client.Settings.SEND_PINGS = false;
            /// <summary>Whether to decode sim stats</summary>
            //client.Settings.ENABLE_SIMSTATS = false;

            /// <summary>Whether to establish connections to HTTP capabilities
            /// servers for simulators</summary>
            //client.Settings.ENABLE_CAPS = true;

            //client.Settings.LOG_RESENDS = false;
            /// <summary>Should we connect to multiple sims? This will allow
            /// viewing in to neighboring simulators and sim crossings
            /// (Experimental)</summary>
            client.Settings.MULTIPLE_SIMS = config.CurrentConfig.Connect4;   // false;
            /// <summary>If true, all object update packets will be decoded in to
            /// native objects. If false, only updates for our own agent will be
            /// decoded. Registering an event handler will force objects for that
            /// type to always be decoded. If this is disabled the object tracking
            /// will have missing or partial prim and avatar information</summary>
            client.Settings.ALWAYS_DECODE_OBJECTS = true;
            /// <summary>If true, when a cached object check is received from the
            /// server the full object info will automatically be requested</summary>
            client.Settings.ALWAYS_REQUEST_OBJECTS = true;

            /// <summary>The capabilities servers are currently designed to
            /// periodically return a 502 error which signals for the client to
            /// re-establish a connection. Set this to true to log those 502 errors</summary>
            //client.Settings.LOG_ALL_CAPS_ERRORS = false;
            /// <summary>If true, any reference received for a folder or item
            /// libsecondlife is not aware of will automatically be fetched.</summary>
            client.Settings.FETCH_MISSING_INVENTORY = true;
            ///// <summary>If true, and <code>SEND_AGENT_UPDATES</code> is true,
            ///// AgentUpdate packets will continuously be sent out to give the bot
            ///// smoother movement and autopiloting</summary>
            //client.Settings.SYNC_PACKETCALLBACKS = true;
            /// <summary>If true, currently visible primitives and avatars will be
            /// stored in dictionaries inside <code>Simulator.Objects</code>. If 
            /// false, a new Avatar or Primitive object will be created each time
            /// an object update packet is received</summary>
            client.Settings.OBJECT_TRACKING = true;
            /// <summary>If true, parcel details will be stored in the 
            /// <code>Simulator.Parcels</code> dictionary as they are received</summary>
            //client.Settings.PARCEL_TRACKING = true;
            client.Settings.STORE_LAND_PATCHES = true;

            client.Settings.USE_ASSET_CACHE = true;
            //client.Settings.ASSET_CACHE_DIR = Application.StartupPath + System.IO.Path.DirectorySeparatorChar + "cache";
            client.Settings.ASSET_CACHE_DIR = appdir + System.IO.Path.DirectorySeparatorChar + client.Self.Name + System.IO.Path.DirectorySeparatorChar + "cache";
            //client.Settings.ASSET_CACHE_MAX_SIZE = (1024 * 1024 * 1024) / 4;  //250MB
            client.Assets.Cache.AutoPruneEnabled = false;

            client.Self.Movement.AutoResetControls = false;
            client.Self.Movement.UpdateInterval = 250;

            //client.Settings.HTTP_INVENTORY = !config.CurrentConfig.DisableHTTPinv;   // false;

            // This is for backward compatibility
            if (config.CurrentConfig.BandwidthThrottle > 500.0f)
            {
                config.CurrentConfig.BandwidthThrottle = 500.0f;
            }

            float throttle = config.CurrentConfig.BandwidthThrottle * 10000f;

            if (config.CurrentConfig.BandwidthThrottle == 500.0f)
            {
                client.Throttle.Total = throttle;
            }
            else
            {
                /// <summary>Enable/disable libsecondlife automatically setting the
                /// bandwidth throttle after connecting to each simulator</summary>
                /// <remarks>The default libsecondlife throttle uses the equivalent of
                /// the maximum bandwidth setting in the official client. If you do not
                /// set a throttle your connection will by default be throttled well
                /// below the minimum values and you may experience connection problems</remarks>
                client.Settings.SEND_AGENT_THROTTLE = false;

                client.Throttle.Cloud = 0.0f;
                client.Throttle.Land = 0.0f;
                client.Throttle.Wind = 0.0f;

                client.Throttle.Task = throttle / 10f;   // 2f * (throttle / 10f);   // 846000.0f;   // 220000.0f;    //1000000;
                client.Throttle.Asset = throttle / 10f;   // 2f * (throttle / 10f);    //220000.0f;
                client.Throttle.Resend = throttle / 10f;   // 3f * (throttle / 10f);  //1000000.0f;   // 
                client.Throttle.Texture = throttle / 10f;   // 2f * (throttle / 10f);     //1000000.0f;
            }

            //client.Throttle.Total = 5000000f;    //4460000.0f;

            client.Settings.THROTTLE_OUTGOING_PACKETS = false;

            //if (config.CurrentConfig.BroadcastID)
            //{
            //    client.Settings.CLIENT_IDENTIFICATION_TAG = new UUID("8201f643-6006-c2ea-fbf3-0a5e8c0874ed");
            //}
            //else
            //{
            //    client.Settings.CLIENT_IDENTIFICATION_TAG = new UUID(UUID.Zero.ToString());
            //}
        }

        public string RemoveReservedCharacters(string strValue)
        {
            //char[] ReservedChars = { '/', ':', '*', '?', '"', '<', '>', '|', '.', ',', '!', ';', '\\', '\'' };

            char[] invalidFileChars = Path.GetInvalidFileNameChars();

            foreach (char strChar in invalidFileChars)
            {
                strValue = strValue.Replace(strChar.ToString(), "");
            }

            return strValue;
        }

        private void InitializeConfig()
        {
            netcom.LoginOptions.FirstName = config.CurrentConfig.FirstName;
            netcom.LoginOptions.LastName = config.CurrentConfig.LastName;
            netcom.LoginOptions.Password = config.CurrentConfig.PasswordMD5;
            netcom.LoginOptions.IsPasswordMD5 = true;
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            config.SaveCurrentConfig();

            //try
            //{
            //    tabsConsole.Controls["notifyIcon1"].Dispose();
            //}
            //catch (Exception exp)
            //{
            //    // do nothing
            //}

            //SaveXMLFile(appdir + "\\MuteList.xml");
            SaveGiverItems(appdir + "\\METAgiverItems.xml");

            client = null;
            Environment.Exit(0); 
        }

        public int Distance3D(int x1, int y1, int z1, int x2, int y2, int z2)
        {
            //     __________________________________
            //d = &#8730; (x2-x1)^2 + (y2-y1)^2 + (z2-z1)^2
            //

            int result = 0;
            double part1 = Math.Pow((x2 - x1), 2);
            double part2 = Math.Pow((y2 - y1), 2);
            double part3 = Math.Pow((z2 - z1), 2);
            double underRadical = part1 + part2 + part3;
            result = (int)Math.Sqrt(underRadical);

            return result;
        }

        public Vector3 SIMsittingPos()
        {
            Vector3 ppos = new Vector3();
            ppos = client.Self.SimPosition;

            if (state.IsSitting)
            {
                if (state.SitPrim != null)
                {
                    ppos = State.SittingPos + client.Self.SimPosition;
                }
            }

            try
            {
                return client.Self.SimPosition;   // ppos;
            }
            catch {

                ppos = new Vector3(0, 0, 0);
                return ppos;
            }
        }

        public bool IMHistyoryExists(string filename, bool isgroup)
        {
            try
            {
                string folder = config.CurrentConfig.LogDir;

                DirectoryInfo di = new DirectoryInfo(folder);

                if (!di.Exists)
                {
                    di.Create(); 
                }

                int cnt = 0;

                foreach (FileInfo fi in di.GetFiles())
                {
                    //string inFile = fi.FullName;
                    string finname = fi.Name;

                    if (finname.Contains(filename))
                    {
                        if (isgroup)
                        {
                            if (finname.Contains("-GROUP-"))
                            {
                                if (finname.Contains(client.Self.FirstName + " " + client.Self.LastName))
                                {
                                    //string filedate = string.Empty;
                                    //string[] file = finname.Split('-');

                                    //filedate = file[1].Trim() + "/" + file[2].Trim() + "/" + file[3].Substring(0, 4).Trim();

                                    cnt += 1;
                                }
                            }
                        }
                        else
                        {
                            if (!finname.Contains("-GROUP-"))
                            {
                                if (finname.Contains(client.Self.FirstName + " " + client.Self.LastName))
                                {
                                    //string filedate = string.Empty;
                                    //string[] file = finname.Split('-');

                                    //filedate = file[1].Trim() + "/" + file[2].Trim() + "/" + file[3].Substring(0, 4).Trim();

                                    cnt += 1;
                                }
                            }
                        }
                    }
                }

                if (cnt == 0)
                {
                    return false;
                }

                return true;
            }
            catch { return false; }
        }

        public string SetTime()
        {
            DateTime dte = DateTime.Now;

            dte = State.GetTimeStamp(dte);

            if (Config.CurrentConfig.UseSLT)
            {
                string _timeZoneId = "Pacific Standard Time";
                DateTime startTime = DateTime.UtcNow;
                TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId);
                dte = TimeZoneInfo.ConvertTime(startTime, TimeZoneInfo.Utc, tst);
            }

            string prefix = dte.ToString("[HH:mm] ");

            return prefix;
        }

        public GridClient Client
        {
            get { return client; }
        }

        public SLNetCom Netcom
        {
            get { return netcom; }
        }

        public ImageCache ImageCache
        {
            get { return imageCache; }
        }

        public StateManager State
        {
            get { return state; }
        }

        public ConfigManager Config
        {
            get { return config; }
        }

        public frmMain MainForm
        {
            get { return mainForm; }
        }

        public TabsConsole TabConsole
        {
            get { return tabsConsole; }
        }

        public bool IsFirstInstance
        {
            get { return firstInstance; }
        }

        public bool OtherInstancesOpen
        {
            get { return otherInstancesOpen; }
            set { otherInstancesOpen = value; }
        }

        public int DialogCount
        {
            get { return dialogcount; }
            set { dialogcount = value; }
        }

        public int NoticeCount
        {
            get { return noticecount; }
            set { noticecount = value; }
        }

        public bool LoggedIn
        {
            get { return loggedin; }
            set { loggedin = value; }
        }

        public bool LogOffClicked
        {
            get { return logoffclicked; }
            set { logoffclicked = value; }
        }

        public bool ReBooted
        {
            get { return rebooted; }
            set { rebooted = value; }
        }

        public AIMLbot.Bot ABot
        {
            get { return myBot; }
        }

        //public DataTable MuteList
        //{
        //    get { return mutelist; }
        //    set { mutelist = value; }
        //}

        public DataTable TP
        {
            get { return tp; }
            set { tp = value; }
        }

        public bool DetectLang
        {
            get { return detectlang; }
            set { detectlang = value; }
        }

        public UUID LookID
        {
            get { return lookID; }
            set { lookID = value; }
        }

        public List<IExtension> EList
        {
            get { return elist; }
            set { elist = value; }
        }

        public IMbox imBox
        {
            get { return imbox; }
            set { imbox = value; }
        }

        public int DialogTimeOut
        {
            get { return dialogtimeout; }
            set { dialogtimeout = value; }
        }

        public DataTable GiverItems
        {
            get { return giveritems; }
            set { giveritems = value; }
        }

        public bool ReadIMs
        {
            get { return readims; }
            set { readims = value; }
        }

        public string AffFile
        {
            get { return afffile; }
            set { afffile = value; }
        }

        public string DictionaryFile
        {
            get { return dicfile; }
            set { dicfile = value; }
        }

        public bool AllowVoice
        {
            get { return allowvoice; }
            set { allowvoice = value; }
        }

        public UUID FavsFolder
        {
            get { return favsfolder; }
            set { favsfolder = value; }
        }
    }
}
