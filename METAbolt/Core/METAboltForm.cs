using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using OpenMetaverse;
using OpenMetaverse.StructuredData;
using System.Threading;
using SLNetworkComm;

namespace METAbolt
{
    public class METAboltForm: Form
    {
        protected METAboltInstance Instance { get { return instance; } }
        private METAboltInstance instance = null;

        protected GridClient client { get { return instance.Client; } }

        public METAboltForm()
            : base()
        {

        }

        public METAboltForm(METAboltInstance instance)
            : base()
        {
            this.instance = instance;
            instance.OnMETAboltFormCreated(this);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
        }
    }    
}
