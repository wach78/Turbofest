using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace OpenGL
{
    /// <summary>
    /// This class will have all the effects names to be used
    /// </summary>
    class Effect
    {
        private object obj;
        private string name;
        private int norpm; //number of runs per/months
        private List<string> namelist;
        private List<int> runslist;
        private List<bool> runAllowedlist;
        private List<int> priolist;
        private List<bool> vetolist;

        /// <summary>
        /// Constructor of the Effect list
        /// </summary>
        /// <param name="obj">A effect object</param>
        /// <param name="data">Effect EventData</param>
        public Effect(object obj, UtilXML.EventData data)
        {
            // added error handling for events that are not here
            if (data == null)
            {
                throw new ArgumentNullException("Effect can not be created as there is no data for this object. This can be because this effect is missing from the file so there is no data to work with.");
            }
            this.name = data.Name;
            this.obj = obj;
            //this.noMoreRun = false;

           // this.veto = data.Veto;
            //this.prio = data.Prio;
            this.namelist = new List<string>(data.Namelist);
            this.runslist = new List<int>(data.Runslist);
            this.runAllowedlist = new List<bool>(data.RunAllowedlist);
            this.priolist = new List<int>(data.Priolist);
            this.vetolist = new List<bool>(data.VetoList);
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

        /// <summary>
        /// Number of runs per month
        /// </summary>
        public int Norpm
        {
            get { return norpm; }
            set { norpm = value; }
        }

        /// <summary>
        /// List of month names
        /// </summary>
        public List<string> Namelist
        {
            get { return namelist; }
        }

        /// <summary>
        /// Number of runs per month
        /// </summary>
        public List<int> Runslist
        {
            get { return runslist; }
        }

        /// <summary>
        /// Allowed to run on month
        /// </summary>
        public List<bool> RunAllowedlist
        {
            get { return runAllowedlist; }
        }

        /// <summary>
        /// Priority on month
        /// </summary>
        public List<int> Priolist
        {
            get { return priolist; }
        }

        /// <summary>
        /// Veto in a month or not
        /// </summary>
        public List<bool> Vetolist
        {
            get { return vetolist; }
        }
       
        /// <summary>
        /// ...
        /// </summary>
        public void init()
        {

        }
    }//class
}//namespace
