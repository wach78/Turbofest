namespace projectX
{
    partial class FrmEvents
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
            this.cmbEvents = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chbVeto = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbPrio = new System.Windows.Forms.ComboBox();
            this.Month = new System.Windows.Forms.Label();
            this.Mjan = new System.Windows.Forms.Label();
            this.Mfeb = new System.Windows.Forms.Label();
            this.Mmar = new System.Windows.Forms.Label();
            this.Mapr = new System.Windows.Forms.Label();
            this.Mmaj = new System.Windows.Forms.Label();
            this.Mjun = new System.Windows.Forms.Label();
            this.Mjul = new System.Windows.Forms.Label();
            this.Maug = new System.Windows.Forms.Label();
            this.Msep = new System.Windows.Forms.Label();
            this.Moct = new System.Windows.Forms.Label();
            this.Mnov = new System.Windows.Forms.Label();
            this.Mdec = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.checkjan = new System.Windows.Forms.CheckBox();
            this.checkfeb = new System.Windows.Forms.CheckBox();
            this.checkmar = new System.Windows.Forms.CheckBox();
            this.checkaug = new System.Windows.Forms.CheckBox();
            this.checkmaj = new System.Windows.Forms.CheckBox();
            this.checkjun = new System.Windows.Forms.CheckBox();
            this.checkjul = new System.Windows.Forms.CheckBox();
            this.checkapr = new System.Windows.Forms.CheckBox();
            this.checksep = new System.Windows.Forms.CheckBox();
            this.checkoct = new System.Windows.Forms.CheckBox();
            this.checknov = new System.Windows.Forms.CheckBox();
            this.checkdec = new System.Windows.Forms.CheckBox();
            this.cmbjan = new System.Windows.Forms.ComboBox();
            this.cmbfeb = new System.Windows.Forms.ComboBox();
            this.cmbmar = new System.Windows.Forms.ComboBox();
            this.cmbapr = new System.Windows.Forms.ComboBox();
            this.cmbmaj = new System.Windows.Forms.ComboBox();
            this.cmbjun = new System.Windows.Forms.ComboBox();
            this.cmbjul = new System.Windows.Forms.ComboBox();
            this.cmbaug = new System.Windows.Forms.ComboBox();
            this.cmbsep = new System.Windows.Forms.ComboBox();
            this.cmboct = new System.Windows.Forms.ComboBox();
            this.cmbnov = new System.Windows.Forms.ComboBox();
            this.cmbdec = new System.Windows.Forms.ComboBox();
            this.btnsave = new System.Windows.Forms.Button();
            this.btncancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbEvents
            // 
            this.cmbEvents.FormattingEnabled = true;
            this.cmbEvents.Location = new System.Drawing.Point(100, 21);
            this.cmbEvents.Name = "cmbEvents";
            this.cmbEvents.Size = new System.Drawing.Size(121, 21);
            this.cmbEvents.TabIndex = 0;
            this.cmbEvents.DropDown += new System.EventHandler(this.cmbEvents_DropDown);
            this.cmbEvents.SelectedValueChanged += new System.EventHandler(this.cmbEvents_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Event";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Veto";
            // 
            // chbVeto
            // 
            this.chbVeto.AutoSize = true;
            this.chbVeto.Location = new System.Drawing.Point(100, 60);
            this.chbVeto.Name = "chbVeto";
            this.chbVeto.Size = new System.Drawing.Size(15, 14);
            this.chbVeto.TabIndex = 3;
            this.chbVeto.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "prio";
            // 
            // cmbPrio
            // 
            this.cmbPrio.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.cmbPrio.FormattingEnabled = true;
            this.cmbPrio.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbPrio.Location = new System.Drawing.Point(100, 81);
            this.cmbPrio.Name = "cmbPrio";
            this.cmbPrio.Size = new System.Drawing.Size(36, 21);
            this.cmbPrio.TabIndex = 5;
            this.cmbPrio.Text = "1";
            // 
            // Month
            // 
            this.Month.AutoSize = true;
            this.Month.Location = new System.Drawing.Point(13, 114);
            this.Month.Name = "Month";
            this.Month.Size = new System.Drawing.Size(37, 13);
            this.Month.TabIndex = 6;
            this.Month.Text = "Month";
            // 
            // Mjan
            // 
            this.Mjan.AutoSize = true;
            this.Mjan.Location = new System.Drawing.Point(13, 142);
            this.Mjan.Name = "Mjan";
            this.Mjan.Size = new System.Drawing.Size(24, 13);
            this.Mjan.TabIndex = 7;
            this.Mjan.Text = "Jan";
            // 
            // Mfeb
            // 
            this.Mfeb.AutoSize = true;
            this.Mfeb.Location = new System.Drawing.Point(12, 169);
            this.Mfeb.Name = "Mfeb";
            this.Mfeb.Size = new System.Drawing.Size(25, 13);
            this.Mfeb.TabIndex = 8;
            this.Mfeb.Text = "Feb";
            this.Mfeb.Click += new System.EventHandler(this.Mfeb_Click);
            // 
            // Mmar
            // 
            this.Mmar.AutoSize = true;
            this.Mmar.Location = new System.Drawing.Point(12, 196);
            this.Mmar.Name = "Mmar";
            this.Mmar.Size = new System.Drawing.Size(25, 13);
            this.Mmar.TabIndex = 9;
            this.Mmar.Text = "Mar";
            // 
            // Mapr
            // 
            this.Mapr.AutoSize = true;
            this.Mapr.Location = new System.Drawing.Point(12, 223);
            this.Mapr.Name = "Mapr";
            this.Mapr.Size = new System.Drawing.Size(23, 13);
            this.Mapr.TabIndex = 10;
            this.Mapr.Text = "Apr";
            // 
            // Mmaj
            // 
            this.Mmaj.AutoSize = true;
            this.Mmaj.Location = new System.Drawing.Point(12, 250);
            this.Mmaj.Name = "Mmaj";
            this.Mmaj.Size = new System.Drawing.Size(24, 13);
            this.Mmaj.TabIndex = 11;
            this.Mmaj.Text = "Maj";
            // 
            // Mjun
            // 
            this.Mjun.AutoSize = true;
            this.Mjun.Location = new System.Drawing.Point(13, 278);
            this.Mjun.Name = "Mjun";
            this.Mjun.Size = new System.Drawing.Size(24, 13);
            this.Mjun.TabIndex = 12;
            this.Mjun.Text = "Jun";
            // 
            // Mjul
            // 
            this.Mjul.AutoSize = true;
            this.Mjul.Location = new System.Drawing.Point(12, 307);
            this.Mjul.Name = "Mjul";
            this.Mjul.Size = new System.Drawing.Size(20, 13);
            this.Mjul.TabIndex = 13;
            this.Mjul.Text = "Jul";
            // 
            // Maug
            // 
            this.Maug.AutoSize = true;
            this.Maug.Location = new System.Drawing.Point(12, 333);
            this.Maug.Name = "Maug";
            this.Maug.Size = new System.Drawing.Size(26, 13);
            this.Maug.TabIndex = 14;
            this.Maug.Text = "Aug";
            // 
            // Msep
            // 
            this.Msep.AutoSize = true;
            this.Msep.Location = new System.Drawing.Point(13, 360);
            this.Msep.Name = "Msep";
            this.Msep.Size = new System.Drawing.Size(26, 13);
            this.Msep.TabIndex = 15;
            this.Msep.Text = "Sep";
            // 
            // Moct
            // 
            this.Moct.AutoSize = true;
            this.Moct.Location = new System.Drawing.Point(12, 380);
            this.Moct.Name = "Moct";
            this.Moct.Size = new System.Drawing.Size(24, 13);
            this.Moct.TabIndex = 16;
            this.Moct.Text = "Oct";
            // 
            // Mnov
            // 
            this.Mnov.AutoSize = true;
            this.Mnov.Location = new System.Drawing.Point(11, 407);
            this.Mnov.Name = "Mnov";
            this.Mnov.Size = new System.Drawing.Size(27, 13);
            this.Mnov.TabIndex = 17;
            this.Mnov.Text = "Nov";
            // 
            // Mdec
            // 
            this.Mdec.AutoSize = true;
            this.Mdec.Location = new System.Drawing.Point(12, 435);
            this.Mdec.Name = "Mdec";
            this.Mdec.Size = new System.Drawing.Size(27, 13);
            this.Mdec.TabIndex = 18;
            this.Mdec.Text = "Dec";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(168, 114);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(32, 13);
            this.label17.TabIndex = 19;
            this.label17.Text = "Runs";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(97, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "this";
            // 
            // checkjan
            // 
            this.checkjan.AutoSize = true;
            this.checkjan.Location = new System.Drawing.Point(100, 142);
            this.checkjan.Name = "checkjan";
            this.checkjan.Size = new System.Drawing.Size(15, 14);
            this.checkjan.TabIndex = 21;
            this.checkjan.UseVisualStyleBackColor = true;
            // 
            // checkfeb
            // 
            this.checkfeb.AutoSize = true;
            this.checkfeb.Location = new System.Drawing.Point(100, 169);
            this.checkfeb.Name = "checkfeb";
            this.checkfeb.Size = new System.Drawing.Size(15, 14);
            this.checkfeb.TabIndex = 22;
            this.checkfeb.UseVisualStyleBackColor = true;
            // 
            // checkmar
            // 
            this.checkmar.AutoSize = true;
            this.checkmar.Location = new System.Drawing.Point(100, 196);
            this.checkmar.Name = "checkmar";
            this.checkmar.Size = new System.Drawing.Size(15, 14);
            this.checkmar.TabIndex = 23;
            this.checkmar.UseVisualStyleBackColor = true;
            // 
            // checkaug
            // 
            this.checkaug.AutoSize = true;
            this.checkaug.Location = new System.Drawing.Point(100, 333);
            this.checkaug.Name = "checkaug";
            this.checkaug.Size = new System.Drawing.Size(15, 14);
            this.checkaug.TabIndex = 24;
            this.checkaug.UseVisualStyleBackColor = true;
            // 
            // checkmaj
            // 
            this.checkmaj.AutoSize = true;
            this.checkmaj.Location = new System.Drawing.Point(100, 250);
            this.checkmaj.Name = "checkmaj";
            this.checkmaj.Size = new System.Drawing.Size(15, 14);
            this.checkmaj.TabIndex = 25;
            this.checkmaj.UseVisualStyleBackColor = true;
            // 
            // checkjun
            // 
            this.checkjun.AutoSize = true;
            this.checkjun.Location = new System.Drawing.Point(100, 278);
            this.checkjun.Name = "checkjun";
            this.checkjun.Size = new System.Drawing.Size(15, 14);
            this.checkjun.TabIndex = 26;
            this.checkjun.UseVisualStyleBackColor = true;
            // 
            // checkjul
            // 
            this.checkjul.AutoSize = true;
            this.checkjul.Location = new System.Drawing.Point(100, 307);
            this.checkjul.Name = "checkjul";
            this.checkjul.Size = new System.Drawing.Size(15, 14);
            this.checkjul.TabIndex = 27;
            this.checkjul.UseVisualStyleBackColor = true;
            // 
            // checkapr
            // 
            this.checkapr.AutoSize = true;
            this.checkapr.Location = new System.Drawing.Point(100, 223);
            this.checkapr.Name = "checkapr";
            this.checkapr.Size = new System.Drawing.Size(15, 14);
            this.checkapr.TabIndex = 28;
            this.checkapr.UseVisualStyleBackColor = true;
            // 
            // checksep
            // 
            this.checksep.AutoSize = true;
            this.checksep.Location = new System.Drawing.Point(100, 360);
            this.checksep.Name = "checksep";
            this.checksep.Size = new System.Drawing.Size(15, 14);
            this.checksep.TabIndex = 29;
            this.checksep.UseVisualStyleBackColor = true;
            // 
            // checkoct
            // 
            this.checkoct.AutoSize = true;
            this.checkoct.Location = new System.Drawing.Point(100, 380);
            this.checkoct.Name = "checkoct";
            this.checkoct.Size = new System.Drawing.Size(15, 14);
            this.checkoct.TabIndex = 30;
            this.checkoct.UseVisualStyleBackColor = true;
            // 
            // checknov
            // 
            this.checknov.AutoSize = true;
            this.checknov.Location = new System.Drawing.Point(100, 407);
            this.checknov.Name = "checknov";
            this.checknov.Size = new System.Drawing.Size(15, 14);
            this.checknov.TabIndex = 31;
            this.checknov.UseVisualStyleBackColor = true;
            // 
            // checkdec
            // 
            this.checkdec.AutoSize = true;
            this.checkdec.Location = new System.Drawing.Point(100, 434);
            this.checkdec.Name = "checkdec";
            this.checkdec.Size = new System.Drawing.Size(15, 14);
            this.checkdec.TabIndex = 32;
            this.checkdec.UseVisualStyleBackColor = true;
            // 
            // cmbjan
            // 
            this.cmbjan.FormattingEnabled = true;
            this.cmbjan.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbjan.Location = new System.Drawing.Point(164, 135);
            this.cmbjan.Name = "cmbjan";
            this.cmbjan.Size = new System.Drawing.Size(36, 21);
            this.cmbjan.TabIndex = 33;
            this.cmbjan.Text = "1";
            // 
            // cmbfeb
            // 
            this.cmbfeb.FormattingEnabled = true;
            this.cmbfeb.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbfeb.Location = new System.Drawing.Point(164, 162);
            this.cmbfeb.Name = "cmbfeb";
            this.cmbfeb.Size = new System.Drawing.Size(36, 21);
            this.cmbfeb.TabIndex = 34;
            this.cmbfeb.Text = "1";
            // 
            // cmbmar
            // 
            this.cmbmar.FormattingEnabled = true;
            this.cmbmar.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbmar.Location = new System.Drawing.Point(164, 189);
            this.cmbmar.Name = "cmbmar";
            this.cmbmar.Size = new System.Drawing.Size(36, 21);
            this.cmbmar.TabIndex = 35;
            this.cmbmar.Text = "1";
            // 
            // cmbapr
            // 
            this.cmbapr.FormattingEnabled = true;
            this.cmbapr.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbapr.Location = new System.Drawing.Point(164, 216);
            this.cmbapr.Name = "cmbapr";
            this.cmbapr.Size = new System.Drawing.Size(36, 21);
            this.cmbapr.TabIndex = 36;
            this.cmbapr.Text = "1";
            // 
            // cmbmaj
            // 
            this.cmbmaj.FormattingEnabled = true;
            this.cmbmaj.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbmaj.Location = new System.Drawing.Point(164, 243);
            this.cmbmaj.Name = "cmbmaj";
            this.cmbmaj.Size = new System.Drawing.Size(36, 21);
            this.cmbmaj.TabIndex = 37;
            this.cmbmaj.Text = "1";
            // 
            // cmbjun
            // 
            this.cmbjun.FormattingEnabled = true;
            this.cmbjun.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbjun.Location = new System.Drawing.Point(164, 271);
            this.cmbjun.Name = "cmbjun";
            this.cmbjun.Size = new System.Drawing.Size(36, 21);
            this.cmbjun.TabIndex = 38;
            this.cmbjun.Text = "1";
            // 
            // cmbjul
            // 
            this.cmbjul.FormattingEnabled = true;
            this.cmbjul.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbjul.Location = new System.Drawing.Point(164, 300);
            this.cmbjul.Name = "cmbjul";
            this.cmbjul.Size = new System.Drawing.Size(36, 21);
            this.cmbjul.TabIndex = 39;
            this.cmbjul.Text = "1";
            // 
            // cmbaug
            // 
            this.cmbaug.FormattingEnabled = true;
            this.cmbaug.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbaug.Location = new System.Drawing.Point(164, 326);
            this.cmbaug.Name = "cmbaug";
            this.cmbaug.Size = new System.Drawing.Size(36, 21);
            this.cmbaug.TabIndex = 40;
            this.cmbaug.Text = "1";
            // 
            // cmbsep
            // 
            this.cmbsep.FormattingEnabled = true;
            this.cmbsep.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbsep.Location = new System.Drawing.Point(164, 353);
            this.cmbsep.Name = "cmbsep";
            this.cmbsep.Size = new System.Drawing.Size(36, 21);
            this.cmbsep.TabIndex = 41;
            this.cmbsep.Text = "1";
            // 
            // cmboct
            // 
            this.cmboct.FormattingEnabled = true;
            this.cmboct.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmboct.Location = new System.Drawing.Point(164, 380);
            this.cmboct.Name = "cmboct";
            this.cmboct.Size = new System.Drawing.Size(36, 21);
            this.cmboct.TabIndex = 42;
            this.cmboct.Text = "1";
            // 
            // cmbnov
            // 
            this.cmbnov.FormattingEnabled = true;
            this.cmbnov.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbnov.Location = new System.Drawing.Point(164, 407);
            this.cmbnov.Name = "cmbnov";
            this.cmbnov.Size = new System.Drawing.Size(36, 21);
            this.cmbnov.TabIndex = 43;
            this.cmbnov.Text = "1";
            // 
            // cmbdec
            // 
            this.cmbdec.FormattingEnabled = true;
            this.cmbdec.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbdec.Location = new System.Drawing.Point(164, 434);
            this.cmbdec.Name = "cmbdec";
            this.cmbdec.Size = new System.Drawing.Size(36, 21);
            this.cmbdec.TabIndex = 44;
            this.cmbdec.Text = "1";
            // 
            // btnsave
            // 
            this.btnsave.Location = new System.Drawing.Point(16, 472);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(75, 23);
            this.btnsave.TabIndex = 45;
            this.btnsave.Text = "Save";
            this.btnsave.UseVisualStyleBackColor = true;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // btncancel
            // 
            this.btncancel.Location = new System.Drawing.Point(146, 472);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(75, 23);
            this.btncancel.TabIndex = 46;
            this.btncancel.Text = "Cancel";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // FrmEvents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 507);
            this.Controls.Add(this.btncancel);
            this.Controls.Add(this.btnsave);
            this.Controls.Add(this.cmbdec);
            this.Controls.Add(this.cmbnov);
            this.Controls.Add(this.cmboct);
            this.Controls.Add(this.cmbsep);
            this.Controls.Add(this.cmbaug);
            this.Controls.Add(this.cmbjul);
            this.Controls.Add(this.cmbjun);
            this.Controls.Add(this.cmbmaj);
            this.Controls.Add(this.cmbapr);
            this.Controls.Add(this.cmbmar);
            this.Controls.Add(this.cmbfeb);
            this.Controls.Add(this.cmbjan);
            this.Controls.Add(this.checkdec);
            this.Controls.Add(this.checknov);
            this.Controls.Add(this.checkoct);
            this.Controls.Add(this.checksep);
            this.Controls.Add(this.checkapr);
            this.Controls.Add(this.checkjul);
            this.Controls.Add(this.checkjun);
            this.Controls.Add(this.checkmaj);
            this.Controls.Add(this.checkaug);
            this.Controls.Add(this.checkmar);
            this.Controls.Add(this.checkfeb);
            this.Controls.Add(this.checkjan);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.Mdec);
            this.Controls.Add(this.Mnov);
            this.Controls.Add(this.Moct);
            this.Controls.Add(this.Msep);
            this.Controls.Add(this.Maug);
            this.Controls.Add(this.Mjul);
            this.Controls.Add(this.Mjun);
            this.Controls.Add(this.Mmaj);
            this.Controls.Add(this.Mapr);
            this.Controls.Add(this.Mmar);
            this.Controls.Add(this.Mfeb);
            this.Controls.Add(this.Mjan);
            this.Controls.Add(this.Month);
            this.Controls.Add(this.cmbPrio);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chbVeto);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbEvents);
            this.Name = "FrmEvents";
            this.Text = "Events";
            this.Load += new System.EventHandler(this.FrmEvents_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbEvents;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chbVeto;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbPrio;
        private System.Windows.Forms.Label Month;
        private System.Windows.Forms.Label Mjan;
        private System.Windows.Forms.Label Mfeb;
        private System.Windows.Forms.Label Mmar;
        private System.Windows.Forms.Label Mapr;
        private System.Windows.Forms.Label Mmaj;
        private System.Windows.Forms.Label Mjun;
        private System.Windows.Forms.Label Mjul;
        private System.Windows.Forms.Label Maug;
        private System.Windows.Forms.Label Msep;
        private System.Windows.Forms.Label Moct;
        private System.Windows.Forms.Label Mnov;
        private System.Windows.Forms.Label Mdec;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkjan;
        private System.Windows.Forms.CheckBox checkfeb;
        private System.Windows.Forms.CheckBox checkmar;
        private System.Windows.Forms.CheckBox checkaug;
        private System.Windows.Forms.CheckBox checkmaj;
        private System.Windows.Forms.CheckBox checkjun;
        private System.Windows.Forms.CheckBox checkjul;
        private System.Windows.Forms.CheckBox checkapr;
        private System.Windows.Forms.CheckBox checksep;
        private System.Windows.Forms.CheckBox checkoct;
        private System.Windows.Forms.CheckBox checknov;
        private System.Windows.Forms.CheckBox checkdec;
        private System.Windows.Forms.ComboBox cmbjan;
        private System.Windows.Forms.ComboBox cmbfeb;
        private System.Windows.Forms.ComboBox cmbmar;
        private System.Windows.Forms.ComboBox cmbapr;
        private System.Windows.Forms.ComboBox cmbmaj;
        private System.Windows.Forms.ComboBox cmbjun;
        private System.Windows.Forms.ComboBox cmbjul;
        private System.Windows.Forms.ComboBox cmbaug;
        private System.Windows.Forms.ComboBox cmbsep;
        private System.Windows.Forms.ComboBox cmboct;
        private System.Windows.Forms.ComboBox cmbnov;
        private System.Windows.Forms.ComboBox cmbdec;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.Button btncancel;
    }
}