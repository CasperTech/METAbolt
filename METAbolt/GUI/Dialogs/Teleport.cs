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
using OpenMetaverse;
using System.Threading;
using OpenMetaverse.Packets;
using SLNetworkComm;
using System.Net;
using System.Diagnostics;
using ExceptionReporting;
using System.Globalization;


namespace METAbolt
{
    public partial class frmTeleport : Form
    {
        private METAboltInstance instance;
        private SLNetCom netcom;
        private GridClient client;
        //private const int GRID_Y_OFFSET = 1279;
        //Base URL for web map api sim images
        //private const String MAP_IMG_URL = "http://secondlife.com/apps/mapapi/grid/map_image/";
        //private METAxCommon.IEvents evs;
        private GridRegion selregion;
        //private int agencnt = 0;
        //private SafeDictionary<string, int> acnt = new SafeDictionary<string,int>();  

        private ExceptionReporter reporter = new ExceptionReporter();

        internal class ThreadExceptionHandler
        {
            public void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
            {
                ExceptionReporter reporter = new ExceptionReporter();
                reporter.Show(e.Exception);
            }
        }

        public frmTeleport(METAboltInstance instance, string sSIM, float sX,float sY,float sZ)
        {
            InitializeComponent();

            SetExceptionReporter();
            Application.ThreadException += new ThreadExceptionHandler().ApplicationThreadException;

            this.instance = instance;
            netcom = this.instance.Netcom;
            client = this.instance.Client;

            AddNetcomEvents();
            AddClientEvents();

            if (string.IsNullOrEmpty(sSIM))
            {
                SetDefaultValues();
            }
            else
            {
                decimal x = (decimal)sX;
                decimal y = (decimal)sY;
                decimal z = (decimal)sZ;

                txtSearchFor.Text = txtRegion.Text = sSIM;
                nudX.Value = x;
                nudY.Value = y;
                nudZ.Value = z;

                StartRegionSearch(); 
            }
        }

        private void SetExceptionReporter()
        {
            reporter.Config.ShowSysInfoTab = false;   // alternatively, set properties programmatically
            reporter.Config.ShowFlatButtons = true;   // this particular config is code-only
            reporter.Config.CompanyName = "METAbolt";
            reporter.Config.ContactEmail = "metabolt@vistalogic.co.uk";
            reporter.Config.EmailReportAddress = "metabolt@vistalogic.co.uk";
            reporter.Config.WebUrl = "http://www.metabolt.net/metaforums/";
            reporter.Config.AppName = "METAbolt";
            reporter.Config.MailMethod = ExceptionReporting.Core.ExceptionReportInfo.EmailMethod.SimpleMAPI;
            reporter.Config.BackgroundColor = Color.White;
            reporter.Config.ShowButtonIcons = false;
            reporter.Config.ShowLessMoreDetailButton = true;
            reporter.Config.TakeScreenshot = true;
            reporter.Config.ShowContactTab = true;
            reporter.Config.ShowExceptionsTab = true;
            reporter.Config.ShowFullDetail = true;
            reporter.Config.ShowGeneralTab = true;
            reporter.Config.ShowSysInfoTab = true;
            reporter.Config.TitleText = "METAbolt Exception Reporter";
        }

        private void AddNetcomEvents()
        {
            netcom.Teleporting += new EventHandler<TeleportingEventArgs>(netcom_Teleporting);
            netcom.TeleportStatusChanged += new EventHandler<TeleportEventArgs>(netcom_TeleportStatusChanged);
            netcom.ClientDisconnected += new EventHandler<DisconnectedEventArgs>(netcom_ClientDisconnected);
        }

        private void AddClientEvents()
        {
            client.Grid.GridRegion += new EventHandler<GridRegionEventArgs>(Grid_OnGridRegion);
        }

        private void RemoveClientEvents()
        {
            client.Grid.GridRegion -= new EventHandler<GridRegionEventArgs>(Grid_OnGridRegion);
            netcom.Teleporting -= new EventHandler<TeleportingEventArgs>(netcom_Teleporting);
            netcom.TeleportStatusChanged -= new EventHandler<TeleportEventArgs>(netcom_TeleportStatusChanged);
            netcom.ClientDisconnected -= new EventHandler<DisconnectedEventArgs>(netcom_ClientDisconnected);
        }

        //Separate thread
        private void Grid_OnGridRegion(object sender, GridRegionEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => Grid_OnGridRegion(sender, e)));
                return;
            }
            
            BeginInvoke(new MethodInvoker(delegate()
            {
                RegionSearchResult(e.Region);
            }));
        }

        //UI thread
        private void RegionSearchResult(GridRegion region)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => RegionSearchResult(region)));
                return;
            }

            RegionSearchResultItem item = new RegionSearchResultItem(instance, region, lbxRegionSearch);
            int index = lbxRegionSearch.Items.Add(item);
            item.ListIndex = index;
            selregion = item.Region;
        }

        private void SetDefaultValues()
        {
            string region = client.Network.CurrentSim.Name;
            decimal x = (decimal)instance.SIMsittingPos().X;
            decimal y = (decimal)instance.SIMsittingPos().Y;
            decimal z = (decimal)instance.SIMsittingPos().Z;

            if (x < 0) x = 0;
            if (x > 256) x = 256;
            if (y < 0) y = 0;
            if (y > 256) y = 256;

            txtRegion.Text = region;
            nudX.Value = x;
            nudY.Value = y;
            nudZ.Value = z;
        }

        private void netcom_TeleportStatusChanged(object sender, TeleportEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => netcom_TeleportStatusChanged(sender, e)));
                return;
            }

            try
            {
                switch (e.Status)
                {
                    case TeleportStatus.Start:
                        RefreshControls();
                        pnlTeleporting.Visible = true;
                        lblTeleportStatus.Visible = true;
                        break;
                    case TeleportStatus.Progress:
                        lblTeleportStatus.Text = e.Message;
                        break;

                    case TeleportStatus.Failed:
                        RefreshControls();
                        pnlTeleporting.Visible = false;
                        MessageBox.Show(e.Message, "Teleport", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;

                    case TeleportStatus.Finished:
                        RefreshControls();
                        pnlTeleporting.Visible = false;
                        //lblTeleportStatus.Visible = false;
                        this.Close();
                        break;
                }
            }
            catch { ; }
        }

        private void netcom_ClientDisconnected(object sender, DisconnectedEventArgs e)
        {
            this.Close();
        }

        private void netcom_Teleporting(object sender, TeleportingEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => netcom_Teleporting(sender, e)));
                return;
            }

            try
            {
                RefreshControls();
            }
            catch
            {
                ;
            }
        }

        private void RefreshControls()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    RefreshControls();
                    return;
                }));
            }

            try
            {
                if (netcom.IsTeleporting)
                {
                    pnlTeleportOptions.Enabled = false;
                    btnTeleport.Enabled = false;
                    pnlTeleporting.Visible = true;
                }
                else
                {
                    pnlTeleportOptions.Enabled = true;
                    btnTeleport.Enabled = true;
                    pnlTeleporting.Visible = false;
                }
            }
            catch
            {
                ;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtRegion_TextChanged(object sender, EventArgs e)
        {
            btnTeleport.Enabled = (txtRegion.Text.Trim().Length > 0);
        }

        private void btnTeleport_Click(object sender, EventArgs e)
        {
            if (instance.State.IsSitting)
            {
                client.Self.Stand();
                instance.State.SetStanding();
            }

            pnlTeleporting.Visible = true;

            if (selregion.RegionHandle == 0 && !string.IsNullOrEmpty(txtRegion.Text))
            {
                //RefreshControls();
                netcom.Teleport(txtRegion.Text.Trim(), new Vector3((float)nudX.Value, (float)nudY.Value, (float)nudZ.Value));
            }
            else
            {
                client.Self.RequestTeleport(selregion.RegionHandle, new Vector3((float)nudX.Value, (float)nudY.Value, (float)nudZ.Value));
            }
        }

        private void frmTeleport_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (netcom.IsTeleporting && netcom.IsLoggedIn)
                e.Cancel = true;
            else
                RemoveClientEvents();
        }

        private void txtSearchFor_TextChanged(object sender, EventArgs e)
        {
            btnFind.Enabled = (txtSearchFor.Text.Trim().Length > 0);
        }

        private void lbxRegionSearch_DoubleClick(object sender, EventArgs e)
        {
            if (lbxRegionSearch.SelectedItem == null) return;
            RegionSearchResultItem item = (RegionSearchResultItem)lbxRegionSearch.SelectedItem;

            selregion = item.Region;  
            txtRegion.Text = item.Region.Name;
            nudX.Value = 128;
            nudY.Value = 128;
            nudZ.Value = 0;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            StartRegionSearch();
        }

        private void StartRegionSearch()
        {
            lbxRegionSearch.Items.Clear();

            client.Grid.RequestMapRegion(txtSearchFor.Text.Trim(), GridLayerType.Objects);
        }

        private void txtSearchFor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            if (!btnFind.Enabled) return;
            e.SuppressKeyPress = true;
            
            StartRegionSearch();
        }

        private void lbxRegionSearch_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index < 0) return;

            RegionSearchResultItem itemToDraw = (RegionSearchResultItem)lbxRegionSearch.Items[e.Index];
            Brush textBrush = null;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                textBrush = new SolidBrush(Color.FromKnownColor(KnownColor.HighlightText));
            else
                textBrush = new SolidBrush(Color.FromKnownColor(KnownColor.ControlText));
            
            Font newFont = new Font(e.Font, FontStyle.Bold);
            SizeF stringSize = e.Graphics.MeasureString(itemToDraw.Region.Name, newFont);
            
            float iconSize = (float)trkIconSize.Value;
            float leftTextMargin = e.Bounds.Left + iconSize + 6.0f;
            float topTextMargin = e.Bounds.Top + 4.0f;
         
            if (itemToDraw.IsImageDownloaded)
            {
                if (itemToDraw.MapImage != null)
                {
                    e.Graphics.DrawImage(itemToDraw.MapImage, new RectangleF(e.Bounds.Left + 4.0f, e.Bounds.Top + 4.0f, iconSize, iconSize));
                }
            }
            else
            {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(200, 200, 200)), e.Bounds.Left + 4.0f, e.Bounds.Top + 4.0f, iconSize, iconSize);

                if (!itemToDraw.IsImageDownloading)
                    itemToDraw.RequestMapImage(125000.0f);
            }
          
            
            e.Graphics.DrawString(itemToDraw.Region.Name, newFont, textBrush, new PointF(leftTextMargin, topTextMargin));

            if (itemToDraw.GotAgentCount)
            {
                string peeps = " person";

                if (itemToDraw.Region.Agents != 1)
                {
                    peeps = " people";
                }

                string s = System.Convert.ToString(itemToDraw.Region.Agents, CultureInfo.CurrentCulture);

                e.Graphics.DrawString(s + peeps, e.Font, textBrush, new PointF(leftTextMargin + stringSize.Width + 6.0f, topTextMargin));
            }
            else
            {
                if (!itemToDraw.GettingAgentCount)
                {
                    itemToDraw.RequestAgentLocations();
                }
            }

            switch (itemToDraw.Region.Access)
            { 
                case SimAccess.PG:
                    e.Graphics.DrawString("PG", e.Font, textBrush, new PointF(leftTextMargin, topTextMargin + stringSize.Height));
                    break;

                case SimAccess.Mature:
                    e.Graphics.DrawString("Mature", e.Font, textBrush, new PointF(leftTextMargin, topTextMargin + stringSize.Height));
                    break;

                case SimAccess.Adult:
                    e.Graphics.DrawString("Adult", e.Font, textBrush, new PointF(leftTextMargin, topTextMargin + stringSize.Height));
                    break;

                case SimAccess.Down:
                    e.Graphics.DrawString("Offline", e.Font, new SolidBrush(Color.Red), new PointF(leftTextMargin, topTextMargin + stringSize.Height));
                    break;
            }

            e.Graphics.DrawLine(new Pen(Color.FromArgb(200, 200, 200)), new Point(e.Bounds.Left, e.Bounds.Bottom - 1), new Point(e.Bounds.Right, e.Bounds.Bottom - 1));
            e.DrawFocusRectangle();

            textBrush.Dispose();
            newFont.Dispose();
            textBrush = null;
            newFont = null;

            lbxRegionSearch.ItemHeight = trkIconSize.Value + 10;
        }

        private void txtSearchFor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
        }

        private void trkIconSize_Scroll(object sender, EventArgs e)
        {
            lbxRegionSearch.ItemHeight = trkIconSize.Value + 10;
        }

        private void pnlTeleportOptions_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmTeleport_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
        }
    }
}