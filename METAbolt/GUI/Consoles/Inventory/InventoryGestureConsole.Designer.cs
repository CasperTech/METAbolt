namespace METAbolt
{
    partial class InventoryGestureConsol
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
            this.btnGesture = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGesture
            // 
            this.btnGesture.AccessibleName = "Play button";
            this.btnGesture.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnGesture.ForeColor = System.Drawing.Color.White;
            this.btnGesture.Location = new System.Drawing.Point(18, 17);
            this.btnGesture.Name = "btnGesture";
            this.btnGesture.Size = new System.Drawing.Size(75, 23);
            this.btnGesture.TabIndex = 0;
            this.btnGesture.Text = "Pla&y";
            this.btnGesture.UseVisualStyleBackColor = false;
            this.btnGesture.Click += new System.EventHandler(this.btnGesture_Click);
            // 
            // InventoryGestureConsol
            // 
            this.AccessibleName = "Gesture console";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnGesture);
            this.Name = "InventoryGestureConsol";
            this.Size = new System.Drawing.Size(272, 251);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGesture;
    }
}
