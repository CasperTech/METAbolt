namespace METAbolt
{
    partial class Pref3D
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Pref3D));
            this.picAI = new System.Windows.Forms.PictureBox();
            this.chkAI = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picAI)).BeginInit();
            this.SuspendLayout();
            // 
            // picAI
            // 
            this.picAI.Image = ((System.Drawing.Image)(resources.GetObject("picAI.Image")));
            this.picAI.Location = new System.Drawing.Point(199, 72);
            this.picAI.Name = "picAI";
            this.picAI.Size = new System.Drawing.Size(15, 15);
            this.picAI.TabIndex = 17;
            this.picAI.TabStop = false;
            this.picAI.Click += new System.EventHandler(this.picAI_Click);
            this.picAI.MouseLeave += new System.EventHandler(this.picAI_MouseLeave);
            this.picAI.MouseHover += new System.EventHandler(this.picAI_MouseHover);
            // 
            // chkAI
            // 
            this.chkAI.AccessibleDescription = "Disables mipmaps for 3D on older graphics cards";
            this.chkAI.AccessibleName = "Disable Mipmaps";
            this.chkAI.AutoSize = true;
            this.chkAI.Location = new System.Drawing.Point(59, 71);
            this.chkAI.Name = "chkAI";
            this.chkAI.Size = new System.Drawing.Size(106, 17);
            this.chkAI.TabIndex = 0;
            this.chkAI.Text = "Disable Mipmaps";
            this.chkAI.UseVisualStyleBackColor = true;
            this.chkAI.CheckedChanged += new System.EventHandler(this.chkAI_CheckedChanged);
            // 
            // Pref3D
            // 
            this.AccessibleName = "AI tab";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.picAI);
            this.Controls.Add(this.chkAI);
            this.Name = "Pref3D";
            this.Size = new System.Drawing.Size(344, 300);
            this.Load += new System.EventHandler(this.PrefAI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picAI)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picAI;
        private System.Windows.Forms.CheckBox chkAI;
    }
}
