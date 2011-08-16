namespace METAbolt
{
    partial class InventoryNotecardConsole
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnEditNotecard = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Notecard Options";
            // 
            // btnEditNotecard
            // 
            this.btnEditNotecard.AccessibleName = "View button";
            this.btnEditNotecard.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnEditNotecard.ForeColor = System.Drawing.Color.White;
            this.btnEditNotecard.Location = new System.Drawing.Point(3, 28);
            this.btnEditNotecard.Name = "btnEditNotecard";
            this.btnEditNotecard.Size = new System.Drawing.Size(128, 23);
            this.btnEditNotecard.TabIndex = 0;
            this.btnEditNotecard.Text = "&View/Edit Notecard";
            this.btnEditNotecard.UseVisualStyleBackColor = false;
            this.btnEditNotecard.Click += new System.EventHandler(this.btnEditNotecard_Click);
            // 
            // InventoryNotecardConsole
            // 
            this.AccessibleName = "Notecard console";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnEditNotecard);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "InventoryNotecardConsole";
            this.Size = new System.Drawing.Size(306, 305);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEditNotecard;
    }
}
