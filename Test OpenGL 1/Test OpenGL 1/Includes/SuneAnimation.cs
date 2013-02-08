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
    class SuneAnimation :IDisposable
    {
       // private int[] images;
        private int image;
        private System.Media.SoundPlayer player;

        private SuneTxtHandler sh;

        private long ticks;
        private long oldTicks;
        private long soundTicks;
        private long soundOldTicks;
        private int currentImage;

        private bool soundTrue = false;
        private bool soundDone = false;

        public SuneAnimation()
        {
            ticks = 0;
            oldTicks = 0;
            soundOldTicks = 0;
            soundOldTicks = 0;
            currentImage = 0;
           // images = new int[4];
            player = new System.Media.SoundPlayer();
     //       sh = new SuneTxtHandler();
            /*
            images[0] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\sune0.bmp");
            images[1] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\sune1.bmp");
            images[2] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\sune2.bmp");
            images[3] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\sune3.bmp");
            */

            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\sune_sprite.bmp");

            player.SoundLocation = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\samples\\laugh.wav";
            
        }

        public void Dispose()
        {
            //base.Finalize();
            GL.DeleteBuffers(1, ref currentImage);
            this.currentImage = -1;
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
                    player.Stop();
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
                player.PlayLooping();
                soundTrue = true;
                soundDone = true;
            }
            stopSample();
        }

        public void Draw()
        {
            playSound();
            updateImages();
            DrawImage();
        //    sh.draw();
        }

    }//class
}//namespace
