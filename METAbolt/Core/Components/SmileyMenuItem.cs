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
using System.Drawing;
using System.Windows.Forms;

namespace METAbolt
{

    /// <summary>
    /// Summary description for EmoticonMenuItem.
    /// </summary>
    public class EmoticonMenuItem : MenuItem
    {

        private const int ICON_WIDTH = 19;
        private const int ICON_HEIGHT = 19;
        private const int ICON_MARGIN = 4;
        private Color backgroundColor, selectionColor, selectionBorderColor;
        private Image image;
        public Image Image
        {
            get { return image; }
            set { image = value; }
        }

        public EmoticonMenuItem()
        {
            this.OwnerDraw = true;
            backgroundColor = SystemColors.ControlLightLight;
            selectionColor = Color.FromArgb(50, 0, 0, 150);
            selectionBorderColor = SystemColors.Highlight;
        }

        public EmoticonMenuItem(Image _image)
            : this()
        {
            image = _image;
        }

        protected override void OnMeasureItem(MeasureItemEventArgs _args)
        {
            _args.ItemWidth = ICON_WIDTH + ICON_MARGIN;
            _args.ItemHeight = ICON_HEIGHT + 2 * ICON_MARGIN;
        }

        protected override void OnDrawItem(DrawItemEventArgs _args)
        {
            Graphics _graphics = _args.Graphics;
            Rectangle _bounds = _args.Bounds;

            DrawBackground(_graphics, _bounds, ((_args.State & DrawItemState.Selected) != 0));
            _graphics.DrawImage(image, _bounds.X + ((_bounds.Width - ICON_WIDTH) / 2), _bounds.Y + ((_bounds.Height - ICON_HEIGHT) / 2));
        }

        private void DrawBackground(Graphics _graphics, Rectangle _bounds, bool _selected)
        {
            if (_selected)
            {
                _graphics.FillRectangle(new SolidBrush(selectionColor), _bounds);
                _graphics.DrawRectangle(new Pen(selectionBorderColor), _bounds.X, _bounds.Y,
                    _bounds.Width - 1, _bounds.Height - 1);
            }
            else
            {
                _graphics.FillRectangle(new SolidBrush(backgroundColor), _bounds);
            }
        }
    }
}
