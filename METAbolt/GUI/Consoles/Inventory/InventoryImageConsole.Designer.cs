namespace METAbolt
{
    partial class InventoryImageConsole
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblStatus = new System.Windows.Forms.Label();
            this.proActivity = new System.Windows.Forms.ProgressBar();
            this.pbxImage = new System.Windows.Forms.PictureBox();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.pnlOptions = new System.Windows.Forms.Panel();
            this.btnView = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.sfdImage = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pbxImage)).BeginInit();
            this.pnlStatus.SuspendLayout();
            this.pnlOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(3, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(126, 13);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Downloading image...";
            // 
            // proActivity
            // 
            this.proActivity.AccessibleName = "Download progress bar";
            this.proActivity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.proActivity.Location = new System.Drawing.Point(3, 16);
            this.proActivity.MarqueeAnimationSpeed = 50;
            this.proActivity.Name = "proActivity";
            this.proActivity.Size = new System.Drawing.Size(294, 16);
            this.proActivity.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.proActivity.TabIndex = 4;
            // 
            // pbxImage
            // 
            this.pbxImage.AccessibleName = "Image display";
            this.pbxImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbxImage.Location = new System.Drawing.Point(23, 48);
            this.pbxImage.Name = "pbxImage";
            this.pbxImage.Size = new System.Drawing.Size(261, 226);
            this.pbxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxImage.TabIndex = 0;
            this.pbxImage.TabStop = false;
            this.pbxImage.Click += new System.EventHandler(this.pbxImage_Click);
            // 
            // pnlStatus
            // 
            this.pnlStatus.AccessibleName = "Download status panel";
            this.pnlStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlStatus.Controls.Add(this.lblStatus);
            this.pnlStatus.Controls.Add(this.proActivity);
            this.pnlStatus.Location = new System.Drawing.Point(4, 5);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(300, 42);
            this.pnlStatus.TabIndex = 3;
            this.pnlStatus.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlStatus_Paint);
            // 
            // pnlOptions
            // 
            this.pnlOptions.AccessibleName = "Image options panel";
            this.pnlOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlOptions.Controls.Add(this.btnView);
            this.pnlOptions.Controls.Add(this.label1);
            this.pnlOptions.Controls.Add(this.btnSave);
            this.pnlOptions.Location = new System.Drawing.Point(3, 3);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Size = new System.Drawing.Size(300, 42);
            this.pnlOptions.TabIndex = 0;
            this.pnlOptions.Visible = false;
            // 
            // btnView
            // 
            this.btnView.AccessibleName = "View image button";
            this.btnView.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnView.ForeColor = System.Drawing.Color.White;
            this.btnView.Location = new System.Drawing.Point(112, 16);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(75, 23);
            this.btnView.TabIndex = 1;
            this.btnView.Text = "&View";
            this.btnView.UseVisualStyleBackColor = false;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Image Options";
            // 
            // btnSave
            // 
            this.btnSave.AccessibleName = "Save button";
            this.btnSave.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnSave.Enabled = false;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(3, 16);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(103, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save to &disk";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // sfdImage
            // 
            this.sfdImage.Filter = "Bitmap (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|Portable Network Graphics (*.png)|*.png|A" +
                "ll files (*.*)|*.*";
            this.sfdImage.Title = "Save Image";
            // 
            // InventoryImageConsole
            // 
            this.AccessibleName = "Image console";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.pbxImage);
            this.Controls.Add(this.pnlOptions);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "InventoryImageConsole";
            this.Size = new System.Drawing.Size(306, 305);
            this.Load += new System.EventHandler(this.InventoryImageConsole_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbxImage)).EndInit();
            this.pnlStatus.ResumeLayout(false);
            this.pnlStatus.PerformLayout();
            this.pnlOptions.ResumeLayout(false);
            this.pnlOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar proActivity;
        private System.Windows.Forms.PictureBox pbxImage;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Panel pnlOptions;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SaveFileDialog sfdImage;
        private System.Windows.Forms.Button btnView;
    }
}
