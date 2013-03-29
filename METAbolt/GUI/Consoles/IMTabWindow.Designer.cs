namespace METAbolt
{
    partial class IMTabWindow
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
            this.rtbIMText_Ex = new System.Windows.Forms.RichTextBox();
            this.cbxInput = new System.Windows.Forms.ComboBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbtnProfile = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbTyping = new System.Windows.Forms.ToolStripButton();
            this.rtbIMText = new Khendys.Controls.ExRichTextBox();
            this.cmenu_Emoticons = new System.Windows.Forms.ContextMenu();
            this.tbar_SendMessage = new System.Windows.Forms.ToolBar();
            this.tbBtn_Emoticons = new System.Windows.Forms.ToolBarButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtbIMText_Ex
            // 
            this.rtbIMText_Ex.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbIMText_Ex.BackColor = System.Drawing.Color.White;
            this.rtbIMText_Ex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbIMText_Ex.HideSelection = false;
            this.rtbIMText_Ex.Location = new System.Drawing.Point(3, 28);
            this.rtbIMText_Ex.Name = "rtbIMText_Ex";
            this.rtbIMText_Ex.ReadOnly = true;
            this.rtbIMText_Ex.Size = new System.Drawing.Size(494, 270);
            this.rtbIMText_Ex.TabIndex = 3;
            this.rtbIMText_Ex.Text = "";
            this.rtbIMText_Ex.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtbIMText_LinkClicked_1);
            // 
            // cbxInput
            // 
            this.cbxInput.AccessibleDescription = "Type your message here to send an IM";
            this.cbxInput.AccessibleName = "IM output";
            this.cbxInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxInput.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cbxInput.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbxInput.FormattingEnabled = true;
            this.cbxInput.Location = new System.Drawing.Point(3, 306);
            this.cbxInput.Name = "cbxInput";
            this.cbxInput.Size = new System.Drawing.Size(372, 21);
            this.cbxInput.TabIndex = 0;
            this.cbxInput.SelectedIndexChanged += new System.EventHandler(this.cbxInput_SelectedIndexChanged);
            this.cbxInput.TextChanged += new System.EventHandler(this.cbxInput_TextChanged);
            this.cbxInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbxInput_KeyDown);
            this.cbxInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cbxInput_KeyUp);
            // 
            // btnSend
            // 
            this.btnSend.AccessibleDescription = "Send the IM you have typed";
            this.btnSend.AccessibleName = "Send";
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.BackColor = System.Drawing.Color.DimGray;
            this.btnSend.Enabled = false;
            this.btnSend.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnSend.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Location = new System.Drawing.Point(422, 304);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleName = "IM toolbar menu";
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbtnProfile,
            this.toolStripButton2,
            this.tsbSave,
            this.tsbClear,
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.tsbTyping});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(500, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // tbtnProfile
            // 
            this.tbtnProfile.AccessibleDescription = "Profile of the avatar you are talking to";
            this.tbtnProfile.AccessibleName = "Profile";
            this.tbtnProfile.Image = global::METAbolt.Properties.Resources.profile;
            this.tbtnProfile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnProfile.Name = "tbtnProfile";
            this.tbtnProfile.Size = new System.Drawing.Size(61, 22);
            this.tbtnProfile.Text = "&Profile";
            this.tbtnProfile.Click += new System.EventHandler(this.tbtnProfile_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Enabled = false;
            this.toolStripButton2.Image = global::METAbolt.Properties.Resources.history;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(65, 22);
            this.toolStripButton2.Text = "History";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.AccessibleDescription = "The IM window contents to log";
            this.tsbSave.AccessibleName = "Save";
            this.tsbSave.Image = global::METAbolt.Properties.Resources.save_16;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(51, 22);
            this.tsbSave.Text = "S&ave";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tsbClear
            // 
            this.tsbClear.AccessibleDescription = "Clear the IM window";
            this.tsbClear.AccessibleName = "Clear";
            this.tsbClear.Image = global::METAbolt.Properties.Resources.RecycleBin;
            this.tsbClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClear.Name = "tsbClear";
            this.tsbClear.Size = new System.Drawing.Size(54, 22);
            this.tsbClear.Text = "Clea&r";
            this.tsbClear.Click += new System.EventHandler(this.tsbClear_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.AccessibleDescription = "Mute the avatar you are talking to";
            this.toolStripButton1.AccessibleName = "Mute";
            this.toolStripButton1.Image = global::METAbolt.Properties.Resources.permission_i;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(55, 22);
            this.toolStripButton1.Text = "M&ute";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbTyping
            // 
            this.tsbTyping.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbTyping.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbTyping.Image = global::METAbolt.Properties.Resources.window_edit;
            this.tsbTyping.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbTyping.Name = "tsbTyping";
            this.tsbTyping.Size = new System.Drawing.Size(23, 22);
            this.tsbTyping.Text = "toolStripButton2";
            this.tsbTyping.ToolTipText = "Typing";
            this.tsbTyping.Visible = false;
            // 
            // rtbIMText
            // 
            this.rtbIMText.AccessibleDescription = "Displays inbound and the responses you have typed";
            this.rtbIMText.AccessibleName = "IM box";
            this.rtbIMText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbIMText.AutoWordSelection = true;
            this.rtbIMText.BackColor = System.Drawing.Color.White;
            this.rtbIMText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbIMText.DetectUrls = true;
            this.rtbIMText.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.rtbIMText.HideSelection = false;
            this.rtbIMText.HiglightColor = Khendys.Controls.RtfColor.Gray;
            this.rtbIMText.Location = new System.Drawing.Point(3, 28);
            this.rtbIMText.Name = "rtbIMText";
            this.rtbIMText.ReadOnly = true;
            this.rtbIMText.ShowSelectionMargin = true;
            this.rtbIMText.Size = new System.Drawing.Size(494, 272);
            this.rtbIMText.TabIndex = 4;
            this.rtbIMText.Text = "";
            this.rtbIMText.TextColor = Khendys.Controls.RtfColor.Black;
            this.rtbIMText.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtbIMText_LinkClicked_1);
            this.rtbIMText.TextChanged += new System.EventHandler(this.rtbIMText_TextChanged);
            // 
            // tbar_SendMessage
            // 
            this.tbar_SendMessage.AccessibleDescription = "List of emoticiocons to include in your message";
            this.tbar_SendMessage.AccessibleName = "Emotiocons";
            this.tbar_SendMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbar_SendMessage.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbBtn_Emoticons});
            this.tbar_SendMessage.ButtonSize = new System.Drawing.Size(15, 15);
            this.tbar_SendMessage.Divider = false;
            this.tbar_SendMessage.Dock = System.Windows.Forms.DockStyle.None;
            this.tbar_SendMessage.DropDownArrows = true;
            this.tbar_SendMessage.Location = new System.Drawing.Point(357, 304);
            this.tbar_SendMessage.Margin = new System.Windows.Forms.Padding(0);
            this.tbar_SendMessage.Name = "tbar_SendMessage";
            this.tbar_SendMessage.ShowToolTips = true;
            this.tbar_SendMessage.Size = new System.Drawing.Size(61, 64);
            this.tbar_SendMessage.TabIndex = 1;
            this.tbar_SendMessage.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            // 
            // tbBtn_Emoticons
            // 
            this.tbBtn_Emoticons.DropDownMenu = this.cmenu_Emoticons;
            this.tbBtn_Emoticons.Name = "tbBtn_Emoticons";
            this.tbBtn_Emoticons.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.tbBtn_Emoticons.Text = "{E}";
            this.tbBtn_Emoticons.ToolTipText = "Insert an emoticon";
            // 
            // IMTabWindow
            // 
            this.AccessibleName = "Private IM window";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.cbxInput);
            this.Controls.Add(this.tbar_SendMessage);
            this.Controls.Add(this.rtbIMText);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.rtbIMText_Ex);
            this.Controls.Add(this.btnSend);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "IMTabWindow";
            this.Size = new System.Drawing.Size(500, 330);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbIMText_Ex;
        private System.Windows.Forms.ComboBox cbxInput;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tbtnProfile;
        private Khendys.Controls.ExRichTextBox rtbIMText;
        private System.Windows.Forms.ContextMenu cmenu_Emoticons;
        private System.Windows.Forms.ToolBar tbar_SendMessage;
        private System.Windows.Forms.ToolBarButton tbBtn_Emoticons;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripButton tsbClear;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton tsbTyping;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
    }
}
