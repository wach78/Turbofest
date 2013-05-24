using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    
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

        public National(ref Chess chess, ref Sound sound)
        {
            ticks = 0;
            oldTicks = 0;
        
            currentImage = 0;
            bakground = chess;
            snd = sound;
            LastDate = string.Empty;

            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/flagga.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/Du gamla du fria (Black Ingvars).ogg", "National");
        }

        ~National()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // free managed resources
                    Util.DeleteTexture(ref currentImage);
                    snd = null;
                    currentImage = 0;
                    image = 0;
                }
                // free native resources if there are any.
                System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
                disposed = true;
            }
        }

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


        public void Play(String Date)
        {
            if (LastDate != Date && snd.PlayingName() != "National")
            {
                snd.Play("National");
                LastDate = Date;
            }
        }

        public void Draw(string Date)
        {
            Play(Date);
            bakground.Draw(Date, Chess.ChessColor.WhiteRed);
            updateImages();
            DrawImage();

        }//Draw
    }//class
}//namespace
