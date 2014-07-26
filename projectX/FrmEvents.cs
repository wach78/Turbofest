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
               
                monthstlist = new string[] { "sep", "okt", "nov", "dec", "jan", "feb", "mar" };
            }
            else
            {
                monthstlist = new string[] { "mar", "apr", "maj", "jun", "jul", "aug", "sep" };
            }

          //  string[] eventlist = new string[] { "TS", "Hajk", "bumbi", "BB", "smurf", "creators", "sune", "dif", "fbk", "rms", "scrollers", "Q", "turbologo", "winlinux" };

            string[] eventlist = new string[] { "SuneAnimation", "Dif", "Fbk", "TurboLogo", "Datasmurf", "RMS", "WinLinux", "Scroller", "Self", "BB", "GummiBears", "Hajk", "TeknatStyle", "Matrix", "Quiz", "Talespin", "ChipDale", "Nerdy", "Trex", "Sailormoon", "GhostBusters", "Zelda","Tardis","Fuck","SilverFang", "MoraT" };
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
             
                checkapr.Enabled = false;
                checkmaj.Enabled = false;
                checkjun.Enabled = false;
                checkjul.Enabled = false;
                checkaug.Enabled = false;

                cmbPrioapr.Enabled = false;
                checkVetoapr.Enabled = false;
                cmbPriomaj.Enabled = false;
                checkVetomaj.Enabled = false;
                cmbPriojun.Enabled = false;
                checkVetojun.Enabled = false;
                cmbPriojul.Enabled = false;
                checkVetojul.Enabled = false;
                cmbPrioaug.Enabled = false;
                checkVetoaug.Enabled = false;

            }
            else if ("Fall".Equals(FrmMain.SpringOrFall))
            {
               
                checkjan.Enabled = false;
                checkfeb.Enabled = false;
                checkokt.Enabled = false;
                checknov.Enabled = false;
                checkdec.Enabled = false;

                cmbPriojan.Enabled = false;
                checkVetojan.Enabled = false;
                cmbPriofeb.Enabled = false;
                checkVetofeb.Enabled = false;

                cmbPriookt.Enabled = false;
                checkVetookt.Enabled = false;
                cmbPrionov.Enabled = false;
                checkVetonov.Enabled = false;
                cmbPriodec.Enabled = false;
                checkVetodec.Enabled = false;
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
            eventdata ev = new eventdata(cmbEvents.Text);

            if ("Spring".Equals(FrmMain.SpringOrFall))
            {
                ev.setData("sep", 1, checksep.Checked, checkVetosep.Checked, Int16.Parse(cmbPriosep.Text));
                ev.setData("okt", 1, checkokt.Checked, checkVetookt.Checked, Int16.Parse(cmbPriookt.Text));
                ev.setData("nov", 1, checknov.Checked, checkVetonov.Checked, Int16.Parse(cmbPrionov.Text));
                ev.setData("dec", 1, checkdec.Checked, checkVetodec.Checked, Int16.Parse(cmbPriodec.Text));
                ev.setData("jan", 1, checkjan.Checked, checkVetojan.Checked, Int16.Parse(cmbPriojan.Text));
                ev.setData("feb", 1, checkfeb.Checked, checkVetofeb.Checked, Int16.Parse(cmbPriofeb.Text));
                ev.setData("mar", 1, checkmar.Checked, checkVetomar.Checked, Int16.Parse(cmbPriomar.Text));
            }
            else if ("Fall".Equals(FrmMain.SpringOrFall))
            {
                ev.setData("sep", 1, checksep.Checked, checkVetosep.Checked, Int16.Parse(cmbPriosep.Text));
                ev.setData("aug", 1, checkaug.Checked, checkVetoaug.Checked, Int16.Parse(cmbPrioaug.Text));
                ev.setData("jul", 1, checkjul.Checked, checkVetojul.Checked, Int16.Parse(cmbPriojul.Text));
                ev.setData("jun", 1, checkjun.Checked, checkVetojun.Checked, Int16.Parse(cmbPriojun.Text));
                ev.setData("maj", 1, checkmaj.Checked, checkVetomaj.Checked, Int16.Parse(cmbPriomaj.Text));
                ev.setData("apr", 1, checkapr.Checked, checkVetoapr.Checked, Int16.Parse(cmbPrioapr.Text));
                ev.setData("mar", 1, checkmar.Checked, checkVetomar.Checked, Int16.Parse(cmbPriomar.Text));
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
            eventdata ev = new eventdata(cmbEvents.Text);

            if ("Spring".Equals(FrmMain.SpringOrFall))
            {
                ev.setData("sep", 1, checksep.Checked, checkVetosep.Checked, Int16.Parse(cmbPriosep.Text));
                ev.setData("okt", 1, checkokt.Checked, checkVetookt.Checked, Int16.Parse(cmbPriookt.Text));
                ev.setData("nov", 1, checknov.Checked, checkVetonov.Checked, Int16.Parse(cmbPrionov.Text));
                ev.setData("dec", 1, checkdec.Checked, checkVetodec.Checked, Int16.Parse(cmbPriodec.Text));
                ev.setData("jan", 1, checkjan.Checked, checkVetojan.Checked, Int16.Parse(cmbPriojan.Text));
                ev.setData("feb", 1, checkfeb.Checked, checkVetofeb.Checked, Int16.Parse(cmbPriofeb.Text));
                ev.setData("mar", 1, checkmar.Checked, checkVetomar.Checked, Int16.Parse(cmbPriomar.Text));
            }
            else if ("Fall".Equals(FrmMain.SpringOrFall))
            {
                ev.setData("sep", 1, checksep.Checked, checkVetosep.Checked, Int16.Parse(cmbPriosep.Text));
                ev.setData("aug", 1, checkaug.Checked, checkVetoaug.Checked, Int16.Parse(cmbPrioaug.Text));
                ev.setData("jul", 1, checkjul.Checked, checkVetojul.Checked, Int16.Parse(cmbPriojul.Text));
                ev.setData("jun", 1, checkjun.Checked, checkVetojun.Checked, Int16.Parse(cmbPriojun.Text));
                ev.setData("maj", 1, checkmaj.Checked, checkVetomaj.Checked, Int16.Parse(cmbPriomaj.Text));
                ev.setData("apr", 1, checkapr.Checked, checkVetoapr.Checked, Int16.Parse(cmbPrioapr.Text));
                ev.setData("mar", 1, checkmar.Checked, checkVetomar.Checked, Int16.Parse(cmbPriomar.Text));
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
                //chbVeto.Checked = obj.Veto;
                //cmbPrio.Text = obj.Prio.ToString();

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
                        else if (cmb.Name == ("cmbPrio" + obj.Namelist[j]))
                        {
                            cmb.Text = obj.Priolist[j].ToString();
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
                        else if (ckb.Name == ("checkVeto" + obj.Namelist[j]))
                        {
                            ckb.Checked = (bool)obj.Vetolist[j];
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
        //private bool veto;
       // private int prio;
        private ArrayList namelist;
        private ArrayList runslist;
        private ArrayList runAllowedlist;
        private ArrayList priolist;
        private ArrayList vetolist;

        public eventdata(string name)
        {
            namelist = new ArrayList();
            runslist = new ArrayList();
            runAllowedlist = new ArrayList();
            priolist = new ArrayList();
            vetolist = new ArrayList();

            this.name = name;
            //this.veto = veto;
           // this.prio = prio;
        }
        public eventdata()
        {
            namelist = new ArrayList();
            runslist = new ArrayList();
            runAllowedlist = new ArrayList();
            priolist = new ArrayList();
            vetolist = new ArrayList();
        }

        public string Name
        {
            get { return name; }
        }

        /*
        public bool Veto
        {
            get { return veto; }
        }

        public int Prio
        {
            get { return prio; }
        }
         */

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

        public ArrayList Priolist
        {
            get { return priolist; }
        }
        public ArrayList Vetolist
        {
            get { return vetolist; }
        }
        /*
        public void setDataMonth(string name,int runs,bool allowed)
        {
            namelist.Add(name);
            runslist.Add(runs);
            runAllowedlist.Add(allowed);
        }
         * */

        public void setData(string name, int runs, bool allowed, bool veto, int prio)
        {
            namelist.Add(name);
            runslist.Add(runs);
            runAllowedlist.Add(allowed);
            priolist.Add(prio);
            vetolist.Add(veto);
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
