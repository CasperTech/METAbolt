namespace METAbolt
{
    partial class frmTranslate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTranslate));
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.txtTo = new System.Windows.Forms.RichTextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.imgFlags = new System.Windows.Forms.ImageList(this.components);
            this.cboLanguage = new METAbolt.ComboEx();
            this.SuspendLayout();
            // 
            // txtFrom
            // 
            this.txtFrom.AccessibleName = "Text to be translated textbox";
            this.txtFrom.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtFrom.Location = new System.Drawing.Point(12, 27);
            this.txtFrom.Multiline = true;
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFrom.Size = new System.Drawing.Size(435, 127);
            this.txtFrom.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.AccessibleName = "Translate button";
            this.button1.BackColor = System.Drawing.Color.RoyalBlue;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(372, 160);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "&Translate";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Enter the text to be translated:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Translation";
            // 
            // button2
            // 
            this.button2.AccessibleName = "Close this window button";
            this.button2.BackColor = System.Drawing.Color.RoyalBlue;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(372, 323);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "&Close";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtTo
            // 
            this.txtTo.AccessibleName = "Translation result textbox";
            this.txtTo.BackColor = System.Drawing.Color.AliceBlue;
            this.txtTo.Location = new System.Drawing.Point(12, 186);
            this.txtTo.Name = "txtTo";
            this.txtTo.ReadOnly = true;
            this.txtTo.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.txtTo.Size = new System.Drawing.Size(435, 131);
            this.txtTo.TabIndex = 3;
            this.txtTo.Text = "";
            // 
            // button3
            // 
            this.button3.AccessibleName = "Copy translation to chat input box button";
            this.button3.BackColor = System.Drawing.Color.RoyalBlue;
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(278, 323);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "C&opy 2 Say";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // imgFlags
            // 
            this.imgFlags.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgFlags.ImageStream")));
            this.imgFlags.TransparentColor = System.Drawing.Color.Transparent;
            this.imgFlags.Images.SetKeyName(0, "33.png");
            this.imgFlags.Images.SetKeyName(1, "1.png");
            this.imgFlags.Images.SetKeyName(2, "2.png");
            this.imgFlags.Images.SetKeyName(3, "3.png");
            this.imgFlags.Images.SetKeyName(4, "4.png");
            this.imgFlags.Images.SetKeyName(5, "5.png");
            this.imgFlags.Images.SetKeyName(6, "6.png");
            this.imgFlags.Images.SetKeyName(7, "7.png");
            this.imgFlags.Images.SetKeyName(8, "8.png");
            this.imgFlags.Images.SetKeyName(9, "9.png");
            this.imgFlags.Images.SetKeyName(10, "10.png");
            this.imgFlags.Images.SetKeyName(11, "11.png");
            this.imgFlags.Images.SetKeyName(12, "12.png");
            this.imgFlags.Images.SetKeyName(13, "13.png");
            this.imgFlags.Images.SetKeyName(14, "14.png");
            this.imgFlags.Images.SetKeyName(15, "15.png");
            this.imgFlags.Images.SetKeyName(16, "16.png");
            this.imgFlags.Images.SetKeyName(17, "17.png");
            this.imgFlags.Images.SetKeyName(18, "18.png");
            this.imgFlags.Images.SetKeyName(19, "19.png");
            this.imgFlags.Images.SetKeyName(20, "20.png");
            this.imgFlags.Images.SetKeyName(21, "21.png");
            this.imgFlags.Images.SetKeyName(22, "22.png");
            this.imgFlags.Images.SetKeyName(23, "23.png");
            this.imgFlags.Images.SetKeyName(24, "24.png");
            this.imgFlags.Images.SetKeyName(25, "25.png");
            this.imgFlags.Images.SetKeyName(26, "26.png");
            this.imgFlags.Images.SetKeyName(27, "27.png");
            this.imgFlags.Images.SetKeyName(28, "28.png");
            this.imgFlags.Images.SetKeyName(29, "29.png");
            this.imgFlags.Images.SetKeyName(30, "30.png");
            this.imgFlags.Images.SetKeyName(31, "31.png");
            this.imgFlags.Images.SetKeyName(32, "32.png");
            this.imgFlags.Images.SetKeyName(33, "33.png");
            // 
            // cboLanguage
            // 
            this.cboLanguage.AccessibleName = "Language selection dropdown box";
            this.cboLanguage.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cboLanguage.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboLanguage.FormattingEnabled = true;
            this.cboLanguage.ICImageList = this.imgFlags;
            this.cboLanguage.Location = new System.Drawing.Point(152, 161);
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.Size = new System.Drawing.Size(208, 21);
            this.cboLanguage.TabIndex = 1;
            this.cboLanguage.SelectedIndexChanged += new System.EventHandler(this.cboLanguage_SelectedIndexChanged);
            // 
            // frmTranslate
            // 
            this.AccessibleName = "Translation window";
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(459, 352);
            this.Controls.Add(this.cboLanguage);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.txtTo);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtFrom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmTranslate";
            this.Text = "METAtranslate";
            this.Load += new System.EventHandler(this.frmTranslate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFrom;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox txtTo;
        private System.Windows.Forms.Button button3;
        private ComboEx cboLanguage;
        private System.Windows.Forms.ImageList imgFlags;
    }
}