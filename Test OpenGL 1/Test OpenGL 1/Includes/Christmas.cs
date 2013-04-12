using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace OpenGL
{
    class Christmas : IEffect
    {
        private int image;
        private int image2;
        private int snowImage;

        private int currentImage;
        private Sound snd;

        private float x;
        private float y;
        private bool disposed = false;
        private int tick;
        
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


            tick = 0;

            x = 1;
            y = 0;
            
            Random r = new Random();
            sf = new SnowFlake[NUMBEROFFLAKES];

            float z = 0.4f;

            for (int i = 0; i < NUMBEROFFLAKES; i++)
            {

                sf[i] = new SnowFlake((r.Next(-18, 15)) / 10.0f, (r.Next(-10, 20) * -1) / 10.0f, r.Next(2, 8) / 1000.0f, snowImage,
                    new Vector2[] {  new Vector2(0.0f + (currentImage * 0.2f), 1.0f),
                                     new Vector2(0.2f + (currentImage * 0.2f), 1.0f),
                                     new Vector2(0.2f + (currentImage * 0.2f), 0.0f), 
                                     new Vector2(0.0f + (currentImage * 0.2f), 0.0f)},  r.Next(5, 10) * 10.0f, z);


                z -= 0.00001f;
                currentImage++;

                if (currentImage == 4)
                    currentImage = 0;

                  
            }
             
           
            
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

                   for (int i = 0; i < NUMBEROFFLAKES; i++)
                   {
                       sf[i].Dispose();
                       sf[i] = null;
                   }
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");
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

            this.tick++;
            /*
            x = (float)(Math.Sin((this.tick * 1.5) * Math.PI / 180)) * 0.8f;
            x += 0.0f;
            y = (float)(Math.Sin((this.tick * 1.5) * Math.PI / 180) * 0.5f);
            y += 0.34f;
            */
            x = (float)(0.004 * Math.Sin((this.tick / 50.0) * 3.1415) * 200);
             
            y = (float)(0.004 * Math.Sin((this.tick / 42.1) * 3.1415) * 150);
            y += 0.37f;

        }//moveImage

        public void Play()
        {
            if (snd.PlayingName() != "XMAS") // this will start once the last sound is done, ie looping.
            {
                snd.Play("XMAS");
            }
        }
        public void Draw(string Date)
        {
            Play();
            moveImage();
           
            drawImage();
            
        }//Draw
    }//class
}//namespace
