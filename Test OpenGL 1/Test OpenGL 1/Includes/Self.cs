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
    /// Self/Creator effect
    /// </summary>
    class Self : IEffect
    {
        private bool disposed = false;
        private int imageWach;
        private int imageKamikazE;
        private long tick;
        private float x;
        private float y;
        private int randomImage;
        private string CurrentDate;
        private Sound snd;

        /// <summary>
        /// Constructor for Self effect
        /// </summary>
        /// <param name="sound">Sound system</param>
        public Self(ref Sound sound)
        {

            imageWach = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/wach.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            imageKamikazE = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/kze.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            randomImage = (Util.Rnd.Next(0, 100)<50? 0:1);
            snd = sound;
            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/Tetris.ogg", "Self");

            tick = 0;
            x = 0.0f;
            y = 0.0f;

            CurrentDate = string.Empty;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Self()
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
                    Util.DeleteTexture(ref imageWach);
                    Util.DeleteTexture(ref imageKamikazE);
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");
                disposed = true;
            }
        }

        // this needs to be set to not load new textures each time... quick and dirty way now...
        /// <summary>
        /// Change image
        /// </summary>
        private void SetSelf()
        {
            if (randomImage == 0)
                randomImage = 1;
            else
                randomImage = 0;
        }

        /// <summary>
        /// Draw image to screen
        /// </summary>
        private void drawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, (randomImage == 1 ? imageKamikazE : imageWach));
            GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-0.2f + x, -1.2f + y, 0.4f); // bottom left  
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.8f + x, -1.2f + y, 0.4f); // bottom right 
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-1.8f + x, -0.2f + y, 0.4f);// top right
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-0.2f + x, -0.2f + y, 0.4f); // top left 
            GL.End();

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

            this.tick++;
            x = (float)(0.004 * Math.Sin((this.tick / 50.0) * 3.1415) * 200);
            x += 1.0f;
            y = (float)(0.004 * Math.Sin((this.tick / 42.1) * 3.1415) * 150);
            y += 0.7f;
        }//DrawImage

        /// <summary>
        /// Play sound
        /// </summary>
        public void Play()
        {
            if (snd.PlayingName() != "Self")
            {
                snd.Play("Self");
            }
        }

        /// <summary>
        /// Draw Self effect on screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            if (CurrentDate != Date)
            {
                CurrentDate = Date;
                SetSelf();
                Play();
            }
            drawImage();

        }//Draw
    }//class
}//namespace
