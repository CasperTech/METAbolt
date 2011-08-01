//  Copyright (c) 2008 - 2011, www.metabolt.net (METAbolt)
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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace METAbolt
{
    public partial class DifuseForm : Form
    {
        //private IContainer components = null;
        private Timer m_clock;
        private bool m_bShowing = true;
        private bool m_bForceClose = false;
        private DialogResult m_origDialogResult;
        private bool m_bDisposeAtEnd = false;

        #region Constructor
        public DifuseForm()
        {
            InitializeComponents();
        }

        public DifuseForm(bool disposeAtEnd)
        {
            m_bDisposeAtEnd = disposeAtEnd;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            components = new System.ComponentModel.Container();
            this.m_clock = new Timer(components);
            this.m_clock.Interval = 1000;
            this.SuspendLayout();

            this.m_clock.Tick += new EventHandler(Animate);

            this.Load += new EventHandler(DifuseForm_Load);
            this.Closing += new CancelEventHandler(DifuseForm_Closing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        #region Event handlers
        private void DifuseForm_Load(object sender, EventArgs e)
        {
            this.Opacity = 0.0;
            m_bShowing = true;

            m_clock.Start();
        }

        private void DifuseForm_Closing(object sender, CancelEventArgs e)
        {
            if (!m_bForceClose)
            {
                m_origDialogResult = this.DialogResult;
                e.Cancel = true;
                m_bShowing = false;
                m_clock.Start();
            }
            else
            {
                this.DialogResult = m_origDialogResult;
            }
        }

        #endregion

        #region Private methods
        private void Animate(object sender, EventArgs e)
        {
            if (m_bShowing)
            {
                if (this.Opacity < 1)
                {
                    this.Opacity += 0.1;
                }
                else
                {
                    m_clock.Stop();
                }
            }
            else
            {
                if (this.Opacity > 0)
                {
                    this.Opacity -= 0.1;
                }
                else
                {
                    m_clock.Stop();
                    m_bForceClose = true;
                    this.Close();
                    if (m_bDisposeAtEnd)
                        this.Dispose();
                }
            }
        }

        #endregion

        private void DifuseForm_Load_1(object sender, EventArgs e)
        {

        }

    }
}