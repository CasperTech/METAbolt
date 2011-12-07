using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PopupControl;
using OpenMetaverse;
using System.IO; 

namespace METAbolt
{
    public partial class PrefSpelling : System.Windows.Forms.UserControl, IPreferencePane
    {
        private string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt\\Spelling\\";

        public PrefSpelling(METAboltInstance instance)
        {
            InitializeComponent();

            GetDictionaries();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        #region IPreferencePane Members

        string IPreferencePane.Name
        {
            get { return "  Spelling"; }
        }

        Image IPreferencePane.Icon
        {
            get { return Properties.Resources.spell_checker; }
        }

        void IPreferencePane.SetPreferences()
        {
            //instance.Config.CurrentConfig.DisableMipmaps = chkAI.Checked;
        }

        #endregion

        private void GetDictionaries()
        {

            DirectoryInfo di = new DirectoryInfo(dir);
            FileInfo[] files = di.GetFiles();

            listBox1.Items.Clear();

            foreach (FileInfo fi in di.GetFiles())
            {
                string inFile = fi.FullName;
                string finname = fi.Name;

                if (fi.Extension == ".dic")
                {
                    listBox1.Items.Add(finname);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
             groupBox1.Enabled = checkBox1.Checked;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = (listBox1.SelectedIndex != -1);
        }
    }
}
