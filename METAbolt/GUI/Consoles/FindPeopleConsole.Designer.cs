namespace METAbolt
{
    partial class FindPeopleConsole
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindPeopleConsole));
            this.lvwFindPeople = new System.Windows.Forms.ListView();
            this.chdName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chdOnline = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pPeople = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pPeople)).BeginInit();
            this.SuspendLayout();
            // 
            // lvwFindPeople
            // 
            this.lvwFindPeople.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lvwFindPeople.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chdName,
            this.chdOnline});
            this.lvwFindPeople.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwFindPeople.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwFindPeople.Location = new System.Drawing.Point(0, 0);
            this.lvwFindPeople.MultiSelect = false;
            this.lvwFindPeople.Name = "lvwFindPeople";
            this.lvwFindPeople.Size = new System.Drawing.Size(474, 319);
            this.lvwFindPeople.TabIndex = 0;
            this.lvwFindPeople.UseCompatibleStateImageBehavior = false;
            this.lvwFindPeople.View = System.Windows.Forms.View.Details;
            this.lvwFindPeople.SelectedIndexChanged += new System.EventHandler(this.lvwFindPeople_SelectedIndexChanged);
            // 
            // chdName
            // 
            this.chdName.Text = "Name";
            this.chdName.Width = 205;
            // 
            // chdOnline
            // 
            this.chdOnline.Text = "Online";
            // 
            // pPeople
            // 
            this.pPeople.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pPeople.Image = ((System.Drawing.Image)(resources.GetObject("pPeople.Image")));
            this.pPeople.Location = new System.Drawing.Point(195, 132);
            this.pPeople.Name = "pPeople";
            this.pPeople.Size = new System.Drawing.Size(76, 78);
            this.pPeople.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pPeople.TabIndex = 45;
            this.pPeople.TabStop = false;
            this.pPeople.Visible = false;
            this.pPeople.Click += new System.EventHandler(this.pPeople_Click);
            // 
            // FindPeopleConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pPeople);
            this.Controls.Add(this.lvwFindPeople);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FindPeopleConsole";
            this.Size = new System.Drawing.Size(474, 319);
            ((System.ComponentModel.ISupportInitialize)(this.pPeople)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvwFindPeople;
        private System.Windows.Forms.ColumnHeader chdName;
        private System.Windows.Forms.ColumnHeader chdOnline;
        private System.Windows.Forms.PictureBox pPeople;
    }
}
