using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace OpenGL
{
    class Outro : IEffect
    {
        private bool disposed;
        private int image;
        private Sound snd;
        private long ticks;
        private long oldTicks;
        private bool delyed;

        public Outro(ref Sound sound)
        {
            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/BOSD.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            snd = sound;
            //snd.CreateSound(Sound.FileType.WAV, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/Blackheart.wav", "Outro");
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/Blackheart.ogg", "Outro");

            disposed = false;
            ticks = 0;
            oldTicks = 0;
            delyed = false;
        }

         ~Outro()
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

        public void Play()
        {
            if (snd.PlayingName() != "Outro") // this will start once the last sound is done
            {
                snd.Play("Outro");
            }
        }


        private void drawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image);
            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(1.7f, -1.0f, 0.4f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-1.7f, -1.0f, 0.4f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-1.7f, 1.0f, 0.4f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(1.7f, 1.0f, 0.4f); // top left 

            GL.End();
            GL.Disable(EnableCap.Texture2D);

        }//DrawImage

        public void Draw(string Date)
        {
            ticks = System.DateTime.Now.Ticks / TimeSpan.TicksPerSecond;

            if (!delyed)
            {
                if (this.oldTicks != 0)
                {
                    if ((this.ticks - this.oldTicks) > 15)
                    {
                       
                        delyed = true;
                    }//inner if
                }//outer if

                if (oldTicks == 0)
                    oldTicks = ticks;
            }
            else
            {
                Play();
            }
            
            drawImage();

        }//Draw
    }//class
}//namespace
