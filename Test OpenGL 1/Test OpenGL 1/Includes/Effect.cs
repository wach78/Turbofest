using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace OpenGL
{
    class Effect
    {
        private Sound snd;
        private Text2D text;
        private Chess chess;

        private object obj;
        private string name;
        private bool veto;
        private bool noMoreRun;
        private int prio;
        private int norpm; //number of runs per/months

        private List<string> namelist;
        private List<int> runslist;
        private List<bool> runAllowedlist;

        public Effect(object obj, UtilXML.EventData data)
        {
            this.name = data.Name;
            this.obj = obj;
            this.noMoreRun = false;

            this.veto = data.Veto;
            this.prio = data.Prio;
            this.namelist = new List<string>(data.Namelist);
            this.runslist = new List<int>(data.Runslist);
            this.runAllowedlist = new List<bool>(data.RunAllowedlist);
        }

       
        public bool Veto
        {
            get { return veto; }
        }

        public int Prio
        {
            get { return prio; }
        }

        public int Norpm
        {
            get { return norpm; }
            set { norpm = value; }
        }

        public List<string> Namelist
        {
            get { return namelist; }
        }

        public List<int> Runslist
        {
            get { return runslist; }
        }

        public List<bool> RunAllowedlist
        {
            get { return runAllowedlist; }
        }
       

        public void init()
        {

        }
    }//class
}//namespace
