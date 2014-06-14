using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using METAxCommon;
using System.Diagnostics;
using OpenMetaverse;

namespace METAbolt
{
    public partial class frmPluginManager : Form
    {
        private METAboltInstance instance;
        private ConfigManager config;
        private bool pluginschanged = false;
        private string plugins = string.Empty;
        private bool ismbapiloaded = false;

        public frmPluginManager(METAboltInstance instance)
        {
            InitializeComponent();

            this.instance = instance;
            config = this.instance.Config;

            if (this.instance.EList != null)
            {
                foreach (IExtension extOn in this.instance.EList)
                {
                    listBox1.Items.Add(extOn.Title);
                }
            }

            plugins = config.CurrentConfig.PluginsToLoad;

            if (!string.IsNullOrEmpty(plugins))
            {
                string[] lplugs = plugins.Split('|');

                foreach (string plug in lplugs)
                {
                    if (!string.IsNullOrEmpty(plug))
                    {
                        listBox2.Items.Add(plug);
                    }
                }
            }

            IExtension result = this.instance.EList.Find(
            delegate(IExtension fn)
            {
                return fn.Title == "MB_LSLAPI";
            }
            );

            if (result != null)
            {
                ismbapiloaded = true;
            }
            else
            {
                ismbapiloaded = false;
            }
        }

        private void frmPluginManager_Load(object sender, EventArgs e)
        {
            txtMavatar.Text = config.CurrentConfig.MasterAvatar.Trim();
            txtMObject.Text = config.CurrentConfig.MasterObject.Trim();

            checkBox12.Checked = config.CurrentConfig.EnforceLSLsecurity;
            chkLSL.Checked = config.CurrentConfig.DisplayLSLcommands;

            groupBox5.Enabled = ismbapiloaded;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select a plugin to add first");
                return;
            }

            listBox2.Items.Add(listBox1.SelectedItem);
            pluginschanged = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select a plugin to remove first");
                return;
            }

            listBox2.Items.Remove(listBox2.SelectedItem);
            pluginschanged = true;
        }

        private void SetPrefs()
        {
            plugins = string.Empty;

            foreach (string item in listBox2.Items)
            {
                plugins += item + "|";
            }

            if (pluginschanged)
            {
                if (plugins.EndsWith("|", StringComparison.CurrentCultureIgnoreCase))
                {
                    plugins = plugins.Substring(0, plugins.Length - 1);
                }

                config.CurrentConfig.PluginsToLoad = plugins;
            }

            config.CurrentConfig.MasterAvatar = txtMavatar.Text;
            config.CurrentConfig.MasterObject = txtMObject.Text;

            config.CurrentConfig.EnforceLSLsecurity = checkBox12.Checked;
            config.CurrentConfig.DisplayLSLcommands = chkLSL.Checked;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetPrefs();
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 0)
            {
                return;
            }

            string itm = listBox1.SelectedItem.ToString();

            if (this.instance.EList != null)
            {
                foreach (IExtension extOn in this.instance.EList)
                {
                    if (extOn.Title == itm)
                    {
                        label5.Text = "Author: " + extOn.Author + Environment.NewLine + "Version: " + extOn.Version + Environment.NewLine + "Description: " + extOn.Description;
                        break; 
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", METAbolt.DataFolder.GetDataFolder() + "\\Extensions\\");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txtMavatar.Text = UUID.Zero.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            txtMObject.Text = UUID.Zero.ToString();
        }
    }
}
