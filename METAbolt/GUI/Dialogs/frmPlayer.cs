//  Copyright (c) 2008-2013, www.metabolt.net
//  All rights reserved.

//  Redistribution and use in source and binary forms, with or without modification, 
//  are permitted provided that the following conditions are met:

//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright notice, 
//    this list of conditions and the following disclaimer in the documentation 
//    and/or other materials provided with the distribution. 
//  * Neither the name METAbolt nor the names of its contributors may be used to 
//    endorse or promote products derived from this software without specific prior 
//    written permission. 

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
using OpenMetaverse;
//using AWS_PAAPI;
using System.IO;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;
//using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using SLNetworkComm;
using WMPLib;
using System.Web.UI.WebControls;

namespace METAbolt
{
    public partial class frmPlayer : Form
    {
        private METAboltInstance instance;
        private GridClient client;
        private string currentartist = string.Empty;
        private string currenttrack = string.Empty;

        private string currentlyrics = string.Empty;
        private string dets = string.Empty;
        //private LyricWiki wiki = new LyricWiki();
        private string albumlink = string.Empty;
        private string lyrics = string.Empty;
        private SLNetCom netcom;
        private List<RadioObj> Stations = new List<RadioObj>();


        public frmPlayer(METAboltInstance instance)
        {
            InitializeComponent();

            this.instance = instance;
            client = this.instance.Client;
            netcom = this.instance.Netcom;


            Disposed += new EventHandler(Player_Disposed);

            netcom.TeleportStatusChanged += new EventHandler<TeleportEventArgs>(TP_Callback);

            PopulateStations();

            listBox2.SelectedIndex = 3;
        }

        private void PopulateStations()
        {
            RadioObj robj = new RadioObj();
            robj.genre = "80s";
            robj.radioname = "SKY.FM";
            robj.radiourl = "http://160.79.128.30:7712";
            Stations.Add(robj);

            robj = new RadioObj();
            robj.genre = "80s";
            robj.radioname = "Club .977";
            robj.radiourl = "http://205.188.215.229:8004";
            Stations.Add(robj);


            robj = new RadioObj();
            robj.genre = "Hitz";
            robj.radioname = "Club .977";
            robj.radiourl = "http://205.188.215.230:8002";
            Stations.Add(robj);

            robj = new RadioObj();
            robj.genre = "Hitz";
            robj.radioname = "181.FM";
            robj.radiourl = "http://205.188.215.228:8002";
            Stations.Add(robj);

            robj = new RadioObj();
            robj.genre = "Hitz";
            robj.radioname = "SKY.FM";
            robj.radiourl = "http://207.200.96.230:8002";
            Stations.Add(robj);


            robj = new RadioObj();
            robj.genre = "Dance";
            robj.radioname = "Ibiza Global Radio";
            robj.radiourl = "http://213.251.162.25:8024";
            Stations.Add(robj);

            robj = new RadioObj();
            robj.genre = "Dance";
            robj.radioname = "TechnoBase.FM";
            robj.radiourl = "http://85.17.26.85:80";
            Stations.Add(robj);

            robj = new RadioObj();
            robj.genre = "Dance";
            robj.radioname = "181.FM";
            robj.radiourl = "http://207.200.96.226:8004";
            Stations.Add(robj);


            robj = new RadioObj();
            robj.genre = "Classical";
            robj.radioname = "SKY.FM";
            robj.radiourl = "http://scfire-mtc-aa04.stream.aol.com:80/stream/1006";
            Stations.Add(robj);


            robj = new RadioObj();
            robj.genre = "Jazz";
            robj.radioname = "SKY.FM";
            robj.radiourl = "http://160.79.128.30:7702";
            Stations.Add(robj);

            robj = new RadioObj();
            robj.genre = "Jazz";
            robj.radioname = "SMOOTHJAZZ.COM";
            robj.radiourl = "http://207.200.96.226:8052";
            Stations.Add(robj);


            robj = new RadioObj();
            robj.genre = "Latin";
            robj.radioname = "SKY.FM";
            robj.radiourl = "http://72.26.204.18:6136";
            Stations.Add(robj);

            robj = new RadioObj();
            robj.genre = "Latin";
            robj.radioname = "LATINO FM EN DIRECTO";
            robj.radiourl = "http://92.48.107.35:8000";
            Stations.Add(robj);


            robj = new RadioObj();
            robj.genre = "Trance";
            robj.radioname = "1.FM";
            robj.radiourl = "http://72.13.83.70:8042";
            Stations.Add(robj);


            robj = new RadioObj();
            robj.genre = "R&B";
            robj.radioname = "HOT 108 Jamz";
            robj.radiourl = "http://205.188.215.229:8040";
            Stations.Add(robj);


            robj = new RadioObj();
            robj.genre = "Rock";
            robj.radioname = "RMF FM";
            robj.radiourl = "http://217.74.72.12:9000";
            Stations.Add(robj);


            robj = new RadioObj();
            robj.genre = "Rock (Classic)";
            robj.radioname = "181.FM";
            robj.radiourl = "http://108.61.73.118:8030";
            Stations.Add(robj);
        }

        private void TP_Callback(object sender, TeleportEventArgs e)
        {
            if (e.Status == TeleportStatus.Finished)
            {
                //Thread.Sleep(8000);

                BeginInvoke((MethodInvoker)delegate 
                {
                    //axWindowsMediaPlayer1.Ctlcontrols.stop();
                    axWindowsMediaPlayer1.URL = this.instance.Config.CurrentConfig.pURL;
                    //axWindowsMediaPlayer1.Ctlcontrols.play();

                    //this.Close();
                });
            }
        }

        private void frmPlayer_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            
            axWindowsMediaPlayer1.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(player_PlayStateChange);
            axWindowsMediaPlayer1.CurrentItemChange += new AxWMPLib._WMPOCXEvents_CurrentItemChangeEventHandler(player_CurrentItemChange);
            //axWindowsMediaPlayer1.CurrentPlaylistChange += new AxWMPLib._WMPOCXEvents_CurrentPlaylistChangeEventHandler(player_CurrentPlaylistChange);
            axWindowsMediaPlayer1.MediaChange += new AxWMPLib._WMPOCXEvents_MediaChangeEventHandler(player_MediaChange);
            axWindowsMediaPlayer1.MediaError += new AxWMPLib._WMPOCXEvents_MediaErrorEventHandler(axWindowsMediaPlayer1_MediaError);

            axWindowsMediaPlayer1.URL = this.instance.Config.CurrentConfig.pURL;
            //axWindowsMediaPlayer1.Ctlcontrols.stop();
            label2.Text = string.Empty;
            label3.Text = string.Empty;
        }

        void axWindowsMediaPlayer1_MediaError(object sender, AxWMPLib._WMPOCXEvents_MediaErrorEvent e)
        {
            try
            // If the Player encounters a corrupt or missing file, 
            // show the hexadecimal error code and URL.
            {
                IWMPMedia2 errSource = e.pMediaObject as IWMPMedia2;
                IWMPErrorItem errorItem = errSource.Error;
                MessageBox.Show("Error " + errorItem.errorCode.ToString("X")
                                + " in " + errSource.sourceURL);
            }
            catch (InvalidCastException)
            // In case pMediaObject is not an IWMPMedia item.
            {
                MessageBox.Show("Error.");
            } 
        }

        private void Player_Disposed(object sender, EventArgs e)
        {
            //client.Self.TeleportProgress -= new EventHandler<TeleportEventArgs>(TP_Callback);
            netcom.TeleportStatusChanged -= new EventHandler<TeleportEventArgs>(TP_Callback);

            axWindowsMediaPlayer1.PlayStateChange -= new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(player_PlayStateChange);
            axWindowsMediaPlayer1.CurrentItemChange -= new AxWMPLib._WMPOCXEvents_CurrentItemChangeEventHandler(player_CurrentItemChange);
            //axWindowsMediaPlayer1.CurrentPlaylistChange -= new AxWMPLib._WMPOCXEvents_CurrentPlaylistChangeEventHandler(player_CurrentPlaylistChange);
            axWindowsMediaPlayer1.MediaChange -= new AxWMPLib._WMPOCXEvents_MediaChangeEventHandler(player_MediaChange);
            axWindowsMediaPlayer1.MediaError -= new AxWMPLib._WMPOCXEvents_MediaErrorEventHandler(axWindowsMediaPlayer1_MediaError);
        }

        private void player_MediaChange(object sender, AxWMPLib._WMPOCXEvents_MediaChangeEvent e)
        {
            GetTrackInfo(axWindowsMediaPlayer1.currentMedia.name);
        }

        //private void player_CurrentPlaylistChange(object sender, AxWMPLib._WMPOCXEvents_CurrentPlaylistChangeEvent e)
        //{
        //    GetTrackInfo(axWindowsMediaPlayer1.currentMedia.name);
        //}

        private void player_CurrentItemChange(object sender, AxWMPLib._WMPOCXEvents_CurrentItemChangeEvent e)
        {
            // Display the name of the new media item.
            GetTrackInfo(axWindowsMediaPlayer1.currentMedia.name);
        }

        private void GetTrackInfo(string track)
        {
            if (track.Contains(" - "))
            {
                char[] delimiters = new char[] { '-' };
                string[] words = track.Split(delimiters);

                if (words.Length > 2)
                {
                    currentartist = @words[0].Trim() + "-" + @words[1].Trim();
                    currenttrack = @words[2].Trim();
                }
                else
                {
                    currentartist = @words[0].Trim();
                    currenttrack = @words[1].Trim();
                }

                if (currenttrack.Contains("("))
                {
                    // get them out
                    int pos = currenttrack.IndexOf("(");
                    currenttrack = currenttrack.Substring(0, pos).Trim();
                }

                if (currentartist.ToLower().Contains("feat"))
                {
                    // get them out
                    int pos = currentartist.ToLower().IndexOf("feat",0);
                    currentartist = currentartist.Substring(0, pos).Trim();
                    //currentartist = currentartist.Substring(0, pos).Trim();
                }

                if (currentlyrics == currenttrack)
                    return;

                DateTime timestamp = DateTime.Now;

                timestamp = this.instance.State.GetTimeStamp(timestamp);

                //if (instance.Config.CurrentConfig.UseSLT)
                //{
                //    string _timeZoneId = "Pacific Standard Time";
                //    DateTime startTime = DateTime.UtcNow;
                //    TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId);
                //    timestamp = TimeZoneInfo.ConvertTime(startTime, TimeZoneInfo.Utc, tst);
                //}

                string eval = timestamp.ToShortTimeString() + ": " + track;

                System.Windows.Forms.ListViewItem finditem = listView1.FindItemWithText(eval);


                if (finditem == null)
                {
                    try
                    {
                        getAlbumArt(false);
                        GetLyrics();
                    }
                    catch { ; }

                    System.Windows.Forms.ListViewItem list = new System.Windows.Forms.ListViewItem();

                    list.Text = timestamp.ToShortTimeString() + ": " + track;
                    list.Tag = albumlink;   // dets;

                    if (!string.IsNullOrEmpty(albumlink))
                    {
                        list.ForeColor = Color.Cyan;
                    }

                    listView1.Items.Add(list);
                }
            }
        }


        // Create an event handler for the PlayStateChange event.
        private void player_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            // Display the bitRate when the player is playing. 
            switch (e.newState)
            {
                case 3:  // Play State = WMPLib.WMPPlayState.wmppsPlaying = 3
                    if (axWindowsMediaPlayer1.network.bitRate != 0)
                    {
                        label1.Text = "Bit Rate: " + axWindowsMediaPlayer1.network.bitRate + " K bits/second";
                        pictureBox4.Visible = true;
                        pictureBox5.Visible = true;
                    }
                    break;

                case (int)WMPLib.WMPPlayState.wmppsStopped:
                    label1.Text = "";
                    pictureBox4.Visible = false;
                    pictureBox5.Visible = false;
                    break;

                case (int)WMPLib.WMPPlayState.wmppsPaused:
                    label1.Text = "";
                    break;

                case (int)WMPLib.WMPPlayState.wmppsBuffering:
                    label1.Text = "Buffering...";
                    break;

                default:
                    label1.Text = "";
                    break;
            }
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void GetLyrics()
        {
            if (string.IsNullOrEmpty(currentartist) || string.IsNullOrEmpty(currenttrack))
                return;

            if (currentlyrics == currenttrack)
                return;

            currentlyrics = currenttrack;

            currentartist = currentartist.Replace(" ", "_");
            currenttrack = currenttrack.Replace(" ", "_");

            //richTextBox1.Text = "Click for lyrics:\nhttp://lyrics.wikia.com/Special:Search?titlesOnly=1&search=" + currentartist + ":" + currenttrack;
            lyrics = "http://lyrics.wikia.com/Special:Search?titlesOnly=1&search=" + currentartist + ":" + currenttrack;
        }

        //private string RipURL(string trURL)
        //{
        //    //HttpWebRequest request = null;
        //    //HttpWebResponse response = null;

        //    //try
        //    //{
        //    //    //Make the http request
        //    //    request = (HttpWebRequest)HttpWebRequest.Create(trURL);
        //    //    request.Timeout = 10000;
        //    //    request.ReadWriteTimeout = 15000;
        //    //    request.KeepAlive = false;
        //    //    response = (HttpWebResponse)request.GetResponse();
        //    //    Stream responseStream = response.GetResponseStream();

        //    //    StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8);
        //    //    string page = readStream.ReadToEnd();

        //    //    // Hello LyricsWiki
        //    //    //Regex reg = new Regex(@"<div class='lyricbox' >((?:.|\n)*?)<p><!--", RegexOptions.IgnoreCase);
        //    //    Regex reg = new Regex(@"<div class='lyricbox' >((?:.|\n)*?)<!--", RegexOptions.IgnoreCase);

        //    //    MatchCollection matches = reg.Matches(page);

        //    //    if (matches.Count != 1 || matches[0].Groups.Count != 2)
        //    //    {
        //    //        // hmmm they have changed the page structure
        //    //        return "Lyrics could not be found/retrieved";
        //    //    }

        //    //    return NormaliseContent(matches[0].Groups[1].Value);
        //    //}
        //    //catch (WebException)
        //    //{
        //    //    return "There has been an error";
        //    //}
        //    //catch (System.Security.SecurityException)
        //    //{
        //    //    return "The re has been an HTTP error connecting to the lyrics site";
        //    //}
        //    //catch
        //    //{
        //    //    return "There has been an error retrieving the lyrics";
        //    //}
        //}

        //private string NormaliseContent(string content)
        //{
        //    string norm = content.Replace("<br />", "\n");
        //    norm = norm.Replace("<p>", "\n\n");
        //    norm = norm.Replace("</p>", "\n\n");
        //    norm = norm.Replace("<b>", "");
        //    norm = norm.Replace("</b>", "");

        //    return norm;
        //}

        private void getAlbumArt(bool reload)
        {
            //if (currenttrack.Contains("&"))
            //{
            //    label2.Text = currenttrack.Replace("&", "&&");
            //}
            //else
            //{
            //    label2.Text = @currenttrack;
            //}

            //if (currentartist.Contains("&"))
            //{
            //    label3.Text = currentartist.Replace("&", "&&");
            //}
            //else
            //{
            //    label3.Text = @currentartist;
            //}

            label2.Text = @currenttrack.Replace("&", "&&");
            label3.Text = @currentartist.Replace("&", "&&");
            
            dets = string.Empty;

            pictureBox1.Image = METAbolt.Properties.Resources.not_found;
            pictureBox1.Refresh();
            pictureBox2.Visible = false;

            string method = "album.getinfo";   // "track.getinfo";
            string apiKey = "164c28853903f4ea97fe0104dbbcc0c2";
            string artist = currentartist;
            string track = currenttrack;
            string theArtWorkUrl = string.Empty;

            try
            {
                string baseUrl = "http://ws.audioscrobbler.com/2.0/?method=" + method + "&api_key=" + apiKey + "&artist=" + artist + "&album=" + track;

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                settings.IgnoreComments = true;

                int a = 0;

                albumlink = string.Empty;

                using (XmlReader reader = XmlReader.Create(baseUrl, settings))
                {
                    while ((reader.Read()))
                    {
                        if ((reader.NodeType == XmlNodeType.Element & "url" == reader.LocalName))
                        {
                            albumlink = reader.ReadElementString("url");
                        }

                        if ((reader.NodeType == XmlNodeType.Element & "image" == reader.LocalName))
                        {
                            if (a == 3)
                            {
                                theArtWorkUrl = reader.ReadElementString("image");
                                break;
                            }
                            else
                            {
                                a = a + 1;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(theArtWorkUrl))
                {
                    pictureBox1.Image = LoadPicture(theArtWorkUrl);
                    pictureBox1.Refresh();
                }
                else
                {
                    //album art not found
                    pictureBox1.Image = METAbolt.Properties.Resources.not_found;
                    pictureBox1.Refresh();
                }

                dets = GetBuyLink();

                if (!string.IsNullOrEmpty(dets))
                {
                    pictureBox2.Visible = true;
                    pictureBox2.Refresh();
                }
                else
                {
                    pictureBox2.Visible = false;
                    pictureBox2.Refresh();
                }

                if (!string.IsNullOrEmpty(albumlink))
                {
                    pictureBox3.Visible = true;
                    pictureBox3.Refresh();
                }
                else
                {
                    pictureBox3.Visible = false;
                    pictureBox3.Refresh();
                }
            }
            catch
            {
                ;
            }
        }

        private string GetBuyLink()
        {
            string apiKey = "164c28853903f4ea97fe0104dbbcc0c2";
            string artist = currentartist;
            string track = currenttrack;
            string theArtWorkUrl = string.Empty;

            try
            {
                string baseUrl = "http://ws.audioscrobbler.com/2.0/?method=track.getbuylinks&country=united%20states&api_key=" + apiKey + "&artist=" + artist + "&track=" + track;

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                settings.IgnoreComments = true;

                //int a = 0;

                string supplier = string.Empty;

                using (XmlReader reader = XmlReader.Create(baseUrl, settings))
                {
                    while ((reader.Read()))
                    {
                        if ((reader.NodeType == XmlNodeType.Element & "supplierName" == reader.LocalName))
                        {
                            supplier = reader.ReadElementString("supplierName");
                        }

                        if ((reader.NodeType == XmlNodeType.Element & "buyLink" == reader.LocalName))
                        {
                            if (supplier.ToLower() == "amazon mp3")
                            {
                                theArtWorkUrl = reader.ReadElementString("buyLink");
                                break;
                            }

                            //if (a == 2)
                            //{
                            //    theArtWorkUrl = reader.ReadElementString("buyLink");
                            //    break;
                            //}
                            //else
                            //{
                            //    a += 1;
                            //}
                        }
                    }
                }

                if (!string.IsNullOrEmpty(theArtWorkUrl))
                {
                    return theArtWorkUrl;
                }
                else
                {
                    //album art not found
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                string exp = ex.Message;
                return string.Empty;
            }
        }

        private Bitmap LoadPicture(string url)
        {
            HttpWebRequest wreq;
            HttpWebResponse wresp;
            Stream mystream;
            Bitmap bmp;

            bmp = null;
            mystream = null;
            wresp = null;
            try
            {
                wreq = (HttpWebRequest)WebRequest.Create(url);
                wreq.AllowWriteStreamBuffering = true;

                wresp = (HttpWebResponse)wreq.GetResponse();

                if ((mystream = wresp.GetResponseStream()) != null)
                    bmp = new Bitmap(mystream);
            }
            finally
            {
                if (mystream != null)
                    mystream.Close();

                if (wresp != null)
                    wresp.Close();
            }

            return (bmp);
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedIndex == 2)
            //{
            //    GetLyrics();
            //}
        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(dets))
            {
                System.Diagnostics.Process.Start(@dets);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(dets))
            {
                System.Diagnostics.Process.Start(@dets);
            }
        }

        private void frmPlayer_ResizeEnd(object sender, EventArgs e)
        {
            //listView1.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);    
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            //ListViewItem idx = listView1.SelectedItems[0];
            //string track = idx.Tag.ToString();
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ListViewItem idx = listView1.SelectedItems[0];
            string track = idx.Tag.ToString();

            if (!string.IsNullOrEmpty(track))
            {
                System.Diagnostics.Process.Start(@track);
            }
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            string mlink = e.LinkText.Replace("http//", string.Empty);

            if (!e.LinkText.StartsWith("http://"))
            {
                System.Diagnostics.Process.Start("http://" + mlink);
            }
            else
            {
                System.Diagnostics.Process.Start(mlink);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(albumlink))
            {
                System.Diagnostics.Process.Start(@albumlink);
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lyrics))
            {
                System.Diagnostics.Process.Start(@lyrics);
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            string ulink = "http://www.youtube.com/results?search_query=" + currentartist + ":" + currenttrack; ;

            System.Diagnostics.Process.Start(@ulink);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) return;

            axWindowsMediaPlayer1.URL = textBox1.Text.Trim();
        }

        private void frmPlayer_Resize(object sender, EventArgs e)
        {
            pictureBox1.Height = pictureBox1.Width;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Parcel parcel = this.instance.MainForm.parcel;

            parcel.MusicURL = textBox1.Text.Trim();
            parcel.Update(client.Network.CurrentSim, false);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                button2.Enabled = false;
            }
            else
            {
                button2.Enabled = true;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1) return;

            string sgenre = listBox2.SelectedItem.ToString();

            listBox3.Items.Clear();
            textBox1.Text = string.Empty;

            foreach (RadioObj rad in Stations)
            {
                if (rad.genre == sgenre)
                {
                    listBox3.Items.Add(new ListItem() { Value = rad.radiourl, Text = rad.radioname });
                }
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex == -1) return;

            ListItem itm = (ListItem)listBox3.SelectedItem;

            textBox1.Text = itm.Value;
        }
    }

    public class RadioObj
    {
        public string radioname { get; set; }
        public string genre { get; set; }
        public string radiourl { get; set; }
    }
}
