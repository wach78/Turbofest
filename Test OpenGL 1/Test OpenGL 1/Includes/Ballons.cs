using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{

    class Ballons : IEffect
    {
        private float x;
        private float y;
        private float speedY;
        private float speedX;

        private float Xpos;

        private int BallonsImage;

        private Vector2[] vecTex;
        private Vector3[] vecPos;
        private bool disposed = false;

        public Ballons(float x, float y, float speedX, float speedY, int BallonsImage, Vector2[] vecTex)
        {
            this.x = x;
            this.y = y;
            this.Xpos = x;
            this.speedY = speedY;
            this.speedX = speedX;
            this.BallonsImage = BallonsImage;
            this.vecTex = vecTex;

            this.vecPos = new Vector3[] {
                                         new Vector3(x + 0.0f,y  -0.2f,1.1f),
                                         new Vector3(x - 0.2f,y  -0.2f,1.1f),
                                         new Vector3(x - 0.2f,y  + 0.0f,1.1f),
                                         new Vector3(x + 0.0f,y  +0.0f,1.1f)
                                        };
        }

        ~Ballons()
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
                    BallonsImage = 0;
                    
                }
                // free native resources if there are any.
                Console.WriteLine(this.GetType().ToString() + " disposed.");
                disposed = true;
            }
        }


        public void Draw(string Date)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, BallonsImage);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led

            GL.TexCoord2(vecTex[0]); GL.Vertex3(vecPos[0]); // bottom left  
            GL.TexCoord2(vecTex[1]); GL.Vertex3(vecPos[1]); // bottom right 
            GL.TexCoord2(vecTex[2]); GL.Vertex3(vecPos[2]);// top right
            GL.TexCoord2(vecTex[3]); GL.Vertex3(vecPos[3]); // top left 

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);


            if (this.y > 1.4f)
            {
                this.y = -1.4f;
            }
            else
            {
                this.y += speedY;
            }

            x = this.Xpos + (float)((0.001 * Math.Sin(500 * y * (Math.PI / 180)) + 0.005)) * 100.0f;

            vecPos[0].Y = y - 0.2f;
            vecPos[1].Y = y - 0.2f;
            vecPos[2].Y = y;
            vecPos[3].Y = y;

            vecPos[0].X = x;
            vecPos[1].X = x - 0.2f;
            vecPos[2].X = x - 0.2f;
            vecPos[3].X = x;

        }
          
    }//class
}//namespace
