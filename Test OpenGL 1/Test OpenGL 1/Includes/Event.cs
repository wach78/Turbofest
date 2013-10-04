using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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
            set { mName = value; }
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
        CrashHandler ch;
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
        NewYear newyear;
        Scroller scroller;
        Self creators;
        BB bb;
        GummiBears GM;
        National NDay;
        Easter easter;
        Hajk hajk;
        midsummer mid;
        Vaffla vaf;
        Walpurgis wp;
        CrayFish crayfish;
        TeknatStyle ts;
        Matrix m;
        Quiz q;

        private bool star;

        //Event Date list
        System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<EventItem>> events;
        System.Collections.Generic.List<string> randomEvent;
        string lastDate;
        string nowDate;

        public Event(DateTime ClockStart, DateTime ClockEnd, int ClockRunTime, System.Xml.Linq.XDocument XMLEvents, ref CrashHandler Crash)
        {
            ch = Crash;
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
            xmas = new Christmas(ref sound);
            smurf = new Datasmurf(ref sound, ref text); // random
            dif = new Dif(ref chess, ref sound); // random
            fbk = new Fbk(ref sound); // random
            hw = new Halloween(ref chess, ref sound, 25);
            lucia = new Lucia(ref chess, ref sound);
            newyear = new NewYear();
            richard = new RMS(ref sound, ref text); // random
            scroller = new Scroller(ref chess, ref sf, ref text); // random
            semla = new Semla();
            sune = new SuneAnimation(ref sound, ref text);
            tl = new TurboLogo(ref sound, ref chess, ((ClockStart.Month >= 1 && ClockStart.Month <= 8)? false:true) ); // vilken termin är det? jan till början av augusti VT, resten HT... random
            valentine = new Valentine(ref sound);
            wl = new WinLinux(ref chess); //random
            creators = new Self(ref sound); // random
            bb = new BB(ref sound); // random
            GM = new GummiBears(ref sound);
            NDay = new National(ref chess, ref sound);
            easter = new Easter(ref sound);
            hajk = new Hajk(ref sound);
            mid = new midsummer(ref sound);
            vaf = new Vaffla();
            wp = new Walpurgis();
            crayfish = new CrayFish();
            ts = new TeknatStyle(ref chess, ref sound, ref text);
            m = new Matrix(ref text);
            q = new Quiz(ref text, false);

            star = false;
            randomEvent = new List<string>(new string[] {"starfield", "TS", "Hajk","bumbi", "BB", "", "smurf","", "sune","dif","", "sune","", "dif", "","fbk", "","rms", "scrollers", "sune", "scrollers", "","turbologo","winlinux", "bumbi","creators"});


            if (ch.CrashDialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                clock.clock = ch.CrashClock;
            }

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
                    events.Add(date, list);
                }

                for (int i = 0; i < events[date].Count; i++)
                {
                    EventItem e = events[date][i];
                    if ("birthday".Equals(e.Type) && "birthday".Equals(ei.Type))
                    {
                        e.Name += "\n\n" + ei.Name;
                        events[date][i] = e;
                    }

                }
                events[date].Add(ei);
              
                name = date = type = string.Empty;
            }
            
            
            // Random effects on dates with no effects.
            DateTime dt = ClockStart;
            while (dt <= ClockEnd)
            {
                date = dt.ToShortDateString();
                if (!events.ContainsKey(date))
                {
                    EventItem ei;

                    if (star)
                    {
                        ei = new EventItem(randomEvent [0], "random", date);
                        star = false;
                    }
                    else
                    {
                        ei = new EventItem(randomEvent[Util.Rnd.Next(1, randomEvent.Count)], "random", date);
                        star = true;
                    }

                   
                    List<EventItem> list = new List<EventItem>(); // seems most bad in my eyes...
                    events.Add(date, list);
                    events[date].Add(ei);
                }
                dt = dt.AddDays(1);
                date = string.Empty;
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
                    if (sune != null) sune.Dispose(); // 1 textur
                    if (hw != null) hw.Dispose(); // 1 textur
                    if (valentine != null) valentine.Dispose(); // 2 texturer
                    if (outro != null) outro.Dispose(); // 1 textur
                    if (intro != null) intro.Dispose(); // 1 textur
                    if (birthday != null) birthday.Dispose(); // 1 textur
                    if (richard != null) richard.Dispose(); // 1 textur
                    if (wl != null) wl.Dispose(); // 1 textur
                    if (lucia != null) lucia.Dispose(); // 1 textur
                    if (advent != null) advent.Dispose(); // 1 textur
                    if (creators != null) creators.Dispose(); // 2 texturer
                    if (newyear != null) newyear.Dispose(); // 1 textur
                    if (scroller != null) scroller.Dispose(); //
                    if (bb != null) bb.Dispose(); // 1 textur
                    if (GM != null) GM.Dispose(); // 7 texturer
                    if (NDay != null) NDay.Dispose(); // 1 textur
                    if (easter != null) easter.Dispose(); // 3 texturer
                    if (hajk != null) hajk.Dispose(); // 1 textur
                    if (mid != null) mid.Dispose(); // 2 texturer
                    if (vaf != null) vaf.Dispose(); // 1 textur
                    if (wp != null) wp.Dispose(); // 1 texturer
                    if (crayfish != null) crayfish.Dispose(); // 8 texturer
                    if (ts != null) ts.Dispose(); // 3 textur
                    if (m != null) m.Dispose(); 
                    // Main effects
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

        public void StopSound()
        {
            sound.StopThread();
        }

        public void Draw()
        {

            

            if (!clock.EndOfRuntime())
            {
                clock.updateClock();
                if (Util.ShowClock)
                {
                    clock.Draw();
                }
            }
            if (clock.EndOfRuntime()) //else //<runtime>13737632</runtime>
            {
                if (Util.ShowClock)
                {
                    System.Diagnostics.Debug.WriteLine("Outro stop sound");
                    sound.StopSound();
                }
                Util.ShowClock = false;
                outro.Draw(nowDate);
                return;
            }

            // Draw effects and events here
            nowDate = clock.CurrentClock().ToShortDateString();
            if (nowDate != lastDate)
            {
                ch.update(clock.clock, clock.CurrentClock());
                
                /*if (sound.PlayingName() != string.Empty)
                {
                    sound.StopSound();
                }*/
                
                sound.StopSound();
                lastDate = nowDate;
                //System.Diagnostics.Debug.WriteLine(nowDate);
                //sune.NewQoute(); // flytta in detta i sune...
                //scroller.getRandomScrollerStuff(); // flytta in detta i scroller
            }

            EventItem ei = null;
            if (events.ContainsKey(nowDate))
            {
               // make this so that we can use more then one...

                if (events[nowDate].Count > 1)
                {
                    ei = events[nowDate][0];
                    for (int i = 0; i < events[nowDate].Count; i++)
                    {
                        if ("effect".Equals(events[nowDate][i].Type))
                        {
                            ei = events[nowDate][i];
                            break;
                        }
                    }
                }
                else
                {
                    ei = events[nowDate][0];
                }
            }

            if (ei != null)
            {
                switch (ei.Type)
                {
                    case "effect":
                        switch (ei.Name)
                        {
                            case "Advent 1":
                                advent.Draw(nowDate, Advent.WhatAdvent.First);
                                break;
                            case "Advent 2":
                                advent.Draw(nowDate, Advent.WhatAdvent.Second);
                                break;
                            case "Advent 3":
                                advent.Draw(nowDate, Advent.WhatAdvent.Third);
                                break;
                            case "Advent 4":
                                advent.Draw(nowDate, Advent.WhatAdvent.Fourth);
                                break;
                            case "JulAfton":
                                xmas.Draw(nowDate);
                                break;
                            case "Halloween":
                                hw.Draw(nowDate);
                                break;
                            case "Lucia":
                                lucia.Draw(nowDate);
                                break;
                            case "Nyårsafton":
                                newyear.Draw(nowDate);
                                break;
                            case "Semla":
                                semla.Draw(nowDate);
                                break;
                            case "Valentine":
                                valentine.Draw(nowDate);
                                break;

                            case "Nationaldagen":
                                NDay.Draw(nowDate);
                                break;

                            case "påsk":
                                easter.Draw(nowDate);
                                break;

                            case "Midsommar":
                                mid.Draw(nowDate);
                                break;

                            case "Våffeldagen":
                                vaf.Draw(nowDate);
                                break;

                            case "Valborgsmässoafton":
                                wp.Draw(nowDate);
                                break;
                            case "Krafta":
                                crayfish.Draw(nowDate);
                                break;
                            default:
                                if (nowDate != lastDate)
                                {
                                    Debug.WriteLine("No effect");
                                }
                                break;
                        }
                        break;
                    case "random":
                        switch (ei.Name)
                        {
                            case "smurf":
                                smurf.Draw(nowDate);
                                break;
                            case "dif":
                                if (difFbk(nowDate))
                                    dif.Draw(nowDate);
                                else
                                    q.Draw(nowDate);
                                break;
                            case "fbk":
                                if (difFbk(nowDate))
                                    fbk.Draw(nowDate);
                                else
                                    q.Draw(nowDate);
                                break;
                            case "rms":
                                richard.Draw(nowDate);
                                break;
                            case "scrollers":
                                scroller.Draw(nowDate);
                                break;
                            case "sune":
                                sune.Draw(nowDate);
                                break;
                            case "turbologo":
                                tl.Draw(nowDate);
                                break;
                            case "winlinux":
                                wl.Draw(nowDate);
                                break;
                            case "creators":
                                creators.Draw(nowDate);
                                break;
                            case "BB":
                                bb.Draw(nowDate);
                                break;
                            case "bumbi":
                                GM.Draw(nowDate);
                                break;
                            case "Hajk":
                                if (sommar(nowDate))
                                    hajk.Draw(nowDate);
                                else
                                    m.Draw(nowDate);
                                break;
                            case "starfield":
                                sf.Draw(nowDate);
                                break;
                            default:
                                if (nowDate != lastDate)
                                {
                                    Debug.WriteLine("No random effect");
                                }
                                break;

                            case "TS":
                                ts.Draw(nowDate);
                                break;
                        }
                        break;
                    case "birthday":
                        birthday.Draw(nowDate, ei.Name); 
                        break;
                    case "text":

                        string[] words = text.SplitFitString(ei.Name, 1.5f, 25.0f);
                        float y = 0.0f;
                        foreach (var n in words)
                        {
                            float middle = n.Length / 2.0f;
                            text.Draw(n, Text2D.FontName.Coolfont, new OpenTK.Vector3(middle * 0.15f, 0.2f - y, 0.4f), new OpenTK.Vector2(0.1f, 0.1f), new OpenTK.Vector2(4.0f, 0.0f), 1.5f); // fix in name...
                            y += 0.25f;
                            Debug.WriteLine(n);
                        }

                       
                        break;
                    default:
                        if (nowDate != lastDate)
                        {
                            Debug.WriteLine("No event");
                        }
                        break;
                }
                
            }

        }
        
        private bool difFbk(string nowDate)
        {
            bool ok = false;
            string month = "";

            if (nowDate != null)
                month = nowDate.Substring(5,2);

            if ("01".Equals(month))
            {
              ok = true;
            }
            else if ("07".Equals(month))
            {
                 ok = true;
            }
          

            return ok;
        }

        private bool sommar(string nowDate)
        {
            bool ok = false;
            string month = "";

            if (nowDate != null)
                month = nowDate.Substring(5, 2);

            if ("06".Equals(month) || "07".Equals(month) || "08".Equals(month))
            {
                ok = true;
            }
           
            return ok;
        }
            
    }//class
}//namespace
