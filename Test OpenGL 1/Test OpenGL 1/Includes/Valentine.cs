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
    class Valentine : IEffect
    {
        private int image;
        private int image2;
        private int heartsImage; 
        private Sound snd;

        private bool disposed = false;

        private int currentImage;
        private Hearts[] h;
        private const int NUMBEROFHEARTS = 100;

        public Valentine(ref Sound sound)
        {
            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/valentine.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            image2 = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/barseback3.png", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            heartsImage = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/h.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            currentImage = 0;
            snd = sound;
            //snd.CreateSound(Sound.FileType.WAV, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/valentine.wav", "Valentine");
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/valentine.ogg", "Valentine");
         
            Random r = new Random();
            h = new Hearts[NUMBEROFHEARTS];

            float z = 0.4f;

            for (int i = 0; i < NUMBEROFHEARTS; i++)
            {



                h[i] = new Hearts((r.Next(-18, 15)) / 10.0f, (r.Next(-10, 20) * -1) / 10.0f, r.Next(2, 8) / 1000.0f, heartsImage,
                    new Vector2[] {  new Vector2(0.0f + (currentImage * 0.2f), 1.0f),
                                     new Vector2(0.2f + (currentImage * 0.2f), 1.0f),
                                     new Vector2(0.2f + (currentImage * 0.2f), 0.0f),
                                     new Vector2(0.0f + (currentImage * 0.2f), 0.0f)}, r.Next(5, 10) * 10.0f, z);

                z -= 0.000001f;
                currentImage++;

                if (currentImage == 4)
                    currentImage = 0;
                
            }//for
             
        }

        ~Valentine()
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
                    Util.DeleteTexture(ref heartsImage);
                    snd = null;

                    for (int i = 0; i < NUMBEROFHEARTS; i++)
                    {
                        h[i].Dispose();
                        h[i] = null;
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
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.0f, -1.1f, 0.8f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-1.6f, -1.1f, 0.8f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-1.6f, 0.1f, 0.8f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(0.0f, 0.1f, 0.8f); // top left 

            GL.End();

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image2);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(1.8f, -1.0f, 1.0f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(0.2f, -1.0f, 1.0f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(0.2f, -0.0f , 1.0f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(1.8f, -0.0f, 1.0f); // top left 

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);


            for (int i = 0; i < NUMBEROFHEARTS; i++)
            {
                h[i].Draw("");
            }
      

        }//DrawImage

        public void Play()
        {
            if (snd.PlayingName() != "Valentine") // this will start once the last sound is done, ie looping.
            {
                snd.Play("Valentine");
            }
        }
        public void Draw(string Date)
        {
            Play();
            drawImage();
        }//Draw
    }//class
}//namespace
