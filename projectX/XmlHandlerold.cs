﻿using System;
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

namespace projectX
{
    class XmlHandler
    {
        private XElement xEle;
        private XDocument xDoc;
        private XmlWriter xWriter;
        private string path;

        public XmlHandler(string fileName,string createOrLoadOrDelete)
        {
            try
            {
                if ("Create".Equals(createOrLoadOrDelete))
                {  
                    path = fixPath(fileName + ".xml");
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = "\t";
                    xWriter = XmlWriter.Create(path, settings);                   
                }

                else if ("Load".Equals(createOrLoadOrDelete) || "XEle".Equals(createOrLoadOrDelete))
                {
                    path = fixPath(fileName);
                    xEle = XElement.Load(path);
                }

                else if ("Delete".Equals(createOrLoadOrDelete) || "XDoc".Equals(createOrLoadOrDelete))
                {
                    path = fixPath(fileName);
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

       public static string fixPath(string file)
       {
           string path = "XMLFiles\\" + file;
           path = Path.Combine("..\\", path);
           path = Path.Combine("..\\", path);

           return path;
       }//fixPath

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

        public IEnumerable<XElement> getParticipant()
        {
            //IEnumerable<XElement> xz = xDoc.Root.Elements("participants").Elements("participant");
            IEnumerable<XElement> element = xDoc.Root.Elements("participants").Elements("participant");

            return element;
        }//getParticipant

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
           
            xWriter.WriteFullEndElement(); ;//participants


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

            var element =
                          (
                           from x in xDoc.Root.Elements("scroller")
                           where x.Element("name").Value == text
                           select x
                          ).FirstOrDefault();

            element.Remove();

            xDoc.Save(path);
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
            List<string> data =
                            xDoc.Root.Elements("data")
                            .Select(x => (string)x)                 
                            .ToList();

            string str = "";
            foreach (string x in data)
            {
                str = x;
            }
            return str;

        }//getDataFromXml


        public XDocument sortXml()
        {
            var participants =(    
                               from x in xDoc.Root.Elements("participants")

                               select x
                              ).FirstOrDefault();

            var newDoc = new XDocument(new XElement("events"));
            XElement[] xx = new XElement[3];


            foreach (var x in participants.Elements())
            {      
                xx[0] = new XElement("name", x.Element("Name").Value );
                xx[1] = new XElement("date", x.Element("DateOfBirth").Value);
                xx[2] = new XElement("type", "birthday");

                newDoc.Root.Add(new XElement("event", xx));
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
            newDoc.Declaration = xDoc.Declaration;
            return newDoc;
        }//sortXml

        
    }//class
}//namespace


