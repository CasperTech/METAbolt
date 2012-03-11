namespace METAbolt
{
    partial class IMTabWindowGroup
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
                WaitForSessionStart.Close();  
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
            this.tbar_SendMessage = new System.Windows.Forms.ToolBar();
            this.tbBtn_Emoticons = new System.Windows.Forms.ToolBarButton();
            this.cmenu_Emoticons = new System.Windows.Forms.ContextMenu();
            this.rtbIMText = new Khendys.Controls.ExRichTextBox();
            this.tbtnProfile = new System.Windows.Forms.ToolStripButton();
            this.cbxInput = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.btnSend = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvwList = new System.Windows.Forms.ListView();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbar_SendMessage
            // 
            this.tbar_SendMessage.AccessibleDescription = "List of emotiocons to include in your message";
            this.tbar_SendMessage.AccessibleName = "Emotiocons";
            this.tbar_SendMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbar_SendMessage.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbBtn_Emoticons});
            this.tbar_SendMessage.ButtonSize = new System.Drawing.Size(15, 15);
            this.tbar_SendMessage.Divider = false;
            this.tbar_SendMessage.Dock = System.Windows.Forms.DockStyle.None;
            this.tbar_SendMessage.DropDownArrows = true;
            this.tbar_SendMessage.Location = new System.Drawing.Point(0, 0);
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
            // rtbIMText
            // 
            this.rtbIMText.AccessibleDescription = "IM window where incoming and outgoing IMs are displayed";
            this.rtbIMText.AccessibleName = "IM Window";
            this.rtbIMText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbIMText.AutoWordSelection = true;
            this.rtbIMText.BackColor = System.Drawing.Color.White;
            this.rtbIMText.DetectUrls = true;
            this.rtbIMText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbIMText.HideSelection = false;
            this.rtbIMText.HiglightColor = Khendys.Controls.RtfColor.Gray;
            this.rtbIMText.Location = new System.Drawing.Point(0, 4);
            this.rtbIMText.Name = "rtbIMText";
            this.rtbIMText.ReadOnly = true;
            this.rtbIMText.ShowSelectionMargin = true;
            this.rtbIMText.Size = new System.Drawing.Size(380, 264);
            this.rtbIMText.TabIndex = 4;
            this.rtbIMText.Text = "";
            this.rtbIMText.TextColor = Khendys.Controls.RtfColor.Black;
            this.rtbIMText.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtbIMText_LinkClicked_1);
            this.rtbIMText.TextChanged += new System.EventHandler(this.rtbIMText_TextChanged);
            // 
            // tbtnProfile
            // 
            this.tbtnProfile.AccessibleDescription = "Display the group information window for this group";
            this.tbtnProfile.AccessibleName = "Group Information";
            this.tbtnProfile.Image = global::METAbolt.Properties.Resources.applications_16;
            this.tbtnProfile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnProfile.Name = "tbtnProfile";
            this.tbtnProfile.Size = new System.Drawing.Size(84, 22);
            this.tbtnProfile.Text = "Group &info";
            this.tbtnProfile.Click += new System.EventHandler(this.tbtnProfile_Click);
            // 
            // cbxInput
            // 
            this.cbxInput.AccessibleDescription = "Type your message here to send";
            this.cbxInput.AccessibleName = "IM output";
            this.cbxInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxInput.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cbxInput.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbxInput.FormattingEnabled = true;
            this.cbxInput.Location = new System.Drawing.Point(3, 308);
            this.cbxInput.Name = "cbxInput";
            this.cbxInput.Size = new System.Drawing.Size(372, 21);
            this.cbxInput.TabIndex = 0;
            this.cbxInput.SelectedIndexChanged += new System.EventHandler(this.cbxInput_SelectedIndexChanged);
            this.cbxInput.TextChanged += new System.EventHandler(this.cbxInput_TextChanged_1);
            this.cbxInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbxInput_KeyDown_1);
            this.cbxInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cbxInput_KeyUp_1);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.tbar_SendMessage);
            this.panel2.Location = new System.Drawing.Point(355, 305);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(61, 24);
            this.panel2.TabIndex = 14;
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
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(500, 25);
            this.toolStrip1.TabIndex = 12;
            this.toolStrip1.Text = "toolStrip1";
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
            this.tsbSave.AccessibleDescription = "Save the contents of the IM window into logs";
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
            this.toolStripButton1.Image = global::METAbolt.Properties.Resources.avs;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(117, 22);
            this.toolStripButton1.Text = "Hide participants";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // btnSend
            // 
            this.btnSend.AccessibleDescription = "Send your IM";
            this.btnSend.AccessibleName = "Send";
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnSend.Enabled = false;
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Location = new System.Drawing.Point(422, 306);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(4, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(376, 18);
            this.label1.TabIndex = 15;
            this.label1.Text = "Initialising group chat session...";
            this.label1.Visible = false;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvwList);
            this.splitContainer1.Panel1MinSize = 0;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.rtbIMText);
            this.splitContainer1.Size = new System.Drawing.Size(497, 274);
            this.splitContainer1.SplitterDistance = 107;
            this.splitContainer1.TabIndex = 17;
            // 
            // lvwList
            // 
            this.lvwList.AccessibleDescription = "The list of participants for this group IM session";
            this.lvwList.AccessibleName = "Participants";
            this.lvwList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwList.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lvwList.GridLines = true;
            this.lvwList.Location = new System.Drawing.Point(4, 3);
            this.lvwList.MultiSelect = false;
            this.lvwList.Name = "lvwList";
            this.lvwList.Size = new System.Drawing.Size(101, 265);
            this.lvwList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvwList.TabIndex = 3;
            this.lvwList.UseCompatibleStateImageBehavior = false;
            this.lvwList.View = System.Windows.Forms.View.List;
            this.lvwList.SelectedIndexChanged += new System.EventHandler(this.lvwList_SelectedIndexChanged);
            this.lvwList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvwList_MouseDoubleClick);
            this.lvwList.MouseEnter += new System.EventHandler(this.lvwList_MouseEnter);
            this.lvwList.MouseLeave += new System.EventHandler(this.lvwList_MouseLeave);
            // 
            // IMTabWindowGroup
            // 
            this.AccessibleName = "Group IM window";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.cbxInput);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnSend);
            this.Name = "IMTabWindowGroup";
            this.Size = new System.Drawing.Size(500, 330);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolBar tbar_SendMessage;
        private System.Windows.Forms.ToolBarButton tbBtn_Emoticons;
        private System.Windows.Forms.ContextMenu cmenu_Emoticons;
        private Khendys.Controls.ExRichTextBox rtbIMText;
        private System.Windows.Forms.ToolStripButton tbtnProfile;
        private System.Windows.Forms.ComboBox cbxInput;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripButton tsbClear;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView lvwList;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
    }
}
