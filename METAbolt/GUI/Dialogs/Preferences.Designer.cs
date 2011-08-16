namespace METAbolt
{
    partial class frmPreferences
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPreferences));
            this.lbxPanes = new System.Windows.Forms.ListBox();
            this.pnlPanes = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.picAutoSit = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picAutoSit)).BeginInit();
            this.SuspendLayout();
            // 
            // lbxPanes
            // 
            this.lbxPanes.BackColor = System.Drawing.Color.White;
            this.lbxPanes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbxPanes.IntegralHeight = false;
            this.lbxPanes.ItemHeight = 36;
            this.lbxPanes.Location = new System.Drawing.Point(12, 12);
            this.lbxPanes.Name = "lbxPanes";
            this.lbxPanes.Size = new System.Drawing.Size(128, 300);
            this.lbxPanes.TabIndex = 0;
            this.lbxPanes.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbxPanes_DrawItem);
            this.lbxPanes.SelectedIndexChanged += new System.EventHandler(this.lbxPanes_SelectedIndexChanged);
            // 
            // pnlPanes
            // 
            this.pnlPanes.AllowDrop = true;
            this.pnlPanes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlPanes.Location = new System.Drawing.Point(146, 12);
            this.pnlPanes.Name = "pnlPanes";
            this.pnlPanes.Size = new System.Drawing.Size(344, 309);
            this.pnlPanes.TabIndex = 1;
            this.pnlPanes.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlPanes_Paint);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleName = "OK and close window button";
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(255, 332);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "O&K";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleName = "Cancel and close window button";
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(336, 332);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.AccessibleName = "Apply changes button";
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnApply.ForeColor = System.Drawing.Color.White;
            this.btnApply.Location = new System.Drawing.Point(417, 332);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "&Apply";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // picAutoSit
            // 
            this.picAutoSit.Image = ((System.Drawing.Image)(resources.GetObject("picAutoSit.Image")));
            this.picAutoSit.Location = new System.Drawing.Point(12, 340);
            this.picAutoSit.Name = "picAutoSit";
            this.picAutoSit.Size = new System.Drawing.Size(15, 15);
            this.picAutoSit.TabIndex = 39;
            this.picAutoSit.TabStop = false;
            this.picAutoSit.Click += new System.EventHandler(this.picAutoSit_Click);
            this.picAutoSit.MouseLeave += new System.EventHandler(this.picAutoSit_MouseLeave);
            this.picAutoSit.MouseHover += new System.EventHandler(this.picAutoSit_MouseHover);
            // 
            // frmPreferences
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(502, 362);
            this.Controls.Add(this.picAutoSit);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pnlPanes);
            this.Controls.Add(this.lbxPanes);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPreferences";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "METAbolt Preferences";
            this.Load += new System.EventHandler(this.frmPreferences_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picAutoSit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbxPanes;
        private System.Windows.Forms.Panel pnlPanes;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.PictureBox picAutoSit;
    }
}