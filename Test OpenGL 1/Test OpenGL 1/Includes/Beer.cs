using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace OpenGL
{
    class Beer
    {
        private int b;
        private int b1;
        private int b2;
        private int b3;

        private FireWorks[] fw;
        private bool disposed = false;

        public Beer()
        {
            b = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/Beer.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            b1 = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/Beer2.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            b2 = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/Beer3.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            b3 = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/Loka.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
         
            fw = new FireWorks [4];
            fw[0] = new FireWorks(b);
            fw[1] = new FireWorks(b1);
            fw[2] = new FireWorks(b2);
            fw[3] = new FireWorks(b3);
        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~Beer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing">Is it disposing?</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // free managed resources
                    Util.DeleteTexture(ref b);
                    Util.DeleteTexture(ref b1);
                    Util.DeleteTexture(ref b2);
                    Util.DeleteTexture(ref b3);
                }
                // free native resources if there are any.
                //  Debug.WriteLine(this.GetType().ToString() + " disposed.");

                disposed = true;
            }
        }
        /// <summary>
        /// Draw Fireworks on screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(String Date)
        {
            for (int i = 0; i < fw.Length; i++)
            {
                fw[i].Draw(Date);
            }
        }

    }//class
}//namespace
