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
    /// Sailormoon effect
    /// </summary>
    class Sailormoon : IEffect
    {
        private Sound snd;
        private Chess bakground;
        private int img;
        private bool disposed = false;
        private string LastDate;

        /// <summary>
        /// Constructor for Sailormoon effect
        /// </summary>
        /// <param name="sound">Sound system</param>
        /// <param name="chess">Chessboard</param>
        public Sailormoon(ref Sound sound,ref Chess chess)
        {
            img = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/sailormoon.jpg");

            bakground = chess;
            snd = sound;
            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/SailorJap.ogg", "Moon");
            LastDate = string.Empty;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Sailormoon()
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
                    Util.DeleteTexture(ref img);
                    //Util.DeleteTexture(ref piratePlane);
                    
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
            GL.BindTexture(TextureTarget.Texture2D, img);
            GL.Enable(EnableCap.Blend); //       
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); //

            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led

            GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.8f, -0.8f, 1.0f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-0.8f, -0.8f, 1.0f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-0.8f, -0.00f, 1.0f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(0.8f, -0.00f, 1.0f); // top left 


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
            if (LastDate != Date && snd.PlayingName() != "Moon") // this will start once the last sound is done, ie looping.
            {
                snd.Play("Moon");
                LastDate = Date;
            }
        }

        /// <summary>
        /// Draw Sailormoon effect on screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            Play(Date);
            bakground.Draw(Date, Chess.ChessColor.BlackWhite);
            DrawImage();
        }//Draw
    }//class
}//namespace 
