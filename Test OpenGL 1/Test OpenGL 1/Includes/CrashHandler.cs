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
        private double clock;
        private FileStream CrashFile;
        DialogResult CrashPresentDialogresult;
        private string FileName;
        XmlDocument XD;

        public CrashHandler()
        {
            clock = 0.0;
            CrashFile = null;
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
                    CrashFile.Close();
                    CrashFile.Dispose();
                    CrashFile = null;
                }

                // Indicate that the instance has been disposed.
                disposed = true;
                System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
            }
        }

        public void CheckCrash()
        {
            if (File.Exists(FileName)) // make this dynamic for each xmlfile...
            {
                /*XD.RemoveAll();
                XD.r
                XD.Load(CrashFile);*/

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

        public void WriteToCrash()
        {
        
        }

        public void update(double current)
        {
            clock = current;
        }


    }
}
