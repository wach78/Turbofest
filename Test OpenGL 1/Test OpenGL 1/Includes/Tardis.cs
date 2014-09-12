using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    /// <summary>
    /// Tardis effect
    /// </summary>
    class Tardis : IEffect
    {
        private Sound snd;
        private int tardis;
        private int vortex;
        private int lightning;
        private int lightning2;
        private bool disposed = false;
        private string LastDate;
        private long ticks;
        private long oldTicks;
        private float x;
        private float y;
        private long tick;
        private float z1;
        private float z2;
        private float lightningx;
        private float lightningy;
        private List<float> xlist;
        private List<float> ylist;

        /// <summary>
        /// Constructor for Tardis effect
        /// </summary>
        /// <param name="sound">Sound system</param>
        public Tardis(ref Sound sound)
        {
            vortex = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/timeVortex.jpg");
            tardis = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/TARDIS.png", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            lightning = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/lightning.png", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            lightning2 = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/lightning2.png", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            
            snd = sound;
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/TARDIS.ogg", "Tardis");
            LastDate = string.Empty;

            this.x = -1.0f;
            this.y = 0.0f;
            this.tick = 0;
            this.ticks = 0;
            this.oldTicks = 0;
            this.z1 = 0.3f;
            this.z2 = 0.5f;
            this.lightningx = 0.0f;
            this.lightningy = 0.0f;
            xlist = new List<float>(new float[] { 0.2f, 0.4f, 0.6f, 0.8f,1.0f,1.2f });
            ylist = new List<float>(new float[] { 0.1f, 0.2f, 0.3f, 0.4f });
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Tardis()
        {
            Dispose(false);
            System.GC.SuppressFinalize(this);
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
                    Util.DeleteTexture(ref tardis);
                    Util.DeleteTexture(ref vortex);
                    Util.DeleteTexture(ref lightning);
                    Util.DeleteTexture(ref lightning2);
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
            GL.BindTexture(TextureTarget.Texture2D, vortex);
            GL.Enable(EnableCap.Blend); //       
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); //

            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led

            GL.TexCoord2(0.0, 1.0); GL.Vertex3(1.7f, -1.0f, 0.4f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-1.7f, -1.0f, 0.4f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-1.7f, 1.0f, 0.4f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(1.7f, 1.0f, 0.4f); // top left 

            GL.End();

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, tardis);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(BeginMode.Quads);



            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(1.5f + x, -0.50f + y, 0.3f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(1.0f + x, -0.50f + y, 0.3f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(1.0f + x, 0.0f + y, 0.3f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(1.5f + x, 0.0f + y, 0.3f); // top left 

            GL.End();


            ///////////blixt och dunder
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, lightning);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(BeginMode.Quads);



            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(1.5f + lightningx, -0.50f - lightningy, z1); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-1.0f - lightningx, -0.50f - lightningy, z1); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-1.0f - lightningx, 0.0f + lightningy, z1);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(1.5f + lightningx, 0.0f + lightningy, z1); // top left 

            GL.End();

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, lightning2);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(BeginMode.Quads);



            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(1.5f + lightningx, -0.50f - lightningy, z2); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-1.0f - lightningx, -0.50f - lightningy, z2); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-1.0f - lightningx, 1.0f + lightningy, z2);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(1.5f + lightningx, 1.0f + lightningy, z2); // top left 



            GL.End();
            GL.Disable(EnableCap.Blend);//
            GL.Disable(EnableCap.Texture2D);


        }//DrawImage

        /// <summary>
        /// Update position
        /// </summary>
        private void updateImge()
        {
            this.tick++;

            x = (float)(0.004 * Math.Sin((this.tick / 50.0) * 3.1415) * 250);
            x -= 1.2f;
            y = (float)(0.004 * Math.Sin((this.tick / 42.1) * 3.1415) * 125);
            y += 0.37f;
        }

        /// <summary>
        /// Shall we show a lightning strike
        /// </summary>
        private void toggleLightning()
        {
            ticks = System.DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            //ändra x och y varsin
            if (this.oldTicks != 0)
            {
                if ((this.ticks - this.oldTicks) > 500)
                {
                    if (this.z1 == 0.5f)
                    {
                        this.z1 = 0;
                    }
                    else
                    {
                        this.z1 = 0.5f;
                        this.lightningx = xlist[Util.Rnd.Next(0, 5)];
                        this.lightningy = xlist[Util.Rnd.Next(0, 5)];
                    }

                    if (this.z2 == 0.5f)
                    {
                        this.z2 = 0;
                    }
                    else
                    {
                        this.z2 = 0.5f;
                        this.lightningx = xlist[Util.Rnd.Next(0, 5)];
                        this.lightningy = xlist[Util.Rnd.Next(0, 5)];
                    }


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
        private void Play(String Date)
        {
            if (LastDate != Date && snd.PlayingName() != "Tardis") // this will start once the last sound is done, ie looping.
            {
                snd.Play("Tardis");
                LastDate = Date;
            }
        }

        /// <summary>
        /// Draw Talespin effect on screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            Play(Date);
            updateImge();
            toggleLightning();
            DrawImage();
        }//Draw
    }//class
}//namespace
