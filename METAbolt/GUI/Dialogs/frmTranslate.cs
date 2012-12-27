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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
//using GoogleTranslationUtils;
//using MB_Translation_Utils;

namespace METAbolt
{
    public partial class frmTranslate : Form
    {
        private METAboltInstance instance;

        public frmTranslate(METAboltInstance instance)
        {
            InitializeComponent();

            this.instance = instance;
            AddLanguages();
        }

        private void AddLanguages()
        {
            cboLanguage.Items.Clear();
            //cboLanguage.Items.Add("Select...");
            cboLanguage.Items.Add(new ComboEx.ICItem("Select...", -1));

            cboLanguage.Items.Add(new ComboEx.ICItem("English/Arabic en|ar", 1));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Chineese(simp) en|zh-CHS", 2));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Chineese(trad) en|zh-CHT", 3));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Croatian en|hr", 4));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Czech en|cs", 5));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Danish en|da", 6));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Dutch en|nl", 7));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Filipino en|tl", 9));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Finnish en|fi", 10));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/French en|fr", 11));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/German en|de", 12));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Greek en|el", 13));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Hebrew en|iw", 14));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Hindi en|hi", 15));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Hungarian en|hu", 16));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Indonesian en|id", 17));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Italian en|it", 18));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Japanese en|ja", 19));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Korean en|ko", 20));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Lithuanian en|lt", 21));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Norwegian en|no", 22));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Polish en|pl", 23));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Portuguese en|p", 24));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Romanian en|ro", 25));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Russian en|ru", 26));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Slovenian en|sl", 27));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Spanish en|es", 28));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Swedish en|sv", 29));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Thai en|th", 30));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Turkish en|tr", 31));
            cboLanguage.Items.Add(new ComboEx.ICItem("English/Ukrainian en|uk", 32));

            cboLanguage.Items.Add("Arabic/English ar|en");
            cboLanguage.Items.Add("Chineese(simp)/English zh-CHS|en");
            cboLanguage.Items.Add("Chineese(trad)/English zh-CHT|en");
            cboLanguage.Items.Add("Croatian/English hr|en");
            cboLanguage.Items.Add("Czech/English cs|en");
            cboLanguage.Items.Add("Danish/English da|en");
            cboLanguage.Items.Add("Dutch/English nl|en");
            cboLanguage.Items.Add("Finnish/English fi|en");
            cboLanguage.Items.Add("Filipino/English tl|en");
            cboLanguage.Items.Add("French/English fr|en");
            cboLanguage.Items.Add("German/English de|en");
            cboLanguage.Items.Add("Greek/English el|en");
            cboLanguage.Items.Add("Hebrew/English iw|en");
            cboLanguage.Items.Add("Hindi/English hi|en");
            cboLanguage.Items.Add("Hungarian/English hu|en");
            cboLanguage.Items.Add("Indonesian/English id|en");
            cboLanguage.Items.Add("Italian/English it|en");
            cboLanguage.Items.Add("Japanese/English ja|en");
            cboLanguage.Items.Add("Korean/English ko|en");
            cboLanguage.Items.Add("Lithuanian/English lt|en");
            cboLanguage.Items.Add("Norwegian/English no|en");
            cboLanguage.Items.Add("Polish/English pl|en");
            cboLanguage.Items.Add("Portuguese/English pt|en");
            cboLanguage.Items.Add("Russian/English ru|en");
            cboLanguage.Items.Add("Romanian/English ro|en");
            cboLanguage.Items.Add("Slovenian/English sl|en");
            cboLanguage.Items.Add("Spanish/English es|en");
            cboLanguage.Items.Add("Swedish/English sv|en");
            cboLanguage.Items.Add("Thai/English th|en");
            cboLanguage.Items.Add("Turkish/English tr|en");
            cboLanguage.Items.Add("Ukrainian/English uk|en");

            cboLanguage.Items.Add("German/French de|fr");
            cboLanguage.Items.Add("Spanish/French es|fr");
            cboLanguage.Items.Add("French/German fr|de");
            cboLanguage.Items.Add("French/Spanish fr|es");
            cboLanguage.SelectedIndex = 0;
        }

        private string GetTranslation(string sTrStr)
        {
            string sPair = GetLangPair(cboLanguage.Text);

            //GoogleTranslationUtils.Translate trans = new GoogleTranslationUtils.Translate(sTrStr, sPair);
            //return trans.Translation;

            //string sPair

            MB_Translation_Utils.Utils trans = new MB_Translation_Utils.Utils();

            string tres = trans.Translate(sTrStr, sPair);
            
            return tres;
        }

        private string GetLangPair(string sPair)
        {
            string[] inputArgs = sPair.Split(' ');

            return inputArgs[1].ToString();
        }

        private void frmTranslate_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cboLanguage.SelectedIndex == -1 || cboLanguage.SelectedIndex == 0) return; 

            txtTo.Text = GetTranslation(txtFrom.Text.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChatConsole console = new ChatConsole(instance);
            console._textBox = _textBox1;
            //console.Show();  
        }

        public string _textBox1
        {
            get { return txtTo.Text; }
        }

        private void cboLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtFrom_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
