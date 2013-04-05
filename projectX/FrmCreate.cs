using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using wach.Validate;
using System.IO;
using System.Media;

namespace projectX
{
    
    public partial class FrmCreate : Form
    {
        private Validate validate;
       // private string springOrFall;
        public FrmCreate()
        {
            InitializeComponent();

            init();


        }

        private void init()
        {
           
            validate = new Validate();
            //springOrFall = "";

            cbRunTime.SelectedItem = "4 hours";

            if (rBtnSpring.Checked)
            {
                cbStartTimeMonth.SelectedItem = "Sep";
                cbEndTimeMonth.SelectedItem = "Mar";
                FrmMain.SpringOrFall = "Spring";
            }

            else if (rbtnFall.Checked)
            {
                cbStartTimeMonth.SelectedItem = "Mar";
                cbEndTimeMonth.SelectedItem = "sep";
                FrmMain.SpringOrFall = "Fall";
            }

        
        }//init

        private void rBtnSpring_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.ShowAlways = true;
            toolTip.SetToolTip(rBtnSpring, "Höstdagjämningen → Vårdagjämningen");
        }//rBtnSpring_MouseHover

        private void rbtnFall_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.ShowAlways = true;
            toolTip.SetToolTip(rbtnFall, "Vårdagjämningen → Höstdagjämningen");

        }//rbtnFall_MouseHover

        private void btnNext_Click(object sender, EventArgs e)
        {
 
            if (validate.validateComboBox(cbStartTimeMonth,"Start Time month can not be empty") &&
                validate.validateComboBox(cbStartTimeDay, "Start Time day can not be empty") &&
                validate.validateComboBox(cbEndTimeMonth,"End Time month can not be empty") &&
                validate.validateComboBox(cbEndTimeDay, "End time day can not be empty"))
            {

             string str = "";

             if ("Spring".Equals(FrmMain.SpringOrFall))
                 str = "VT";

             if ("Fall".Equals(FrmMain.SpringOrFall))
                 str = "HT";

                string file = "Turbofest";
                file += str;
                file += DateTime.Now.Year;

                string path = XmlHandler.fixPath(file +  ".xml");

                string fileName = Path.GetFullPath(path);
  
                if (!File.Exists(fileName))
                {
                    Logic logic = new Logic();
                    logic.createparty(FrmMain.SpringOrFall, file, cbStartTimeMonth.SelectedItem + " " + cbStartTimeDay.SelectedItem,
                      cbEndTimeMonth.SelectedItem + " " + cbEndTimeDay.SelectedItem, cbRunTime.SelectedItem + "");

                    this.Dispose();
                }
                else
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("File alredy exits.");
                    DialogResult = DialogResult.None;
                }

                
            }                
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
                        
            //firstPanel.Show();         
            
        }//rbtnFall_MouseHover

        private void btnCancelSecondPanel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void cbStartTimeMonth_SelectedValueChanged(object sender, EventArgs e)
        {
            validate.addDaysToComboBox(cbStartTimeMonth.Text, ref cbStartTimeDay);

            cbStartTimeDay.SelectedIndex = 0;
        }//cbStartTimeMonth_SelectedValueChanged

        private void cbEndTimeMonth_SelectedValueChanged(object sender, EventArgs e)
        {
            validate.addDaysToComboBox(cbEndTimeMonth.Text, ref cbEndTimeDay);

            cbEndTimeDay.SelectedIndex = 0;
        }//cbEndTimeMonth_SelectedValueChanged

        private void rBtnSpring_CheckedChanged(object sender, EventArgs e)
        {
            cbStartTimeMonth.SelectedItem = "Sep";
            cbEndTimeMonth.SelectedItem = "Mar";
            FrmMain.SpringOrFall = "Spring";
        }

        private void rbtnFall_CheckedChanged(object sender, EventArgs e)
        {
            cbStartTimeMonth.SelectedItem = "Mar";
            cbEndTimeMonth.SelectedItem = "Sep";
            FrmMain.SpringOrFall = "Fall";
        }

     
    }//class
}//namespace
