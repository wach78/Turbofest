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
        private bool soundTrue = false;
        private bool soundDone = false;
        private short soundTimes = 0;

        public SuneAnimation(ref Sound sound)
        {
            ticks = 0;
            oldTicks = 0;
            soundOldTicks = 0;
            soundOldTicks = 0;
            currentImage = 0;
            sh = new SuneTxtHandler();
            snd = sound;

            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\sune_sprite.bmp");
            snd.CreateSound(Sound.FileType.WAV, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\samples\\laugh.wav", "Sune");
            
        }

        public void Dispose()
        {
            //base.Finalize();
            Util.DeleteTexture(ref currentImage);
            snd = null;
            System.GC.SuppressFinalize(this);
        }

        public void DrawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0.0 + (currentImage * 0.25), 1.0); GL.Vertex3(1.8f, -0.85f, 1.0f ); // bottom left // x y z alla i mitten Y-led 
            GL.TexCoord2(0.25 + (currentImage * 0.25), 1.0); GL.Vertex3(1.0f, -0.85f, 1.0f); // bottom right // alla till vänster x-led
            GL.TexCoord2(0.25 + (currentImage * 0.25), 0.0); GL.Vertex3(1.0f, 0.10f, 1.0f);// top right
            GL.TexCoord2(0.0 + (currentImage * 0.25), 0.0); GL.Vertex3(1.8f, 0.10f, 1.0f); // top left 

            GL.End();
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
            stopSample();
        }

        public void Draw(string Date)
        {
            playSound();
            updateImages();
            DrawImage();
            sh.Draw(Date);
        }

    }//class
}//namespace
