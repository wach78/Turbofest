﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace OpenGL
{
    /// <summary>
    /// Hajk effect
    /// </summary>
    class Hajk : IEffect
    {
        private bool disposed;
        private int image;
        private Sound snd;
        private string LastDate;

        /// <summary>
        /// Constructor for Hajk effect
        /// </summary>
        /// <param name="sound">Sound system</param>
        public Hajk(ref Sound sound)
        {
            image = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/Hajk.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            snd = sound;

            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/HAJK.ogg", "Hajk");
            disposed = false;
            LastDate = string.Empty;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Hajk()
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
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.6f, -1.0f, 1.0f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-0.6f, -1.0f, 1.0f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-0.6f, 0.2f, 1.0f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(0.6f, 0.2f, 1.0f); // top left 

            GL.End();
            GL.Disable(EnableCap.Texture2D);

        }//DrawImage

        /// <summary>
        /// Play sound
        /// </summary>
        /// <param name="Date">New date?</param>
        public void Play(string Date)
        {
            if (LastDate != Date && snd.PlayingName() != "Hajk") // this will start once the last sound is done, ie looping.
            {
                snd.Play("Hajk");
                LastDate = Date;
            }
        }

        /// <summary>
        /// Draw Hajk effect on screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            Play(Date);
            drawImage();
        }//Draw
    }//class
}//namespace
