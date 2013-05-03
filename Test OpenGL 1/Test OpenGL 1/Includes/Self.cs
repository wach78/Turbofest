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
    class Self
    {
        private bool disposed = false;
        private int imageWach;
        private int imageKamikazE;

        private long tick;
        private float x;
        private float y;
        private int randomIamge;
        private string CurrentDate;
        private Sound snd;

        public Self(ref Sound sound)
        {

            imageWach = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/wach.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            imageKamikazE = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/kze.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            randomIamge = -1;
            SetRandomSelf();
            snd = sound;
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/samples/tetris.ogg", "Self");

            tick = 0;
            x = 0.0f;
            y = 0.0f;

            CurrentDate = string.Empty;
        }

        ~Self()
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
                    Util.DeleteTexture(ref imageWach);
                    Util.DeleteTexture(ref imageKamikazE);

                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");
                disposed = true;
            }
        }

        // this needs to be set to not load new textures each time... quick and dirty way now...
        private void SetRandomSelf()
        {
            if (randomIamge == 0)
                randomIamge = 1;
            else
                randomIamge = 0;
        }

        private void drawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, (randomIamge == 1? imageKamikazE:imageWach));
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(BeginMode.Quads);

         
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(-0.2f + x, -1.2f + y, 0.4f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-1.8f + x, -1.2f + y, 0.4f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-1.8f + x, -0.2f + y, 0.4f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(-0.2f + x, -0.2f + y, 0.4f); // top left 

            GL.End();

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

            this.tick++;



            x = (float)(0.004 * Math.Sin((this.tick / 50.0) * 3.1415) * 200);
            x += 1.0f;
            y = (float)(0.004 * Math.Sin((this.tick / 42.1) * 3.1415) * 150);
            y += 0.7f;




        }//DrawImage

        public void Play(string Date)
        {

            if (snd.PlayingName() != "Self")
            {    
                snd.Play("Self");
            }
        }


        public void Draw(string Date)
        {
            if (CurrentDate != Date)
            {
                CurrentDate = Date;
                SetRandomSelf();
                Play(Date);
            }
            drawImage();

        }//Draw
    }//class
}//namespace