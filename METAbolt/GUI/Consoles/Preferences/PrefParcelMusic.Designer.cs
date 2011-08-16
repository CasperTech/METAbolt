namespace METAbolt
{
    partial class PrefParcelMusic
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkParcelMusic = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkParcelMusic);
            this.groupBox2.Location = new System.Drawing.Point(3, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(338, 48);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "METAplayer";
            // 
            // chkParcelMusic
            // 
            this.chkParcelMusic.AutoSize = true;
            this.chkParcelMusic.Location = new System.Drawing.Point(6, 20);
            this.chkParcelMusic.Name = "chkParcelMusic";
            this.chkParcelMusic.Size = new System.Drawing.Size(67, 17);
            this.chkParcelMusic.TabIndex = 1;
            this.chkParcelMusic.Text = "Disabled";
            this.chkParcelMusic.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(304, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "* Turning METAplayer on/off requires METAbolt to be restarted";
            // 
            // PrefParcelMusic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Name = "PrefParcelMusic";
            this.Size = new System.Drawing.Size(344, 300);
            this.Load += new System.EventHandler(this.PrefParcelMusic_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkParcelMusic;
        private System.Windows.Forms.Label label1;
    }
}
