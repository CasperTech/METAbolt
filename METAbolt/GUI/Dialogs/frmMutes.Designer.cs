namespace METAbolt
{
    partial class frmMutes
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
            this.GW = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.picHelp = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.GW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHelp)).BeginInit();
            this.SuspendLayout();
            // 
            // GW
            // 
            this.GW.AllowUserToResizeRows = false;
            this.GW.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.GW.BackgroundColor = System.Drawing.Color.White;
            this.GW.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GW.Location = new System.Drawing.Point(2, 2);
            this.GW.MultiSelect = false;
            this.GW.Name = "GW";
            this.GW.Size = new System.Drawing.Size(466, 293);
            this.GW.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.AccessibleName = "OK button to close the window";
            this.button1.BackColor = System.Drawing.Color.RoyalBlue;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(393, 301);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "&OK";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // picHelp
            // 
            this.picHelp.Image = global::METAbolt.Properties.Resources.Help_and_Support_16;
            this.picHelp.Location = new System.Drawing.Point(2, 303);
            this.picHelp.Name = "picHelp";
            this.picHelp.Size = new System.Drawing.Size(15, 15);
            this.picHelp.TabIndex = 39;
            this.picHelp.TabStop = false;
            this.picHelp.MouseLeave += new System.EventHandler(this.picHelp_MouseLeave);
            this.picHelp.MouseHover += new System.EventHandler(this.picHelp_MouseHover);
            // 
            // frmMutes
            // 
            this.AccessibleName = "Mute list window";
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(470, 330);
            this.ControlBox = false;
            this.Controls.Add(this.picHelp);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.GW);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMutes";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Mute List";
            this.Load += new System.EventHandler(this.frmMutes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHelp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView GW;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox picHelp;
    }
}