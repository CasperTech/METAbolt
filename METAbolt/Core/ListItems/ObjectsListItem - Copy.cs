using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;

namespace METAbolt
{
    public class ObjectsListItem
    {
        private Primitive prim;
        private GridClient client;
        private ListBox listBox;
        private bool gotProperties = false;
        private bool gettingProperties = false;

        public ObjectsListItem(Primitive prim, GridClient client, ListBox listBox)
        {
            this.prim = prim;
            this.client = client;
            this.listBox = listBox;
        }

        public void RequestProperties()
        {
            try
            {
                //if (string.IsNullOrEmpty(prim.Properties.Name))
                //if (prim.Properties == null) // Rollback ).9.2.1
                if (prim.Properties == null)   // || string.IsNullOrEmpty(prim.Properties.Name)) //GM changed it to BOTH!
                {
                    gettingProperties = true;
                    client.Objects.OnObjectPropertiesFamily += new ObjectManager.ObjectPropertiesFamilyCallback(Objects_OnObjectPropertiesFamily);
                    client.Objects.RequestObjectPropertiesFamily(client.Network.CurrentSim, prim.ID);
                }
                else
                {
                    gotProperties = true;
                    OnPropertiesReceived(EventArgs.Empty);
                }
            }
            catch (Exception exp)
            {
                string sex = exp.Message;
            }
        }

        private void Objects_OnObjectPropertiesFamily(Simulator simulator, Primitive.ObjectProperties properties, ReportType type)
        {
            if (properties.ObjectID != prim.ID) return;

            try
            {
                gettingProperties = false;
                gotProperties = true;
                prim.Properties = properties;

                listBox.BeginInvoke(
                    new OnPropReceivedRaise(OnPropertiesReceived),
                    new object[] { EventArgs.Empty });
            }
            catch (Exception exp)
            {
                string sex = exp.Message;
            }
        }

        public override string ToString()
        {
            try
            {
                //return (string.IsNullOrEmpty(prim.Properties.Name) ? "..." : prim.Properties.Name);
                //return (prim.Properties == null ? "..." : prim.Properties.Name); // Rollback ).9.2.1
                //GM changed to BOTH!
                if (prim.Properties == null) return "???";
                return (string.IsNullOrEmpty(prim.Properties.Name) ? "..." : prim.Properties.Name);
            }
            catch (Exception ex)
            {
                string sexp = ex.Message.ToString();
                return "***";
            }
        }

        public event EventHandler PropertiesReceived;
        private delegate void OnPropReceivedRaise(EventArgs e);
        protected virtual void OnPropertiesReceived(EventArgs e)
        {
            if (PropertiesReceived != null) PropertiesReceived(this, e);
        }

        public Primitive Prim
        {
            get { return prim; }
        }

        public bool GotProperties
        {
            get { return gotProperties; }
        }

        public bool GettingProperties
        {
            get { return gettingProperties; }
        }
    }
}
