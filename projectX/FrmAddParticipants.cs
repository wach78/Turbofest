using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using wach.Validate;

namespace projectX
{
    public partial class FrmAddParticipants : Form
    {
        private Validate validate;
        private string[] data;
      
        public string[] Data
        {
                get { return data; }
                private set { data = value; }
        }
    
        public FrmAddParticipants()
        {
            InitializeComponent();
            init();   
        }

        public FrmAddParticipants(string[] d)
        {
            InitializeComponent();
            init();

            txtName.Text = d[0];
            txtSection.Text = d[1];

            string month, day;

            Logic.adjustDate(d[2],out month,out day);
            cbDateOfBirthMonth.SelectedItem = month;
            cbDateOfBirthDay.SelectedItem = int.Parse(day);
            
            txtAllergy.Text = d[3];

            if ("Yes".Equals(d[4]))
            {
                cbVegetarian.Checked = true;
            }

            else if ("No".Equals(d[4]))
            {
                cbVegetarian.Checked = false;
            }

            cbPaid.SelectedItem = d[5];
        }

        private void init()
        {
            validate = new Validate();
            data = new String[6];
            cbDateOfBirthMonth.SelectedIndex = 0;
            cbDateOfBirthDay.SelectedIndex = 0;
        }
        
    
       
        private void cbDateOfBirthMonth_SelectedValueChanged(object sender, EventArgs e)
        {
            validate.addDaysToComboBox(cbDateOfBirthMonth.Text, ref cbDateOfBirthDay);
            cbDateOfBirthDay.SelectedIndex = 0;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (DialogResult != DialogResult.Cancel && validate.validateTxtBox(txtName, "Name can not be empty") &&
                validate.validateComboBox(cbDateOfBirthDay, "Date Of Birth, can not be empty") &&
                validate.validateComboBox(cbDateOfBirthMonth, "Date Of Birth, can not be empty") &&
                validate.validateComboBox(cbPaid," paid, can not be empty"))
             {
                Data[0] = txtName.Text;
                Data[1] = txtSection.Text;
                Data[2] = cbDateOfBirthMonth.SelectedItem + " " + cbDateOfBirthDay.SelectedItem;
                Data[3] = txtAllergy.Text;

                if (cbVegetarian.Checked)
                    Data[4] = "Yes";
                else
                    Data[4] = "No";

                Data[5] = cbPaid.SelectedItem + "";
             }

            else if (DialogResult != DialogResult.Cancel)
                DialogResult = DialogResult.None;
        }//btnOk_Click

        private void FrmAddParticipants_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*
            if (DialogResult != DialogResult.Cancel && validate.validateTxtBox(txtName, "Name can not be empty") &&
                validate.validateComboBox(cbDateOfBirthDay, "Date Of Birth, can not be empty") &&
                validate.validateComboBox(cbDateOfBirthMonth, "Date Of Birth, can not be empty"))
            {
               
            }
             
            else if (DialogResult != DialogResult.Cancel)
                DialogResult = DialogResult.None;

               */
           
        }
       
    }//class
}//namespace
