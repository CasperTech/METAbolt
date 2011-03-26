namespace METAbolt
{
    partial class InventoryConsole
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryConsole));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tstInventory = new System.Windows.Forms.ToolStrip();
            this.tbtnNew = new System.Windows.Forms.ToolStripDropDownButton();
            this.tmnuNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tmnuNewLandmark = new System.Windows.Forms.ToolStripMenuItem();
            this.tmnuNewNotecard = new System.Windows.Forms.ToolStripMenuItem();
            this.tmnuNewScript = new System.Windows.Forms.ToolStripMenuItem();
            this.snapshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbtnOrganize = new System.Windows.Forms.ToolStripDropDownButton();
            this.tmnuCut = new System.Windows.Forms.ToolStripMenuItem();
            this.tmnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tmnuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.tmnuRename = new System.Windows.Forms.ToolStripMenuItem();
            this.tmnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.expandInventoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unExpandInventoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbtnSort = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.button6 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tstInventory.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.tstInventory);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Size = new System.Drawing.Size(632, 432);
            this.splitContainer1.SplitterDistance = 301;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.CausesValidation = false;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.LabelEdit = true;
            this.treeView1.Location = new System.Drawing.Point(0, 25);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(297, 370);
            this.treeView1.TabIndex = 3;
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            this.treeView1.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView1_AfterLabelEdit);
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 395);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(297, 33);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(4, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(22, 21);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DarkGreen;
            this.button1.Enabled = false;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(273, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(51, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "search";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(29, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(238, 21);
            this.textBox1.TabIndex = 4;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.Click += new System.EventHandler(this.textBox1_Click);
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
            this.textBox1.Enter += new System.EventHandler(this.textBox1_Enter);
            // 
            // tstInventory
            // 
            this.tstInventory.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tstInventory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbtnNew,
            this.toolStripSeparator1,
            this.tbtnOrganize,
            this.tbtnSort,
            this.toolStripButton2});
            this.tstInventory.Location = new System.Drawing.Point(0, 0);
            this.tstInventory.Name = "tstInventory";
            this.tstInventory.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.tstInventory.Size = new System.Drawing.Size(297, 25);
            this.tstInventory.TabIndex = 2;
            this.tstInventory.Text = "toolStrip1";
            // 
            // tbtnNew
            // 
            this.tbtnNew.AutoToolTip = false;
            this.tbtnNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmnuNewFolder,
            this.toolStripMenuItem1,
            this.tmnuNewLandmark,
            this.tmnuNewNotecard,
            this.tmnuNewScript,
            this.snapshotToolStripMenuItem});
            this.tbtnNew.Enabled = false;
            this.tbtnNew.Image = global::METAbolt.Properties.Resources.add_16;
            this.tbtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnNew.Name = "tbtnNew";
            this.tbtnNew.Size = new System.Drawing.Size(60, 22);
            this.tbtnNew.Text = "New";
            // 
            // tmnuNewFolder
            // 
            this.tmnuNewFolder.Image = global::METAbolt.Properties.Resources.folder_closed_16;
            this.tmnuNewFolder.Name = "tmnuNewFolder";
            this.tmnuNewFolder.Size = new System.Drawing.Size(127, 22);
            this.tmnuNewFolder.Text = "Folder";
            this.tmnuNewFolder.Visible = false;
            this.tmnuNewFolder.Click += new System.EventHandler(this.tmnuNewFolder_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(124, 6);
            this.toolStripMenuItem1.Visible = false;
            // 
            // tmnuNewLandmark
            // 
            this.tmnuNewLandmark.Enabled = false;
            this.tmnuNewLandmark.Name = "tmnuNewLandmark";
            this.tmnuNewLandmark.Size = new System.Drawing.Size(127, 22);
            this.tmnuNewLandmark.Text = "Landmark";
            this.tmnuNewLandmark.Visible = false;
            // 
            // tmnuNewNotecard
            // 
            this.tmnuNewNotecard.Image = global::METAbolt.Properties.Resources.documents_16;
            this.tmnuNewNotecard.Name = "tmnuNewNotecard";
            this.tmnuNewNotecard.Size = new System.Drawing.Size(127, 22);
            this.tmnuNewNotecard.Text = "Notecard";
            this.tmnuNewNotecard.Click += new System.EventHandler(this.tmnuNewNotecard_Click);
            // 
            // tmnuNewScript
            // 
            this.tmnuNewScript.Image = global::METAbolt.Properties.Resources.lsl_scripts_16;
            this.tmnuNewScript.Name = "tmnuNewScript";
            this.tmnuNewScript.Size = new System.Drawing.Size(127, 22);
            this.tmnuNewScript.Text = "Script";
            this.tmnuNewScript.Click += new System.EventHandler(this.tmnuNewScript_Click);
            // 
            // snapshotToolStripMenuItem
            // 
            this.snapshotToolStripMenuItem.Name = "snapshotToolStripMenuItem";
            this.snapshotToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.snapshotToolStripMenuItem.Text = "Snapshot";
            this.snapshotToolStripMenuItem.Visible = false;
            this.snapshotToolStripMenuItem.Click += new System.EventHandler(this.snapshotToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tbtnOrganize
            // 
            this.tbtnOrganize.AutoToolTip = false;
            this.tbtnOrganize.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmnuCut,
            this.tmnuCopy,
            this.tmnuPaste,
            this.toolStripMenuItem2,
            this.tmnuRename,
            this.tmnuDelete,
            this.toolStripSeparator2,
            this.expandInventoryToolStripMenuItem,
            this.unExpandInventoryToolStripMenuItem,
            this.refreshFolderToolStripMenuItem});
            this.tbtnOrganize.Enabled = false;
            this.tbtnOrganize.Image = global::METAbolt.Properties.Resources.System_16;
            this.tbtnOrganize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnOrganize.Name = "tbtnOrganize";
            this.tbtnOrganize.Size = new System.Drawing.Size(83, 22);
            this.tbtnOrganize.Text = "Organize";
            // 
            // tmnuCut
            // 
            this.tmnuCut.Image = global::METAbolt.Properties.Resources.cut_16;
            this.tmnuCut.Name = "tmnuCut";
            this.tmnuCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.tmnuCut.Size = new System.Drawing.Size(185, 22);
            this.tmnuCut.Text = "Cut";
            this.tmnuCut.Click += new System.EventHandler(this.tmnuCut_Click);
            // 
            // tmnuCopy
            // 
            this.tmnuCopy.Enabled = false;
            this.tmnuCopy.Image = global::METAbolt.Properties.Resources.copy_16;
            this.tmnuCopy.Name = "tmnuCopy";
            this.tmnuCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.tmnuCopy.Size = new System.Drawing.Size(185, 22);
            this.tmnuCopy.Text = "Copy";
            this.tmnuCopy.Visible = false;
            this.tmnuCopy.Click += new System.EventHandler(this.tmnuCopy_Click);
            // 
            // tmnuPaste
            // 
            this.tmnuPaste.Image = global::METAbolt.Properties.Resources.paste_16;
            this.tmnuPaste.Name = "tmnuPaste";
            this.tmnuPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.tmnuPaste.Size = new System.Drawing.Size(185, 22);
            this.tmnuPaste.Text = "Paste";
            this.tmnuPaste.Click += new System.EventHandler(this.tmnuPaste_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(182, 6);
            // 
            // tmnuRename
            // 
            this.tmnuRename.Name = "tmnuRename";
            this.tmnuRename.Size = new System.Drawing.Size(185, 22);
            this.tmnuRename.Text = "Rename";
            this.tmnuRename.Click += new System.EventHandler(this.tmnuRename_Click);
            // 
            // tmnuDelete
            // 
            this.tmnuDelete.Image = global::METAbolt.Properties.Resources.delete_16;
            this.tmnuDelete.Name = "tmnuDelete";
            this.tmnuDelete.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.tmnuDelete.Size = new System.Drawing.Size(185, 22);
            this.tmnuDelete.Text = "Delete";
            this.tmnuDelete.Click += new System.EventHandler(this.tmnuDelete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(182, 6);
            // 
            // expandInventoryToolStripMenuItem
            // 
            this.expandInventoryToolStripMenuItem.Name = "expandInventoryToolStripMenuItem";
            this.expandInventoryToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.expandInventoryToolStripMenuItem.Text = "Expand Inventory";
            this.expandInventoryToolStripMenuItem.Click += new System.EventHandler(this.expandInventoryToolStripMenuItem_Click_1);
            // 
            // unExpandInventoryToolStripMenuItem
            // 
            this.unExpandInventoryToolStripMenuItem.Name = "unExpandInventoryToolStripMenuItem";
            this.unExpandInventoryToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.unExpandInventoryToolStripMenuItem.Text = "Un expand Inventory";
            this.unExpandInventoryToolStripMenuItem.Click += new System.EventHandler(this.unExpandInventoryToolStripMenuItem_Click);
            // 
            // refreshFolderToolStripMenuItem
            // 
            this.refreshFolderToolStripMenuItem.Name = "refreshFolderToolStripMenuItem";
            this.refreshFolderToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.refreshFolderToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.refreshFolderToolStripMenuItem.Text = "Refresh Inventory";
            this.refreshFolderToolStripMenuItem.Click += new System.EventHandler(this.refreshFolderToolStripMenuItem_Click);
            // 
            // tbtnSort
            // 
            this.tbtnSort.AutoToolTip = false;
            this.tbtnSort.Enabled = false;
            this.tbtnSort.Image = global::METAbolt.Properties.Resources.copy_16;
            this.tbtnSort.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnSort.Name = "tbtnSort";
            this.tbtnSort.Size = new System.Drawing.Size(57, 22);
            this.tbtnSort.Text = "Sort";
            this.tbtnSort.Click += new System.EventHandler(this.tbtnSort_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::METAbolt.Properties.Resources.Url_History_16;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(72, 22);
            this.toolStripButton2.Text = "Changer";
            this.toolStripButton2.ToolTipText = "Auto clothes changer";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button5);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.listBox1);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.trackBar1);
            this.panel2.Controls.Add(this.button6);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.textBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(323, 428);
            this.panel2.TabIndex = 0;
            this.panel2.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 368);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 13);
            this.label5.TabIndex = 38;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.button2.Enabled = false;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(155, 231);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(89, 23);
            this.button2.TabIndex = 37;
            this.button2.Text = "Wear selected";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Red;
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(236, 401);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 36;
            this.button5.Text = "Done";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Red;
            this.button3.Enabled = false;
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(18, 231);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(112, 23);
            this.button3.TabIndex = 35;
            this.button3.Text = "Remove selected";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "Selected folders:";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(18, 65);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(226, 160);
            this.listBox1.TabIndex = 33;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.CornflowerBlue;
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(18, 401);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 31;
            this.button4.Text = "Start";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(197, 300);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 30;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 271);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(216, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Outfit change frequency (10 mins intervals)";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(12, 287);
            this.trackBar1.Maximum = 120;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(185, 45);
            this.trackBar1.SmallChange = 10;
            this.trackBar1.TabIndex = 28;
            this.trackBar1.TickFrequency = 10;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Green;
            this.button6.ForeColor = System.Drawing.Color.White;
            this.button6.Location = new System.Drawing.Point(250, 24);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(61, 23);
            this.button6.TabIndex = 27;
            this.button6.Text = "Add";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Name of Clothes folder:";
            // 
            // textBox2
            // 
            this.textBox2.ForeColor = System.Drawing.Color.DimGray;
            this.textBox2.Location = new System.Drawing.Point(18, 26);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(226, 21);
            this.textBox2.TabIndex = 25;
            this.textBox2.Text = "Select folder from inventory";
            this.textBox2.Enter += new System.EventHandler(this.textBox2_Enter);
            // 
            // timer1
            // 
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 327);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 13);
            this.label6.TabIndex = 39;
            // 
            // InventoryConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "InventoryConsole";
            this.Size = new System.Drawing.Size(632, 432);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tstInventory.ResumeLayout(false);
            this.tstInventory.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip tstInventory;
        private System.Windows.Forms.ToolStripDropDownButton tbtnNew;
        private System.Windows.Forms.ToolStripMenuItem tmnuNewFolder;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tmnuNewLandmark;
        private System.Windows.Forms.ToolStripMenuItem tmnuNewNotecard;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton tbtnOrganize;
        private System.Windows.Forms.ToolStripMenuItem tmnuCut;
        private System.Windows.Forms.ToolStripMenuItem tmnuCopy;
        private System.Windows.Forms.ToolStripMenuItem tmnuPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tmnuRename;
        private System.Windows.Forms.ToolStripMenuItem tmnuDelete;
        private System.Windows.Forms.ToolStripDropDownButton tbtnSort;
        private System.Windows.Forms.ToolStripMenuItem tmnuNewScript;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem unExpandInventoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshFolderToolStripMenuItem;
        public System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripMenuItem snapshotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandInventoryToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label6;
    }
}
