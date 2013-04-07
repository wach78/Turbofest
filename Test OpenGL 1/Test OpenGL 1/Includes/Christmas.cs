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

        private int currentImage;
        private Sound snd;

        private bool leftBorder;
        private bool rightBorder;
        private bool topBorder;
        private bool bottomBorder;

        private float x;
        private float y;
        private bool disposed = false;
        
         private SnowFlake[] sf;
         private const int NUMBEROFFLAKES = 150;
        
      

        public Christmas(ref Sound sound)
        {
            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\Xmas.bmp");
            image2 = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\godjul.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            snowImage = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\snow1_db.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));

            currentImage = 0;
            snd = sound;
            //snd.CreateSound(Sound.FileType.WAV, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\samples\\xmas.wav", "smurf");
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\samples\\xmas.ogg", "XMAS");

            leftBorder = true;
            rightBorder = false;
            topBorder = true;
            bottomBorder = false;

            x = 1;
            y = 0;
            
            Random r = new Random();
            sf = new SnowFlake[NUMBEROFFLAKES];

            for (int i = 0; i < NUMBEROFFLAKES; i++)
            {

                sf[i] = new SnowFlake((r.Next(-30, 40)) / 10.0f, (r.Next(-10, 20) * -1) / 10.0f, 0.00001f, r.Next(1, 10) / 1000.0f, snowImage,
                    new Vector2[] {  new Vector2(0.0f + (currentImage * 0.2f), 1.0f),
                                     new Vector2(0.2f + (currentImage * 0.2f), 1.0f),
                                     new Vector2(0.2f + (currentImage * 0.2f), 0.0f),
                                     new Vector2(0.0f + (currentImage * 0.2f), 0.0f)});


                currentImage++;

                if (currentImage == 4)
                    currentImage = 0;

                  
            }
             
                /*
                    new Vector3[] {new Vector3(0.0f,-0.1f,1.1f),
                                   new Vector3(-0.1f,-0.1f,1.1f),
                                   new Vector3(0.1f,0.0f,1.1f),
                                   new Vector3(0.0f,0.0f,1.1f)},
                 */
            
        }

        ~Christmas()
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
                   Util.DeleteTexture(ref image2);
                   Util.DeleteTexture(ref snowImage);
                   snd = null;
                   image = 0;
                   image2 = 0;
                   snowImage = 0;

                   for (int i = 0; i < NUMBEROFFLAKES; i++)
                   {
                       sf[i].Dispose();
                       sf[i] = null;
                   }
                }
                // free native resources if there are any.
                Console.WriteLine(this.GetType().ToString() + " disposed.");
                disposed = true;
            }
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
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.8f + x, -0.8f + y, 0.45f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-0.8f+ x, -0.8f + y, 0.45f ); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-.8f + x, -0.0f + y, 0.45f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(0.8f + x, -0.0f + y , 0.45f); // top left 

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

            
            for (int i = 0; i < NUMBEROFFLAKES; i++)
            {
                sf[i].Draw("");
            }
            

        }//DrawImage

        private void moveImage()
        {
            if (x < -0.8f)
            {
                rightBorder = true;
                leftBorder = false;
            }

            if (x > 0.8f)
            {
                leftBorder = true;
                rightBorder = false;
            }

            if (y > 1.20f)
            {
                topBorder = true;
                bottomBorder = false;
            }

            if (y < 0.0f && y < -0.48f)
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
        public void Play()
        {
            if (snd.PlayingName() != "XMAS") // this will start once the last sound is done, ie looping.
            {
                snd.Play("XMAS");
            }
        }
        public void Draw(string Date)
        {
            play();
            moveImage();
            drawImage();
            
        }//Draw
    }//class
}//namespace
