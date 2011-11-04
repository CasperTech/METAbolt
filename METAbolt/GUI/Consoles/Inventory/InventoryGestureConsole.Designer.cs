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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
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
            this.btnGesture.Text = "Previe&w";
            this.btnGesture.UseVisualStyleBackColor = false;
            this.btnGesture.Click += new System.EventHandler(this.btnGesture_Click);
            // 
            // button1
            // 
            this.button1.AccessibleName = "Play button";
            this.button1.BackColor = System.Drawing.Color.RoyalBlue;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(110, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Acti&vate";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.AccessibleName = "Play button";
            this.button2.BackColor = System.Drawing.Color.RoyalBlue;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(191, 17);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "De-activa&te";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // InventoryGestureConsol
            // 
            this.AccessibleName = "Gesture console";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnGesture);
            this.Name = "InventoryGestureConsol";
            this.Size = new System.Drawing.Size(272, 251);
            this.Load += new System.EventHandler(this.InventoryGestureConsol_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGesture;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}
