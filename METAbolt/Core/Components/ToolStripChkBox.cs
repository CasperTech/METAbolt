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

// Some of this code and concept
// was borrowed from: http://www.xtremedotnettalk.com/showthread.php?t=100015
// and http://www.someuser77.com/guides/adding-controls-to-toolstrip/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace METAbolt
{
    public class WToolStripControlHost : ToolStripControlHost
    {
        public WToolStripControlHost()
            : base(new Control())
        {

        }

        public WToolStripControlHost(Control c)
            : base(c)
        {
        }
    }

    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public partial class ToolStripCheckBox : WToolStripControlHost
    {
        public ToolStripCheckBox()
            : base(new CheckBox())
        {
            ToolStripCheckBoxControl.MouseHover += new EventHandler(chk_MouseHover);
        }

        public CheckBox ToolStripCheckBoxControl
        {
            get { return Control as CheckBox; }
        }

        void chk_MouseHover(object sender, EventArgs e)
        {
            this.OnMouseHover(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            ToolStripCheckBoxControl.Text = this.Text;
        }

        //expose checkbox.enabled as property
        public bool ToolStripCheckBoxEnabled
        {
            get { return ToolStripCheckBoxControl.Enabled; }
            set { ToolStripCheckBoxControl.Enabled = value; }
        }

        public bool Checked
        {
            get { return ToolStripCheckBoxControl.Checked; }
            set { ToolStripCheckBoxControl.Checked = value; }
        }

        protected override void OnSubscribeControlEvents(Control c)
        {
            // Call the base method so the basic events are unsubscribed.
            base.OnSubscribeControlEvents(c);

            CheckBox ToolStripCheckBoxControl = (CheckBox)c;
            // Add the event.
            ToolStripCheckBoxControl.CheckedChanged += new EventHandler(OnCheckChanged);
        }

        protected override void OnUnsubscribeControlEvents(Control c)
        {
            // Call the base method so the basic events are unsubscribed.
            base.OnUnsubscribeControlEvents(c);


            CheckBox ToolStripCheckBoxControl = (CheckBox)c;
            // Remove the event.
            ToolStripCheckBoxControl.CheckedChanged -= new EventHandler(OnCheckChanged);
        }

        public event EventHandler CheckedChanged;

        private void OnCheckChanged(object sender, EventArgs e)
        {
            if (CheckedChanged != null)
            {
                CheckedChanged(this, e);
            }
        }
    }
}