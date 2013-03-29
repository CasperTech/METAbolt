namespace METAbolt
{
    partial class WornAttachments
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
            this.btnTouch = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lbxPrims = new System.Windows.Forms.ListBox();
            this.lbxPrimGroup = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.pBar3 = new System.Windows.Forms.PictureBox();
            this.txtUUID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPoint = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pBar3)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTouch
            // 
            this.btnTouch.AccessibleDescription = "Touch or click the selected object";
            this.btnTouch.AccessibleName = "Touch or Click button";
            this.btnTouch.BackColor = System.Drawing.Color.DimGray;
            this.btnTouch.Enabled = false;
            this.btnTouch.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnTouch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.btnTouch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTouch.ForeColor = System.Drawing.Color.White;
            this.btnTouch.Location = new System.Drawing.Point(283, 439);
            this.btnTouch.Name = "btnTouch";
            this.btnTouch.Size = new System.Drawing.Size(77, 23);
            this.btnTouch.TabIndex = 5;
            this.btnTouch.Text = "&Touch/Click";
            this.btnTouch.UseVisualStyleBackColor = false;
            this.btnTouch.Click += new System.EventHandler(this.btnTouch_Click);
            // 
            // btnClose
            // 
            this.btnClose.AccessibleName = "Close this window button";
            this.btnClose.BackColor = System.Drawing.Color.DimGray;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(366, 439);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(51, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lbxPrims
            // 
            this.lbxPrims.AccessibleName = "Current woen attachments listbox";
            this.lbxPrims.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbxPrims.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbxPrims.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbxPrims.FormattingEnabled = true;
            this.lbxPrims.HorizontalScrollbar = true;
            this.lbxPrims.IntegralHeight = false;
            this.lbxPrims.ItemHeight = 18;
            this.lbxPrims.Location = new System.Drawing.Point(3, 3);
            this.lbxPrims.Name = "lbxPrims";
            this.lbxPrims.Size = new System.Drawing.Size(414, 203);
            this.lbxPrims.Sorted = true;
            this.lbxPrims.TabIndex = 0;
            this.lbxPrims.Visible = false;
            this.lbxPrims.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbxPrims_DrawItem);
            this.lbxPrims.SelectedIndexChanged += new System.EventHandler(this.lbxPrims_SelectedIndexChanged);
            // 
            // lbxPrimGroup
            // 
            this.lbxPrimGroup.AccessibleDescription = "List of child objects of the selected attachment";
            this.lbxPrimGroup.AccessibleName = "Linked objects listbox";
            this.lbxPrimGroup.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbxPrimGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbxPrimGroup.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbxPrimGroup.FormattingEnabled = true;
            this.lbxPrimGroup.HorizontalScrollbar = true;
            this.lbxPrimGroup.IntegralHeight = false;
            this.lbxPrimGroup.ItemHeight = 18;
            this.lbxPrimGroup.Location = new System.Drawing.Point(3, 288);
            this.lbxPrimGroup.Name = "lbxPrimGroup";
            this.lbxPrimGroup.Size = new System.Drawing.Size(414, 145);
            this.lbxPrimGroup.Sorted = true;
            this.lbxPrimGroup.TabIndex = 2;
            this.lbxPrimGroup.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbxPrimGroup_DrawItem);
            this.lbxPrimGroup.SelectedIndexChanged += new System.EventHandler(this.lbxPrimGroup_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 272);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Linked Objects:";
            // 
            // label1
            // 
            this.label1.AccessibleName = "Total number of attachments textbox";
            this.label1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(292, 210);
            this.label1.Name = "label1";
            this.label1.ReadOnly = true;
            this.label1.Size = new System.Drawing.Size(125, 21);
            this.label1.TabIndex = 1;
            this.label1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AccessibleName = "Total child objects textbox";
            this.label2.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(3, 441);
            this.label2.Name = "label2";
            this.label2.ReadOnly = true;
            this.label2.Size = new System.Drawing.Size(149, 21);
            this.label2.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 418);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 25;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(3, 471);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(414, 59);
            this.label5.TabIndex = 26;
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // button2
            // 
            this.button2.AccessibleName = "3D view button";
            this.button2.BackColor = System.Drawing.Color.RoyalBlue;
            this.button2.Enabled = false;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(221, 439);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(56, 23);
            this.button2.TabIndex = 56;
            this.button2.Text = "&View 3D";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // pBar3
            // 
            this.pBar3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pBar3.Image = global::METAbolt.Properties.Resources.wait30trans;
            this.pBar3.Location = new System.Drawing.Point(192, 92);
            this.pBar3.Name = "pBar3";
            this.pBar3.Size = new System.Drawing.Size(30, 30);
            this.pBar3.TabIndex = 60;
            this.pBar3.TabStop = false;
            this.pBar3.Visible = false;
            // 
            // txtUUID
            // 
            this.txtUUID.AccessibleName = "Total number of attachments textbox";
            this.txtUUID.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.txtUUID.BackColor = System.Drawing.SystemColors.Control;
            this.txtUUID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUUID.Location = new System.Drawing.Point(36, 210);
            this.txtUUID.Name = "txtUUID";
            this.txtUUID.ReadOnly = true;
            this.txtUUID.Size = new System.Drawing.Size(241, 21);
            this.txtUUID.TabIndex = 61;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1, 213);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 62;
            this.label6.Text = "UUID:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1, 245);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 13);
            this.label7.TabIndex = 63;
            this.label7.Text = "Attachment Point:";
            // 
            // txtPoint
            // 
            this.txtPoint.AccessibleName = "Total number of attachments textbox";
            this.txtPoint.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.txtPoint.BackColor = System.Drawing.SystemColors.Control;
            this.txtPoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPoint.Location = new System.Drawing.Point(101, 242);
            this.txtPoint.Name = "txtPoint";
            this.txtPoint.ReadOnly = true;
            this.txtPoint.Size = new System.Drawing.Size(316, 21);
            this.txtPoint.TabIndex = 64;
            // 
            // WornAttachments
            // 
            this.AccessibleName = "Worn attachments window";
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(420, 534);
            this.Controls.Add(this.txtPoint);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtUUID);
            this.Controls.Add(this.pBar3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbxPrimGroup);
            this.Controls.Add(this.lbxPrims);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnTouch);
            this.Controls.Add(this.label6);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WornAttachments";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Worn Attachments";
            this.Load += new System.EventHandler(this.WornAssets_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pBar3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTouch;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListBox lbxPrims;
        private System.Windows.Forms.ListBox lbxPrimGroup;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox label1;
        private System.Windows.Forms.TextBox label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pBar3;
        private System.Windows.Forms.TextBox txtUUID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPoint;
    }
}