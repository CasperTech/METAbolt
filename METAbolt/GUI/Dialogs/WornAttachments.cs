//  Copyright (c) 2008-2013, www.metabolt.net
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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SLNetworkComm;
using OpenMetaverse;
using System.Threading;
using System.Globalization;


namespace METAbolt
{
    public partial class WornAttachments : Form
    {
        private METAboltInstance instance;
        private GridClient client;
        private Avatar av = null;
        private Dictionary<uint, AttachmentsListItem> listItems = new Dictionary<uint, AttachmentsListItem>();
        //private Dictionary<uint, AttachmentsListItem> groupItems = new Dictionary<uint, AttachmentsListItem>();

        public WornAttachments(METAboltInstance instance, Avatar av)
        {
            InitializeComponent();

            Disposed += new EventHandler(WornAttachments_Disposed);

            this.instance = instance;
            client = this.instance.Client;
            this.av = av;

            client.Network.SimChanged += new EventHandler<SimChangedEventArgs>(SIM_OnSimChanged);
            //client.Self.TeleportProgress += new EventHandler<TeleportEventArgs>(Self_TeleportProgress);
        }

        private void SIM_OnSimChanged(object sender, SimChangedEventArgs e)
        {
            if (!this.IsHandleCreated) return;

            lock (listItems)
            {
                listItems.Clear();
            }

            BeginInvoke(new MethodInvoker(delegate()
            {
                pBar3.Visible = true;
                lbxPrims.Items.Clear();
                lbxPrimGroup.Items.Clear();

                ThreadPool.QueueUserWorkItem(delegate(object sync)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Thread.Sleep(5000);
                    ReLoadItems();
                    //GetAttachments();
                    Cursor.Current = Cursors.Default;
                });
            }));
        }

        //private void Self_TeleportProgress(object sender, TeleportEventArgs e)
        //{
        //    if (!this.IsHandleCreated) return;

        //    switch (e.Status)
        //    {
        //        case TeleportStatus.Start:
        //        case TeleportStatus.Progress:
        //        case TeleportStatus.Failed:
        //        case TeleportStatus.Cancelled:
        //            return;

        //        case TeleportStatus.Finished:
        //            ThreadPool.QueueUserWorkItem(delegate(object sync)
        //            {
        //                Cursor.Current = Cursors.WaitCursor;
        //                Thread.Sleep(6000);
        //                ReLoadItems();
        //                //GetAttachments();
        //                Cursor.Current = Cursors.Default;
        //            });

        //            return;
        //    }
        //}

        private void ReLoadItems()
        {
            try
            {
                Avatar sav = client.Network.CurrentSim.ObjectsAvatars.Find(delegate(Avatar fa)
                {
                    return fa.ID == av.ID;
                }
                );

                if (sav != null)
                {
                    List<Primitive> prims = client.Network.CurrentSim.ObjectsPrimitives.FindAll(
                            new Predicate<Primitive>(delegate(Primitive prim)
                            {
                                try
                                {
                                    return (prim.ParentID == sav.LocalID);
                                }
                                catch { return false; }
                            }));

                    this.BeginInvoke(new MethodInvoker(delegate()
                    {
                        lbxPrims.BeginUpdate();

                        foreach (Primitive prim in prims)
                        {
                            try
                            {
                                AttachmentsListItem item = new AttachmentsListItem(prim, client, lbxPrims);

                                if (!listItems.ContainsKey(prim.LocalID))
                                {
                                    listItems.Add(prim.LocalID, item);

                                    item.PropertiesReceived += new EventHandler(item_PropertiesReceived);
                                    item.RequestProperties();
                                }
                            }
                            catch
                            {
                                ;
                            }
                        }

                        lbxPrims.EndUpdate();
                        lbxPrims.Visible = true;

                        pBar3.Visible = false;
                    }));
                }
                else
                {
                    //this.Close();
                    this.Dispose();
                }
            }
            catch { ; }
        }

        private void WornAssets_Load(object sender, EventArgs e)
        {
            this.CenterToParent();

            GetAttachments();
        }

        private void GetAttachments()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    GetAttachments();
                }));

                return;
            }

            List<Primitive> prims = client.Network.CurrentSim.ObjectsPrimitives.FindAll(
                new Predicate<Primitive>(delegate(Primitive prim)
                {
                    try
                    {
                        return (prim.ParentID == av.LocalID);
                    }
                    catch { return false; }
                }));

            lbxPrims.BeginUpdate();
            lbxPrims.Items.Clear();

            foreach (Primitive prim in prims)
            {
                try
                {
                    AttachmentsListItem item = new AttachmentsListItem(prim, client, lbxPrims);

                    if (!listItems.ContainsKey(prim.LocalID))
                    {
                        listItems.Add(prim.LocalID, item);

                        item.PropertiesReceived += new EventHandler(item_PropertiesReceived);
                        item.RequestProperties();
                    }
                }
                catch
                {
                    ;
                }
            }

            lbxPrims.EndUpdate();
            lbxPrims.Visible = true;
        }

        private void lbxPrims_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index < 0) return;

            AttachmentsListItem itemToDraw = (AttachmentsListItem)lbxPrims.Items[e.Index];

            Brush textBrush = null;
            //float fsize = 12.0f;
            Font boldFont = new Font(e.Font, FontStyle.Bold);
            Font regularFont = new Font(e.Font, FontStyle.Regular);

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                textBrush = new SolidBrush(Color.FromKnownColor(KnownColor.HighlightText));
            }
            else
            {
                textBrush = new SolidBrush(Color.FromKnownColor(KnownColor.ControlText));
            }

            string name = string.Empty;
            string wornat = string.Empty;
            string stas = string.Empty;

            try
            {
                if (itemToDraw.Prim.Properties == null)
                //if (itemToDraw.Prim == null)
                {
                    name = "...";
                    wornat = "...";
                }
                else
                {
                    name = itemToDraw.Prim.Properties.Name;
                    wornat = "worn on: " + itemToDraw.Prim.PrimData.AttachmentPoint.ToString();

                    if ((itemToDraw.Prim.Flags & PrimFlags.Touch) == PrimFlags.Touch)
                    {
                        stas = " (Touch)";
                    }
                    else
                    {
                        stas = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                name = "...";
                wornat = "...";
                Logger.Log(ex.Message, Helpers.LogLevel.Debug, ex);
            }

            SizeF nameSize = e.Graphics.MeasureString(name, boldFont);
            float nameX = e.Bounds.Left + 4;
            float nameY = e.Bounds.Top + 2;

            e.Graphics.DrawString(name, boldFont, textBrush, nameX, nameY);
            e.Graphics.DrawString(wornat, regularFont, textBrush, nameX + nameSize.Width + 8, nameY);

            SizeF nameSize1 = e.Graphics.MeasureString(wornat, regularFont);

            e.Graphics.DrawString(stas, boldFont, textBrush, nameX + nameSize.Width + nameSize1.Width + 4, nameY);

            e.DrawFocusRectangle();

            boldFont.Dispose();
            regularFont.Dispose();
            textBrush.Dispose();
            boldFont = null;
            regularFont = null;
            textBrush = null;
        }

        private void item_PropertiesReceived(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                AttachmentsListItem item = (AttachmentsListItem)sender;

                if (listItems.ContainsKey(item.Prim.LocalID))
                {
                    lbxPrims.BeginUpdate();
                    lbxPrims.Items.Add(item);
                    lbxPrims.EndUpdate();

                    label1.Text = "Ttl: " + lbxPrims.Items.Count.ToString(CultureInfo.CurrentCulture) + " attachments";
                }
            }));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            //this.Dispose();
        }

        private void btnTouch_Click(object sender, EventArgs e)
        {
            try
            {
                int iGx = lbxPrimGroup.SelectedIndex;

                if (iGx == -1)
                {
                    int iDx = lbxPrims.SelectedIndex;

                    if (iDx != -1)
                    {
                        AttachmentsListItem item = (AttachmentsListItem)lbxPrims.Items[iDx];

                        if (item == null) return;

                        client.Self.Touch(item.Prim.LocalID);
                        label4.Text = "Touched " + item.Prim.Properties.Name;
                    }
                    else
                    {
                        MessageBox.Show("You must select an attachment first", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    AttachmentsListItem gitem = (AttachmentsListItem)lbxPrimGroup.Items[iGx];
                    client.Self.Touch(gitem.Prim.LocalID);
                    label4.Text = "Touched " + gitem.Prim.Properties.Name;
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Worn Attachments: " + ex.Message, Helpers.LogLevel.Error);
            }
        }

        private void lbxPrimGroup_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index < 0) return;

            AttachmentsListItem itemToDraw = (AttachmentsListItem)lbxPrimGroup.Items[e.Index];

            Brush textBrush = null;
            //float fsize = 12.0f;
            Font boldFont = new Font(e.Font, FontStyle.Bold);
            Font regularFont = new Font(e.Font, FontStyle.Regular);

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                textBrush = new SolidBrush(Color.FromKnownColor(KnownColor.HighlightText));
            }
            else
            {
                textBrush = new SolidBrush(Color.FromKnownColor(KnownColor.ControlText));
            }

            string name = string.Empty;
            string wornat = string.Empty;

            try
            {
                if (itemToDraw.Prim.Properties == null)
                {
                    name = "...";
                    wornat = string.Empty;
                }
                else
                {
                    name = itemToDraw.Prim.Properties.Name;

                    if ((itemToDraw.Prim.Flags & PrimFlags.Touch) == PrimFlags.Touch)
                    {
                        wornat = "(Touch)";
                    }
                    else
                    {
                        wornat = string.Empty;
                    }
                }
            }
            catch
            {
                name = "...";
                wornat = "...";
                //Logger.Log(ex.Message, Helpers.LogLevel.Debug, ex);
            }

            SizeF nameSize = e.Graphics.MeasureString(name, boldFont);
            float nameX = e.Bounds.Left + 4;
            float nameY = e.Bounds.Top + 2;

            e.Graphics.DrawString(name, boldFont, textBrush, nameX, nameY);
            e.Graphics.DrawString(wornat, regularFont, textBrush, nameX + nameSize.Width + 8, nameY);

            e.DrawFocusRectangle();

            boldFont.Dispose();
            regularFont.Dispose();
            textBrush.Dispose();
            boldFont = null;
            regularFont = null;
            textBrush = null;
        }

        private void lbxPrims_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtUUID.Text = string.Empty;
            txtPoint.Text = string.Empty;  
   
            label4.Text = string.Empty;

            lbxPrimGroup.BeginUpdate();

            int iDx = lbxPrims.SelectedIndex;
            lbxPrimGroup.Items.Clear();
            label2.Text = string.Empty;

            if (iDx < 0)
            {
                btnTouch.Enabled = false;
                button2.Enabled = false;
                return;
            }
            else
            {
                button2.Enabled = true;
            }

            AttachmentsListItem item = (AttachmentsListItem)lbxPrims.Items[iDx];

            if ((item.Prim.Flags & PrimFlags.Touch) == PrimFlags.Touch)
            {
                btnTouch.Enabled = true;
            }
            else
            {
                btnTouch.Enabled = false;
            }

            txtUUID.Text = item.Prim.ID.ToString();
            txtPoint.Text = item.Prim.PrimData.AttachmentPoint.ToString(); 

            List<Primitive> group = client.Network.CurrentSim.ObjectsPrimitives.FindAll(
                delegate(Primitive prim)
                {
                    return (prim.ParentID == item.Prim.LocalID);
                }
            );

            label5.Text = item.Prim.Text;

            foreach (Primitive gprim in group)
            {
                try
                {
                    AttachmentsListItem gitem = new AttachmentsListItem(gprim, client, lbxPrimGroup);

                    gitem.PropertiesReceived += new EventHandler(gitem_PropertiesReceived);
                    gitem.RequestProperties();
                }
                catch
                {
                    ;
                }
            }

            lbxPrimGroup.EndUpdate();
        }

        private void gitem_PropertiesReceived(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                AttachmentsListItem item = (AttachmentsListItem)sender;

                lbxPrimGroup.Items.Add(item);

                label2.Text = "Ttl: " + lbxPrimGroup.Items.Count.ToString(CultureInfo.CurrentCulture) + " linked objects";
            }));
        }

        private void lbxPrimGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            label4.Text = string.Empty;

            int iDx = lbxPrimGroup.SelectedIndex;

            if (iDx < 0)
            {
                btnTouch.Enabled = false;
                button2.Enabled = false;
                return;
            }
            else
            {
                button2.Enabled = true;
            }

            AttachmentsListItem item = (AttachmentsListItem)lbxPrimGroup.Items[iDx];

            if ((item.Prim.Flags & PrimFlags.Touch) == PrimFlags.Touch)
            {
                btnTouch.Enabled = true;
            }
            else
            {
                btnTouch.Enabled = false;
            }

            label5.Text = item.Prim.Text;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int iDx = lbxPrims.SelectedIndex;
            AttachmentsListItem item = (AttachmentsListItem)lbxPrims.Items[iDx];

            if (item == null) return;
            (new META3D(instance, item.Prim.LocalID, item.Prim)).Show();

            //using (META3D frm = new META3D(instance, item.Prim.LocalID, item.Prim))
            //{
            //    frm.Show(); 
            //}
        }

        private void WornAttachments_FormClosing(object sender, FormClosingEventArgs e)
        {
            //client.Network.SimChanged -= new EventHandler<SimChangedEventArgs>(SIM_OnSimChanged);
            //client.Self.TeleportProgress -= new EventHandler<TeleportEventArgs>(Self_TeleportProgress);

            listItems.Clear();
            lbxPrims.Items.Clear();
            lbxPrimGroup.Items.Clear();
        }

        void WornAttachments_Disposed(object sender, EventArgs e)
        {
            client.Network.SimChanged -= new EventHandler<SimChangedEventArgs>(SIM_OnSimChanged);
            //client.Self.TeleportProgress -= new EventHandler<TeleportEventArgs>(Self_TeleportProgress);

            //lock (listItems)
            //{
            //    listItems.Clear();
            //}

            //GC.Collect();
        }
    }
}
