//  Copyright (c) 2008-2011, www.metabolt.net
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
using OpenMetaverse;
using OpenMetaverse.Imaging;

namespace METAbolt
{
    public partial class UploadImage : Form
    {
        private METAboltInstance instance;
        private GridClient client;
        private Bitmap img;
        //private string ext;
        private string file;
        private byte[] ImgUp;

        public UploadImage(METAboltInstance instance, Image img, string file, string ext)
        {
            InitializeComponent();

            this.instance = instance;
            client = this.instance.Client;

            this.img = (Bitmap)img;
            //this.ext = ext;
            this.file = file;
        }

        private void ImageViewer_Load(object sender, EventArgs e)
        {
            textBox1.Text = System.IO.Path.GetFileNameWithoutExtension(file);

            label3.Text = "Loading image " + file;
            byte[] jpeg2k = LoadImage(file);

            if (jpeg2k == null)
            {
                label3.Text = "Failed to compress image"; 
                return;
            }

            label3.Text = "Ready";
            pbView.Image = img;
        }

        private byte[] LoadImage(string fileName)
        {
            string lowfilename = fileName.ToLower();
            Bitmap bitmap = null;

            try
            {
                if (lowfilename.EndsWith(".jp2") || lowfilename.EndsWith(".j2c"))
                {
                    Image image;
                    ManagedImage managedImage;

                    // Upload JPEG2000 images untouched
                    ImgUp = System.IO.File.ReadAllBytes(fileName);

                    OpenJPEG.DecodeToImage(ImgUp, out managedImage, out image);
                    bitmap = (Bitmap)image;
                }
                else
                {
                    if (lowfilename.EndsWith(".tga"))
                        bitmap = LoadTGAClass.LoadTGA(fileName);
                    else
                        bitmap = (Bitmap)System.Drawing.Image.FromFile(fileName);

                    int oldwidth = bitmap.Width;
                    int oldheight = bitmap.Height;

                    if (!IsPowerOfTwo((uint)oldwidth) || !IsPowerOfTwo((uint)oldheight))
                    {
                        Bitmap resized = new Bitmap(256, 256, bitmap.PixelFormat);
                        Graphics graphics = Graphics.FromImage(resized);

                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        graphics.InterpolationMode =
                           System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(bitmap, 0, 0, 256, 256);

                        bitmap.Dispose();
                        bitmap = resized;

                        oldwidth = 256;
                        oldheight = 256;
                    }

                    // Handle resizing to prevent excessively large images
                    if (oldwidth > 1024 || oldheight > 1024)
                    {
                        int newwidth = (oldwidth > 1024) ? 1024 : oldwidth;
                        int newheight = (oldheight > 1024) ? 1024 : oldheight;

                        Bitmap resized = new Bitmap(newwidth, newheight, bitmap.PixelFormat);
                        Graphics graphics = Graphics.FromImage(resized);

                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(bitmap, 0, 0, newwidth, newheight);

                        bitmap.Dispose();
                        bitmap = resized;
                    }

                    ImgUp = OpenJPEG.EncodeFromImage(bitmap, false);
                }
            }
            catch (Exception ex)
            {
                label3.Text = ex.ToString() + " SL Image Upload ";
                return null;
            }

            return ImgUp;
        }

        private static bool IsPowerOfTwo(uint n)
        {
            return (n & (n - 1)) == 0 && n != 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }        

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text += " uploaded by METAbolt " + DateTime.Now.ToLongDateString(); 

            label3.Text = "Uploading image...";
            UploadImg(textBox1.Text, textBox2.Text);    
        }

        private void UploadImg(string fname, string desc)
        {
            if (ImgUp != null)
            {
                //string name = System.IO.Path.GetFileNameWithoutExtension(file);
                UUID folder = client.Inventory.FindFolderForType(AssetType.Texture);

                client.Inventory.RequestCreateItemFromAsset(ImgUp, fname, desc, AssetType.Texture, InventoryType.Texture,
                folder, Img_Upload);
            }
        }

        private void Img_Upload(bool success, string status, UUID itemID, UUID assetID)
        {
            if (InvokeRequired)
            {
                if (IsHandleCreated)
                {
                    BeginInvoke(new MethodInvoker(() => Img_Upload(success, status, itemID, assetID)));
                }

                return;
            }

            if (success)
            {
                label3.Text = "Image uploaded successfully";
            }
            else
            {
                label3.Text = "Upload failed";
            }
        }
    }
}
