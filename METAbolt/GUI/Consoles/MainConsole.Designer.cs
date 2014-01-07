namespace METAbolt
{
    partial class MainConsole
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainConsole));
            this.btnLogin = new System.Windows.Forms.Button();
            this.pnlLoggingIn = new System.Windows.Forms.Panel();
            this.lblLoginStatus = new System.Windows.Forms.Label();
            this.pnlLoginPage = new System.Windows.Forms.Panel();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.pnlLoginPrompt = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.cboUserList = new System.Windows.Forms.ComboBox();
            this.chkCmd = new System.Windows.Forms.CheckBox();
            this.chkPWD = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCustomLoginUri = new System.Windows.Forms.TextBox();
            this.cbxGrid = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.cbxLocation = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.pnlLoggingIn.SuspendLayout();
            this.pnlLoginPage.SuspendLayout();
            this.pnlLoginPrompt.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.AccessibleDescription = "Click this button to login to your chosen Grid after you have entered your userna" +
    "me and password details";
            this.btnLogin.AccessibleName = "Login button";
            this.btnLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLogin.BackColor = System.Drawing.Color.DimGray;
            this.btnLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(3, 438);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(116, 26);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // pnlLoggingIn
            // 
            this.pnlLoggingIn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLoggingIn.Controls.Add(this.lblLoginStatus);
            this.pnlLoggingIn.Location = new System.Drawing.Point(125, 368);
            this.pnlLoggingIn.Name = "pnlLoggingIn";
            this.pnlLoggingIn.Size = new System.Drawing.Size(554, 97);
            this.pnlLoggingIn.TabIndex = 17;
            this.pnlLoggingIn.Visible = false;
            // 
            // lblLoginStatus
            // 
            this.lblLoginStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLoginStatus.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoginStatus.Location = new System.Drawing.Point(3, 3);
            this.lblLoginStatus.Name = "lblLoginStatus";
            this.lblLoginStatus.Size = new System.Drawing.Size(548, 51);
            this.lblLoginStatus.TabIndex = 12;
            this.lblLoginStatus.Text = "Login status goes here.";
            this.lblLoginStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlLoginPage
            // 
            this.pnlLoginPage.AccessibleDescription = "Displays the METAbolt start web page which contains useful links and summary of c" +
    "urrent forum topics etc";
            this.pnlLoginPage.AccessibleName = "METAbolt start page";
            this.pnlLoginPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLoginPage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlLoginPage.BackgroundImage")));
            this.pnlLoginPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlLoginPage.Controls.Add(this.webBrowser1);
            this.pnlLoginPage.Location = new System.Drawing.Point(3, 4);
            this.pnlLoginPage.Name = "pnlLoginPage";
            this.pnlLoginPage.Size = new System.Drawing.Size(676, 358);
            this.pnlLoginPage.TabIndex = 10;
            this.pnlLoginPage.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlLoginPage_Paint);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(676, 358);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            this.webBrowser1.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser1_Navigating);
            this.webBrowser1.NewWindow += new System.ComponentModel.CancelEventHandler(this.webBrowser1_NewWindow);
            // 
            // pnlLoginPrompt
            // 
            this.pnlLoginPrompt.AccessibleDescription = "Enter your required login details in this area";
            this.pnlLoginPrompt.AccessibleName = "Login detail entry";
            this.pnlLoginPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLoginPrompt.BackColor = System.Drawing.Color.White;
            this.pnlLoginPrompt.Controls.Add(this.button2);
            this.pnlLoginPrompt.Controls.Add(this.button1);
            this.pnlLoginPrompt.Controls.Add(this.txtFirstName);
            this.pnlLoginPrompt.Controls.Add(this.cboUserList);
            this.pnlLoginPrompt.Controls.Add(this.chkCmd);
            this.pnlLoginPrompt.Controls.Add(this.chkPWD);
            this.pnlLoginPrompt.Controls.Add(this.label6);
            this.pnlLoginPrompt.Controls.Add(this.txtCustomLoginUri);
            this.pnlLoginPrompt.Controls.Add(this.cbxGrid);
            this.pnlLoginPrompt.Controls.Add(this.label5);
            this.pnlLoginPrompt.Controls.Add(this.label1);
            this.pnlLoginPrompt.Controls.Add(this.label4);
            this.pnlLoginPrompt.Controls.Add(this.txtLastName);
            this.pnlLoginPrompt.Controls.Add(this.label2);
            this.pnlLoginPrompt.Controls.Add(this.txtPassword);
            this.pnlLoginPrompt.Controls.Add(this.cbxLocation);
            this.pnlLoginPrompt.Controls.Add(this.label3);
            this.pnlLoginPrompt.Location = new System.Drawing.Point(124, 368);
            this.pnlLoginPrompt.Name = "pnlLoginPrompt";
            this.pnlLoginPrompt.Size = new System.Drawing.Size(554, 97);
            this.pnlLoginPrompt.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.BackgroundImage = global::METAbolt.Properties.Resources.rebake;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Location = new System.Drawing.Point(243, 68);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(23, 23);
            this.button2.TabIndex = 17;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::METAbolt.Properties.Resources.window_edit;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Location = new System.Drawing.Point(219, 68);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(23, 23);
            this.button1.TabIndex = 16;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtFirstName
            // 
            this.txtFirstName.AccessibleDescription = "Enter your SL login first name";
            this.txtFirstName.AccessibleName = "First Name";
            this.txtFirstName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtFirstName.Location = new System.Drawing.Point(6, 16);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtFirstName.Size = new System.Drawing.Size(174, 21);
            this.txtFirstName.TabIndex = 1;
            this.txtFirstName.Click += new System.EventHandler(this.txtFirstName_Click);
            this.txtFirstName.Enter += new System.EventHandler(this.txtFirstName_Enter);
            // 
            // cboUserList
            // 
            this.cboUserList.AccessibleDescription = "Select user name from the list of user names you have used";
            this.cboUserList.AccessibleName = "SL user name list";
            this.cboUserList.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboUserList.FormattingEnabled = true;
            this.cboUserList.Location = new System.Drawing.Point(6, 16);
            this.cboUserList.Name = "cboUserList";
            this.cboUserList.Size = new System.Drawing.Size(191, 21);
            this.cboUserList.TabIndex = 15;
            this.cboUserList.SelectedIndexChanged += new System.EventHandler(this.cboUserList_SelectedIndexChanged);
            // 
            // chkCmd
            // 
            this.chkCmd.AccessibleDescription = "Option to create a BAT file for the avatar you are logging in with next time you " +
    "start METAbolt";
            this.chkCmd.AccessibleName = "Create BAT file";
            this.chkCmd.AutoSize = true;
            this.chkCmd.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.chkCmd.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightSteelBlue;
            this.chkCmd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.chkCmd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkCmd.Location = new System.Drawing.Point(424, 44);
            this.chkCmd.Name = "chkCmd";
            this.chkCmd.Size = new System.Drawing.Size(110, 17);
            this.chkCmd.TabIndex = 6;
            this.chkCmd.Text = "Create av BAT file";
            this.chkCmd.UseVisualStyleBackColor = true;
            // 
            // chkPWD
            // 
            this.chkPWD.AccessibleDescription = "Option to remember your password next time you start METAbolt";
            this.chkPWD.AccessibleName = "Remember password";
            this.chkPWD.AutoSize = true;
            this.chkPWD.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.chkPWD.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightSteelBlue;
            this.chkPWD.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.chkPWD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkPWD.Location = new System.Drawing.Point(275, 45);
            this.chkPWD.Name = "chkPWD";
            this.chkPWD.Size = new System.Drawing.Size(140, 17);
            this.chkPWD.TabIndex = 5;
            this.chkPWD.Text = "Remember my password";
            this.chkPWD.UseVisualStyleBackColor = true;
            this.chkPWD.CheckedChanged += new System.EventHandler(this.chkPWD_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(272, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Login Uri";
            // 
            // txtCustomLoginUri
            // 
            this.txtCustomLoginUri.AccessibleDescription = "Enter the URI of the \"other\" Grid you wish to login to";
            this.txtCustomLoginUri.AccessibleName = "URI";
            this.txtCustomLoginUri.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtCustomLoginUri.Enabled = false;
            this.txtCustomLoginUri.Location = new System.Drawing.Point(326, 70);
            this.txtCustomLoginUri.Name = "txtCustomLoginUri";
            this.txtCustomLoginUri.Size = new System.Drawing.Size(224, 21);
            this.txtCustomLoginUri.TabIndex = 8;
            // 
            // cbxGrid
            // 
            this.cbxGrid.AccessibleDescription = "Select the name of the Grid you wish to connect to";
            this.cbxGrid.AccessibleName = "Grid list";
            this.cbxGrid.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cbxGrid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxGrid.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbxGrid.FormattingEnabled = true;
            this.cbxGrid.Items.AddRange(new object[] {
            "SL Main Grid (Agni)",
            "SL Beta Grid (Aditi)",
            "Other"});
            this.cbxGrid.Location = new System.Drawing.Point(56, 70);
            this.cbxGrid.Name = "cbxGrid";
            this.cbxGrid.Size = new System.Drawing.Size(210, 21);
            this.cbxGrid.TabIndex = 7;
            this.cbxGrid.SelectedIndexChanged += new System.EventHandler(this.cbxGrid_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Grid";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "First Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(200, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Last Name";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // txtLastName
            // 
            this.txtLastName.AccessibleDescription = "Enter your SL login last name";
            this.txtLastName.AccessibleName = "Last name";
            this.txtLastName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtLastName.Location = new System.Drawing.Point(203, 16);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(164, 21);
            this.txtLastName.TabIndex = 2;
            this.txtLastName.Click += new System.EventHandler(this.txtLastName_Click);
            this.txtLastName.TextChanged += new System.EventHandler(this.txtLastName_TextChanged);
            this.txtLastName.Enter += new System.EventHandler(this.txtLastName_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(370, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.AccessibleDescription = "Ebter your SL password";
            this.txtPassword.AccessibleName = "Password";
            this.txtPassword.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtPassword.Location = new System.Drawing.Point(373, 16);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(177, 21);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.Click += new System.EventHandler(this.txtPassword_Click);
            this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
            // 
            // cbxLocation
            // 
            this.cbxLocation.AccessibleDescription = "Select the location you wish to login to";
            this.cbxLocation.AccessibleName = "Location dropbox";
            this.cbxLocation.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cbxLocation.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbxLocation.FormattingEnabled = true;
            this.cbxLocation.Items.AddRange(new object[] {
            "My Home",
            "My Last Location"});
            this.cbxLocation.Location = new System.Drawing.Point(56, 43);
            this.cbxLocation.Name = "cbxLocation";
            this.cbxLocation.Size = new System.Drawing.Size(210, 21);
            this.cbxLocation.TabIndex = 4;
            this.cbxLocation.SelectedIndexChanged += new System.EventHandler(this.cbxLocation_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Location";
            // 
            // timer2
            // 
            this.timer2.Interval = 305000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 7.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label7.Location = new System.Drawing.Point(4, 365);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 12);
            this.label7.TabIndex = 18;
            this.label7.Text = "version";
            // 
            // MainConsole
            // 
            this.AccessibleDescription = "Console to login to SL or other OpenSim based grid";
            this.AccessibleName = "Login Console";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pnlLoginPrompt);
            this.Controls.Add(this.pnlLoggingIn);
            this.Controls.Add(this.pnlLoginPage);
            this.Controls.Add(this.btnLogin);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainConsole";
            this.Size = new System.Drawing.Size(682, 466);
            this.Load += new System.EventHandler(this.MainConsole_Load);
            this.pnlLoggingIn.ResumeLayout(false);
            this.pnlLoginPage.ResumeLayout(false);
            this.pnlLoginPrompt.ResumeLayout(false);
            this.pnlLoginPrompt.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Panel pnlLoggingIn;
        private System.Windows.Forms.Label lblLoginStatus;
        private System.Windows.Forms.Panel pnlLoginPage;
        private System.Windows.Forms.Panel pnlLoginPrompt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCustomLoginUri;
        private System.Windows.Forms.ComboBox cbxGrid;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.ComboBox cbxLocation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkPWD;
        private System.Windows.Forms.CheckBox chkCmd;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ComboBox cboUserList;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}
