using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    class Outro : IEffect
    {
        private bool disposed;
        private int image;
        private Sound snd;


        public Outro(ref Sound sound)
        {
            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\BOSD.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            snd = sound;
            snd.CreateSound(Sound.FileType.WAV, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\Samples\\Blackheart.wav", "Outro");

            disposed = false;
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
                    snd = null;
                }
                // free native resources if there are any.
                Console.WriteLine(this.GetType().ToString() + " disposed.");
                disposed = true;
            }
        }

        public void Play()
        {
            if (snd.PlayingName() != "Outro") // this will start once the last sound is done, ie looping.
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
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(2.0f, -1.2f, 0.7f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-2.0f, -1.2f, 0.7f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-2.0f, 1.20f, 0.7f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(2.0f, 1.20f, 0.7f); // top left 

            GL.End();
            GL.Disable(EnableCap.Texture2D);

        }//DrawImage

        public void Draw(string Date)
        {
            Play();
            drawImage();

        }//Draw
    }//class
}//namespace
