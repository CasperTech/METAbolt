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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using SLNetworkComm;
using OpenMetaverse;
//using Khendys.Controls;
//using Yedda;
using System.Windows.Forms;
using AIMLbot;
using METAbrain;
using System.Timers;
using System.IO;
using ExceptionReporting;
 

namespace METAbolt
{
    public class IMTextManager
    {
        private METAboltInstance instance;
        private SLNetCom netcom;
        private ITextPrinter textPrinter;
        private UUID sessionID = UUID.Zero;
        private string sessionAVname = string.Empty;
        //private string sessionGroupName = string.Empty;  
        private GridClient client;
        //private string tName = string.Empty;
        //private string tPwd = string.Empty;
        //private bool TEnabled = false;
        //private bool tweet = true;
        //private string tweetname = string.Empty;
        public mBrain answer;

        //private ArrayList textBuffer;
        private bool showTimestamps;
        private AIMLbot.Bot myBot;
        private string lastspeaker = string.Empty;
        private bool classiclayout = false;
        private METAbrain brain;
        private ExceptionReporter reporter = new ExceptionReporter();

        internal class ThreadExceptionHandler
        {
            public void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
            {
                ExceptionReporter reporter = new ExceptionReporter();
                reporter.Show(e.Exception);
            }
        }

        //public IMTextManager(METAboltInstance instance, ITextPrinter textPrinter, UUID sessionID)
        //{
        //    this.sessionID = sessionID;

        //    this.textPrinter = textPrinter;
        //    this.textBuffer = new ArrayList();

        //    this.instance = instance;
        //    client = this.instance.Client;
        //    netcom = this.instance.Netcom;
        //    AddNetcomEvents();

        //    showTimestamps = this.instance.Config.CurrentConfig.IMTimestamps;
        //    tName = this.instance.Config.CurrentConfig.TweeterName;
        //    tPwd = this.instance.Config.CurrentConfig.TweeterPwd;
        //    TEnabled = this.instance.Config.CurrentConfig.EnableTweeter;
        //    tweet = this.instance.Config.CurrentConfig.Tweet;
        //    tweetname = this.instance.Config.CurrentConfig.TweeterUser;
        //    classiclayout = this.instance.Config.CurrentConfig.ClassicChatLayout;

        //    this.instance.Config.ConfigApplied += new EventHandler<ConfigAppliedEventArgs>(Config_ConfigApplied);

        //    myBot = this.instance.ABot;
        //}

        public IMTextManager(METAboltInstance instance, ITextPrinter textPrinter, UUID sessionID, string groupname, Group grp)
        {
            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.sessionID = sessionID;
            //this.sessionGroupName = groupname; 

            this.textPrinter = textPrinter;
            //this.textBuffer = new ArrayList();

            this.instance = instance;
            client = this.instance.Client;
            netcom = this.instance.Netcom;
            AddNetcomEvents();

            showTimestamps = this.instance.Config.CurrentConfig.IMTimestamps;
            //tName = this.instance.Config.CurrentConfig.TweeterName;
            //tPwd = this.instance.Config.CurrentConfig.TweeterPwd;
            //TEnabled = this.instance.Config.CurrentConfig.EnableTweeter;
            //tweet = this.instance.Config.CurrentConfig.Tweet;
            //tweetname = this.instance.Config.CurrentConfig.TweeterUser;
            classiclayout = this.instance.Config.CurrentConfig.ClassicChatLayout;

            this.instance.Config.ConfigApplied += new EventHandler<ConfigAppliedEventArgs>(Config_ConfigApplied);

            myBot = this.instance.ABot;

            //if (this.instance.Config.CurrentConfig.AIon)
            //{
            //    myBot = this.instance.ABot;
            //    //brain = new METAbrain(instance, myBot);
            //}
        }

        public IMTextManager(METAboltInstance instance, ITextPrinter textPrinter, UUID sessionID, string avname)
        {
            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.sessionID = sessionID;
            this.sessionAVname = avname;

            this.textPrinter = textPrinter;
            //this.textBuffer = new ArrayList();

            this.instance = instance;
            client = this.instance.Client;
            netcom = this.instance.Netcom;
            AddNetcomEvents();

            showTimestamps = this.instance.Config.CurrentConfig.IMTimestamps;
            //tName = this.instance.Config.CurrentConfig.TweeterName;
            //tPwd = this.instance.Config.CurrentConfig.TweeterPwd;
            //TEnabled = this.instance.Config.CurrentConfig.EnableTweeter;
            //tweet = this.instance.Config.CurrentConfig.Tweet;
            //tweetname = this.instance.Config.CurrentConfig.TweeterUser;
            classiclayout = this.instance.Config.CurrentConfig.ClassicChatLayout;

            this.instance.Config.ConfigApplied += new EventHandler<ConfigAppliedEventArgs>(Config_ConfigApplied);

            myBot = this.instance.ABot;

            //if (this.instance.Config.CurrentConfig.AIon)
            //{
            //    myBot = this.instance.ABot;
            //    //brain = new METAbrain(instance, myBot);
            //}
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

        private void Config_ConfigApplied(object sender, ConfigAppliedEventArgs e)
        {
            showTimestamps = e.AppliedConfig.IMTimestamps;
            //ReprintAllText();

            //tName = e.AppliedConfig.TweeterName;
            //tPwd = e.AppliedConfig.TweeterPwd;
            //TEnabled = e.AppliedConfig.EnableTweeter;
            //tweet = this.instance.Config.CurrentConfig.Tweet;
            //tweetname = this.instance.Config.CurrentConfig.TweeterUser;
        }

        private void AddNetcomEvents()
        {
            netcom.InstantMessageReceived += new EventHandler<InstantMessageEventArgs>(netcom_InstantMessageReceived);
            netcom.InstantMessageSent += new EventHandler<SLNetworkComm.InstantMessageSentEventArgs>(netcom_InstantMessageSent);
        }

        private void RemoveNetcomEvents()
        {
            netcom.InstantMessageReceived -= netcom_InstantMessageReceived;
            netcom.InstantMessageSent -= netcom_InstantMessageSent;
        }

        private void netcom_InstantMessageSent(object sender, InstantMessageSentEventArgs e)
        {
            if (e.SessionID != sessionID) return;

            //textBuffer.Add(e);

            //int lines = textBuffer.Count;
            //int maxlines = this.instance.Config.CurrentConfig.lineMax;

            //if (lines > maxlines && maxlines > 0)
            //{
            //    CheckBufferSize();
            //    return;
            //}

            ProcessIM(e);
        }

        private void netcom_InstantMessageReceived(object sender, InstantMessageEventArgs e)
        {
            if (e.IM.IMSessionID != sessionID)
            {
                return;
            }

            if (e.IM.Dialog == InstantMessageDialog.StartTyping ||
                e.IM.Dialog == InstantMessageDialog.StopTyping ||
                e.IM.Dialog == InstantMessageDialog.MessageFromObject)
                return;

            string cp = METAbolt.Properties.Resources.ChairPrefix;

            if (e.IM.FromAgentID != client.Self.AgentID)
            {
                //textBuffer.Add(e);

                //int lines = textBuffer.Count;
                //int maxlines = this.instance.Config.CurrentConfig.lineMax;

                //if (lines > maxlines && maxlines > 0)
                //{
                //    CheckBufferSize();
                //    return;
                //}

                ProcessIM(e);
            }
            //GM new bit to show our Chair Announcing
            //not pretty but how else can we catch just the calling stuff?
            else if (e.IM.FromAgentID == client.Self.AgentID && e.IM.Message.StartsWith(cp))
            {
                //textBuffer.Add(e);

                //int lines = textBuffer.Count;
                //int maxlines = this.instance.Config.CurrentConfig.lineMax;

                //if (lines > maxlines && maxlines > 0)
                //{
                //    CheckBufferSize();
                //    return;
                //}

                ProcessIM(e);
            }
        }

        public void ProcessIM(object e)
        {
            if (e is InstantMessageEventArgs)
                this.ProcessIncomingIM((InstantMessageEventArgs)e);
            else if (e is InstantMessageSentEventArgs)
                this.ProcessOutgoingIM((InstantMessageSentEventArgs)e);
        }

        private void ProcessOutgoingIM(InstantMessageSentEventArgs e)
        {
            PrintIM(DateTime.Now , e.TargetID.ToString(), netcom.LoginOptions.FullName, e.Message, e.SessionID);
        }

        private void ProcessIncomingIM(InstantMessageEventArgs e)
        {
            // Check to see if avatar is muted
            if (instance.IsAvatarMuted(e.IM.FromAgentID))
                return;

            string iuid = this.instance.Config.CurrentConfig.IgnoreUID;

            if (e.IM.Message.Contains(iuid)) return; // Ignore Im for plugins use etc.

            bool isgroup = this.instance.State.GroupStore.ContainsKey(e.IM.IMSessionID);

            if (isgroup)
            {
                // Check to see if group IMs are disabled
                if (instance.Config.CurrentConfig.DisableGroupIMs)
                    return;
            }
            
            PrintIM(DateTime.Now, e.IM.FromAgentID.ToString(), e.IM.FromAgentName, e.IM.Message, e.IM.IMSessionID);

            //string msg = ">>> " + e.IM.FromAgentName + ": " + e.IM.Message;

            //// Handles twitter
            //if (TEnabled)
            //{
            //    if (!isgroup)
            //    {
            //        Yedda.Twitter twit = new Yedda.Twitter();
            //        string resp = string.Empty;

            //        if (tweet)
            //        {
            //            // if enabled print to Twitter
            //            resp = twit.UpdateAsJSON(tName, tPwd, msg);
            //        }
            //        else
            //        {
            //            // it's a direct message
            //            resp = twit.Send(tName, tPwd, tweetname, msg);
            //        }

            //        if (resp != "OK")
            //        {
            //            Logger.Log("Twitter error: " + resp, Helpers.LogLevel.Warning);
            //        }
            //    }
            //}

            if (!isgroup)
            {
                if (instance.State.IsBusy)
                {
                    string responsemsg = this.instance.Config.CurrentConfig.BusyReply;
                    client.Self.InstantMessage(client.Self.Name, e.IM.FromAgentID, responsemsg, e.IM.IMSessionID, InstantMessageDialog.BusyAutoResponse, InstantMessageOnline.Offline, instance.SIMsittingPos(), UUID.Zero, new byte[0]); 
                }
                else
                {
                    // Handles METAbrain
                    if (this.instance.Config.CurrentConfig.AIon)
                    {
                        if (e.IM.FromAgentID == client.Self.AgentID) return;
                        if (client.Self.GroupChatSessions.ContainsKey(e.IM.IMSessionID)) return;
                        if (e.IM.FromAgentName == "Second Life") return;
                        if (e.IM.FromAgentName.Contains("Linden")) return;
                        if (e.IM.Dialog == InstantMessageDialog.SessionSend) return;

                        ////METAbrain brain = new METAbrain(instance, myBot, e);
                        brain = new METAbrain(instance, myBot);
                        brain.StartProcess(e);
                    }
                }
            }
        }

        private void LogMessage(DateTime timestamp, string uuid, string fromName, string msg, bool group, string groupname)
        {
            if (!instance.Config.CurrentConfig.SaveIMs)
                return;

            string folder = instance.Config.CurrentConfig.LogDir;

            if (!folder.EndsWith("\\"))
            {
                folder += "\\";
            }
    
            // Log the message
            string filename = string.Empty;

            if (group)
            {
                filename = "IM-" + timestamp.Date.ToString() + "-" + client.Self.FirstName + " " + client.Self.LastName + "-GROUP-" + groupname + ".txt";
            }
            else
            {
                filename = "IM-" + timestamp.Date.ToString() + "-" + client.Self.FirstName + " " + client.Self.LastName + "-" + sessionAVname + ".txt";
            }

            filename = filename.Replace("/", "-");
            //filename = filename.Replace(" ", "_");
            filename = filename.Replace(":", "-");

            StreamWriter SW;
            string path = folder + filename;
            string line = "[" + timestamp.ToShortTimeString() + "] " + fromName + ": " + msg; 

            bool exists = false;

            // Check if the file exists
            try
            {
                exists = File.Exists(path);
            }
            catch
            {
                ; 
            }

            if (exists)
            {
                try
                {
                    SW = File.AppendText(path);
                    SW.WriteLine(line);
                    SW.Dispose(); 
                }
                catch
                {
                    ;
                }
            }
            else
            {
                try
                {
                    SW = File.CreateText(path);
                    SW.WriteLine(line);
                    SW.Dispose();
                }
                catch (Exception ex)
                {
                    string exp = ex.Message;
                }
            }
        }

        private void PrintIM(DateTime timestamp, string uuid, string fromName, string message, UUID ssessionID)
        {
            StringBuilder sb = new StringBuilder();

            if (classiclayout)
            {
                if (showTimestamps)
                {
                    timestamp = this.instance.State.GetTimeStamp(timestamp);

                    //if (instance.Config.CurrentConfig.UseSLT)
                    //{
                    //    string _timeZoneId = "Pacific Standard Time";
                    //    DateTime startTime = DateTime.UtcNow;
                    //    TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId);
                    //    timestamp = TimeZoneInfo.ConvertTime(startTime, TimeZoneInfo.Utc, tst);
                    //}

                    try
                    {
                        textPrinter.SetSelectionForeColor(Color.Gray);
                    }
                    catch
                    {
                        ;
                    }

                    textPrinter.PrintText(timestamp.ToString("[HH:mm] "));
                }

                try
                {
                    textPrinter.SetSelectionForeColor(Color.Black);
                }
                catch
                {
                    ;
                }

                if (message.StartsWith("/me "))
                {
                    sb.Append(fromName);
                    sb.Append(message.Substring(3));
                }
                else
                {
                    sb.Append(message);


                    string avid = "http://mbprofile:" + uuid.ToString();

                    if (fromName.ToLower() != client.Self.Name.ToLower())
                    {
                        if (!string.IsNullOrEmpty(fromName))
                        {
                            textPrinter.PrintLink(fromName, avid + "&" + fromName);
                        }

                        textPrinter.PrintText(": ");
                    }
                    else
                    {
                        textPrinter.PrintText(fromName + ": ");
                    }
                }
            }
            else
            {
                if (message.StartsWith("/me "))
                {
                    sb.Append(message.Substring(3));
                }
                else
                {
                    sb.Append(message);
                }

                textPrinter.SetSelectionForeColor(Color.Black);

                try
                {
                    string avid = "http://mbprofile:" + uuid.ToString();

                    if (lastspeaker != fromName)
                    {
                        if (fromName.ToLower() != client.Self.Name.ToLower())
                        {
                            //textPrinter.PrintLinkHeader(fromName, avid + "&" + fromName);
                            textPrinter.PrintLinkHeader(fromName, uuid.ToString(), avid + "&" + fromName);
                        }
                        else
                        {
                            textPrinter.SetFontStyle(FontStyle.Bold);
                            textPrinter.PrintHeader(fromName);
                        }

                        lastspeaker = fromName;
                    }
                }
                catch
                {
                    ;
                }

                textPrinter.SetFontStyle(FontStyle.Regular);
                textPrinter.SetSelectionBackColor(Color.White);

                if (showTimestamps)
                {
                    timestamp = this.instance.State.GetTimeStamp(timestamp);

                    //if (instance.Config.CurrentConfig.UseSLT)
                    //{
                    //    string _timeZoneId = "Pacific Standard Time";
                    //    DateTime startTime = DateTime.UtcNow;
                    //    TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId);
                    //    timestamp = TimeZoneInfo.ConvertTime(startTime, TimeZoneInfo.Utc, tst);
                    //}

                    textPrinter.SetSelectionForeColor(Color.Gray);
                    textPrinter.SetOffset(6);
                    textPrinter.SetFontSize(6.5f);
                    textPrinter.PrintDate(timestamp.ToString("[HH:mm] "));
                    textPrinter.SetFontSize(8.5f);
                    textPrinter.SetOffset(0);
                }
            }

            textPrinter.SetSelectionForeColor(Color.Black);

            textPrinter.PrintTextLine(sb.ToString());

            string groupname = string.Empty;
            bool groupfound = this.instance.State.GroupStore.TryGetValue(ssessionID, out groupname);

            LogMessage(timestamp, uuid, fromName, sb.ToString(), groupfound, groupname);

            sb = null;
        }

        //public void ReprintAllText()
        //{
        //    try
        //    {
        //        textPrinter.ClearText();

        //        foreach (object obj in textBuffer)
        //            ProcessIM(obj);
        //    }
        //    catch
        //    {
        //        ;
        //    }
        //}

        private void CheckBufferSize()
        {
            //int lines = textBuffer.Count;
            //int maxlines = this.instance.Config.CurrentConfig.lineMax;

            //if (maxlines == 0)
            //    return;

            //if (lines > maxlines)
            //{
            //    int lineno = maxlines / 2;

            //    for (int a = 0; a < lineno; a++)
            //    {
            //        textBuffer.RemoveAt(a);
            //    }

            //    ReprintAllText();
            //}
        }

        public void ClearInternalBuffer()
        {
            //textBuffer.Clear();
        }

        /// <summary>
        /// Instruct the TextPrinter to clear the contents of the window
        /// </summary>
        public void ClearAllText()
        {
            textPrinter.ClearText();
        }

        public void CleanUp()
        {
            RemoveNetcomEvents();

            //textBuffer.Clear();
            //textBuffer = null;


            textPrinter = null;
        }

        public ITextPrinter TextPrinter
        {
            get { return textPrinter; }
            set { textPrinter = value; }
        }

        public bool ShowTimestamps
        {
            get { return showTimestamps; }
            set { showTimestamps = value; }
        }

        public UUID SessionID
        {
            get { return sessionID; }
            set { sessionID = value; }
        }
    }
}
