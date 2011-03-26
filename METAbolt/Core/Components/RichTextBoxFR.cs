//  Copyright (c) 2008 - 2010, www.metabolt.net
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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace METAbolt
{
    public partial class RichTextBoxFR : RichTextBox
    {
        public void FindAndReplace(string FindText, string ReplaceText)
        {
            this.Find(FindText);

            if (!(this.SelectionLength == 0))
            {
                this.SelectedText = ReplaceText;
            }
            else
            {
                MessageBox.Show("The following text was not found: " + FindText);
            }
        }


        public void FindAndReplace(string FindText, string ReplaceText, bool ReplaceAll, bool MatchCase, bool WholeWord)
        {
            switch (ReplaceAll)
            {
                case false:
                    if (MatchCase == true)
                    {
                        if (WholeWord == true)
                        {
                            this.Find(FindText, RichTextBoxFinds.MatchCase | RichTextBoxFinds.WholeWord);
                        }
                        else
                        {
                            this.Find(FindText, RichTextBoxFinds.MatchCase);
                        }
                    }
                    else
                    {
                        if (WholeWord == true)
                        {
                            this.Find(FindText, RichTextBoxFinds.WholeWord);
                        }
                        else
                        {
                            this.Find(FindText);
                        }
                    }

                    if (!(this.SelectionLength == 0))
                    {
                        this.SelectedText = ReplaceText;
                    }
                    else
                    {
                        MessageBox.Show("The following text was not found: " + FindText);
                    }

                    break;

                case true:
                    int i = 0;
                    for (i = 0; i <= this.TextLength; i++)
                    {
                        if (MatchCase == true)
                        {
                            if (WholeWord == true)
                            {
                                this.Find(FindText, RichTextBoxFinds.MatchCase | RichTextBoxFinds.WholeWord);
                            }
                            else
                            {
                                this.Find(FindText, RichTextBoxFinds.MatchCase);
                            }
                        }
                        else
                        {
                            if (WholeWord == true)
                            {
                                this.Find(FindText, RichTextBoxFinds.WholeWord);
                            }
                            else
                            {
                                this.Find(FindText);
                            }
                        }

                        if (!(this.SelectionLength == 0))
                        {
                            this.SelectedText = ReplaceText;
                        }
                        else
                        {
                            MessageBox.Show(i + " occurrence(s) replaced");
                        }
                    }

                    break;
            }
        }
    }
}