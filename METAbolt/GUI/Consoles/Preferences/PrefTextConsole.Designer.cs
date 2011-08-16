namespace METAbolt
{
    partial class PrefTextConsole
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrefTextConsole));
            this.chkIMTimestamps = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkGroupNotices = new System.Windows.Forms.CheckBox();
            this.chkGIMs = new System.Windows.Forms.CheckBox();
            this.chkSound = new System.Windows.Forms.CheckBox();
            this.chkSLT = new System.Windows.Forms.CheckBox();
            this.chkChatTimestamps = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkSmileys = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.nud = new System.Windows.Forms.NumericUpDown();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtDir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkChat = new System.Windows.Forms.CheckBox();
            this.chkIMs = new System.Windows.Forms.CheckBox();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkIMTimestamps
            // 
            this.chkIMTimestamps.AccessibleName = "IM timestamp option";
            this.chkIMTimestamps.AutoSize = true;
            this.chkIMTimestamps.Location = new System.Drawing.Point(6, 32);
            this.chkIMTimestamps.Name = "chkIMTimestamps";
            this.chkIMTimestamps.Size = new System.Drawing.Size(145, 17);
            this.chkIMTimestamps.TabIndex = 1;
            this.chkIMTimestamps.Text = "Enable timestamps in IMs";
            this.chkIMTimestamps.UseVisualStyleBackColor = true;
            this.chkIMTimestamps.CheckedChanged += new System.EventHandler(this.chkIMTimestamps_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.AccessibleName = "Messaging panel";
            this.groupBox2.Controls.Add(this.chkGroupNotices);
            this.groupBox2.Controls.Add(this.chkGIMs);
            this.groupBox2.Controls.Add(this.chkSound);
            this.groupBox2.Controls.Add(this.chkSLT);
            this.groupBox2.Controls.Add(this.chkIMTimestamps);
            this.groupBox2.Controls.Add(this.chkChatTimestamps);
            this.groupBox2.Location = new System.Drawing.Point(3, -2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(193, 114);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Messaging";
            // 
            // chkGroupNotices
            // 
            this.chkGroupNotices.AccessibleName = "Group notices option";
            this.chkGroupNotices.AutoSize = true;
            this.chkGroupNotices.Location = new System.Drawing.Point(6, 92);
            this.chkGroupNotices.Name = "chkGroupNotices";
            this.chkGroupNotices.Size = new System.Drawing.Size(132, 17);
            this.chkGroupNotices.TabIndex = 5;
            this.chkGroupNotices.Text = "Disable Group Notices";
            this.chkGroupNotices.UseVisualStyleBackColor = true;
            // 
            // chkGIMs
            // 
            this.chkGIMs.AccessibleName = "Group IMs option";
            this.chkGIMs.AutoSize = true;
            this.chkGIMs.Location = new System.Drawing.Point(6, 77);
            this.chkGIMs.Name = "chkGIMs";
            this.chkGIMs.Size = new System.Drawing.Size(113, 17);
            this.chkGIMs.TabIndex = 4;
            this.chkGIMs.Text = "Disable Group IMs";
            this.chkGIMs.UseVisualStyleBackColor = true;
            this.chkGIMs.CheckedChanged += new System.EventHandler(this.chkGIMs_CheckedChanged);
            // 
            // chkSound
            // 
            this.chkSound.AccessibleName = "Play sound option";
            this.chkSound.AutoSize = true;
            this.chkSound.Location = new System.Drawing.Point(6, 62);
            this.chkSound.Name = "chkSound";
            this.chkSound.Size = new System.Drawing.Size(182, 17);
            this.chkSound.TabIndex = 3;
            this.chkSound.Text = "Play sound on notifications/alerts";
            this.chkSound.UseVisualStyleBackColor = true;
            // 
            // chkSLT
            // 
            this.chkSLT.AccessibleName = "Use SLT option";
            this.chkSLT.AutoSize = true;
            this.chkSLT.Location = new System.Drawing.Point(6, 47);
            this.chkSLT.Name = "chkSLT";
            this.chkSLT.Size = new System.Drawing.Size(134, 17);
            this.chkSLT.TabIndex = 2;
            this.chkSLT.Text = "Use SLT in timestamps";
            this.chkSLT.UseVisualStyleBackColor = true;
            this.chkSLT.CheckedChanged += new System.EventHandler(this.chkSLT_CheckedChanged);
            // 
            // chkChatTimestamps
            // 
            this.chkChatTimestamps.AccessibleName = "Chat timestamp option";
            this.chkChatTimestamps.AutoSize = true;
            this.chkChatTimestamps.Location = new System.Drawing.Point(6, 17);
            this.chkChatTimestamps.Name = "chkChatTimestamps";
            this.chkChatTimestamps.Size = new System.Drawing.Size(149, 17);
            this.chkChatTimestamps.TabIndex = 0;
            this.chkChatTimestamps.Text = "Enable timestamps in chat";
            this.chkChatTimestamps.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.AccessibleName = "Smileys panel";
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.chkSmileys);
            this.groupBox3.Location = new System.Drawing.Point(199, 59);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(142, 53);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Smileys";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.DarkGray;
            this.label4.Location = new System.Drawing.Point(21, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "(requires re-start)";
            // 
            // chkSmileys
            // 
            this.chkSmileys.AccessibleName = "Smileys option";
            this.chkSmileys.AutoSize = true;
            this.chkSmileys.Location = new System.Drawing.Point(6, 19);
            this.chkSmileys.Name = "chkSmileys";
            this.chkSmileys.Size = new System.Drawing.Size(97, 17);
            this.chkSmileys.TabIndex = 0;
            this.chkSmileys.Text = "Disable smileys";
            this.chkSmileys.UseVisualStyleBackColor = true;
            this.chkSmileys.CheckedChanged += new System.EventHandler(this.chkSmileys_CheckedChanged_1);
            // 
            // groupBox4
            // 
            this.groupBox4.AccessibleName = "Chat buffer panel";
            this.groupBox4.Controls.Add(this.nud);
            this.groupBox4.Controls.Add(this.pictureBox1);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Location = new System.Drawing.Point(199, -1);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(142, 60);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Chat buffer";
            // 
            // nud
            // 
            this.nud.AccessibleDescription = "Select the number of lines";
            this.nud.AccessibleName = "Interval setting";
            this.nud.BackColor = System.Drawing.Color.WhiteSmoke;
            this.nud.Location = new System.Drawing.Point(70, 20);
            this.nud.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nud.Name = "nud";
            this.nud.Size = new System.Drawing.Size(48, 21);
            this.nud.TabIndex = 0;
            this.nud.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(125, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(15, 15);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            this.pictureBox1.MouseHover += new System.EventHandler(this.pictureBox1_MouseHover);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "lines (0 = unrestricted)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Clear every ";
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleName = "IM busy reponse panel";
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(6, 215);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(338, 82);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "IM busy response";
            // 
            // textBox1
            // 
            this.textBox1.AccessibleName = "Response message textbox";
            this.textBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox1.Location = new System.Drawing.Point(31, 20);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(298, 56);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "The Resident you messaged is in \'busy mode\' which means they have requested not t" +
                "o be disturbed.  Your message will still be shown in their IM panel for later vi" +
                "ewing.";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.AccessibleName = "Logging options panel";
            this.groupBox5.Controls.Add(this.button2);
            this.groupBox5.Controls.Add(this.button1);
            this.groupBox5.Controls.Add(this.txtDir);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.chkChat);
            this.groupBox5.Controls.Add(this.chkIMs);
            this.groupBox5.Location = new System.Drawing.Point(6, 118);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(335, 91);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Logging Options";
            // 
            // button2
            // 
            this.button2.AccessibleName = "View log files in Microsoft Explorer button";
            this.button2.Location = new System.Drawing.Point(283, 62);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(50, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "View";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.AccessibleName = "Change the log folder path button";
            this.button1.Location = new System.Drawing.Point(223, 62);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Change";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtDir
            // 
            this.txtDir.AccessibleName = "Patch to log folder textbox";
            this.txtDir.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtDir.Location = new System.Drawing.Point(45, 64);
            this.txtDir.Name = "txtDir";
            this.txtDir.Size = new System.Drawing.Size(176, 21);
            this.txtDir.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Path:";
            // 
            // chkChat
            // 
            this.chkChat.AccessibleName = "Use log for chat option";
            this.chkChat.AutoSize = true;
            this.chkChat.Location = new System.Drawing.Point(11, 44);
            this.chkChat.Name = "chkChat";
            this.chkChat.Size = new System.Drawing.Size(221, 17);
            this.chkChat.TabIndex = 1;
            this.chkChat.Text = "Save a log of Local Chat on my computer";
            this.chkChat.UseVisualStyleBackColor = true;
            // 
            // chkIMs
            // 
            this.chkIMs.AccessibleName = "Use log for IMs option";
            this.chkIMs.AutoSize = true;
            this.chkIMs.Checked = true;
            this.chkIMs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIMs.Location = new System.Drawing.Point(11, 21);
            this.chkIMs.Name = "chkIMs";
            this.chkIMs.Size = new System.Drawing.Size(187, 17);
            this.chkIMs.TabIndex = 0;
            this.chkIMs.Text = "Save a log of IMs on my computer";
            this.chkIMs.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.AccessibleName = "IM initial reponse panel";
            this.groupBox6.Controls.Add(this.textBox2);
            this.groupBox6.Location = new System.Drawing.Point(6, 303);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(338, 82);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "IM initial response";
            // 
            // textBox2
            // 
            this.textBox2.AccessibleName = "Initial message textbox";
            this.textBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox2.Location = new System.Drawing.Point(31, 20);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(298, 56);
            this.textBox2.TabIndex = 0;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // PrefTextConsole
            // 
            this.AccessibleName = "text tab";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PrefTextConsole";
            this.Size = new System.Drawing.Size(344, 397);
            this.Load += new System.EventHandler(this.PrefTextConsole_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkIMTimestamps;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkChatTimestamps;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkSmileys;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nud;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox chkSLT;
        private System.Windows.Forms.CheckBox chkSound;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox chkIMs;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkChat;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.CheckBox chkGroupNotices;
        private System.Windows.Forms.CheckBox chkGIMs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox textBox2;
    }
}
