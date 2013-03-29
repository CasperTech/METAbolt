namespace METAbolt
{
    partial class InventoryAnimationConsole
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
            this.btnAnimate = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAnimate
            // 
            this.btnAnimate.AccessibleName = "Animate button";
            this.btnAnimate.BackColor = System.Drawing.Color.DimGray;
            this.btnAnimate.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnAnimate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.btnAnimate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnimate.ForeColor = System.Drawing.Color.White;
            this.btnAnimate.Location = new System.Drawing.Point(10, 12);
            this.btnAnimate.Name = "btnAnimate";
            this.btnAnimate.Size = new System.Drawing.Size(75, 23);
            this.btnAnimate.TabIndex = 0;
            this.btnAnimate.Text = "An&imate";
            this.btnAnimate.UseVisualStyleBackColor = false;
            this.btnAnimate.Click += new System.EventHandler(this.btnAnimate_Click);
            // 
            // btnStop
            // 
            this.btnStop.AccessibleName = "Stop animation button";
            this.btnStop.BackColor = System.Drawing.Color.DimGray;
            this.btnStop.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Location = new System.Drawing.Point(91, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Sto&p";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // InventoryAnimationConsole
            // 
            this.AccessibleName = "Animation console";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnAnimate);
            this.Controls.Add(this.btnStop);
            this.Name = "InventoryAnimationConsole";
            this.Size = new System.Drawing.Size(293, 281);
            this.Load += new System.EventHandler(this.InventoryAnimationConsole_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAnimate;
        private System.Windows.Forms.Button btnStop;
    }
}
