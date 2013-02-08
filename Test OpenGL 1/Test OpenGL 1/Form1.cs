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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenTK.DisplayDevice dev = OpenTK.DisplayDevice.Default;
            OpenTK.GameWindow asd2 = new OpenTK.GameWindow(1024,768, OpenTK.Graphics.GraphicsMode.Default, "Project X", OpenTK.GameWindowFlags.Default, dev, 3, 0, OpenTK.Graphics.GraphicsContextFlags.Debug);
            //asd2.Run();

            GLWindow asd3 = new GLWindow(null, null);
            asd3.Run();
            
        }


    }
}
