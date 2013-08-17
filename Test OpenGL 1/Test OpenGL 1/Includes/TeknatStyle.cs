using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace OpenGL
{
    class TeknatStyle : IEffect
    {
        private int slideshowImage1;
        private int slideshowImage2;
        private int slideshowImage3;
        private int currentImage;
        private int currentSlideShow;
        private Sound snd;
        private Chess bakground;
        private Text2D text;

        private bool disposed;

        private string LastDate;
        private long ticks;
        private long oldTicks;

        public TeknatStyle(ref Chess chess, ref Sound sound, ref Text2D txt)
        {
            disposed = false;
            slideshowImage1 = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/bildspel1.jpg");
            slideshowImage2 = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/bildspel2.jpg");
            slideshowImage3 = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/bildspel3.jpg");
            snd = sound;
            bakground = chess;
            text = txt;

            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/ts.ogg", "TS");
            currentImage = 0;
            currentSlideShow = 0;

            LastDate = string.Empty;
            ticks = 0;
            oldTicks = 0;
        }

       ~TeknatStyle()
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
                    Util.DeleteTexture(ref slideshowImage1);
                    Util.DeleteTexture(ref slideshowImage2);
                    Util.DeleteTexture(ref slideshowImage3);

                    currentImage = 0;
                    currentSlideShow = 0;
                    ticks = 0;
                    oldTicks = 0;
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");

                disposed = true;
            }
        }


        public void DrawImage()
        {
            GL.Enable(EnableCap.Texture2D);

            if (currentSlideShow == 0)
            {
                GL.BindTexture(TextureTarget.Texture2D, slideshowImage1);
            }
            else if (currentSlideShow == 1)
            {
                GL.BindTexture(TextureTarget.Texture2D, slideshowImage2);
            }
            else if (currentSlideShow == 2)
            {
                GL.BindTexture(TextureTarget.Texture2D, slideshowImage3);
            }

            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0.0f + (currentImage * 0.25f), 1.0f); GL.Vertex3(1.0f, -1.25f, 1.0f); // bottom left // x y z alla i mitten Y-led 
            GL.TexCoord2(0.25f + (currentImage * 0.25f), 1.0f); GL.Vertex3(-1.0f, -1.25f, 1.0f); // bottom right // alla till vänster x-led
            GL.TexCoord2(0.25f + (currentImage * 0.25f), 0.0f); GL.Vertex3(-1.0f, 0.0f, 1.0f);// top right
            GL.TexCoord2(0.0f + (currentImage * 0.25f), 0.0f); GL.Vertex3(1.0f, 0.0f, 1.0f); // top left 

            GL.End();
            GL.Disable(EnableCap.Texture2D);

        }

        public void Play(string Date)
        {
            if (LastDate != Date && snd.PlayingName() != "TS")
            {
                snd.Play("TS");
                LastDate = Date;
            }
        }

        public void updateImages()
        {
            ticks = System.DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;


            if (this.oldTicks != 0)
            {
                if ((this.ticks - this.oldTicks) > 2000)
                {
                    currentImage++;

                    if (currentImage > 3)
                    {
                        currentImage = 0;
                        currentSlideShow++;
                    }

                    if (currentSlideShow > 2)
                        currentSlideShow = 0;


                    oldTicks = ticks;
                }//inner if
            }//outer if

            if (oldTicks == 0)
                oldTicks = ticks;

         

        }

        public void Draw(string Date)
        {
            if (LastDate != Date)
            {
                currentImage = 0;
                currentSlideShow = 0;
            }

            Play(Date);
            bakground.Draw(Date, Chess.ChessColor.BlackPurple);
            text.Draw("TekNat Style", Text2D.FontName.CandyPurple, new OpenTK.Vector3(0.9f, 0.2f, 1.0f ), new OpenTK.Vector2(0.10f, 0.10f), new OpenTK.Vector2(0.0f, 0.0f), 1.5f);
            updateImages();
            DrawImage();
        }

    }//class
}//namespace
