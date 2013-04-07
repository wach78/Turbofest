using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    class Datasmurf : IEffect
    {
        private Sound snd;
        private int image;
        private float x;
        private float y;

        private bool leftBorder;
        private bool rightBorder;
        private bool topBorder;
        private bool bottomBorder;
        private bool disposed = false;

        public Datasmurf(ref Sound sound)
        {
            x = -1.0f;
            y = 0.0f;
            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\dataSmurf.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255,0,255));

            snd = sound;
            //snd.CreateSound(Sound.FileType.WAV, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\samples\\datasmurf.wav", "Smurf");
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\samples\\datasmurf.ogg", "Smurf");
            leftBorder = true; 
            rightBorder = false;
            topBorder = true;
            bottomBorder = false;
        }

        ~Datasmurf()
        {
            Dispose(false);
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
                    image = 0;
                }
                // free native resources if there are any.
                Console.WriteLine(this.GetType().ToString() + " disposed.");
                disposed = true;
            }
        }

        public void Dispose()
        {
            Util.DeleteTexture(ref image);
            this.image = -1;
            snd = null;
            Console.WriteLine(this.GetType().ToString() + " disposed.");
            System.GC.SuppressFinalize(this);
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
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(2.0f + x, -0.85f + y, 1.0f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(1.0f + x, -0.85f + y, 1.0f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(1.0f + x, 0.10f + y, 1.0f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(2.0f + x, 0.10f + y, 1.0f); // top left 

            GL.End();
            GL.Disable(EnableCap.Blend);//
            GL.Disable(EnableCap.Texture2D);


        }//DrawImage

        private void moveImage()
        {
            if (x < -3.10f)
            {
                rightBorder = true;
                leftBorder = false;
            }

            if (x > 0.0f)
            {
                leftBorder = true;
                rightBorder = false;
            }

            if (y > 1.20f)
            {
                topBorder = true;
                bottomBorder = false;
            }

            if (y < 0.0f && y < -0.48f)
            {
                bottomBorder = true;
                topBorder = false;
            }

            if (!topBorder)
            {
                y += 0.010f;
            }

            if (!bottomBorder)
            {
                y -= 0.010f;
            }

            if (!rightBorder)
            {
                x -= 0.010f;
            }

            if (!leftBorder)
            {
                x += 0.010f;
            }

        }//moveImage

        private void Play()
        {
            if (snd.PlayingName() != "Smurf") // this will start once the last sound is done, ie looping.
            {
                snd.Play("Smurf");
            }
        }
        public void Draw(string Date)
        {
            Play();
            moveImage();
            DrawImage();
        }//Draw
    }//class
}//namespace 
