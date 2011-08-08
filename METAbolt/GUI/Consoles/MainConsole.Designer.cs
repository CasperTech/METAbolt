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
            this.btnInfo = new System.Windows.Forms.Button();
            this.pnlLoginPage = new System.Windows.Forms.Panel();
            this.lblInitWebBrowser = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnlLoginPrompt = new System.Windows.Forms.Panel();
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
            this.btnLogin.BackColor = System.Drawing.Color.CornflowerBlue;
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
            // btnInfo
            // 
            this.btnInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInfo.Location = new System.Drawing.Point(3, 412);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(116, 23);
            this.btnInfo.TabIndex = 18;
            this.btnInfo.Text = "Show Grid Status";
            this.btnInfo.UseVisualStyleBackColor = true;
            this.btnInfo.Visible = false;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
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
            this.pnlLoginPage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlLoginPage.Controls.Add(this.lblInitWebBrowser);
            this.pnlLoginPage.Location = new System.Drawing.Point(3, 4);
            this.pnlLoginPage.Name = "pnlLoginPage";
            this.pnlLoginPage.Size = new System.Drawing.Size(676, 358);
            this.pnlLoginPage.TabIndex = 10;
            this.pnlLoginPage.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlLoginPage_Paint);
            // 
            // lblInitWebBrowser
            // 
            this.lblInitWebBrowser.AutoSize = true;
            this.lblInitWebBrowser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInitWebBrowser.Location = new System.Drawing.Point(1, 0);
            this.lblInitWebBrowser.Name = "lblInitWebBrowser";
            this.lblInitWebBrowser.Size = new System.Drawing.Size(150, 13);
            this.lblInitWebBrowser.TabIndex = 0;
            this.lblInitWebBrowser.Text = "Initializing web browser...";
            this.lblInitWebBrowser.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1680000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pnlLoginPrompt
            // 
            this.pnlLoginPrompt.AccessibleDescription = "Enter your required login details in this area";
            this.pnlLoginPrompt.AccessibleName = "Login detail entry";
            this.pnlLoginPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLoginPrompt.BackColor = System.Drawing.Color.WhiteSmoke;
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
            this.pnlLoginPrompt.Location = new System.Drawing.Point(125, 369);
            this.pnlLoginPrompt.Name = "pnlLoginPrompt";
            this.pnlLoginPrompt.Size = new System.Drawing.Size(554, 97);
            this.pnlLoginPrompt.TabIndex = 0;
            // 
            // txtFirstName
            // 
            this.txtFirstName.AccessibleDescription = "Enter your SL login first name";
            this.txtFirstName.AccessibleName = "First Name";
            this.txtFirstName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtFirstName.Location = new System.Drawing.Point(6, 16);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(174, 21);
            this.txtFirstName.TabIndex = 1;
            this.txtFirstName.Click += new System.EventHandler(this.txtFirstName_Click);
            this.txtFirstName.Enter += new System.EventHandler(this.txtFirstName_Enter);
            // 
            // cboUserList
            // 
            this.cboUserList.AccessibleDescription = "Select user name from the list of user names you have used";
            this.cboUserList.AccessibleName = "SL user name list";
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
            this.chkCmd.Location = new System.Drawing.Point(424, 44);
            this.chkCmd.Name = "chkCmd";
            this.chkCmd.Size = new System.Drawing.Size(113, 17);
            this.chkCmd.TabIndex = 6;
            this.chkCmd.Text = "Create av BAT file";
            this.chkCmd.UseVisualStyleBackColor = true;
            // 
            // chkPWD
            // 
            this.chkPWD.AccessibleDescription = "Option to remember your password next time you start METAbolt";
            this.chkPWD.AccessibleName = "Remember password";
            this.chkPWD.AutoSize = true;
            this.chkPWD.Location = new System.Drawing.Point(275, 45);
            this.chkPWD.Name = "chkPWD";
            this.chkPWD.Size = new System.Drawing.Size(143, 17);
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
            // MainConsole
            // 
            this.AccessibleDescription = "Console to login to SL or other OpenSim based grid";
            this.AccessibleName = "Login Console";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.pnlLoginPrompt);
            this.Controls.Add(this.pnlLoggingIn);
            this.Controls.Add(this.pnlLoginPage);
            this.Controls.Add(this.btnInfo);
            this.Controls.Add(this.btnLogin);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainConsole";
            this.Size = new System.Drawing.Size(682, 466);
            this.Load += new System.EventHandler(this.MainConsole_Load);
            this.pnlLoggingIn.ResumeLayout(false);
            this.pnlLoginPage.ResumeLayout(false);
            this.pnlLoginPage.PerformLayout();
            this.pnlLoginPrompt.ResumeLayout(false);
            this.pnlLoginPrompt.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Panel pnlLoggingIn;
        private System.Windows.Forms.Label lblLoginStatus;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.Panel pnlLoginPage;
        private System.Windows.Forms.Label lblInitWebBrowser;
        private System.Windows.Forms.Timer timer1;
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
    }
}
