using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
//using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace OpenGL
{
    /// <summary>
    /// Advent event.
    /// </summary>
    class Advent : IEffect
    {
        /// <summary>
        /// Enum for what advent it is, first to fourth advent.
        /// </summary>
        public enum WhatAdvent { First = 1, Second, Third, Fourth };

        private bool disposed = false;
        private Sound snd;
        private int texture;
        private Vector3[] Ghost;
        private SizeF Size;
        private float Speed;
        private float X;
        private float Y;
        private float Z;
        private long tick = 0;

        /// <summary>
        /// Constructor for Advent effect
        /// </summary>
        /// <param name="sound">The sound system to play sounds</param>
        public Advent(ref Sound sound)
        {
            snd = sound;
            texture = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/ljus.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(244,143,143));
            //snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/lucia.ogg", "lucia"); 

            Ghost = new Vector3[4];
            Size = new SizeF(0.4f, 0.8f);
            Speed = 0.0025f;

            X = 0.0f - Size.Width / 2.0f;
            Y = -0.5f - Size.Height / 2.0f;
            Z = 0.45f;
            Ghost[0] = new Vector3(X, Y, Z); // red
            Ghost[1] = new Vector3(X, Y + Size.Height, Z); // blue
            Ghost[2] = new Vector3(X + Size.Width, Y + Size.Height, Z); // green
            Ghost[3] = new Vector3(X + Size.Width, Y, Z); // yellow

        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Advent()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            //base.Finalize();
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing">Is it diposing?</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // free managed resources
                    Util.DeleteTexture(ref texture);
                }
                // free native resources if there are any.
                disposed = true;
                System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
            }
        }

        /// <summary>
        /// Move the image on the screen
        /// </summary>
        public void Move()
        {
            //kze: fix new movment pathern...
            tick++;
            //float mx = (1.65f - Size.Width);
            /*X = (tick / 80.0f) % (mx * 2.0f);
            if (X >= mx)
            {
                X = mx - (X - mx);
            }*/
            X += Speed;
            if (X >= (1.65f - Size.Width))
            {
                Speed = -Speed;
            }
            else if (X <= -1.65f)
            {
                Speed = -Speed;
            }

            Y = (float)Math.Sin(tick / 42.1f) * 0.3f - Size.Height * 0.75f;

            Ghost[0].Xy = new Vector2(X, Y);
            Ghost[1].Xy = new Vector2(X, Y + Size.Height);
            Ghost[2].Xy = new Vector2(X + Size.Width, Y + Size.Height);
            Ghost[3].Xy = new Vector2(X + Size.Width, Y);
        }

        /// <summary>
        /// Draw Advent effect on screen
        /// </summary>
        /// <param name="Date"></param>
        public void Draw(string Date)
        {
            Draw(Date, WhatAdvent.First);
        }

        /// <summary>
        /// Draw Advent effect on screen
        /// </summary>
        /// <param name="Date">What is the date</param>
        /// <param name="adventnumber">Enum what advent number is it</param>
        public void Draw(string Date, WhatAdvent adventnumber)
        {
            // Spiders that draws after eachother and end ontop durring run will be hidden...

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.BindTexture(TextureTarget.Texture2D, texture);

            GL.Begin(BeginMode.Quads);
            // Fix this to be dynamic, I don't have the feeling for it currently...
            if (adventnumber == WhatAdvent.First)
            {
                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(X, Y, Z); // bottom right 
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(X, Y + Size.Height, Z); // Top right
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(X + Size.Width, Y + Size.Height, Z);// top left
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(X + Size.Width, Y, Z); // bottom left
            }
            else if (adventnumber == WhatAdvent.Second)
            {

                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(X - Size.Width, Y, Z); // bottom right 
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(X - Size.Width, Y + Size.Height, Z); // Top right
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(X , Y + Size.Height, Z);// top left
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(X , Y, Z); // bottom left

                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(X + Size.Width, Y, Z); // bottom right 
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(X + Size.Width, Y + Size.Height, Z); // Top right
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(X + Size.Width*2, Y + Size.Height, Z);// top left
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(X + Size.Width*2, Y, Z); // bottom left
            }
            else if (adventnumber == WhatAdvent.Third)
            {
                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(X, Y, Z); // bottom right 
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(X, Y + Size.Height, Z); // Top right
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(X + Size.Width, Y + Size.Height, Z);// top left
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(X + Size.Width, Y, Z); // bottom left

                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(X - Size.Width*2, Y, Z); // bottom right 
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(X - Size.Width*2, Y + Size.Height, Z); // Top right
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(X - Size.Width, Y + Size.Height, Z);// top left
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(X - Size.Width, Y, Z); // bottom left

                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(X + Size.Width*2, Y, Z); // bottom right 
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(X + Size.Width*2, Y + Size.Height, Z); // Top right
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(X + Size.Width * 3, Y + Size.Height, Z);// top left
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(X + Size.Width * 3, Y, Z); // bottom left
            }
            else 
            {

                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(X - Size.Width*3, Y, Z); // bottom right 
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(X - Size.Width*3, Y + Size.Height, Z); // Top right
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(X - Size.Width*2, Y + Size.Height, Z);// top left
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(X - Size.Width*2, Y, Z); // bottom left

                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(X - Size.Width, Y, Z); // bottom right 
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(X - Size.Width, Y + Size.Height, Z); // Top right
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(X, Y + Size.Height, Z);// top left
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(X, Y, Z); // bottom left

                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(X + Size.Width, Y, Z); // bottom right 
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(X + Size.Width, Y + Size.Height, Z); // Top right
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(X + Size.Width * 2, Y + Size.Height, Z);// top left
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(X + Size.Width * 2, Y, Z); // bottom left

                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(X + Size.Width*3, Y, Z); // bottom right 
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(X + Size.Width*3, Y + Size.Height, Z); // Top right
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(X + Size.Width * 4, Y + Size.Height, Z);// top left
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(X + Size.Width * 4, Y, Z); // bottom left
            }

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            //Move();
        }

    }
}

