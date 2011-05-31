//  This code is largely based on the PrimWorkshop form of Radegast
//  Parts are based on the PrimWorkshop project of libopenmv
//
//  Copyright (c) 2009-2011, Radegast Development Team
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
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using OpenMetaverse;
using OpenMetaverse.StructuredData;
using OpenMetaverse.Imaging;
using OpenMetaverse.Rendering;
using OpenMetaverse.Assets;
using OpenTK.Graphics.OpenGL;
using SLNetworkComm;
using System.Threading;
using System.Diagnostics;
using PopupControl;
using System.Media;
using ExceptionReporting;

namespace METAbolt
{
    public partial class META3D : Form
    {
        public OpenTK.GLControl glControl = null; 
        public bool UseMultiSampling = true;
        public bool RenderingEnabled = false;
        public bool Wireframe = false;
        Dictionary<uint, FacetedMesh> Prims = new Dictionary<uint, FacetedMesh>();
        public uint RootPrimLocalID = 0;
        public Vector3 Center = Vector3.Zero;

        private Popup toolTip;
        private CustomToolTip customToolTip;

        int[] TexturePointers = new int[1];
        Dictionary<UUID, Image> Textures = new Dictionary<UUID, Image>();
        METAboltInstance instance;
        private GridClient client;
        private SLNetCom netcom;
        MeshmerizerR renderer;
        OpenTK.Graphics.GraphicsMode GLMode = null;
        AutoResetEvent TextureThreadContextReady = new AutoResetEvent(false);
        BlockingQueue<TextureLoadItem> PendingTextures = new BlockingQueue<TextureLoadItem>();
        float[] lightPos = new float[] { 0f, 0f, 1f, 0f };

        private bool TakeScreenShot = false;
        private bool snapped = false;
        bool dragging = false;
        int dragX, dragY, downX, downY;
        bool TextureThreadRunning = true;

        private ExceptionReporter reporter = new ExceptionReporter();

        internal class ThreadExceptionHandler
        {
            public void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
            {
                ExceptionReporter reporter = new ExceptionReporter();
                reporter.Show(e.Exception);
            }
        }

        public META3D(METAboltInstance instance, uint rootLocalID) : base() 
        {
            try
            {
                this.RootPrimLocalID = rootLocalID;

                InitializeComponent();

                string msg1 = "Drag (left mouse down) to rotate object\n" +
                                "ALT+Drag to Zoom\n" +
                                "Ctrl+Drag to Pan\n" +
                                "Wheel in/out to Zoom in/out\n\n" +
                                "Click camera then object for snapshot";

                toolTip = new Popup(customToolTip = new CustomToolTip(instance, msg1));
                toolTip.AutoClose = false;
                toolTip.FocusOnOpen = false;
                toolTip.ShowingAnimation = toolTip.HidingAnimation = PopupAnimations.Blend;

                Disposed += new EventHandler(META3D_Disposed);
                UseMultiSampling = cbAA.Checked = true;
                cbAA.CheckedChanged += cbAA_CheckedChanged;

                this.instance = instance;
                client = this.instance.Client;
                netcom = this.instance.Netcom;

                TexturePointers[0] = 0;

                renderer = new MeshmerizerR();

                client.Objects.TerseObjectUpdate += new EventHandler<TerseObjectUpdateEventArgs>(Objects_TerseObjectUpdate);
                client.Objects.ObjectUpdate += new EventHandler<PrimEventArgs>(Objects_ObjectUpdate);
                client.Objects.ObjectDataBlockUpdate += new EventHandler<ObjectDataBlockUpdateEventArgs>(Objects_ObjectDataBlockUpdate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "METAbolt 1");   
                //reporter.Show(ex);
            }
        }

        private void SetExceptionReporter()
        {
            reporter.Config.ShowSysInfoTab = false;   // alternatively, set properties programmatically
            reporter.Config.ShowFlatButtons = true;   // this particular config is code-only
            reporter.Config.CompanyName = "METAbolt";
            reporter.Config.ContactEmail = "support@vistalogic.co.uk";
            reporter.Config.EmailReportAddress = "support@vistalogic.co.uk";
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

        void META3D_Disposed(object sender, EventArgs e)
        {
            if (glControl != null)
            {
                glControl.Dispose();
            }

            glControl = null;

            client.Objects.TerseObjectUpdate -= new EventHandler<TerseObjectUpdateEventArgs>(Objects_TerseObjectUpdate);
            client.Objects.ObjectUpdate -= new EventHandler<PrimEventArgs>(Objects_ObjectUpdate);
            client.Objects.ObjectDataBlockUpdate -= new EventHandler<ObjectDataBlockUpdateEventArgs>(Objects_ObjectDataBlockUpdate);
        }

        void Objects_TerseObjectUpdate(object sender, TerseObjectUpdateEventArgs e)
        {
            if (Prims.ContainsKey(e.Prim.LocalID))
            {
                UpdatePrimBlocking(e.Prim);
            }
        }

        void Objects_ObjectUpdate(object sender, PrimEventArgs e)
        {
            if (Prims.ContainsKey(e.Prim.LocalID) || Prims.ContainsKey(e.Prim.ParentID))
            {
                UpdatePrimBlocking(e.Prim);
            }
        }

        void Objects_ObjectDataBlockUpdate(object sender, ObjectDataBlockUpdateEventArgs e)
        {
            if (Prims.ContainsKey(e.Prim.LocalID))
            {
                UpdatePrimBlocking(e.Prim);
            }
        }

        public void SetupGLControl()
        {
            RenderingEnabled = false;

            if (glControl != null)
                glControl.Dispose();
            glControl = null;

            GLMode = null;

            try
            {
                if (!UseMultiSampling)
                {
                    GLMode = new OpenTK.Graphics.GraphicsMode(OpenTK.DisplayDevice.Default.BitsPerPixel, 24, 8, 0);
                }
                else
                {
                    for (int aa = 0; aa <= 4; aa += 2)
                    {
                        var testMode = new OpenTK.Graphics.GraphicsMode(OpenTK.DisplayDevice.Default.BitsPerPixel, 24, 8, aa);
                        if (testMode.Samples == aa)
                        {
                            GLMode = testMode;
                        }
                    }
                }
            }
            catch
            {
                GLMode = null;
            }

            try
            {
                if (GLMode == null)
                {
                    // Try default mode
                    glControl = new OpenTK.GLControl();
                }
                else
                {
                    glControl = new OpenTK.GLControl(GLMode);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, Helpers.LogLevel.Warning, client);
                glControl = null;
            }

            if (glControl == null)
            {
                Logger.Log("Failed to initialize OpenGL control, cannot continue", Helpers.LogLevel.Error, client);
                return;
            }

            Logger.Log("Initializing OpenGL mode: " + GLMode.ToString(), Helpers.LogLevel.Info);

            glControl.Paint += glControl_Paint;
            glControl.Resize += glControl_Resize;
            glControl.MouseDown += glControl_MouseDown;
            glControl.MouseUp += glControl_MouseUp;
            glControl.MouseMove += glControl_MouseMove;
            glControl.MouseWheel += glControl_MouseWheel;
            glControl.Load += new EventHandler(glControl_Load);
            glControl.Disposed += new EventHandler(glControl_Disposed);
            glControl.Click += new EventHandler(glControl_Click);
            glControl.BackColor = Color.RoyalBlue;
            glControl.Dock = DockStyle.Fill;
            glControl.TabIndex = 0;
            Controls.Add(glControl);
            glControl.BringToFront();
            glControl.Focus();
        }

        void glControl_Disposed(object sender, EventArgs e)
        {
            TextureThreadRunning = false;
            PendingTextures.Close();
        }

        void glControl_Click(object sender, EventArgs e)
        {
            if (TakeScreenShot)
            {
                snapped = true;
            }
        }

        void glControl_Load(object sender, EventArgs e)
        {
            try
            {
                GL.ShadeModel(ShadingModel.Smooth);
                GL.ClearColor(Color.RoyalBlue);   //   (0f, 0f, 0f, 0f);

                //GL.LightModel(LightModelParameter.LightModelAmbient, new float[] { 0.5f, 0.5f, 0.5f, 1.0f });

                GL.Enable(EnableCap.Lighting);
                GL.Enable(EnableCap.Light0);
                GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0.5f, 0.5f, 0.5f, 1f });
                GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 0.3f, 0.3f, 0.3f, 1f });
                GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 0.8f, 0.8f, 0.8f, 1.0f });
                GL.Light(LightName.Light0, LightParameter.Position, lightPos);

                GL.ClearDepth(1.0d);
                GL.Enable(EnableCap.DepthTest);
                GL.Enable(EnableCap.ColorMaterial);
                GL.Enable(EnableCap.CullFace);
                GL.ColorMaterial(MaterialFace.Front, ColorMaterialParameter.AmbientAndDiffuse);
                GL.ColorMaterial(MaterialFace.Front, ColorMaterialParameter.Specular);

                GL.DepthMask(true);
                GL.DepthFunc(DepthFunction.Lequal);
                GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
                GL.MatrixMode(MatrixMode.Projection);

                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                RenderingEnabled = true;

                // Call the resizing function which sets up the GL drawing window
                // and will also invalidate the GL control
                glControl_Resize(null, null);

                glControl.Context.MakeCurrent(null);
                TextureThreadContextReady.Reset();

                var textureThread = new Thread(() => TextureThread())
                {
                    IsBackground = true,
                    Name = "TextureLoadingThread"
                };

                textureThread.Start();
                TextureThreadContextReady.WaitOne(1000, false);
                glControl.MakeCurrent();

                //Application.Idle += Application_Idle;
                //sw.Start(); 
            }
            catch (Exception ex)
            {
                RenderingEnabled = false;
                MessageBox.Show(ex.Message, "METAbolt 3");
            }
        }

        //float rotation = 0;
        //void Application_Idle(object sender, EventArgs e)
        //{
        //    double milliseconds = ComputeTimeSlice();
        //    Accumulate(milliseconds);
        //    Animate(milliseconds);
        //}

        //private double ComputeTimeSlice()
        //{
        //    sw.Stop();
        //    double timeslice = sw.Elapsed.TotalMilliseconds;
        //    sw.Reset();
        //    sw.Start();
        //    return timeslice;
        //}

        //private void Animate(double milliseconds)
        //{
        //    float deltaRotation = (float)milliseconds / 20.0f;
        //    rotation += deltaRotation;
        //    glControl.Invalidate();

        //    ThreadPool.QueueUserWorkItem(sync =>
        //    {
        //        if (client.Network.CurrentSim.ObjectsPrimitives.ContainsKey(RootPrimLocalID))
        //        {
        //            UpdatePrimBlocking(client.Network.CurrentSim.ObjectsPrimitives[RootPrimLocalID]);
        //            var children = client.Network.CurrentSim.ObjectsPrimitives.FindAll((Primitive p) => { return p.ParentID == RootPrimLocalID; });
        //            children.ForEach(p => UpdatePrimBlocking(p));
        //        }
        //    }
        //    );
        //}

        //double accumulator = 0;
        //int idleCounter = 0;
        //private void Accumulate(double milliseconds)
        //{
        //    idleCounter++;
        //    accumulator += milliseconds;
        //    if (accumulator > 1000)
        //    {
        //        label1.Text = idleCounter.ToString();
        //        accumulator -= 1000;
        //        idleCounter = 0; // don't forget to reset the counter!
        //    }
        //}

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            if (!RenderingEnabled) return;

            //// A LL
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Render(false);

            // A LL
            if (TakeScreenShot)
            {
                if (snapped)
                {
                    SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.camera_clic_with_flash);
                    simpleSound.Play();
                    simpleSound.Dispose();

                    capScreenBeforeNextSwap();
                    TakeScreenShot = false;
                    snapped = false;
                }
            }

            glControl.SwapBuffers();
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            if (!RenderingEnabled) return;
            glControl.MakeCurrent();

            GL.ClearColor(Color.RoyalBlue);   //   0.39f, 0.58f, 0.93f, 1.0f);

            GL.Viewport(0, 0, glControl.Width, glControl.Height);

            GL.PushMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            float dAspRat = (float)glControl.Width / (float)glControl.Height;
            GluPerspective(50f, dAspRat, 0.1f, 100.0f);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PopMatrix();

            //// A LL
            //glControl.Invalidate();
        }

        #region Mouse handling


        private void glControl_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                int newVal = Utils.Clamp(scrollZoom.Value + e.Delta / 75, scrollZoom.Minimum, scrollZoom.Maximum);

                if (scrollZoom.Value != newVal)
                {
                    scrollZoom.Value = newVal;
                    glControl_Resize(null, null);
                    GLInvalidate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "METAbolt 4");
            }
        }

        FacetedMesh RightclickedPrim;
        int RightclickedFaceID;

        private void glControl_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle)
                {
                    dragging = true;
                    downX = dragX = e.X;
                    downY = dragY = e.Y;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (TryPick(e.X, e.Y, out RightclickedPrim, out RightclickedFaceID))
                    {
                        ctxObjects.Show(glControl, e.X, e.Y);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "METAbolt 5");
            }
        }

        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (dragging)
                {
                    int deltaX = e.X - dragX;
                    int deltaY = e.Y - dragY;

                    if (e.Button == MouseButtons.Left)
                    {
                        if (ModifierKeys == Keys.Control || ModifierKeys == (Keys.Alt | Keys.Control | Keys.Shift))
                        {
                            Center.X -= deltaX / 100f;
                            Center.Z += deltaY / 100f;
                        }

                        if (ModifierKeys == Keys.Alt)
                        {
                            Center.Y -= deltaY / 25f;

                            int newYaw = scrollYaw.Value + deltaX;
                            if (newYaw < 0) newYaw += 360;
                            if (newYaw > 360) newYaw -= 360;

                            scrollYaw.Value = newYaw;

                        }

                        if (ModifierKeys == Keys.None || ModifierKeys == (Keys.Alt | Keys.Control))
                        {
                            int newRoll = scrollRoll.Value + deltaY;
                            if (newRoll < 0) newRoll += 360;
                            if (newRoll > 360) newRoll -= 360;

                            scrollRoll.Value = newRoll;


                            int newYaw = scrollYaw.Value + deltaX;
                            if (newYaw < 0) newYaw += 360;
                            if (newYaw > 360) newYaw -= 360;

                            scrollYaw.Value = newYaw;

                        }
                    }
                    else if (e.Button == MouseButtons.Middle)
                    {
                        Center.X -= deltaX / 100f;
                        Center.Z += deltaY / 100f;

                    }

                    dragX = e.X;
                    dragY = e.Y;

                    GLInvalidate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "METAbolt 6");
            }
        }

        private void glControl_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    dragging = false;

                    if (e.X == downX && e.Y == downY) // click
                    {
                        FacetedMesh picked;
                        int faceID;

                        if (TryPick(e.X, e.Y, out picked, out faceID))
                        {
                            client.Self.Grab(picked.Prim.LocalID, Vector3.Zero, Vector3.Zero, Vector3.Zero, faceID, Vector3.Zero, Vector3.Zero, Vector3.Zero);
                            client.Self.DeGrab(picked.Prim.LocalID);
                        }
                    }

                    GLInvalidate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "METAbolt 7");
            }
        }
        #endregion Mouse handling

        #region Texture thread
        void TextureThread()
        {
            try
            {
                OpenTK.INativeWindow window = new OpenTK.NativeWindow();
                OpenTK.Graphics.IGraphicsContext context = new OpenTK.Graphics.GraphicsContext(GLMode, window.WindowInfo);
                context.MakeCurrent(window.WindowInfo);
                TextureThreadContextReady.Set();
                PendingTextures.Open();
                Logger.DebugLog("Started Texture Thread");

                while (window.Exists && TextureThreadRunning)
                {
                    window.ProcessEvents();

                    TextureLoadItem item = null;

                    if (!PendingTextures.Dequeue(Timeout.Infinite, ref item)) continue;

                    if (LoadTexture(item.TeFace.TextureID, ref item.Data.Texture, false))
                    {
                        GL.GenTextures(1, out item.Data.TexturePointer);
                        GL.BindTexture(TextureTarget.Texture2D, item.Data.TexturePointer);

                        Bitmap bitmap = new Bitmap(item.Data.Texture);

                        bool hasAlpha;
                        if (item.Data.Texture.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                        {
                            hasAlpha = true;
                        }
                        else
                        {
                            hasAlpha = false;
                        }
                        item.Data.IsAlpha = hasAlpha;

                        bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        Rectangle rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);


                        BitmapData bitmapData =
                            bitmap.LockBits(
                            rectangle,
                            ImageLockMode.ReadOnly,
                            hasAlpha ? System.Drawing.Imaging.PixelFormat.Format32bppArgb : System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                        GL.TexImage2D(
                            TextureTarget.Texture2D,
                            0,
                            hasAlpha ? PixelInternalFormat.Rgba : PixelInternalFormat.Rgb8,
                            bitmap.Width,
                            bitmap.Height,
                            0,
                            hasAlpha ? OpenTK.Graphics.OpenGL.PixelFormat.Bgra : OpenTK.Graphics.OpenGL.PixelFormat.Bgr,
                            PixelType.UnsignedByte,
                            bitmapData.Scan0);

                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
                        //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, 1);
                        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                        bitmap.UnlockBits(bitmapData);
                        bitmap.Dispose();

                        GL.Flush();
                        GLInvalidate();
                    }
                }

                Logger.DebugLog("Texture thread exited");
            }
            catch (Exception ex)
            {
                Logger.Log("META3D TextureThread: Your video card is too old and does not support Mipmap generation", Helpers.LogLevel.Warning);  
            }            
        }
        #endregion Texture thread

        private void META3D_Shown(object sender, EventArgs e)
        {
            SetupGLControl();

            ThreadPool.QueueUserWorkItem(sync =>
                {
                    if (client.Network.CurrentSim.ObjectsPrimitives.ContainsKey(RootPrimLocalID))
                    {
                        UpdatePrimBlocking(client.Network.CurrentSim.ObjectsPrimitives[RootPrimLocalID]);
                        var children = client.Network.CurrentSim.ObjectsPrimitives.FindAll((Primitive p) => { return p.ParentID == RootPrimLocalID; });
                        children.ForEach(p => UpdatePrimBlocking(p));
                    }
                }
            );
        }

        #region Public methods
        public void SetView(Vector3 center, int roll, int pitch, int yaw, int zoom)
        {
            this.Center = center;
            scrollRoll.Value = roll;
            scrollPitch.Value = pitch;
            scrollYaw.Value = yaw;
            scrollZoom.Value = zoom;
        }

        public FacetedMesh GenerateFacetedMesh(Primitive prim, OSDMap MeshData, DetailLevel LOD)
        {
            FacetedMesh ret = new FacetedMesh();

            ret.Faces = new List<Face>();
            ret.Prim = prim;
            ret.Profile = new Profile();
            ret.Profile.Faces = new List<ProfileFace>();
            ret.Profile.Positions = new List<Vector3>();
            ret.Path = new OpenMetaverse.Rendering.Path();
            ret.Path.Points = new List<PathPoint>();

            try
            {
                OSD facesOSD = null;

                switch (LOD)
                {
                    default:
                    case DetailLevel.Highest:
                        facesOSD = MeshData["high_lod"];
                        break;

                    case DetailLevel.High:
                        facesOSD = MeshData["medium_lod"];
                        break;

                    case DetailLevel.Medium:
                        facesOSD = MeshData["low_lod"];
                        break;

                    case DetailLevel.Low:
                        facesOSD = MeshData["lowest_lod"];
                        break;
                }

                if (facesOSD == null || !(facesOSD is OSDArray))
                {
                    return ret;
                }

                OSDArray decodedMeshOsdArray = (OSDArray)facesOSD;



                for (int faceNr = 0; faceNr < decodedMeshOsdArray.Count; faceNr++)
                {
                    OSD subMeshOsd = decodedMeshOsdArray[faceNr];
                    Face oface = new Face();
                    oface.ID = faceNr;
                    oface.Vertices = new List<Vertex>();
                    oface.Indices = new List<ushort>();
                    oface.TextureFace = prim.Textures.GetFace((uint)faceNr);

                    if (subMeshOsd is OSDMap)
                    {
                        OSDMap subMeshMap = (OSDMap)subMeshOsd;

                        Vector3 posMax = ((OSDMap)subMeshMap["PositionDomain"])["Max"];
                        Vector3 posMin = ((OSDMap)subMeshMap["PositionDomain"])["Min"];

                        Vector2 texPosMax = ((OSDMap)subMeshMap["TexCoord0Domain"])["Max"];
                        Vector2 texPosMin = ((OSDMap)subMeshMap["TexCoord0Domain"])["Min"];


                        byte[] posBytes = subMeshMap["Position"];
                        byte[] norBytes = subMeshMap["Normal"];
                        byte[] texBytes = subMeshMap["TexCoord0"];

                        for (int i = 0; i < posBytes.Length; i += 6)
                        {
                            ushort uX = Utils.BytesToUInt16(posBytes, i);
                            ushort uY = Utils.BytesToUInt16(posBytes, i + 2);
                            ushort uZ = Utils.BytesToUInt16(posBytes, i + 4);

                            Vertex vx = new Vertex();

                            vx.Position = new Vector3(
                                Utils.UInt16ToFloat(uX, posMin.X, posMax.X),
                                Utils.UInt16ToFloat(uY, posMin.Y, posMax.Y),
                                Utils.UInt16ToFloat(uZ, posMin.Z, posMax.Z));

                            ushort nX = Utils.BytesToUInt16(norBytes, i);
                            ushort nY = Utils.BytesToUInt16(norBytes, i + 2);
                            ushort nZ = Utils.BytesToUInt16(norBytes, i + 4);

                            vx.Normal = new Vector3(
                                Utils.UInt16ToFloat(nX, posMin.X, posMax.X),
                                Utils.UInt16ToFloat(nY, posMin.Y, posMax.Y),
                                Utils.UInt16ToFloat(nZ, posMin.Z, posMax.Z));

                            var vertexIndexOffset = oface.Vertices.Count * 4;

                            if (texBytes != null && texBytes.Length >= vertexIndexOffset + 4)
                            {
                                ushort tX = Utils.BytesToUInt16(texBytes, vertexIndexOffset);
                                ushort tY = Utils.BytesToUInt16(texBytes, vertexIndexOffset + 2);

                                vx.TexCoord = new Vector2(
                                    Utils.UInt16ToFloat(tX, texPosMin.X, texPosMax.X),
                                    Utils.UInt16ToFloat(tY, texPosMin.Y, texPosMax.Y));
                            }

                            oface.Vertices.Add(vx);
                        }

                        byte[] triangleBytes = subMeshMap["TriangleList"];
                        for (int i = 0; i < triangleBytes.Length; i += 6)
                        {
                            ushort v1 = (ushort)(Utils.BytesToUInt16(triangleBytes, i));
                            oface.Indices.Add(v1);
                            ushort v2 = (ushort)(Utils.BytesToUInt16(triangleBytes, i + 2));
                            oface.Indices.Add(v2);
                            ushort v3 = (ushort)(Utils.BytesToUInt16(triangleBytes, i + 4));
                            oface.Indices.Add(v3);
                        }
                    }
                    ret.Faces.Add(oface);
                }

            }
            catch (Exception ex)
            {
                Logger.Log("Failed to decode mesh asset: " + ex.Message, Helpers.LogLevel.Warning);
            }
            return ret;
        }
        #endregion Public methods

        #region Private methods (the meat)
        private OpenTK.Vector3 WorldToScreen(OpenTK.Vector3 world)
        {
            OpenTK.Vector3 screen = new OpenTK.Vector3(0,0,0);

            double[] ModelViewMatrix = new double[16];
            double[] ProjectionMatrix = new double[16];
            int[] Viewport = new int[4];

            GL.GetInteger(GetPName.Viewport, Viewport);
            GL.GetDouble(GetPName.ModelviewMatrix, ModelViewMatrix);
            GL.GetDouble(GetPName.ProjectionMatrix, ProjectionMatrix);

            OpenTK.Graphics.Glu.Project(world,
                ModelViewMatrix,
                ProjectionMatrix,
                Viewport,
                out screen);

            screen.Y = glControl.Height - screen.Y;

            return screen;
        }

        OpenTK.Graphics.TextPrinter Printer = new OpenTK.Graphics.TextPrinter(OpenTK.Graphics.TextQuality.High);

        private void RenderText()
        {
            lock (Prims)
            {
                int primNr = 0;
                foreach (FacetedMesh mesh in Prims.Values)
                {
                    primNr++;
                    Primitive prim = mesh.Prim;
                    if (!string.IsNullOrEmpty(prim.Text))
                    {
                        string text = System.Text.RegularExpressions.Regex.Replace(prim.Text, "(\r?\n)+", "\n");
                        OpenTK.Vector3 screenPos = OpenTK.Vector3.Zero;
                        OpenTK.Vector3 primPos = OpenTK.Vector3.Zero;

                        // Is it child prim
                        if (prim.ParentID == RootPrimLocalID)
                        {
                            primPos = new OpenTK.Vector3(prim.Position.X, prim.Position.Y, prim.Position.Z);
                        }

                        primPos.Z += prim.Scale.Z * 0.7f;
                        screenPos = WorldToScreen(primPos);
                        Printer.Begin();

                        Color color = Color.FromArgb((int)(prim.TextColor.A * 255), (int)(prim.TextColor.R * 255), (int)(prim.TextColor.G * 255), (int)(prim.TextColor.B * 255));

                        using (Font f = new Font(FontFamily.GenericSansSerif, 10f, FontStyle.Bold))
                        {
                            var size = Printer.Measure(text, f);
                            screenPos.X -= size.BoundingBox.Width / 2;
                            screenPos.Y -= size.BoundingBox.Height;

                            // Shadow
                            if (color != Color.Gray)
                            {
                                Printer.Print(text, f, Color.Gray, new RectangleF(screenPos.X + 1, screenPos.Y + 1, size.BoundingBox.Width, size.BoundingBox.Height), OpenTK.Graphics.TextPrinterOptions.Default, OpenTK.Graphics.TextAlignment.Center);
                            }

                            Printer.Print(text, f, color, new RectangleF(screenPos.X, screenPos.Y, size.BoundingBox.Width, size.BoundingBox.Height), OpenTK.Graphics.TextPrinterOptions.Default, OpenTK.Graphics.TextAlignment.Center);
                        }

                        Printer.End();
                    }
                }
            }
        }

        private void RenderAvatar()
        {
            //GL.Color3(0f, 1f, 0f);

            //client.Network.CurrentSim.ObjectsAvatars.ForEach(
            //    delegate(Avatar avatar)
            //    {
            //        //if (avatar.ID == client.Self.AgentID)
            //        //{
            //        //    GL.PushMatrix();
            //        //    GL.Translate(avatar.Position.X, avatar.Position.Y, avatar.Position.Z);

            //        //    IntPtr q = OpenTK.Graphics.Glu.NewQuadric();
            //        //    OpenTK.Graphics.Glu.Sphere(q, 1.0d, 10, 10);
            //        //    OpenTK.Graphics.Glu.DeleteQuadric(q);
            //        //    GL.PopMatrix();


            //        //    //GLU.GLUquadric quad = Glu.gluNewQuadric();
            //        //    //Glu.gluSphere(quad, 1.0d, 10, 10);
            //        //    //Glu.gluDeleteQuadric(quad);

            //        //    GL.PopMatrix();
            //        //}
            //    }
            //);

            //GL.Color3(1f, 1f, 1f);
        }

        private void RenderObjects(RenderPass pass)
        {
            lock (Prims)
            {
                int primNr = 0;
                foreach (FacetedMesh mesh in Prims.Values)
                {
                    primNr++;
                    Primitive prim = mesh.Prim;
                    // Individual prim matrix
                    GL.PushMatrix();

                    if (prim.ParentID == RootPrimLocalID)
                    {
                        FacetedMesh parent = null;
                        if (Prims.TryGetValue(prim.ParentID, out parent))
                        {
                            // Apply prim translation and rotation relative to the root prim
                            GL.MultMatrix(Math3D.CreateRotationMatrix(parent.Prim.Rotation));
                            //GL.MultMatrixf(Math3D.CreateTranslationMatrix(parent.Prim.Position));
                        }

                        // Prim roation relative to root
                        GL.MultMatrix(Math3D.CreateTranslationMatrix(prim.Position));
                    }

                    // Prim roation
                    GL.MultMatrix(Math3D.CreateRotationMatrix(prim.Rotation));

                    // Prim scaling
                    GL.Scale(prim.Scale.X, prim.Scale.Y, prim.Scale.Z);

                    // Draw the prim faces
                    for (int j = 0; j < mesh.Faces.Count; j++)
                    {
                        Primitive.TextureEntryFace teFace = mesh.Prim.Textures.FaceTextures[j];
                        Face face = mesh.Faces[j];
                        FaceData data = (FaceData)face.UserData;

                        if (teFace == null)
                            teFace = mesh.Prim.Textures.DefaultTexture;

                        if (pass != RenderPass.Picking)
                        {
                            bool belongToAlphaPass = (teFace.RGBA.A < 0.99) || data.IsAlpha;

                            if (belongToAlphaPass && pass != RenderPass.Alpha) continue;
                            if (!belongToAlphaPass && pass == RenderPass.Alpha) continue;

                            // Don't render transparent faces
                            if (teFace.RGBA.A <= 0.01f) continue;

                            switch (teFace.Shiny)
                            {
                                case Shininess.High:
                                    GL.Material(MaterialFace.Front, MaterialParameter.Shininess, 94f);
                                    break;

                                case Shininess.Medium:
                                    GL.Material(MaterialFace.Front, MaterialParameter.Shininess, 64f);
                                    break;

                                case Shininess.Low:
                                    GL.Material(MaterialFace.Front, MaterialParameter.Shininess, 24f);
                                    break;


                                case Shininess.None:
                                default:
                                    GL.Material(MaterialFace.Front, MaterialParameter.Shininess, 0f);
                                    break;
                            }

                            var faceColor = new float[] { teFace.RGBA.R, teFace.RGBA.G, teFace.RGBA.B, teFace.RGBA.A };

                            GL.Color4(faceColor);
                            GL.Material(MaterialFace.Front, MaterialParameter.AmbientAndDiffuse, faceColor);
                            GL.Material(MaterialFace.Front, MaterialParameter.Specular, faceColor);

                            if (data.TexturePointer != 0)
                            {
                                GL.Enable(EnableCap.Texture2D);
                            }
                            else
                            {
                                GL.Disable(EnableCap.Texture2D);
                            }

                            // Bind the texture
                            GL.BindTexture(TextureTarget.Texture2D, data.TexturePointer);
                        }
                        else
                        {
                            data.PickingID = primNr;
                            var primNrBytes = Utils.Int16ToBytes((short)primNr);
                            var faceColor = new byte[] { primNrBytes[0], primNrBytes[1], (byte)j, 255 };

                            GL.Color4(faceColor);
                        }

                        GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, data.TexCoords);
                        GL.VertexPointer(3, VertexPointerType.Float, 0, data.Vertices);
                        GL.NormalPointer(NormalPointerType.Float, 0, data.Normals);
                        GL.DrawElements(BeginMode.Triangles, data.Indices.Length, DrawElementsType.UnsignedShort, data.Indices);

                    }

                    // Pop the prim matrix
                    GL.PopMatrix();
                }
            }
        }

        private void Render(bool picking)
        {
            glControl.MakeCurrent();

            if (picking)
            {
                GL.ClearColor(Color.RoyalBlue);   // (1f, 1f, 1f, 1f);
            }
            else
            {
                GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f);
            }

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            // Setup wireframe or solid fill drawing mode
            if (Wireframe && !picking)
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            else
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }

            var mLookAt = OpenTK.Matrix4d.LookAt(
                    Center.X, (double)scrollZoom.Value * 0.1d + Center.Y, Center.Z,
                    Center.X, Center.Y, Center.Z,
                    0d, 0d, 1d);
            GL.MultMatrix(ref mLookAt);

            //GL.Light(LightName.Light0, LightParameter.Position, lightPos);

            // Push the world matrix
            GL.PushMatrix();

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.EnableClientState(ArrayCap.NormalArray);

            // World rotations
            GL.Rotate((float)scrollRoll.Value, 1f, 0f, 0f);
            GL.Rotate((float)scrollPitch.Value, 0f, 1f, 0f);
            GL.Rotate((float)scrollYaw.Value, 0f, 0f, 1f);

            if (picking)
            {
                RenderObjects(RenderPass.Picking);
            }
            else
            {
                RenderObjects(RenderPass.Simple);
                RenderObjects(RenderPass.Alpha);
                RenderText();
                RenderAvatar();
            }

            // Pop the world matrix
            GL.PopMatrix();

            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.NormalArray);

            GL.Flush();
        }

        private void GluPerspective(float fovy, float aspect, float zNear, float zFar)
        {
            try
            {
                float fH = (float)Math.Tan(fovy / 360 * (float)Math.PI) * zNear;
                float fW = fH * aspect;
                GL.Frustum(-fW, fW, -fH, fH, zNear, zFar);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "METAbolt 13");
            }
        }

        private bool TryPick(int x, int y, out FacetedMesh picked, out int faceID)
        {
            // Save old attributes
            GL.PushAttrib(AttribMask.AllAttribBits);

            // Disable some attributes to make the objects flat / solid color when they are drawn
            GL.Disable(EnableCap.Fog);
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Dither);
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.LineStipple);
            GL.Disable(EnableCap.PolygonStipple);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.AlphaTest);

            Render(true);

            byte[] color = new byte[4];
            GL.ReadPixels(x, glControl.Height - y, 1, 1, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, color);

            GL.PopAttrib();

            int primID = Utils.BytesToUInt16(color, 0);
            faceID = color[2];

            picked = null;

            lock (Prims)
            {
                foreach (var mesh in Prims.Values)
                {
                    foreach (var face in mesh.Faces)
                    {
                        if (((FaceData)face.UserData).PickingID == primID)
                        {
                            picked = mesh;
                            break;
                        }
                    }

                    if (picked != null) break;
                }
            }

            return picked != null;
        }

        private void UpdatePrimBlocking(Primitive prim)
        {

            FacetedMesh mesh = null;
            FacetedMesh existingMesh = null;

            lock (Prims)
            {
                if (Prims.ContainsKey(prim.LocalID))
                {
                    existingMesh = Prims[prim.LocalID];
                }
            }

            if (prim.Textures == null)
                return;

            try
            {
                if (prim.Sculpt != null && prim.Sculpt.SculptTexture != UUID.Zero)
                {
                    if (prim.Sculpt.Type != SculptType.Mesh)
                    { 
                        // Regular sculptie
                        Image img = null;
                        if (!LoadTexture(prim.Sculpt.SculptTexture, ref img, true))
                            return;
                        mesh = renderer.GenerateFacetedSculptMesh(prim, (Bitmap)img, DetailLevel.Highest);
                    }
                    else
                    { 
                        // Mesh
                        AutoResetEvent gotMesh = new AutoResetEvent(false);
                        bool meshSuccess = false;

                        client.Assets.RequestMesh(prim.Sculpt.SculptTexture, (success, meshAsset) =>
                            {
                                if (!success || !meshAsset.Decode())
                                {
                                    Logger.Log("Failed to fetch or decode the mesh asset", Helpers.LogLevel.Warning, client);
                                }
                                else
                                {
                                    mesh = GenerateFacetedMesh(prim, meshAsset.MeshData, DetailLevel.Highest);
                                    meshSuccess = true;
                                }
                                gotMesh.Set();
                            });

                        if (!gotMesh.WaitOne(20 * 1000, false)) return;
                        if (!meshSuccess) return;
                    }
                }
                else
                {
                    mesh = renderer.GenerateFacetedMesh(prim, DetailLevel.Highest);
                }
            }
            catch (Exception ex)
            {
                return;
            }

            // Create a FaceData struct for each face that stores the 3D data
            // in a OpenGL friendly format
            for (int j = 0; j < mesh.Faces.Count; j++)
            {
                Face face = mesh.Faces[j];
                FaceData data = new FaceData();

                // Vertices for this face
                data.Vertices = new float[face.Vertices.Count * 3];
                data.Normals = new float[face.Vertices.Count * 3];
                for (int k = 0; k < face.Vertices.Count; k++)
                {
                    data.Vertices[k * 3 + 0] = face.Vertices[k].Position.X;
                    data.Vertices[k * 3 + 1] = face.Vertices[k].Position.Y;
                    data.Vertices[k * 3 + 2] = face.Vertices[k].Position.Z;

                    data.Normals[k * 3 + 0] = face.Vertices[k].Normal.X;
                    data.Normals[k * 3 + 1] = face.Vertices[k].Normal.Y;
                    data.Normals[k * 3 + 2] = face.Vertices[k].Normal.Z;
                }

                // Indices for this face
                data.Indices = face.Indices.ToArray();

                // Texture transform for this face
                Primitive.TextureEntryFace teFace = prim.Textures.GetFace((uint)j);
                renderer.TransformTexCoords(face.Vertices, face.Center, teFace);

                // Texcoords for this face
                data.TexCoords = new float[face.Vertices.Count * 2];
                for (int k = 0; k < face.Vertices.Count; k++)
                {
                    data.TexCoords[k * 2 + 0] = face.Vertices[k].TexCoord.X;
                    data.TexCoords[k * 2 + 1] = face.Vertices[k].TexCoord.Y;
                }

                // Set the UserData for this face to our FaceData struct
                face.UserData = data;
                mesh.Faces[j] = face;


                if (existingMesh != null &&
                    j < existingMesh.Faces.Count &&
                    existingMesh.Faces[j].TextureFace.TextureID == teFace.TextureID &&
                    ((FaceData)existingMesh.Faces[j].UserData).TexturePointer != 0
                    )
                {
                    FaceData existingData = (FaceData)existingMesh.Faces[j].UserData;
                    data.TexturePointer = existingData.TexturePointer;
                }
                else
                {

                    var textureItem = new TextureLoadItem()
                    {
                        Data = data,
                        Prim = prim,
                        TeFace = teFace
                    };

                    PendingTextures.Enqueue(textureItem);
                }
            }

            lock (Prims)
            {
                Prims[prim.LocalID] = mesh;
            }

            GLInvalidate();
        }

        private bool LoadTexture(UUID textureID, ref Image texture, bool removeAlpha)
        {
            ManualResetEvent gotImage = new ManualResetEvent(false);
            Image img = null;

            if (Textures.ContainsKey(textureID))
            {
                texture = Textures[textureID];
                return true;
            }

            try
            {
                gotImage.Reset();
                instance.Client.Assets.RequestImage(textureID, (TextureRequestState state, AssetTexture assetTexture) =>
                    {
                        if (state == TextureRequestState.Finished)
                        {
                            ManagedImage mi;
                            OpenJPEG.DecodeToImage(assetTexture.AssetData, out mi);

                            if (removeAlpha)
                            {
                                if ((mi.Channels & ManagedImage.ImageChannels.Alpha) != 0)
                                {
                                    mi.ConvertChannels(mi.Channels & ~ManagedImage.ImageChannels.Alpha);
                                }
                            }

                            img = LoadTGAClass.LoadTGA(new MemoryStream(mi.ExportTGA()));
                            Textures[textureID] = (System.Drawing.Image)img.Clone();
                        }
                        gotImage.Set();
                    }
                );
                gotImage.WaitOne(30 * 1000, false);
                if (img != null)
                {
                    texture = img;
                    Wireframe = false;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, Helpers.LogLevel.Error, instance.Client, ex);
                return false;
            }
        }

        private void GLInvalidate()
        {
            if (glControl == null || !RenderingEnabled) return;

            if (InvokeRequired)
            {
                if (IsHandleCreated)
                {
                    BeginInvoke(new MethodInvoker(() => GLInvalidate()));
                }
                return;
            }

            glControl.Invalidate();
        }
        #endregion Private methods (the meat)

        #region Form controls handlers
        private void scroll_ValueChanged(object sender, EventArgs e)
        {
            GLInvalidate();
        }

        private void scrollZoom_ValueChanged(object sender, EventArgs e)
        {
            glControl_Resize(null, null);
            GLInvalidate();
        }

        private void chkWireFrame_CheckedChanged(object sender, EventArgs e)
        {
            Wireframe = chkWireFrame.Checked;
            GLInvalidate();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            scrollYaw.Value = 90;
            scrollPitch.Value = 0;
            scrollRoll.Value = 0;
            scrollZoom.Value = -30;
            Center = Vector3.Zero;

            GLInvalidate();
        }

        private void oBJToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "OBJ files (*.obj)|*.obj";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (!MeshToOBJ.MeshesToOBJ(Prims, dialog.FileName))
                {
                    MessageBox.Show("Failed to save file " + dialog.FileName +
                        ". Ensure that you have permission to write to that file and it is currently not in use");
                }
            }
        }

        private void cbAA_CheckedChanged(object sender, EventArgs e)
        {
            //instance.GlobalSettings["use_multi_sampling"] = UseMultiSampling = cbAA.Checked;
            SetupGLControl();
        }

        #endregion Form controls handlers

        #region Context menu
        private void ctxObjects_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (instance.State.IsSitting)
            {
                sitToolStripMenuItem.Text = "Stand up";
            }
            else if (RightclickedPrim.Prim.Properties != null
                && !string.IsNullOrEmpty(RightclickedPrim.Prim.Properties.SitName))
            {
                sitToolStripMenuItem.Text = RightclickedPrim.Prim.Properties.SitName;
            }
            else
            {
                sitToolStripMenuItem.Text = "Sit";
            }

            if (RightclickedPrim.Prim.Properties != null
                && !string.IsNullOrEmpty(RightclickedPrim.Prim.Properties.TouchName))
            {
                touchToolStripMenuItem.Text = RightclickedPrim.Prim.Properties.TouchName;
            }
            else
            {
                touchToolStripMenuItem.Text = "Touch";
            }

            if (RightclickedPrim.Prim.Properties != null)
            {
                if (RightclickedPrim.Prim.Properties.OwnerID == client.Self.AgentID)
                {
                    takeToolStripMenuItem.Enabled = true;
                    deleteToolStripMenuItem.Enabled = true;
                }
                else
                {
                    takeToolStripMenuItem.Enabled = false;
                    deleteToolStripMenuItem.Enabled = false;
                }
            }
        }

        private void touchToolStripMenuItem_Click(object sender, EventArgs e)
        {

            client.Self.Grab(RightclickedPrim.Prim.LocalID, Vector3.Zero, Vector3.Zero, Vector3.Zero, RightclickedFaceID, Vector3.Zero, Vector3.Zero, Vector3.Zero);
            Thread.Sleep(100);
            client.Self.DeGrab(RightclickedPrim.Prim.LocalID);
        }

        private void sitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!instance.State.IsSitting)
            {
                instance.State.SetSitting(true, RightclickedPrim.Prim.ID);
            }
            else
            {
                instance.State.SetSitting(false, UUID.Zero);
            }
        }

        private void takeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            client.Inventory.RequestDeRezToInventory(RightclickedPrim.Prim.LocalID);
            Close();
        }

        private void returnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            client.Inventory.RequestDeRezToInventory(RightclickedPrim.Prim.LocalID, DeRezDestination.ReturnToOwner, UUID.Zero, UUID.Random());
            Close();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RightclickedPrim.Prim.Properties != null && RightclickedPrim.Prim.Properties.OwnerID != client.Self.AgentID)
                returnToolStripMenuItem_Click(sender, e);
            else
            {
                client.Inventory.RequestDeRezToInventory(RightclickedPrim.Prim.LocalID, DeRezDestination.AgentInventoryTake, client.Inventory.FindFolderForType(AssetType.TrashFolder), UUID.Random());
            }
            Close();
        }
        #endregion Context menu

        private void scrollYaw_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            TakeScreenShot = true;
            snapped = false;
        }

        private void capScreenBeforeNextSwap()
        {
            snapped = false;
            TakeScreenShot = false;

            Bitmap bmp = GrabScreenshot();
            //Image img = (Image)bmp;

            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\METAbolt\\";
            string filename = "Object_Snaphot_" + DateTime.Now.ToString() + ".png";
            filename = filename.Replace("/", "-");
            filename = filename.Replace(":", "-");

            try
            {
                saveFileDialog1.Filter = "PNG files (*.png)|*.png";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.FileName = filename;
                saveFileDialog1.InitialDirectory = path;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    bmp.Save(saveFileDialog1.FileName, ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "METAbolt 17");
            }
        }

        public Bitmap GrabScreenshot()
        {
            Bitmap bmp = new Bitmap(glControl.Width, glControl.Height);
            try
            {                
                System.Drawing.Imaging.BitmapData data = bmp.LockBits(glControl.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly,
                    System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                GL.ReadPixels(0, 0, glControl.Width, glControl.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);

                GL.Finish();

                bmp.UnlockBits(data);
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "METAbolt 18");
            }

            return bmp;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void picAutoSit_MouseHover(object sender, EventArgs e)
        {
            toolTip.Show(picAutoSit);
        }

        private void picAutoSit_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Close();
        }

        private void picAutoSit_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://www.metabolt.net/metawiki/");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }

    #region Helper classes
    public class FaceData
    {
        public float[] Vertices;
        public ushort[] Indices;
        public float[] TexCoords;
        public float[] Normals;
        public int TexturePointer;
        public int PickingID = -1;
        public System.Drawing.Image Texture;
        public bool IsAlpha;
    }

    public class TextureLoadItem
    {
        public FaceData Data;
        public Primitive Prim;
        public Primitive.TextureEntryFace TeFace;
    }

    public static class Render
    {
        public static IRendering Plugin;
    }

    public static class MeshToOBJ
    {
        public static bool MeshesToOBJ(Dictionary<uint, FacetedMesh> meshes, string filename)
        {
            StringBuilder obj = new StringBuilder();
            StringBuilder mtl = new StringBuilder();

            FileInfo objFileInfo = new FileInfo(filename);

            string mtlFilename = objFileInfo.FullName.Substring(objFileInfo.DirectoryName.Length + 1,
                objFileInfo.FullName.Length - (objFileInfo.DirectoryName.Length + 1) - 4) + ".mtl";

            obj.AppendLine("# Created by libprimrender");
            obj.AppendLine("mtllib ./" + mtlFilename);
            obj.AppendLine();

            mtl.AppendLine("# Created by libprimrender");
            mtl.AppendLine();

            int primNr = 0;
            foreach (FacetedMesh mesh in meshes.Values)
            {
                for (int j = 0; j < mesh.Faces.Count; j++)
                {
                    Face face = mesh.Faces[j];

                    if (face.Vertices.Count > 2)
                    {
                        string mtlName = String.Format("material{0}-{1}", primNr, face.ID);
                        Primitive.TextureEntryFace tex = face.TextureFace;
                        string texName = tex.TextureID.ToString() + ".tga";

                        // FIXME: Convert the source to TGA (if needed) and copy to the destination

                        float shiny = 0.00f;
                        switch (tex.Shiny)
                        {
                            case Shininess.High:
                                shiny = 1.00f;
                                break;
                            case Shininess.Medium:
                                shiny = 0.66f;
                                break;
                            case Shininess.Low:
                                shiny = 0.33f;
                                break;
                        }

                        obj.AppendFormat("g face{0}-{1}{2}", primNr, face.ID, Environment.NewLine);

                        mtl.AppendLine("newmtl " + mtlName);
                        mtl.AppendFormat("Ka {0} {1} {2}{3}", tex.RGBA.R, tex.RGBA.G, tex.RGBA.B, Environment.NewLine);
                        mtl.AppendFormat("Kd {0} {1} {2}{3}", tex.RGBA.R, tex.RGBA.G, tex.RGBA.B, Environment.NewLine);
                        //mtl.AppendFormat("Ks {0} {1} {2}{3}");
                        mtl.AppendLine("Tr " + tex.RGBA.A);
                        mtl.AppendLine("Ns " + shiny);
                        mtl.AppendLine("illum 1");
                        if (tex.TextureID != UUID.Zero && tex.TextureID != Primitive.TextureEntry.WHITE_TEXTURE)
                            mtl.AppendLine("map_Kd ./" + texName);
                        mtl.AppendLine();

                        // Write the vertices, texture coordinates, and vertex normals for this side
                        for (int k = 0; k < face.Vertices.Count; k++)
                        {
                            Vertex vertex = face.Vertices[k];

                            #region Vertex

                            Vector3 pos = vertex.Position;

                            // Apply scaling
                            pos *= mesh.Prim.Scale;

                            // Apply rotation
                            pos *= mesh.Prim.Rotation;

                            // The root prim position is sim-relative, while child prim positions are
                            // parent-relative. We want to apply parent-relative translations but not
                            // sim-relative ones
                            if (mesh.Prim.ParentID != 0)
                                pos += mesh.Prim.Position;

                            obj.AppendFormat("v {0} {1} {2}{3}", pos.X, pos.Y, pos.Z, Environment.NewLine);

                            #endregion Vertex

                            #region Texture Coord

                            obj.AppendFormat("vt {0} {1}{2}", vertex.TexCoord.X, vertex.TexCoord.Y,
                                Environment.NewLine);

                            #endregion Texture Coord

                            #region Vertex Normal

                            // HACK: Sometimes normals are getting set to <NaN,NaN,NaN>
                            if (!Single.IsNaN(vertex.Normal.X) && !Single.IsNaN(vertex.Normal.Y) && !Single.IsNaN(vertex.Normal.Z))
                                obj.AppendFormat("vn {0} {1} {2}{3}", vertex.Normal.X, vertex.Normal.Y, vertex.Normal.Z,
                                    Environment.NewLine);
                            else
                                obj.AppendLine("vn 0.0 1.0 0.0");

                            #endregion Vertex Normal
                        }

                        obj.AppendFormat("# {0} vertices{1}", face.Vertices.Count, Environment.NewLine);
                        obj.AppendLine();
                        obj.AppendLine("usemtl " + mtlName);

                        #region Elements

                        // Write all of the faces (triangles) for this side
                        for (int k = 0; k < face.Indices.Count / 3; k++)
                        {
                            obj.AppendFormat("f -{0}/-{0}/-{0} -{1}/-{1}/-{1} -{2}/-{2}/-{2}{3}",
                                face.Vertices.Count - face.Indices[k * 3 + 0],
                                face.Vertices.Count - face.Indices[k * 3 + 1],
                                face.Vertices.Count - face.Indices[k * 3 + 2],
                                Environment.NewLine);
                        }

                        obj.AppendFormat("# {0} elements{1}", face.Indices.Count / 3, Environment.NewLine);
                        obj.AppendLine();

                        #endregion Elements
                    }
                }
                primNr++;
            }

            try
            {
                File.WriteAllText(filename, obj.ToString());
                File.WriteAllText(mtlFilename, mtl.ToString());
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }

    public static class Math3D
    {
        // Column-major:
        // |  0  4  8 12 |
        // |  1  5  9 13 |
        // |  2  6 10 14 |
        // |  3  7 11 15 |

        public static float[] CreateTranslationMatrix(Vector3 v)
        {
            float[] mat = new float[16];

            mat[12] = v.X;
            mat[13] = v.Y;
            mat[14] = v.Z;
            mat[0] = mat[5] = mat[10] = mat[15] = 1;

            return mat;
        }

        public static float[] CreateRotationMatrix(Quaternion q)
        {
            float[] mat = new float[16];

            // Transpose the quaternion (don't ask me why)
            q.X = q.X * -1f;
            q.Y = q.Y * -1f;
            q.Z = q.Z * -1f;

            float x2 = q.X + q.X;
            float y2 = q.Y + q.Y;
            float z2 = q.Z + q.Z;
            float xx = q.X * x2;
            float xy = q.X * y2;
            float xz = q.X * z2;
            float yy = q.Y * y2;
            float yz = q.Y * z2;
            float zz = q.Z * z2;
            float wx = q.W * x2;
            float wy = q.W * y2;
            float wz = q.W * z2;

            mat[0] = 1.0f - (yy + zz);
            mat[1] = xy - wz;
            mat[2] = xz + wy;
            mat[3] = 0.0f;

            mat[4] = xy + wz;
            mat[5] = 1.0f - (xx + zz);
            mat[6] = yz - wx;
            mat[7] = 0.0f;

            mat[8] = xz - wy;
            mat[9] = yz + wx;
            mat[10] = 1.0f - (xx + yy);
            mat[11] = 0.0f;

            mat[12] = 0.0f;
            mat[13] = 0.0f;
            mat[14] = 0.0f;
            mat[15] = 1.0f;

            return mat;
        }

        public static float[] CreateScaleMatrix(Vector3 v)
        {
            float[] mat = new float[16];

            mat[0] = v.X;
            mat[5] = v.Y;
            mat[10] = v.Z;
            mat[15] = 1;

            return mat;
        }
    }

    public enum RenderPass
    {
        Picking,
        Simple,
        Alpha
    }
    #endregion Helper classes
}
