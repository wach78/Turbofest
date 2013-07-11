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
    /// CrayFish event.
    /// </summary>
    class CrayFish : IEffect
    {
        

        private bool disposed = false;
        
        private int[] texture;
        private Vector3[] Shellfish;
        private SizeF Size;
        private float Speed;
        private float X;
        private float Y;
        private float Z;
        private long tick = 0;
        private int CurrentMove = 0;

        public CrayFish()
        {
            texture = new int[8];
            texture[0] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/krafta0.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            texture[1] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/krafta1.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            texture[2] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/krafta2.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            texture[3] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/krafta3.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            texture[4] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/krafta4.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            texture[5] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/krafta5.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            texture[6] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/krafta6.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            texture[7] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/krafta7.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));


            Shellfish = new Vector3[4];
            Size = new SizeF(0.6f, 0.8f);
            Speed = 0.0025f;
            CurrentMove = 0;

            //X = 0.0f - Size.Width / 2.0f;
            //Y = -0.5f - Size.Height / 2.0f;
            X = 0.0f;
            Y = 0.0f;
            Z = 0.45f;
            Shellfish[0] = new Vector3(X, Y, Z); // red
            Shellfish[1] = new Vector3(X, Y + Size.Height, Z); // blue
            Shellfish[2] = new Vector3(X + Size.Width, Y + Size.Height, Z); // green
            Shellfish[3] = new Vector3(X + Size.Width, Y, Z); // yellow

        }

        ~CrayFish()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            //base.Finalize();
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
                    Util.DeleteTexture(ref texture[0]);
                    Util.DeleteTexture(ref texture[1]);
                    Util.DeleteTexture(ref texture[2]);
                    Util.DeleteTexture(ref texture[3]);
                    Util.DeleteTexture(ref texture[4]);
                    Util.DeleteTexture(ref texture[5]);
                    Util.DeleteTexture(ref texture[6]);
                    Util.DeleteTexture(ref texture[7]);
                }
                // free native resources if there are any.
                disposed = true;
                System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
            }
        }

        
        public void Move()
        {
            // Make intro move

            // Make left right move

        }

        public void Draw(string Date)
        {
            // Spiders that draws after eachother and end ontop durring run will be hidden...

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.BindTexture(TextureTarget.Texture2D, texture[0]);

            GL.Begin(BeginMode.Quads);
            // Fix this to be dynamic, I don't have the feeling for it currently...

                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(X, Y, Z); // bottom right 
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(X, Y + Size.Height, Z); // Top right
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(X + Size.Width, Y + Size.Height, Z);// top left
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(X + Size.Width, Y, Z); // bottom left

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            //Move();
        }

    }
}

