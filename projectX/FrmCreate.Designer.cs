namespace projectX
{
    partial class FrmCreate
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.datePanel = new System.Windows.Forms.Panel();
            this.cbRunTime = new System.Windows.Forms.ComboBox();
            this.cbEndTimeDay = new System.Windows.Forms.ComboBox();
            this.cbStartTimeDay = new System.Windows.Forms.ComboBox();
            this.cbEndTimeMonth = new System.Windows.Forms.ComboBox();
            this.cbStartTimeMonth = new System.Windows.Forms.ComboBox();
            this.lblRunTime = new System.Windows.Forms.Label();
            this.lblEndTime = new System.Windows.Forms.Label();
            this.labelStartTime = new System.Windows.Forms.Label();
            this.gBoxSpringOrFall = new System.Windows.Forms.GroupBox();
            this.rbtnFall = new System.Windows.Forms.RadioButton();
            this.rBtnSpring = new System.Windows.Forms.RadioButton();
            this.datePanel.SuspendLayout();
            this.gBoxSpringOrFall.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(12, 223);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 25);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNext
            // 
            this.btnNext.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnNext.Location = new System.Drawing.Point(162, 223);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(50, 25);
            this.btnNext.TabIndex = 5;
            this.btnNext.Text = "Ok";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // datePanel
            // 
            this.datePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.datePanel.Controls.Add(this.cbRunTime);
            this.datePanel.Controls.Add(this.cbEndTimeDay);
            this.datePanel.Controls.Add(this.cbStartTimeDay);
            this.datePanel.Controls.Add(this.cbEndTimeMonth);
            this.datePanel.Controls.Add(this.cbStartTimeMonth);
            this.datePanel.Controls.Add(this.lblRunTime);
            this.datePanel.Controls.Add(this.lblEndTime);
            this.datePanel.Controls.Add(this.labelStartTime);
            this.datePanel.Location = new System.Drawing.Point(12, 102);
            this.datePanel.Name = "datePanel";
            this.datePanel.Size = new System.Drawing.Size(200, 115);
            this.datePanel.TabIndex = 3;
            // 
            // cbRunTime
            // 
            this.cbRunTime.FormattingEnabled = true;
            this.cbRunTime.Items.AddRange(new object[] {
            "3 hours",
            "3.5 hours",
            "4 hours",
            "4.5 hours ",
            "5 hours",
            "Test 1 hour"});
            this.cbRunTime.Location = new System.Drawing.Point(61, 87);
            this.cbRunTime.Name = "cbRunTime";
            this.cbRunTime.Size = new System.Drawing.Size(110, 21);
            this.cbRunTime.TabIndex = 7;
            // 
            // cbEndTimeDay
            // 
            this.cbEndTimeDay.FormattingEnabled = true;
            this.cbEndTimeDay.Location = new System.Drawing.Point(132, 44);
            this.cbEndTimeDay.Name = "cbEndTimeDay";
            this.cbEndTimeDay.Size = new System.Drawing.Size(39, 21);
            this.cbEndTimeDay.TabIndex = 14;
            // 
            // cbStartTimeDay
            // 
            this.cbStartTimeDay.FormattingEnabled = true;
            this.cbStartTimeDay.Location = new System.Drawing.Point(132, 13);
            this.cbStartTimeDay.Name = "cbStartTimeDay";
            this.cbStartTimeDay.Size = new System.Drawing.Size(39, 21);
            this.cbStartTimeDay.TabIndex = 13;
            // 
            // cbEndTimeMonth
            // 
            this.cbEndTimeMonth.FormattingEnabled = true;
            this.cbEndTimeMonth.Items.AddRange(new object[] {
            "Jan",
            "Feb",
            "Mar",
            "Apr",
            "Maj",
            "Jun",
            "Jul",
            "Aug",
            "Sep",
            "Okt",
            "Nov",
            "Dec"});
            this.cbEndTimeMonth.Location = new System.Drawing.Point(62, 44);
            this.cbEndTimeMonth.Name = "cbEndTimeMonth";
            this.cbEndTimeMonth.Size = new System.Drawing.Size(48, 21);
            this.cbEndTimeMonth.TabIndex = 12;
            this.cbEndTimeMonth.SelectedValueChanged += new System.EventHandler(this.cbEndTimeMonth_SelectedValueChanged);
            // 
            // cbStartTimeMonth
            // 
            this.cbStartTimeMonth.FormattingEnabled = true;
            this.cbStartTimeMonth.Items.AddRange(new object[] {
            "Jan",
            "Feb",
            "Mar",
            "Apr",
            "Maj",
            "Jun",
            "Jul",
            "Aug",
            "Sep",
            "Okt",
            "Nov",
            "Dec"});
            this.cbStartTimeMonth.Location = new System.Drawing.Point(62, 13);
            this.cbStartTimeMonth.Name = "cbStartTimeMonth";
            this.cbStartTimeMonth.Size = new System.Drawing.Size(48, 21);
            this.cbStartTimeMonth.TabIndex = 12;
            this.cbStartTimeMonth.SelectedValueChanged += new System.EventHandler(this.cbStartTimeMonth_SelectedValueChanged);
            // 
            // lblRunTime
            // 
            this.lblRunTime.AutoSize = true;
            this.lblRunTime.Location = new System.Drawing.Point(3, 87);
            this.lblRunTime.Name = "lblRunTime";
            this.lblRunTime.Size = new System.Drawing.Size(52, 13);
            this.lblRunTime.TabIndex = 2;
            this.lblRunTime.Text = "Run time:";
            // 
            // lblEndTime
            // 
            this.lblEndTime.AutoSize = true;
            this.lblEndTime.Location = new System.Drawing.Point(5, 44);
            this.lblEndTime.Name = "lblEndTime";
            this.lblEndTime.Size = new System.Drawing.Size(51, 13);
            this.lblEndTime.TabIndex = 1;
            this.lblEndTime.Text = "End time:";
            // 
            // labelStartTime
            // 
            this.labelStartTime.AutoSize = true;
            this.labelStartTime.Location = new System.Drawing.Point(3, 13);
            this.labelStartTime.Name = "labelStartTime";
            this.labelStartTime.Size = new System.Drawing.Size(54, 13);
            this.labelStartTime.TabIndex = 0;
            this.labelStartTime.Text = "Start time:";
            // 
            // gBoxSpringOrFall
            // 
            this.gBoxSpringOrFall.Controls.Add(this.rbtnFall);
            this.gBoxSpringOrFall.Controls.Add(this.rBtnSpring);
            this.gBoxSpringOrFall.Location = new System.Drawing.Point(12, 12);
            this.gBoxSpringOrFall.Name = "gBoxSpringOrFall";
            this.gBoxSpringOrFall.Size = new System.Drawing.Size(200, 72);
            this.gBoxSpringOrFall.TabIndex = 2;
            this.gBoxSpringOrFall.TabStop = false;
            // 
            // rbtnFall
            // 
            this.rbtnFall.AutoSize = true;
            this.rbtnFall.Location = new System.Drawing.Point(70, 42);
            this.rbtnFall.Name = "rbtnFall";
            this.rbtnFall.Size = new System.Drawing.Size(41, 17);
            this.rbtnFall.TabIndex = 1;
            this.rbtnFall.TabStop = true;
            this.rbtnFall.Text = "Fall";
            this.rbtnFall.UseVisualStyleBackColor = true;
            this.rbtnFall.CheckedChanged += new System.EventHandler(this.rbtnFall_CheckedChanged);
            this.rbtnFall.MouseHover += new System.EventHandler(this.rbtnFall_MouseHover);
            // 
            // rBtnSpring
            // 
            this.rBtnSpring.AutoSize = true;
            this.rBtnSpring.Location = new System.Drawing.Point(70, 19);
            this.rBtnSpring.Name = "rBtnSpring";
            this.rBtnSpring.Size = new System.Drawing.Size(55, 17);
            this.rBtnSpring.TabIndex = 0;
            this.rBtnSpring.TabStop = true;
            this.rBtnSpring.Text = "Spring";
            this.rBtnSpring.UseVisualStyleBackColor = true;
            this.rBtnSpring.CheckedChanged += new System.EventHandler(this.rBtnSpring_CheckedChanged);
            this.rBtnSpring.MouseHover += new System.EventHandler(this.rBtnSpring_MouseHover);
            // 
            // FrmCreate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(227, 254);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.gBoxSpringOrFall);
            this.Controls.Add(this.datePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCreate";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "New Party";
            this.datePanel.ResumeLayout(false);
            this.datePanel.PerformLayout();
            this.gBoxSpringOrFall.ResumeLayout(false);
            this.gBoxSpringOrFall.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel datePanel;
        private System.Windows.Forms.GroupBox gBoxSpringOrFall;
        private System.Windows.Forms.RadioButton rbtnFall;
        private System.Windows.Forms.RadioButton rBtnSpring;
        private System.Windows.Forms.Label lblRunTime;
        private System.Windows.Forms.Label lblEndTime;
        private System.Windows.Forms.Label labelStartTime;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.ComboBox cbEndTimeDay;
        private System.Windows.Forms.ComboBox cbStartTimeDay;
        private System.Windows.Forms.ComboBox cbEndTimeMonth;
        private System.Windows.Forms.ComboBox cbStartTimeMonth;
        private System.Windows.Forms.ComboBox cbRunTime;
    }
}