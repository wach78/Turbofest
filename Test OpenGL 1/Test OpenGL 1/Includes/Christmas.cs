﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace OpenGL
{
    /// <summary>
    /// Christmas effect
    /// </summary>
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
        private string LastDate;

        /// <summary>
        /// Constructor for Christmas effect
        /// </summary>
        /// <param name="sound">Sound system</param>
        public Christmas(ref Sound sound)
        {
            image = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/xmas.bmp");
            image2 = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/godjul.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            snowImage = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/snow1_db.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));

            currentImage = 0;
            snd = sound;
            //snd.CreateSound(Sound.FileType.WAV, Util.CurrentExecutionPath + "/Samples/xmas.wav", "smurf");
            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/xmas.ogg", "XMAS");

            LastDate = string.Empty;
            tick = 0;

            x = 1;
            y = 0;
            
            //Random r = new Random();
            sf = new SnowFlake[NUMBEROFFLAKES];

            float z = 0.4f;

            for (int i = 0; i < NUMBEROFFLAKES; i++)
            {

                sf[i] = new SnowFlake((Util.Rnd.Next(-18, 15)) / 10.0f, (Util.Rnd.Next(-10, 20) * -1) / 10.0f, Util.Rnd.Next(2, 8) / 1000.0f, snowImage,
                    new Vector2[] {  new Vector2(0.0f + (currentImage * 0.2f), 1.0f),
                                     new Vector2(0.2f + (currentImage * 0.2f), 1.0f),
                                     new Vector2(0.2f + (currentImage * 0.2f), 0.0f), 
                                     new Vector2(0.0f + (currentImage * 0.2f), 0.0f)}, Util.Rnd.Next(5, 10) * 10.0f, z);


                z -= 0.00001f;
                currentImage++;

                if (currentImage == 4)
                    currentImage = 0;
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Christmas()
        {
            Dispose(false);
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
                   Util.DeleteTexture(ref image);
                   Util.DeleteTexture(ref image2);
                   Util.DeleteTexture(ref snowImage);

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

        /// <summary>
        /// Draw image to screen
        /// </summary>
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

        /// <summary>
        /// Move image
        /// </summary>
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

        /// <summary>
        /// Play sound
        /// </summary>
        /// <param name="Date">New date?</param>
        public void Play(String Date)
        {
            if (LastDate != Date && snd.PlayingName() != "XMAS") 
            {
                snd.Play("XMAS");
                LastDate = Date;
            }
        }

        /// <summary>
        /// Draw to screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            Play(Date);
            moveImage();
           
            drawImage();
            
        }//Draw
    }//class
}//namespace
