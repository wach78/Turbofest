using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace OpenGL
{
    class NewYear : IEffect
    {

        private bool disposed;
        private int image;

        private MoreFireWorks mfw;

        public NewYear()
        {
            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/newyear.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            mfw = new MoreFireWorks();

        }

        ~NewYear()
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
                    mfw.Dispose();
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");
                disposed = true;
            }
        }

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

        public void Draw(string Date)
        {
            drawImage();
            mfw.Draw(Date);
            
        }//Draw
    }//class
}//namspace
