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
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SLNetworkComm;
using OpenMetaverse;
using OpenMetaverse.Imaging;
using OpenMetaverse.Assets;

namespace METAbolt
{
    public partial class InventoryImageConsole : UserControl
    {
        private METAboltInstance instance;
        private SLNetCom netcom;
        private GridClient client;
        private InventoryItem item;

        public InventoryImageConsole(METAboltInstance instance, InventoryItem item)
        {
            InitializeComponent();

            this.instance = instance;
            netcom = this.instance.Netcom;
            client = this.instance.Client;
            this.item = item;
            
            if (instance.ImageCache.ContainsImage(item.AssetUUID))
                SetFinalImage(instance.ImageCache.GetImage(item.AssetUUID));
            else
            {
                this.Disposed += new EventHandler(InventoryImageConsole_Disposed);
                //client.Assets.OnImageRecieveProgress += new AssetManager.ImageReceiveProgressCallback(Assets_OnImageReceived);
            }
        }

        private void InventoryImageConsole_Disposed(object sender, EventArgs e)
        {
            //client.Assets.OnImageRecieveProgress -= new AssetManager.ImageReceiveProgressCallback(Assets_OnImageReceived);
        }

        //comes in on separate thread
        private void Assets_OnImageReceived(TextureRequestState image, AssetTexture texture)
        {
            if (texture.AssetID != item.AssetUUID) return;

            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => Assets_OnImageReceived(image, texture)));
                return;
            }

            BeginInvoke(new OnSetStatusText(SetStatusText), new object[] { "Image downloaded. Decoding..." });

            //System.Drawing.Image decodedImage = ImageHelper.Decode(image.AssetData);

            //if (decodedImage == null)
            //{
            //    BeginInvoke(new OnSetStatusText(SetStatusText), new object[] { "D'oh! Error decoding image." });
            //    BeginInvoke(new MethodInvoker(DoErrorState));
            //    return;
            //}

            //instance.ImageCache.AddImage(image.ID, decodedImage);
            //BeginInvoke(new OnSetFinalImage(SetFinalImage), new object[] { decodedImage });

            ManagedImage mImg;
            Image sImage = null;

            //System.Drawing.Image decodedImage = ImageHelper.Decode(image.AssetData);
            //System.Drawing.Image decodedImage = OpenJPEGNet.OpenJPEG.DecodeToImage(image.AssetData);
            Boolean iret = OpenJPEG.DecodeToImage(texture.AssetData, out mImg, out sImage);
            System.Drawing.Image decodedImage = sImage;

            if (decodedImage == null)
            {
                BeginInvoke(new OnSetStatusText(SetStatusText), new object[] { "D'oh! Error decoding image." });
                BeginInvoke(new MethodInvoker(DoErrorState));
                return;
            }

            instance.ImageCache.AddImage(texture.AssetID, decodedImage);
            BeginInvoke(new OnSetFinalImage(SetFinalImage), new object[] { decodedImage });
        }

        //called on GUI thread
        private delegate void OnSetFinalImage(System.Drawing.Image finalImage);
        private void SetFinalImage(System.Drawing.Image finalImage)
        {
            pbxImage.Image = finalImage;

            pnlOptions.Visible = true;
            pnlStatus.Visible = false;
            
            // TPV change to allow only the creator to save the image. 31 Mar 2010

            //if ((item.Permissions.OwnerMask & PermissionMask.Copy) == PermissionMask.Copy)
            //{
            //    if ((item.Permissions.OwnerMask & PermissionMask.Modify) == PermissionMask.Modify)
            //    {
            //        btnSave.Click += delegate(object sender, EventArgs e)
            //        {
            //            if (sfdImage.ShowDialog() == DialogResult.OK)
            //            {
            //                switch (sfdImage.FilterIndex)
            //                {
            //                    case 1: //BMP
            //                        pbxImage.Image.Save(sfdImage.FileName, ImageFormat.Bmp);
            //                        break;

            //                    case 2: //JPG
            //                        pbxImage.Image.Save(sfdImage.FileName, ImageFormat.Jpeg);
            //                        break;

            //                    case 3: //PNG
            //                        pbxImage.Image.Save(sfdImage.FileName, ImageFormat.Png);
            //                        break;

            //                    default:
            //                        pbxImage.Image.Save(sfdImage.FileName, ImageFormat.Bmp);
            //                        break;
            //                }
            //            }
            //        };

            //        btnSave.Enabled = true;
            //    }
            //    else
            //    {
            //        btnSave.Enabled = false;
            //    }
            //}

            if (item.CreatorID == client.Self.AgentID)
            {
                btnSave.Click += delegate(object sender, EventArgs e)
                {
                    if (sfdImage.ShowDialog() == DialogResult.OK)
                    {
                        switch (sfdImage.FilterIndex)
                        {
                            case 1: //BMP
                                pbxImage.Image.Save(sfdImage.FileName, ImageFormat.Bmp);
                                break;

                            case 2: //JPG
                                pbxImage.Image.Save(sfdImage.FileName, ImageFormat.Jpeg);
                                break;

                            case 3: //PNG
                                pbxImage.Image.Save(sfdImage.FileName, ImageFormat.Png);
                                break;

                            default:
                                pbxImage.Image.Save(sfdImage.FileName, ImageFormat.Bmp);
                                break;
                        }
                    }
                };

                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
            }
        }

        //called on GUI thread
        private delegate void OnSetStatusText(string text);
        private void SetStatusText(string text)
        {
            lblStatus.Text = text;
        }

        private void DoErrorState()
        {
            lblStatus.Visible = true;
            lblStatus.ForeColor = Color.Red;
            proActivity.Visible = false;

            pnlStatus.Visible = true;
            pnlOptions.Visible = false;
        }

        private void pbxImage_Click(object sender, EventArgs e)
        {

        }

        private void InventoryImageConsole_Load(object sender, EventArgs e)
        {
            if (instance.ImageCache.ContainsImage(item.AssetUUID))
            {
                SetFinalImage(instance.ImageCache.GetImage(item.AssetUUID));
            }
            else
            {
                client.Assets.RequestImage(item.AssetUUID, ImageType.Normal, Assets_OnImageReceived);
            }
        }

        private void pnlStatus_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            (new ImageViewer(instance, pbxImage.Image)).Show();
        }
    }
}
