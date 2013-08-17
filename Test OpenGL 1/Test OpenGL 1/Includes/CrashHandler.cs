using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Windows.Forms;


namespace OpenGL
{
    public class CrashHandler : IDisposable
    {
        private bool disposed = false;
        private bool SelfExit;
        private double clock;
        private FileStream CrashFile;
        DialogResult CrashPresentDialogresult;
        private string FileName;
        XmlDocument XD;

        public CrashHandler()
        {
            clock = 0.0;
            CrashFile = null;
            SelfExit = false;
            try
            {
                XD = new XmlDocument();
                FileName = Path.GetDirectoryName(Application.ExecutablePath) + "/Crash.xml";
                CrashFile = System.IO.File.Open(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        ~CrashHandler()
        {
            //Dispose(true);
        }

        public void Dispose()
        {
            Dispose(true);
            //base.Finalize();
            //GL.DeleteBuffers(2, this.m_tex); // this might bug out or?
            //this.m_tex = null; // protects for miss use if GC haven't been collecting...
            
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // If you need thread safety, use a lock around these  
            // operations, as well as in your methods that use the resource. 
            if (!disposed)
            {
                if (disposing)
                {
                    if (CrashFile != null)
                    {
                        CrashFile.Flush(true);
                        CrashFile.Close();
                        CrashFile.Dispose();
                        CrashFile = null;
                    }
                }

                // Indicate that the instance has been disposed.
                disposed = true;
                System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
            }
        }

        public DialogResult CrashDialogResult
        {
            get { return CrashPresentDialogresult; }
        }

        public double CrashClock
        {
            get { return clock; }
        }

        public bool Exit
        {
            get { return SelfExit; }
            set { SelfExit = value; }
        }

        public DialogResult CheckCrash()
        {
            if (File.Exists(FileName)) // make this dynamic for each xmlfile...
            {
                try
                {
                    XD.RemoveAll();
                    XD.Load(CrashFile);
                    XmlNodeList xnl = XD.GetElementsByTagName("runtime");
                    if (xnl.Count > 0)
                    {
                        clock = double.Parse(xnl[0].InnerText);
                    }
                    //System.Xml.Linq.XElement xd = System.Xml.Linq.XElement.Load(CrashFile);
                }
                catch (XmlException) // this is for xml file errors ie. not valid and so on...
                {
                    // create xml file  and does things here...
                    
                    XD.AppendChild(XD.CreateXmlDeclaration("1.0", "UTF-8", null));

                    XmlNode xn = XD.CreateNode(XmlNodeType.Element, null, "crashdata", null);
                    XmlNode xn2 = XD.CreateNode(XmlNodeType.Element, null, "runtime", null);
                    XmlNode xn2b = XD.CreateNode(XmlNodeType.Text, null, null, null);
                    XmlNode xn3 = XD.CreateNode(XmlNodeType.Element, null, "datetime", null);
                    XmlNode xn3b = XD.CreateNode(XmlNodeType.Text, null, null, null);

                    xn2b.Value = (0.0).ToString();
                    xn2.AppendChild(xn2b);
                    xn3b.Value = "0000-00-00 00:00:00";
                    xn3.AppendChild(xn3b);

                    xn.AppendChild(xn2);
                    xn.AppendChild(xn3);
                    XD.AppendChild(xn);
                    //CrashFile.Seek(0, SeekOrigin.Begin);
                    XD.Save(CrashFile);
                    CrashFile.Flush(true);
                    clock = double.Parse(xn2b.Value);
                }
                catch (Exception ex) // all else ...
                {
                    throw ex;
                }
                if (clock != 0.0)
                {
                    CrashPresentDialogresult = MessageBox.Show("There is a crash file pressent do you want to continue from it?", "Crash file pressent!", MessageBoxButtons.YesNoCancel);
                    if (CrashPresentDialogresult == DialogResult.Yes)
                    {
                        //load it and get the data...
                    }
                    else if (CrashPresentDialogresult == DialogResult.No) // remove file, and use 0.0 as time.
                    {
                        //File.Delete(FileName); // or just reset?
                    }
                    else
                    {
                        //do nothing! and don't start!
                    }
                }
                else
                {
                    CrashPresentDialogresult = DialogResult.No;
                }
            }
            else
            {
                CrashPresentDialogresult = DialogResult.No;
            }
            return CrashPresentDialogresult;
        }

        public void Clear()
        {
            try
            {
                CrashFile.Close();
                File.Delete(FileName);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                CrashFile = null; // might be bad as this might bug out dispose...
            }
        }

        public void update(double current, DateTime currentDateTime)
        {
            clock = current;
            XmlNodeList xnl = XD.GetElementsByTagName("crashdata");
            if (xnl.Count > 0)
            {
                CrashFile.Seek(0, SeekOrigin.Begin); // need to clear file here!!!
                CrashFile.SetLength(0);
                /*xnl[0].InnerText = clock.ToString();
                XD.Save(CrashFile);
                CrashFile.Flush(true);*/
                xnl = XD.GetElementsByTagName("runtime");
                if (xnl.Count == 1)
                {
                    xnl[0].InnerText = clock.ToString();
                }
                xnl = XD.GetElementsByTagName("datetime");
                if (xnl.Count == 1)
                {
                    xnl[0].InnerText = currentDateTime.ToString();
                }


                XD.Save(CrashFile);
                CrashFile.Flush(true);
            }
        }


    }
}
