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
using System.IO;
using System.Media;
using System.Drawing.Printing;
using System.Xml;

namespace projectX
{
    public partial class FrmMain : Form
    {
        private static string fileName;
        public static string FileName
        {
            get { return fileName; }
            private set { fileName = value; }
        }

        private static string springOrFall;
        public static string SpringOrFall
        {
            get { return springOrFall; }
            set { springOrFall = value; }
        }

        private Logic logic;
        private int m_lngPrintingPage;
        private int m_lngPrintingRow;
        frmPrinterSetup frmPS = new frmPrinterSetup();

        public FrmMain()
        {
            InitializeComponent();
            init();
  
            uppdateFilesToListView();
        }

     

        public void uppdateFilesToListView()
        {
            string path = XmlHandler.fixPath("");// catalog XMLFiles

            lViewParty.Items.Clear();
            string[] files = Directory.GetFiles(path, "*.xml", SearchOption.TopDirectoryOnly);

           foreach (string f in files)
           {
               lViewParty.Items.Add(Path.GetFileNameWithoutExtension(f));
           }            
        }//uppdateFilesToListView

        private void init()
        {
            logic = new Logic();
            btnAdd.Enabled = false;
            btnStart.Enabled = false;
            AdminToolStripMenuItem.Enabled = false;
            printToolStripMenuItem.Enabled = false;
            printPreviewToolStripMenuItem.Enabled = false;

            m_lngPrintingPage = 1;
            m_lngPrintingRow = 0;

            ColumnHeader column = new ColumnHeader();
            column.Width = 150;
            column.Text = "Name";
            column.AutoResize(ColumnHeaderAutoResizeStyle.None);

            lViewParty.Columns.Add(column);

            lViewParty.View = View.Details;
            lViewParty.GridLines = true;
            lViewParty.FullRowSelect = true;
            lViewParty.MultiSelect = false;
            fileName = "";

            FrmAdmin.Res = "800x600" + "@" + OpenTK.DisplayDevice.AvailableDisplays[0].RefreshRate;
        
           
        }//init

        private void btnCreate_Click(object sender, EventArgs e)
        {
            FrmCreate create = new FrmCreate();

            DialogResult resultat = create.ShowDialog();

            if (resultat == DialogResult.OK)
                uppdateFilesToListView();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FrmParticipants Participants = new FrmParticipants(ref printDocument);

            Participants.ShowDialog();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (lViewParty.SelectedIndices.Count > 0)
            {
                fileName = lViewParty.SelectedItems[0].Text;

                String season = fileName.Substring(9,2);

                if ("VT".Equals(season))
                {
                    SpringOrFall = "Spring";
                }
                else if ("HT".Equals(season))
                {
                    SpringOrFall = "Fall";
                }

                fileName += ".xml";

                btnAdd.Enabled = true;
                btnStart.Enabled = true;
                AdminToolStripMenuItem.Enabled = true;
                printToolStripMenuItem.Enabled = true;
                printPreviewToolStripMenuItem.Enabled = true;

                logic.getData(fileName);

                toolStripStatusLabel1.Text = Path.GetFileNameWithoutExtension(fileName);
            }
            /*else
            {
                throw new Exception("You need to select something");
            }*/
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                fileName = lViewParty.SelectedItems[0].Text;
                fileName += ".xml";

                string path = XmlHandler.fixPath(fileName);


                fileName = Path.GetFullPath(path);

                if (File.Exists(fileName))
                {
                    SystemSounds.Beep.Play();
                    DialogResult result = MessageBox.Show("Delete this file?", "Important ", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        File.Delete(fileName);
                        uppdateFilesToListView();
                    }
                }
                else
                {
                    MessageBox.Show("File does not exist!");
                }
            }
            catch (Exception)
            {

            }
            
        }//btnDel_Click

        private void closeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }

        OpenGL.GLWindow gw = null;
        private void btnStart_Click(object sender, EventArgs e)
        {
            XmlHandler xmlStuff = new XmlHandler(fileName, "XDoc");
            if (gw != null)
            {
                gw.Dispose();
                Console.WriteLine("Starting dispose of old Gwindow.");
                gw = null;
            }
            OpenGL.CrashHandler ch = new OpenGL.CrashHandler();
            if (ch.CheckCrash() != System.Windows.Forms.DialogResult.Cancel)
            {
                gw = new OpenGL.GLWindow(xmlStuff.sortXml(), xmlStuff.getDataFromXml(), FrmAdmin.Resolution, ref ch);
                gw.Run(); // possible to start in another thread??
            }
            if (ch.Exit)
            {
                ch.Clear();
            }
            ch.Dispose();
            ch = null;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAdmin admin = new FrmAdmin();
            admin.ShowDialog();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDocument.Print();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Printer dialog throwed a error. " + ex.Message);
            }
        

        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            
            Graphics g = e.Graphics;
            SolidBrush Brush = new SolidBrush(Color.Black);

            System.Xml.Linq.XDocument dataDoc = logic.sort(fileName); // catch error here?

            int column = 1; // we need 3 columns, eventname, date, done
            float height = 0;
            Font wbfont = new Font("arial", 12, FontStyle.Bold);
            Font wfont = new Font("arial", 10);
            Pen pen = new Pen(Brush);

            height += wbfont.Height/2;
            g.DrawString("Event", wbfont, Brush, 10, height);
            g.DrawString("Date", wbfont, Brush, (int)(e.MarginBounds.Width * 0.50), height);
            g.DrawString("Done", wbfont, Brush, (int)(e.MarginBounds.Width * 0.80), height);
            height += wbfont.Height + 2;
            //height += wfont.Height+2;
            
            g.DrawLine(pen, new Point(0, (int)height), new Point(e.MarginBounds.Width, (int)height));
            int maxRows = dataDoc.Root.Elements("event").Count();
            height += 2;
            for (; m_lngPrintingRow < maxRows; ) // ugly but works for atleast 2 pages...
            //foreach (System.Xml.Linq.XElement item in dataDoc.Root.Elements("event").Count())
            {
                System.Xml.Linq.XElement item = dataDoc.Root.Elements("event").ElementAt(m_lngPrintingRow);
                //height += wfont.Height;
                g.DrawString(item.Element("type").Value + " - " + item.Element("name").Value.Replace("\n", " "), wfont, Brush, 10, height);
                g.DrawString(item.Element("date").Value, wfont, Brush, (int)(e.MarginBounds.Width * 0.50), height);
                height += wfont.Height + 2;
                g.DrawLine(pen, new Point(0, (int)height), new Point(e.MarginBounds.Width, (int)height));
                height += 2;
                column++;
                //to use this we need to have a external page counter and row counter as this will make it possible to track for more pages and where to start...
                if (height >= e.MarginBounds.Height)
                {
                    e.HasMorePages = true;
                    m_lngPrintingPage++;
                    break; // force to leave the loop as it would be none ending if it end up in here...
                }
                else
                {
                    e.HasMorePages = false;
                    m_lngPrintingRow++;
                    column = 0;
                }

            }
            column = 0;
            height -= 2.5f;
            g.DrawLine(pen, new Point(0, 0), new Point(0, (int)height)); //Left line
            g.DrawLine(pen, new Point(0, 0), new Point(e.MarginBounds.Width, 0)); //Top line
            g.DrawLine(pen, new Point(e.MarginBounds.Width, 0), new Point(e.MarginBounds.Width, (int)height)); //Right Line
            //g.DrawLine(pen, new Point(0, (int)height), new Point(e.MarginBounds.Width, (int)height)); // bottom line
            g.DrawLine(pen, new Point((int)(e.MarginBounds.Width * 0.50)-5, 0), new Point((int)(e.MarginBounds.Width * 0.50)-5, (int)height)); // column line 1-2
            g.DrawLine(pen, new Point((int)(e.MarginBounds.Width * 0.80)-5, 0), new Point((int)(e.MarginBounds.Width * 0.80)-5, (int)height)); // column line 2-3
            // g.DrawString(dsEvents.Tables[0].Rows[0].ToString(), new Font("arial", 9), Brush, 10, 10);
            
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {

            /*printDialog.AllowPrintToFile = false;
            printDialog.AllowCurrentPage = false;*/
            //printPreviewDialog = new PrintPreviewDialog();
            try
            {
                printPreviewDialog.Document = printDocument;
                printPreviewDialog.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Printer dialog throwed a error. " + ex.Message);
            }
            
        }

        private void printDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            m_lngPrintingRow = 0;
            m_lngPrintingPage = 1;
        }

        private void lViewParty_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lViewParty.SelectedItems[0].Selected)
                {
                    fileName = lViewParty.SelectedItems[0].Text;

                    String season = fileName.Substring(9, 2);

                    if ("VT".Equals(season))
                    {
                        SpringOrFall = "Spring";
                    }
                    else if ("HT".Equals(season))
                    {
                        SpringOrFall = "Fall";
                    }

                    fileName += ".xml";

                    btnAdd.Enabled = true;
                    btnStart.Enabled = true;
                    AdminToolStripMenuItem.Enabled = true;
                    printToolStripMenuItem.Enabled = true;
                    printPreviewToolStripMenuItem.Enabled = true;

                    logic.getData(fileName);

                    toolStripStatusLabel1.Text = Path.GetFileNameWithoutExtension(fileName);
                }
            }//try
            catch (Exception)
            {

            }
        }
        
        private void printerSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string defaultprinter = pageSetupDialog.PrinterSettings.PrinterName;
            if (frmPS.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (defaultprinter != frmPS.selectedPrinter())
                {
                    pageSetupDialog.PrinterSettings.PrinterName = frmPS.selectedPrinter();
                    printDocument.PrinterSettings = pageSetupDialog.PrinterSettings;
                }
            }
            /*
            pageSetupDialog.Document = printDocument;
            pageSetupDialog.ShowDialog();*/
        }

        private void lViewParty_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = lViewParty.Columns[e.ColumnIndex].Width;
        }

       
           
        

         
        
    }//class
}//Namespace
