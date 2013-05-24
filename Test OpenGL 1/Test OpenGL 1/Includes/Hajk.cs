using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace OpenGL
{
    class Hajk : IEffect
    {
        private bool disposed;
        private int image;
        private Sound snd;
        private string LastDate;

        public Hajk(ref Sound sound)
        {
            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/Hajk.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            snd = sound;

            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/HAJK.ogg", "Hajk");
            disposed = false;
            LastDate = string.Empty;
        }

        ~Hajk()
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
                    snd = null;
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
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.6f, -1.0f, 1.0f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-0.6f, -1.0f, 1.0f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-0.6f, 0.2f, 1.0f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(0.6f, 0.2f, 1.0f); // top left 

            GL.End();
            GL.Disable(EnableCap.Texture2D);

        }//DrawImage

        public void Play(string Date)
        {
            if (LastDate != Date && snd.PlayingName() != "Hajk") // this will start once the last sound is done, ie looping.
            {
                snd.Play("Hajk");
                LastDate = Date;
            }
        }

        public void Draw(string Date)
        {
            Play(Date);
            drawImage();
        }//Draw
    }//class
}//namespace
