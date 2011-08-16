namespace METAbolt
{
    partial class FindGroups
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindGroups));
            this.lvwFindGroups = new System.Windows.Forms.ListView();
            this.chdGroupName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chdDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pGroups = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pGroups)).BeginInit();
            this.SuspendLayout();
            // 
            // lvwFindGroups
            // 
            this.lvwFindGroups.BackColor = System.Drawing.Color.White;
            this.lvwFindGroups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chdGroupName,
            this.chdDescription});
            this.lvwFindGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwFindGroups.FullRowSelect = true;
            this.lvwFindGroups.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwFindGroups.Location = new System.Drawing.Point(0, 0);
            this.lvwFindGroups.MultiSelect = false;
            this.lvwFindGroups.Name = "lvwFindGroups";
            this.lvwFindGroups.Size = new System.Drawing.Size(522, 361);
            this.lvwFindGroups.TabIndex = 1;
            this.lvwFindGroups.UseCompatibleStateImageBehavior = false;
            this.lvwFindGroups.View = System.Windows.Forms.View.Details;
            this.lvwFindGroups.SelectedIndexChanged += new System.EventHandler(this.lvwFindGroups_SelectedIndexChanged);
            // 
            // chdGroupName
            // 
            this.chdGroupName.Text = "Group";
            this.chdGroupName.Width = 205;
            // 
            // chdDescription
            // 
            this.chdDescription.Text = "Description";
            this.chdDescription.Width = 350;
            // 
            // pGroups
            // 
            this.pGroups.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pGroups.Image = ((System.Drawing.Image)(resources.GetObject("pGroups.Image")));
            this.pGroups.Location = new System.Drawing.Point(223, 141);
            this.pGroups.Name = "pGroups";
            this.pGroups.Size = new System.Drawing.Size(76, 78);
            this.pGroups.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pGroups.TabIndex = 46;
            this.pGroups.TabStop = false;
            this.pGroups.Visible = false;
            this.pGroups.Click += new System.EventHandler(this.pGroups_Click);
            // 
            // FindGroups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pGroups);
            this.Controls.Add(this.lvwFindGroups);
            this.Name = "FindGroups";
            this.Size = new System.Drawing.Size(522, 361);
            ((System.ComponentModel.ISupportInitialize)(this.pGroups)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvwFindGroups;
        private System.Windows.Forms.ColumnHeader chdGroupName;
        private System.Windows.Forms.ColumnHeader chdDescription;
        private System.Windows.Forms.PictureBox pGroups;
    }
}
