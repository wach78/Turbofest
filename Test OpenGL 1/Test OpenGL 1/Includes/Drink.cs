using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
namespace OpenGL
{
    class Drink
    {
        private Sound snd;
        private bool disposed;
        private string LastDate;
        private Beer b;
        private int image;

        public Drink(ref Sound sound)
        {
            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/ibeer.png");
            snd = sound;

            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/Drink.ogg", "Drink");
            b = new Beer();
            disposed = false;
            LastDate = string.Empty;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Drink()
        {
            Dispose(false);
        }

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
                    b.Dispose();
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");
                disposed = true;
            }
        }
        private void DrawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image);
            GL.Enable(EnableCap.Blend); //       
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); //

            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led

            GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.8f, -0.8f, 1.0f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-0.6f, -0.8f, 1.0f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-0.6f, -0.00f, 1.0f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(0.8f, -0.00f, 1.0f); // top left 


            GL.End();

        }//DrawImage

        /// <summary>
        /// Play sound
        /// </summary>
        public void Play()
        {
            if (snd.PlayingName() != "Drink") // this will start once the last sound is done, ie looping.
            {
                snd.Play("Drink");
            }
        }
        public void Draw(string Date)
        {
            Play();
            DrawImage();
            b.Draw(Date);
        }//Draw

    }//class

    
}//namespace
