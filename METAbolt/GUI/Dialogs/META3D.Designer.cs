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
            this.btnClose = new System.Windows.Forms.Button();
            this.picAutoSit = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnResetView = new System.Windows.Forms.Button();
            this.cbAA = new System.Windows.Forms.CheckBox();
            this.chkWireFrame = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ctxObjects = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.touchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.takeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.returnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
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
            // btnClose
            // 
            this.btnClose.AccessibleName = "Close this window button";
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.RoyalBlue;
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
            this.picAutoSit.Image = global::METAbolt.Properties.Resources.Help_and_Support_16;
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
            // chkWireFrame
            // 
            this.chkWireFrame.AutoSize = true;
            this.chkWireFrame.Location = new System.Drawing.Point(539, 135);
            this.chkWireFrame.Name = "chkWireFrame";
            this.chkWireFrame.Size = new System.Drawing.Size(74, 17);
            this.chkWireFrame.TabIndex = 21;
            this.chkWireFrame.Text = "Wireframe";
            this.chkWireFrame.UseVisualStyleBackColor = true;
            this.chkWireFrame.Visible = false;
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
            this.takeToolStripMenuItem,
            this.returnToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.ctxObjects.Name = "ctxObjects";
            this.ctxObjects.Size = new System.Drawing.Size(110, 120);
            // 
            // touchToolStripMenuItem
            // 
            this.touchToolStripMenuItem.Name = "touchToolStripMenuItem";
            this.touchToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.touchToolStripMenuItem.Text = "Touch";
            // 
            // sitToolStripMenuItem
            // 
            this.sitToolStripMenuItem.Name = "sitToolStripMenuItem";
            this.sitToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.sitToolStripMenuItem.Text = "Sit";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(106, 6);
            // 
            // takeToolStripMenuItem
            // 
            this.takeToolStripMenuItem.Name = "takeToolStripMenuItem";
            this.takeToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.takeToolStripMenuItem.Text = "Take";
            // 
            // returnToolStripMenuItem
            // 
            this.returnToolStripMenuItem.Name = "returnToolStripMenuItem";
            this.returnToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.returnToolStripMenuItem.Text = "Return";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
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
            this.ClientSize = new System.Drawing.Size(644, 605);
            this.Controls.Add(this.gbZoom);
            this.Controls.Add(this.btnResetView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.scrollZoom);
            this.Controls.Add(this.chkWireFrame);
            this.Controls.Add(this.cbAA);
            this.Controls.Add(this.scrollYaw);
            this.Controls.Add(this.scrollRoll);
            this.Controls.Add(this.scrollPitch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "META3D";
            this.Text = "META3D";
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
        public System.Windows.Forms.CheckBox chkWireFrame;
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

    }
}

