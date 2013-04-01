using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    class Christmas : IEffect
    {
        private int image;
        private int image2;
        private int snowImage;

        //private int currentImage;
        private Sound snd;

        private bool leftBorder;
        private bool rightBorder;
        private bool topBorder;
        private bool bottomBorder;

        private float x;
        private float y;

        /*
         private SnowFlake[] sf;
         private const int NUMBEROFFLAKES = 13;
        */
      

        public Christmas(ref Sound sound)
        {
            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\Xmas.bmp");
            image2 = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\godjul.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            snowImage = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\snow1_db.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));

           // currentImage = 0;
            snd = sound;
            snd.CreateSound(Sound.FileType.WAV, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\samples\\datasmurf.wav", "smurf");
            leftBorder = true;
            rightBorder = false;
            topBorder = true;
            bottomBorder = false;

            x = 1;
            y = 0;
            /*
            Random r = new Random();
            sf = new SnowFlake[NUMBEROFFLAKES];

            for (int i = 0; i < NUMBEROFFLAKES; i++)
            {



                sf[i] = new SnowFlake((r.Next(-10, 20) * -1)/10.0f, 0.002f, 0.02f, -0.4f, 0.5f, snowImage,
                    new Vector2[] {  new Vector2(0.0f + (currentImage * 0.2f), 1.0f),
                                     new Vector2(0.2f + (currentImage * 0.2f), 1.0f),
                                     new Vector2(0.2f + (currentImage * 0.2f), 0.0f),
                                     new Vector2(0.0f + (currentImage * 0.2f), 0.0f)});


                currentImage++;

                if (currentImage == 4)
                    currentImage = 0;

                  
            }
             */
                /*
                    new Vector3[] {new Vector3(0.0f,-0.1f,1.1f),
                                   new Vector3(-0.1f,-0.1f,1.1f),
                                   new Vector3(0.1f,0.0f,1.1f),
                                   new Vector3(0.0f,0.0f,1.1f)},
                 */
            snd.CreateSound(Sound.FileType.WAV, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\samples\\xmas.wav", "xmas");
        }

        public void Dispose()
        {
            //base.Finalize();
            Util.DeleteTexture(ref image);
            Util.DeleteTexture(ref image2);
            Util.DeleteTexture(ref snowImage);
            this.image = -1;
            this.image2 = -1;
            this.snowImage = -1;
            snd = null;
            System.GC.SuppressFinalize(this);
        }

        private void drawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image);
            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.8f, -1.2f, 0.8f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-0.8f, -1.2f, 0.8f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-.8f, 0.0f, 0.8f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(0.8f, 0.0f, 0.8f); // top left 

            GL.End();
            
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image2);
            GL.Enable(EnableCap.Blend);      
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); 
            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.8f + x, -0.8f + y, 1.0f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-0.8f+ x, -0.8f + y, 1.0f ); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-.8f + x, -0.0f + y, 1.0f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(0.8f + x, -0.0f + y , 1.0f); // top left 

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

            /*
            for (int i = 0; i < NUMBEROFFLAKES; i++)
            {
                sf[i].Draw();
            }
            */

        }//DrawImage

        private void moveImage()
        {
            if (Math.Round(x, 2) < -0.8)
            {
                rightBorder = true;
                leftBorder = false;
            }

            if (Math.Round(x, 2) > 0.8)
            {
                leftBorder = true;
                rightBorder = false;
            }

            if (Math.Round(y, 2) > 1.20)
            {
                topBorder = true;
                bottomBorder = false;
            }

            if (Math.Round(y, 2) < 0.0 && Math.Round(y, 2) < -0.48)
            {
                bottomBorder = true;
                topBorder = false;
            }

            if (!topBorder)
            {
                y += 0.020f;
            }

            if (!bottomBorder)
            {
                y -= 0.020f;
            }

            if (!rightBorder)
            {
                x -= 0.020f;
            }

            if (!leftBorder)
            {
                x += 0.020f;
            }

        }//moveImage
        public void play()
        {
            //player.Play();
        }
        public void Stop()
        {
            //player.Stop();
        }
        public void Draw(string Date)
        {
            moveImage();
            drawImage();
        }//Draw
    }//class
}//namespace
