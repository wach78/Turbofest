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
    /// Talespin effect
    /// </summary>
    class Talespin : IEffect
    {
        private Sound snd;
        private int seaDuck;
        private float x;
        private float y;
        private bool disposed = false;
        private string LastDate;
        private long tick;
 
        /// <summary>
        /// Constructor for Talespin effect
        /// </summary>
        /// <param name="sound">Sound system</param>
        public Talespin(ref Sound sound)
        {
            x = -1.0f;
            y = 0.0f;
            seaDuck = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/seaDuck.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));

            snd = sound;
            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/Talespin.ogg", "Talespin");
            LastDate = string.Empty;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Talespin()
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
                    Util.DeleteTexture(ref seaDuck);
                    //Util.DeleteTexture(ref piratePlane);
                    
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");

                disposed = true;
            }
        }

        /// <summary>
        /// Update positions
        /// </summary>
        private void moveImage()
        {
            this.tick++;

            x = (float)(Math.Tan((this.tick / 1.5) * Math.PI / 180) );
            x = x * -1.0f;
            y = (float)(Math.Sin((this.tick / 1.5) * Math.PI / 180) * 0.6f);
            y += 0.25f;

        }//moveImage

        /// <summary>
        /// Draw image to screen
        /// </summary>
        private void DrawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, seaDuck);
            GL.Enable(EnableCap.Blend); //       
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); //

            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0 , 1.0); GL.Vertex3(2.0f + x, -0.8f + y, 0.4f); // bottom left  
            GL.TexCoord2(1.0  , 1.0); GL.Vertex3(1.1f + x, -0.8f + y, 0.4f); // bottom right 
            GL.TexCoord2(1.0  , 0.0); GL.Vertex3(1.1f + x, 0.0f + y, 0.4f);// top right
            GL.TexCoord2(0.0  , 0.0); GL.Vertex3(2.0f + x, 0.0f + y, 0.4f); // top left 

            //GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.8f, -0.8f, 1.0f); // bottom left  
            //GL.TexCoord2(1.0, 1.0); GL.Vertex3(-0.8f, -0.8f, 1.0f); // bottom right 
            //GL.TexCoord2(1.0, 0.0); GL.Vertex3(-0.8f, -0.00f, 1.0f);// top right
            //GL.TexCoord2(0.0, 0.0); GL.Vertex3(0.8f, -0.00f, 1.0f); // top left 


            GL.End();
            GL.Disable(EnableCap.Blend);//
            GL.Disable(EnableCap.Texture2D);


        }//DrawImage

        /// <summary>
        /// Play sound
        /// </summary>
        /// <param name="Date">New date?</param>
        private void Play(String Date)
        {
            if (LastDate != Date && snd.PlayingName() != "Talespin") // this will start once the last sound is done, ie looping.
            {
                snd.Play("Talespin");
                LastDate = Date;
            }
        }

        /// <summary>
        /// Draw Talespin effect on screen
        /// </summary>
        /// <param name="Date"></param>
        public void Draw(string Date)
        {
          
            Play(Date);
            moveImage();
            DrawImage();
        }//Draw
    }//class
}//namespace
