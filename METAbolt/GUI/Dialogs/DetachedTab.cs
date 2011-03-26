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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
using SLNetworkComm;

namespace METAbolt
{
    public partial class frmDetachedTab : Form
    {
        private METAboltInstance instance;
        private METAboltTab tab;

        //For reattachment
        private ToolStrip strip;
        private Panel container;

        public frmDetachedTab(METAboltInstance instance, METAboltTab tab)
        {
            InitializeComponent();

            this.instance = instance;
            this.tab = tab;
            this.Controls.Add(tab.Control);
            tab.Control.BringToFront();

            AddTabEvents();
            this.Text = tab.Label + " (tab) - METAbolt";

            ApplyConfig(this.instance.Config.CurrentConfig);
            this.instance.Config.ConfigApplied += new EventHandler<ConfigAppliedEventArgs>(Config_ConfigApplied);
        }

        private void Config_ConfigApplied(object sender, ConfigAppliedEventArgs e)
        {
            ApplyConfig(e.AppliedConfig);
        }

        private void ApplyConfig(Config config)
        {
            if (config.InterfaceStyle == 0) //System
                tstMain.RenderMode = ToolStripRenderMode.System;
            else if (config.InterfaceStyle == 1) //Office 2003
                tstMain.RenderMode = ToolStripRenderMode.ManagerRenderMode;
        }

        private void AddTabEvents()
        {
            tab.TabPartiallyHighlighted += new EventHandler(tab_TabPartiallyHighlighted);
            tab.TabUnhighlighted += new EventHandler(tab_TabUnhighlighted);
        }

        private void tab_TabUnhighlighted(object sender, EventArgs e)
        {
            tlblTyping.Visible = false;
        }

        private void tab_TabPartiallyHighlighted(object sender, EventArgs e)
        {
            tlblTyping.Visible = true;
        }

        private void frmDetachedTab_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tab.Detached)
            {
                if (tab.AllowClose)
                    tab.Close();
                else
                    tab.AttachTo(strip, container);
            }
        }

        private void tbtnReattach_Click(object sender, EventArgs e)
        {
            tab.AttachTo(strip, container);
            this.Close();
        }

        public ToolStrip ReattachStrip
        {
            get { return strip; }
            set { strip = value; }
        }

        public Panel ReattachContainer
        {
            get { return container; }
            set { container = value; }
        }

        private void frmDetachedTab_Load(object sender, EventArgs e)
        {

        }
    }
}