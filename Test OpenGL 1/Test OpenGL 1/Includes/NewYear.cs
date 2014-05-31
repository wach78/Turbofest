using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace OpenGL
{
    /// <summary>
    /// NewYear effect
    /// </summary>
    class NewYear : IEffect
    {
        private bool disposed;
        private int image;
        private MoreFireWorks mfw;

        /// <summary>
        /// Constructor for NewYear effect
        /// </summary>
        public NewYear()
        {
            image = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/newyear.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            mfw = new MoreFireWorks();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~NewYear()
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
                    Util.DeleteTexture(ref image);
                    mfw.Dispose();
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");
                disposed = true;
            }
        }

        /// <summary>
        /// Draw image to screen
        /// </summary>
        private void drawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image);
            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(1.7f, -1.0f, 0.45f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-1.7f, -1.0f, 0.45f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-1.7f, 1.0f, 0.45f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(1.7f, 1.0f, 0.45f); // top left 

            GL.End();
            GL.Disable(EnableCap.Texture2D);
        }//DrawImage

        /// <summary>
        /// Draw NewYear effect on screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            drawImage();
            mfw.Draw(Date);
        }//Draw
    }//class
}//namspace
