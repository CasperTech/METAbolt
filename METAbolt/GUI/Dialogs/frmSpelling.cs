using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;
using NHunspell;
using System.Text.RegularExpressions;
using SLNetworkComm;

namespace METAbolt
{
    public partial class frmSpelling : Form
    {
        private METAboltInstance instance;
        private SLNetCom netcom;
        private Hunspell hunspell = new Hunspell();   //("en_us.aff", "en_us.dic");
        private string dir = METAbolt.DataFolder.GetDataFolder() + "\\Spelling\\";
        //private string words = string.Empty;
        private int start = 0;
        private int indexOfSearchText = 0;
        private string[] swords;
        //private int swordind = -1;
        private string currentword = string.Empty;
        private List<string> mistakes = new List<string>();
        private ChatType ctype;
        private string afffile = string.Empty;
        private string dicfile = string.Empty;
        private string dic = string.Empty;
        private bool ischat = true;
        //private string tabname = string.Empty;
        private UUID target = UUID.Zero;
        private UUID session = UUID.Zero;
        private bool isgroup = false;

        public frmSpelling(METAboltInstance instance, string sentence, string[] swords, ChatType type)
        {
            InitializeComponent();

            this.instance = instance;

            afffile = this.instance.AffFile;   // "en_GB.aff";
            dicfile = this.instance.DictionaryFile;   // "en_GB.dic";

            string[] idic = dicfile.Split('.');
            dic = dir + idic[0];

            if (!System.IO.File.Exists(dic + ".csv"))
            {
                System.IO.File.Create(dic + ".csv");
            }

            try
            {
                hunspell.Load(dir + afffile, dir + dicfile);   //("en_us.aff", "en_us.dic");
                ReadWords();
            }
            catch
            {
                //string exp = ex.Message; 
            }

            //words = sentence;
            richTextBox1.Text = sentence;
            this.swords = swords;
            this.ctype = type;

            ischat = true;
        }

        public frmSpelling(METAboltInstance instance, string sentence, string[] swords, bool type, UUID target, UUID session)
        {
            InitializeComponent();

            this.instance = instance;
            netcom = this.instance.Netcom;

            afffile = this.instance.AffFile;   // "en_GB.aff";
            dicfile = this.instance.DictionaryFile;   // "en_GB.dic";

            string[] idic = dicfile.Split('.');
            dic = dir + idic[0];

            if (!System.IO.File.Exists(dic + ".csv"))
            {
                System.IO.File.Create(dic + ".csv");
            }

            try
            {
                hunspell.Load(dir + afffile, dir + dicfile);   //("en_us.aff", "en_us.dic");
                ReadWords();
            }
            catch
            {
                //string exp = ex.Message;
            }

            //words = sentence;
            richTextBox1.Text = sentence;
            this.swords = swords;

            isgroup = type;
            ischat = false;
            this.target = target;
            this.session = session; 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //instance.TabConsole.chatConsole._textBox = richTextBox1.Text;

            this.Close(); 
        }

        private void frmSpelling_Load(object sender, EventArgs e)
        {
            CheckSpellings();
        }

        private void CheckSpellings()
        {
            mistakes.Clear();
            listBox1.Items.Clear();  
 
            foreach (string word in swords)
            {
                string cword = Regex.Replace(word, @"[^a-zA-Z0-9]", "");

                bool correct = hunspell.Spell(cword);

                if (!correct)
                {
                    InHighligtWord(cword);

                    mistakes.Add(cword);
                }
            }

            if (mistakes.Count > 0)
            {
                start = 0;
                indexOfSearchText = 0;

                currentword = mistakes[0];

                HighligtWord(currentword);

                List<string> suggestions = hunspell.Suggest(currentword);

                foreach (String entry in suggestions)
                {
                    listBox1.Items.Add(entry);
                }
            }
        }

        private void ContSearch()
        {
            listBox1.Items.Clear();
            button4.Enabled = false; 

            if (currentword.Contains(currentword))
            {
                mistakes.Remove(currentword);
            }

            if (mistakes.Count < 1)
            {
                //if (ischat)
                //{
                //    instance.TabConsole.chatConsole._textBox = richTextBox1.Text;
                //}
                currentword = string.Empty;

                this.Close();
            }
            else
            {
                currentword = mistakes[0];

                HighligtWord(mistakes[0]);

                List<string> suggestions = hunspell.Suggest(mistakes[0]);

                foreach (String entry in suggestions)
                {
                    listBox1.Items.Add(entry);
                }
            }
        }

        private void HighligtWord(string word)
        {
            int startindex = 0;

            startindex = FindText(word.Trim(), start, richTextBox1.Text.Length);

            if (startindex >= 0)
            {
                // Set the highlight color as red
                richTextBox1.SelectionColor = Color.Red;
                richTextBox1.SelectionBackColor = Color.Yellow;   
                // Find the end index. End Index = number of characters in textbox
                int endindex = word.Length;
                // Highlight the search string
                richTextBox1.Select(startindex, endindex);
                // mark the start position after the position of
                // last search string
                start = startindex + endindex;
            }
        }

        private void ReplaceWord(string word)
        {
            if (start >= 0)
            {
                int endindex = currentword.Length;

                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.SelectionBackColor = Color.White;

                richTextBox1.SelectionStart = start;
                richTextBox1.SelectionLength = endindex;

                richTextBox1.SelectedText = word;  

                endindex = word.Length;

                // last search string
                start = start + endindex;

                ContSearch();
            }
        }

        private void InHighligtWord(string word)
        {
            int startindex = 0;

            startindex = FindText(word.Trim(), start, richTextBox1.Text.Length);

            if (startindex >= 0)
            {
                // Set the highlight color as red
                richTextBox1.SelectionColor = Color.Red;
                //richTextBox1.SelectionBackColor = Color.Yellow;
                // Find the end index. End Index = number of characters in textbox
                int endindex = word.Length;
                // Highlight the search string
                richTextBox1.Select(startindex, endindex);
                // mark the start position after the position of
                // last search string
                start = startindex + endindex;
            }
        }

        public int FindText(string txtToSearch, int searchStart, int searchEnd)
        {
            // Unselect the previously searched string
            if (searchStart > 0 && searchEnd > 0 && indexOfSearchText >= 0)
            {
                //richTextBox1.Undo();
                richTextBox1.ForeColor = Color.Black;
                richTextBox1.SelectionBackColor = Color.White; 
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
                    // Find the position of search string in RichTextBox
                    indexOfSearchText = richTextBox1.Find(txtToSearch, searchStart, searchEnd, RichTextBoxFinds.None);
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

        private void button2_Click(object sender, EventArgs e)
        {
            //instance.TabConsole.chatConsole._textBox = richTextBox1.Text;

            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ContSearch();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentword))
            {
                hunspell.Add(currentword);
                AddWord(currentword);
            }

            ContSearch();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button4.Enabled = (listBox1.SelectedIndex != -1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                start = start - currentword.Length;
                ReplaceWord(listBox1.SelectedItem.ToString());

                listBox1.SelectedIndex = -1;
                button4.Enabled = false; 
            }
        }

        private void frmSpelling_FormClosing(object sender, FormClosingEventArgs e)
        {
            string message = richTextBox1.Text;
            string message1 = string.Empty;
            string message2 = string.Empty;

            if (message.Length == 0) return;

            if (ischat)
            {
                instance.TabConsole.chatConsole.SendChat(richTextBox1.Text, ctype);
            }
            else
            {
                if (!isgroup)
                {
                    //netcom.SendInstantMessage(richTextBox1.Text, target, session);
                    if (message.Length > 1023)
                    {
                        message1 = message.Substring(0, 1022);
                        netcom.SendInstantMessage(message1, target, session);

                        if (message.Length > 2046)
                        {
                            message2 = message.Substring(1023, 2045);
                            netcom.SendInstantMessage(message2, target, session);
                        }
                    }
                    else
                    {
                        netcom.SendInstantMessage(message, target, session); ;
                    }
                }
                else
                {
                    if (message.Length > 1023)
                    {
                        message1 = message.Substring(0, 1022);
                        netcom.SendInstantMessageGroup(message1, target, session);

                        if (message.Length > 2046)
                        {
                            message2 = message.Substring(1023, 2045);
                            netcom.SendInstantMessageGroup(message2, target, session);
                        }
                    }
                    else
                    {
                        netcom.SendInstantMessageGroup(message, target, session); ;
                    }
                }
            }
        }

        private void AddWord(string aword)
        {
            string dicfilea = dic + ".csv";

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(dicfilea, true))
            {
                file.WriteLine(aword + ",");
            }

            richTextBox1.Undo();
            richTextBox1.ForeColor = Color.Black;
            richTextBox1.SelectionBackColor = Color.White;

            instance.Config.ApplyCurrentConfig(); 

            CheckSpellings();
        }

        private void ReadWords()
        {
            using (CsvFileReader reader = new CsvFileReader(dic + ".csv"))
            {
                CsvRow row = new CsvRow();

                while (reader.ReadRow(row))
                {
                    foreach (string s in row)
                    {
                        hunspell.Add(s);
                    }
                }

                reader.Dispose();  
            }
        }
    }
}
