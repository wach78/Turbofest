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
            private float X;
            private float Y;
            private float Z;
            private SizeF Size;
            private float Scale;
            private int Texture;
            private Vector3 Speed;

            public Spider(Vector3 Start, float Width, float Height, float SizeOfSpider, int TextureOfSpider)
            {
                vSpiders = new Vector3[4];
                Scale = SizeOfSpider;
                Size = new SizeF(Width, Height);
                Texture = TextureOfSpider;
                Speed = new Vector3(0.0015f, 0.0015f, 0.0f);
                X = Start.X;
                Y = Start.Y;
                Y = Start.Z;
                vSpiders[0] = Start; // bottom left
                vSpiders[1] = new Vector3(Start.X + Width, Start.Y, Start.Z); // bottom right
                vSpiders[2] = new Vector3(Start.X + Width, Start.Y + Height, Start.Z); // top right
                vSpiders[3] = new Vector3(Start.X, Start.Y + Height, Start.Z); // top left
            }

            public Spider(float Width, float Height, float Z, float SizeOfSpider, int TextureOfSpider)
            {
                vSpiders = new Vector3[4];
                Scale = SizeOfSpider;
                Size = new SizeF(Width, Height);
                Texture = TextureOfSpider;
                Speed = new Vector3(/*0.0025f*/Util.Rnd.Next(20, 50) / 1000.0f, /*-0.0010f*/ -(Util.Rnd.Next(10, 30) / 10000.0f), 0.0f);
                
                float[] viewp = Util.GetViewport();
                /*Matrix4 projectionM = new Matrix4();
                Matrix4 modelviewM = new Matrix4();
                GL.GetFloat(GetPName.ProjectionMatrix, out projectionM);
                GL.GetFloat(GetPName.ModelviewMatrix, out modelviewM);*/
                
                int rndX = Util.Rnd.Next(0, (int)(viewp[2]));
                int rndY = Util.Rnd.Next(0, (int)(viewp[3]));
                
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
                X += Util.Rnd.Next(-10,13) / 10.0f ;
                Y += Util.Rnd.Next(-5, 7) / 10.0f;
                //Y = rndY;
                this.Z = Z;
                Matrix4 mvp = Util.GetMVP();
                Vector3 tmp = new Vector3(X, Y, Z);
                Vector3.TransformVector(ref tmp, ref mvp, out tmp);
                X = tmp.X;
                Y = tmp.Y;
                //Vector3.TransformVector(ref tmp, ref modelviewM, out tmp);
                //Vector3.TransformVector(ref tmp, ref projectionM, out tmp);
                //Console.WriteLine(rndX + ", " + rndY + ". " + tmp.X + ", " + tmp.Y);

                vSpiders[0] = new Vector3(X + Size.Width, Y, Z); // bottom left
                vSpiders[1] = new Vector3(X, Y, Z); // bottom right
                vSpiders[2] = new Vector3(X, Y + Size.Height, Z); // top right
                vSpiders[3] = new Vector3(X + Size.Width, Y + Size.Height, Z); // top left
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
                //float X = this.X + (float)Math.Sin( (this.Y* (this.Y<0? -1:1)) / 3.0f);
                // Need to fix this so that it world on world-coords...
                //float[] viewport = Util.GetViewport();
                //Matrix4 mvp = Util.GetMVP();
                //Vector3 pointVTopLeft = Vector3.Transform(new Vector3(this.X, Y, Z), mvp); // need to change this with 0.1 or so to correct the screen, done mut still small misses
                //float screenY = ((pointVTopLeft.Y / pointVTopLeft.Z) + 1) * viewport[3] / 2;
                //float X = this.X + (float)Math.Sin(screenY / 20.0) * 0.3f;
                float X = this.X + (float)Math.Sin(this.Y * 20.0f + Speed.X) * 0.3f;
                //X = this.X + (float)((0.001 * Math.Sin(5000 * this.Y * (Math.PI / 180)) + Speed.X)) * 100.0f;
                
                if (this.Y < -1.4f)
                {
                    this.Y = 1.4f;
                }
                else
                {
                    this.Y += Speed.Y;
                }
                vSpiders[0].Xy = new Vector2(X + Size.Width, this.Y);
                vSpiders[1].Xy = new Vector2(X, this.Y);
                vSpiders[2].Xy = new Vector2(X, this.Y + Size.Height);
                vSpiders[3].Xy = new Vector2(X + Size.Width, this.Y + Size.Height);
            }

            public void Draw(string Date)
            {
                GL.BindTexture(TextureTarget.Texture2D, Texture);
                GL.Enable(EnableCap.Texture2D);
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                
                /*GL.Begin(BeginMode.Quads);
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(vSpiders[0]);
                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(vSpiders[1]);
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(vSpiders[2]);
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(vSpiders[3]); 
                GL.End();*/

                
                //float coefficients[] = {constant,linear,quadratic}; // defailt (1,0,0)
                GL.PointParameter(PointParameterName.PointDistanceAttenuation, new float[] { 0.0f, 0.00000003f, 0.0f }); // float[] val = new float[]{constant,linear * correction, quadratic * correction, 1};
                GL.TexEnv(TextureEnvTarget.PointSprite, TextureEnvParameter.CoordReplace, 1.0f); // 
                GL.Enable(EnableCap.PointSprite);
                GL.PointSize(Size.Width); // set this to 30-ish and remove GL.PointParameter...
                GL.PushAttrib(AttribMask.CurrentBit);
                GL.Begin(BeginMode.Points);
                GL.Vertex3(vSpiders[1]);
                GL.End();
                GL.PopAttrib();
                GL.Disable(EnableCap.PointSprite);

                GL.Disable(EnableCap.Blend);
                GL.Disable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, 0);
                Move();
            }
        }

        private bool disposed = false;
        Chess chess;
        private int textureSpider;
        private int textureGhost;
        private int MaxSpiders; // not really needed...
        private Spider[] Spiders;
        private Vector3[] Ghost;
        private SizeF Size;
        private float X;
        private float Y;
        private float Z;
        long tick = 0;

        public Halloween(ref Chess chessboard, int NumberOfSpiders)
        {
            chess = chessboard;
            textureSpider = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/spider.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.Black);
            textureGhost = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/halloween.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255,0,255));
            MaxSpiders = NumberOfSpiders;
            Spiders = new Spider[MaxSpiders];
            Ghost = new Vector3[4];
            Size = new SizeF(1.0f, 1.0f);
            float sZ = 0.4f;
            for (int i = 0; i < MaxSpiders; i++)
            {
                Spiders[i] = new Spider(0.1f, 0.1f, Z, 1.0f, textureSpider);
                sZ -= sZ / (NumberOfSpiders + 2000);
            }
            X = Util.Rnd.Next(-3, 3) / 10.0f;
            Y = Util.Rnd.Next(-3, 3) / 10.0f;
            Z = 0.3f;
            Ghost[0] = new Vector3(X, Y, Z); // red
            Ghost[1] = new Vector3(X, Y + Size.Height, Z); // blue
            Ghost[2] = new Vector3(X + Size.Width, Y + Size.Height, Z); // green
            Ghost[3] = new Vector3(X + Size.Width, Y, Z); // yellow

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
                    chess = null;
                }
                // free native resources if there are any.
                disposed = true;
                Console.WriteLine(this.GetType().ToString() + " disposed.");
            }
        }
        
        public void Move()
        {
            tick++;
            X = (float)Math.Sin(tick / 22.1f) * 0.6f - Size.Width / 2;
            Y = (float)Math.Cos(tick / 22.1f) * 0.4f - Size.Height / 2;

            Ghost[0].Xy = new Vector2(X, Y);
            Ghost[1].Xy = new Vector2(X, Y + Size.Height);
            Ghost[2].Xy = new Vector2(X + Size.Width, Y + Size.Height);
            Ghost[3].Xy = new Vector2(X + Size.Width, Y);
        }

        public void Draw(string Date)
        {
            chess.Draw(Date, Chess.ChessColor.PurpleGreen);
            // Spiders that draws after eachother and end ontop durring run will be hidden...
            foreach (Spider s in Spiders)
            {
                s.Draw(Date);
            }

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.BindTexture(TextureTarget.Texture2D, textureGhost);
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

