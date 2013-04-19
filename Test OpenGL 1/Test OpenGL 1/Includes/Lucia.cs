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
    /// Lucia event.
    /// </summary>
    class Lucia : IEffect
    {
        private bool disposed = false;
        Chess chess;
        Sound snd;
        private int texture;
        private Vector3[] Ghost;
        private SizeF Size;
        private float Speed;
        private float X;
        private float Y;
        private float Z;
        private long tick = 0;

        public Lucia(ref Chess chessboard, ref Sound sound)
        {
            chess = chessboard;
            snd = sound;
            texture = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/lucia.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.Black);
            //snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/lucia.ogg", "lucia"); 

            Ghost = new Vector3[4];
            Size = new SizeF(0.4f, 0.8f);
            Speed = 0.0045f;

            X = Util.Rnd.Next(-3, 3) / 10.0f;
            Y = Util.Rnd.Next(-3, 3) / 10.0f;
            Z = 0.45f;
            Ghost[0] = new Vector3(X, Y, Z); // red
            Ghost[1] = new Vector3(X, Y + Size.Height, Z); // blue
            Ghost[2] = new Vector3(X + Size.Width, Y + Size.Height, Z); // green
            Ghost[3] = new Vector3(X + Size.Width, Y, Z); // yellow

        }

        ~Lucia()
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
                    Util.DeleteTexture(ref texture);
                    chess = null;
                }
                // free native resources if there are any.
                disposed = true;
                System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
            }
        }

        
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

        public void Draw(string Date)
        {
            chess.Draw(Date, Chess.ChessColor.WhiteRed);
            // Spiders that draws after eachother and end ontop durring run will be hidden...

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.BindTexture(TextureTarget.Texture2D, texture);

            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(Ghost[0]); // bottom right 
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(Ghost[1]); // Top right
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(Ghost[2]);// top left
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(Ghost[3]); // bottom left
            /*
            GL.Color3(Color.Red); GL.Vertex3(Ghost[0]); // bottom left 
            GL.Color3(Color.Yellow); GL.Vertex3(Ghost[1]); // bottom right
            GL.Color3(Color.Green); GL.Vertex3(Ghost[2]);// top right
            GL.Color3(Color.Blue); GL.Vertex3(Ghost[3]); // top left 
            */
            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            Move();
        }

    }
}

