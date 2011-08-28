namespace METAbolt
{
    partial class FindPlaces
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
            this.lvwFindPlaces = new System.Windows.Forms.ListView();
            this.chdName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chdTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pPlaces = new System.Windows.Forms.PictureBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblInformation = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtInformation = new System.Windows.Forms.TextBox();
            this.txtLocation = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pPlaces)).BeginInit();
            this.SuspendLayout();
            // 
            // lvwFindPlaces
            // 
            this.lvwFindPlaces.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lvwFindPlaces.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lvwFindPlaces.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chdName,
            this.chdTime});
            this.lvwFindPlaces.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.lvwFindPlaces.FullRowSelect = true;
            this.lvwFindPlaces.Location = new System.Drawing.Point(2, 0);
            this.lvwFindPlaces.MultiSelect = false;
            this.lvwFindPlaces.Name = "lvwFindPlaces";
            this.lvwFindPlaces.Size = new System.Drawing.Size(348, 387);
            this.lvwFindPlaces.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvwFindPlaces.TabIndex = 0;
            this.lvwFindPlaces.UseCompatibleStateImageBehavior = false;
            this.lvwFindPlaces.View = System.Windows.Forms.View.Details;
            this.lvwFindPlaces.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvwFindPlaces_ColumnClick);
            this.lvwFindPlaces.SelectedIndexChanged += new System.EventHandler(this.lvwFindPlaces_SelectedIndexChanged);
            // 
            // chdName
            // 
            this.chdName.Text = "Name";
            this.chdName.Width = 270;
            // 
            // chdTime
            // 
            this.chdTime.Text = "Traffic";
            this.chdTime.Width = 70;
            // 
            // pPlaces
            // 
            this.pPlaces.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pPlaces.Image = global::METAbolt.Properties.Resources.wait30trans;
            this.pPlaces.Location = new System.Drawing.Point(166, 180);
            this.pPlaces.Name = "pPlaces";
            this.pPlaces.Size = new System.Drawing.Size(30, 30);
            this.pPlaces.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pPlaces.TabIndex = 43;
            this.pPlaces.TabStop = false;
            this.pPlaces.Visible = false;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Location = new System.Drawing.Point(357, 26);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 44;
            this.lblName.Text = "Name:";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblDescription.Location = new System.Drawing.Point(356, 71);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 13);
            this.lblDescription.TabIndex = 45;
            this.lblDescription.Text = "Description:";
            // 
            // lblInformation
            // 
            this.lblInformation.AutoSize = true;
            this.lblInformation.BackColor = System.Drawing.Color.Transparent;
            this.lblInformation.Location = new System.Drawing.Point(357, 262);
            this.lblInformation.Name = "lblInformation";
            this.lblInformation.Size = new System.Drawing.Size(59, 13);
            this.lblInformation.TabIndex = 46;
            this.lblInformation.Text = "Information";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.BackColor = System.Drawing.Color.Transparent;
            this.lblLocation.Location = new System.Drawing.Point(357, 309);
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
            this.button1.Location = new System.Drawing.Point(360, 361);
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
            this.txtName.Location = new System.Drawing.Point(360, 42);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(281, 18);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtDescription.Location = new System.Drawing.Point(360, 87);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(281, 160);
            this.txtDescription.TabIndex = 2;
            // 
            // txtInformation
            // 
            this.txtInformation.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtInformation.Location = new System.Drawing.Point(360, 278);
            this.txtInformation.Name = "txtInformation";
            this.txtInformation.ReadOnly = true;
            this.txtInformation.Size = new System.Drawing.Size(281, 18);
            this.txtInformation.TabIndex = 3;
            // 
            // txtLocation
            // 
            this.txtLocation.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.txtLocation.Location = new System.Drawing.Point(359, 325);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.ReadOnly = true;
            this.txtLocation.Size = new System.Drawing.Size(281, 18);
            this.txtLocation.TabIndex = 4;
            // 
            // FindPlaces
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.txtInformation);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.lblInformation);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.pPlaces);
            this.Controls.Add(this.lvwFindPlaces);
            this.Name = "FindPlaces";
            this.Size = new System.Drawing.Size(657, 387);
            this.Load += new System.EventHandler(this.FindPlaces_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pPlaces)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvwFindPlaces;
        private System.Windows.Forms.ColumnHeader chdName;
        private System.Windows.Forms.ColumnHeader chdTime;
        private System.Windows.Forms.PictureBox pPlaces;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblInformation;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtInformation;
        private System.Windows.Forms.TextBox txtLocation;
    }
}
