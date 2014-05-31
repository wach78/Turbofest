using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    /// <summary>
    /// National day effect
    /// </summary>
    class National : IEffect
    {
        private Chess bakground;
        private Sound snd;
        private int currentImage;
        private int image;
        private long ticks;
        private long oldTicks;
        private string LastDate;
        private bool disposed = false;

        /// <summary>
        /// constructor of National day effect
        /// </summary>
        /// <param name="chess">Chessboard</param>
        /// <param name="sound">Sound system</param>
        public National(ref Chess chess, ref Sound sound)
        {
            ticks = 0;
            oldTicks = 0;
        
            currentImage = 0;
            bakground = chess;
            snd = sound;
            LastDate = string.Empty;

            image = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/flagga.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/Du gamla du fria (Black Ingvars).ogg", "National");
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~National()
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
                    currentImage = 0;
                    image = 0;
                }
                // free native resources if there are any.
                System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
                disposed = true;
            }
        }

        /// <summary>
        /// Change the image that is showing
        /// </summary>
        public void updateImages()
        {
            ticks = System.DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            if (this.oldTicks != 0)
            {
                if ((this.ticks - this.oldTicks) > 300)
                {
                    currentImage++;

                    if (currentImage > 1)
                        currentImage = 0;

                    oldTicks = ticks;
                }//inner if
            }//outer if

            if (oldTicks == 0)
                oldTicks = ticks;

        }

        /// <summary>
        /// Draw image to screen
        /// </summary>
        public void DrawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); 

            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0.0f + (currentImage * 0.5f), 1.0f); GL.Vertex3(0.7f, -0.55f, 1.0f); // bottom left // x y z alla i mitten Y-led 
            GL.TexCoord2(0.5f + (currentImage * 0.5f), 1.0f); GL.Vertex3(-0.8f, -0.55f, 1.0f); // bottom right // alla till vänster x-led
            GL.TexCoord2(0.5f + (currentImage * 0.5f), 0.0f); GL.Vertex3(-0.8f, 0.1f, 1.0f);// top right
            GL.TexCoord2(0.0f + (currentImage * 0.5f), 0.0f); GL.Vertex3(0.7f, 0.1f, 1.0f); // top left 

      

            GL.End();
            GL.Disable(EnableCap.Texture2D);

        }

        /// <summary>
        /// Play sound
        /// </summary>
        /// <param name="Date">New date?</param>
        public void Play(String Date)
        {
            if (LastDate != Date && snd.PlayingName() != "National")
            {
                snd.Play("National");
                LastDate = Date;
            }
        }

        /// <summary>
        /// Draw National day effect on screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            Play(Date);
            bakground.Draw(Date, Chess.ChessColor.Swe);
            updateImages();
            DrawImage();

        }//Draw
    }//class
}//namespace
