namespace METAbolt
{
    partial class FindLand
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
            this.lvwFindLand = new System.Windows.Forms.ListView();
            this.chdName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chdTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chdPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chdSQPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pLand = new System.Windows.Forms.PictureBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblInformation = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtInformation = new System.Windows.Forms.TextBox();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkMature = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pLand)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvwFindLand
            // 
            this.lvwFindLand.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwFindLand.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lvwFindLand.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chdName,
            this.chdTime,
            this.chdPrice,
            this.chdSQPrice});
            this.lvwFindLand.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.lvwFindLand.FullRowSelect = true;
            this.lvwFindLand.Location = new System.Drawing.Point(2, 0);
            this.lvwFindLand.MultiSelect = false;
            this.lvwFindLand.Name = "lvwFindLand";
            this.lvwFindLand.Size = new System.Drawing.Size(385, 387);
            this.lvwFindLand.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvwFindLand.TabIndex = 0;
            this.lvwFindLand.UseCompatibleStateImageBehavior = false;
            this.lvwFindLand.View = System.Windows.Forms.View.Details;
            this.lvwFindLand.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvwFindLand_ColumnClick);
            this.lvwFindLand.SelectedIndexChanged += new System.EventHandler(this.lvwFindLand_SelectedIndexChanged);
            // 
            // chdName
            // 
            this.chdName.Text = "Name";
            this.chdName.Width = 190;
            // 
            // chdTime
            // 
            this.chdTime.Text = "Area";
            this.chdTime.Width = 65;
            // 
            // chdPrice
            // 
            this.chdPrice.Text = "Price";
            this.chdPrice.Width = 65;
            // 
            // chdSQPrice
            // 
            this.chdSQPrice.Text = "SqM Price";
            // 
            // pLand
            // 
            this.pLand.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pLand.Image = global::METAbolt.Properties.Resources.wait30trans;
            this.pLand.Location = new System.Drawing.Point(177, 178);
            this.pLand.Name = "pLand";
            this.pLand.Size = new System.Drawing.Size(30, 30);
            this.pLand.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pLand.TabIndex = 43;
            this.pLand.TabStop = false;
            this.pLand.Visible = false;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Location = new System.Drawing.Point(31, 24);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 44;
            this.lblName.Text = "Name:";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblDescription.Location = new System.Drawing.Point(31, 71);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 13);
            this.lblDescription.TabIndex = 45;
            this.lblDescription.Text = "Description:";
            // 
            // lblInformation
            // 
            this.lblInformation.AutoSize = true;
            this.lblInformation.BackColor = System.Drawing.Color.Transparent;
            this.lblInformation.Location = new System.Drawing.Point(31, 260);
            this.lblInformation.Name = "lblInformation";
            this.lblInformation.Size = new System.Drawing.Size(59, 13);
            this.lblInformation.TabIndex = 46;
            this.lblInformation.Text = "Information";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.BackColor = System.Drawing.Color.Transparent;
            this.lblLocation.Location = new System.Drawing.Point(31, 302);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(51, 13);
            this.lblLocation.TabIndex = 47;
            this.lblLocation.Text = "Location:";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.RoyalBlue;
            this.button1.Enabled = false;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(34, 354);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Teleport";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtName.Location = new System.Drawing.Point(34, 40);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(248, 18);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtDescription.Location = new System.Drawing.Point(34, 87);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(248, 160);
            this.txtDescription.TabIndex = 2;
            // 
            // txtInformation
            // 
            this.txtInformation.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtInformation.Location = new System.Drawing.Point(34, 276);
            this.txtInformation.Name = "txtInformation";
            this.txtInformation.ReadOnly = true;
            this.txtInformation.Size = new System.Drawing.Size(248, 18);
            this.txtInformation.TabIndex = 3;
            // 
            // txtLocation
            // 
            this.txtLocation.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLocation.Location = new System.Drawing.Point(34, 318);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.ReadOnly = true;
            this.txtLocation.Size = new System.Drawing.Size(247, 18);
            this.txtLocation.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.chkMature);
            this.panel1.Controls.Add(this.txtDescription);
            this.panel1.Controls.Add(this.txtLocation);
            this.panel1.Controls.Add(this.lblName);
            this.panel1.Controls.Add(this.txtInformation);
            this.panel1.Controls.Add(this.lblDescription);
            this.panel1.Controls.Add(this.lblInformation);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.lblLocation);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(389, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(290, 384);
            this.panel1.TabIndex = 48;
            // 
            // chkMature
            // 
            this.chkMature.AutoSize = true;
            this.chkMature.Enabled = false;
            this.chkMature.Location = new System.Drawing.Point(227, 3);
            this.chkMature.Name = "chkMature";
            this.chkMature.Size = new System.Drawing.Size(59, 17);
            this.chkMature.TabIndex = 48;
            this.chkMature.Text = "Mature";
            this.chkMature.UseVisualStyleBackColor = true;
            // 
            // FindLand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pLand);
            this.Controls.Add(this.lvwFindLand);
            this.Name = "FindLand";
            this.Size = new System.Drawing.Size(682, 387);
            ((System.ComponentModel.ISupportInitialize)(this.pLand)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvwFindLand;
        private System.Windows.Forms.ColumnHeader chdName;
        private System.Windows.Forms.ColumnHeader chdTime;
        private System.Windows.Forms.PictureBox pLand;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblInformation;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtInformation;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.ColumnHeader chdPrice;
        private System.Windows.Forms.ColumnHeader chdSQPrice;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkMature;
    }
}
