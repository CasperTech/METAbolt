namespace METAbolt
{
    partial class frmDebugLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDebugLog));
            this.tabLogs = new System.Windows.Forms.TabControl();
            this.tpgInfo = new System.Windows.Forms.TabPage();
            this.rtbInfo = new System.Windows.Forms.RichTextBox();
            this.tpgWarning = new System.Windows.Forms.TabPage();
            this.rtbWarning = new System.Windows.Forms.RichTextBox();
            this.tpgError = new System.Windows.Forms.TabPage();
            this.rtbError = new System.Windows.Forms.RichTextBox();
            this.tpgDebug = new System.Windows.Forms.TabPage();
            this.rtbDebug = new System.Windows.Forms.RichTextBox();
            this.tpgMonitor = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.dataChart1 = new SystemMonitor.DataChart();
            this.label9 = new System.Windows.Forms.Label();
            this.dataChart3 = new SystemMonitor.DataChart();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tpgIP = new System.Windows.Forms.TabPage();
            this.rtBox1 = new System.Windows.Forms.RichTextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabLogs.SuspendLayout();
            this.tpgInfo.SuspendLayout();
            this.tpgWarning.SuspendLayout();
            this.tpgError.SuspendLayout();
            this.tpgDebug.SuspendLayout();
            this.tpgMonitor.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tpgIP.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabLogs
            // 
            this.tabLogs.AccessibleDescription = "Information, warning, error, debug and TCP/IP tabs";
            this.tabLogs.AccessibleName = "Tabs";
            this.tabLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabLogs.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabLogs.Controls.Add(this.tpgInfo);
            this.tabLogs.Controls.Add(this.tpgWarning);
            this.tabLogs.Controls.Add(this.tpgError);
            this.tabLogs.Controls.Add(this.tpgDebug);
            this.tabLogs.Controls.Add(this.tpgMonitor);
            this.tabLogs.Controls.Add(this.tpgIP);
            this.tabLogs.Location = new System.Drawing.Point(12, 12);
            this.tabLogs.Name = "tabLogs";
            this.tabLogs.SelectedIndex = 0;
            this.tabLogs.Size = new System.Drawing.Size(548, 385);
            this.tabLogs.TabIndex = 0;
            // 
            // tpgInfo
            // 
            this.tpgInfo.AccessibleDescription = "Information on general activity";
            this.tpgInfo.AccessibleName = "Information tab";
            this.tpgInfo.Controls.Add(this.rtbInfo);
            this.tpgInfo.Location = new System.Drawing.Point(4, 25);
            this.tpgInfo.Name = "tpgInfo";
            this.tpgInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpgInfo.Size = new System.Drawing.Size(540, 356);
            this.tpgInfo.TabIndex = 0;
            this.tpgInfo.Text = "Info";
            this.tpgInfo.UseVisualStyleBackColor = true;
            // 
            // rtbInfo
            // 
            this.rtbInfo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rtbInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbInfo.Location = new System.Drawing.Point(3, 3);
            this.rtbInfo.Name = "rtbInfo";
            this.rtbInfo.ReadOnly = true;
            this.rtbInfo.Size = new System.Drawing.Size(534, 350);
            this.rtbInfo.TabIndex = 0;
            this.rtbInfo.Text = "";
            this.rtbInfo.WordWrap = false;
            // 
            // tpgWarning
            // 
            this.tpgWarning.Controls.Add(this.rtbWarning);
            this.tpgWarning.Location = new System.Drawing.Point(4, 25);
            this.tpgWarning.Name = "tpgWarning";
            this.tpgWarning.Padding = new System.Windows.Forms.Padding(3);
            this.tpgWarning.Size = new System.Drawing.Size(540, 356);
            this.tpgWarning.TabIndex = 1;
            this.tpgWarning.Text = "Warning";
            this.tpgWarning.UseVisualStyleBackColor = true;
            // 
            // rtbWarning
            // 
            this.rtbWarning.AccessibleDescription = "All warnings";
            this.rtbWarning.AccessibleName = "Warning tab";
            this.rtbWarning.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rtbWarning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbWarning.Location = new System.Drawing.Point(3, 3);
            this.rtbWarning.Name = "rtbWarning";
            this.rtbWarning.ReadOnly = true;
            this.rtbWarning.Size = new System.Drawing.Size(534, 350);
            this.rtbWarning.TabIndex = 0;
            this.rtbWarning.Text = "";
            this.rtbWarning.WordWrap = false;
            // 
            // tpgError
            // 
            this.tpgError.Controls.Add(this.rtbError);
            this.tpgError.Location = new System.Drawing.Point(4, 25);
            this.tpgError.Name = "tpgError";
            this.tpgError.Padding = new System.Windows.Forms.Padding(3);
            this.tpgError.Size = new System.Drawing.Size(540, 356);
            this.tpgError.TabIndex = 2;
            this.tpgError.Text = "Error";
            this.tpgError.UseVisualStyleBackColor = true;
            // 
            // rtbError
            // 
            this.rtbError.AccessibleDescription = "All errors that have occured";
            this.rtbError.AccessibleName = "Error tab";
            this.rtbError.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rtbError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbError.Location = new System.Drawing.Point(3, 3);
            this.rtbError.Name = "rtbError";
            this.rtbError.ReadOnly = true;
            this.rtbError.Size = new System.Drawing.Size(534, 350);
            this.rtbError.TabIndex = 0;
            this.rtbError.Text = "";
            this.rtbError.WordWrap = false;
            // 
            // tpgDebug
            // 
            this.tpgDebug.AccessibleDescription = "All debug information";
            this.tpgDebug.AccessibleName = "Debug tab";
            this.tpgDebug.Controls.Add(this.rtbDebug);
            this.tpgDebug.Location = new System.Drawing.Point(4, 25);
            this.tpgDebug.Name = "tpgDebug";
            this.tpgDebug.Padding = new System.Windows.Forms.Padding(3);
            this.tpgDebug.Size = new System.Drawing.Size(540, 356);
            this.tpgDebug.TabIndex = 3;
            this.tpgDebug.Text = "Debug";
            this.tpgDebug.UseVisualStyleBackColor = true;
            // 
            // rtbDebug
            // 
            this.rtbDebug.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rtbDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDebug.Location = new System.Drawing.Point(3, 3);
            this.rtbDebug.Name = "rtbDebug";
            this.rtbDebug.ReadOnly = true;
            this.rtbDebug.Size = new System.Drawing.Size(534, 350);
            this.rtbDebug.TabIndex = 0;
            this.rtbDebug.Text = "";
            this.rtbDebug.WordWrap = false;
            // 
            // tpgMonitor
            // 
            this.tpgMonitor.BackColor = System.Drawing.Color.White;
            this.tpgMonitor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tpgMonitor.Controls.Add(this.label14);
            this.tpgMonitor.Controls.Add(this.label13);
            this.tpgMonitor.Controls.Add(this.label11);
            this.tpgMonitor.Controls.Add(this.dataChart1);
            this.tpgMonitor.Controls.Add(this.label9);
            this.tpgMonitor.Controls.Add(this.dataChart3);
            this.tpgMonitor.Controls.Add(this.label8);
            this.tpgMonitor.Controls.Add(this.label7);
            this.tpgMonitor.Controls.Add(this.label6);
            this.tpgMonitor.Controls.Add(this.label5);
            this.tpgMonitor.Controls.Add(this.label3);
            this.tpgMonitor.Controls.Add(this.label2);
            this.tpgMonitor.Controls.Add(this.panel1);
            this.tpgMonitor.Controls.Add(this.label12);
            this.tpgMonitor.Controls.Add(this.label10);
            this.tpgMonitor.Location = new System.Drawing.Point(4, 25);
            this.tpgMonitor.Name = "tpgMonitor";
            this.tpgMonitor.Size = new System.Drawing.Size(540, 356);
            this.tpgMonitor.TabIndex = 5;
            this.tpgMonitor.Text = "Network Monitor";
            this.tpgMonitor.Click += new System.EventHandler(this.tpgMonitor_Click);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(433, 50);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(100, 13);
            this.label14.TabIndex = 43;
            this.label14.Text = "00:00:00";
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Gray;
            this.label13.Location = new System.Drawing.Point(513, 306);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(13, 13);
            this.label13.TabIndex = 42;
            this.label13.Text = "0";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.Gray;
            this.label11.Location = new System.Drawing.Point(511, 176);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(13, 13);
            this.label11.TabIndex = 41;
            this.label11.Text = "0";
            // 
            // dataChart1
            // 
            this.dataChart1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataChart1.BackColor = System.Drawing.Color.Black;
            this.dataChart1.ChartType = SystemMonitor.ChartType.Stick;
            this.dataChart1.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataChart1.GridColor = System.Drawing.Color.SeaGreen;
            this.dataChart1.GridPixels = 8;
            this.dataChart1.InitialHeight = 100;
            this.dataChart1.LineColor = System.Drawing.Color.GreenYellow;
            this.dataChart1.Location = new System.Drawing.Point(21, 214);
            this.dataChart1.Name = "dataChart1";
            this.dataChart1.Size = new System.Drawing.Size(490, 105);
            this.dataChart1.TabIndex = 34;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Gray;
            this.label9.Location = new System.Drawing.Point(513, 212);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(25, 13);
            this.label9.TabIndex = 40;
            this.label9.Text = "100";
            // 
            // dataChart3
            // 
            this.dataChart3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataChart3.BackColor = System.Drawing.Color.Silver;
            this.dataChart3.ChartType = SystemMonitor.ChartType.Stick;
            this.dataChart3.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataChart3.GridColor = System.Drawing.Color.Yellow;
            this.dataChart3.GridPixels = 8;
            this.dataChart3.InitialHeight = 100;
            this.dataChart3.LineColor = System.Drawing.Color.Green;
            this.dataChart3.Location = new System.Drawing.Point(21, 84);
            this.dataChart3.Name = "dataChart3";
            this.dataChart3.Size = new System.Drawing.Size(490, 105);
            this.dataChart3.TabIndex = 31;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Gray;
            this.label8.Location = new System.Drawing.Point(511, 82);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 13);
            this.label8.TabIndex = 39;
            this.label8.Text = "250";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(319, 335);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(192, 13);
            this.label7.TabIndex = 38;
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(316, 322);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(195, 13);
            this.label6.TabIndex = 37;
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label5.Location = new System.Drawing.Point(20, 335);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 13);
            this.label5.TabIndex = 36;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.ForestGreen;
            this.label3.Location = new System.Drawing.Point(20, 322);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 35;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(219, 198);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 13);
            this.label2.TabIndex = 33;
            this.label2.Text = "Bandwidth Usage (KB/Sec)";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(530, 40);
            this.panel1.TabIndex = 32;
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.BackColor = System.Drawing.Color.DimGray;
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(396, 8);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(59, 23);
            this.button4.TabIndex = 1;
            this.button4.Text = "start";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.BackColor = System.Drawing.Color.DimGray;
            this.button5.Enabled = false;
            this.button5.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.button5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(461, 8);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(59, 23);
            this.button5.TabIndex = 2;
            this.button5.Text = "stop";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label4.Location = new System.Drawing.Point(14, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 5;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label12.Location = new System.Drawing.Point(249, 68);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(69, 13);
            this.label12.TabIndex = 30;
            this.label12.Text = "Latency (ms)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Gray;
            this.label10.Location = new System.Drawing.Point(18, 50);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(0, 13);
            this.label10.TabIndex = 12;
            // 
            // tpgIP
            // 
            this.tpgIP.AccessibleDescription = "Utility to ping and dsiplay IP and domain information";
            this.tpgIP.AccessibleName = "TCP/IP tab";
            this.tpgIP.BackColor = System.Drawing.Color.White;
            this.tpgIP.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tpgIP.Controls.Add(this.rtBox1);
            this.tpgIP.Controls.Add(this.button3);
            this.tpgIP.Controls.Add(this.label1);
            this.tpgIP.Controls.Add(this.button1);
            this.tpgIP.Controls.Add(this.textBox1);
            this.tpgIP.Location = new System.Drawing.Point(4, 25);
            this.tpgIP.Name = "tpgIP";
            this.tpgIP.Size = new System.Drawing.Size(540, 356);
            this.tpgIP.TabIndex = 4;
            this.tpgIP.Text = "TCP/IP - Ping";
            // 
            // rtBox1
            // 
            this.rtBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtBox1.BackColor = System.Drawing.Color.Black;
            this.rtBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtBox1.ForeColor = System.Drawing.Color.Aqua;
            this.rtBox1.Location = new System.Drawing.Point(7, 39);
            this.rtBox1.Name = "rtBox1";
            this.rtBox1.ReadOnly = true;
            this.rtBox1.Size = new System.Drawing.Size(518, 310);
            this.rtBox1.TabIndex = 3;
            this.rtBox1.Text = "";
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.BackColor = System.Drawing.Color.DimGray;
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(475, 10);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(50, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Ping";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "URL/IP:";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.DimGray;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(419, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(55, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Display";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(48, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(365, 21);
            this.textBox1.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.DimGray;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(485, 403);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.BackColor = System.Drawing.Color.DimGray;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(12, 403);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cl&ear logs";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmDebugLog
            // 
            this.AccessibleDescription = "Displays warning, errors and debug information to help troubleshooting";
            this.AccessibleName = "Debug window";
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(572, 438);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabLogs);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDebugLog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Debug Log - METAbolt";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDebugLog_FormClosing);
            this.Load += new System.EventHandler(this.frmDebugLog_Load);
            this.Shown += new System.EventHandler(this.frmDebugLog_Shown);
            this.tabLogs.ResumeLayout(false);
            this.tpgInfo.ResumeLayout(false);
            this.tpgWarning.ResumeLayout(false);
            this.tpgError.ResumeLayout(false);
            this.tpgDebug.ResumeLayout(false);
            this.tpgMonitor.ResumeLayout(false);
            this.tpgMonitor.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tpgIP.ResumeLayout(false);
            this.tpgIP.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabLogs;
        private System.Windows.Forms.TabPage tpgInfo;
        private System.Windows.Forms.TabPage tpgWarning;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabPage tpgError;
        private System.Windows.Forms.TabPage tpgDebug;
        private System.Windows.Forms.RichTextBox rtbInfo;
        private System.Windows.Forms.RichTextBox rtbWarning;
        private System.Windows.Forms.RichTextBox rtbError;
        private System.Windows.Forms.RichTextBox rtbDebug;
        private System.Windows.Forms.TabPage tpgIP;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.RichTextBox rtBox1;
        private System.Windows.Forms.TabPage tpgMonitor;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private SystemMonitor.DataChart dataChart3;
        private System.Windows.Forms.Panel panel1;
        private SystemMonitor.DataChart dataChart1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label14;
    }
}