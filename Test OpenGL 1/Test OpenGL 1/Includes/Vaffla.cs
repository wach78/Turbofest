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
    /// Vaffla effect
    /// </summary>
    class Vaffla : IEffect
    {
        private bool disposed;
        private int image;
        private float x;
        private float y;
        private long tick;

        /// <summary>
        /// Construtor for Vaffla effect
        /// </summary>
        public Vaffla ()
        {
            image = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/vaffla.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
         
            disposed = false; 
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Vaffla()
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
                  
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");

                disposed = true;
            }
        }

        /// <summary>
        /// Draw image to screen
        /// </summary>
        private void DrawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image);
            GL.Enable(EnableCap.Blend); //       
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); //

            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(1.6f + x, -0.85f + y, 0.4f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(1.0f + x, -0.85f + y, 0.4f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(1.0f + x, 0.10f + y, 0.4f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(1.6f + x, 0.10f + y, 0.4f); // top left 

            GL.End();
            GL.Disable(EnableCap.Blend);//
            GL.Disable(EnableCap.Texture2D);

            this.tick++;
           

            x = (float)(0.004 * Math.Sin((this.tick / 50.0) * 3.1415) * 250);
            x -= 1.2f;
            y = (float)(0.004 * Math.Sin((this.tick / 42.1) * 3.1415) * 125);
            y += 0.37f;
        }//DrawImage

        /// <summary>
        /// Draw Vaffla effect on screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            DrawImage();
        }//Draw

    }//class
}//namespace
