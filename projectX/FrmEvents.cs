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

                cmbPrioApr.Enabled = false;
                checkVetoApr.Enabled = false;
                cmbPrioMaj.Enabled = false;
                checkVetoMaj.Enabled = false;
                cmbPrioJun.Enabled = false;
                checkVetoJun.Enabled = false;
                cmbPrioJul.Enabled = false;
                checkVetoJul.Enabled = false;
                cmbPrioAug.Enabled = false;
                checkVetoAug.Enabled = false;

            }
            else if ("Fall".Equals(FrmMain.SpringOrFall))
            {
                cmbjan.Enabled = false;
                checkjan.Enabled = false;
                cmbfeb.Enabled = false;
                checkfeb.Enabled = false;

                cmbokt.Enabled = false;
                checkokt.Enabled = false;
                cmbnov.Enabled = false;
                checknov.Enabled = false;
                cmbdec.Enabled = false;
                checkdec.Enabled = false;

                cmbPrioJan.Enabled = false;
                checkVetoJan.Enabled = false;
                cmbPrioFeb.Enabled = false;
                checkVetoFeb.Enabled = false;

                cmbPrioOkt.Enabled = false;
                checkVetoOkt.Enabled = false;
                cmbPrioNov.Enabled = false;
                checkVetoNov.Enabled = false;
                cmbPrioDec.Enabled = false;
                checkVetoDec.Enabled = false;
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
                ev.setData("sep", Int16.Parse(cmbsep.Text), checksep.Checked, checkVetoSep.Checked, Int16.Parse(cmbPrioSep.Text));
                ev.setData("okt", Int16.Parse(cmbokt.Text), checkokt.Checked, checkVetoOkt.Checked, Int16.Parse(cmbPrioOkt.Text));
                ev.setData("nov", Int16.Parse(cmbnov.Text), checknov.Checked, checkVetoNov.Checked, Int16.Parse(cmbPrioNov.Text));
                ev.setData("dec", Int16.Parse(cmbdec.Text), checkdec.Checked, checkVetoDec.Checked, Int16.Parse(cmbPrioDec.Text));
                ev.setData("jan", Int16.Parse(cmbjan.Text), checkjan.Checked, checkVetoJan.Checked, Int16.Parse(cmbPrioJan.Text));
                ev.setData("feb", Int16.Parse(cmbfeb.Text), checkfeb.Checked, checkVetoFeb.Checked, Int16.Parse(cmbPrioFeb.Text));
                ev.setData("mar", Int16.Parse(cmbmar.Text), checkmar.Checked, checkVetoMar.Checked, Int16.Parse(cmbPrioMar.Text));
            }
            else if ("Fall".Equals(FrmMain.SpringOrFall))
            {
                ev.setData("sep", Int16.Parse(cmbsep.Text), checksep.Checked, checkVetoSep.Checked, Int16.Parse(cmbPrioSep.Text));
                ev.setData("aug", Int16.Parse(cmbaug.Text), checkaug.Checked, checkVetoAug.Checked, Int16.Parse(cmbPrioAug.Text));
                ev.setData("jul", Int16.Parse(cmbjul.Text), checkjul.Checked, checkVetoJul.Checked, Int16.Parse(cmbPrioJul.Text));
                ev.setData("jun", Int16.Parse(cmbjun.Text), checkjun.Checked, checkVetoJun.Checked, Int16.Parse(cmbPrioJun.Text));
                ev.setData("maj", Int16.Parse(cmbmaj.Text), checkmaj.Checked, checkVetoMaj.Checked, Int16.Parse(cmbPrioMaj.Text));
                ev.setData("apr", Int16.Parse(cmbapr.Text), checkapr.Checked, checkVetoApr.Checked, Int16.Parse(cmbPrioApr.Text));
                ev.setData("mar", Int16.Parse(cmbmar.Text), checkmar.Checked, checkVetoMar.Checked, Int16.Parse(cmbPrioMar.Text));
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
                ev.setData("sep", Int16.Parse(cmbsep.Text), checksep.Checked, checkVetoSep.Checked, Int16.Parse(cmbPrioSep.Text));
                ev.setData("okt", Int16.Parse(cmbokt.Text), checkokt.Checked, checkVetoOkt.Checked, Int16.Parse(cmbPrioOkt.Text));
                ev.setData("nov", Int16.Parse(cmbnov.Text), checknov.Checked, checkVetoNov.Checked, Int16.Parse(cmbPrioNov.Text));
                ev.setData("dec", Int16.Parse(cmbdec.Text), checkdec.Checked, checkVetoDec.Checked, Int16.Parse(cmbPrioDec.Text));
                ev.setData("jan", Int16.Parse(cmbjan.Text), checkjan.Checked, checkVetoJan.Checked, Int16.Parse(cmbPrioJan.Text));
                ev.setData("feb", Int16.Parse(cmbfeb.Text), checkfeb.Checked, checkVetoFeb.Checked, Int16.Parse(cmbPrioFeb.Text));
                ev.setData("mar", Int16.Parse(cmbmar.Text), checkmar.Checked, checkVetoMar.Checked, Int16.Parse(cmbPrioMar.Text));
            }
            else if ("Fall".Equals(FrmMain.SpringOrFall))
            {
                ev.setData("sep", Int16.Parse(cmbsep.Text), checksep.Checked, checkVetoSep.Checked, Int16.Parse(cmbPrioSep.Text));
                ev.setData("aug", Int16.Parse(cmbaug.Text), checkaug.Checked, checkVetoAug.Checked, Int16.Parse(cmbPrioAug.Text));
                ev.setData("jul", Int16.Parse(cmbjul.Text), checkjul.Checked, checkVetoJul.Checked, Int16.Parse(cmbPrioJul.Text));
                ev.setData("jun", Int16.Parse(cmbjun.Text), checkjun.Checked, checkVetoJun.Checked, Int16.Parse(cmbPrioJun.Text));
                ev.setData("maj", Int16.Parse(cmbmaj.Text), checkmaj.Checked, checkVetoMaj.Checked, Int16.Parse(cmbPrioMaj.Text));
                ev.setData("apr", Int16.Parse(cmbapr.Text), checkapr.Checked, checkVetoApr.Checked, Int16.Parse(cmbPrioApr.Text));
                ev.setData("mar", Int16.Parse(cmbmar.Text), checkmar.Checked, checkVetoMar.Checked, Int16.Parse(cmbPrioMar.Text));
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

                string strtmp;
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
