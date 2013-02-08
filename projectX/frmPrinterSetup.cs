using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace projectX
{
    public partial class frmPrinterSetup : Form
    {
        string strSelected;
        public frmPrinterSetup()
        {
            PageSettings ps = new PageSettings();
            InitializeComponent();
            strSelected = ps.PrinterSettings.PrinterName;
        }

        private void frmPrinterSetup_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                listBox1.Items.Add(printer);
                if (printer == strSelected)
                {
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                }
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            strSelected = listBox1.SelectedItem.ToString();
        }

        public string selectedPrinter()
        {
            return strSelected;
        }
    }
}
