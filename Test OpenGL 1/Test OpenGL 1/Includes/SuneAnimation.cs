using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    class SuneAnimation : IEffect
    {
        private int image;

        private SuneTxtHandler sh;

        private long ticks;
        private long oldTicks;
        private long soundTicks;
        private long soundOldTicks;
        private int currentImage;

        private Sound snd;
        //private Text2D text;
        private bool soundTrue = false;
        private bool soundDone = false;
        private bool disposed = false;
        //private short soundTimes = 0;
        private string LastPlayedDate;

        public SuneAnimation(ref Sound sound, ref Text2D Text)
        {
            ticks = 0;
            oldTicks = 0;
            soundOldTicks = 0;
            soundOldTicks = 0;
            currentImage = 0;
            sh = new SuneTxtHandler(ref Text, false);
            snd = sound;
            //text = Text;
            LastPlayedDate = string.Empty;
            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/sune_sprite.bmp");
            //snd.CreateSound(Sound.FileType.WAV, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/laugh.wav", "Sune");
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/laugh.ogg", "Sune");
            
        }

        ~SuneAnimation()
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
                    Util.DeleteTexture(ref image);
                    sh.Dispose();
                    sh = null;
                    currentImage = 0;
                }
                // free native resources if there are any.
                System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
                disposed = true;
            }
        }

        public void DrawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0.0f + (currentImage * 0.25f), 1.0f); GL.Vertex3(2.2f, -1.15f, 1.0f ); // bottom left // x y z alla i mitten Y-led 
            GL.TexCoord2(0.25f + (currentImage * 0.25f), 1.0f); GL.Vertex3(1.0f, -1.15f, 1.0f); // bottom right // alla till vänster x-led
            GL.TexCoord2(0.25f + (currentImage * 0.25f), 0.0f); GL.Vertex3(1.0f, 0.10f, 1.0f);// top right
            GL.TexCoord2(0.0f + (currentImage * 0.25f), 0.0f); GL.Vertex3(2.2f, 0.10f, 1.0f); // top left 

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

        }

        public void stopSample()
        {
            soundTicks = System.DateTime.Now.Ticks / TimeSpan.TicksPerSecond;

            if (this.soundOldTicks != 0)
            {
                if ((this.soundTicks - this.soundOldTicks) > 5 && soundTrue)
                {
                    soundTrue = false;
                    if (snd.PlayingName() != "Sune")
                    {
                        snd.Play("Sune");
                    }
                }// if
            }//outer if

            if (soundOldTicks == 0)
                soundOldTicks = soundTicks;

        }

        private void random(string Date)
        {
            if (LastPlayedDate != Date)
            {
                NewQoute();
                LastPlayedDate = Date;
            }

        }

        public void updateImages()
        {
            ticks = System.DateTime.Now.Ticks  / TimeSpan.TicksPerMillisecond; 

            if (this.oldTicks != 0)
            {
                if ((this.ticks - this.oldTicks) > 30)
                {
                    currentImage++;

                    if (currentImage > 3)
                        currentImage = 0;

                    oldTicks = ticks;
                }//inner if
            }//outer if

            if (oldTicks == 0)
                oldTicks = ticks;
           
        }

        public void playSound()
        {
            if (!soundTrue && !soundDone)
            {
                if (snd.PlayingName() != "Sune")
                {
                    snd.Play("Sune");
                }
                soundTrue = true;
                soundDone = true;
            }
           // stopSample();
        }

        public void NewQoute()
        {
            sh.drawInit(false);
            soundTrue = false;
            soundDone = false;
        }

        public void Draw(string Date)
        {
            random(Date);
            playSound();
            updateImages();
            DrawImage();
            sh.Draw(Date);
        }

    }//class
}//namespace
