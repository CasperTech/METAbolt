namespace METAbolt
{
    partial class frmPluginManager
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
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chkLSL = new System.Windows.Forms.CheckBox();
            this.checkBox12 = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtMObject = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMavatar = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Gray;
            this.label4.Location = new System.Drawing.Point(22, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(220, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Note: Any changes requires METAbolt restart";
            // 
            // button2
            // 
            this.button2.AccessibleName = "Un-chose selected plugin button";
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(179, 99);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(20, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "<";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.AccessibleName = "Chose selected plugin button";
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(179, 72);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(20, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = ">";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(310, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Select one or more plugins to autorun each time METAbolt starts";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(207, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Selected plugins";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Available plugins";
            // 
            // listBox2
            // 
            this.listBox2.AccessibleName = "Chosen plugins listbox";
            this.listBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(208, 70);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(149, 147);
            this.listBox2.TabIndex = 13;
            // 
            // listBox1
            // 
            this.listBox1.AccessibleName = "Available plugins lixtbox";
            this.listBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(22, 70);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(149, 147);
            this.listBox1.TabIndex = 9;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(208, 467);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 14;
            this.button3.Text = "Apply";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(296, 467);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 15;
            this.button4.Text = "Close";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.AccessibleName = "Master avatar panel";
            this.groupBox5.Controls.Add(this.chkLSL);
            this.groupBox5.Controls.Add(this.checkBox12);
            this.groupBox5.Controls.Add(this.label17);
            this.groupBox5.Controls.Add(this.txtMObject);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.txtMavatar);
            this.groupBox5.Location = new System.Drawing.Point(22, 317);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(335, 138);
            this.groupBox5.TabIndex = 16;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "MB LSL API Master Avatar and Object";
            // 
            // chkLSL
            // 
            this.chkLSL.AccessibleName = "Disable icon option";
            this.chkLSL.AutoSize = true;
            this.chkLSL.Location = new System.Drawing.Point(79, 115);
            this.chkLSL.Name = "chkLSL";
            this.chkLSL.Size = new System.Drawing.Size(251, 17);
            this.chkLSL.TabIndex = 64;
            this.chkLSL.Text = "Display LSL commands on MB_LSLAPI window";
            this.chkLSL.UseVisualStyleBackColor = true;
            // 
            // checkBox12
            // 
            this.checkBox12.AccessibleDescription = "Enforce security so that LSL commands are only accepted from the specified avatar" +
    " and object";
            this.checkBox12.AccessibleName = "Enforce LSL security";
            this.checkBox12.AutoSize = true;
            this.checkBox12.Location = new System.Drawing.Point(79, 95);
            this.checkBox12.Name = "checkBox12";
            this.checkBox12.Size = new System.Drawing.Size(254, 17);
            this.checkBox12.TabIndex = 63;
            this.checkBox12.Text = "Limit LSL commands to above avatar and object";
            this.checkBox12.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(6, 60);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(73, 14);
            this.label17.TabIndex = 62;
            this.label17.Text = "Object UUID:";
            // 
            // txtMObject
            // 
            this.txtMObject.AccessibleName = "UUID textbox";
            this.txtMObject.BackColor = System.Drawing.Color.White;
            this.txtMObject.Location = new System.Drawing.Point(79, 57);
            this.txtMObject.Name = "txtMObject";
            this.txtMObject.Size = new System.Drawing.Size(227, 20);
            this.txtMObject.TabIndex = 61;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 34);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 14);
            this.label7.TabIndex = 60;
            this.label7.Text = "Avatar UUID:";
            // 
            // txtMavatar
            // 
            this.txtMavatar.AccessibleName = "UUID textbox";
            this.txtMavatar.BackColor = System.Drawing.Color.White;
            this.txtMavatar.Location = new System.Drawing.Point(79, 31);
            this.txtMavatar.Name = "txtMavatar";
            this.txtMavatar.Size = new System.Drawing.Size(227, 20);
            this.txtMavatar.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(19, 236);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(338, 68);
            this.label5.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 223);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Information:";
            // 
            // frmPluginManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(383, 502);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.listBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPluginManager";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Plugin Manager";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmPluginManager_Load);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox checkBox12;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtMObject;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtMavatar;
        private System.Windows.Forms.CheckBox chkLSL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}