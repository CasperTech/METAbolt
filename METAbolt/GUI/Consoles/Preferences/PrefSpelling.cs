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
        private METAboltInstance instance;
        private string dir = METAbolt.DataFolder.GetDataFolder() + "\\Spelling\\";
        private string lang = string.Empty;
        private Popup toolTip3;
        private CustomToolTip customToolTip;

        public PrefSpelling(METAboltInstance instance)
        {
            InitializeComponent();

            this.instance = instance; 

            GetDictionaries();

            string msg = "Enables spell checking in public chat and IMs.\n\nClick for online help";
            toolTip3 = new Popup(customToolTip = new CustomToolTip(instance, msg));
            toolTip3.AutoClose = false;
            toolTip3.FocusOnOpen = false;
            toolTip3.ShowingAnimation = toolTip3.HidingAnimation = PopupAnimations.Blend;

            checkBox1.Checked = instance.Config.CurrentConfig.EnableSpelling;
            lang = instance.Config.CurrentConfig.SpellLanguage;

            label2.Text = "Selected language: " + lang;

            listBox1.SelectedItem = lang + ".dic";

            SetFlag();

            //this.instance.DictionaryFile = lang + ".dic";
            //this.instance.AffFile = lang + ".aff";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lang = listBox1.Items[listBox1.SelectedIndex].ToString();

            //string[] sfile = lang.Split('.');
            //string file = lang = sfile[0];

            this.instance.DictionaryFile = lang + ".dic";
            this.instance.AffFile = lang + ".aff";

            label2.Text = "Selected language: " + lang;
            SetFlag();
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
            instance.Config.CurrentConfig.EnableSpelling = checkBox1.Checked;
            instance.Config.CurrentConfig.SpellLanguage = lang;
        }

        #endregion

        private void GetDictionaries()
        {

            DirectoryInfo di = new DirectoryInfo(dir);
            //FileInfo[] files = di.GetFiles();

            listBox1.Items.Clear();

            foreach (FileInfo fi in di.GetFiles())
            {
                //string inFile = fi.FullName;
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

            SetSelFlag();
        }

        private void SetFlag()
        {
            string[] sfile = lang.Split('_');

            picFlag.Image = ilFlags.Images[sfile[1] + ".png"];
        }

        private void SetSelFlag()
        {
            string sellang = listBox1.Items[listBox1.SelectedIndex].ToString();
            string[] sfile = sellang.Split('_');

            sfile = sfile[1].Split('.');

            picFlag.Image = ilFlags.Images[sfile[0] + ".png"];
        }

        private void picSpell_MouseHover(object sender, EventArgs e)
        {
            toolTip3.Show(picSpell);
        }

        private void picSpell_MouseLeave(object sender, EventArgs e)
        {
            toolTip3.Close();
        }

        private void picSpell_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://www.metabolt.net/metawiki/How-customise-and-enable-spell-checking.ashx");
        }
    }
}
