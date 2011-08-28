namespace METAbolt
{
    partial class FindEvents
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
            this.lvwFindEvents = new System.Windows.Forms.ListView();
            this.chdName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chdTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.pEvents = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pEvents)).BeginInit();
            this.SuspendLayout();
            // 
            // lvwFindEvents
            // 
            this.lvwFindEvents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lvwFindEvents.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lvwFindEvents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chdName,
            this.chdTime});
            this.lvwFindEvents.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.lvwFindEvents.FullRowSelect = true;
            this.lvwFindEvents.Location = new System.Drawing.Point(0, 0);
            this.lvwFindEvents.MultiSelect = false;
            this.lvwFindEvents.Name = "lvwFindEvents";
            this.lvwFindEvents.Size = new System.Drawing.Size(315, 387);
            this.lvwFindEvents.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvwFindEvents.TabIndex = 0;
            this.lvwFindEvents.UseCompatibleStateImageBehavior = false;
            this.lvwFindEvents.View = System.Windows.Forms.View.Details;
            this.lvwFindEvents.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvwFindEvents_ColumnClick);
            this.lvwFindEvents.SelectedIndexChanged += new System.EventHandler(this.lvwFindEvents_SelectedIndexChanged);
            // 
            // chdName
            // 
            this.chdName.Text = "Name";
            this.chdName.Width = 190;
            // 
            // chdTime
            // 
            this.chdTime.Text = "Time (PT)";
            this.chdTime.Width = 100;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.label1.Location = new System.Drawing.Point(321, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.label2.Location = new System.Drawing.Point(321, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Type:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.label4.Location = new System.Drawing.Point(321, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Date:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.label5.Location = new System.Drawing.Point(321, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Event duration:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.label6.Location = new System.Drawing.Point(321, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Run by:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.label7.Location = new System.Drawing.Point(321, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Location:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.label8.Location = new System.Drawing.Point(321, 128);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Cover charge:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.label9.Location = new System.Drawing.Point(321, 144);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Description:";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox1.Location = new System.Drawing.Point(324, 160);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(313, 204);
            this.textBox1.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.RoyalBlue;
            this.button1.Enabled = false;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(325, 364);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Teleport";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.RoyalBlue;
            this.button2.Enabled = false;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(406, 364);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(93, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Create Event";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.textBox2.Location = new System.Drawing.Point(426, 3);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(211, 18);
            this.textBox2.TabIndex = 1;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.textBox3.Location = new System.Drawing.Point(426, 23);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(211, 18);
            this.textBox3.TabIndex = 2;
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.textBox5.Location = new System.Drawing.Point(426, 43);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(211, 18);
            this.textBox5.TabIndex = 3;
            // 
            // textBox6
            // 
            this.textBox6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.textBox6.Location = new System.Drawing.Point(426, 63);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(211, 18);
            this.textBox6.TabIndex = 4;
            // 
            // textBox7
            // 
            this.textBox7.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.textBox7.Location = new System.Drawing.Point(426, 83);
            this.textBox7.Name = "textBox7";
            this.textBox7.ReadOnly = true;
            this.textBox7.Size = new System.Drawing.Size(211, 18);
            this.textBox7.TabIndex = 5;
            // 
            // textBox8
            // 
            this.textBox8.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.textBox8.Location = new System.Drawing.Point(426, 103);
            this.textBox8.Name = "textBox8";
            this.textBox8.ReadOnly = true;
            this.textBox8.Size = new System.Drawing.Size(211, 18);
            this.textBox8.TabIndex = 6;
            // 
            // textBox9
            // 
            this.textBox9.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.textBox9.Location = new System.Drawing.Point(426, 123);
            this.textBox9.Name = "textBox9";
            this.textBox9.ReadOnly = true;
            this.textBox9.Size = new System.Drawing.Size(211, 18);
            this.textBox9.TabIndex = 7;
            // 
            // pEvents
            // 
            this.pEvents.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pEvents.Image = global::METAbolt.Properties.Resources.wait30trans;
            this.pEvents.Location = new System.Drawing.Point(136, 174);
            this.pEvents.Name = "pEvents";
            this.pEvents.Size = new System.Drawing.Size(30, 30);
            this.pEvents.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pEvents.TabIndex = 44;
            this.pEvents.TabStop = false;
            this.pEvents.Visible = false;
            // 
            // FindEvents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pEvents);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvwFindEvents);
            this.Name = "FindEvents";
            this.Size = new System.Drawing.Size(640, 389);
            this.Load += new System.EventHandler(this.FindEvents_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pEvents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvwFindEvents;
        private System.Windows.Forms.ColumnHeader chdName;
        private System.Windows.Forms.ColumnHeader chdTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.PictureBox pEvents;

    }
}
