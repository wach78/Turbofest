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
    /// <summary>
    /// Midsummer effect
    /// </summary>
    class Midsummer : IEffect
    {
        private int image;
        private int image2;
        private int currentImage;
        private Sound snd;
        private bool disposed = false;
        private string LastDate;
        private Raindrops[] sf;
        private const int NUMBEROFRAINDROPS = 75;

        /// <summary>
        /// Constructor for Midsummer effect
        /// </summary>
        /// <param name="sound">Sound system</param>
        public Midsummer(ref Sound sound)
        {
            image = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/rain.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            image2 = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/mid.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            currentImage = 0;
            snd = sound;

            LastDate = string.Empty;
            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/grodorna.ogg", "Midsummer");
            sf = new Raindrops[NUMBEROFRAINDROPS];

            float z = 0.4f;

            for (int i = 0; i < NUMBEROFRAINDROPS; i++)
            {

                sf[i] = new Raindrops((Util.Rnd.Next(-18, 15)) / 10.0f, (Util.Rnd.Next(-10, 20) * -1) / 10.0f, Util.Rnd.Next(2, 8) / 1000.0f, image,
                    new Vector2[] {  new Vector2(0.0f + (currentImage * 0.33f), 1.0f),
                                     new Vector2(0.2f + (currentImage * 0.33f), 1.0f),
                                     new Vector2(0.2f + (currentImage * 0.33f), 0.0f), 
                                     new Vector2(0.0f + (currentImage * 0.33f), 0.0f)}, Util.Rnd.Next(5, 10) * 10.0f, z);


                z -= 0.00001f;
                currentImage++;

                if (currentImage == 2)
                    currentImage = 0;
                  
            }
                
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Midsummer()
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
                   Util.DeleteTexture(ref image2);

                   for (int i = 0; i < NUMBEROFRAINDROPS; i++)
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

        /// <summary>
        /// Draw image to screen
        /// </summary>
        private void drawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image2);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(BeginMode.Quads);
            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.6f, -1.0f, 1.0f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-0.6f, -1.0f, 1.0f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-0.6f, 0.2f, 1.0f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(0.6f, 0.2f, 1.0f); // top left 

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

            for (int i = 0; i < NUMBEROFRAINDROPS; i++)
            {
                sf[i].Draw("");
            }


        }//DrawImage

        /// <summary>
        /// Play sound
        /// </summary>
        /// <param name="Date">New date?</param>
        public void Play(String Date)
        {
            if (LastDate != Date && snd.PlayingName() != "Midsummer")
            {
                snd.Play("Midsummer");
                LastDate = Date;
            }
        }

        /// <summary>
        /// draw Midsummer effect on screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            Play(Date);
            drawImage();
        }//Draw
    }//class


}//namespace
