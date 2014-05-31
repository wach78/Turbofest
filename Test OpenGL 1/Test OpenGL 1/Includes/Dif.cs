using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    /// <summary>
    /// Degerfors IF effect
    /// </summary>
    class Dif : IEffect
    {
        private Chess bakground;
        private Sound snd;
        private int image;
        private float x;
        private float y;
        private bool disposed = false;
        private int tick;
        private string LastDate;

        /// <summary>
        /// Constructor for Degerfors IF effect
        /// </summary>
        /// <param name="chess"></param>
        /// <param name="sound"></param>
        public Dif(ref Chess chess, ref Sound sound)
        {
            bakground = chess;
            x = 0.0f;
            y = 0.0f;
            image = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/dif2.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            snd = sound;
            tick = 0;
            LastDate = string.Empty;

            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/imperial.ogg", "Dif");
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Dif()
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
                    bakground = null;
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");

                disposed = true;
            }
        }

        /// <summary>
        /// Draw image on screen
        /// </summary>
        private void DrawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image);
            GL.Enable(EnableCap.Blend);       
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);  

            GL.Begin(BeginMode.Quads);
           
            // x y z
            // alla i mitten Y-led  alla till vänster x-led //
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(1.6f+x, -0.55f+y, 0.4f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(1.0f+x, -0.55f+y, 0.4f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(1.0f+x, 0.10f+y, 0.4);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(1.6f+x, 0.10f+y, 0.4f); // top left 

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

        }//DrawImage
   
        /// <summary>
        /// Move image
        /// </summary>
        private void moveImage()
        {                   
            this.tick++;

            x = (float)(Math.Tan((this.tick * 1.5) * Math.PI / 180));
            x -= 1.0f;
            y = (float)(Math.Cos((this.tick * 1.5) * Math.PI / 180) * 0.6f);
            y += 0.25f;
             
        }//moveImage

        /// <summary>
        /// Play sound
        /// </summary>
        /// <param name="Date">New date?</param>
        public void Play(String Date)
        {
            if (LastDate != Date && snd.PlayingName() != "Dif") 
            {
                snd.Play("Dif");
                LastDate = Date;
            }
        }

        /// <summary>
        /// Draw on to screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            Play(Date);
            bakground.Draw(Date, Chess.ChessColor.WhiteRed);
            moveImage();
            DrawImage();
            
        }//Draw
    }//class
}//namespace
