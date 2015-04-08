// 
// METABolt Metaverse Client, forked from RADISHGHAST
// Copyright (c) 2015, METABolt Development Team
// Copyright (c) 2009-2014, RADISHGHAST Development Team
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//       this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name "METAbolt", nor "RADISHGHAST", nor the names of its
//       contributors may be used to endorse or promote products derived from
//       this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
// $Id$
//
using System;
using System.Drawing;
using System.Windows.Forms;
using log4net.Core;

namespace METAbolt
{
    public partial class DebugConsole : METAboltTabControl
    {
        public DebugConsole()
            : this(null)
        {
        }

        public DebugConsole(METAboltInstance instance)
            :base(instance)
        {
            InitializeComponent();
            Disposed += new EventHandler(DebugConsole_Disposed);
            METAboltAppender.Log += new EventHandler<LogEventArgs>(METAboltAppender_Log);
        }

        void DebugConsole_Disposed(object sender, EventArgs e)
        {
            METAboltAppender.Log -= new EventHandler<LogEventArgs>(METAboltAppender_Log);
        }

        void METAboltAppender_Log(object sender, LogEventArgs e)
        {
            if (!IsHandleCreated) return;

            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => METAboltAppender_Log(sender, e)));
                return;
            }

            rtbLog.SelectionColor = Color.FromKnownColor(KnownColor.WindowText);
            rtbLog.AppendText(string.Format("{0} [", e.LogEntry.TimeStamp.ToString("HH:mm:ss")));

            if (e.LogEntry.Level == Level.Error)
            {
                rtbLog.SelectionColor = Color.Red;
            }
            else if (e.LogEntry.Level == Level.Warn)
            {
                rtbLog.SelectionColor = Color.Yellow;
            }
            else if (e.LogEntry.Level == Level.Info)
            {
                rtbLog.SelectionColor = Color.Green;
            }
            else
            {
                rtbLog.SelectionColor = Color.Gray;
            }

            rtbLog.AppendText(e.LogEntry.Level.Name);
            rtbLog.SelectionColor = Color.FromKnownColor(KnownColor.WindowText);
            rtbLog.AppendText(string.Format("]: - {0}{1}", e.LogEntry.MessageObject.ToString(), Environment.NewLine));
        }

        private void rtbLog_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            instance.MainForm.ProcessLink(e.LinkText);
        }

    }
}
