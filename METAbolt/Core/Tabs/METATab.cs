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
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Media;

namespace METAbolt
{
    public partial class METAboltTab : IDisposable
    {
        private ToolStripButton button;
        private Control control;
        private Button defaultControlButton;
        private string name;
        private string label;
        private METAboltTab mergedTab;
        private Form owner;
        private string originalLabel;

        private bool allowMerge = true;
        private bool allowDetach = true;
        private bool allowClose = true;

        private bool partialHighlighted = false;
        private bool highlighted = false;
        private bool selected = false;
        private bool detached = false;
        private bool merged = false;
        private string selectedtab = string.Empty;
        private bool disposed = false;

        ~METAboltTab()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                mergedTab.Dispose();
            }

            // TODO: Call the appropriate methods to clean up unmanaged resources here

            // we're done
            disposed = true;
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        public METAboltTab(ToolStripButton button, Control control, string name, string label)
        {
            this.button = button;
            this.control = control;
            this.name = name;
            this.label = label;
        }

        public void Close()
        {
            if (!allowClose) return;

            if (button != null)
            {
                button.Dispose();
                button = null;
            }

            if (control != null)
            {
                control.Dispose();
                control = null;
            }

            OnTabClosed(EventArgs.Empty);
        }

        public void Select()
        {
            if (detached) return;

            control.Visible = true;
            control.BringToFront();

            if (!partialHighlighted) Unhighlight();
            button.Checked = true;
            selected = true;
            selectedtab = this.name;

            OnTabSelected(EventArgs.Empty);
        }

        public void Deselect()
        {
            if (detached) return;

            if (control != null) control.Visible = false;
            if (button != null) button.Checked = false;
            selected = false;

            OnTabDeselected(EventArgs.Empty);
        }

        public void PartialHighlight()
        {
            if (detached)
            {
                //do nothing?!
            }
            else
            {
                button.Image = null;
                button.ForeColor = Color.Blue;
            }

            partialHighlighted = true;
            OnTabPartiallyHighlighted(EventArgs.Empty);
        }

        public void Highlight()
        {
            if (selected) return;

            if (detached)
            {
                if (!owner.Focused)
                    FormFlash.Flash(owner);
            }
            else
            {
                button.Image = Properties.Resources.arrow_forward_16;
                button.ForeColor = Color.Red;
            }

            //if (this.instance.Config.CurrentConfig.PlaySound)
            //{
                //SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.notify);
                //simpleSound.Play();
            //}

            highlighted = true;
            OnTabHighlighted(EventArgs.Empty);
        }

        public void Unhighlight()
        {
            if (detached)
            {
                FormFlash.Unflash(owner);
            }
            else
            {
                button.Image = null;
                button.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
            }

            highlighted = partialHighlighted = false;
            OnTabUnhighlighted(EventArgs.Empty);
        }

        public void AttachTo(ToolStrip strip, Panel container)
        {
            if (!allowDetach) return;
            if (!detached) return;

            strip.Items.Add(button);
            container.Controls.Add(control);

            owner = null;
            detached = false;
            OnTabAttached(EventArgs.Empty);
        }

        public void Detach(METAboltInstance instance)
        {
            if (!allowDetach) return;
            if (detached) return;

            owner = new frmDetachedTab(instance, this);
            detached = true;
            OnTabDetached(EventArgs.Empty);            
        }

        public void MergeWith(METAboltTab tab)
        {
            if (!allowMerge) return;
            if (merged) return;

            SplitContainer container = new SplitContainer();
            container.Dock = DockStyle.Fill;
            container.BorderStyle = BorderStyle.Fixed3D;
            container.SplitterDistance = container.Width / 2;
            container.Panel1.Controls.Add(control);
            container.Panel2.Controls.Add(tab.Control);

            control.Visible = true;
            tab.Control.Visible = true;

            control = container;
            tab.Control = container;
            
            mergedTab = tab;
            tab.mergedTab = this;

            originalLabel = label;
            tab.originalLabel = tab.label;
            this.Label = label + "+" + tab.Label;
            
            merged = tab.merged = true;

            OnTabMerged(EventArgs.Empty);
        }

        public METAboltTab Split()
        {
            if (!allowMerge) return null;
            if (!merged) return null;

            METAboltTab returnTab = mergedTab;
            mergedTab = null;
            returnTab.mergedTab = null;

            SplitContainer container = (SplitContainer)control;
            control = container.Panel1.Controls[0];
            returnTab.Control = container.Panel2.Controls[0];
            merged = returnTab.merged = false;

            this.Label = originalLabel;
            OnTabSplit(EventArgs.Empty);

            return returnTab;
        }

        public ToolStripButton Button
        {
            get { return button; }
            set { button = value; }
        }

        public Control Control
        {
            get { return control; }
            set { control = value; }
        }

        public Button DefaultControlButton
        {
            get { return defaultControlButton; }
            set { defaultControlButton = value; }
        }

        public string Name
        {
            get { return name; }
        }

        public string Label
        {
            get { return label; }
            set { label = button.Text = value; }
        }

        public METAboltTab MergedTab
        {
            get { return mergedTab; }
        }

        public Form Owner
        {
            get { return owner; }
        }

        public bool AllowMerge
        {
            get { return allowMerge; }
            set { allowMerge = value; }
        }

        public bool AllowDetach
        {
            get { return allowDetach; }
            set { allowDetach = value; }
        }

        public bool AllowClose
        {
            get { return allowClose; }
            set { allowClose = value; }
        }

        public bool PartiallyHighlighted
        {
            get { return partialHighlighted; }
        }

        public bool Highlighted
        {
            get { return highlighted; }
        }

        public bool Selected
        {
            get { return selected; }
        }

        public bool Detached
        {
            get { return detached; }
        }

        public bool Merged
        {
            get { return merged; }
        }

        public string SelectedTab
        {
            get { return selectedtab ; }
            set { selectedtab = value; }
        }
    }
}
