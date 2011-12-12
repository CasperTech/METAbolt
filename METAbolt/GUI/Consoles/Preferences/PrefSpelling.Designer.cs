namespace METAbolt
{
    partial class PrefSpelling
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrefSpelling));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ilFlags = new System.Windows.Forms.ImageList(this.components);
            this.picFlag = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFlag)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(59, 27);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(219, 147);
            this.listBox1.Sorted = true;
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(82, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Available language dictionaries:";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(156, 180);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Set Selected";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(41, 23);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(116, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Enable spell check";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.picFlag);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(38, 46);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(339, 236);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 211);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Current language: ";
            // 
            // ilFlags
            // 
            this.ilFlags.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilFlags.ImageStream")));
            this.ilFlags.TransparentColor = System.Drawing.Color.Transparent;
            this.ilFlags.Images.SetKeyName(0, "ad.png");
            this.ilFlags.Images.SetKeyName(1, "ae.png");
            this.ilFlags.Images.SetKeyName(2, "af.png");
            this.ilFlags.Images.SetKeyName(3, "ag.png");
            this.ilFlags.Images.SetKeyName(4, "ai.png");
            this.ilFlags.Images.SetKeyName(5, "al.png");
            this.ilFlags.Images.SetKeyName(6, "am.png");
            this.ilFlags.Images.SetKeyName(7, "an.png");
            this.ilFlags.Images.SetKeyName(8, "ao.png");
            this.ilFlags.Images.SetKeyName(9, "ar.png");
            this.ilFlags.Images.SetKeyName(10, "as.png");
            this.ilFlags.Images.SetKeyName(11, "at.png");
            this.ilFlags.Images.SetKeyName(12, "au.png");
            this.ilFlags.Images.SetKeyName(13, "aw.png");
            this.ilFlags.Images.SetKeyName(14, "ax.png");
            this.ilFlags.Images.SetKeyName(15, "az.png");
            this.ilFlags.Images.SetKeyName(16, "ba.png");
            this.ilFlags.Images.SetKeyName(17, "bb.png");
            this.ilFlags.Images.SetKeyName(18, "bd.png");
            this.ilFlags.Images.SetKeyName(19, "be.png");
            this.ilFlags.Images.SetKeyName(20, "bf.png");
            this.ilFlags.Images.SetKeyName(21, "bg.png");
            this.ilFlags.Images.SetKeyName(22, "bh.png");
            this.ilFlags.Images.SetKeyName(23, "bi.png");
            this.ilFlags.Images.SetKeyName(24, "bj.png");
            this.ilFlags.Images.SetKeyName(25, "bm.png");
            this.ilFlags.Images.SetKeyName(26, "bn.png");
            this.ilFlags.Images.SetKeyName(27, "bo.png");
            this.ilFlags.Images.SetKeyName(28, "br.png");
            this.ilFlags.Images.SetKeyName(29, "bs.png");
            this.ilFlags.Images.SetKeyName(30, "bt.png");
            this.ilFlags.Images.SetKeyName(31, "bv.png");
            this.ilFlags.Images.SetKeyName(32, "bw.png");
            this.ilFlags.Images.SetKeyName(33, "by.png");
            this.ilFlags.Images.SetKeyName(34, "bz.png");
            this.ilFlags.Images.SetKeyName(35, "ca.png");
            this.ilFlags.Images.SetKeyName(36, "catalonia.png");
            this.ilFlags.Images.SetKeyName(37, "cc.png");
            this.ilFlags.Images.SetKeyName(38, "cd.png");
            this.ilFlags.Images.SetKeyName(39, "cf.png");
            this.ilFlags.Images.SetKeyName(40, "cg.png");
            this.ilFlags.Images.SetKeyName(41, "ch.png");
            this.ilFlags.Images.SetKeyName(42, "ci.png");
            this.ilFlags.Images.SetKeyName(43, "ck.png");
            this.ilFlags.Images.SetKeyName(44, "cl.png");
            this.ilFlags.Images.SetKeyName(45, "cm.png");
            this.ilFlags.Images.SetKeyName(46, "cn.png");
            this.ilFlags.Images.SetKeyName(47, "co.png");
            this.ilFlags.Images.SetKeyName(48, "cr.png");
            this.ilFlags.Images.SetKeyName(49, "cs.png");
            this.ilFlags.Images.SetKeyName(50, "cu.png");
            this.ilFlags.Images.SetKeyName(51, "cv.png");
            this.ilFlags.Images.SetKeyName(52, "cx.png");
            this.ilFlags.Images.SetKeyName(53, "cy.png");
            this.ilFlags.Images.SetKeyName(54, "cz.png");
            this.ilFlags.Images.SetKeyName(55, "de.png");
            this.ilFlags.Images.SetKeyName(56, "dj.png");
            this.ilFlags.Images.SetKeyName(57, "dk.png");
            this.ilFlags.Images.SetKeyName(58, "dm.png");
            this.ilFlags.Images.SetKeyName(59, "do.png");
            this.ilFlags.Images.SetKeyName(60, "dz.png");
            this.ilFlags.Images.SetKeyName(61, "ec.png");
            this.ilFlags.Images.SetKeyName(62, "ee.png");
            this.ilFlags.Images.SetKeyName(63, "eg.png");
            this.ilFlags.Images.SetKeyName(64, "eh.png");
            this.ilFlags.Images.SetKeyName(65, "england.png");
            this.ilFlags.Images.SetKeyName(66, "er.png");
            this.ilFlags.Images.SetKeyName(67, "es.png");
            this.ilFlags.Images.SetKeyName(68, "et.png");
            this.ilFlags.Images.SetKeyName(69, "europeanunion.png");
            this.ilFlags.Images.SetKeyName(70, "fam.png");
            this.ilFlags.Images.SetKeyName(71, "fi.png");
            this.ilFlags.Images.SetKeyName(72, "fj.png");
            this.ilFlags.Images.SetKeyName(73, "fk.png");
            this.ilFlags.Images.SetKeyName(74, "fm.png");
            this.ilFlags.Images.SetKeyName(75, "fo.png");
            this.ilFlags.Images.SetKeyName(76, "fr.png");
            this.ilFlags.Images.SetKeyName(77, "ga.png");
            this.ilFlags.Images.SetKeyName(78, "gb.png");
            this.ilFlags.Images.SetKeyName(79, "gd.png");
            this.ilFlags.Images.SetKeyName(80, "ge.png");
            this.ilFlags.Images.SetKeyName(81, "gf.png");
            this.ilFlags.Images.SetKeyName(82, "gh.png");
            this.ilFlags.Images.SetKeyName(83, "gi.png");
            this.ilFlags.Images.SetKeyName(84, "gl.png");
            this.ilFlags.Images.SetKeyName(85, "gm.png");
            this.ilFlags.Images.SetKeyName(86, "gn.png");
            this.ilFlags.Images.SetKeyName(87, "gp.png");
            this.ilFlags.Images.SetKeyName(88, "gq.png");
            this.ilFlags.Images.SetKeyName(89, "gr.png");
            this.ilFlags.Images.SetKeyName(90, "gs.png");
            this.ilFlags.Images.SetKeyName(91, "gt.png");
            this.ilFlags.Images.SetKeyName(92, "gu.png");
            this.ilFlags.Images.SetKeyName(93, "gw.png");
            this.ilFlags.Images.SetKeyName(94, "gy.png");
            this.ilFlags.Images.SetKeyName(95, "hk.png");
            this.ilFlags.Images.SetKeyName(96, "hm.png");
            this.ilFlags.Images.SetKeyName(97, "hn.png");
            this.ilFlags.Images.SetKeyName(98, "hr.png");
            this.ilFlags.Images.SetKeyName(99, "ht.png");
            this.ilFlags.Images.SetKeyName(100, "hu.png");
            this.ilFlags.Images.SetKeyName(101, "id.png");
            this.ilFlags.Images.SetKeyName(102, "ie.png");
            this.ilFlags.Images.SetKeyName(103, "il.png");
            this.ilFlags.Images.SetKeyName(104, "in.png");
            this.ilFlags.Images.SetKeyName(105, "io.png");
            this.ilFlags.Images.SetKeyName(106, "iq.png");
            this.ilFlags.Images.SetKeyName(107, "ir.png");
            this.ilFlags.Images.SetKeyName(108, "is.png");
            this.ilFlags.Images.SetKeyName(109, "it.png");
            this.ilFlags.Images.SetKeyName(110, "jm.png");
            this.ilFlags.Images.SetKeyName(111, "jo.png");
            this.ilFlags.Images.SetKeyName(112, "jp.png");
            this.ilFlags.Images.SetKeyName(113, "ke.png");
            this.ilFlags.Images.SetKeyName(114, "kg.png");
            this.ilFlags.Images.SetKeyName(115, "kh.png");
            this.ilFlags.Images.SetKeyName(116, "ki.png");
            this.ilFlags.Images.SetKeyName(117, "km.png");
            this.ilFlags.Images.SetKeyName(118, "kn.png");
            this.ilFlags.Images.SetKeyName(119, "kp.png");
            this.ilFlags.Images.SetKeyName(120, "kr.png");
            this.ilFlags.Images.SetKeyName(121, "kw.png");
            this.ilFlags.Images.SetKeyName(122, "ky.png");
            this.ilFlags.Images.SetKeyName(123, "kz.png");
            this.ilFlags.Images.SetKeyName(124, "la.png");
            this.ilFlags.Images.SetKeyName(125, "lb.png");
            this.ilFlags.Images.SetKeyName(126, "lc.png");
            this.ilFlags.Images.SetKeyName(127, "li.png");
            this.ilFlags.Images.SetKeyName(128, "lk.png");
            this.ilFlags.Images.SetKeyName(129, "lr.png");
            this.ilFlags.Images.SetKeyName(130, "ls.png");
            this.ilFlags.Images.SetKeyName(131, "lt.png");
            this.ilFlags.Images.SetKeyName(132, "lu.png");
            this.ilFlags.Images.SetKeyName(133, "lv.png");
            this.ilFlags.Images.SetKeyName(134, "ly.png");
            this.ilFlags.Images.SetKeyName(135, "ma.png");
            this.ilFlags.Images.SetKeyName(136, "mc.png");
            this.ilFlags.Images.SetKeyName(137, "md.png");
            this.ilFlags.Images.SetKeyName(138, "me.png");
            this.ilFlags.Images.SetKeyName(139, "mg.png");
            this.ilFlags.Images.SetKeyName(140, "mh.png");
            this.ilFlags.Images.SetKeyName(141, "mk.png");
            this.ilFlags.Images.SetKeyName(142, "ml.png");
            this.ilFlags.Images.SetKeyName(143, "mm.png");
            this.ilFlags.Images.SetKeyName(144, "mn.png");
            this.ilFlags.Images.SetKeyName(145, "mo.png");
            this.ilFlags.Images.SetKeyName(146, "mp.png");
            this.ilFlags.Images.SetKeyName(147, "mq.png");
            this.ilFlags.Images.SetKeyName(148, "mr.png");
            this.ilFlags.Images.SetKeyName(149, "ms.png");
            this.ilFlags.Images.SetKeyName(150, "mt.png");
            this.ilFlags.Images.SetKeyName(151, "mu.png");
            this.ilFlags.Images.SetKeyName(152, "mv.png");
            this.ilFlags.Images.SetKeyName(153, "mw.png");
            this.ilFlags.Images.SetKeyName(154, "mx.png");
            this.ilFlags.Images.SetKeyName(155, "my.png");
            this.ilFlags.Images.SetKeyName(156, "mz.png");
            this.ilFlags.Images.SetKeyName(157, "na.png");
            this.ilFlags.Images.SetKeyName(158, "nc.png");
            this.ilFlags.Images.SetKeyName(159, "ne.png");
            this.ilFlags.Images.SetKeyName(160, "nf.png");
            this.ilFlags.Images.SetKeyName(161, "ng.png");
            this.ilFlags.Images.SetKeyName(162, "ni.png");
            this.ilFlags.Images.SetKeyName(163, "nl.png");
            this.ilFlags.Images.SetKeyName(164, "no.png");
            this.ilFlags.Images.SetKeyName(165, "np.png");
            this.ilFlags.Images.SetKeyName(166, "nr.png");
            this.ilFlags.Images.SetKeyName(167, "nu.png");
            this.ilFlags.Images.SetKeyName(168, "nz.png");
            this.ilFlags.Images.SetKeyName(169, "om.png");
            this.ilFlags.Images.SetKeyName(170, "pa.png");
            this.ilFlags.Images.SetKeyName(171, "pe.png");
            this.ilFlags.Images.SetKeyName(172, "pf.png");
            this.ilFlags.Images.SetKeyName(173, "pg.png");
            this.ilFlags.Images.SetKeyName(174, "ph.png");
            this.ilFlags.Images.SetKeyName(175, "pk.png");
            this.ilFlags.Images.SetKeyName(176, "pl.png");
            this.ilFlags.Images.SetKeyName(177, "pm.png");
            this.ilFlags.Images.SetKeyName(178, "pn.png");
            this.ilFlags.Images.SetKeyName(179, "pr.png");
            this.ilFlags.Images.SetKeyName(180, "ps.png");
            this.ilFlags.Images.SetKeyName(181, "pt.png");
            this.ilFlags.Images.SetKeyName(182, "pw.png");
            this.ilFlags.Images.SetKeyName(183, "py.png");
            this.ilFlags.Images.SetKeyName(184, "qa.png");
            this.ilFlags.Images.SetKeyName(185, "re.png");
            this.ilFlags.Images.SetKeyName(186, "ro.png");
            this.ilFlags.Images.SetKeyName(187, "rs.png");
            this.ilFlags.Images.SetKeyName(188, "ru.png");
            this.ilFlags.Images.SetKeyName(189, "rw.png");
            this.ilFlags.Images.SetKeyName(190, "sa.png");
            this.ilFlags.Images.SetKeyName(191, "sb.png");
            this.ilFlags.Images.SetKeyName(192, "sc.png");
            this.ilFlags.Images.SetKeyName(193, "scotland.png");
            this.ilFlags.Images.SetKeyName(194, "sd.png");
            this.ilFlags.Images.SetKeyName(195, "se.png");
            this.ilFlags.Images.SetKeyName(196, "sg.png");
            this.ilFlags.Images.SetKeyName(197, "sh.png");
            this.ilFlags.Images.SetKeyName(198, "si.png");
            this.ilFlags.Images.SetKeyName(199, "sj.png");
            this.ilFlags.Images.SetKeyName(200, "sk.png");
            this.ilFlags.Images.SetKeyName(201, "sl.png");
            this.ilFlags.Images.SetKeyName(202, "sm.png");
            this.ilFlags.Images.SetKeyName(203, "sn.png");
            this.ilFlags.Images.SetKeyName(204, "so.png");
            this.ilFlags.Images.SetKeyName(205, "sr.png");
            this.ilFlags.Images.SetKeyName(206, "st.png");
            this.ilFlags.Images.SetKeyName(207, "sv.png");
            this.ilFlags.Images.SetKeyName(208, "sy.png");
            this.ilFlags.Images.SetKeyName(209, "sz.png");
            this.ilFlags.Images.SetKeyName(210, "tc.png");
            this.ilFlags.Images.SetKeyName(211, "td.png");
            this.ilFlags.Images.SetKeyName(212, "tf.png");
            this.ilFlags.Images.SetKeyName(213, "tg.png");
            this.ilFlags.Images.SetKeyName(214, "th.png");
            this.ilFlags.Images.SetKeyName(215, "tj.png");
            this.ilFlags.Images.SetKeyName(216, "tk.png");
            this.ilFlags.Images.SetKeyName(217, "tl.png");
            this.ilFlags.Images.SetKeyName(218, "tm.png");
            this.ilFlags.Images.SetKeyName(219, "tn.png");
            this.ilFlags.Images.SetKeyName(220, "to.png");
            this.ilFlags.Images.SetKeyName(221, "tr.png");
            this.ilFlags.Images.SetKeyName(222, "tt.png");
            this.ilFlags.Images.SetKeyName(223, "tv.png");
            this.ilFlags.Images.SetKeyName(224, "tw.png");
            this.ilFlags.Images.SetKeyName(225, "tz.png");
            this.ilFlags.Images.SetKeyName(226, "ua.png");
            this.ilFlags.Images.SetKeyName(227, "ug.png");
            this.ilFlags.Images.SetKeyName(228, "um.png");
            this.ilFlags.Images.SetKeyName(229, "us.png");
            this.ilFlags.Images.SetKeyName(230, "uy.png");
            this.ilFlags.Images.SetKeyName(231, "uz.png");
            this.ilFlags.Images.SetKeyName(232, "va.png");
            this.ilFlags.Images.SetKeyName(233, "vc.png");
            this.ilFlags.Images.SetKeyName(234, "ve.png");
            this.ilFlags.Images.SetKeyName(235, "vg.png");
            this.ilFlags.Images.SetKeyName(236, "vi.png");
            this.ilFlags.Images.SetKeyName(237, "vn.png");
            this.ilFlags.Images.SetKeyName(238, "vu.png");
            this.ilFlags.Images.SetKeyName(239, "wales.png");
            this.ilFlags.Images.SetKeyName(240, "wf.png");
            this.ilFlags.Images.SetKeyName(241, "ws.png");
            this.ilFlags.Images.SetKeyName(242, "ye.png");
            this.ilFlags.Images.SetKeyName(243, "yt.png");
            this.ilFlags.Images.SetKeyName(244, "za.png");
            this.ilFlags.Images.SetKeyName(245, "zm.png");
            this.ilFlags.Images.SetKeyName(246, "zw.png");
            // 
            // picFlag
            // 
            this.picFlag.Location = new System.Drawing.Point(21, 27);
            this.picFlag.Name = "picFlag";
            this.picFlag.Size = new System.Drawing.Size(16, 11);
            this.picFlag.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picFlag.TabIndex = 4;
            this.picFlag.TabStop = false;
            // 
            // PrefSpelling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label1);
            this.Name = "PrefSpelling";
            this.Size = new System.Drawing.Size(380, 308);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFlag)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox picFlag;
        private System.Windows.Forms.ImageList ilFlags;
    }
}
