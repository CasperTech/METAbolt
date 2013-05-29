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
using System.Drawing;
using System.Data;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using Khendys.Controls;
using System.Resources;
using System.Threading;
//using System.Runtime.InteropServices;
using System.Globalization;
using System.Net;
using System.IO;


namespace METAbolt
{
    public class RichTextBoxPrinter : ITextPrinter
    {
        private METAboltInstance instance;
        private ExRichTextBox rtb;
        private bool hideSmileys;
        private ConfigManager config;
        private string headerfont = "Tahoma";
        private string headerfontstyle = "Normal";
        private float headerfontsize = 8.5f;
        private FontStyle fontsy;
        private FontStyle fontst;
        private Color bkcolour = Color.Lavender;
        private string textfont = "Tahoma";
        private string textfontstyle = "Normal";
        private float textfontsize = 8.5f;
        //private int _findex = 0;
        //private Color bgcolour = Color.White;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr window, int message, int wparam, int lparam);
        const int WM_VSCROLL = 0x115;
        const int SB_BOTTOM = 7;

        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam);
        //const int WM_USER = 0x400;
        //const int EM_HIDESELECTION = WM_USER + 63;

        public RichTextBoxPrinter(METAboltInstance instance, ExRichTextBox textBox)
        {
            rtb = textBox;

            this.instance = instance;
            config = this.instance.Config;

            hideSmileys = config.CurrentConfig.ChatSmileys;
            config.ConfigApplied += new EventHandler<ConfigAppliedEventArgs>(Config_ConfigApplied);

            //rtb.BackColor = bgcolour = config.CurrentConfig.BgColour; 

            if (config.CurrentConfig.HeaderFont != null)
            {
                headerfont = config.CurrentConfig.HeaderFont;
                headerfontstyle = config.CurrentConfig.HeaderFontStyle;
                headerfontsize = config.CurrentConfig.HeaderFontSize;
                bkcolour = config.CurrentConfig.HeaderBackColour;
            }

            switch (headerfontstyle.ToLower(CultureInfo.CurrentCulture))
            {
                case "bold":
                    fontsy = FontStyle.Bold;
                    break;
                case "italic":
                    fontsy = FontStyle.Italic;
                    break;
                default:
                    fontsy = FontStyle.Regular;
                    break;
            }

            if (config.CurrentConfig.TextFont != null)
            {
                textfont = config.CurrentConfig.TextFont;
                textfontstyle = config.CurrentConfig.TextFontStyle;
                textfontsize = config.CurrentConfig.TextFontSize;
            }

            switch (textfontstyle.ToLower(CultureInfo.CurrentCulture))
            {
                case "bold":
                    fontst = FontStyle.Bold;
                    break;
                case "italic":
                    fontst = FontStyle.Italic;
                    break;
                default:
                    fontst = FontStyle.Regular;
                    break;
            }
        }

        private void Config_ConfigApplied(object sender, ConfigAppliedEventArgs e)
        {
            headerfont = config.CurrentConfig.HeaderFont;
            headerfontstyle = config.CurrentConfig.HeaderFontStyle;
            headerfontsize = config.CurrentConfig.HeaderFontSize;
            bkcolour = config.CurrentConfig.HeaderBackColour;

            textfont = config.CurrentConfig.TextFont;
            textfontstyle = config.CurrentConfig.TextFontStyle;
            textfontsize = config.CurrentConfig.TextFontSize;

            hideSmileys = e.AppliedConfig.ChatSmileys;
        }

        #region ITextPrinter Members

        public void ScrollToBottom()
        {
            SendMessage(rtb.Handle, WM_VSCROLL, SB_BOTTOM, 0);
        }

        void AppendNStext(string text)
        {
            //bool focused = rtb.Focused;
            ////backup initial selection
            int selection = rtb.SelectionStart;
            //int length = rtb.SelectionLength;
            ////allow autoscroll if selection is at end of text
            bool autoscroll = (selection == rtb.Text.Length);

            //if (!autoscroll)
            //{
            //    //shift focus from RichTextBox to some other control
            //    if (focused) rtb.Parent.Focus();
            //    //hide selection
            //    SendMessage(rtb.Handle, EM_HIDESELECTION, 1, 0);
            //}

            //if (focused)
            //{
            //    //rtb.HideSelection = true;
            //}
            //else
            //{
            //    //rtb.HideSelection = false;
            //}

            rtb.AppendText(text);

            if (autoscroll) ScrollToBottom();

            //if (!autoscroll)
            //{
            //    //restore initial selection
            //    rtb.SelectionStart = selection;
            //    rtb.SelectionLength = length;
            //    //unhide selection
            //    SendMessage(rtb.Handle, EM_HIDESELECTION, 0, 0);
            //    //restore focus to RichTextBox
            //    if (focused) rtb.Focus();
            //}
        }

        public void PrintHeader(string text)
        {
            if (this.rtb.InvokeRequired) this.rtb.BeginInvoke((MethodInvoker)delegate { PrintHeader(text); });
            else
            {
                if (text == null) return;

                rtb.SelectionFont = new Font(headerfont, headerfontsize, fontsy);

                rtb.SelectionBackColor = bkcolour;
                rtb.SelectionColor = Color.Black;

                AppendNStext(Environment.NewLine + " " + text);   // + Environment.NewLine);

                //CheckBufferSize();

                int cwidth = rtb.Width - (text.Length);
                string buff = string.Empty;

                for (int a = 0; a < cwidth; a++)
                {
                    buff += " ";
                }

                //int line = rtb.GetLineFromCharIndex(cpos);

                AppendNStext(buff + Environment.NewLine);

                CheckBufferSize();
            }
        }

        public void PrintLinkHeader(string text, string uuid, string link)
        {
            if (this.rtb.InvokeRequired) this.rtb.BeginInvoke((MethodInvoker)delegate { PrintLinkHeader(text, uuid, link); });
            else
            {
                if (text == null) return;

                AppendNStext(Environment.NewLine);

                //CheckBufferSize();

                //string[] str = text.Split(' ');
                //string url = "https://my-secondlife.s3.amazonaws.com/users/" + str[0].ToLower() + "." + str[1].ToLower() + "/sl_image.png?" + uuid.Replace("-", "");
                //Stream ImageStream = new WebClient().OpenRead(url);
                //Image img = Image.FromStream(ImageStream);

                //Bitmap bmp = new Bitmap(img, 25, 20);
                //bmp.Tag = uuid;

                //rtb.InsertImage((Image)bmp);

                rtb.SelectionFont = new Font(headerfont, headerfontsize, fontsy);
                rtb.SelectionBackColor = Color.White;
                rtb.SelectionBackColor = bkcolour;
                rtb.SelectionFont = new Font(rtb.SelectionFont, FontStyle.Bold);
                //rtb.InsertLink(" " + text, link);

                int cwidth = rtb.Width - (text.Length);
                string buff = string.Empty;

                for (int a = 0; a < cwidth; a++)
                {
                    buff += " ";
                }

                rtb.InsertLink(" " + text, link);

                rtb.SelectionBackColor = bkcolour;
                rtb.SelectionFont = new Font(rtb.SelectionFont, FontStyle.Bold);
                AppendNStext(buff);

                CheckBufferSize();
            }
        }

        public void PrintLinkHeader(string text, string link)
        {
            if (this.rtb.InvokeRequired) this.rtb.BeginInvoke((MethodInvoker)delegate { PrintLinkHeader(text, link); });
            else
            {
                if (text == null) return;

                rtb.SelectionFont = new Font(headerfont, headerfontsize, fontsy);

                rtb.SelectionBackColor = Color.White;
                AppendNStext(Environment.NewLine);

                //CheckBufferSize();

                rtb.SelectionBackColor = bkcolour;
                rtb.SelectionFont = new Font(rtb.SelectionFont, FontStyle.Bold);
                //rtb.InsertLink(" " + text, link);

                int cwidth = rtb.Width - (text.Length);
                string buff = string.Empty;

                for (int a = 0; a < cwidth; a++)
                {
                    buff += " ";
                }

                //rtb.SelectionBackColor = bkcolour;
                //rtb.AppendText(buff);
                //rtb.AppendText(Environment.NewLine + " " + text + buff);
                rtb.InsertLink(" " + text + buff, link);

                CheckBufferSize();
            }
        }

        public void PrintDate(string text)
        {
            if (this.rtb.InvokeRequired) this.rtb.BeginInvoke((MethodInvoker)delegate { PrintDate(text); });
            else
            {
                if (text == null) return;

                rtb.Select(rtb.TextLength, 0);

                rtb.SelectionColor = Color.DarkGray;
                ////rtb.SelectionCharOffset = 6;
                rtb.SelectionFont = new Font(rtb.SelectionFont.Name, textfontsize - 1, rtb.SelectionFont.Style);
                rtb.SelectionCharOffset = 10;
                AppendNStext(text);
                
                //rtb.AppendTextAsRtf(text, new Font(rtb.SelectionFont.Name, textfontsize - 2), RtfColor.Gray, RtfColor.White);
                rtb.SelectionFont = new Font(config.CurrentConfig.TextFont, config.CurrentConfig.TextFontSize, fontst);
                //rtb.SelectionFont = new Font(textfont, textfontsize, fontst);
                //rtb.SelectionColor = Color.Black;
                rtb.SelectionCharOffset = 0;
                //rtb.SelectionBackColor = rtb.BackColor = bgcolour;
            }
        }

        public void PrintLink(string text, string link)
        {
            if (this.rtb.InvokeRequired) this.rtb.BeginInvoke((MethodInvoker)delegate { PrintLink(text, link); });
            else
            {
                if (text == null) return;

                rtb.SelectionFont = new Font(textfont, textfontsize, fontst);

                //rtb.AppendText(text + Environment.NewLine);
                rtb.InsertLink(text, link);
                //rtb.SelectionBackColor = rtb.BackColor = bgcolour;
            }
        }

        public void PrintText(string text)
        {
            if (this.rtb.InvokeRequired) this.rtb.BeginInvoke((MethodInvoker)delegate { PrintText(text); });
            else
            {
                // bkcolour = config.CurrentConfig.HeaderBackColour;

                if (text == null) return;

                //rtb.SelectionFont = new Font(textfont, textfontsize, fontst);
                ////rtb.SelectionBackColor = rtb.BackColor = bgcolour;

                if (text.Contains("secondlife:///"))
                {
                    if (!text.Contains("http://secondlife:///"))
                    {
                        text = text.Replace("secondlife:///", "http://secondlife:///");
                    }
                }

                if (text.Contains("secondlife://"))
                {
                    if (!text.Contains("http://secondlife://"))
                    {
                        text = text.Replace("secondlife://", "http://secondlife:///");
                    }
                }

                //rtb.Text += Environment.NewLine;

                AppendNStext(text + Environment.NewLine);

                rtb.SelectionFont = new Font(rtb.SelectionFont.Name, textfontsize, rtb.SelectionFont.Style);

                //rtb.AppendTextAsRtf(text, new Font(textfont, textfontsize, fontst));

                CheckBufferSize();

                int _findex = rtb.Text.Length - text.Length; // To be SAFE this has to be done after 'append' like this due to the buffer or we will get index out of range error when trying to replace

                PutSmiley(_findex);
            }
        }

        public void PrintClassicTextDate(string text)
        {
            if (this.rtb.InvokeRequired) this.rtb.BeginInvoke((MethodInvoker)delegate { PrintClassicTextDate(text); });
            else
            {
                // bkcolour = config.CurrentConfig.HeaderBackColour;

                if (text == null) return;

                //rtb.SelectionFont = new Font(textfont, textfontsize, fontst);
                ////rtb.SelectionBackColor = rtb.BackColor = bgcolour;

                if (text.Contains("secondlife:///"))
                {
                    if (!text.Contains("http://secondlife:///"))
                    {
                        text = text.Replace("secondlife:///", "http://secondlife:///");
                    }
                }

                if (text.Contains("secondlife://"))
                {
                    if (!text.Contains("http://secondlife://"))
                    {
                        text = text.Replace("secondlife://", "http://secondlife:///");
                    }
                }

                //rtb.Text += Environment.NewLine;

                AppendNStext(text);

                rtb.SelectionFont = new Font(rtb.SelectionFont.Name, textfontsize, rtb.SelectionFont.Style);

                //rtb.AppendTextAsRtf(text, new Font(textfont, textfontsize, fontst));

                CheckBufferSize();

                int _findex = rtb.Text.Length - text.Length; // To be SAFE this has to be done after 'append' like this due to the buffer or we will get index out of range error when trying to replace

                PutSmiley(_findex);
            }
        }

        public void PrintTextLine(string text)
        {
            if (this.rtb.InvokeRequired) this.rtb.BeginInvoke((MethodInvoker)delegate { PrintTextLine(text); });
            else
            {
                // bkcolour = config.CurrentConfig.HeaderBackColour;

                if (text == null) return;

                //rtb.SelectionFont = new Font(textfont, textfontsize, fontst);
                //rtb.SelectionBackColor = rtb.BackColor = bgcolour;

                if (text.Contains("secondlife:///"))
                {
                    if (!text.Contains("http://secondlife:///"))
                    {
                        text = text.Replace("secondlife:///", "http://secondlife:///");
                    }
                }

                if (text.Contains("secondlife://"))
                {
                    if (!text.Contains("http://secondlife://"))
                    {
                        text = text.Replace("secondlife://", "http://secondlife:///");
                    }
                }

                //rtb.ReadOnly = false;
                //int index = rtb.SelectionStart;
                //int line = rtb.GetLineFromCharIndex(index);
                //int lines = rtb.Lines.Length;

                //if (line < lines)
                //{
                //    rtb.Text += text + Environment.NewLine;
                //}
                //else
                //{
                //    rtb.AppendText(text + Environment.NewLine);
                //}
                //rtb.ReadOnly = true;

                //rtb.Text += Environment.NewLine;

                //rtb.AppendTextAsRtf(text, new Font(textfont, textfontsize, fontst));

                AppendNStext(text + Environment.NewLine);

                rtb.SelectionFont = new Font(rtb.SelectionFont.Name, textfontsize, rtb.SelectionFont.Style);

                CheckBufferSize();

                int _findex = rtb.Text.Length - text.Length - Environment.NewLine.Length;

                PutSmiley(_findex);
            }
        }

        private void CheckBufferSize()
        {
            int lines = rtb.Lines.Length;   
            int maxlines = this.instance.Config.CurrentConfig.lineMax;

            if (maxlines == 0)
                return;

            if (lines > maxlines)
            {
                //rtb.Select(0, rtb.GetFirstCharIndexFromLine(1)); // select the first line
                //rtb.SelectionStart = 0;

                //rtb.SelectionLength = rtb.Text.IndexOf("\n", 0) + 1;
                //rtb.SelectedText = "";

                //rtb.SelectedText = "";

                //int start_index = rtb.GetFirstCharIndexFromLine(1);
                //int count = rtb.Lines[1].Length;

                //rtb.Text = rtb.Text.Remove(start_index, count);

                int cpos = rtb.SelectionStart; 
                rtb.ReadOnly = false;
                rtb.SelectionStart = 0;
                rtb.SelectionLength = rtb.GetFirstCharIndexFromLine(1);
                rtb.SelectedText = "";
                rtb.ReadOnly = true;
                rtb.SelectionStart = cpos;
            }
        }

        private void PutSmiley(int _findex)
        {
            if (this.rtb.InvokeRequired) this.rtb.BeginInvoke((MethodInvoker)delegate { PutSmiley(_findex); });
            else
            {
                if (hideSmileys)
                    return;

                try
                {
                    int _index;

                    // Angel smile
                    if ((_index = rtb.Find("angelsmile;", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "angelsmile;", Smileys.AngelSmile);
                    }
                    // Angry smile
                    if ((_index = rtb.Find("angry;", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "angry;", Smileys.AngrySmile);
                    }
                    // Beer
                    if ((_index = rtb.Find("beer;", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "beer;", Smileys.Beer);
                    }

                    if ((_index = rtb.Find("brokenheart;", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "brokenheart;", Smileys.BrokenHeart);
                    }

                    if ((_index = rtb.Find("bye", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "bye", Smileys.bye);
                    }

                    if ((_index = rtb.Find("clap;", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "clap;", Smileys.clap);
                    }

                    if ((_index = rtb.Find(":S", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, ":S", Smileys.ConfusedSmile);
                    }

                    if ((_index = rtb.Find("cry;", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "cry;", Smileys.CrySmile);
                    }

                    if ((_index = rtb.Find(";)", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, ";)", Smileys.wink);
                    }

                    if ((_index = rtb.Find("devil;", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "devil;", Smileys.DevilSmile);
                    }

                    if ((_index = rtb.Find("duh;", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "duh;", Smileys.duh);
                    }

                    if ((_index = rtb.Find("embarassed;", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "embarassed;", Smileys.EmbarassedSmile);
                    }

                    if ((_index = rtb.Find(":)", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, ":)", Smileys.happy0037);
                    }

                    if ((_index = rtb.Find("heart;", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "heart;", Smileys.heart);
                    }

                    if ((_index = rtb.Find("help", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "help", Smileys.help);
                    }

                    if ((_index = rtb.Find("liar;", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "liar;", Smileys.liar);
                    }

                    if ((_index = rtb.Find("lol", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "lol", Smileys.lol);
                    }

                    if ((_index = rtb.Find("cool;", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "cool;", Smileys.cool);
                    }

                    if ((_index = rtb.Find("oops", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "oops", Smileys.oops);
                    }

                    if ((_index = rtb.Find("shhh", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "shhh", Smileys.shhh);
                    }

                    if ((_index = rtb.Find("sigh", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "sigh", Smileys.sigh);
                    }

                    if ((_index = rtb.Find(":X", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, ":X", Smileys.silenced);
                    }

                    if ((_index = rtb.Find("thinking;", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "thinking;", Smileys.think);
                    }

                    if ((_index = rtb.Find("thumbsup;", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "thumbsup;", Smileys.ThumbsUp);
                    }

                    if ((_index = rtb.Find("whistle;", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "whistle;", Smileys.whistle);
                    }

                    if ((_index = rtb.Find("zzzzz", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "zzzzz", Smileys.zzzzz);
                    }

                    if ((_index = rtb.Find("wow", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "wow", Smileys.wow);
                    }

                    if ((_index = rtb.Find("muah;", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "muah;", Smileys.kiss);
                    }

                    if ((_index = rtb.Find(":(", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, ":(", Smileys.sad);
                    }

                    if ((_index = rtb.Find("yawn;", _findex, RichTextBoxFinds.NoHighlight)) > -1)
                    {
                        ReplaceWithSmiley(_index, "yawn;", Smileys.yawn);
                    }
                }
                catch { ; }
            }
        }

        private void ReplaceWithSmiley(int _index, string text, Bitmap smiley)
        {
            rtb.Select(_index, text.Length);
            rtb.InsertImage(smiley);

            int newindex = (_index + text.Length) - 1;

            try
            {
                if ((_index = rtb.Find(text, newindex, RichTextBoxFinds.NoHighlight)) > -1)
                {
                    ReplaceWithSmiley(_index, text, smiley);
                }
            }
            catch { ; }

            rtb.Select(rtb.Text.Length, 0);
        }

        public void ClearText()
        {
            if (this.rtb.InvokeRequired) this.rtb.BeginInvoke((MethodInvoker)delegate { ClearText(); });
            else
            {
                rtb.Clear();
            }
        }

        public int TLength()
        {
            return rtb.Size.Width;
        }

        public void SetSelectionForeColor(System.Drawing.Color color)
        {
            if (this.rtb.InvokeRequired) this.rtb.BeginInvoke((MethodInvoker)delegate { SetSelectionForeColor(color); });
            else
            {
                rtb.SelectionColor = color;
            }
        }

        public void SetSelectionBackColor(System.Drawing.Color color)
        {
            if (this.rtb.InvokeRequired) this.rtb.BeginInvoke((MethodInvoker)delegate { SetSelectionBackColor(color); });
            else
            {
                rtb.SelectionBackColor = color;
            }
        }

        public void SetFont(System.Drawing.Font font)
        {
            if (this.rtb.InvokeRequired) this.rtb.BeginInvoke((MethodInvoker)delegate { SetFont(font); });
            else
            {
                rtb.SelectionFont = font;
            }
        }

        public void SetFontStyle(System.Drawing.FontStyle fontstyle)
        {
            if (this.rtb.InvokeRequired) this.rtb.BeginInvoke((MethodInvoker)delegate { SetFontStyle(fontstyle); });
            else
            {
                rtb.SelectionFont = new Font(rtb.SelectionFont, fontstyle);
            }
        }

        public void SetFontSize(float size)
        {
            if (this.rtb.InvokeRequired) this.rtb.BeginInvoke((MethodInvoker)delegate { SetFontSize(size); });
            else
            {
                rtb.SelectionFont = new Font(rtb.SelectionFont.Name, size, rtb.SelectionFont.Style);
            }
        }

        public void SetOffset(int offset)
        {
            if (this.rtb.InvokeRequired) this.rtb.BeginInvoke((MethodInvoker)delegate { SetOffset(offset); });
            else
            {
                rtb.SelectionCharOffset = offset;
            }
        }

        #endregion
    }
}