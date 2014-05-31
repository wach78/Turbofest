using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Diagnostics;

namespace OpenGL
{
    /// <summary>
    /// Datasmurf effect
    /// </summary>
    class Datasmurf : IEffect
    {
        private Sound snd;
        private Text2D text;
        private int image;
        private float x;
        private float y;
        private bool disposed = false;
        private int tick;
        private string LastDate;
        private long ticks;
        private long oldTicks;
        private int currentImage;

        /// <summary>
        /// Constructor for Datasmurf effect
        /// </summary>
        /// <param name="sound">Sound system</param>
        /// <param name="txt">Text printing</param>
        public Datasmurf(ref Sound sound, ref Text2D txt)
        {
            x = -1.0f;
            y = 0.0f;
            image = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/dataSmurf.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255,0,255));

            snd = sound;
            text = txt;
            //snd.CreateSound(Sound.FileType.WAV, Util.CurrentExecutionPath + "/Samples/datasmurf.wav", "Smurf");
            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/datasmurf.ogg", "Smurf");

            tick = 0;
            LastDate = string.Empty;

            currentImage = 0;
            ticks = oldTicks = 0;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Datasmurf()
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
                    Util.DeleteTexture(ref image);
                    text = null;
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
            GL.BindTexture(TextureTarget.Texture2D, image);
            GL.Enable(EnableCap.Blend); //       
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); //

            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0 + (currentImage * 0.2f), 1.0); GL.Vertex3(2.0f + x, -0.75f + y, 0.4f); // bottom left  
            GL.TexCoord2(0.2 + (currentImage * 0.2f), 1.0); GL.Vertex3(1.1f + x, -0.75f + y, 0.4f); // bottom right 
            GL.TexCoord2(0.2 + (currentImage * 0.2f), 0.0); GL.Vertex3(1.1f + x, 0.10f + y, 0.4f);// top right
            GL.TexCoord2(0.0 + (currentImage * 0.2f), 0.0); GL.Vertex3(2.0f + x, 0.10f + y, 0.4f); // top left 

            GL.End();
            GL.Disable(EnableCap.Blend);//
            GL.Disable(EnableCap.Texture2D);


        }//DrawImage

        /// <summary>
        /// Move image
        /// </summary>
        private void moveImage()
        {
            
            this.tick++;
            
            x = (float)(Math.Tan((this.tick / 1.5) * Math.PI / 180));
            x = x * -1.0f;
            y = (float)(Math.Sin((this.tick / 1.5) * Math.PI / 180) * 0.6f);
            y += 0.25f;
         
        }//moveImage

        /// <summary>
        /// Draw text to screen
        /// </summary>
        private void drawText()
        {
            text.Draw("Smurfar", Text2D.FontName.Coolfont, new Vector3(0.8f, 0.0f, 0.5f), new OpenTK.Vector2(0.10f, 0.10f), new OpenTK.Vector2(0.0f, 0.0f), 2.0f);
            text.Draw("Internet", Text2D.FontName.Coolfont, new Vector3(0.8f, -0.4f, 0.5f ), new OpenTK.Vector2(0.10f, 0.10f), new OpenTK.Vector2(0.0f, 0.0f), 2.0f);
        }

        /// <summary>
        /// Play sound
        /// </summary>
        /// <param name="Date">New date?</param>
        private void Play(String Date)
        {
            if ( LastDate != Date && snd.PlayingName() != "Smurf") // this will start once the last sound is done, ie looping.
            {
                snd.Play("Smurf");
                LastDate = Date;
            }
        }

        /// <summary>
        /// Change image ticks are over a set number
        /// </summary>
        public void updateImages()
        {
            ticks = System.DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            if (this.oldTicks != 0)
            {
                if ((this.ticks - this.oldTicks) > 4200)
                {
                    currentImage++;

                    if (currentImage > 4)
                        currentImage = 0;

                    oldTicks = ticks;
                }//inner if
            }//outer if

            if (oldTicks == 0)
                oldTicks = ticks;
        }

        /// <summary>
        /// Draw to screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            if (LastDate != Date)
            {
                currentImage = 0;
                ticks = oldTicks = 0;
            }

            Play(Date);
            drawText();
            moveImage();
            updateImages();
            DrawImage();
        }//Draw
    }//class
}//namespace 
