using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using OpenTK;

namespace OpenGL
{
    class Frozen : IEffect
    {
        private bool disposed;
        private int image;
        private Sound snd;
        private string LastDate;
        private int snowImage;
        private int currentImage;
        private SnowFlake[] sf;
        private const int NUMBEROFFLAKES = 150;

        public Frozen(ref Sound sound)
        {
            image = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/Frozen.jpg", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            snd = sound;
           
            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/Frozen.ogg", "Frozen");
            disposed = false;
            LastDate = string.Empty;

            snowImage = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/snow1_db.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            currentImage = 0;
            sf = new SnowFlake[NUMBEROFFLAKES];
            float z = 0.4f;

            for (int i = 0; i < NUMBEROFFLAKES; i++)
            {

                sf[i] = new SnowFlake((Util.Rnd.Next(-18, 15)) / 10.0f, (Util.Rnd.Next(-10, 20) * -1) / 10.0f, Util.Rnd.Next(2, 8) / 1000.0f, snowImage,
                    new Vector2[] {  new Vector2(0.0f + (currentImage * 0.2f), 1.0f),
                                     new Vector2(0.2f + (currentImage * 0.2f), 1.0f),
                                     new Vector2(0.2f + (currentImage * 0.2f), 0.0f),
                                     new Vector2(0.0f + (currentImage * 0.2f), 0.0f)}, Util.Rnd.Next(5, 10) * 10.0f, z);


                z -= 0.00001f;
                currentImage++;

                if (currentImage == 4)
                    currentImage = 0;
            }
        }

        ~Frozen()
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
                    Util.DeleteTexture(ref snowImage);

                    for (int i = 0; i < NUMBEROFFLAKES; i++)
                    {
                        sf[i].Dispose();
                        sf[i] = null;
                    }
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
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.9f, -0.9f, 1.0f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-0.9f, -0.9f, 1.0f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-0.9f, -0.00f, 1.0f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(0.9f, -0.00f, 1.0f); // top left 

            GL.End();
            GL.Disable(EnableCap.Texture2D);


            for (int i = 0; i < NUMBEROFFLAKES; i++)
            {
                sf[i].Draw("");
            }

        }//DrawImage
       

        /// <summary>
        /// Play sound
        /// </summary>
        /// <param name="Date">New date?</param>
        public void Play(string Date)
        {
            if (LastDate != Date && snd.PlayingName() != "Frozen")
            {
                snd.Play("Frozen");
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
