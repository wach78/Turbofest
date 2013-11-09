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
        private string[] runInThisMonths;

        public Effect(string name,object obj,ref Sound sound, ref Text2D txt, ref Chess chess)
        {
            this.name = name;
            this.obj = obj;
            this.snd = sound;
            this.text = txt;
            this.chess = chess;
            this.noMoreRun = false;
        }
        public bool Veto
        {
            get { return veto; }
            set { veto = value; }
        }

        public int Prio
        {
            get { return prio; }
            set { prio = value; }
        }

        public int Norpm
        {
            get { return norpm; }
            set { norpm = value; }
        }

        public string[] RunInThisMonths
        {
            get { return runInThisMonths; }
            set { runInThisMonths = value; }
        }

        public void init()
        {

        }
    }//class
}//namespace
