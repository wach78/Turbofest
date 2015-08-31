using System;
using System.Windows.Forms;
using wach.Validate;
using System.Xml;
using System.Media;

namespace projectX
{
    public partial class FrmAdmin : Form
    {
        private Validate validate;
        private Logic logic;
        private string  fileName;
        private string oldRunTime;
        private static string resolution;

        public static int[] Resolution
        {
            get
            {
                int w = int.Parse(resolution.Substring(0,resolution.IndexOf("x")));
                int h = int.Parse(resolution.Substring(resolution.IndexOf("x") + 1, resolution.IndexOf("@") - resolution.IndexOf("x")-1));
                int r = int.Parse(resolution.Substring(resolution.IndexOf("@") + 1));

                return new int[]{w, h, r}; 
            }
    
        }

        public static string Res
        {
            set { resolution = value; }
        }
        public FrmAdmin()
        {
            InitializeComponent();
            init();
            validate = new Validate();
            logic = new Logic();
            fileName = FrmMain.FileName.ToString();
            loadData();
        }

        private void init()
        {
            ColumnHeader column = new ColumnHeader();
            column.Width = 255;
            column.Text = "Name";

            listViewScrollers.Columns.Add(column);

            listViewScrollers.View = View.Details;
            listViewScrollers.GridLines = true;
            listViewScrollers.FullRowSelect = true;
            listViewScrollers.MultiSelect = false;

            screenSize();

            cbResolution.SelectedItem = resolution; // rhis might bug out...
        }//init

        private void screenSize()
        {
            
            foreach (var res in OpenTK.DisplayDevice.Default.AvailableResolutions) 
            {
                if (res.BitsPerPixel == Screen.PrimaryScreen.BitsPerPixel && !cbResolution.Items.Contains(res.Width + "x" + res.Height + "@" + res.RefreshRate)) 
                {
                     cbResolution.Items.Add(res.Width +"x"+ res.Height+"@"+res.RefreshRate);
                }
            }
            
        }

        private void loadData()
        {
            string data = logic.getData(fileName);
            //2013-03-012012-09-0114400000
            cbStartTimeMonth.SelectedItem =  Logic.monthName(data.Substring(5,2));
            cbStartTimeDay.SelectedItem = int.Parse(data.Substring(8, 2));

            cbEndTimeMonth.SelectedItem = Logic.monthName(data.Substring(15, 2));
            cbEndTimeDay.SelectedItem = int.Parse(data.Substring(18, 2));

            data = data.Remove(0,20);
            oldRunTime = data;
            cbRunTime.SelectedItem = Logic.runTimeString(data);

            updateList();
        }//loadData

        private void updateList()
        {
            listViewScrollers.Items.Clear();
            
            System.Xml.XmlDocument loadDoc = new System.Xml.XmlDocument();
            
            string path = XmlHandler.fixPath("Scrollers" +"/Scrollers.xml");
            loadDoc.Load(path);
            XmlNodeList n= loadDoc.SelectNodes("scrollers/scroller");

	 	    for (int i = 0; i < n.Count; i++)
	        {
	            XmlNode name = n.Item(i).SelectSingleNode("name");         
                listViewScrollers.Items.Add(name.InnerText).ToString();
	        }
            
        }//updateList

        private void cbStartTimeMonth_SelectedValueChanged(object sender, EventArgs e)
        {
            validate.addDaysToComboBox(cbStartTimeMonth.Text, ref cbStartTimeDay);
            cbStartTimeDay.SelectedIndex = 0;
        }

        private void cbEndTimeMonth_SelectedValueChanged(object sender, EventArgs e)
        {
            validate.addDaysToComboBox(cbEndTimeMonth.Text, ref cbEndTimeDay);

            cbEndTimeDay.SelectedIndex = 0;
        }

        private void btnChangeDate_Click(object sender, EventArgs e)
        {
            logic.updateData(fileName,oldRunTime, cbStartTimeMonth.SelectedItem + " " + cbStartTimeDay.SelectedItem,
                      cbEndTimeMonth.SelectedItem + " " + cbEndTimeDay.SelectedItem, cbRunTime.SelectedItem + "");
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtAddScroller.Text != "")
            {
                Logic logic = new Logic();
                logic.addScroller(txtAddScroller.Text);
                updateList();
                txtAddScroller.Text = "";
            }            
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            
            SystemSounds.Beep.Play();
            DialogResult result = MessageBox.Show("Delete this file?", "Important ", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                Logic logic = new Logic();
                logic.delScroller(listViewScrollers.SelectedItems[0].Text);
                updateList();
            }   
        }

        private void cbResolution_SelectedValueChanged(object sender, EventArgs e)
        {
            resolution = cbResolution.Text;
        }

        private void eventP_Click(object sender, EventArgs e)
        {
            FrmEvents effects = new FrmEvents();
            effects.ShowDialog();
        }
    }//class
}//namespace
