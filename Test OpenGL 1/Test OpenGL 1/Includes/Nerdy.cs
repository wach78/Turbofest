using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace OpenGL
{
    /// <summary>
    /// Nerdy effect
    /// </summary>
    class Nerdy : IEffect
    {
        private int n1;
        private int n2;
        private int n3;
        private int n4;
        private int n5;
        private int n6;
        private int currentImage;
        private Sound snd;
        private Chess bakground;
        private bool disposed;
        private string LastDate;
        private long ticks;
        private long oldTicks;

        /// <summary>
        /// Constructor for Nerdy effect
        /// </summary>
        /// <param name="chess">Chessboard</param>
        /// <param name="sound">Sound system</param>
        public Nerdy(ref Chess chess, ref Sound sound)
        {
            disposed = false;
            n1 = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/n1.jpg");
            n2 = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/n2.jpg");
            n3 = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/n3.jpg");
            n4 = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/n4.jpg");
            n5 = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/n5.png");
            n6 = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/n6.png");
            snd = sound;
            bakground = chess;

            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/Nerdy.ogg", "Nerdy");
            currentImage = 0;

            LastDate = string.Empty;
            ticks = 0;
            oldTicks = 0;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Nerdy()
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
                    Util.DeleteTexture(ref n1);
                    Util.DeleteTexture(ref n2);
                    Util.DeleteTexture(ref n3);
                    Util.DeleteTexture(ref n4);
                    Util.DeleteTexture(ref n5);
                    Util.DeleteTexture(ref n6);
                    currentImage = 0;
                
                    ticks = 0;
                    oldTicks = 0;
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");

                disposed = true;
            }
        }

        /// <summary>
        /// Draw image to screen
        /// </summary>
        public void DrawImage()
        {
            GL.Enable(EnableCap.Texture2D);

            if (currentImage == 0)
            {
                GL.BindTexture(TextureTarget.Texture2D, n1);
            }
            else if (currentImage == 1)
            {
                GL.BindTexture(TextureTarget.Texture2D, n2);
            }
            else if (currentImage == 2)
            {
                GL.BindTexture(TextureTarget.Texture2D, n3);
            }
            else if (currentImage == 3)
            {
                GL.BindTexture(TextureTarget.Texture2D, n4);
            }
            else if (currentImage == 4)
            {
                GL.BindTexture(TextureTarget.Texture2D, n5);
            }
            else if (currentImage == 5)
            {
                GL.BindTexture(TextureTarget.Texture2D, n6);
            }

            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0.0f , 1.0f); GL.Vertex3(1.0f, -1.25f, 1.0f); // bottom left // x y z alla i mitten Y-led 
            GL.TexCoord2(1.0f  , 1.0f); GL.Vertex3(-1.0f, -1.25f, 1.0f); // bottom right // alla till vänster x-led
            GL.TexCoord2(1.0f , 0.0f); GL.Vertex3(-1.0f, 0.0f, 1.0f);// top right
            GL.TexCoord2(0.0f  , 0.0f); GL.Vertex3(1.0f, 0.0f, 1.0f); // top left 

            GL.End();
            GL.Disable(EnableCap.Texture2D);

        }

        /// <summary>
        /// Play sound
        /// </summary>
        /// <param name="Date">New date?</param>
        public void Play(string Date)
        {
            if (LastDate != Date && snd.PlayingName() != "Nerdy")
            {
                snd.Play("Nerdy");
                LastDate = Date;
            }
        }

        /// <summary>
        /// Change visible image
        /// </summary>
        public void updateImages()
        {
            ticks = System.DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;


            if (oldTicks != 0)
            {
                if ((ticks - oldTicks) > 6000)
                {
                    currentImage++;

                    if (currentImage > 5)
                    {
                        currentImage = 0;
                    }

                    oldTicks = ticks;
                }//inner if
            }//outer if

            if (oldTicks == 0)
                oldTicks = ticks;
        }

        /// <summary>
        /// Draw Nerdy effect on screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            if (LastDate != Date)
            {
                currentImage = 0;
                oldTicks = 0;
            }

            Play(Date);
            bakground.Draw(Date, Chess.ChessColor.BlackPurple);
            updateImages();
            DrawImage();
        }
    }
}
