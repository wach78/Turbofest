using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Diagnostics;

namespace OpenGL
{
    class Semla : IEffect
    {
        private int image;
        private float x;
        private float y;

        private long tick;

        private bool disposed = false;

        public Semla()
        {
            x = -1.0f;
            y = 0.0f;
            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/semla.bmp",TextureMinFilter.Linear,TextureMagFilter.Linear,TextureWrapMode.Clamp,TextureWrapMode.Clamp,System.Drawing.Color.FromArgb(255, 0, 255));
            tick = 0;

        }

        ~Semla()
         {
            Dispose(false);
         }
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);

        }

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
            

        }//DrawImage


        public void Draw(string Date)
        {
          
            this.tick++;
           // x = (float)Math.Sin(tick / 22.1f) * 0.6f - Size.Width / 2;
           // y = (float)Math.Cos(tick / 22.1f) * 0.4f - Size.Height / 2;
            /*
            x = (float)(Math.Sin((this.tick * 1.5) * Math.PI / 180));
            x -= 1.3f;
            y = (float)(Math.Cos((this.tick * 1.5) * Math.PI / 180) * 0.5f);
            y += 0.34f;
            */
            x = (float)(0.004 * Math.Sin((this.tick / 50.0) * 3.1415) * 250);
            x -= 1.2f;
            y = (float)(0.004 * Math.Sin((this.tick / 42.1) * 3.1415) * 125);
            y += 0.37f;

        

            DrawImage();
        }//Draw

    }//class
}//namespace
