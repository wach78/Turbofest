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
    /// Halloween event.
    /// </summary>
    class Halloween : IEffect
    {
        class Spider : IEffect
        {
            private bool disposed = false;
            private Vector3[] vSpiders;
            private float Size;
            private int Texture;
            private Vector3 Speed;
            private int MoveMaxSideways;
            private int MovedSideways;
            private bool Left;

            public Spider(Vector3 Start, float Width, float Height, float SizeOfSpider, int TextureOfSpider)
            {
                vSpiders = new Vector3[4];
                Size = SizeOfSpider;
                Texture = TextureOfSpider;
                Speed = new Vector3(0.0015f, 0.0015f, 0.0f);
                MoveMaxSideways = Util.Rnd.Next(3, 8);
                MovedSideways = 0;
                Left = Util.Rnd.Next(0, 1)==1 ? true : false;

                vSpiders[0] = Start; // bottom left
                vSpiders[1] = new Vector3(Start.X + Width, Start.Y, Start.Z); // bottom right
                vSpiders[2] = new Vector3(Start.X + Width, Start.Y + Height, Start.Z); // top right
                vSpiders[3] = new Vector3(Start.X, Start.Y + Height, Start.Z); // top left
            }

            public Spider(float Width, float Height, float SizeOfSpider, int TextureOfSpider)
            {
                vSpiders = new Vector3[4];
                Size = SizeOfSpider;
                Texture = TextureOfSpider;
                Speed = new Vector3(0.0015f, -0.0015f, 0.0f);
                MoveMaxSideways = Util.Rnd.Next(20,50);
                MovedSideways = 0;
                Left = Util.Rnd.Next(0, 1) == 1 ? true : false;

                float[] viewp = Util.GetViewport();
                /*Matrix4 projectionM = new Matrix4();
                Matrix4 modelviewM = new Matrix4();
                GL.GetFloat(GetPName.ProjectionMatrix, out projectionM);
                GL.GetFloat(GetPName.ModelviewMatrix, out modelviewM);*/
                
                int rndX = Util.Rnd.Next(0, (int)(viewp[2]));
                int rndY = Util.Rnd.Next(0, (int)(viewp[3]));
                float X, Y, Z;
                X = 2.0f * rndX / (viewp[2]) - 1;
                Y = -2.0f * rndY / (viewp[3]) + 1;
                /*
                X = (rndX - viewp[0]) / viewp[2] * 2.0f - 1;
                Y = (rndY - viewp[1]) / viewp[3] * 2.0f - 1;
                Z = 1.0f;

                Matrix4 mvp = Util.GetMVP();
                mvp.Invert();
                Vector3 tmp = new Vector3(X, Y, Z);
                Vector3.TransformVector(ref tmp, ref mvp, out tmp); // not sure this is the right one...
                Console.WriteLine(rndX + ", " + rndY + ". " +tmp.X + ", " + tmp.Y);
                */
                X += Util.Rnd.Next(-10,10) / 10.0f ;
                Y += Util.Rnd.Next(-5, 5) / 10.0f;
                //Y = rndY;
                Z = 1.0f;
                Matrix4 mvp = Util.GetMVP();
                Vector3 tmp = new Vector3(X, Y, Z);
                Vector3.TransformVector(ref tmp, ref mvp, out tmp);
                //Vector3.TransformVector(ref tmp, ref modelviewM, out tmp);
                //Vector3.TransformVector(ref tmp, ref projectionM, out tmp);
                //Console.WriteLine(rndX + ", " + rndY + ". " + tmp.X + ", " + tmp.Y);

                vSpiders[0] = new Vector3(tmp.X + Width, tmp.Y, 1.0f); // bottom left
                vSpiders[1] = new Vector3(tmp.X, tmp.Y, 1.0f); // bottom right
                vSpiders[2] = new Vector3(tmp.X, tmp.Y + Height, 1.0f); // top right
                vSpiders[3] = new Vector3(tmp.X + Width, tmp.Y + Height, 1.0f); // top left
            }

            ~Spider()
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
                        Texture = 0;
                    }
                    // free native resources if there are any.
                    Console.WriteLine(this.GetType().ToString() + " disposed.");
                    disposed = true;
                }
            }

            public void Move()
            {
                Vector2 vx;
                if (MovedSideways > MoveMaxSideways || (MovedSideways <= 0 && Left))
                {
                    Left = !Left;
                }
                for (int i = 0; i < vSpiders.Length; i++)
                {
                    vx = new Vector2(vSpiders[i].X + (!Left? Speed.X : -Speed.X), vSpiders[i].Y + Speed.Y);
                    vSpiders[i].Xy = vx;
                    //Vector3.Add(ref vSpiders[i], ref Speed, out vSpiders[i]);
                }
                if (!Left)
                {
                    MovedSideways++;
                }
                else
                {
                    MovedSideways--;
                }
            }

            public void Draw(string Date)
            {
                GL.BindTexture(TextureTarget.Texture2D, Texture);
                GL.Enable(EnableCap.Texture2D);
                GL.Enable(EnableCap.Blend);
                //GL.PushAttrib(AttribMask.CurrentBit);
                GL.Begin(BeginMode.Quads);
                /*GL.Color4(Color.White); GL.Vertex3(vSpiders[0]);
                GL.Color4(Color.Red); GL.Vertex3(vSpiders[1]);
                GL.Color4(Color.Green); GL.Vertex3(vSpiders[2]);
                GL.Color4(Color.Yellow); GL.Vertex3(vSpiders[3]); */
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(vSpiders[0]);
                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(vSpiders[1]);
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(vSpiders[2]);
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(vSpiders[3]); 
                GL.End();
                //GL.PopAttrib();
                GL.Disable(EnableCap.Blend);
                GL.Disable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, 0);
                Move();
            }
        }

        private bool disposed = false;
        private int textureSpider;
        private int textureGhost;
        private int MaxSpiders; // not really needed...
        private Spider[] Spiders;

        public Halloween(int NumberOfSpiders)
        {
            textureSpider = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/spider.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.Transparent);
            textureGhost = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/halloween.bmp");
            MaxSpiders = NumberOfSpiders;
            Spiders = new Spider[MaxSpiders];
            for (int i = 0; i < MaxSpiders; i++)
            {
                Spiders[i] = new Spider(0.1f, 0.1f, 1.0f, textureSpider);
            }

        }

        ~Halloween()
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
                    Util.DeleteTexture(ref textureSpider);
                    Util.DeleteTexture(ref textureGhost);
                    for (int i = 0; i < MaxSpiders; i++)
                    {
                        Spiders[i].Dispose();
                        Spiders[i] = null;
                    }
                }
                // free native resources if there are any.
                disposed = true;
                Console.WriteLine(this.GetType().ToString() + " disposed.");
            }
        }

        public void Draw(string Date)
        {
            foreach (Spider s in Spiders)
            {
                s.Draw(Date);
            }
        }

    }
}
