//  Copyright (c) 2008 - 2014, www.metabolt.net (METAbolt)
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
using PopupControl;
using System.Globalization;


namespace METAbolt
{
    public partial class frmPreferences : Form
    {
        private METAboltInstance instance;
        private Dictionary<string, IPreferencePane> panes;
        private IPreferencePane selectedPane;
        private Popup toolTip;
        private CustomToolTip customToolTip;

        public frmPreferences(METAboltInstance instance)
        {
            InitializeComponent();

            string msg1 = "Click for help on how to use Application/Preferences";
            toolTip = new Popup(customToolTip = new CustomToolTip(instance, msg1));
            toolTip.AutoClose = false;
            toolTip.FocusOnOpen = false;
            toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;

            this.instance = instance;
            panes = new Dictionary<string, IPreferencePane>();
            //tcons = new PrefTextConsole(instance);  
            
            AddPreferencePane(new PrefGeneralConsole(instance));
            AddPreferencePane(new PrefTextConsole(instance));
            AddPreferencePane(new PrefAI(instance));
            //AddPreferencePane(new PrefTwitter(instance));
            AddPreferencePane(new PrefChairAnn(instance));
            AddPreferencePane(new PrefProxy(instance));
            AddPreferencePane(new PrefPlugin(instance));
            AddPreferencePane(new PrefMETAgiver(instance));
            AddPreferencePane(new Pref3D(instance));
            AddPreferencePane(new PrefSpelling(instance));
            lbxPanes.SelectedIndex = 0;
        }

        private void AddPreferencePane(IPreferencePane pane)
        {
            lbxPanes.Items.Add(new PreferencePaneListItem(pane.Name, pane.Icon));

            Control paneControl = (Control)pane;
            paneControl.Dock = DockStyle.Fill;
            paneControl.Visible = false;
            pnlPanes.Controls.Add(paneControl);

            panes.Add(pane.Name, pane);
        }

        private void SelectPreferencePane(string name)
        {
            IPreferencePane pane = panes[name];
            if (pane == selectedPane) return;
            
            Control paneControl = (Control)pane;
            Control selectedPaneControl = selectedPane as Control;

            paneControl.Visible = true;
            if (selectedPaneControl != null) selectedPaneControl.Visible = false;

            selectedPane = pane;
        }

        private void Apply()
        {
            foreach (KeyValuePair<string, IPreferencePane> kvp in panes)
                kvp.Value.SetPreferences();

            instance.Config.ApplyCurrentConfig();
        }

        private void lbxPanes_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index < 0) return;
            
            PreferencePaneListItem itemToDraw = (PreferencePaneListItem)lbxPanes.Items[e.Index];
            Brush textBrush = null;
            Font textFont = null;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                textBrush = new SolidBrush(Color.FromKnownColor(KnownColor.HighlightText));
                textFont = new Font(e.Font, FontStyle.Bold);
            }
            else
            {
                textBrush = new SolidBrush(Color.FromKnownColor(KnownColor.ControlText));
                textFont = new Font(e.Font, FontStyle.Regular);
            }
            
            SizeF stringSize = e.Graphics.MeasureString(itemToDraw.Name, textFont);
            float stringX = e.Bounds.Left + 4 + itemToDraw.Icon.Width;
            float stringY = e.Bounds.Top + 2 + ((itemToDraw.Icon.Height / 2) - (stringSize.Height / 2));

            e.Graphics.DrawImage(itemToDraw.Icon, e.Bounds.Left + 2, e.Bounds.Top + 2);
            e.Graphics.DrawString(itemToDraw.Name, textFont, textBrush, stringX, stringY);

            e.DrawFocusRectangle();

            textFont.Dispose();
            textBrush.Dispose();
            textFont = null;
            textBrush = null;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Apply();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Apply();
        }

        private void lbxPanes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxPanes.SelectedItem == null) return;
            PreferencePaneListItem item = (PreferencePaneListItem)lbxPanes.SelectedItem;

            // Screen size change to accomodate initial IM feature thx to Elmo Clarity 20/12/2010

            if (item.Name.ToLower(CultureInfo.CurrentCulture) == "text")
            {
                this.Height = 460;
            }
            else
            {
                this.Height = 390;
            }

            SelectPreferencePane(item.Name);
        }

        private void frmPreferences_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
        }

        private void pnlPanes_Paint(object sender, PaintEventArgs e)
        {

        }

        private void picAutoSit_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://www.metabolt.net/metawiki/Using-Preferences.ashx");
        }

        private void picAutoSit_MouseHover(object sender, EventArgs e)
        {
            toolTip.Show(picAutoSit);
        }

        private void picAutoSit_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Close();
        }
    }
}