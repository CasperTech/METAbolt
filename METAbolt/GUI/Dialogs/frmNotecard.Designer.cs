namespace METAbolt
{
    partial class frmNotecard
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNotecard));
            this.btnClose = new System.Windows.Forms.Button();
            this.lblSaveStatus = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.Menu1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsChkWord = new METAbolt.ToolStripCheckBox();
            this.tsChkCase = new METAbolt.ToolStripCheckBox();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.tsFindText = new System.Windows.Forms.ToolStripTextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.tsReplaceText = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsLn = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsCol = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.rtbNotecard = new METAbolt.RichTextBoxFR();
            this.toolStrip1.SuspendLayout();
            this.Menu1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(571, 479);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblSaveStatus
            // 
            this.lblSaveStatus.AccessibleName = "Status label";
            this.lblSaveStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSaveStatus.AutoSize = true;
            this.lblSaveStatus.Location = new System.Drawing.Point(-3, 484);
            this.lblSaveStatus.Name = "lblSaveStatus";
            this.lblSaveStatus.Size = new System.Drawing.Size(68, 13);
            this.lblSaveStatus.TabIndex = 3;
            this.lblSaveStatus.Text = "Save status.";
            this.lblSaveStatus.Visible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleName = "Main menu bar";
            this.toolStrip1.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripDropDownButton2,
            this.tsChkWord,
            this.tsChkCase,
            this.toolStripButton2,
            this.tsFindText});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(658, 25);
            this.toolStrip1.TabIndex = 47;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.AccessibleName = "File menu";
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::METAbolt.Properties.Resources.documents_16;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(54, 22);
            this.toolStripDropDownButton1.Text = "File";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.AccessibleName = "Close menu option";
            this.closeToolStripMenuItem.Image = global::METAbolt.Properties.Resources.delete_16;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.AccessibleName = "Edit menu";
            this.toolStripDropDownButton2.DropDown = this.Menu1;
            this.toolStripDropDownButton2.Image = global::METAbolt.Properties.Resources.pref23png;
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(56, 22);
            this.toolStripDropDownButton2.Text = "Edit";
            this.toolStripDropDownButton2.Click += new System.EventHandler(this.toolStripDropDownButton2_Click);
            // 
            // Menu1
            // 
            this.Menu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.toolStripSeparator5,
            this.selectAllToolStripMenuItem});
            this.Menu1.Name = "Menu1";
            this.Menu1.OwnerItem = this.toolStripDropDownButton2;
            this.Menu1.Size = new System.Drawing.Size(162, 54);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.AccessibleName = "Copy menu option";
            this.copyToolStripMenuItem.Image = global::METAbolt.Properties.Resources.copy_16;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.copyToolStripMenuItem.Text = "Copy        Ctrl+C";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(158, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.AccessibleName = "Select all menu option";
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.selectAllToolStripMenuItem.Text = "Select all  Ctrl+A";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // tsChkWord
            // 
            this.tsChkWord.AccessibleName = "Match word option";
            this.tsChkWord.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsChkWord.BackColor = System.Drawing.SystemColors.Control;
            this.tsChkWord.Checked = false;
            this.tsChkWord.Name = "tsChkWord";
            this.tsChkWord.Size = new System.Drawing.Size(90, 22);
            this.tsChkWord.Text = "Match word";
            this.tsChkWord.ToolStripCheckBoxEnabled = true;
            // 
            // tsChkCase
            // 
            this.tsChkCase.AccessibleName = "Match case option";
            this.tsChkCase.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsChkCase.BackColor = System.Drawing.SystemColors.Control;
            this.tsChkCase.Checked = false;
            this.tsChkCase.Name = "tsChkCase";
            this.tsChkCase.Size = new System.Drawing.Size(86, 22);
            this.tsChkCase.Text = "Match case";
            this.tsChkCase.ToolStripCheckBoxEnabled = true;
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.AccessibleName = "Find/Find next button";
            this.toolStripButton2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(61, 22);
            this.toolStripButton2.Text = "Find Next";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // tsFindText
            // 
            this.tsFindText.AccessibleName = "Search box";
            this.tsFindText.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsFindText.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tsFindText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tsFindText.Name = "tsFindText";
            this.tsFindText.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton4,
            this.toolStripButton5,
            this.tsReplaceText,
            this.toolStripButton1});
            this.toolStrip2.Location = new System.Drawing.Point(0, 25);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(658, 25);
            this.toolStrip2.TabIndex = 49;
            this.toolStrip2.Text = "toolStrip2";
            this.toolStrip2.Visible = false;
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(69, 22);
            this.toolStripButton4.Text = "Replace All";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(52, 22);
            this.toolStripButton5.Text = "Replace";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // tsReplaceText
            // 
            this.tsReplaceText.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsReplaceText.Name = "tsReplaceText";
            this.tsReplaceText.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::METAbolt.Properties.Resources.delete_16;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.AccessibleName = "Information bar";
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsLn,
            this.toolStripStatusLabel2,
            this.tsCol,
            this.toolStripStatusLabel1,
            this.tsStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 506);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(658, 22);
            this.statusStrip1.TabIndex = 51;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsLn
            // 
            this.tsLn.Name = "tsLn";
            this.tsLn.Size = new System.Drawing.Size(29, 17);
            this.tsLn.Text = "Ln 1";
            this.tsLn.Visible = false;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel2.Enabled = false;
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.Gray;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel2.Text = "|";
            this.toolStripStatusLabel2.Visible = false;
            // 
            // tsCol
            // 
            this.tsCol.Name = "tsCol";
            this.tsCol.Size = new System.Drawing.Size(34, 17);
            this.tsCol.Text = "Col 1";
            this.tsCol.Visible = false;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel1.Enabled = false;
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Gray;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel1.Text = "|";
            this.toolStripStatusLabel1.Visible = false;
            // 
            // tsStatus
            // 
            this.tsStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsStatus.ForeColor = System.Drawing.Color.Red;
            this.tsStatus.Name = "tsStatus";
            this.tsStatus.Size = new System.Drawing.Size(48, 17);
            this.tsStatus.Text = "Ready...";
            this.tsStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsStatus.Click += new System.EventHandler(this.tsStatus_Click);
            // 
            // rtbNotecard
            // 
            this.rtbNotecard.AccessibleName = "Notecard content/body";
            this.rtbNotecard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbNotecard.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rtbNotecard.Location = new System.Drawing.Point(0, 28);
            this.rtbNotecard.Name = "rtbNotecard";
            this.rtbNotecard.Size = new System.Drawing.Size(658, 445);
            this.rtbNotecard.TabIndex = 0;
            this.rtbNotecard.Text = "";
            this.rtbNotecard.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtbNotecard_KeyDown);
            // 
            // frmNotecard
            // 
            this.AccessibleName = "Notecard window";
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(658, 528);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lblSaveStatus);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.rtbNotecard);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmNotecard";
            this.Text = "Notecard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmNotecardEditor_FormClosing);
            this.Load += new System.EventHandler(this.frmNotecardEditor_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.Menu1.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblSaveStatus;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private ToolStripCheckBox tsChkWord;
        private ToolStripCheckBox tsChkCase;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripTextBox tsFindText;
        private System.Windows.Forms.ContextMenuStrip Menu1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripTextBox tsReplaceText;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsLn;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel tsCol;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tsStatus;
        private RichTextBoxFR rtbNotecard;
    }
}