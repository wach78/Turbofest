using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenGL
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FrmDialog : Form
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Caption"></param>
        /// <param name="Buttons"></param>
        public FrmDialog(/*IWin32Window Parent,*/ string Text, string Caption, MessageBoxButtons Buttons)
        {
            //this.Parent = (Control)Parent;
            InitializeComponent();
            this.Text = Caption;
            label1.Text = Text;
            if (Buttons == MessageBoxButtons.AbortRetryIgnore)
            {
                button1.Text = "Abort";
                button1.DialogResult = DialogResult.Abort;
                button2.Text = "Retry";
                button2.DialogResult = DialogResult.Retry;
                button3.Text = "Ignore";
                button3.DialogResult = DialogResult.Ignore;
            }
            else if (Buttons == MessageBoxButtons.OK)
            {
                button1.Text = "OK";
                button1.DialogResult = DialogResult.OK;
                button2.Enabled = false;
                button2.Visible = false;
                button3.Enabled = false;
                button3.Visible = false;
            }
            else if (Buttons == MessageBoxButtons.OKCancel)
            {
                button1.Text = "OK";
                button1.DialogResult = DialogResult.OK;
                button2.Text = "Cancel";
                button2.DialogResult = DialogResult.Cancel;
                button3.Enabled = false;
                button3.Visible = false;
            }
            else if (Buttons == MessageBoxButtons.RetryCancel)
            {
                button1.Text = "Retry";
                button1.DialogResult = DialogResult.Retry;
                button2.Text = "Cancel";
                button2.DialogResult = DialogResult.Cancel;
                button3.Enabled = false;
                button3.Visible = false;
            }
            else if (Buttons == MessageBoxButtons.YesNo)
            {
                button1.Text = "Yes";
                button1.DialogResult = DialogResult.Yes;
                button2.Text = "No";
                button2.DialogResult = DialogResult.No;
                button3.Enabled = false;
                button3.Visible = false;
            }
            else if (Buttons == MessageBoxButtons.YesNoCancel)
            {
                button1.Text = "Yes";
                button1.DialogResult = DialogResult.Yes;
                button2.Text = "No";
                button2.DialogResult = DialogResult.No;
                button3.Text = "Cancel";
                button3.DialogResult = DialogResult.Cancel;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = button1.DialogResult;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = button1.DialogResult;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = button1.DialogResult;
            this.Close();
        }

        
    }
}
