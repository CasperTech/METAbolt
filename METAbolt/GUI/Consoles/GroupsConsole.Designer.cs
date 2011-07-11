namespace METAbolt
{
    partial class GroupsConsole
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupsConsole));
            this.cmdInfo = new System.Windows.Forms.Button();
            this.cmdActivate = new System.Windows.Forms.Button();
            this.cmdIM = new System.Windows.Forms.Button();
            this.cmdLeave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lstGroups = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdInfo
            // 
            this.cmdInfo.AccessibleDescription = "Display group information window";
            this.cmdInfo.AccessibleName = "Information";
            this.cmdInfo.BackColor = System.Drawing.Color.RoyalBlue;
            this.cmdInfo.Enabled = false;
            this.cmdInfo.ForeColor = System.Drawing.Color.White;
            this.cmdInfo.Location = new System.Drawing.Point(271, 408);
            this.cmdInfo.Name = "cmdInfo";
            this.cmdInfo.Size = new System.Drawing.Size(43, 23);
            this.cmdInfo.TabIndex = 3;
            this.cmdInfo.Text = "Info";
            this.cmdInfo.UseVisualStyleBackColor = false;
            this.cmdInfo.Click += new System.EventHandler(this.cmdInfo_Click_1);
            // 
            // cmdActivate
            // 
            this.cmdActivate.AccessibleDescription = "Activate selected group i.e. wear as tag";
            this.cmdActivate.AccessibleName = "Activate";
            this.cmdActivate.BackColor = System.Drawing.Color.RoyalBlue;
            this.cmdActivate.Enabled = false;
            this.cmdActivate.ForeColor = System.Drawing.Color.White;
            this.cmdActivate.Location = new System.Drawing.Point(195, 408);
            this.cmdActivate.Name = "cmdActivate";
            this.cmdActivate.Size = new System.Drawing.Size(70, 23);
            this.cmdActivate.TabIndex = 2;
            this.cmdActivate.Text = "Activate";
            this.cmdActivate.UseVisualStyleBackColor = false;
            this.cmdActivate.Click += new System.EventHandler(this.cmdActivate_Click);
            // 
            // cmdIM
            // 
            this.cmdIM.AccessibleDescription = "IM selected group";
            this.cmdIM.AccessibleName = "IM";
            this.cmdIM.BackColor = System.Drawing.Color.RoyalBlue;
            this.cmdIM.Enabled = false;
            this.cmdIM.ForeColor = System.Drawing.Color.White;
            this.cmdIM.Location = new System.Drawing.Point(136, 408);
            this.cmdIM.Name = "cmdIM";
            this.cmdIM.Size = new System.Drawing.Size(53, 23);
            this.cmdIM.TabIndex = 1;
            this.cmdIM.Text = "IM";
            this.cmdIM.UseVisualStyleBackColor = false;
            this.cmdIM.Click += new System.EventHandler(this.cmdCreate_Click);
            // 
            // cmdLeave
            // 
            this.cmdLeave.AccessibleDescription = "Leave this group";
            this.cmdLeave.AccessibleName = "Leave";
            this.cmdLeave.BackColor = System.Drawing.Color.RoyalBlue;
            this.cmdLeave.Enabled = false;
            this.cmdLeave.ForeColor = System.Drawing.Color.White;
            this.cmdLeave.Location = new System.Drawing.Point(320, 408);
            this.cmdLeave.Name = "cmdLeave";
            this.cmdLeave.Size = new System.Drawing.Size(54, 23);
            this.cmdLeave.TabIndex = 4;
            this.cmdLeave.Text = "Leave";
            this.cmdLeave.UseVisualStyleBackColor = false;
            this.cmdLeave.Click += new System.EventHandler(this.cmdLeave_Click);
            // 
            // label1
            // 
            this.label1.AccessibleDescription = "Currently active/worn group";
            this.label1.AccessibleName = "Current group";
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(136, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 15;
            // 
            // button1
            // 
            this.button1.AccessibleDescription = "Create a new group";
            this.button1.AccessibleName = "Create";
            this.button1.BackColor = System.Drawing.Color.RoyalBlue;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(440, 408);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Create Group";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Name of new group:";
            // 
            // textBox1
            // 
            this.textBox1.AccessibleDescription = "Name of the new group to be created";
            this.textBox1.AccessibleName = "Group Name";
            this.textBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox1.Location = new System.Drawing.Point(25, 43);
            this.textBox1.MaxLength = 35;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(330, 20);
            this.textBox1.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Group Charter/Description:";
            // 
            // textBox2
            // 
            this.textBox2.AccessibleDescription = "Description of the new group";
            this.textBox2.AccessibleName = "Charter";
            this.textBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox2.Location = new System.Drawing.Point(25, 120);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(330, 134);
            this.textBox2.TabIndex = 11;
            // 
            // button2
            // 
            this.button2.AccessibleDescription = "Create the new group";
            this.button2.AccessibleName = "Create group";
            this.button2.BackColor = System.Drawing.Color.RoyalBlue;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(209, 260);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(70, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "Create";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.AccessibleName = "Cancel";
            this.button3.BackColor = System.Drawing.Color.RoyalBlue;
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(285, 260);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(70, 23);
            this.button3.TabIndex = 13;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Silver;
            this.label4.Location = new System.Drawing.Point(22, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(197, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "(minimum 4 and maximum 35 characters)";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(138, 99);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(384, 294);
            this.panel1.TabIndex = 7;
            this.panel1.Visible = false;
            // 
            // lstGroups
            // 
            this.lstGroups.AccessibleDescription = "List og groups your avatar is a member of";
            this.lstGroups.AccessibleName = "Groups";
            this.lstGroups.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lstGroups.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstGroups.FormattingEnabled = true;
            this.lstGroups.Location = new System.Drawing.Point(0, 6);
            this.lstGroups.Name = "lstGroups";
            this.lstGroups.Size = new System.Drawing.Size(384, 286);
            this.lstGroups.TabIndex = 0;
            this.lstGroups.SelectedIndexChanged += new System.EventHandler(this.lstGroups_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lstGroups);
            this.panel2.Location = new System.Drawing.Point(138, 99);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(384, 294);
            this.panel2.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AccessibleDescription = "Total number of groups in the list";
            this.label6.AccessibleName = "Total groups";
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(136, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 13);
            this.label6.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.AccessibleDescription = "The UUID of the selected group";
            this.label5.AccessibleName = "UUID";
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(139, 73);
            this.label5.Name = "label5";
            this.label5.ReadOnly = true;
            this.label5.Size = new System.Drawing.Size(381, 20);
            this.label5.TabIndex = 6;
            // 
            // button4
            // 
            this.button4.AccessibleDescription = "Leave this group";
            this.button4.AccessibleName = "Leave";
            this.button4.BackColor = System.Drawing.Color.RoyalBlue;
            this.button4.Enabled = false;
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(380, 408);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(54, 23);
            this.button4.TabIndex = 22;
            this.button4.Text = "Invite";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // GroupsConsole
            // 
            this.AccessibleName = "Groups Console";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdInfo);
            this.Controls.Add(this.cmdActivate);
            this.Controls.Add(this.cmdIM);
            this.Controls.Add(this.cmdLeave);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.DoubleBuffered = true;
            this.Name = "GroupsConsole";
            this.Size = new System.Drawing.Size(567, 439);
            this.Load += new System.EventHandler(this.GroupsConsole_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdInfo;
        private System.Windows.Forms.Button cmdActivate;
        private System.Windows.Forms.Button cmdIM;
        private System.Windows.Forms.Button cmdLeave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox lstGroups;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox label5;
        private System.Windows.Forms.Button button4;
    }
}
