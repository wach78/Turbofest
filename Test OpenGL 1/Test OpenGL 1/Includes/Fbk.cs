using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    class Fbk : IEffect
    {
        private bool disposed = false;
        private int image;
        private Sound snd;

        public Fbk(ref Sound sound)
        {
            image = 0;
            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\fbk2.png");
            snd = sound;
            snd.CreateSound(Sound.FileType.WAV, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/fbk.wav", "FBK");
        }

        ~Fbk()
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
            if (disposing)
            {
                // free managed resources
                GL.DeleteBuffers(1, ref image);
                this.image = -1;
                snd = null;
            }
            // free native resources if there are any.

            disposed = true;
        }


        private void drawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image);
            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.8f, -0.8f, 1.0f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-0.8f, -0.8f, 1.0f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-.8f, -0.00f, 1.0f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(0.8f, -0.00f, 1.0f); // top left 

            GL.End();
            GL.Disable(EnableCap.Texture2D);

        }//DrawImage

        public void Play()
        {
            if (snd.PlayingName() != "FBK") // this will start once the last sound is done, ie looping.
            {
                snd.Play("FBK");
            }
        }
        public void Stop()
        {
            //player.Stop();
        }
        public void Draw(string Date)
        {
            Play();
            drawImage();
        }//Draw

    }//class
}//namespace
