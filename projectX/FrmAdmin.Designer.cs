namespace projectX
{
    partial class FrmAdmin
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
            this.btnChangeDate = new System.Windows.Forms.Button();
            this.btncancel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbRunTime = new System.Windows.Forms.ComboBox();
            this.lblRunTime = new System.Windows.Forms.Label();
            this.cbEndTimeDay = new System.Windows.Forms.ComboBox();
            this.cbStartTimeDay = new System.Windows.Forms.ComboBox();
            this.cbEndTimeMonth = new System.Windows.Forms.ComboBox();
            this.cbStartTimeMonth = new System.Windows.Forms.ComboBox();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.listViewScrollers = new System.Windows.Forms.ListView();
            this.txtAddScroller = new System.Windows.Forms.TextBox();
            this.btnDel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnChangeDate
            // 
            this.btnChangeDate.Location = new System.Drawing.Point(3, 18);
            this.btnChangeDate.Name = "btnChangeDate";
            this.btnChangeDate.Size = new System.Drawing.Size(100, 25);
            this.btnChangeDate.TabIndex = 0;
            this.btnChangeDate.Text = "Change settings";
            this.btnChangeDate.UseVisualStyleBackColor = true;
            this.btnChangeDate.Click += new System.EventHandler(this.btnChangeDate_Click);
            // 
            // btncancel
            // 
            this.btncancel.Location = new System.Drawing.Point(142, 18);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(50, 25);
            this.btncancel.TabIndex = 1;
            this.btncancel.Text = "Done";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(13, 29);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(50, 25);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cbRunTime);
            this.panel1.Controls.Add(this.lblRunTime);
            this.panel1.Controls.Add(this.cbEndTimeDay);
            this.panel1.Controls.Add(this.cbStartTimeDay);
            this.panel1.Controls.Add(this.cbEndTimeMonth);
            this.panel1.Controls.Add(this.cbStartTimeMonth);
            this.panel1.Controls.Add(this.lblEndDate);
            this.panel1.Controls.Add(this.lblStartDate);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(197, 120);
            this.panel1.TabIndex = 3;
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
            this.cbRunTime.Location = new System.Drawing.Point(62, 85);
            this.cbRunTime.Name = "cbRunTime";
            this.cbRunTime.Size = new System.Drawing.Size(110, 21);
            this.cbRunTime.TabIndex = 20;
            // 
            // lblRunTime
            // 
            this.lblRunTime.AutoSize = true;
            this.lblRunTime.Location = new System.Drawing.Point(4, 85);
            this.lblRunTime.Name = "lblRunTime";
            this.lblRunTime.Size = new System.Drawing.Size(52, 13);
            this.lblRunTime.TabIndex = 19;
            this.lblRunTime.Text = "Run time:";
            // 
            // cbEndTimeDay
            // 
            this.cbEndTimeDay.FormattingEnabled = true;
            this.cbEndTimeDay.Location = new System.Drawing.Point(135, 48);
            this.cbEndTimeDay.Name = "cbEndTimeDay";
            this.cbEndTimeDay.Size = new System.Drawing.Size(39, 21);
            this.cbEndTimeDay.TabIndex = 18;
            // 
            // cbStartTimeDay
            // 
            this.cbStartTimeDay.FormattingEnabled = true;
            this.cbStartTimeDay.Location = new System.Drawing.Point(135, 17);
            this.cbStartTimeDay.Name = "cbStartTimeDay";
            this.cbStartTimeDay.Size = new System.Drawing.Size(39, 21);
            this.cbStartTimeDay.TabIndex = 17;
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
            this.cbEndTimeMonth.Location = new System.Drawing.Point(65, 48);
            this.cbEndTimeMonth.Name = "cbEndTimeMonth";
            this.cbEndTimeMonth.Size = new System.Drawing.Size(48, 21);
            this.cbEndTimeMonth.TabIndex = 15;
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
            this.cbStartTimeMonth.Location = new System.Drawing.Point(65, 17);
            this.cbStartTimeMonth.Name = "cbStartTimeMonth";
            this.cbStartTimeMonth.Size = new System.Drawing.Size(48, 21);
            this.cbStartTimeMonth.TabIndex = 16;
            this.cbStartTimeMonth.SelectedValueChanged += new System.EventHandler(this.cbStartTimeMonth_SelectedValueChanged);
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new System.Drawing.Point(4, 49);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(55, 13);
            this.lblEndDate.TabIndex = 7;
            this.lblEndDate.Text = "End Date:";
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(4, 17);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(58, 13);
            this.lblStartDate.TabIndex = 6;
            this.lblStartDate.Text = "Start Date:";
            // 
            // listViewScrollers
            // 
            this.listViewScrollers.Location = new System.Drawing.Point(215, 12);
            this.listViewScrollers.Name = "listViewScrollers";
            this.listViewScrollers.Size = new System.Drawing.Size(259, 120);
            this.listViewScrollers.TabIndex = 4;
            this.listViewScrollers.UseCompatibleStateImageBehavior = false;
            // 
            // txtAddScroller
            // 
            this.txtAddScroller.Location = new System.Drawing.Point(13, 3);
            this.txtAddScroller.Name = "txtAddScroller";
            this.txtAddScroller.Size = new System.Drawing.Size(229, 20);
            this.txtAddScroller.TabIndex = 5;
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(192, 29);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(50, 25);
            this.btnDel.TabIndex = 6;
            this.btnDel.Text = "Delete";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnChangeDate);
            this.panel2.Controls.Add(this.btncancel);
            this.panel2.Location = new System.Drawing.Point(12, 138);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(197, 65);
            this.panel2.TabIndex = 7;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.txtAddScroller);
            this.panel3.Controls.Add(this.btnAdd);
            this.panel3.Controls.Add(this.btnDel);
            this.panel3.Location = new System.Drawing.Point(215, 138);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(259, 65);
            this.panel3.TabIndex = 8;
            // 
            // FrmAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 213);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.listViewScrollers);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAdmin";
            this.ShowInTaskbar = false;
            this.Text = "Admin Party";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnChangeDate;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.ComboBox cbEndTimeDay;
        private System.Windows.Forms.ComboBox cbStartTimeDay;
        private System.Windows.Forms.ComboBox cbEndTimeMonth;
        private System.Windows.Forms.ComboBox cbStartTimeMonth;
        private System.Windows.Forms.ComboBox cbRunTime;
        private System.Windows.Forms.Label lblRunTime;
        private System.Windows.Forms.ListView listViewScrollers;
        private System.Windows.Forms.TextBox txtAddScroller;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
    }
}