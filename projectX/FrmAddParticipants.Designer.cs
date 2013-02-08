namespace projectX
{
    partial class FrmAddParticipants
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbPaid = new System.Windows.Forms.ComboBox();
            this.lblpaid = new System.Windows.Forms.Label();
            this.cbDateOfBirthDay = new System.Windows.Forms.ComboBox();
            this.cbDateOfBirthMonth = new System.Windows.Forms.ComboBox();
            this.cbVegetarian = new System.Windows.Forms.CheckBox();
            this.txtAllergy = new System.Windows.Forms.TextBox();
            this.txtSection = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblVegetarian = new System.Windows.Forms.Label();
            this.lblAllergy = new System.Windows.Forms.Label();
            this.lblDateOfBirth = new System.Windows.Forms.Label();
            this.lblSection = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cbPaid);
            this.panel1.Controls.Add(this.lblpaid);
            this.panel1.Controls.Add(this.cbDateOfBirthDay);
            this.panel1.Controls.Add(this.cbDateOfBirthMonth);
            this.panel1.Controls.Add(this.cbVegetarian);
            this.panel1.Controls.Add(this.txtAllergy);
            this.panel1.Controls.Add(this.txtSection);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.lblVegetarian);
            this.panel1.Controls.Add(this.lblAllergy);
            this.panel1.Controls.Add(this.lblDateOfBirth);
            this.panel1.Controls.Add(this.lblSection);
            this.panel1.Controls.Add(this.lblName);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(212, 186);
            this.panel1.TabIndex = 0;
            // 
            // cbPaid
            // 
            this.cbPaid.FormattingEnabled = true;
            this.cbPaid.Items.AddRange(new object[] {
            "NO",
            "YES"});
            this.cbPaid.Location = new System.Drawing.Point(88, 154);
            this.cbPaid.Name = "cbPaid";
            this.cbPaid.Size = new System.Drawing.Size(43, 21);
            this.cbPaid.TabIndex = 14;
            // 
            // lblpaid
            // 
            this.lblpaid.AutoSize = true;
            this.lblpaid.Location = new System.Drawing.Point(17, 154);
            this.lblpaid.Name = "lblpaid";
            this.lblpaid.Size = new System.Drawing.Size(31, 13);
            this.lblpaid.TabIndex = 13;
            this.lblpaid.Text = "Paid:";
            // 
            // cbDateOfBirthDay
            // 
            this.cbDateOfBirthDay.FormattingEnabled = true;
            this.cbDateOfBirthDay.Location = new System.Drawing.Point(145, 68);
            this.cbDateOfBirthDay.Name = "cbDateOfBirthDay";
            this.cbDateOfBirthDay.Size = new System.Drawing.Size(43, 21);
            this.cbDateOfBirthDay.TabIndex = 12;
            // 
            // cbDateOfBirthMonth
            // 
            this.cbDateOfBirthMonth.FormattingEnabled = true;
            this.cbDateOfBirthMonth.Items.AddRange(new object[] {
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
            this.cbDateOfBirthMonth.Location = new System.Drawing.Point(91, 68);
            this.cbDateOfBirthMonth.Name = "cbDateOfBirthMonth";
            this.cbDateOfBirthMonth.Size = new System.Drawing.Size(48, 21);
            this.cbDateOfBirthMonth.TabIndex = 11;
            this.cbDateOfBirthMonth.SelectedValueChanged += new System.EventHandler(this.cbDateOfBirthMonth_SelectedValueChanged);
            // 
            // cbVegetarian
            // 
            this.cbVegetarian.AutoSize = true;
            this.cbVegetarian.Location = new System.Drawing.Point(88, 129);
            this.cbVegetarian.Name = "cbVegetarian";
            this.cbVegetarian.Size = new System.Drawing.Size(15, 14);
            this.cbVegetarian.TabIndex = 8;
            this.cbVegetarian.UseVisualStyleBackColor = true;
            // 
            // txtAllergy
            // 
            this.txtAllergy.Location = new System.Drawing.Point(88, 103);
            this.txtAllergy.Name = "txtAllergy";
            this.txtAllergy.Size = new System.Drawing.Size(100, 20);
            this.txtAllergy.TabIndex = 7;
            // 
            // txtSection
            // 
            this.txtSection.Location = new System.Drawing.Point(88, 37);
            this.txtSection.Name = "txtSection";
            this.txtSection.Size = new System.Drawing.Size(100, 20);
            this.txtSection.TabIndex = 6;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(88, 13);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 20);
            this.txtName.TabIndex = 5;
            // 
            // lblVegetarian
            // 
            this.lblVegetarian.AutoSize = true;
            this.lblVegetarian.Location = new System.Drawing.Point(16, 130);
            this.lblVegetarian.Name = "lblVegetarian";
            this.lblVegetarian.Size = new System.Drawing.Size(61, 13);
            this.lblVegetarian.TabIndex = 4;
            this.lblVegetarian.Text = "Vegetarian:";
            // 
            // lblAllergy
            // 
            this.lblAllergy.AutoSize = true;
            this.lblAllergy.Location = new System.Drawing.Point(16, 103);
            this.lblAllergy.Name = "lblAllergy";
            this.lblAllergy.Size = new System.Drawing.Size(41, 13);
            this.lblAllergy.TabIndex = 3;
            this.lblAllergy.Text = "Allergy:";
            // 
            // lblDateOfBirth
            // 
            this.lblDateOfBirth.AutoSize = true;
            this.lblDateOfBirth.Location = new System.Drawing.Point(17, 71);
            this.lblDateOfBirth.Name = "lblDateOfBirth";
            this.lblDateOfBirth.Size = new System.Drawing.Size(68, 13);
            this.lblDateOfBirth.TabIndex = 2;
            this.lblDateOfBirth.Text = "Date of birth:";
            // 
            // lblSection
            // 
            this.lblSection.AutoSize = true;
            this.lblSection.Location = new System.Drawing.Point(16, 37);
            this.lblSection.Name = "lblSection";
            this.lblSection.Size = new System.Drawing.Size(46, 13);
            this.lblSection.TabIndex = 1;
            this.lblSection.Text = "Section:";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(16, 13);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Location = new System.Drawing.Point(12, 204);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(212, 46);
            this.panel2.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(139, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(20, 9);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(50, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FrmAddParticipants
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 259);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAddParticipants";
            this.ShowInTaskbar = false;
            this.Text = "New Participant";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAddParticipants_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox cbVegetarian;
        private System.Windows.Forms.TextBox txtAllergy;
        private System.Windows.Forms.TextBox txtSection;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblVegetarian;
        private System.Windows.Forms.Label lblAllergy;
        private System.Windows.Forms.Label lblDateOfBirth;
        private System.Windows.Forms.Label lblSection;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ComboBox cbDateOfBirthMonth;
        private System.Windows.Forms.ComboBox cbDateOfBirthDay;
        private System.Windows.Forms.ComboBox cbPaid;
        private System.Windows.Forms.Label lblpaid;
    }
}