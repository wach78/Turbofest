using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projectX
{
  
    public partial class FrmEvents : Form
    {
        private ArrayList objlist;
        private XmlHandler xml;
        public FrmEvents()
        {
            InitializeComponent();
            string[] monthstlist;

            if ("Spring".Equals(FrmMain.SpringOrFall))
            {
               
                monthstlist = new string[] { "sep", "oct", "nov", "dec", "jan", "feb", "mar" };
            }
            else
            {
                monthstlist = new string[] { "mar", "apr", "maj", "jun", "jul", "aug", "sep" };
            }

          //  string[] eventlist = new string[] { "TS", "Hajk", "bumbi", "BB", "smurf", "creators", "sune", "dif", "fbk", "rms", "scrollers", "Q", "turbologo", "winlinux" };

            string[] eventlist = new string[] { "SuneAnimation", "Dif", "Fbk", "TurboLogo", "Datasmurf", "RMS", "WinLinux", "Scroller", "Self", "BB", "GummiBears", "Hajk", "TeknatStyle", "Matrix", "Quiz" };
            Array.Sort(eventlist);

            string path = XmlHandler.fixPath("randomeffects" + FrmMain.SpringOrFall +  ".xml","eff");
            string fileName = Path.GetFullPath(path);

            if (File.Exists(fileName))
            {
               xml = new XmlHandler("randomeffects" + FrmMain.SpringOrFall + ".xml", "XDoc", "eff");
            }
            else
            {
               xml = new XmlHandler("randomeffects" + FrmMain.SpringOrFall, "Create", "eff");
               xml.createDefoultXmlFileEffecs(eventlist, monthstlist);

               xml = new XmlHandler("randomeffects" + FrmMain.SpringOrFall + ".xml", "XDoc", "eff");
             
            }
        

            
            
            objlist = new ArrayList(xml.Loadeffectdata());
           
            cmbEvents.Items.AddRange(eventlist);
            cmbEvents.SelectedIndex = 0;
         
            if ("Spring".Equals(FrmMain.SpringOrFall))
            {
                cmbapr.Enabled = false;
                checkapr.Enabled = false;
                cmbmaj.Enabled = false;
                checkmaj.Enabled = false;
                cmbjun.Enabled = false;
                checkjun.Enabled = false;
                cmbjul.Enabled = false;
                checkjul.Enabled = false;
                cmbaug.Enabled = false;
                checkaug.Enabled = false;

            }
            else if ("Fall".Equals(FrmMain.SpringOrFall))
            {
                cmbjan.Enabled = false;
                checkjan.Enabled = false;
                cmbfeb.Enabled = false;
                checkfeb.Enabled = false;

                cmboct.Enabled = false;
                checkoct.Enabled = false;
                cmbnov.Enabled = false;
                checknov.Enabled = false;
                cmbdec.Enabled = false;
                checkdec.Enabled = false;
            }

            foreach (eventdata obj in objlist)
            {
                //???
            }
            
           
        }

        private void FrmEvents_Load(object sender, EventArgs e)
        {

        }

        private void Mfeb_Click(object sender, EventArgs e)
        {

        }

        
         
        
        private void cmbEvents_DropDown(object sender, EventArgs e)
        {
            eventdata ev = new eventdata(cmbEvents.Text, chbVeto.Checked, Int16.Parse(cmbPrio.Text));

            if ("Spring".Equals(FrmMain.SpringOrFall))
            {
                ev.setDataMonth("sep", Int16.Parse(cmbsep.Text), checksep.Checked);
                ev.setDataMonth("okt", Int16.Parse(cmboct.Text), checkoct.Checked);
                ev.setDataMonth("nov", Int16.Parse(cmbnov.Text), checknov.Checked);
                ev.setDataMonth("dec", Int16.Parse(cmbdec.Text), checkdec.Checked);
                ev.setDataMonth("jan", Int16.Parse(cmbjan.Text), checkjan.Checked);
                ev.setDataMonth("feb", Int16.Parse(cmbfeb.Text), checkfeb.Checked);
                ev.setDataMonth("mar", Int16.Parse(cmbmar.Text), checkmar.Checked);
            }
            else if ("Fall".Equals(FrmMain.SpringOrFall))
            {
                ev.setDataMonth("sep", Int16.Parse(cmbsep.Text), checksep.Checked);
                ev.setDataMonth("aug", Int16.Parse(cmbaug.Text), checkaug.Checked);
                ev.setDataMonth("jul", Int16.Parse(cmbjul.Text), checkjul.Checked);
                ev.setDataMonth("jun", Int16.Parse(cmbjun.Text), checkjun.Checked);
                ev.setDataMonth("maj", Int16.Parse(cmbmaj.Text), checkmaj.Checked);
                ev.setDataMonth("apr", Int16.Parse(cmbapr.Text), checkapr.Checked);
                ev.setDataMonth("mar", Int16.Parse(cmbmar.Text), checkmar.Checked);
            }

            int index = searchObj(objlist, cmbEvents.Text);
            if (index != -1)
                objlist.RemoveAt(index);

            objlist.Add(ev);
           
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            eventdata ev = new eventdata(cmbEvents.Text, chbVeto.Checked, Int16.Parse(cmbPrio.Text));

            if ("Spring".Equals(FrmMain.SpringOrFall))
            {
                ev.setDataMonth("sep", Int16.Parse(cmbsep.Text), checksep.Checked);
                ev.setDataMonth("okt", Int16.Parse(cmboct.Text), checkoct.Checked);
                ev.setDataMonth("nov", Int16.Parse(cmbnov.Text), checknov.Checked);
                ev.setDataMonth("dec", Int16.Parse(cmbdec.Text), checkdec.Checked);
                ev.setDataMonth("jan", Int16.Parse(cmbjan.Text), checkjan.Checked);
                ev.setDataMonth("feb", Int16.Parse(cmbfeb.Text), checkfeb.Checked);
                ev.setDataMonth("mar", Int16.Parse(cmbmar.Text), checkmar.Checked);
            }
            else if ("Fall".Equals(FrmMain.SpringOrFall))
            {
                ev.setDataMonth("sep", Int16.Parse(cmbsep.Text), checksep.Checked);
                ev.setDataMonth("aug", Int16.Parse(cmbaug.Text), checkaug.Checked);
                ev.setDataMonth("jul", Int16.Parse(cmbjul.Text), checkjul.Checked);
                ev.setDataMonth("jun", Int16.Parse(cmbjun.Text), checkjun.Checked);
                ev.setDataMonth("maj", Int16.Parse(cmbmaj.Text), checkmaj.Checked);
                ev.setDataMonth("apr", Int16.Parse(cmbapr.Text), checkapr.Checked);
                ev.setDataMonth("mar", Int16.Parse(cmbmar.Text), checkmar.Checked);
            }

            int index = searchObj(objlist, cmbEvents.Text);
            if (index != -1)
                objlist.RemoveAt(index);

            objlist.Add(ev);

            objlist.Sort();
            XmlHandler xml = new XmlHandler("randomeffects" + FrmMain.SpringOrFall, "Create", "eff");
            xml.createXmlFileEffecs(objlist);
            this.Close();
        }

        public int searchObj(ArrayList list, string searchString)
        {
            int index = -1;
            int n = 0;
            foreach (eventdata obj in list)
            {
                if (obj.Name == searchString)
                {
                    index = n;
                    break;
                }
                n++;
            }

            return index;
        }

        private void cmbEvents_SelectedValueChanged(object sender, EventArgs e)
        {
            int index = searchObj(objlist, cmbEvents.Text);
            eventdata obj = new eventdata();
            if (index != -1)
            {
                obj = (eventdata)objlist[index];
                chbVeto.Checked = obj.Veto;
                cmbPrio.Text = obj.Prio.ToString();

                ArrayList ckeckBoxList = new ArrayList();
                ArrayList comboBoxList = new ArrayList();
                foreach (var control in this.Controls)
                {
                    if (control is CheckBox)
                    {
                        ckeckBoxList.Add(((CheckBox)control));
                        
                    }
                    else if (control is ComboBox)
                    {
                        comboBoxList.Add(((ComboBox)control)); 
                    }
                }

                int len = comboBoxList.Count;
                int namelen = obj.Namelist.Count;
               

                ComboBox cmb;
                for (int i = 0; i < len; i++)
                {
                    cmb = (ComboBox)comboBoxList[i];
                    for (int j = 0; j < namelen; j++)
                    {
                        if (cmb.Name == ("cmb" +obj.Namelist[j]))
                        {
                            cmb.Text = obj.Runslist[j].ToString();
                            break;
                        }
                    }
                }
                CheckBox ckb;
                int len2 = ckeckBoxList.Count;
                for (int i = 0; i < len2; i++)
                {
                    ckb = (CheckBox)ckeckBoxList[i];
                    for (int j = 0; j < namelen; j++)
                    {
                        if (ckb.Name == ("check" + obj.Namelist[j]))
                        {
                            ckb.Checked = (bool)obj.RunAllowedlist[j];
                            break;
                        }
                    }
                }
                
            }
        }
    }//class
    public class eventdata : IComparable
    {
        private String name;
        private bool veto;
        private int prio;
        private ArrayList namelist;
        private ArrayList runslist;
        private ArrayList runAllowedlist;

        public eventdata(string name, bool veto, int prio)
        {
            namelist = new ArrayList();
            runslist = new ArrayList();
            runAllowedlist = new ArrayList();

            this.name = name;
            this.veto = veto;
            this.prio = prio;
        }
         public eventdata()
        {
            namelist = new ArrayList();
            runslist = new ArrayList();
            runAllowedlist = new ArrayList();
        }

        public string Name
        {
            get { return name; }
        }

        public bool Veto
        {
            get { return veto; }
        }

        public int Prio
        {
            get { return prio; }
        }
        public ArrayList Namelist
        {
            get { return namelist; }
        }
        public ArrayList Runslist
        {
            get { return runslist; }
        }
        public ArrayList RunAllowedlist
        {
            get { return runAllowedlist; }
        }

        public void setDataMonth(string name,int runs,bool allowed)
        {
            namelist.Add(name);
            runslist.Add(runs);
            runAllowedlist.Add(allowed);
        }

        public int CompareTo(object obj)
        {
            eventdata Compare = (eventdata)obj;
            int result = this.name.CompareTo(Compare.name);
            if (result == 0)
                result = this.name.CompareTo(Compare.name);
            return result;
        }
       
    }//class
}//namespace
