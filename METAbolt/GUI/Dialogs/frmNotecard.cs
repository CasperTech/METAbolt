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

namespace METAbolt
{
    public partial class frmNotecard : Form
    {
        private METAboltInstance instance;

        private string lheader = string.Empty;
        private string notecardContent = string.Empty;  

        int start = 0;
        int indexOfSearchText = 0;
        string prevsearchtxt = string.Empty;
        private bool nreadonly = false;
        private string searchfor = string.Empty;  

        public frmNotecard(METAboltInstance instance, string file, string searchfor)
        {
            InitializeComponent();

            this.instance = instance;            
            this.Text = file + " - METAbolt";

            rtbNotecard.LoadFile(file, RichTextBoxStreamType.PlainText);
            this.searchfor = searchfor; 
        }

        private void frmNotecardEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripDropDownButton2_Click(object sender, EventArgs e)
        {

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbNotecard.Copy(); 
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbNotecard.SelectAll(); 
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (tsChkCase.Checked)
            {
                if (tsChkWord.Checked)
                {
                    rtbNotecard.FindAndReplace(tsFindText.Text, tsReplaceText.Text, false, true, true);
                }
                else
                {
                    rtbNotecard.FindAndReplace(tsFindText.Text, tsReplaceText.Text, false, true, false);
                }
            }
            else
            {
                if (tsChkWord.Checked)
                {
                    rtbNotecard.FindAndReplace(tsFindText.Text, tsReplaceText.Text, false, false, true);
                }
                else
                {
                    rtbNotecard.FindAndReplace(tsFindText.Text, tsReplaceText.Text, false, false, false);
                }
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (tsChkCase.Checked)
            {
                if (tsChkWord.Checked)
                {
                    rtbNotecard.FindAndReplace(tsFindText.Text, tsReplaceText.Text, true, true, true);
                }
                else
                {
                    rtbNotecard.FindAndReplace(tsFindText.Text, tsReplaceText.Text, true, true, false);
                }
            }
            else
            {
                if (tsChkWord.Checked)
                {
                    rtbNotecard.FindAndReplace(tsFindText.Text, tsReplaceText.Text, true, false, true);
                }
                else
                {
                    rtbNotecard.FindAndReplace(tsFindText.Text, tsReplaceText.Text, true, false, false);
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Find();
        }

        private void Find()
        {
            // All this could go into the extended rtb component in the future

            int startindex = 0;

            if (prevsearchtxt != string.Empty)
            {
                if (prevsearchtxt != tsFindText.Text.Trim())
                {
                    startindex = 0;
                    start = 0;
                    indexOfSearchText = 0;
                }
            }

            prevsearchtxt = tsFindText.Text.Trim();

            //int linenumber = rtbScript.GetLineFromCharIndex(rtbScript.SelectionStart) + 1;
            //Point pnt = rtbScript.GetPositionFromCharIndex(rtbScript.SelectionStart);

            if (tsFindText.Text.Length > 0)
                startindex = FindNext(tsFindText.Text.Trim(), start, rtbNotecard.Text.Length);

            // If string was found in the RichTextBox, highlight it
            if (startindex > 0)
            {
                // Set the highlight color as red
                rtbNotecard.SelectionColor = Color.LightBlue;
                // Find the end index. End Index = number of characters in textbox
                int endindex = tsFindText.Text.Length;
                // Highlight the search string
                rtbNotecard.Select(startindex, endindex);
                // mark the start position after the position of 
                // last search string
                start = startindex + endindex;

                if (start == rtbNotecard.TextLength || start > rtbNotecard.TextLength)
                {
                    startindex = 0;
                    start = 0;
                    indexOfSearchText = 0;
                }
            }
            else if (startindex == -1)
            {
                startindex = 0;
                start = 0;
                indexOfSearchText = 0;
            }
        }

        public int FindNext(string txtToSearch, int searchStart, int searchEnd)
        {
            // Unselect the previously searched string
            if (searchStart > 0 && searchEnd > 0 && indexOfSearchText >= 0)
            {
                rtbNotecard.Undo();
            }

            // Set the return value to -1 by default.
            int retVal = -1;

            // A valid starting index should be specified.
            // if indexOfSearchText = -1, the end of search
            if (searchStart >= 0 && indexOfSearchText >= 0)
            {
                // A valid ending index 
                if (searchEnd > searchStart || searchEnd == -1)
                {
                    // Determine if it's a match case or what
                    RichTextBoxFinds mcase = RichTextBoxFinds.None;

                    if (tsChkCase.Checked)
                    {
                        mcase = RichTextBoxFinds.MatchCase;
                    }


                    if (tsChkWord.Checked)
                    {
                        mcase |= RichTextBoxFinds.WholeWord;
                    }

                    // Find the position of search string in RichTextBox
                    indexOfSearchText = rtbNotecard.Find(txtToSearch, searchStart, searchEnd, mcase);
                    // Determine whether the text was found in richTextBox1.
                    if (indexOfSearchText != -1)
                    {
                        // Return the index to the specified search text.
                        retVal = indexOfSearchText;
                    }
                }
            }

            return retVal;
        }

        private void GetCurrentLine()
        {
            int linenumber = rtbNotecard.GetLineFromCharIndex(rtbNotecard.SelectionStart) + 1;
            tsLn.Text = "Ln " + linenumber.ToString();
        }

        private void GetCurrentCol()
        {
            int colnumber = rtbNotecard.SelectionStart - rtbNotecard.GetFirstCharIndexOfCurrentLine() + 1;
            tsCol.Text = "Ln " + colnumber.ToString();
        }

        private void rtbNotecard_TextChanged(object sender, EventArgs e)
        {

        }

        private void rtbNotecard_SelectionChanged(object sender, EventArgs e)
        {
            GetCurrentLine();
            GetCurrentCol();
        }
       
        private void frmNotecardEditor_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            
            //rtbNotecard.ReadOnly = true;
            rtbNotecard.Focus(); 
            tsFindText.Text = searchfor;
            rtbNotecard.Focus();
            Find();
            rtbNotecard.Focus();
        }

        private void rtbNotecard_KeyDown(object sender, KeyEventArgs e)
        {
            if (nreadonly)
            {
                if ((e.Control) && (e.KeyCode == Keys.C))
                {
                    e.Handled = true;
                }
            }
        }

        private void tsSave_Click(object sender, EventArgs e)
        {

        }

        private void tsStatus_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}