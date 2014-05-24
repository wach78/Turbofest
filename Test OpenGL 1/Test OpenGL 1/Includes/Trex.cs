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
    /// T-rex effect
    /// </summary>
    class Trex : IEffect
    {
        private int pony;
        private int t;
        private int image;
        private bool disposed;
        private string LastDate;
        private long ticks;
        private long oldTicks;
        private long tick;
        private long tick2;
        private float y;
        private float x;
        private float yt;
        private float xt;
        private bool ponyrun;
        private bool trexrun;
        private bool showend;
        private Sound snd;

        /// <summary>
        /// Constructor for T-rex effect
        /// </summary>
        /// <param name="sound">Sound system</param>
        public Trex(ref Sound sound)
        {
            disposed = false;
            ponyrun = false;
            trexrun = false;
            showend = false;
            pony = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/pony.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            t = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/Trex.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            image = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/this_is.png");
          
            snd = sound;
            //snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/Nerdy.ogg", "Nerdy");
         
            LastDate = string.Empty;
            ticks = 0;
            ticks = 0;
            oldTicks = 0;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Trex()
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
                    Util.DeleteTexture(ref pony);
                    Util.DeleteTexture(ref t);
                    Util.DeleteTexture(ref image);
        
                    ticks = 0;
                    oldTicks = 0;
                    ponyrun = false;
                    trexrun = false;
                    showend = false;
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");

                disposed = true;
            }
        }

        /// <summary>
        /// Draw image to screen
        /// </summary>
        private void DrawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, pony);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            /*
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(-0.8f, -1.2f, 0.8f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-1.8f, -1.2f, 0.8f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-1.8f, -0.2f, 0.8f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(-0.8f, -0.2f, 0.8f); // top left 
            */
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(1.2f + x, -1.2f + y, 0.8f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-0.8f + x, -1.2f + y, 0.8f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-0.8f + x, -0.2f + y, 0.8f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(1.2f + x, -0.2f + y, 0.8f); // top left 

            GL.End();

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, t);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(-2.0f + xt, -1.6f + yt, 0.8f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-3.2f + xt, -1.6f + yt, 0.8f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-3.2f + xt, -0.0f + yt, 0.8f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(-2.0f + xt, -0.0f + yt, 0.8f); // top left 


            GL.End();

            if (showend)
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
            }

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

            if (ponyrun)
            {
                this.tick++;
                this.y = (float)Math.Abs(0.001 * Math.Sin((this.tick / 42.1) * 3.1415) * 200);
                this.x += 0.007f;
            }

            if (this.xt > 2.2)
            {
                trexrun = false;
            }
            if (trexrun)
            {
                this.tick2++;
                this.yt = (float)Math.Abs(0.001 * Math.Sin((this.tick2 / 42.1) * 3.1415) * 200);
                this.xt += 0.005f;
            }

            

        }

        /// <summary>
        /// Update images positions
        /// </summary>
        public void updateImages()
        {
            ticks = System.DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;


            if (this.oldTicks != 0)
            {
                if (!ponyrun)
                {
                    if ((this.ticks - this.oldTicks) > 22000)
                    {
                        ponyrun = true;
                        trexrun = true;
                        oldTicks = ticks;
                    }//inner if
                }

                if ((this.ticks - this.oldTicks) > 32000)
                {
                    showend = true;
                    oldTicks = ticks;
                }//inner if

            }//outer if

            if (oldTicks == 0)
                oldTicks = ticks;

        }

        /// <summary>
        /// Play sound
        /// </summary>
        /// <param name="Date">New date?</param>
        public void Play(string Date)
        {
            if (LastDate != Date && snd.PlayingName() != "")
            {
                snd.Play("");
                LastDate = Date;
            }
        }

        /// <summary>
        /// Draw T-rex effect on screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            if (LastDate != Date)
            {
               // ponyrun = false;
                //trexrun = false;
                //showend = true;
            }

            Play(Date);
            
            updateImages();
            DrawImage();
        }

    }//class
}//namespace
