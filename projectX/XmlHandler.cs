using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

using System.Xml.Linq;
using System.Diagnostics;
using System.Web;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Printing;
using System.Collections;

namespace projectX
{
    class XmlHandler
    {
        private XElement xEle;
        private XDocument xDoc;
        private XmlWriter xWriter;
        private string path;

        public XmlHandler(string fileName, string createOrLoadOrDelete, string optional = "turbo")
        {
            // System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + filename
            try
            {
                if ("Create".Equals(createOrLoadOrDelete))
                {
                    path = fixPath(fileName + ".xml", optional);
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = "\t";
                    xWriter = XmlWriter.Create(path, settings);                   
                }
                else if ("Load".Equals(createOrLoadOrDelete) || "XEle".Equals(createOrLoadOrDelete))
                {
                    path = fixPath(fileName, optional);
                    xEle = XElement.Load(path);
                }
                else if ("Delete".Equals(createOrLoadOrDelete) || "XDoc".Equals(createOrLoadOrDelete))
                {
                    path = fixPath(fileName, optional);
                    xDoc = XDocument.Load(path);
                }
                
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }             

            
       }

       private void readOnlyFile(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.ReadOnly);
        }//readOnlyFile

        private void writeToFileAccepted(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
        }//writeToFileAccepted

        public static string fixPath(string file, string optional = "turbo")
       {
           if (File.Exists(file))
           {
               return file;
           }
           else  if ("turbo".Equals(optional))
           {
               return System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/XMLFiles/" + file;
           }
           else if ("eff".Equals(optional))
           {
               return System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/XMLFiles/Effects/" + file;
           }
           else
           {
               return file;
           }

       }//fixPath

       public IEnumerable<XElement> getParticipant()
       {
           //IEnumerable<XElement> xz = xDoc.Root.Elements("participants").Elements("participant");
           IEnumerable<XElement> element = null;
           try
           {
               element = xDoc.Root.Elements("participants").Elements("participant");
           }
           catch
           {
               MessageBox.Show("Error while geting Participants");
           }

           return element;
       }//getParticipant

       public void deletepartcipant(System.Windows.Forms.DataGridViewRow row)
       {
           
           var element =
                         (
                          from x in xDoc.Root.Elements("participants").Elements("participant")
                          where x.Element("Name").Value == row.Cells[0].Value.ToString() && x.Element("Section").Value == row.Cells[1].Value.ToString() &&
                          x.Element("DateOfBirth").Value == row.Cells[2].Value.ToString() && x.Element("Allergy").Value == row.Cells[3].Value.ToString() &&
                          x.Element("Vegetarian").Value == row.Cells[4].Value.ToString() && x.Element("Paid").Value == row.Cells[5].Value.ToString() 
                          select x
                         ).FirstOrDefault();

           element.Remove();
           
           /*
           XElement element = null;

           foreach (XElement x in xDoc.Root.Elements("participants").Elements("participant"))
           {
               if (x.Element("Name").Value == row.Cells[0].Value.ToString() && x.Element("Section").Value == row.Cells[1].Value.ToString() &&
                          x.Element("DateOfBirth").Value == row.Cells[2].Value.ToString() && x.Element("Allergy").Value == row.Cells[3].Value.ToString() &&
                          x.Element("Vegetarian").Value == row.Cells[4].Value.ToString() && x.Element("Paid").Value == row.Cells[5].Value.ToString())
               {
                   element = x;
               }
           }
           element.Remove();
           */
           xDoc.Save(path);
       }//deletepartcipant

        public void writeParticipantToXmlFile(string name, string section, string dateOfBirth, string allergy, string vegetarian,string paid)
        {         
            xEle.Elements("participants").LastOrDefault().Add(new XElement("participant",
                    new XElement("Name", name),
                    new XElement("Section", section),
                    new XElement("DateOfBirth", dateOfBirth),
                    new XElement("Allergy", allergy),
                    new XElement("Vegetarian", vegetarian),
                    new XElement("Paid", paid)
            ));
            
           xEle.Save(path);
              
        }//writeParticipantToXmlFile

        public void writeFeastToXmlFile(string name, string date, string type)
        {
            xEle.Elements("feasts").LastOrDefault().Add(new XElement("feast",
                 new XElement("name", name),
                 new XElement("date", date),
                 new XElement("type", type)
  
        ));

            xEle.Save(path);
        }//writeFeastToXmlFile

        public void writeScrollerToXmlFile(string text)
        {
            try 
            {                                                        // You are getting the exception because:
                if (xEle != null)                                    // doc.Element("stock").Add(newElement);  
                {                                                    // stock is the root node, and doc.Element("stock") returns null.
                    xEle.Add(new XElement("scroller",                // What you are actually trying to do is to add an item in your xml.  
                         new XElement("name", text),                 // Try the following:
                         new XElement("type", "scroll")              // doc.Add(newElement);
                                                                     
                    ));

                    xEle.Save(path);
                }
            }
            catch (Exception)
            {
            }
        }//writeFeastToXmlFile
        
        public void updateParticipant(System.Windows.Forms.DataGridViewRow row, string[] data,string date)
        {
            var element =
              (
               from x in xDoc.Root.Elements("participants").Elements("participant")
               where x.Element("Name").Value == row.Cells[0].Value.ToString() && x.Element("Section").Value == row.Cells[1].Value.ToString() &&
               x.Element("DateOfBirth").Value == row.Cells[2].Value.ToString() && x.Element("Allergy").Value == row.Cells[3].Value.ToString() &&
               x.Element("Vegetarian").Value == row.Cells[4].Value.ToString() && x.Element("Paid").Value == row.Cells[5].Value.ToString()
               select x
              ).FirstOrDefault();

            element.SetElementValue("Name",data[0]);
            element.SetElementValue("Section", data[1]);
            element.SetElementValue("DateOfBirth", date);
            element.SetElementValue("Allergy", data[3]);
            element.SetElementValue("Vegetarian", data[4]);
            element.SetElementValue("Paid", data[5]);

            xDoc.Save(path);
        }//updateParticipant

        public void createXmlFile(string startDate, string endDate, string runTime)
        {  
            xWriter.WriteStartDocument();
            
            xWriter.WriteStartElement("turbo");
            xWriter.WriteStartElement("data");

            xWriter.WriteStartElement("startDate");
            xWriter.WriteString(startDate);
            xWriter.WriteEndElement();
            xWriter.WriteStartElement("endDate");
            xWriter.WriteString(endDate);
            xWriter.WriteEndElement();
            xWriter.WriteStartElement("runTime");
            xWriter.WriteString(runTime);
            xWriter.WriteEndElement();

            xWriter.WriteEndElement();//data

            xWriter.WriteStartElement("feasts");

  

            

            xWriter.WriteFullEndElement();//Feasts

            
            xWriter.WriteStartElement("participants");

          //  xWriter.WriteStartElement("participant");
          //  xWriter.WriteFullEndElement(); ;//participant
           
            xWriter.WriteFullEndElement(); //participants


            xWriter.WriteEndElement(); //turbo
            xWriter.WriteEndDocument();
            xWriter.Close();    

        }//createXmlFile

        public void createScrollersXmlFile()
        {
            xWriter.WriteStartDocument();

            xWriter.WriteStartElement("scrollers");
            xWriter.WriteFullEndElement();
           
            xWriter.WriteEndDocument();
            xWriter.Close();    

        }//createScrollersXmlFile

        public void deleteScroller(string text)
        {
            try
            {
                var element =
                              (
                               from x in xDoc.Root.Elements("scroller")
                               where x.Element("name").Value == text 
                               select x
                              ).FirstOrDefault();

                element.Remove();

                xDoc.Save(path);
            }
            catch(Exception )
            {
            }
        }//deleteScroller

        public void uppdateData(string startDate, string oldRunTime,string endDate, string runTime)
        {
              var element =(
                           from x in xDoc.Root.Elements("data")
                           where  x.Element("runTime").Value == oldRunTime
                           select x
                           ).FirstOrDefault();

              element.SetElementValue("startDate", startDate);
              element.SetElementValue("endDate", endDate);
              element.SetElementValue("runTime", runTime);
            
              xDoc.Save(path);
        }//updateData

        public string getDataFromXml()
        {
            string str = "";
            try
            {
                List<string> data =
                                xDoc.Root.Elements("data")
                                .Select(x => (string)x)
                                .ToList();
                
                foreach (string x in data)
                {
                    str = x;
                }
               
            }
            catch (Exception)
            {

            }

            return str;
        }//getDataFromXml


        public XDocument sortXml()
        {
            //get startDate and endDate

           string data = getDataFromXml();
           string startDate = data.Substring(0, 10);
           string endDate = data.Substring(10, 10);


           // where DateTime.Parse(x.Element("DateOfBirth").Value) >= DateTime.Parse(startDate) &&
             //                       DateTime.Parse(x.Element("DateOfBirth").Value) <= DateTime.Parse(endDate)

            
           var participants = (
                          from x in xDoc.Root.Elements("participants")
                          select x
                          ).FirstOrDefault();

            
            /*
           var participants = (
                              from x in xDoc.Root.Element("participants").Elements("participant")

                              let date = DateTime.Parse(x.Element("DateOfBirth").Value)
                              where date >= DateTime.Parse(startDate) && date <= DateTime.Parse(endDate)
                              select x
                              ).ToList();
            */

        //   Debug.WriteLine(participants);
         //  Debug.WriteLine("*********************");
            var newDoc = new XDocument(new XElement("events"));
            XElement[] xx = new XElement[3];


            foreach (var x in participants.Elements("participant"))
            {      
                DateTime date = DateTime.Parse( x.Element("DateOfBirth").Value);

                if (date >= DateTime.Parse(startDate) && date <= DateTime.Parse(endDate))
                {

                    xx[0] = new XElement("name", x.Element("Name").Value);
                    xx[1] = new XElement("date", x.Element("DateOfBirth").Value);
                    xx[2] = new XElement("type", "birthday");

                    newDoc.Root.Add(new XElement("event", xx));
                }
            }


            var feasts = (
                   from x in xDoc.Root.Elements("feasts")

                   select x
                  ).FirstOrDefault();


            foreach (var x in feasts.Elements())
            {               
                xx[0] = new XElement("name", x.Element("name").Value);
                xx[1] = new XElement("date", x.Element("date").Value);
                xx[2] = new XElement("type", x.Element("type").Value);

                newDoc.Root.Add(new XElement("event", xx));
            }

            newDoc = new XDocument(new XElement("events",
                from p in newDoc.Root.Elements("event")
                orderby p.Element("date").Value
                select p));

            //Debug.WriteLine(newDoc);
            return newDoc;
        }//sortXml

        public void createXmlFileEffecs(ArrayList list)
        {
            xWriter.WriteStartDocument();
            xWriter.WriteStartElement("effects");

            foreach (eventdata obj in list)
            {

                xWriter.WriteStartElement("effect");
                xWriter.WriteStartElement("Name");
                xWriter.WriteString(obj.Name);
                xWriter.WriteEndElement(); //name

                xWriter.WriteStartElement("Veto");
                xWriter.WriteString(obj.Veto.ToString());
                xWriter.WriteEndElement(); //Veto

                xWriter.WriteStartElement("Prio");
                xWriter.WriteString(obj.Prio.ToString());
                xWriter.WriteEndElement(); //Prio

                ArrayList listname = new ArrayList(obj.Namelist);
                ArrayList listruns = new ArrayList(obj.Runslist);
                ArrayList listrunallowed = new ArrayList(obj.RunAllowedlist);

                int len = listname.Count;
                 xWriter.WriteStartElement("Months");
                 for (int i = 0; i < len; i++)
                 {
                     xWriter.WriteStartElement("Month");
                     xWriter.WriteStartElement("Name");
                     xWriter.WriteString(listname[i].ToString());
                     xWriter.WriteEndElement(); //Name

                     xWriter.WriteStartElement("Runs");
                     xWriter.WriteString(listruns[i].ToString());
                     xWriter.WriteEndElement(); //runs

                     xWriter.WriteStartElement("RunAllowed");
                     xWriter.WriteString(listrunallowed[i].ToString());
                     xWriter.WriteEndElement(); //RunAllowed

                     xWriter.WriteEndElement(); //Month
                 }
                xWriter.WriteEndElement(); //Months

                xWriter.WriteEndElement();//effect

            }
          //  xWriter.WriteFullEndElement(); // name

            xWriter.WriteEndElement(); // effects
            xWriter.WriteEndDocument();
            xWriter.Close();  
        }

        public ArrayList Loadeffectdata()
        {
            ArrayList objlist = new ArrayList();
            var effects = xDoc.Descendants("effect");

            foreach (var effect in effects)
            {
                eventdata ev = new eventdata(effect.Element("Name").Value, bool.Parse(effect.Element("Veto").Value), Int16.Parse(effect.Element("Prio").Value));
             
                var months = effect.Descendants("Month");

                foreach (var m in months)
                {
                    ev.setDataMonth(m.Element("Name").Value, Int16.Parse(m.Element("Runs").Value), bool.Parse(m.Element("RunAllowed").Value));
                }

                objlist.Add(ev);
            }

            return objlist;
        }

    }//class
}//namespace


