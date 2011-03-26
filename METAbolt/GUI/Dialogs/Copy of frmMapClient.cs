using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using OpenMetaverse;

namespace METAbolt
{
    public partial class frmMapClient : Form
    {
        private METAboltInstance instance;
        private GridClient client; // = new GridClient();
        private Dictionary<Interface, TabPage> Interfaces = new Dictionary<Interface, TabPage>();

        public frmMapClient(METAboltInstance instance)
        {
            //Client.Network.OnLogin += new NetworkManager.LoginCallback(Network_OnLogin);

            InitializeComponent();

            this.instance = instance;
            //netcom = this.instance.Netcom;
            client = this.instance.Client;

            client.Settings.MULTIPLE_SIMS = false;

            //this.instance.MainForm.Load += new EventHandler(MainForm_Load);

            //ApplyConfig(this.instance.Config.CurrentConfig);
            //this.instance.Config.ConfigApplied += new EventHandler<ConfigAppliedEventArgs>(Config_ConfigApplied);

            RegisterAllPlugins(Assembly.GetExecutingAssembly());
            EnablePlugins(true);
        }

        private void EnablePlugins(bool enable)
        {
            tabControl.TabPages.Clear();
            //tabControl.TabPages.Add(tabLogin);

            if (enable)
            {
                lock (Interfaces)
                {
                    foreach (TabPage page in Interfaces.Values)
                    {
                        tabControl.TabPages.Add(page);
                    }
                }
            }
        }

        private void RegisterAllPlugins(Assembly assembly)
        {
            foreach (Type t in assembly.GetTypes())
            {
                try
                {
                    if (t.IsSubclassOf(typeof(Interface)))
                    {
                        ConstructorInfo[] infos = t.GetConstructors();
                        Interface iface = (Interface)infos[0].Invoke(new object[] { instance,this });
                        RegisterPlugin(iface);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void RegisterPlugin(Interface iface)
        {
            TabPage page = new TabPage();
            tabControl.TabPages.Add(page);

            iface.Client = client;
            iface.TabPage = page;

            if (!Interfaces.ContainsKey(iface))
            {
                lock (Interfaces) Interfaces.Add(iface, page);
            }

            iface.Initialize();

            page.Text = iface.Name;
            page.ToolTipText = iface.Description;
        }

        private void frmMapClient_Paint(object sender, PaintEventArgs e)
        {
            lock (Interfaces)
            {
                foreach (Interface iface in Interfaces.Keys)
                {
                    iface.Paint(sender, e);
                }
            }
        }

        private void tabLogin_Click(object sender, EventArgs e)
        {

        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();  
        }

        private void frmMapClient_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
