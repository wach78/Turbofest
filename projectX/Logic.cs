using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace projectX
{
    class Logic
    {
        private int year;
        
        public Logic()
        {
            year = DateTime.Now.Year;         
        }

        private int checkFullMoon(int month, int day)
        {
            int c, e, b = 0;
            double jd = 0;

            if (month < 3)
            {
                year--;
                month +=12;
            }

            month++;

            c = (int)(365.25 * DateTime.Now.Year);
            e = (int)(30.6 * month);
            jd = (c + e + day) - 694039.09;
            jd /= 29.53;
            b = (int)jd;
            jd -= b;
            b = (int)(jd * 8 + 0.5);
            b = b & 7;

            return b;
        }

        private DateTime returnFullmoonDate()
        {
            bool again = true;
            int i = 0;
            int month = 3;
            int day = 20;
            int[] arr = new int[35];
            int[] dArr = new int[35];
            int[] mArr = new int[35];

            while (again)
            {
                arr[i] = checkFullMoon(month, day);
                mArr[i] = month;
                dArr[i] = day;

                i++;
                        
                day++;

                if (day == 31)
                {
                    month = 4;
                    day = 1;
                }

                if (month == 4 && day == 25)
                    again = false;
            }//while

            int index = 0;

            for (int j = 0; j < arr.Length; j++)
            {
                if (arr[j].Equals(4) && arr[j + 1] != 4)
                {
                    index = j;
                    break;
                }
            }

            return new DateTime(DateTime.Now.Year, mArr[index], dArr[index]);
        }//returnFullmoonDate

        private DateTime easterDate()
        {
            DateTime date = new DateTime();
            date = returnFullmoonDate();

            bool again = true;

            while (again)
            {
                if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    break;
                }
                else
                    date = date.AddDays(1);
            }

            return date;
        }//easterDate

        private DateTime semlaDate()
        {
            DateTime date = new DateTime();
            date = easterDate();
            date = date.AddDays(-47);

            return date;
        }//semlaDate

        private DateTime midsommarAfton()
        {
            //fredag 19 juni-25 juni
            DateTime date = new DateTime(year,6,19);
            bool again = true;

            while (again)
            {
                if (date.DayOfWeek == DayOfWeek.Friday)
                {
                    break;
                }
                else
                    date = date.AddDays(1);

            }//while

            return date;
        }//midsommarAfton

        private DateTime findFirstAdvent()
        {
            //Fjärde söndagen före juldagen, rörligt (27 november-3 december) 

            bool again = true;
            int month = 12;
            int day = 25;

            year = adjustYearForSpring(month);

            DateTime date = new DateTime();

            while (again)
            {
                date = new DateTime(year, month, day);
            
                if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    break;
                }
                else
                    day--;
                           
            }//while

            date = date.AddDays(- 21);
            return date;

        }//findFirstAdvent

        public string getShortDate(DateTime date)
        {
           return date.ToShortDateString();
        }//getShortDate

        private string monthNumber(string month)
        {
            string value = "";

            switch (month)
            {
                case "Jan": value = "01"; break;
                case "Feb": value = "02"; break;
                case "Mar": value = "03"; break;
                case "Apr": value = "04"; break;
                case "Maj": value = "05"; break;
                case "Jun": value = "06"; break;
                case "Jul": value = "07"; break;
                case "Aug": value = "08"; break;
                case "Sep": value = "09"; break;
                case "Okt": value = "10"; break;
                case "Nov": value = "11"; break;
                case "Dec": value = "12"; break;
            }//switch

            return value;
        }// monthNumber

        public static string monthName(string month)
        {
            string value = "";

            switch (month)
            {
                case "01": value = "Jan"; break;
                case "02": value = "Feb"; break;
                case "03": value = "Mar"; break;
                case "04": value = "Apr"; break;
                case "05": value = "Maj"; break;
                case "06": value = "Jun"; break;
                case "07": value = "Jul"; break;
                case "08": value = "Aug"; break;
                case "09": value = "Sep"; break;
                case "10": value = "Okt"; break;
                case "11": value = "Nov"; break;
                case "12": value = "Dec"; break;
            }//switch

            return value;
        }// monthName

        private int calculateRunTime(string runTime)
        {
            int runValue = 0;

            switch (runTime)
            {
                case "3 hours": runValue = 3 * 60 * 60 * 1000; break;
                case "3.5 hours": runValue = (3 * 60 * 60 * 1000) + (30 * 60 *1000); break;
                case "4 hours": runValue = 4 * 60 * 60 * 1000; break;
                case "4.5 hours": runValue = (4 * 60 * 60 * 1000) + (30 * 60 * 1000); break;
                case "5 hours": runValue = 5 * 60 * 60 * 1000; break;
                case "Test 1 hour": runValue = 60 * 60 * 1000; break;
            }//switch 
            return runValue;
        }//calculateRunTime

        public static string runTimeString(string timeInMs)
        {
            string value = "";

            switch (timeInMs)
            {
                case "10800000": value = "3 hours"; break;
                case "12600000": value = "3.5 hours"; break;
                case "14400000": value = "4 hours"; break;
                case "16200000": value = "4.5 hours"; break;
                case "18000000": value = "5 hours"; break;
                case "3600000" : value = "1 hour"; break; ;
            }//switch

            return value;
        }// runTimeString


        private string adjustStringFormatForDate(string input, string startEndBirth)
        {    
            string month = monthNumber(input.Substring(0, input.IndexOf(" ")));
            string day = input.Substring(input.IndexOf(" "), input.Length - input.IndexOf(" "));

            if (int.Parse(day) < 10)
                 day =  day.Replace(" ", "0");
            else
                day = day.Replace(" ", "");


            if ("Start".Equals(startEndBirth) && "Spring".Equals(FrmMain.SpringOrFall))
            {
                year = adjustYearForSpring(int.Parse(month));
            }
            else if ("Start".Equals(startEndBirth) && "Fall".Equals(FrmMain.SpringOrFall))
            {

            }
            else if ("End".Equals(startEndBirth) && "Spring".Equals(FrmMain.SpringOrFall))
            {
                if (year < DateTime.Now.Year)
                    year++;
            }
            else if ("End".Equals(startEndBirth) && "Fall".Equals(FrmMain.SpringOrFall))
            {
            }
            else if ("Birth".Equals(startEndBirth) && "Fall".Equals(FrmMain.SpringOrFall))
            {

            }
            else if ("Birth".Equals(startEndBirth))
                year = adjustYearForSpring(int.Parse(month));
          
                

           return year + "-" + month + "-" + day;         
        }//adjustStringFormatForDate

        private int adjustYearForSpring(int month)
        {
            int partyYear = 0;

            if (month >= 9)
                partyYear = year - 1;
            else
            {
                if (year < DateTime.Now.Year)
                    year++;
                else
                    partyYear = year;
            }
            
            return partyYear;
        }//adjustYearForSpring

        public static void adjustDate(string input, out string month, out string day)
        {
            input = input.Remove(0, input.IndexOf("-")+1);
            month = monthName(input.Substring(0, input.IndexOf("-")) );
            day = input.Substring(3,2);

        }//adjusDate

        public void createparty(string springOrFall, string fileName, string startDate,string endDate, string runTime)
        {
            XmlHandler xmlStuff = new XmlHandler(fileName,"Create");
            xmlStuff.createXmlFile(adjustStringFormatForDate(startDate,"Start"), adjustStringFormatForDate(endDate,"End") , calculateRunTime(runTime)+"");

            XmlHandler xmlStuff2 = new XmlHandler(fileName + ".xml", "Load");
            DateTime date = new DateTime();

            if ("Spring".Equals(springOrFall))
            {
                year--; //justering av year 
                date = new DateTime(year, 10, 24);
                xmlStuff2.writeFeastToXmlFile("FN-dagen", getShortDate(date), "Text");

                date = new DateTime(year, 10, 31);
                xmlStuff2.writeFeastToXmlFile("Halloween", getShortDate(date), "Effect");
              
                date = new DateTime(year, 12, 13);
                xmlStuff2.writeFeastToXmlFile("Lucia", getShortDate(date), "Effect");
                year++;//justering av year 

                date = findFirstAdvent();

                xmlStuff2.writeFeastToXmlFile("Advent", getShortDate(date), "Effect");

                date = date.AddDays(7);
                xmlStuff2.writeFeastToXmlFile("Advent", getShortDate(date), "Effect");

                date = date.AddDays(7);
                xmlStuff2.writeFeastToXmlFile("Advent", getShortDate(date), "Effect");

                date = date.AddDays(7);
                xmlStuff2.writeFeastToXmlFile("Advent", getShortDate(date), "Effect");

                date = new DateTime(year,12,24);
                xmlStuff2.writeFeastToXmlFile("JulAfton", getShortDate(date), "Effect");

                date = date.AddDays(1);
                xmlStuff2.writeFeastToXmlFile("JulDagen", getShortDate(date), "Text");

                date = date.AddDays(2);
                xmlStuff2.writeFeastToXmlFile("Mellandagsrea", getShortDate(date), "Text");

                date = new DateTime(year, 12, 31);
                xmlStuff2.writeFeastToXmlFile("Nyårsafton", getShortDate(date), "Effect");

                date = new DateTime(year, 12, 24);
                date = date.AddDays(13);
                xmlStuff2.writeFeastToXmlFile("Trettondedag jul", getShortDate(date), "Text");

                date = semlaDate();
                xmlStuff2.writeFeastToXmlFile("Semla", getShortDate(date), "Effect");
            }

            else if ("Fall".Equals(springOrFall))
            {

                date = new DateTime(year,3,25);
                xmlStuff2.writeFeastToXmlFile("Våffeldagen", getShortDate(date), "Effect");
              
                date = easterDate();// påskdagen
                date = date.AddDays(-1);//påskafton
                xmlStuff2.writeFeastToXmlFile("påsk", getShortDate(date), "Effect");

                date = new DateTime(year, 4, 30);
                xmlStuff2.writeFeastToXmlFile("Valborgsmässoafton", getShortDate(date), "Effect");

                date = new DateTime(year, 6, 6);
                xmlStuff2.writeFeastToXmlFile("Nationaldagen", getShortDate(date), "Effect");

                date = midsommarAfton();
                xmlStuff2.writeFeastToXmlFile("Midsommar", getShortDate(date), "Effect");

                date = new DateTime(year, 7, 14);
                xmlStuff2.writeFeastToXmlFile("kronprinsesans födelsedag", getShortDate(date), "Text");

                date = new DateTime(year, 8, 8);
                xmlStuff2.writeFeastToXmlFile("Kräftpremiär", getShortDate(date), "Text");

            }

        }//createparty

        public void createScrollers()
        {
            string fileName = "scrollers";

            fileName = Path.Combine("Scrollers\\", fileName);
            XmlHandler xmlStuff = new XmlHandler(fileName, "Create");
            xmlStuff.createScrollersXmlFile();

            fileName += ".xml";
            XmlHandler xmlStuff2 = new XmlHandler(fileName, "XEle");

            xmlStuff2.writeScrollerToXmlFile("turbophest");
            xmlStuff2.writeScrollerToXmlFile("detta är en liten text som scrollar förbi i rutan");
            xmlStuff2.writeScrollerToXmlFile("turbophest");
            xmlStuff2.writeScrollerToXmlFile("hoppas ni trivs.");
            xmlStuff2.writeScrollerToXmlFile("turbophest");
            xmlStuff2.writeScrollerToXmlFile("idag händer ingenting");
            xmlStuff2.writeScrollerToXmlFile("teknatsektionens sexmästeri");
            xmlStuff2.writeScrollerToXmlFile("oj vilken fin effekt");
            xmlStuff2.writeScrollerToXmlFile("hur många dagar visas turbologgan totalt?");

        }//createScrollers

        public void addparticpant(string fileName,string name, string section, string dateOfBirth, string allergy, string vegetarian, string paid)
        {
            //fileName = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + fileName; 
            XmlHandler xmlStuff = new XmlHandler(fileName,"Load");
            xmlStuff.writeParticipantToXmlFile(name, section, adjustStringFormatForDate(dateOfBirth,"Birth"), allergy, vegetarian,paid);
        }//addparticpant

        public void delParticpant(string fileName, System.Windows.Forms.DataGridViewRow row)
        {
            //fileName = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + fileName;
            XmlHandler xmlStuff = new XmlHandler(fileName, "Delete");
            xmlStuff.deletepartcipant(row);
        }//delParticpant

        public IEnumerable<XElement> getParticpant(string fileName)
        {
            //fileName = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + fileName;
            XmlHandler xmlStuff = new XmlHandler(fileName, "Delete");
            return xmlStuff.getParticipant();
        }

        public void updateParticpant(string fileName, System.Windows.Forms.DataGridViewRow row, string[] data)
        {
            //fileName = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + fileName;
            XmlHandler xmlStuff = new XmlHandler(fileName, "Delete");
            xmlStuff.updateParticipant(row, data, adjustStringFormatForDate(data[2],"Birth"));
        }//addParticpant

        public string getData(string fileName)
        {
            XmlHandler xmlStuff = new XmlHandler(fileName, "XDoc");
            return xmlStuff.getDataFromXml();
        }//getData

        public void updateData(string fileName,string oldRunTime,string sd, string ed,string rt)
        {
            XmlHandler xmlStuff = new XmlHandler(fileName, "XDoc");
            xmlStuff.uppdateData(adjustStringFormatForDate(sd, "Start"), oldRunTime, adjustStringFormatForDate(ed, "end"), calculateRunTime(rt) + "");
        }//uppdateData

        public void addScroller(string text)
        {
            string file = "scrollers.xml";
            file = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\XMLFiles\\Scrollers\\" + file;
            XmlHandler xmlStuff = new XmlHandler(file, "XEle");
            xmlStuff.writeScrollerToXmlFile(text);
        }//addScroller

        public void delScroller(string text)
        {
            string file = "scrollers.xml";
            file = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\XMLFiles\\Scrollers\\" + file;
            XmlHandler xmlStuff = new XmlHandler(file, "Delete");
            xmlStuff.deleteScroller(text);

        }//delScroller

        public XDocument sort(string fileName)
        {
            XmlHandler xmlStuff = new XmlHandler(fileName, "XDoc");
            return xmlStuff.sortXml();
        }//sort

  

        

    }//class
}//namespace
