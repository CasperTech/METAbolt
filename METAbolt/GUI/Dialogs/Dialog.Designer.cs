namespace METAbolt
{
    partial class frmDialog
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.cboReply = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.tsButtons = new System.Windows.Forms.ToolStrip();
            this.button1 = new System.Windows.Forms.Button();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AccessibleName = "Dialog name";
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(375, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "label1";
            // 
            // cboReply
            // 
            this.cboReply.AccessibleName = "Action options";
            this.cboReply.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cboReply.FormattingEnabled = true;
            this.cboReply.Location = new System.Drawing.Point(116, 190);
            this.cboReply.Name = "cboReply";
            this.cboReply.Size = new System.Drawing.Size(56, 21);
            this.cboReply.TabIndex = 2;
            this.cboReply.Text = "Select...";
            this.cboReply.Visible = false;
            // 
            // button2
            // 
            this.button2.AccessibleName = "Ignore and close this dialog";
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.DimGray;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(337, 188);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(50, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "&Ignore";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.AcceptsReturn = true;
            this.txtMessage.AcceptsTab = true;
            this.txtMessage.AccessibleName = "Dialog information";
            this.txtMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(12, 44);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessage.Size = new System.Drawing.Size(375, 138);
            this.txtMessage.TabIndex = 5;
            this.txtMessage.TextChanged += new System.EventHandler(this.txtMessage_TextChanged);
            // 
            // button3
            // 
            this.button3.AccessibleDescription = "to avoid getting flooded by incoming dialogs";
            this.button3.AccessibleName = "Reset flood buffer";
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.BackColor = System.Drawing.Color.DimGray;
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(60, 188);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(118, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "R&eset flood buffer";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.AccessibleName = "Mute the sending object";
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button4.BackColor = System.Drawing.Color.DimGray;
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(12, 188);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(42, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "M&ute";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(396, 95);
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 217);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(396, 95);
            this.toolStripContainer1.TabIndex = 8;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.tsButtons);
            // 
            // tsButtons
            // 
            this.tsButtons.AccessibleName = "Main METAbolt menu";
            this.tsButtons.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.tsButtons.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tsButtons.Dock = System.Windows.Forms.DockStyle.None;
            this.tsButtons.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.tsButtons.Location = new System.Drawing.Point(0, 0);
            this.tsButtons.Name = "tsButtons";
            this.tsButtons.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tsButtons.ShowItemToolTips = false;
            this.tsButtons.Size = new System.Drawing.Size(396, 0);
            this.tsButtons.Stretch = true;
            this.tsButtons.TabIndex = 1;
            this.tsButtons.Text = "toolStrip2";
            // 
            // button1
            // 
            this.button1.AccessibleName = "Ignore and close this dialog";
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.DimGray;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(281, 188);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Se&nd";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmDialog
            // 
            this.AccessibleDescription = "Displays script generated dialogs to interact with objects";
            this.AccessibleName = "Dialog window";
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Alert;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(397, 313);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.cboReply);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDialog";
            this.Opacity = 0.85D;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dialog";
            this.Load += new System.EventHandler(this.Dialog_Load);
            this.MouseEnter += new System.EventHandler(this.frmDialog_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.frmDialog_MouseLeave);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ComboBox cboReply;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip tsButtons;
        private System.Windows.Forms.Button button1;
    }
}