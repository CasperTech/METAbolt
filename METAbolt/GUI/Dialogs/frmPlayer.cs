//  Copyright (c) 2008-2011, www.metabolt.net
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
using AWS_PAAPI;
using System.IO;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Threading;

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


        public frmPlayer(METAboltInstance instance)
        {
            InitializeComponent();

            this.instance = instance;
            client = this.instance.Client;

            Disposed += new EventHandler(Player_Disposed);

            client.Self.TeleportProgress += new EventHandler<TeleportEventArgs>(TP_Callback);
        }

        private void TP_Callback(object sender, TeleportEventArgs e)
        {
            if (e.Status == TeleportStatus.Finished)
            {
                BeginInvoke((MethodInvoker)delegate 
                {
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                    axWindowsMediaPlayer1.URL = this.instance.Config.CurrentConfig.pURL;
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                });
            }
        }

        private void frmPlayer_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            
            axWindowsMediaPlayer1.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(player_PlayStateChange);
            axWindowsMediaPlayer1.CurrentItemChange += new AxWMPLib._WMPOCXEvents_CurrentItemChangeEventHandler(player_CurrentItemChange);
            axWindowsMediaPlayer1.CurrentPlaylistChange += new AxWMPLib._WMPOCXEvents_CurrentPlaylistChangeEventHandler(player_CurrentPlaylistChange);
            axWindowsMediaPlayer1.MediaChange += new AxWMPLib._WMPOCXEvents_MediaChangeEventHandler(player_MediaChange);

            axWindowsMediaPlayer1.URL = this.instance.Config.CurrentConfig.pURL;
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            label2.Text = string.Empty;
            label3.Text = string.Empty;
        }

        private void Player_Disposed(object sender, EventArgs e)
        {
            client.Self.TeleportProgress -= new EventHandler<TeleportEventArgs>(TP_Callback);
            axWindowsMediaPlayer1.PlayStateChange -= new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(player_PlayStateChange);
            axWindowsMediaPlayer1.CurrentItemChange -= new AxWMPLib._WMPOCXEvents_CurrentItemChangeEventHandler(player_CurrentItemChange);
            axWindowsMediaPlayer1.CurrentPlaylistChange -= new AxWMPLib._WMPOCXEvents_CurrentPlaylistChangeEventHandler(player_CurrentPlaylistChange);
            axWindowsMediaPlayer1.MediaChange -= new AxWMPLib._WMPOCXEvents_MediaChangeEventHandler(player_MediaChange);
        }

        private void player_MediaChange(object sender, AxWMPLib._WMPOCXEvents_MediaChangeEvent e)
        {
            GetTrackInfo(axWindowsMediaPlayer1.currentMedia.name);
        }

        private void player_CurrentPlaylistChange(object sender, AxWMPLib._WMPOCXEvents_CurrentPlaylistChangeEvent e)
        {
            GetTrackInfo(axWindowsMediaPlayer1.currentMedia.name);
        }

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
                currentartist = words[0].Trim();
                currenttrack = words[1].Trim();

                if (currenttrack.Contains("("))
                {
                    // get them out
                    int pos = currenttrack.IndexOf("(", StringComparison.CurrentCulture);
                    currenttrack = currenttrack.Substring(0, pos).Trim();
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

                ListViewItem finditem = listView1.FindItemWithText(eval);


                if (finditem == null)
                {
                    try
                    {
                        getAlbumArt(false);
                    }
                    catch { ; }

                    ListViewItem list = new ListViewItem();

                    list.Text = timestamp.ToShortTimeString() + ": " + track;
                    list.Tag = dets;

                    if (!string.IsNullOrEmpty(dets))
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
                    }
                    break;

                case (int)WMPLib.WMPPlayState.wmppsStopped:
                    label1.Text = "";
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
            //LyricsResult result;
            //bool sexists = true;   // false;

            if (string.IsNullOrEmpty(currentartist) || string.IsNullOrEmpty(currenttrack))
                return;

            if (currentlyrics == currenttrack)
                return;

            //try
            //{
            //    sexists = wiki.checkSongExists(currentartist, currenttrack);
            //}
            //catch (Exception ex)
            //{
            //    string exp = ex.Message; 
            //}

            currentlyrics = currenttrack;

            currentartist = currentartist.Replace(" ", "_");
            currenttrack = currenttrack.Replace(" ", "_");

            //richTextBox1.Text = "Click for lyrics:\nhttp://lyrics.wikia.com/lyrics/" + currentartist.ToUpper() + ":" + currenttrack.ToUpper();
            richTextBox1.Text = "Click for lyrics:\nhttp://lyrics.wikia.com/Special:Search?titlesOnly=1&search=" + currentartist + ":" + currenttrack;

            //if (sexists)
            //{
            //    // get the lyrics and add to richtextbox
            //    //result = wiki.getSong(currentartist, currenttrack);
            //    string lyricurl = "http://lyrics.wikia.com/lyrics/" + currentartist + ":" + currenttrack;   // result.url;

            //    //richTextBox1.Text = "Lyrics from: www.lyricwiki.org\n\n" + RipURL(lyricurl);
            //    richTextBox1.Text = "Lyrics from: http://lyrics.wikia.com/\n\n" + RipURL(lyricurl);

            //    //Encoding iso8859 = Encoding.GetEncoding("ISO-8859-1");
            //    //richTextBox1.Text = Encoding.UTF8.GetString(iso8859.GetBytes(result.lyrics));

            //    ////getAlbumArt();
            //}
            //else
            //{
            //    richTextBox1.Text = "Lyrics not found for ~" + currenttrack + "~";
            //}
        }

        private string RipURL(string trURL)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                //Make the http request
                request = (HttpWebRequest)HttpWebRequest.Create(trURL);
                request.Timeout = 10000;
                request.ReadWriteTimeout = 15000;
                request.KeepAlive = false;
                response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();

                StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8);
                string page = readStream.ReadToEnd();

                // Hello LyricsWiki
                //Regex reg = new Regex(@"<div class='lyricbox' >((?:.|\n)*?)<p><!--", RegexOptions.IgnoreCase);
                Regex reg = new Regex(@"<div class='lyricbox' >((?:.|\n)*?)<!--", RegexOptions.IgnoreCase);

                MatchCollection matches = reg.Matches(page);

                if (matches.Count != 1 || matches[0].Groups.Count != 2)
                {
                    // hmmm they have changed the page structure
                    return "Lyrics could not be found/retrieved";
                }

                return NormaliseContent(matches[0].Groups[1].Value);
            }
            catch (WebException)
            {
                return "There has been an error";
            }
            catch (System.Security.SecurityException)
            {
                return "The re has been an HTTP error connecting to the lyrics site";
            }
            catch
            {
                return "There has been an error retrieving the lyrics";
            }
        }

        private string NormaliseContent(string content)
        {
            string norm = content.Replace("<br />", "\n");
            norm = norm.Replace("<p>", "\n\n");
            norm = norm.Replace("</p>", "\n\n");
            norm = norm.Replace("<b>", "");
            norm = norm.Replace("</b>", "");

            return norm;
        }

        private void getAlbumArt(bool reload)
        {
            label2.Text = currenttrack;
            label3.Text = currentartist;
            dets = string.Empty;

            pictureBox1.Image = METAbolt.Properties.Resources.not_found;
            pictureBox1.Refresh();
            pictureBox2.Visible = false;
            //string sysculture = Thread.CurrentThread.CurrentCulture.ToString();   

            ConnectAWS serv = new ConnectAWS();

            AWS_PAAPI.ItemSearchResponse response = serv.ConnectUSA(reload, currentartist, currenttrack);

            if (response == null || response.Items[0].Item == null)
            {
                if (!reload)
                {
                    response = null;
                    serv = null;

                    serv = new ConnectAWS();

                    response = serv.ConnectUK(reload, currentartist, currenttrack, "uk");
                }
                else
                {
                    return;
                }
            }

            try
            {
                if (response != null && response.Items[0].Item != null)
                {
                    if (response.Items.Length > 0)
                    {
                        foreach (AWS_PAAPI.Items items in response.Items)
                        {
                            if (items == null)
                            {
                                if (!reload)
                                {
                                    getAlbumArt(true);
                                    return;
                                }
                                else
                                {
                                    return;
                                }
                            }

                            foreach (var item in items.Item)
                            {
                                if (item == null)
                                {
                                    if (!reload)
                                    {
                                        getAlbumArt(true);
                                        return;
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }

                                string iurl = string.Empty;

                                if (item.SmallImage != null)
                                {
                                    iurl = item.SmallImage.URL;
                                }

                                //string asin = item.ASIN;
                                dets = HttpUtility.HtmlDecode(item.DetailPageURL);
                                dets = HttpUtility.UrlDecode(dets);
                                dets = dets.Trim();
                                //AmazonProductAdvtApi.CustomerReviews revs = item.CustomerReviews;
                                //AmazonProductAdvtApi.EditorialReview[] edr = item.EditorialReviews;

                                //if (revs != null)
                                //{
                                //    decimal rating = revs.AverageRating;
                                //    label4.Text = "Rating: " + rating.ToString();
                                //}
                                //else
                                //{
                                //    label4.Text = "Rank: " + item.SalesRank;
                                //}

                                if (!string.IsNullOrEmpty(dets))
                                {
                                    //dets = dets.Replace(".com", ".co.uk");
                                    //dets = dets.Replace("&tag=ws", "&tag=" + amtag);
                                    pictureBox2.Visible = true;
                                }
                                else
                                {
                                    pictureBox2.Visible = false;
                                }

                                if (!string.IsNullOrEmpty(iurl))
                                {
                                    pictureBox1.Image = LoadPicture(item.SmallImage.URL);
                                    pictureBox1.Refresh();

                                    return;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (!reload)
                    {
                        // if we have arrived here the track albun wasn't found
                        // so display any album from the artist
                        // better than displaying the questions mark icon
                        getAlbumArt(true);
                        return;
                    }
                }
            }
            catch
            {
                ;
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
            if (tabControl1.SelectedIndex == 2)
            {
                GetLyrics();
            }
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
            ListViewItem idx = listView1.SelectedItems[0];
            string track = idx.Tag.ToString();

            if (!string.IsNullOrEmpty(track))
            {
                System.Diagnostics.Process.Start(@track);
            }
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            string mlink = e.LinkText.Replace("http//", string.Empty);

            if (!e.LinkText.StartsWith("http://", StringComparison.CurrentCulture))
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
    }
}
