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
    /// Walpurgis effect
    /// </summary>
    class Walpurgis : IEffect
    {
        private bool disposed = false;
        private int image;
        private MoreFireWorks mfw;

        /// <summary>
        /// Cosntructor for Walpurgis effect
        /// </summary>
        public Walpurgis()
        {
            image = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/bonefire.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            mfw = new MoreFireWorks();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Walpurgis()
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
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.8f, -1.0f, 1.0f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-0.8f, -1.0f, 1.0f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-0.8f, -0.00f, 1.0f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(0.8f, -0.00f, 1.0f); // top left 

            GL.End();
            GL.Disable(EnableCap.Texture2D);

        }//DrawImage

        /// <summary>
        /// Draw Walpurgis effect on screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            drawImage();
            mfw.Draw(Date);
        }//Draw
    }//class
}//namepsece 
