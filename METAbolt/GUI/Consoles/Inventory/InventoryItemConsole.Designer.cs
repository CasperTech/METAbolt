namespace METAbolt
{
    partial class InventoryItemConsole
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
            this.label5 = new System.Windows.Forms.Label();
            this.txtItemOwner = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtItemDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtItemCreator = new System.Windows.Forms.TextBox();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUUID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.pnlItemTypeProp = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnTP = new System.Windows.Forms.Button();
            this.btnGive = new System.Windows.Forms.Button();
            this.btnDetach = new System.Windows.Forms.Button();
            this.btnWear = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtItemUUID = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.pnlItemTypeProp.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 163);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Owner:";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // txtItemOwner
            // 
            this.txtItemOwner.AccessibleName = "Owner name textbox";
            this.txtItemOwner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtItemOwner.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtItemOwner.Location = new System.Drawing.Point(73, 160);
            this.txtItemOwner.Name = "txtItemOwner";
            this.txtItemOwner.ReadOnly = true;
            this.txtItemOwner.Size = new System.Drawing.Size(230, 21);
            this.txtItemOwner.TabIndex = 4;
            this.txtItemOwner.TextChanged += new System.EventHandler(this.txtItemOwner_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Description:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // txtItemDescription
            // 
            this.txtItemDescription.AccessibleName = "Description textbox";
            this.txtItemDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtItemDescription.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtItemDescription.Location = new System.Drawing.Point(73, 109);
            this.txtItemDescription.Name = "txtItemDescription";
            this.txtItemDescription.ReadOnly = true;
            this.txtItemDescription.Size = new System.Drawing.Size(230, 21);
            this.txtItemDescription.TabIndex = 2;
            this.txtItemDescription.TextChanged += new System.EventHandler(this.txtItemDescription_TextChanged);
            this.txtItemDescription.Leave += new System.EventHandler(this.txtItemDescription_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, -2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Item Properties";
            // 
            // txtItemCreator
            // 
            this.txtItemCreator.AccessibleName = "Creator name textbox";
            this.txtItemCreator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtItemCreator.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtItemCreator.Location = new System.Drawing.Point(73, 134);
            this.txtItemCreator.Name = "txtItemCreator";
            this.txtItemCreator.ReadOnly = true;
            this.txtItemCreator.Size = new System.Drawing.Size(230, 21);
            this.txtItemCreator.TabIndex = 3;
            this.txtItemCreator.TextChanged += new System.EventHandler(this.txtItemCreator_TextChanged);
            // 
            // txtItemName
            // 
            this.txtItemName.AccessibleName = "Name textbox";
            this.txtItemName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtItemName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtItemName.Location = new System.Drawing.Point(73, 34);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.ReadOnly = true;
            this.txtItemName.Size = new System.Drawing.Size(230, 21);
            this.txtItemName.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Creator:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Name:";
            // 
            // txtUUID
            // 
            this.txtUUID.AccessibleName = "UUID textbox";
            this.txtUUID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUUID.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtUUID.Location = new System.Drawing.Point(73, 59);
            this.txtUUID.Name = "txtUUID";
            this.txtUUID.ReadOnly = true;
            this.txtUUID.Size = new System.Drawing.Size(230, 21);
            this.txtUUID.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "UUID (asset):";
            // 
            // label7
            // 
            this.label7.AccessibleName = "Teleport status label";
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label7.Location = new System.Drawing.Point(49, 101);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(217, 32);
            this.label7.TabIndex = 25;
            this.label7.Text = "label7";
            this.label7.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.AccessibleName = "Teleport progress bar";
            this.progressBar1.Location = new System.Drawing.Point(45, 137);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(221, 13);
            this.progressBar1.TabIndex = 26;
            this.progressBar1.Visible = false;
            this.progressBar1.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // pnlItemTypeProp
            // 
            this.pnlItemTypeProp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlItemTypeProp.Controls.Add(this.progressBar1);
            this.pnlItemTypeProp.Controls.Add(this.label7);
            this.pnlItemTypeProp.Location = new System.Drawing.Point(14, 212);
            this.pnlItemTypeProp.Name = "pnlItemTypeProp";
            this.pnlItemTypeProp.Size = new System.Drawing.Size(278, 214);
            this.pnlItemTypeProp.TabIndex = 8;
            this.pnlItemTypeProp.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlItemTypeProp_Paint);
            // 
            // checkBox1
            // 
            this.checkBox1.AccessibleName = "Modify perms option";
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(75, 187);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(58, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Modify";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AccessibleName = "Copy perms option";
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(136, 187);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(51, 17);
            this.checkBox2.TabIndex = 6;
            this.checkBox2.Text = "Copy";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AccessibleName = "transfer perms option";
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(190, 187);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(67, 17);
            this.checkBox3.TabIndex = 7;
            this.checkBox3.Text = "Transfer";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 189);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 13);
            this.label8.TabIndex = 30;
            this.label8.Text = "Permissions:";
            // 
            // btnTP
            // 
            this.btnTP.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnTP.ForeColor = System.Drawing.Color.White;
            this.btnTP.Location = new System.Drawing.Point(59, 434);
            this.btnTP.Name = "btnTP";
            this.btnTP.Size = new System.Drawing.Size(75, 23);
            this.btnTP.TabIndex = 10;
            this.btnTP.Text = "Teleport";
            this.btnTP.UseVisualStyleBackColor = false;
            this.btnTP.Visible = false;
            this.btnTP.Click += new System.EventHandler(this.btnTP_Click);
            // 
            // btnGive
            // 
            this.btnGive.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnGive.Enabled = false;
            this.btnGive.ForeColor = System.Drawing.Color.White;
            this.btnGive.Location = new System.Drawing.Point(8, 434);
            this.btnGive.Name = "btnGive";
            this.btnGive.Size = new System.Drawing.Size(46, 23);
            this.btnGive.TabIndex = 9;
            this.btnGive.Text = "Give";
            this.btnGive.UseVisualStyleBackColor = false;
            this.btnGive.Visible = false;
            this.btnGive.Click += new System.EventHandler(this.btnGive_Click);
            // 
            // btnDetach
            // 
            this.btnDetach.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnDetach.ForeColor = System.Drawing.Color.White;
            this.btnDetach.Location = new System.Drawing.Point(136, 434);
            this.btnDetach.Name = "btnDetach";
            this.btnDetach.Size = new System.Drawing.Size(75, 23);
            this.btnDetach.TabIndex = 11;
            this.btnDetach.Text = "Detach";
            this.btnDetach.UseVisualStyleBackColor = false;
            this.btnDetach.Visible = false;
            this.btnDetach.Click += new System.EventHandler(this.btnDetach_Click);
            // 
            // btnWear
            // 
            this.btnWear.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnWear.ForeColor = System.Drawing.Color.White;
            this.btnWear.Location = new System.Drawing.Point(217, 434);
            this.btnWear.Name = "btnWear";
            this.btnWear.Size = new System.Drawing.Size(75, 23);
            this.btnWear.TabIndex = 12;
            this.btnWear.Text = "Wear";
            this.btnWear.UseVisualStyleBackColor = false;
            this.btnWear.Visible = false;
            this.btnWear.Click += new System.EventHandler(this.btnWear_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label9.Location = new System.Drawing.Point(3, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(12, 13);
            this.label9.TabIndex = 31;
            this.label9.Text = "t";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // txtItemUUID
            // 
            this.txtItemUUID.AccessibleName = "UUID textbox";
            this.txtItemUUID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtItemUUID.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtItemUUID.Location = new System.Drawing.Point(73, 84);
            this.txtItemUUID.Name = "txtItemUUID";
            this.txtItemUUID.ReadOnly = true;
            this.txtItemUUID.Size = new System.Drawing.Size(230, 21);
            this.txtItemUUID.TabIndex = 32;
            this.txtItemUUID.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 87);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 13);
            this.label10.TabIndex = 33;
            this.label10.Text = "UUID (item):";
            // 
            // InventoryItemConsole
            // 
            this.AccessibleName = "Item console";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.txtItemUUID);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnTP);
            this.Controls.Add(this.btnGive);
            this.Controls.Add(this.btnDetach);
            this.Controls.Add(this.btnWear);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.txtUUID);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtItemOwner);
            this.Controls.Add(this.pnlItemTypeProp);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtItemDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtItemCreator);
            this.Controls.Add(this.txtItemName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "InventoryItemConsole";
            this.Size = new System.Drawing.Size(306, 487);
            this.Load += new System.EventHandler(this.InventoryItemConsole_Load);
            this.pnlItemTypeProp.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtItemOwner;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtItemDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtItemCreator;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUUID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Panel pnlItemTypeProp;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnTP;
        private System.Windows.Forms.Button btnGive;
        private System.Windows.Forms.Button btnDetach;
        private System.Windows.Forms.Button btnWear;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtItemUUID;
        private System.Windows.Forms.Label label10;
    }
}
