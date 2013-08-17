using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Xml;
using System.IO;
using System.Drawing.Printing;
using System.Diagnostics;

namespace projectX
{
    public partial class FrmParticipants : Form
    {
        private XmlReader xmlFile;

        private string path;
        private string[] participantData;
        private int m_lngPrintingPage;
        private int m_lngPrintingRow;

        public FrmParticipants(ref PrintDocument pd)
        {
            InitializeComponent();
            m_lngPrintingPage = 1;
            m_lngPrintingRow = 0;

            path = XmlHandler.fixPath(FrmMain.FileName.ToString());
            participantData = new string[6];
            updateGridView();
            //printDocument = new PrintDocument();//pd;
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void updateGridView()
        {
            try
            {
                xmlFile = XmlReader.Create(path, new XmlReaderSettings());

                DataSet dsParticipant = new DataSet("participant");
                dsParticipant.ReadXml(xmlFile);
                dataGridView1.DataSource = dsParticipant;
                dataGridView1.DataMember = "participant";
            }

            catch (Exception )
            {
               // MessageBox.Show(ex.ToString());
            }

            finally
            {
                if (xmlFile != null)
                     xmlFile.Close();
            }
        }//updateGridView

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FrmAddParticipants add = new FrmAddParticipants();

            DialogResult resultat = add.ShowDialog();
            

            if (resultat == System.Windows.Forms.DialogResult.OK)
            {
                Logic logic = new Logic();
                logic.addparticpant(path,add.Data[0], add.Data[1], add.Data[2], add.Data[3], add.Data[4], add.Data[5]);
                updateGridView();
            }
            else if (resultat == System.Windows.Forms.DialogResult.Cancel)
                 add.Dispose();


        }// btnAdd_Click

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Logic logic = new Logic();
            
            logic.delParticpant(path,dataGridView1.CurrentRow);
            updateGridView();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            participantData[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            participantData[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            participantData[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            participantData[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            participantData[4] = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            participantData[5] = dataGridView1.CurrentRow.Cells[5].Value.ToString();

            FrmAddParticipants add = new FrmAddParticipants(participantData);

            DialogResult resultat = add.ShowDialog();

            if (resultat == System.Windows.Forms.DialogResult.OK)
            {
                Logic logic = new Logic();
                logic.updateParticpant(path,dataGridView1.CurrentRow, add.Data);
                updateGridView();
            }
            else if (resultat == System.Windows.Forms.DialogResult.Cancel)
                add.Dispose();

        }//dataGridView1_RowEnter

        private void btnPrintParticipants_Click(object sender, EventArgs e)
        {
            //printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;
             //System.Drawing.Printing.PrinterSettings.InstalledPrinters

            printPreviewDialog.ShowDialog();
        }

        private void printDocument_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            m_lngPrintingPage = 1;
            m_lngPrintingRow = 0;
            
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Logic logic = new Logic();
            IEnumerable<System.Xml.Linq.XElement> dataDoc = logic.getParticpant(this.path);
            Graphics g = e.Graphics;
            SolidBrush Brush = new SolidBrush(Color.Black);

            float height = 0;
            Font wbfont = new Font("arial", 12, FontStyle.Bold);
            Font wfont = new Font("arial", 10);
            Pen pen = new Pen(Brush);
            
            //height += wbfont.Height / 2;
            g.DrawString("Name", wbfont, Brush, 5, height + wbfont.Height);
            g.DrawString("Section", wbfont, Brush, (int)(e.MarginBounds.Width * 0.25), height + wbfont.Height);
            g.DrawString("Birth", wbfont, Brush, (int)(e.MarginBounds.Width * 0.40), height + wbfont.Height);
            g.DrawString("Veget-", wbfont, Brush, (int)(e.MarginBounds.Width * 0.55), height);
            g.DrawString("arian", wbfont, Brush, (int)(e.MarginBounds.Width * 0.55), height + wbfont.Height);
            g.DrawString("Allergy", wbfont, Brush, (int)(e.MarginBounds.Width * 0.65), height + wbfont.Height);
            g.DrawString("Paid", wbfont, Brush, (int)(e.MarginBounds.Width * 0.93), height + wbfont.Height);
            height += wbfont.Height * 2 + 2;
            //height += wfont.Height+2;

            g.DrawLine(pen, new Point(0, (int)height), new Point(e.MarginBounds.Width, (int)height));
            int maxRows = dataDoc.Count();
            height += 2;

            for (; m_lngPrintingRow < maxRows; ) 
            {
                System.Xml.Linq.XElement item = dataDoc.ElementAt(m_lngPrintingRow);
                
                g.DrawString(item.Element("Name").Value, wfont, Brush, 5, height);
                g.DrawString(item.Element("Section").Value, wfont, Brush, (int)(e.MarginBounds.Width * 0.25), (int)height);
                g.DrawString(item.Element("DateOfBirth").Value, wfont, Brush, (int)(e.MarginBounds.Width * 0.40), (int)height);
                g.DrawString(item.Element("Vegetarian").Value, wbfont, Brush, (int)(e.MarginBounds.Width * 0.55), (int)height);
                g.DrawString(item.Element("Allergy").Value, wfont, Brush, (int)(e.MarginBounds.Width * 0.65), (int)height);
                g.DrawString(item.Element("Paid").Value, wbfont, Brush, (int)(e.MarginBounds.Width * 0.93), (int)height);
                height += wbfont.Height;
                g.DrawLine(pen, new Point(0, (int)height), new Point(e.MarginBounds.Width, (int)height)); //Left line
                height += 2;

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
                }
            }
            height -= 2;
            g.DrawLine(pen, new Point(0, 0), new Point(0, (int)height)); //Left line
            g.DrawLine(pen, new Point(0, 0), new Point(e.MarginBounds.Width, 0)); //Top line
            g.DrawLine(pen, new Point(e.MarginBounds.Width, 0), new Point(e.MarginBounds.Width, (int)height)); //Right Line
            //g.DrawLine(pen, new Point(0, (int)height), new Point(e.MarginBounds.Width, (int)height)); //Bottom Line
            g.DrawLine(pen, new Point((int)(e.MarginBounds.Width * 0.25) - 3, 0), new Point((int)(e.MarginBounds.Width * 0.25) - 3, (int)height)); // column line 1-2
            g.DrawLine(pen, new Point((int)(e.MarginBounds.Width * 0.40) - 3, 0), new Point((int)(e.MarginBounds.Width * 0.40) - 3, (int)height)); // column line 2-3
            g.DrawLine(pen, new Point((int)(e.MarginBounds.Width * 0.55) - 3, 0), new Point((int)(e.MarginBounds.Width * 0.55) - 3, (int)height)); // column line 3-4
            g.DrawLine(pen, new Point((int)(e.MarginBounds.Width * 0.65) - 3, 0), new Point((int)(e.MarginBounds.Width * 0.65) - 3, (int)height)); // column line 4-5
            g.DrawLine(pen, new Point((int)(e.MarginBounds.Width * 0.93) - 3, 0), new Point((int)(e.MarginBounds.Width * 0.93) - 3, (int)height)); // column line 5-6
        }

        
    }//class
}//namcespace
