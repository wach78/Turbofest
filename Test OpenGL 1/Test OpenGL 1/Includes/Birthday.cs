using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    class Birthday : IEffect
    {
        private int image;
        private int ballonsImage;
        private Sound snd;
        private Text2D text;

        private int currentImage;
        private Ballons[] b;
        private const int NUMBEROFBALLONS = 20;
        private bool disposed = false;

        public Birthday(ref Sound sound, ref Text2D txt)
        {
            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\tarta.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            ballonsImage = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\ballons.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            snd = sound;
            text = txt;
            snd.CreateSound(Sound.FileType.WAV, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/birthday.wav", "Birthday");
            currentImage = 0;

            Random r = new Random();
            b = new Ballons[NUMBEROFBALLONS];

            for (int i = 0; i < NUMBEROFBALLONS; i++)
            {
                b[i] = new Ballons((r.Next(-30, 40)) / 10.0f, (r.Next(-10, 20) * -1) / 10.0f, 0.00001f, r.Next(1, 10) / 1000.0f, ballonsImage,
                    new Vector2[] {  new Vector2(0.0f + (currentImage * 0.2f), 1.0f),
                                     new Vector2(0.2f + (currentImage * 0.2f), 1.0f),
                                     new Vector2(0.2f + (currentImage * 0.2f), 0.0f),
                                     new Vector2(0.0f + (currentImage * 0.2f), 0.0f)});


                currentImage++;

                if (currentImage == 4)
                    currentImage = 0;

            }//for
        }
        ~Birthday()
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
                    image = 0;
                    snd = null;
                    for (int i = 0; i < NUMBEROFBALLONS; i++)
                    {
                        b[i].Dispose();
                        b[i] = null;
                    }
                }
                // free native resources if there are any.
                Console.WriteLine(this.GetType().ToString() + " disposed.");
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
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(-0.8f, -1.2f, 0.8f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-1.8f, -1.2f, 0.8f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-1.8f, -0.2f, 0.8f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(-0.8f, -0.2f, 0.8f); // top left 

            GL.End();

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

            
            for (int i = 0; i < NUMBEROFBALLONS; i++)
            {
                b[i].Draw();
            }
            

        }//DrawImage
        public void Play()
        {
            if (snd.PlayingName() != "Birthday") // this will start once the last sound is done, ie looping.
            {
                snd.Play("Birthday");
            }
        }

        public void Draw(string Date)
        {
            Play();
            drawImage();

        }//Draw

    }//class
}//namespace
