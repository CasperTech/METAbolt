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
using OpenMetaverse;
using Nini.Config;
using System.Windows.Forms;
using System.Drawing;
using MD5library;
using System.IO;
using System.Diagnostics;
using System.Globalization;


namespace METAbolt
{
    public class Config
    {
        private int configVersion = 1;

        private int mainWindowState = 0;
        private int interfaceStyle = 1; //0 = System, 1 = Office 2003

        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string passwordMD5 = string.Empty;
        private int loginLocationType = 0;
        private string loginLocation = string.Empty;
        private int loginGrid = 0;
        private string loginUri = string.Empty;
        private bool irempwd = false;
        //private bool iradar = false;
        private string purl = string.Empty;
        private string murl = string.Empty;
        private int linemax = 5000;

        private bool chatTimestamps = true;
        private bool imTimestamps = true;
        private bool chatSmileys = false;
        private bool parcelmusic = false;
        private bool parcelmedia = false;
        private UUID objectsfolder = UUID.Zero;
        private string usernamelist = string.Empty; 

        //private string tweetername = string.Empty;
        //private string tweeterpwd = string.Empty;
        //private bool enabletweeter = false;
        //private bool enablechattweets = false;
        //private bool tweet = true;
        //private string tweetuser = string.Empty;

        private bool connect4 = false;
        private bool aion = false;
        private bool autosit = false;
        private int radarrange = 64;
        private bool useslt = false;
        private int objectrange = 20;
        private string groupmanpro = string.Empty;
        private bool playsound = false;
        private bool metahide = false;
        private string busyreply = "The Resident you messaged is in 'busy mode' which means they have requested not to be disturbed.  Your message will still be shown in their IM panel for later viewing.";
        private string initialIMreply = string.Empty;
        private bool declineinv = false;
        private bool replyAI = false;
        private string replyText = "I am sorry but I didn't understand what you said or I haven't been taught a response for it. Can you try again, making sure your sentences are short and clear.";

        //added by GM on 2-JUL-2009
        private string groupManagerUid = "ned49b54-325d-486a-af3m"; //respecting the prior default value hard-wiring
        private string ignoreUid = "ned49b54-325d-123a-x33m";
        private UUID chairAnnouncerUuid = UUID.Zero;
        private int chairAnnouncerInterval = 5;
        private UUID chairAnnouncerGroup1 = UUID.Zero;
        private UUID chairAnnouncerGroup2 = UUID.Zero;
        private UUID chairAnnouncerGroup3 = UUID.Zero;
        private UUID chairAnnouncerGroup4 = UUID.Zero;
        private UUID chairAnnouncerGroup5 = UUID.Zero;
        private UUID chairAnnouncerGroup6 = UUID.Zero;
        private bool chairAnnouncerEnabled = false;
        private bool chairAnnouncerChat = true;
        //added by GM on 1-APR-2010
        private string chairAnnouncerAdvert = "Brought to you by METAbolt and Machin's Machines";

        // Incoming command identifier 04 Aug 2009
        private string commandinid = "ned34b54-3765-439j-fds5";

        private bool saveims = true;
        private bool savechat = false;
        //private string logdir = Application.StartupPath.ToString() + "\\Logs\\";
        private string logdir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt" + "\\Logs\\";

        private bool disablegnotices = false;
        private bool disablegims = false;
        private bool bufferapplied = false;

        private bool disablenotifications = false;
        private bool disableinboundgroupinvites = false;
        private bool disablelookat = true;

        private bool useproxy = false;
        private string proxyurl = string.Empty;
        private string proxyport = string.Empty;
        private string proxyuser = string.Empty;
        private string proxypwd = string.Empty;
        private bool givepressie = false;
        private bool autorestart = true;
        private int logofftime = 0;
        private float bandwidththrottle = 500;
        private bool logofftimerchanged = true;
        private bool useclassicchatlayout = false;
        private string headerfont = "Tahoma";
        private string headerfontstyle = "Regular";
        private float headerfontsize = 8.5f;
        private int headerbackcolour = Color.Lavender.ToArgb();
        //private int bgcolour = Color.White.ToArgb();   
        private string textfont = "Tahoma";
        private string textfontstyle = "Regular";
        private float textfontsize = 8.5f;
        private string pluginstoload = string.Empty;

        private bool playfriendonline = false;
        private bool playfriendoffline = false;
        private bool playimreceived = false;
        private bool playgroupimreceived = false;
        private bool playgroupnotice = false;
        private bool playintentoryitem = false;
        private bool playpaymentreceived = false;
        private bool autoacceptitems = false;
        private bool startminimised = false;
        private string adremove = string.Empty;
        private string masteravatar = UUID.Zero.ToString();   //    string.Empty;
        private string masterobject = UUID.Zero.ToString();
        private bool enforcelslsecurity = true;
        private bool autotransfer = false;
        private bool sortbydistance = true;
        private bool disabletrayicon = false;
        private bool disablefriendsnotifications = false;
        private bool disabletyping = false;
        private bool autoacceptfriends = false;
        private int restarttime = 10;
        private bool disablemipmaps = false;
        private bool displaylslcommands = true;
        private bool multilingualai = false;
        private bool enablespellcheck = false;
        private string spelllang = "en_GB";
        //private bool broadcastid = true;
        private bool hidedisconnectprompt = false;
        private bool disableradar = false;
        private bool restrictradar = false;
        private bool disablevoice = false;
        private bool disablefavs = false;
        private bool disablehttpinv = true;
        private bool disableradarminimap = false;
        private string appmenupos = "Top";
        private string landmenupos = "Top";
        private string fnmenupos = "Top";
        private bool usellsd = false;
        private int chatbufferlimit = 20;
        private int scripturlbufferlimit = 5;

        public Config()
        {

        }

        public static Config LoadFrom(string filename)
        {
            Config config = new Config();

            try
            {
                IConfigSource conf = new IniConfigSource(filename);

                config.Version = conf.Configs["General"].GetInt("Version", 0);

                config.MainWindowState = conf.Configs["Interface"].GetInt("MainWindowState", 0);
                config.InterfaceStyle = conf.Configs["Interface"].GetInt("Style", 1);

                // Login
                config.FirstName = conf.Configs["Login"].GetString("FirstName", string.Empty);
                config.LastName = conf.Configs["Login"].GetString("LastName", string.Empty);

                string epwd = conf.Configs["Login"].GetString("Password", string.Empty);

                config.LoginGrid = conf.Configs["Login"].GetInt("Grid", 0);
                config.LoginUri = conf.Configs["Login"].GetString("Uri", string.Empty);
                config.LoginLocationType = conf.Configs["Login"].GetInt("LocationType", 0);
                config.LoginLocation = conf.Configs["Login"].GetString("Location", string.Empty);
                config.iRemPWD = conf.Configs["Login"].GetBoolean("iRemPWD", false);
                config.UserNameList = conf.Configs["Login"].GetString("UserNameList", string.Empty);

                // General
                config.Connect4 = conf.Configs["General"].GetBoolean("Connect4", false);
                config.DisableNotifications = conf.Configs["General"].GetBoolean("DisableNotifications", false);
                config.DisableInboundGroupInvites = conf.Configs["General"].GetBoolean("DisableInboundGroupInvites", false);
                config.AutoSit = conf.Configs["General"].GetBoolean("AutoSit", false);
                config.RadarRange = conf.Configs["General"].GetInt("RadarRange", 64);
                config.ObjectRange = conf.Configs["General"].GetInt("ObjectRange", 20);
                config.GroupManPro = conf.Configs["General"].GetString("GroupManPro");
                config.HeaderFont = conf.Configs["General"].GetString("HeaderFont", "Tahoma");
                config.HeaderFontStyle = conf.Configs["General"].GetString("HeaderFontStyle", "Regular");
                config.HeaderFontSize = conf.Configs["General"].GetFloat("HeaderFontSize", 8.5f);
                config.StartMinimised = conf.Configs["General"].GetBoolean("StartMinimised", false);
                config.HideDisconnectPrompt = conf.Configs["General"].GetBoolean("HideDisconnectPrompt", false);

                try
                {
                    int clr = conf.Configs["General"].GetInt("HeaderBackColour", Color.Lavender.ToArgb());
                    config.HeaderBackColour = Color.FromArgb(clr);
                }
                catch
                {
                    config.HeaderBackColour = Color.Lavender;
                }

                config.TextFont = conf.Configs["General"].GetString("TextFont", "Tahoma");
                config.TextFontStyle = conf.Configs["General"].GetString("TextFontStyle", "Regular");
                config.TextFontSize = conf.Configs["General"].GetFloat("TextFontSize", 8.5f);
                config.GivePresent = conf.Configs["General"].GetBoolean("GivePresent", false);
                config.HideMeta = conf.Configs["General"].GetBoolean("HideMeta", true);
                config.DeclineInv = conf.Configs["General"].GetBoolean("DeclineInv", false);
                config.DisableLookAt = conf.Configs["General"].GetBoolean("DisableLookAt", true);
                config.ClassicChatLayout = conf.Configs["General"].GetBoolean("ClassicChatLayout", false);
                config.AutoRestart = conf.Configs["General"].GetBoolean("AutoRestart", false);
                config.LogOffTime = conf.Configs["General"].GetInt("LogOffTime", 0);
                config.ReStartTime = conf.Configs["General"].GetInt("ReStartTime", 10);
                config.BandwidthThrottle = conf.Configs["General"].GetFloat("BandwidthThrottle", 500f);

                config.PlayFriendOnline = conf.Configs["General"].GetBoolean("PlayFriendOnline", false);
                config.PlayFriendOffline = conf.Configs["General"].GetBoolean("PlayFriendOffline", false);
                config.PlayIMreceived = conf.Configs["General"].GetBoolean("PlayIMreceived", false);
                config.PlayGroupIMreceived = conf.Configs["General"].GetBoolean("PlayGroupIMreceived", false);
                config.PlayGroupNoticeReceived = conf.Configs["General"].GetBoolean("PlayGroupNoticeReceived", false);
                config.PlayInventoryItemReceived = conf.Configs["General"].GetBoolean("PlayInventoryItemReceived", false);
                config.PlayPaymentReceived = conf.Configs["General"].GetBoolean("PlayPaymentReceived", false);
                config.AutoAcceptItems = conf.Configs["General"].GetBoolean("AutoAcceptItems", false);
                config.AdRemove = conf.Configs["General"].GetString("AdRemove", string.Empty);
                config.MasterAvatar = conf.Configs["General"].GetString("MasterAvatar", UUID.Zero.ToString());
                config.EnforceLSLsecurity = conf.Configs["General"].GetBoolean("EnforceLSLsecurity", true);
                config.DisplayLSLcommands = conf.Configs["General"].GetBoolean("DisplayLSLcommands", true);

                // backward compatibility pre V 0.9.47.0

                if (string.IsNullOrEmpty(config.MasterAvatar))
                {
                    config.MasterAvatar = UUID.Zero.ToString();   
                }

                config.MasterObject = conf.Configs["General"].GetString("MasterObject", UUID.Zero.ToString());
                config.AutoTransfer = conf.Configs["General"].GetBoolean("AutoTransfer", false);
                config.DisableTrayIcon = conf.Configs["General"].GetBoolean("DisableTrayIcon", false);
                config.DisableFriendsNotifications = conf.Configs["General"].GetBoolean("DisableFriendsNotifications", false);
                config.DisableTyping = conf.Configs["General"].GetBoolean("DisableTyping", false);
                config.AutoAcceptFriends = conf.Configs["General"].GetBoolean("AutoAcceptFriends", false);
                //config.BroadcastID = conf.Configs["General"].GetBoolean("BroadcastID", true);
                config.DisableRadar = conf.Configs["General"].GetBoolean("DisableRadar", false);
                config.RestrictRadar = conf.Configs["General"].GetBoolean("RestrictRadar", false);
                config.DisableVoice = conf.Configs["General"].GetBoolean("DisableVoice", false);
                config.DisableFavs = conf.Configs["General"].GetBoolean("DisableFavs", false);
                config.DisableHTTPinv = conf.Configs["General"].GetBoolean("DisableHTTPinv", true);
                config.DisableRadarImageMiniMap = conf.Configs["General"].GetBoolean("DisableRadarImageMiniMap", false);
                config.AppMenuPos = conf.Configs["General"].GetString("AppMenuPos", "Top");
                config.LandMenuPos = conf.Configs["General"].GetString("LandMenuPos", "Top");
                config.FnMenuPos = conf.Configs["General"].GetString("FnMenuPos", "Top");
                config.UseLLSD = conf.Configs["General"].GetBoolean("UseLLSD", false);
                config.ChatBufferLimit = conf.Configs["General"].GetInt("ChatBufferLimit", 20);
                config.ScriptUrlBufferLimit = conf.Configs["General"].GetInt("ScriptUrlBufferLimit", 5);
                
                // AI    
                config.AIon = conf.Configs["AI"].GetBoolean("AIon", false);
                config.replyAI = conf.Configs["AI"].GetBoolean("ReplyAI", false);
                config.replyText = conf.Configs["AI"].GetString("ReplyText", "I am sorry but I didn't understand what you said or I haven't been taught a response for it. Can you try again, making sure your sentences are short and clear.");
                config.MultiLingualAI = conf.Configs["AI"].GetBoolean("MultiLingualAI", false);

                config.ChatTimestamps = conf.Configs["Text"].GetBoolean("ChatTimestamps", true);
                config.IMTimestamps = conf.Configs["Text"].GetBoolean("IMTimestamps", true);
                config.ChatSmileys = conf.Configs["Text"].GetBoolean("ChatSmileys", false);
                config.lineMax = conf.Configs["Text"].GetInt("lineMax", 5000);
                config.ParcelMusic = conf.Configs["Text"].GetBoolean("ParcelMusic", true);
                config.ParcelMedia = conf.Configs["Text"].GetBoolean("ParcelMedia", true);
                config.UseSLT = conf.Configs["Text"].GetBoolean("UseSLT", false);
                config.PlaySound = conf.Configs["Text"].GetBoolean("PlaySound", false);
                config.SaveIMs = conf.Configs["Text"].GetBoolean("SaveIMs", true);
                config.SaveChat = conf.Configs["Text"].GetBoolean("SaveChat", false);
                config.LogDir = conf.Configs["Text"].GetString("LogDir", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt" + "\\Logs\\");
                config.DisableGroupNotices = conf.Configs["Text"].GetBoolean("DisableGroupNotices", true);
                config.DisableGroupIMs = conf.Configs["Text"].GetBoolean("DisableGroupIMs", false);
                config.BusyReply = conf.Configs["Text"].GetString("BusyReply", "The Resident you messaged is in 'busy mode' which means they have requested not to be disturbed.  Your message will still be shown in their IM panel for later viewing.");
                config.InitialIMReply = conf.Configs["Text"].GetString("InitialIMReply", "");

                // Proxy
                config.UseProxy =  conf.Configs["Proxy"].GetBoolean("UseProxy", false);
                config.ProxyURL = conf.Configs["Proxy"].GetString("ProxyURL", string.Empty);
                config.ProxyPort = conf.Configs["Proxy"].GetString("ProxyPort", string.Empty);
                config.ProxyUser = conf.Configs["Proxy"].GetString("ProxyUser", string.Empty);
                config.ProxyPWD = conf.Configs["Proxy"].GetString("ProxyPWD", string.Empty);

                // META3D    
                try
                {
                    config.DisableMipmaps = conf.Configs["META3D"].GetBoolean("DisableMipmaps", false);
                }
                catch { ; }
                                   
                //config.TweeterName = conf.Configs["Twitter"].GetString("TweeterName", string.Empty);
                //config.TweeterPwd = conf.Configs["Twitter"].GetString("TweeterPwd", string.Empty);
                //config.EnableTweeter = conf.Configs["Twitter"].GetBoolean("EnableTweeter", false);
                //config.EnableChatTweets = conf.Configs["Twitter"].GetBoolean("EnableChatTweets", false);
                //config.Tweet = conf.Configs["Twitter"].GetBoolean("Tweet", false);
                //config.TweeterUser = conf.Configs["Twitter"].GetString("TweeterUser", string.Empty);

                config.PluginsToLoad = conf.Configs["LoadedPlugIns"].GetString("PluginsToLoad", string.Empty);

                try
                {
                    if (!string.IsNullOrEmpty(epwd))
                    {
                        Crypto cryp = new Crypto(Crypto.SymmProvEnum.Rijndael);
                        //string cpwd = cryp.Decrypting(epwd);
                        string cpwd = cryp.Decrypt(epwd);

                        config.PasswordMD5 = cpwd;
                    }
                    else
                    {
                        config.PasswordMD5 = epwd;
                    }
                }
                catch
                {
                    epwd = config.PasswordMD5 = string.Empty;
                    MessageBox.Show("An error occured while decrypting your stored password.\nThis could mean you have the old format INI file. \nYou will have to re-enter your password so it can be ecrypted with the new method.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Process.Start("explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt\\");
                }

                //added by GM on 2-JUL-2009
                config.groupManagerUid = conf.Configs["PlugIn"].GetString("GroupManager", "ned49b54-325d-486a-af3m");
                config.chairAnnouncerUuid = UUID.Parse(conf.Configs["PlugIn"].GetString("ChairAnnouncer", UUID.Zero.ToString()));
                config.chairAnnouncerInterval = conf.Configs["PlugIn"].GetInt("ChairAnnouncerInterval", 5);
                config.chairAnnouncerGroup1 = UUID.Parse(conf.Configs["PlugIn"].GetString("ChairAnnouncerGroup1", UUID.Zero.ToString()));
                config.chairAnnouncerGroup2 = UUID.Parse(conf.Configs["PlugIn"].GetString("ChairAnnouncerGroup2", UUID.Zero.ToString()));
                config.chairAnnouncerGroup3 = UUID.Parse(conf.Configs["PlugIn"].GetString("ChairAnnouncerGroup3", UUID.Zero.ToString()));
                config.chairAnnouncerGroup4 = UUID.Parse(conf.Configs["PlugIn"].GetString("ChairAnnouncerGroup4", UUID.Zero.ToString()));
                config.chairAnnouncerGroup5 = UUID.Parse(conf.Configs["PlugIn"].GetString("ChairAnnouncerGroup5", UUID.Zero.ToString()));
                config.chairAnnouncerGroup6 = UUID.Parse(conf.Configs["PlugIn"].GetString("ChairAnnouncerGroup6", UUID.Zero.ToString()));
                config.chairAnnouncerEnabled = conf.Configs["PlugIn"].GetBoolean("ChairAnnouncerEnabled", false);
                config.chairAnnouncerChat = conf.Configs["PlugIn"].GetBoolean("ChairAnnouncerChat", true);
                //added by GM on 1-APR-2010
                config.chairAnnouncerAdvert = conf.Configs["PlugIn"].GetString("ChairAnnouncerAdvert", "Brought to you by METAbolt and Machin's Machines");
                //throw new Exception("Test");

                try
                {
                    // Spelling
                    config.EnableSpelling = conf.Configs["Spelling"].GetBoolean("EnableSpelling", false);
                    config.SpellLanguage = conf.Configs["Spelling"].GetString("SpellLanguage", "en_GB");
                }
                catch { ; }
            }
            catch (Exception exp)
            {
                try
                {
                    exp.HelpLink = "http://www.metabolt.net/METAforums/yaf_postsm708_Crash-on-launch.aspx#post708";
                    //Logger.Log("ERROR while loading config file'" + filename + "'. Your settings may not have fully loaded. Message: " + exp.Message, Helpers.LogLevel.Error);
                    MessageBox.Show("The was an error when loading your Config (METAbolt.ini) file.\nNot all of your settings may have been loaded.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch { ; }
            }

            return config;
        }

        public void Save(string filename)
        {
            IniConfigSource source = new IniConfigSource();

            // General
            IConfig config = source.AddConfig("General");
            config.Set("Version", configVersion.ToString(CultureInfo.CurrentCulture));
            //config.Set("iRadar", iradar.ToString());
            config.Set("Connect4", connect4.ToString(CultureInfo.CurrentCulture));
            config.Set("DisableNotifications", disablenotifications.ToString(CultureInfo.CurrentCulture));
            config.Set("DisableInboundGroupInvites", disableinboundgroupinvites.ToString(CultureInfo.CurrentCulture));
            config.Set("AutoSit", autosit.ToString(CultureInfo.CurrentCulture));
            config.Set("RadarRange", radarrange.ToString(CultureInfo.CurrentCulture));
            config.Set("ObjectRange", objectrange.ToString(CultureInfo.CurrentCulture));
            config.Set("GroupManPro", groupmanpro);
            config.Set("GivePresent", givepressie.ToString(CultureInfo.CurrentCulture));
            config.Set("HideMeta", metahide.ToString(CultureInfo.CurrentCulture));
            config.Set("DeclineInv", declineinv.ToString(CultureInfo.CurrentCulture));
            config.Set("DisableLookAt", disablelookat);
            config.Set("AutoRestart", autorestart.ToString(CultureInfo.CurrentCulture));
            config.Set("LogOffTime", logofftime.ToString(CultureInfo.CurrentCulture));
            config.Set("ReStartTime", restarttime.ToString(CultureInfo.CurrentCulture));
            config.Set("BandwidthThrottle", bandwidththrottle.ToString(CultureInfo.CurrentCulture));
            config.Set("ClassicChatLayout", useclassicchatlayout.ToString(CultureInfo.CurrentCulture));
            config.Set("HideDisconnectPrompt", hidedisconnectprompt.ToString(CultureInfo.CurrentCulture));

            if (headerfont == null)
            {
                headerfont = "Tahoma";
                headerfontstyle = "Regular";
                headerfontsize = 8.5f;
                headerbackcolour = Color.Lavender.ToArgb();
            }

            config.Set("HeaderFont", headerfont);
            config.Set("HeaderFontStyle", headerfontstyle);
            config.Set("HeaderFontSize", headerfontsize.ToString(CultureInfo.CurrentCulture));
            config.Set("HeaderBackColour", headerbackcolour.ToString(CultureInfo.CurrentCulture));
            //config.Set("BgColour", bgcolour.ToString());

            if (textfont == null)
            {
                textfont = "Tahoma";
                textfontstyle = "Regular";
                textfontsize = 8.5f;
            }

            config.Set("TextFont", textfont);
            config.Set("TextFontStyle", textfontstyle);
            config.Set("TextFontSize", textfontsize.ToString(CultureInfo.CurrentCulture));
            config.Set("PlayFriendOnline", playfriendonline.ToString(CultureInfo.CurrentCulture));
            config.Set("PlayFriendOffline", playfriendoffline.ToString(CultureInfo.CurrentCulture));
            config.Set("PlayIMreceived", playimreceived.ToString(CultureInfo.CurrentCulture));
            config.Set("PlayGroupIMreceived", playgroupimreceived.ToString(CultureInfo.CurrentCulture));
            config.Set("PlayGroupNoticeReceived", playgroupnotice.ToString(CultureInfo.CurrentCulture));
            config.Set("PlayInventoryItemReceived", playintentoryitem.ToString(CultureInfo.CurrentCulture));
            config.Set("PlayPaymentReceived", playpaymentreceived.ToString(CultureInfo.CurrentCulture));
            config.Set("AutoAcceptItems", autoacceptitems.ToString(CultureInfo.CurrentCulture));
            config.Set("StartMinimised", startminimised.ToString(CultureInfo.CurrentCulture));
            config.Set("AdRemove", adremove);
            config.Set("MasterAvatar", masteravatar);
            config.Set("MasterObject", masterobject);
            config.Set("EnforceLSLsecurity", enforcelslsecurity.ToString(CultureInfo.CurrentCulture));
            config.Set("DisplayLSLcommands", displaylslcommands.ToString(CultureInfo.CurrentCulture));  
            config.Set("AutoTransfer", autotransfer.ToString(CultureInfo.CurrentCulture));
            config.Set("DisableTrayIcon", disabletrayicon.ToString(CultureInfo.CurrentCulture));
            config.Set("DisableFriendsNotifications", disablefriendsnotifications.ToString(CultureInfo.CurrentCulture));
            config.Set("DisableTyping", disabletyping.ToString(CultureInfo.CurrentCulture));
            config.Set("AutoAcceptFriends", autoacceptfriends.ToString(CultureInfo.CurrentCulture));
            //config.Set("BroadcastID", broadcastid.ToString(CultureInfo.CurrentCulture));
            config.Set("DisableRadar", disableradar.ToString(CultureInfo.CurrentCulture));
            config.Set("RestrictRadar", restrictradar.ToString(CultureInfo.CurrentCulture));
            config.Set("DisableVoice", disablevoice.ToString(CultureInfo.CurrentCulture));
            config.Set("DisableFavs", disablefavs.ToString(CultureInfo.CurrentCulture));
            config.Set("DisableHTTPinv", disablehttpinv.ToString(CultureInfo.CurrentCulture));
            config.Set("DisableRadarImageMiniMap", disableradarminimap.ToString(CultureInfo.CurrentCulture));
            config.Set("AppMenuPos", appmenupos.ToString(CultureInfo.CurrentCulture));
            config.Set("LandMenuPos", landmenupos.ToString(CultureInfo.CurrentCulture));
            config.Set("FnMenuPos", fnmenupos.ToString(CultureInfo.CurrentCulture));
            config.Set("UseLLSD", usellsd.ToString(CultureInfo.CurrentCulture));

            config.Set("ChatBufferLimit", chatbufferlimit.ToString(CultureInfo.CurrentCulture));
            config.Set("ScriptUrlBufferLimit", scripturlbufferlimit.ToString(CultureInfo.CurrentCulture));
            
            // Interface
            config = source.AddConfig("Interface");
            config.Set("MainWindowState", mainWindowState.ToString(CultureInfo.CurrentCulture));
            config.Set("Style", interfaceStyle.ToString(CultureInfo.CurrentCulture));

            // Login
            config = source.AddConfig("Login");
            config.Set("FirstName", firstName);
            config.Set("LastName", lastName);

            if (irempwd)
            {
                string epwd = passwordMD5;

                if (!string.IsNullOrEmpty(epwd))
                {
                    Crypto cryp = new Crypto(Crypto.SymmProvEnum.Rijndael);
                    //string cpwd = cryp.Encrypting(epwd);
                    string cpwd = cryp.Encrypt(epwd);

                    config.Set("Password", cpwd);
                }
            }
            else
            {
                config.Set("Password", string.Empty);
            }

            config.Set("UserNameList", usernamelist);
            config.Set("Grid", loginGrid.ToString(CultureInfo.CurrentCulture));
            config.Set("Uri", loginUri);
            config.Set("LocationType", loginLocationType.ToString(CultureInfo.CurrentCulture));
            config.Set("Location", loginLocation);
            config.Set("iRemPWD", irempwd.ToString(CultureInfo.CurrentCulture));

            // AI
            config = source.AddConfig("AI");
            config.Set("AIon", aion.ToString(CultureInfo.CurrentCulture));
            config.Set("ReplyAI", replyAI.ToString(CultureInfo.CurrentCulture));
            config.Set("ReplyText", replyText);
            config.Set("MultiLingualAI", multilingualai.ToString(CultureInfo.CurrentCulture));

            // Text
            config = source.AddConfig("Text");
            config.Set("ChatTimestamps", chatTimestamps.ToString(CultureInfo.CurrentCulture));
            config.Set("IMTimestamps", imTimestamps.ToString(CultureInfo.CurrentCulture));
            config.Set("ChatSmileys", chatSmileys.ToString(CultureInfo.CurrentCulture));
            config.Set("ParcelMusic", parcelmusic.ToString(CultureInfo.CurrentCulture));
            config.Set("ParcelMedia", parcelmedia.ToString(CultureInfo.CurrentCulture));
            config.Set("lineMax", linemax.ToString(CultureInfo.CurrentCulture));
            config.Set("UseSLT", useslt.ToString(CultureInfo.CurrentCulture));
            config.Set("PlaySound", playsound.ToString(CultureInfo.CurrentCulture));
            config.Set("BusyReply", busyreply);
            config.Set("InitialIMReply", initialIMreply);
            config.Set("SaveIMs", saveims.ToString(CultureInfo.CurrentCulture));
            config.Set("SaveChat", savechat.ToString(CultureInfo.CurrentCulture));
            config.Set("LogDir", logdir);
            config.Set("DisableGroupNotices", disablegnotices.ToString(CultureInfo.CurrentCulture));
            config.Set("DisableGroupIMs", disablegims.ToString(CultureInfo.CurrentCulture));

            //// Twitter
            //config = source.AddConfig("Twitter");
            //config.Set("TweeterName", tweetername);
            //config.Set("TweeterPwd", tweeterpwd);
            //config.Set("EnableTweeter", enabletweeter.ToString());
            //config.Set("EnableChatTweets", enablechattweets.ToString());   
            //config.Set("Tweet", tweet.ToString());
            //config.Set("TweeterUser", tweetuser);

            // Proxy
            config = source.AddConfig("Proxy");
            config.Set("UseProxy", useproxy.ToString(CultureInfo.CurrentCulture));
            config.Set("ProxyURL", proxyurl);
            config.Set("ProxyPort", proxyport);
            config.Set("ProxyUser", proxyuser);
            config.Set("ProxyPWD", proxypwd);

            // META3D
            config = source.AddConfig("META3D");
            config.Set("DisableMipmaps", disablemipmaps.ToString(CultureInfo.CurrentCulture));

            // Plugins Loaded
            config = source.AddConfig("LoadedPlugIns");
            config.Set("PluginsToLoad", pluginstoload);   

            // Plugins
            //added by GM on 2-JUL-2009
            config = source.AddConfig("PlugIn");
            //don't save if default
            if (groupManagerUid != "ned49b54-325d-486a-af3m")
            {
                config.Set("GroupManager", groupManagerUid);
            }
            config.Set("ChairAnnouncer", chairAnnouncerUuid.ToString());
            config.Set("ChairAnnouncerInterval", chairAnnouncerInterval);
            config.Set("ChairAnnouncerGroup1", chairAnnouncerGroup1.ToString());
            config.Set("ChairAnnouncerGroup2", chairAnnouncerGroup2.ToString());
            config.Set("ChairAnnouncerGroup3", chairAnnouncerGroup3.ToString());
            config.Set("ChairAnnouncerGroup4", chairAnnouncerGroup4.ToString());
            config.Set("ChairAnnouncerGroup5", chairAnnouncerGroup5.ToString());
            config.Set("ChairAnnouncerGroup6", chairAnnouncerGroup6.ToString());
            config.Set("ChairAnnouncerEnabled", chairAnnouncerEnabled);
            config.Set("ChairAnnouncerChat", chairAnnouncerChat);
            //added by GM on 1-APR-2009
            config.Set("ChairAnnouncerAdvert", chairAnnouncerAdvert);

            config = source.AddConfig("Spelling");
            config.Set("EnableSpelling", enablespellcheck.ToString(CultureInfo.CurrentCulture));
            config.Set("SpellLanguage", spelllang);

            FileInfo newFileInfo = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt", filename));

            if (newFileInfo.Exists)
            {
                if (newFileInfo.IsReadOnly)
                {
                    newFileInfo.IsReadOnly = false;
                }
            }

            source.Save(filename);
        }

        public int Version
        {
            get { return configVersion; }
            set { configVersion = value; }
        }

        public int MainWindowState
        {
            get { return mainWindowState; }
            set { mainWindowState = value; }
        }

        public int InterfaceStyle
        {
            get { return interfaceStyle; }
            set { interfaceStyle = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string PasswordMD5
        {
            get { return passwordMD5; }
            set { passwordMD5 = value; }
        }

        //public bool Md5
        //{
        //    get { return md5; }
        //    set { md5 = value; }
        //}

        public int LoginLocationType
        {
            get { return loginLocationType; }
            set { loginLocationType = value; }
        }

        public string LoginLocation
        {
            get { return loginLocation; }
            set { loginLocation = value; }
        }

        public int LoginGrid
        {
            get { return loginGrid; }
            set { loginGrid = value; }
        }

        public string LoginUri
        {
            get { return loginUri; }
            set { loginUri = value; }
        }

        public bool ChatTimestamps
        {
            get { return chatTimestamps; }
            set { chatTimestamps = value; }
        }

        public bool ChatSmileys
        {
            get { return chatSmileys; }
            set { chatSmileys = value; }
        }

        public bool IMTimestamps
        {
            get { return imTimestamps; }
            set { imTimestamps = value; }
        }

        public bool ParcelMusic
        {
            get { return parcelmusic; }
            set { parcelmusic = value; }
        }

        public bool ParcelMedia
        {
            get { return parcelmedia; }
            set { parcelmedia = value; }
        }

        public bool iRemPWD
        {
            get { return irempwd; }
            set { irempwd = value; }
        }

        //public bool iRadar
        //{
        //    get { return iradar; }
        //    set { iradar = value; }
        //}

        public string pURL
        {
            get { return purl; }
            set { purl = value; }
        }

        public string mURL
        {
            get { return murl; }
            set { murl = value; }
        }

        public UUID ObjectsFolder
        {
            get { return objectsfolder; }
            set { objectsfolder = value; }
        }

        public bool DisableNotifications
        {
            get { return disablenotifications; }
            set { disablenotifications = value; }
        }

        public int lineMax
        {
            get { return linemax; }
            set { linemax = value; }
        }

        //public bool EnableTweeter
        //{
        //    get { return enabletweeter; }
        //    set { enabletweeter = value; }
        //}

        //public bool EnableChatTweets
        //{
        //    get { return enablechattweets; }
        //    set { enablechattweets = value; }
        //}

        //public string TweeterName
        //{
        //    get { return tweetername; }
        //    set { tweetername = value; }
        //}

        //public string TweeterPwd
        //{
        //    get { return tweeterpwd; }
        //    set { tweeterpwd = value; }
        //}

        //public bool Tweet
        //{
        //    get { return tweet; }
        //    set { tweet = value; }
        //}

        //public string TweeterUser
        //{
        //    get { return tweetuser; }
        //    set { tweetuser = value; }
        //}

        public bool Connect4
        {
            get { return connect4; }
            set { connect4 = value; }
        }

        public bool AIon
        {
            get { return aion; }
            set { aion = value; }
        }

        public bool AutoSit
        {
            get { return autosit; }
            set { autosit = value; }
        }

        public int RadarRange
        {
            get { return radarrange; }
            set { radarrange = value; }
        }

        public bool UseSLT
        {
            get { return useslt; }
            set { useslt = value; }
        }

        public int ObjectRange
        {
            get { return objectrange; }
            set { objectrange = value; }
        }

        public string GroupManPro
        {
            get { return groupmanpro; }
            set { groupmanpro = value; }
        }

        public bool PlaySound
        {
            get { return playsound; }
            set { playsound = value; }
        }

        public bool HideMeta
        {
            get { return metahide; }
            set { metahide = value; }
        }

        public string BusyReply
        {
            get { return busyreply; }
            set { busyreply = value; }
        }

        public string InitialIMReply
        {
            get { return initialIMreply; }
            set { initialIMreply = value; }
        }

        public bool DeclineInv
        {
            get { return declineinv; }
            set { declineinv = value; }
        }

        public bool ReplyAI
        {
            get { return replyAI; }
            set { replyAI = value; }
        }

        public string ReplyText
        {
            get { return replyText; }
            set { replyText = value; }
        }

        //added by GM on 2-JUL-2009
        public string GroupManagerUID
        {
            get { return groupManagerUid; }
            set { groupManagerUid = value; }
        }

        public UUID ChairAnnouncerUUID
        {
            get { return chairAnnouncerUuid; }
            set { chairAnnouncerUuid = value; }
        }

        public int ChairAnnouncerInterval
        {
            get { return chairAnnouncerInterval < 5 ? 5 : chairAnnouncerInterval; } //spam protection
            set { chairAnnouncerInterval = value; }
        }

        public UUID ChairAnnouncerGroup1
        {
            get { return chairAnnouncerGroup1; }
            set { chairAnnouncerGroup1 = value; }
        }

        public UUID ChairAnnouncerGroup2
        {
            get { return chairAnnouncerGroup2; }
            set { chairAnnouncerGroup2 = value; }
        }

        public UUID ChairAnnouncerGroup3
        {
            get { return chairAnnouncerGroup3; }
            set { chairAnnouncerGroup3 = value; }
        }

        public UUID ChairAnnouncerGroup4
        {
            get { return chairAnnouncerGroup4; }
            set { chairAnnouncerGroup4 = value; }
        }

        public UUID ChairAnnouncerGroup5
        {
            get { return chairAnnouncerGroup5; }
            set { chairAnnouncerGroup5 = value; }
        }

        public UUID ChairAnnouncerGroup6
        {
            get { return chairAnnouncerGroup6; }
            set { chairAnnouncerGroup6 = value; }
        }

        public bool ChairAnnouncerEnabled
        {
            get { return chairAnnouncerEnabled; }
            set { chairAnnouncerEnabled = value; }
        }

        public bool ChairAnnouncerChat
        {
            get { return chairAnnouncerChat; }
            set { chairAnnouncerChat = value; }
        }

        public string ChairAnnouncerAdvert
        {
            get { return chairAnnouncerAdvert; }
            set { chairAnnouncerAdvert = value; }
        }


        public string CommandInID
        {
            get { return commandinid; }
            set { commandinid = value; }
        }

        public bool SaveIMs
        {
            get { return saveims; }
            set { saveims = value; }
        }

        public bool SaveChat
        {
            get { return savechat; }
            set { savechat = value; }
        }

        public string LogDir
        {
            get { return logdir; }
            set { logdir = value; }
        }

        public bool DisableGroupNotices
        {
            get { return disablegnotices; }
            set { disablegnotices = value; }
        }

        public bool DisableInboundGroupInvites
        {
            get { return disableinboundgroupinvites; }
            set { disableinboundgroupinvites = value; }
        }

        public bool DisableGroupIMs
        {
            get { return disablegims; }
            set { disablegims = value; }
        }

        public bool BufferApplied
        {
            get { return bufferapplied; }
            set { bufferapplied = value; }
        }

        public bool DisableLookAt
        {
            get { return disablelookat; }
            set { disablelookat = value; }
        }

        public bool GivePresent
        {
            get { return givepressie; }
            set { givepressie = value; }
        }

        public bool UseProxy
        {
            get { return useproxy; }
            set { useproxy = value; }
        }

        public string ProxyURL
        {
            get { return proxyurl; }
            set { proxyurl = value; }
        }

        public string ProxyPort
        {
            get { return proxyport; }
            set { proxyport = value; }
        }

        public string ProxyUser
        {
            get { return proxyuser; }
            set { proxyuser = value; }
        }

        public string ProxyPWD
        {
            get { return proxypwd; }
            set { proxypwd = value; }
        }

        public bool AutoRestart
        {
            get { return autorestart; }
            set { autorestart = value; }
        }

        public int LogOffTime
        {
            get { return logofftime; }
            set { logofftime = value; }
        }

        public float BandwidthThrottle
        {
            get { return bandwidththrottle; }
            set { bandwidththrottle = value; }
        }

        public bool LogOffTimerChanged
        {
            get { return logofftimerchanged; }
            set { logofftimerchanged = value; }
        }

        public bool ClassicChatLayout
        {
            get { return useclassicchatlayout; }
            set { useclassicchatlayout = value; }
        }

        public string HeaderFont
        {
            get { return headerfont; }
            set { headerfont = value; }
        }

        public string HeaderFontStyle
        {
            get { return headerfontstyle; }
            set { headerfontstyle = value; }
        }

        public float HeaderFontSize
        {
            get { return headerfontsize; }
            set { headerfontsize = value; }
        }

        public Color HeaderBackColour
        {
            get { return Color.FromArgb(headerbackcolour) ; }
            set { headerbackcolour = value.ToArgb(); }
        }

        //public Color BgColour
        //{
        //    get { return Color.FromArgb(bgcolour); }
        //    set { bgcolour = value.ToArgb(); }
        //}

        public string TextFont
        {
            get { return textfont; }
            set { textfont = value; }
        }

        public string TextFontStyle
        {
            get { return textfontstyle; }
            set { textfontstyle = value; }
        }

        public float TextFontSize
        {
            get { return textfontsize; }
            set { textfontsize = value; }
        }

        public string PluginsToLoad
        {
            get { return pluginstoload; }
            set { pluginstoload = value; }
        }

        public bool PlayFriendOnline
        {
            get { return playfriendonline; }
            set { playfriendonline = value; }
        }

        public bool PlayFriendOffline
        {
            get { return playfriendoffline; }
            set { playfriendoffline = value; }
        }

        public bool PlayIMreceived
        {
            get { return playimreceived; }
            set { playimreceived = value; }
        }

        public bool PlayGroupIMreceived
        {
            get { return playgroupimreceived; }
            set { playgroupimreceived = value; }
        }

        public bool PlayGroupNoticeReceived
        {
            get { return playgroupnotice; }
            set { playgroupnotice = value; }
        }

        public bool PlayInventoryItemReceived
        {
            get { return playintentoryitem; }
            set { playintentoryitem = value; }
        }

        public bool PlayPaymentReceived
        {
            get { return playpaymentreceived; }
            set { playpaymentreceived = value; }
        }

        public bool AutoAcceptItems
        {
            get { return autoacceptitems; }
            set { autoacceptitems = value; }
        }

        public bool StartMinimised
        {
            get { return startminimised; }
            set { startminimised = value; }
        }

        public string AdRemove
        {
            get { return adremove; }
            set { adremove = value; }
        }

        public string MasterAvatar
        {
            get { return masteravatar; }
            set { masteravatar = value; }
        }

        public string MasterObject
        {
            get { return masterobject; }
            set { masterobject = value; }
        }

        public bool AutoTransfer
        {
            get { return autotransfer; }
            set { autotransfer = value; }
        }

        public bool SortByDistance
        {
            get { return sortbydistance; }
            set { sortbydistance = value; }
        }

        public bool DisableTrayIcon
        {
            get { return disabletrayicon; }
            set { disabletrayicon = value; }
        }

        public string IgnoreUID
        {
            get { return ignoreUid; }
            set { ignoreUid = value; }
        }

        public string UserNameList
        {
            get { return usernamelist; }
            set { usernamelist = value; }
        }

        public bool DisableFriendsNotifications
        {
            get { return disablefriendsnotifications; }
            set { disablefriendsnotifications = value; }
        }

        public bool DisableTyping
        {
            get { return disabletyping ; }
            set { disabletyping = value; }
        }

        public bool AutoAcceptFriends
        {
            get { return autoacceptfriends; }
            set { autoacceptfriends = value; }
        }

        public int ReStartTime
        {
            get { return restarttime; }
            set { restarttime = value; }
        }

        public bool DisableMipmaps
        {
            get { return disablemipmaps; }
            set { disablemipmaps = value; }
        }

        public bool EnforceLSLsecurity
        {
            get { return enforcelslsecurity; }
            set { enforcelslsecurity = value; }
        }

        public bool DisplayLSLcommands
        {
            get { return displaylslcommands; }
            set { displaylslcommands = value; }
        }
        public bool MultiLingualAI
        {
            get { return multilingualai; }
            set { multilingualai = value; }
        }

        public bool EnableSpelling
        {
            get { return enablespellcheck; }
            set { enablespellcheck = value; }
        }

        public string SpellLanguage
        {
            get { return spelllang; }
            set { spelllang = value; }
        }

        public bool HideDisconnectPrompt
        {
            get { return hidedisconnectprompt; }
            set { hidedisconnectprompt = value; }
        }

        public bool DisableRadar
        {
            get { return disableradar; }
            set { disableradar = value; }
        }

        public bool RestrictRadar
        {
            get { return restrictradar; }
            set { restrictradar = value; }
        }

        public bool DisableVoice
        {
            get { return disablevoice; }
            set { disablevoice = value; }
        }

        public bool DisableFavs
        {
            get { return disablefavs; }
            set { disablefavs = value; }
        }

        public bool DisableHTTPinv
        {
            get { return disablehttpinv; }
            set { disablehttpinv = value; }
        }

        public bool DisableRadarImageMiniMap
        {
            get { return disableradarminimap; }
            set { disableradarminimap = value; }
        }

        public string AppMenuPos
        {
            get { return appmenupos; }
            set { appmenupos = value; }
        }

        public string LandMenuPos
        {
            get { return landmenupos; }
            set { landmenupos = value; }
        }

        public string FnMenuPos
        {
            get { return fnmenupos; }
            set { fnmenupos = value; }
        }

        public bool UseLLSD
        {
            get { return usellsd; }
            set { usellsd = value; }
        }

        public int ChatBufferLimit
        {
            get { return chatbufferlimit; }
            set { chatbufferlimit = value; }
        }

        public int ScriptUrlBufferLimit
        {
            get { return scripturlbufferlimit; }
            set { scripturlbufferlimit = value; }
        }
    }
}