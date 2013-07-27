namespace METAbolt
{
    partial class META3D
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                TextureThreadContextReady.Close();
                Printer.Dispose();
                PendingTextures.Close();
                Prims.Clear();
                Textures.Clear();
                glControl.Context.Dispose(); 
                glControl.Dispose();
                renderer = null;
                GLMode = null;
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(META3D));
            this.scrollRoll = new System.Windows.Forms.HScrollBar();
            this.scrollPitch = new System.Windows.Forms.HScrollBar();
            this.scrollYaw = new System.Windows.Forms.HScrollBar();
            this.scrollZoom = new System.Windows.Forms.HScrollBar();
            this.gbZoom = new System.Windows.Forms.GroupBox();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.picAutoSit = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnResetView = new System.Windows.Forms.Button();
            this.cbAA = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ctxObjects = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.touchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.payBuyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.takeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.returnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gbZoom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAutoSit)).BeginInit();
            this.ctxObjects.SuspendLayout();
            this.SuspendLayout();
            // 
            // scrollRoll
            // 
            this.scrollRoll.Location = new System.Drawing.Point(435, 106);
            this.scrollRoll.Maximum = 360;
            this.scrollRoll.Name = "scrollRoll";
            this.scrollRoll.Size = new System.Drawing.Size(200, 16);
            this.scrollRoll.TabIndex = 9;
            this.scrollRoll.Visible = false;
            // 
            // scrollPitch
            // 
            this.scrollPitch.Location = new System.Drawing.Point(435, 75);
            this.scrollPitch.Maximum = 360;
            this.scrollPitch.Name = "scrollPitch";
            this.scrollPitch.Size = new System.Drawing.Size(200, 16);
            this.scrollPitch.TabIndex = 10;
            this.scrollPitch.Visible = false;
            // 
            // scrollYaw
            // 
            this.scrollYaw.Location = new System.Drawing.Point(435, 49);
            this.scrollYaw.Maximum = 360;
            this.scrollYaw.Name = "scrollYaw";
            this.scrollYaw.Size = new System.Drawing.Size(200, 16);
            this.scrollYaw.TabIndex = 11;
            this.scrollYaw.Value = 90;
            this.scrollYaw.Visible = false;
            // 
            // scrollZoom
            // 
            this.scrollZoom.LargeChange = 1;
            this.scrollZoom.Location = new System.Drawing.Point(435, 9);
            this.scrollZoom.Maximum = 0;
            this.scrollZoom.Minimum = -300;
            this.scrollZoom.Name = "scrollZoom";
            this.scrollZoom.Size = new System.Drawing.Size(200, 16);
            this.scrollZoom.TabIndex = 19;
            this.scrollZoom.Value = -30;
            this.scrollZoom.Visible = false;
            // 
            // gbZoom
            // 
            this.gbZoom.Controls.Add(this.button8);
            this.gbZoom.Controls.Add(this.button7);
            this.gbZoom.Controls.Add(this.button6);
            this.gbZoom.Controls.Add(this.button5);
            this.gbZoom.Controls.Add(this.button4);
            this.gbZoom.Controls.Add(this.button3);
            this.gbZoom.Controls.Add(this.button2);
            this.gbZoom.Controls.Add(this.btnClose);
            this.gbZoom.Controls.Add(this.picAutoSit);
            this.gbZoom.Controls.Add(this.button1);
            this.gbZoom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbZoom.Location = new System.Drawing.Point(0, 560);
            this.gbZoom.Name = "gbZoom";
            this.gbZoom.Size = new System.Drawing.Size(644, 45);
            this.gbZoom.TabIndex = 8;
            this.gbZoom.TabStop = false;
            // 
            // button8
            // 
            this.button8.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button8.BackColor = System.Drawing.Color.Transparent;
            this.button8.ForeColor = System.Drawing.Color.RoyalBlue;
            this.button8.Location = new System.Drawing.Point(228, 16);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(20, 20);
            this.button8.TabIndex = 47;
            this.button8.Text = "T";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            this.button8.MouseHover += new System.EventHandler(this.button8_MouseHover);
            // 
            // button7
            // 
            this.button7.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button7.BackColor = System.Drawing.Color.Yellow;
            this.button7.ForeColor = System.Drawing.Color.RoyalBlue;
            this.button7.Location = new System.Drawing.Point(208, 16);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(20, 20);
            this.button7.TabIndex = 46;
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            this.button7.MouseHover += new System.EventHandler(this.button7_MouseHover);
            // 
            // button6
            // 
            this.button6.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button6.BackColor = System.Drawing.Color.Green;
            this.button6.ForeColor = System.Drawing.Color.RoyalBlue;
            this.button6.Location = new System.Drawing.Point(188, 16);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(20, 20);
            this.button6.TabIndex = 45;
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            this.button6.MouseHover += new System.EventHandler(this.button6_MouseHover);
            // 
            // button5
            // 
            this.button5.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button5.BackColor = System.Drawing.Color.Red;
            this.button5.ForeColor = System.Drawing.Color.RoyalBlue;
            this.button5.Location = new System.Drawing.Point(167, 16);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(20, 20);
            this.button5.TabIndex = 44;
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            this.button5.MouseHover += new System.EventHandler(this.button5_MouseHover);
            // 
            // button4
            // 
            this.button4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button4.BackColor = System.Drawing.Color.Black;
            this.button4.ForeColor = System.Drawing.Color.RoyalBlue;
            this.button4.Location = new System.Drawing.Point(146, 16);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(20, 20);
            this.button4.TabIndex = 43;
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            this.button4.MouseHover += new System.EventHandler(this.button4_MouseHover);
            // 
            // button3
            // 
            this.button3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button3.BackColor = System.Drawing.Color.White;
            this.button3.ForeColor = System.Drawing.Color.RoyalBlue;
            this.button3.Location = new System.Drawing.Point(125, 16);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(20, 20);
            this.button3.TabIndex = 42;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            this.button3.MouseHover += new System.EventHandler(this.button3_MouseHover);
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button2.BackColor = System.Drawing.Color.RoyalBlue;
            this.button2.ForeColor = System.Drawing.Color.RoyalBlue;
            this.button2.Location = new System.Drawing.Point(104, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(20, 20);
            this.button2.TabIndex = 41;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            this.button2.MouseHover += new System.EventHandler(this.button2_MouseHover);
            // 
            // btnClose
            // 
            this.btnClose.AccessibleName = "Close this window button";
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.DimGray;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(587, 14);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(51, 23);
            this.btnClose.TabIndex = 40;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // picAutoSit
            // 
            this.picAutoSit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.picAutoSit.Image = ((System.Drawing.Image)(resources.GetObject("picAutoSit.Image")));
            this.picAutoSit.Location = new System.Drawing.Point(6, 19);
            this.picAutoSit.Name = "picAutoSit";
            this.picAutoSit.Size = new System.Drawing.Size(15, 15);
            this.picAutoSit.TabIndex = 39;
            this.picAutoSit.TabStop = false;
            this.picAutoSit.Click += new System.EventHandler(this.picAutoSit_Click);
            this.picAutoSit.MouseLeave += new System.EventHandler(this.picAutoSit_MouseLeave);
            this.picAutoSit.MouseHover += new System.EventHandler(this.picAutoSit_MouseHover);
            // 
            // button1
            // 
            this.button1.AccessibleDescription = "Takes a snapshot of tje displayed object on to your hard drive";
            this.button1.AccessibleName = "Snapshot";
            this.button1.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.BackgroundImage = global::METAbolt.Properties.Resources.camera;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Location = new System.Drawing.Point(298, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(32, 32);
            this.button1.TabIndex = 24;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.MouseHover += new System.EventHandler(this.button1_MouseHover);
            // 
            // btnResetView
            // 
            this.btnResetView.Location = new System.Drawing.Point(437, 158);
            this.btnResetView.Name = "btnResetView";
            this.btnResetView.Size = new System.Drawing.Size(94, 23);
            this.btnResetView.TabIndex = 22;
            this.btnResetView.Text = "Reset View";
            this.btnResetView.UseVisualStyleBackColor = true;
            this.btnResetView.Visible = false;
            // 
            // cbAA
            // 
            this.cbAA.AutoSize = true;
            this.cbAA.Checked = true;
            this.cbAA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAA.Location = new System.Drawing.Point(435, 135);
            this.cbAA.Name = "cbAA";
            this.cbAA.Size = new System.Drawing.Size(82, 17);
            this.cbAA.TabIndex = 21;
            this.cbAA.Text = "Anti-aliasing";
            this.cbAA.UseVisualStyleBackColor = true;
            this.cbAA.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(398, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Zoom";
            this.label1.Visible = false;
            // 
            // ctxObjects
            // 
            this.ctxObjects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.touchToolStripMenuItem,
            this.sitToolStripMenuItem,
            this.toolStripMenuItem1,
            this.payBuyToolStripMenuItem,
            this.toolStripSeparator1,
            this.takeToolStripMenuItem,
            this.returnToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.ctxObjects.Name = "ctxObjects";
            this.ctxObjects.Size = new System.Drawing.Size(119, 148);
            this.ctxObjects.Opening += new System.ComponentModel.CancelEventHandler(this.ctxObjects_Opening);
            // 
            // touchToolStripMenuItem
            // 
            this.touchToolStripMenuItem.Name = "touchToolStripMenuItem";
            this.touchToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.touchToolStripMenuItem.Text = "Touch";
            // 
            // sitToolStripMenuItem
            // 
            this.sitToolStripMenuItem.Name = "sitToolStripMenuItem";
            this.sitToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.sitToolStripMenuItem.Text = "Sit";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(115, 6);
            // 
            // payBuyToolStripMenuItem
            // 
            this.payBuyToolStripMenuItem.Name = "payBuyToolStripMenuItem";
            this.payBuyToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.payBuyToolStripMenuItem.Text = "Pay/Buy";
            this.payBuyToolStripMenuItem.Click += new System.EventHandler(this.payBuyToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(115, 6);
            // 
            // takeToolStripMenuItem
            // 
            this.takeToolStripMenuItem.Name = "takeToolStripMenuItem";
            this.takeToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.takeToolStripMenuItem.Text = "Take";
            // 
            // returnToolStripMenuItem
            // 
            this.returnToolStripMenuItem.Name = "returnToolStripMenuItem";
            this.returnToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.returnToolStripMenuItem.Text = "Return";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "jpg";
            // 
            // META3D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(644, 605);
            this.Controls.Add(this.gbZoom);
            this.Controls.Add(this.btnResetView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.scrollZoom);
            this.Controls.Add(this.cbAA);
            this.Controls.Add(this.scrollYaw);
            this.Controls.Add(this.scrollRoll);
            this.Controls.Add(this.scrollPitch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "META3D";
            this.ShowInTaskbar = false;
            this.Text = "META3D";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.META3D_FormClosing);
            this.Shown += new System.EventHandler(this.META3D_Shown);
            this.gbZoom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picAutoSit)).EndInit();
            this.ctxObjects.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.HScrollBar scrollRoll;
        public System.Windows.Forms.HScrollBar scrollPitch;
        public System.Windows.Forms.HScrollBar scrollYaw;
        public System.Windows.Forms.HScrollBar scrollZoom;
        public System.Windows.Forms.GroupBox gbZoom;
        public System.Windows.Forms.ContextMenuStrip ctxObjects;
        public System.Windows.Forms.CheckBox cbAA;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button btnResetView;
        public System.Windows.Forms.ToolStripMenuItem touchToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem sitToolStripMenuItem;
        public System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem takeToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem returnToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.PictureBox picAutoSit;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripMenuItem payBuyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.ToolTip toolTip1;

    }
}

