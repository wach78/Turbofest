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
    /// Fuck me gently effect
    /// </summary>
    class Fuck : IEffect
    {
        private Sound snd;
        private Chess bakground;
        private int img;
        private bool disposed = false;
        private string LastDate;

        /// <summary>
        /// Constructor for fuck me gently effect
        /// </summary>
        /// <param name="sound">Sound system</param>
        /// <param name="chess">Chessboard</param>
        public Fuck(ref Sound sound,ref Chess chess)
        {
            img = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/fuck.jpg");
            bakground = chess;
            snd = sound;
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/Fuck.ogg", "Fuck");
            LastDate = string.Empty;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Fuck()
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
            if (LastDate != Date && snd.PlayingName() != "Fuck") // this will start once the last sound is done, ie looping.
            {
                snd.Play("Fuck");
                LastDate = Date;
            }
        }

        /// <summary>
        /// Draw Fuck me gently effect on screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            Play(Date);
            bakground.Draw(Date, Chess.ChessColor.Heart);
            DrawImage();
        }//Draw
    }//class
}//namespace
