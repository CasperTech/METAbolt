namespace METAbolt
{
    partial class PrefAI
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
            this.label4 = new System.Windows.Forms.Label();
            this.picAI = new System.Windows.Forms.PictureBox();
            this.chkAI = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.chkReply = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picAI)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.DarkGray;
            this.label4.Location = new System.Drawing.Point(162, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "(requires METAbolt re-start)";
            // 
            // picAI
            // 
            this.picAI.Image = global::METAbolt.Properties.Resources.Help_and_Support_16;
            this.picAI.Location = new System.Drawing.Point(317, 28);
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
            this.chkAI.AccessibleName = "Enable METAbrain option";
            this.chkAI.AutoSize = true;
            this.chkAI.Location = new System.Drawing.Point(12, 28);
            this.chkAI.Name = "chkAI";
            this.chkAI.Size = new System.Drawing.Size(134, 17);
            this.chkAI.TabIndex = 0;
            this.chkAI.Text = "Enable METAbrain (AI)";
            this.chkAI.UseVisualStyleBackColor = true;
            this.chkAI.CheckedChanged += new System.EventHandler(this.chkAI_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.trackBar1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(12, 240);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(301, 100);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // trackBar1
            // 
            this.trackBar1.AutoSize = false;
            this.trackBar1.Location = new System.Drawing.Point(102, 49);
            this.trackBar1.Maximum = 20;
            this.trackBar1.Minimum = 5;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(104, 30);
            this.trackBar1.TabIndex = 19;
            this.trackBar1.TickFrequency = 5;
            this.trackBar1.Value = 5;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(43, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 16);
            this.label3.TabIndex = 18;
            this.label3.Text = "Chat range:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(25, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(181, 17);
            this.checkBox1.TabIndex = 17;
            this.checkBox1.Text = "Enable METAbrain in public chat";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AccessibleName = "METAbrain panel";
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.chkReply);
            this.panel1.Location = new System.Drawing.Point(28, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(285, 139);
            this.panel1.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.AccessibleName = "Reply message textbox";
            this.textBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox1.Location = new System.Drawing.Point(30, 40);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(239, 82);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "I am sorry but I didn\'t understand what you said or I haven\'t been taught a respo" +
                "nse for it. Can you try again, making sure your sentences are short and clear.";
            // 
            // chkReply
            // 
            this.chkReply.AccessibleName = "Reply option";
            this.chkReply.AutoSize = true;
            this.chkReply.Location = new System.Drawing.Point(9, 12);
            this.chkReply.Name = "chkReply";
            this.chkReply.Size = new System.Drawing.Size(245, 17);
            this.chkReply.TabIndex = 2;
            this.chkReply.Text = "Reply with below when an answer is not found";
            this.chkReply.UseVisualStyleBackColor = true;
            // 
            // PrefAI
            // 
            this.AccessibleName = "AI tab";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.picAI);
            this.Controls.Add(this.chkAI);
            this.Name = "PrefAI";
            this.Size = new System.Drawing.Size(344, 300);
            this.Load += new System.EventHandler(this.PrefAI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picAI)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox picAI;
        private System.Windows.Forms.CheckBox chkAI;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkReply;
        private System.Windows.Forms.TextBox textBox1;
    }
}
