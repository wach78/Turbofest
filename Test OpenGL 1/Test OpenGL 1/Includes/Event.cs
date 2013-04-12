﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.Event
{
    enum EventType {Effect, Text, Random};

    interface IEventItem //: IDisposable
    {
        void Draw(string Date);
    }

    /*class Advent : IEventItem
    {
        private OpenGL.Advent item;
        private OpenGL.Advent.WhatAdvent CurrentAdvent;

        public Advent(ref OpenGL.Sound sound)
        {
            item = new OpenGL.Advent(ref sound);
        }

        public void Draw(string Date)
        {
            item.Draw(Date, CurrentAdvent);
        }

        public OpenGL.Advent.WhatAdvent Current
        {
            get { return CurrentAdvent; }
            set { CurrentAdvent = value; }
        }

    }

    class Birthday : IEventItem
    {
        private OpenGL.Birthday item;
        private string Name;

        public Birthday(ref OpenGL.Sound sound, ref OpenGL.Text2D text, ref OpenGL.Chess chess)
        {
            item = new OpenGL.Birthday(ref sound, ref text, ref chess);
        }

        public void Draw(string Date)
        {
            item.Draw(Date); // fixa ett namn in här
        }

        public string Current
        {
            get { return Name; }
            set { Name = value; }
        }
    }

    class RMS : IEventItem
    {
        private OpenGL.RMS item;

        public RMS(ref OpenGL.Sound sound, ref OpenGL.Text2D text)
        {
            item = new OpenGL.RMS(ref sound, ref text);
        }

        public void Draw(string Date)
        {
            item.Draw(Date); // fixa ett namn in här
        }
    }*/

    class EventItem
    {
        private string mName;
        private string mType;
        private string mDate;

        public EventItem(string Name, string Type, string Date)
        {
            mName = Name;
            mType = Type;
            mDate = Date;
        }

        public string Name
        {
            get { return mName; }
            set { }
        }

        public string Type
        {
            get { return mType; }
            set { }
        }

        public string Date
        {
            get { return mDate; }
            set { }
        }
    }

    class Event
    {
        private bool disposed = false;
        // Time
        OpenGL.PartyClock clock;
        // Effect needed effects
        OpenGL.Sound sound;
        OpenGL.Text2D text;
        OpenGL.Chess chess;
        OpenGL.Starfield sf;


        //Effects
        SuneAnimation sune;
        Dif dif;
        Fbk fbk;
        Christmas xmas;
        Semla semla;
        TurboLogo tl;
        Datasmurf smurf;
        Halloween hw;
        Valentine valentine;
        Outro outro;
        Intro intro;
        Birthday birthday;
        RMS richard;
        WinLinux wl;
        Lucia lucia;
        Advent advent;

        //Event Date list
        System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<EventItem>> events;
        string lastDate;
        string nowDate;


        public Event(DateTime ClockStart, DateTime ClockEnd, int ClockRunTime, System.Xml.Linq.XDocument XMLEvents)
        {
            events = new Dictionary<string, List<EventItem>>();
            clock = new PartyClock(ClockStart, ClockEnd, ClockRunTime);
            Util.ShowClock = true;
            sound = new Sound(true);
            text = new Text2D();
            chess = new Chess();
            sf = new Starfield(150);

            intro = new Intro(ref sound, ref text);
            outro = new Outro(ref sound);

            advent = new Advent(ref sound);
            birthday = new Birthday(ref sound, ref text, ref chess);
            richard = new RMS(ref sound, ref text);
            hw = new Halloween(ref chess, 25);

            string name, date, type;
            // Event dates setup
            foreach (var item in XMLEvents.Descendants("event"))
            {
                name = item.Element("name").Value;
                date = item.Element("date").Value;
                type = item.Element("type").Value.ToLower();
                EventItem ei = new EventItem(name, type, date);
                if (!events.ContainsKey(date))
                { 
                    List<EventItem> list = new List<EventItem>(); // seems most bad in my eyes...
                    list.Add(ei);
                    events.Add(date, list);
                }
                else
                {
                    events[date].Add(ei);
                }
                name = date = type = string.Empty;
            }
            
        }

        #region Dispose
        ~Event()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            //base.Finalize();
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // free managed resources
                    if (sound != null) sound.Dispose();

                    if (clock != null) clock.Dispose(); // 2 texturer
                    if (sune != null) sune.Dispose(); // 1-2 texturer
                    if (fbk != null) fbk.Dispose(); // 1 textur
                    if (dif != null) dif.Dispose(); // 1 textur
                    if (xmas != null) xmas.Dispose(); // 3 texturer
                    if (semla != null) semla.Dispose(); // 1 textur
                    if (tl != null) tl.Dispose(); // 1 textur
                    if (smurf != null) smurf.Dispose(); // 1 textur
                    if (hw != null) hw.Dispose(); // 1 textur
                    if (valentine != null) valentine.Dispose(); // 2 texturer
                    if (outro != null) outro.Dispose(); // 1 textur
                    if (intro != null) intro.Dispose(); // 1 textur
                    if (birthday != null) birthday.Dispose(); // 1 textur
                    if (richard != null) richard.Dispose(); // 1 textur
                    if (wl != null) wl.Dispose(); // 1 textur
                    if (lucia != null) lucia.Dispose(); // 1 textur
                    if (advent != null) advent.Dispose(); // 1 textur

                    if (sf != null) sf.Dispose(); // 0 texturer
                    if (text != null) text.Dispose(); // 9 texturer
                    if (chess != null) chess.Dispose(); // 7 texturer
                }
                // free native resources if there are any.
                disposed = true;
                System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
            }
        }
        #endregion

        public void Draw()
        {
            if (!clock.EndOfRuntime())
            {
                clock.updateClock();
            }
            else 
            {
                Util.ShowClock = false;
            }

            // Draw effects and events here
            nowDate = clock.CurrentClock().ToShortDateString();
            if (nowDate != lastDate)
            {
                if (lastDate != string.Empty)
                {
                    sound.StopSound();
                }
                lastDate = nowDate;
                //sune.NewQoute();
            }

            if (Util.ShowClock)
            {
                clock.Draw();
            }


            EventItem ei = null;
            if (events.ContainsKey(nowDate))
            {
                ei = events[nowDate][0];// make this so that we can use more then one...
            }

            if (ei != null)
            {
                switch (ei.Type)
                {
                    case "effect":
                        switch (ei.Name)
                        {
                            case "Halloween":
                                hw.Draw(nowDate);
                                break;
                            default:
                                Console.WriteLine("No Effect");
                                break;
                        }
                        break;
                    case "birthday":
                        birthday.Draw(nowDate); // fix in name...
                        break;
                    case "text":
                        text.Draw(ei.Name, Text2D.FontName.Coolfont, new OpenTK.Vector3(1.0f, 0.0f, 0.4f), new OpenTK.Vector2(0.1f, 0.1f), new OpenTK.Vector2()); // fix in name...
                        break;
                    default:
                        Console.WriteLine("No Event");
                        break;
                }
                
            }

        }
    }
}
