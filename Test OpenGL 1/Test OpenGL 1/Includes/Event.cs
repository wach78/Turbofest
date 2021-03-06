﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenGL.Event
{
    /// <summary>
    /// What event type is this?
    /// </summary>
    enum EventType {Effect, Text, Random};

    /// <summary>
    /// Interface for eventitem
    /// </summary>
    interface IEventItem
    {
        void Draw(string Date);
    }
    
    /// <summary>
    /// Event object
    /// </summary>
    class objdata : IComparable
    {
        private string name;
        private bool veto;
        private int prio;
        private int runs;

        public objdata(string name, bool veto, int prio, int runs)
        {
            this.name = name;
            this.veto = veto;
            this.prio = prio;
            this.runs = runs;
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
        public int Runs
        {
            get { return runs; }
        }

        public void decRuns()
        {
            this.runs--;
            this.prio--;
        }

        public void noMoreVeto()
        {
            this.veto = false;
            this.runs = 0;
        }

        public void vetoAgain()
        {
            this.veto = true;
            this.runs = 1;
        }

       

        public int CompareTo(object obj)
        {
            objdata Compare = (objdata)obj;
            int result = this.prio.CompareTo(Compare.prio);
            if (result == 0)
            {
                result = Util.Rnd.Next(0,1);
                if (result == 0)
                    result = -1;
            }
              
            return result;
        }
    }

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

    class Event : IDisposable
    {
        private bool disposed = false;
        private static int eventnum = 0;
       
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
        Midsummer mid;
        Vaffla vaf;
        Walpurgis wp;
        CrayFish crayfish;
        TeknatStyle ts;
        Matrix m;
        Quiz q;

        MoraT mt;
        SilverFang silverFang;
        Fuck fuck;
        Tardis tardis;
        Zelda zelda;
        GhostBusters gb;
        Sailormoon sailormoon;
        Talespin talepsin;
        ChipAndDale cd;
        Nerdy nerd;
        Trex trex;

        Swine swine;
        Tjall tjall;

        Ronja ronja;
        Emil emil;
        Djungelboken djungelboken;
        Fabbe fabbe;
        Drink drink;
        Frozen frozen;

        private EventItem eventCurrent;
        private Dictionary<string, List<objdata>> runEffectInMonth;

        //Event Date list
        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<EventItem>> events;
        private System.Collections.Generic.List<string> randomEvent;
        private string lastDate;
        private string nowDate;

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
            tl = new TurboLogo(ref sound, ref chess, (OpenGL.Util.SpringOrFall.Equals("Spring")? true:false)/*((ClockStart.Month >= 1 && ClockStart.Month <= 8)? false:true)*/ ); // vilken termin är det? jan till början av augusti VT, resten HT... random
            valentine = new Valentine(ref sound);
            wl = new WinLinux(ref chess); //random
            creators = new Self(ref sound); // random
            bb = new BB(ref sound); // random
            GM = new GummiBears(ref sound);
            NDay = new National(ref chess, ref sound);
            easter = new Easter(ref sound);
            hajk = new Hajk(ref sound);
            mid = new Midsummer(ref sound);
            vaf = new Vaffla();
            wp = new Walpurgis();
            crayfish = new CrayFish();
            
            ts = new TeknatStyle(ref chess, ref sound, ref text);
            m = new Matrix(ref text);
            q = new Quiz(ref text, false, ref sound);
            talepsin = new Talespin(ref sound);
            cd = new ChipAndDale(ref sound, ref chess);
            nerd = new Nerdy(ref chess, ref sound);
            trex = new Trex(ref sound);
            sailormoon = new Sailormoon(ref sound,ref chess);
            gb = new GhostBusters(ref sound);
            zelda = new Zelda(ref sound, ref chess);
            tardis = new Tardis(ref sound);
            fuck = new Fuck(ref sound, ref chess);

            silverFang = new SilverFang(ref sound);
            mt = new MoraT(ref sound);

            swine = new Swine(ref chess, ref text);
            tjall = new Tjall(ref chess, ref text);

            ronja = new Ronja(ref sound);
            emil = new Emil(ref sound);
            djungelboken = new Djungelboken(ref sound);
            fabbe = new Fabbe(ref sound);
            drink = new Drink(ref sound);
            frozen = new Frozen(ref sound);

            eventCurrent = null; // event item for events to be triggerd in clock_NewDate
            //randomEvent = new List<string>(new string[] { "starfield", "SuneAnimation", "TurboLogo", "Datasmurf", "WinLinux", "Scroller", "BB", "GummiBears", "TeknatStyle", "Matrix"});
            randomEvent = new List<string>(new string[] { "starfield", "Nerdy", "Talespin", "Sailormoon", "GhostBusters", "Zelda", "Tardis", "Fuck", "SilverFang", "MoraT" });
            //new stuff
             List<UtilXML.EventData> ed = UtilXML.Loadeffectdata();

            // TODO: Make a clean list with all events allowed to be used implement so that it is actaully usable instead of a switch at the bottom of this file.
            Dictionary<string, Effect> effects = new Dictionary<string, Effect>()
            {
                {"SuneAnimation", new Effect(sune, ed.Find(e => e.Name == "SuneAnimation"))},
                {"Dif",new Effect(dif, ed.Find(e => e.Name == "Dif"))},
                {"Fbk",new Effect(fbk, ed.Find(e => e.Name == "Fbk"))},
                {"TurboLogo",new Effect(tl, ed.Find(e => e.Name == "TurboLogo"))},
                {"Datasmurf", new Effect(smurf, ed.Find(e => e.Name == "Datasmurf"))},
                {"RMS",new Effect(richard, ed.Find(e => e.Name == "RMS"))},
                {"WinLinux",new Effect(wl, ed.Find(e => e.Name == "WinLinux"))},
                {"Scroller",new Effect(scroller, ed.Find(e => e.Name == "Scroller"))},
                {"Self",new Effect(creators, ed.Find(e => e.Name == "Self"))},
                {"BB",new Effect(bb, ed.Find(e => e.Name == "BB"))},
                {"GummiBears",new Effect(GM, ed.Find(e => e.Name == "GummiBears"))},
                {"Hajk",new Effect(hajk, ed.Find(e => e.Name == "Hajk"))},
                {"TeknatStyle",new Effect(ts, ed.Find(e => e.Name == "TeknatStyle"))},
                {"Matrix",new Effect(m, ed.Find(e => e.Name == "Matrix"))},
                {"Quiz",new Effect(q, ed.Find(e => e.Name == "Quiz"))},
                {"Talespin",new Effect(talepsin, ed.Find(e => e.Name == "Talespin"))},
                {"ChipDale",new Effect(cd, ed.Find(e => e.Name == "ChipDale"))},
                {"Nerdy",new Effect(nerd, ed.Find(e => e.Name == "Nerdy"))},
              /*  {"Trex",new Effect(trex, ed.Find(e => e.Name == "Trex"))},*/
                {"Sailormoon",new Effect(sailormoon, ed.Find(e => e.Name == "Sailormoon"))},
                {"GhostBusters",new Effect(gb, ed.Find(e => e.Name == "GhostBusters"))},
                {"Zelda",new Effect(zelda, ed.Find(e => e.Name == "Zelda"))},
                {"Tardis",new Effect(tardis, ed.Find(e => e.Name == "Tardis"))},
                {"Fuck",new Effect(fuck, ed.Find(e => e.Name == "Fuck"))},
                {"SilverFang",new Effect(silverFang, ed.Find(e => e.Name == "SilverFang"))},
                {"MoraT",new Effect(mt, ed.Find(e => e.Name == "MoraT"))},
                {"Ronja",new Effect(ronja, ed.Find(e => e.Name == "Ronja"))},
                {"Emil",new Effect(emil, ed.Find(e => e.Name == "Emil"))},
                {"Djungelboken",new Effect(djungelboken, ed.Find(e => e.Name == "Djungelboken"))},
                {"Fabbe",new Effect(fabbe, ed.Find(e => e.Name == "Fabbe"))},
                {"Drink",new Effect(drink, ed.Find(e => e.Name == "Drink"))},
                {"Frozen",new Effect(drink, ed.Find(e => e.Name == "Frozen"))}
            };

            runEffectInMonth = new Dictionary<string, List<objdata>>();

            string[] months = Util.monthlist(); 
            int counter;
            foreach (KeyValuePair<string, Effect> pair in effects)
            {
                counter = 0;
                foreach (bool b in pair.Value.RunAllowedlist)
                {
                    if (b == true)
                    {
                        if (!runEffectInMonth.ContainsKey(months[counter]))
                        {
                            runEffectInMonth.Add(months[counter], new List<objdata>());
                        }

                        runEffectInMonth[months[counter]].Add(new objdata(pair.Key, pair.Value.Vetolist[counter], pair.Value.Priolist[counter], pair.Value.Runslist[counter]));  
                    }
                    counter++; 
                }
            }


            clock.NewDate += clock_NewDate; // Event listener

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

            // this needs to be fixed nicer...
            if (events.ContainsKey(ClockEnd.ToShortDateString()))
            {
                events[ClockEnd.ToShortDateString()].Clear(); // force this to be top..
                events[ClockEnd.ToShortDateString()].Add( new EventItem("outro", "outro", ClockEnd.ToShortDateString()) );
            }
            else
            {
                events.Add(ClockEnd.ToShortDateString(), new List<EventItem>() { new EventItem("outro", "outro", ClockEnd.ToShortDateString()) });
            }
            
            // Random effects on dates with no effects and check against new list of allowed things for them...
            DateTime dt = ClockStart;
            bool star = (Util.Rnd.Next(0, 1000) < 500 ? true:false); // make this random at the start too?
            int num = 0;
         
            
            while (dt <= ClockEnd)
            {
                date = dt.ToShortDateString();
                if (!events.ContainsKey(date))
                {
                    EventItem ei;

                    if (num == 0 || num == 1)
                    {
                        ei = new EventItem("starfield", "random", date);
                    }
                    else
                    {
                        //ei = new EventItem(randomEvent[Util.Rnd.Next(1, randomEvent.Count)], "random", date);

                        string month = "";
                        if (dt != null)
                            month = dt.Month.ToString();

                        switch (month)
                        {
                            case "1": month = "jan"; break;
                            case "2": month = "feb"; break;
                            case "3": month = "mar"; break;
                            case "4": month = "apr"; break;
                            case "5": month = "maj"; break;
                            case "6": month = "jun"; break;
                            case "7": month = "jul"; break;
                            case "8": month = "aug"; break;
                            case "9": month = "sep"; break;
                            case "10": month = "okt"; break;
                            case "11": month = "nov"; break;
                            case "12": month = "dec"; break;
                        }//switch

                        if (runEffectInMonth.ContainsKey(month))
                        {
                            List<objdata> mobj = runEffectInMonth[month];

                            List<objdata> vetolist = new List<objdata>();
                            List<objdata> novetolist = new List<objdata>();

                            foreach (objdata n in mobj)
                            {

                                if ("Quiz".Equals(n.Name) && eventnum == 4)
                                {
                                    n.vetoAgain();
                                    eventnum = 0;
                                }


                                if (n.Veto == true)
                                {
                                    if (n.Runs > 0)
                                        vetolist.Add(n);
                                }
                                else
                                {
                                    if (n.Runs > 0)
                                        novetolist.Add(n);
                                }
                            }

                            vetolist.Sort();
                            novetolist.Sort();


                            if (vetolist.Count > 0)
                            {
                                ei = new EventItem(vetolist[0].Name, "random", date);
                                vetolist[0].noMoreVeto();
                            }
                            else if (novetolist.Count > 0)
                            {
                                ei = new EventItem(novetolist[0].Name, "random", date);
                                novetolist[0].decRuns();
                                if (eventnum < 4)
                                    eventnum++;
                            }
                            else
                            {
                                ei = new EventItem(randomEvent[Util.Rnd.Next(1, randomEvent.Count)], "random", date);
                            }
                        }
                        else
                        {
                            ei = new EventItem(randomEvent[Util.Rnd.Next(1, randomEvent.Count)], "random", date);
                        }
                    }
                            
                    num++;
                    if (num == 3)
                    {
                        num = 0;
                    }
                    ei = new EventItem("Self", "random", date); // this is for debuging new events
                    events.Add(date, new List<EventItem>());
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

        // TODO: Dispose of all events and graphics, sound for does events in a safe way
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // free managed resources
                    if (sound != null) sound.Dispose();

                    if (clock != null) clock.Dispose(); // 2 texturer
                    if (sune != null) sune.Dispose(); // 1 texturer
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
                    if (ts != null) ts.Dispose(); // 3 texturer
                    if (m != null) m.Dispose();
                    if (q != null) q.Dispose();
                    if (talepsin != null) talepsin.Dispose();
                    if (cd != null) cd.Dispose();
                    if (nerd != null) nerd.Dispose();
                    if (trex != null) trex.Dispose();
                    if (sailormoon != null) sailormoon.Dispose();
                    if (gb != null) gb.Dispose();
                    if (zelda != null) zelda.Dispose();
                    if (tardis != null) tardis.Dispose();
                    if (fuck != null) fuck.Dispose();
                    if (silverFang != null) silverFang.Dispose();
                    if (mt != null) mt.Dispose();
                    if (swine != null) swine.Dispose();
                    if (tjall != null) tjall.Dispose();
                    if (ronja != null) ronja.Dispose();
                    if (emil != null) emil.Dispose();
                    if (djungelboken != null) djungelboken.Dispose();
                    if (fabbe != null) fabbe.Dispose();
                    if (drink != null) drink.Dispose();
                    if (frozen != null) chess.Dispose(); 
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

        /// <summary>
        /// Event listener on new date and this is to be done...
        /// </summary>
        public void clock_NewDate() // TODO: clock_NewDate, Check this so we do not crash in unlucky states
        {
            lastDate = nowDate;
            nowDate = clock.CurrentClock().ToShortDateString(); // change to dtNew?
            ch.update(clock.clock, clock.CurrentClock());

            EventItem LastEvent = null;

            //sound.StopSound(); // this needs to be checked agains last and new event if there is no sound lets play it out?
            
            if (events.ContainsKey(nowDate))
            {
                // make this so that we can use more then one...

                LastEvent = eventCurrent;

                eventCurrent = events[nowDate][0];
                if (events[nowDate].Count > 1)
                {
                    for (int i = 0; i < events[nowDate].Count; i++)
                    {
                        if ("effect".Equals(events[nowDate][i].Type)) // effects prio over birthday and other things...
                        {
                            eventCurrent = events[nowDate][i];
                           
                            break;
                        }
                    }
                }

                if (LastEvent == null) // TODO: if there have not been a event this helps it not to bug out but why does it go here....
                {
                    sound.StopSound();
                }
                // Double events with sound buggs this out...
                else if ((eventCurrent.Name != "starfield" || eventCurrent.Type == "effect" || eventCurrent.Type == "birthday" ||
                    eventCurrent.Type == "text" || eventCurrent.Type == "outro") || 
                    (LastEvent.Type == "random" && eventCurrent.Type == "random" && eventCurrent.Name != "starfield")) // birth day or special day too....
                {
                    sound.StopSound();
                }
                 
            }  
            else // safty if moved to event trigger...
            {
                eventCurrent = null; // TODO: this might be bugging stuff out if we are unlucky
            }

            System.Diagnostics.Debug.WriteLine("Date updated in events.");
        }

       

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

            if (eventCurrent != null)
            {
                switch (eventCurrent.Type)
                {
                    case "effect":
                        switch (eventCurrent.Name)
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
                            case "Kräftpremiär":
                                crayfish.Draw(nowDate);
                                break;
                            case "swine":
                                swine.Draw(nowDate);
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
                        switch (eventCurrent.Name)
                        {
                            case "Datasmurf":
                                smurf.Draw(nowDate);
                                break;
                            case "Dif":             
                                    dif.Draw(nowDate);                
                                break;
                            case "Fbk":  
                                    fbk.Draw(nowDate);                             
                                break;
                            case "RMS":
                                richard.Draw(nowDate);
                                break;
                            case "Scroller":
                                scroller.Draw(nowDate);
                                break;
                            case "SuneAnimation":
                                sune.Draw(nowDate);
                                break;
                            case "TurboLogo":
                                tl.Draw(nowDate);
                                break;
                            case "WinLinux":
                                wl.Draw(nowDate);
                                break;
                            case "Self":
                                creators.Draw(nowDate);
                                break;
                            case "BB":
                                bb.Draw(nowDate);
                                break;
                            case "GummiBears":
                                GM.Draw(nowDate);
                                break;
                            case "Hajk":                       
                                    hajk.Draw(nowDate);
                                break;
                            case "starfield":
                                sf.Draw(nowDate);
                                break;
                            case"Quiz":
                                q.Draw(nowDate);
                                break;
                            case "TeknatStyle":
                                ts.Draw(nowDate);
                                break;
                            case "Matrix":
                                  m.Draw(nowDate);
                                break;
                            case "Talespin":
                                talepsin.Draw(nowDate);
                                break;
                            case "ChipDale":
                                cd.Draw(nowDate);
                                break;
                            case "Nerdy":
                                nerd.Draw(nowDate);
                                break;
                            case "Trex":
                                trex.Draw(nowDate);
                                break;
                            case "Sailormoon":
                                sailormoon.Draw(nowDate);
                                break;
                            case "GhostBusters":
                                gb.Draw(nowDate);
                                break;
                            case "Zelda":
                                zelda.Draw(nowDate);
                                break;
                            case "Tardis":
                                tardis.Draw(nowDate);
                                break;
                            case "Fuck":
                                fuck.Draw(nowDate);
                                break;
                            case "SilverFang":
                                silverFang.Draw(nowDate);
                                break;
                            case "MoraT":
                                mt.Draw(nowDate);
                                break;
                            case "Ronja":
                                ronja.Draw(nowDate);
                                break;
                            case "Emil":
                                emil.Draw(nowDate);
                                break;
                            case "Djungelboken":
                                djungelboken.Draw(nowDate);
                                break;

                            case "Fabbe":
                                fabbe.Draw(nowDate);
                                break;
                            case "Drink":
                                drink.Draw(nowDate);
                                break;
                            case "Frozen":
                                frozen.Draw(nowDate);
                                break;

                            default:
                                if (nowDate != lastDate)
                                {
                                    Debug.WriteLine("No random effect");
                                }
                                break;
                        }
                        break;
                    case "birthday":
                        birthday.Draw(nowDate, eventCurrent.Name); 
                        break;
                    case "text":
                        string[] words = text.SplitFitString(eventCurrent.Name, 1.5f, 25.0f);
                        float y = 0.0f;
                        foreach (var n in words)
                        {
                            float middle = n.Length / 2.0f;
                            text.Draw(n, Text2D.FontName.Coolfont, new OpenTK.Vector3(middle * 0.15f, 0.2f - y, 0.4f), new OpenTK.Vector2(0.1f, 0.1f), new OpenTK.Vector2(4.0f, 0.0f), 1.5f); // fix in name...
                            y += 0.25f;
                            //Debug.WriteLine(n);
                        }
                        break;
                    case "outro":
                        outro.Draw(nowDate);
                        break;
                    default:
                        if (nowDate != lastDate)
                        {
                            Debug.WriteLine("No event");
                        }
                        break;
                }
            }
            lastDate = nowDate;
        }
        /*
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
            */
    }//class
}//namespace
