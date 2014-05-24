using System;
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
    /// Ballons effect
    /// </summary>
    class Ballons : IEffect
    {
        private float x;
        private float y;
        private float speedY;
        private float z;
        private float XLed;
        private float Xpos;
        private int BallonsImage;
        private Vector2[] vecTex;
        private Vector3[] vecPos;
        private bool disposed = false;

        /// <summary>
        /// Constuctor for the Ballons effect
        /// </summary>
        /// <param name="x">X position on screen</param>
        /// <param name="y">Y position on screen</param>
        /// <param name="speedY">What speed is it going with</param>
        /// <param name="BallonsImage">The TextureID to be used</param>
        /// <param name="vecTex">Texture coordinates on the image</param>
        /// <param name="XLed">?</param>
        /// <param name="z">The Z-deept to place the ballon at</param>
        public Ballons(float x, float y, float speedY, int BallonsImage, Vector2[] vecTex, float XLed, float z)
        {
            this.x = x;
            this.y = y;
            this.Xpos = x;
            this.speedY = speedY;
            this.z = z;
            this.XLed = XLed;
            this.BallonsImage = BallonsImage;
            this.vecTex = vecTex;

            this.vecPos = new Vector3[] {
                                         new Vector3(x + 0.0f,y  -0.2f,this.z),
                                         new Vector3(x - 0.2f,y  -0.2f,this.z),
                                         new Vector3(x - 0.2f,y  + 0.0f,this.z),
                                         new Vector3(x + 0.0f,y  +0.0f,this.z)
                                        };
        }

        /// <summary>
        /// Distructor
        /// </summary>
        ~Ballons()
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
                    BallonsImage = -1;
                    
                }
                // free native resources if there are any.
                //Debug.WriteLine(this.GetType().ToString() + " disposed.");

                disposed = true;
            }
        }

        /// <summary>
        /// Draw ballons on screen
        /// </summary>
        /// <param name="Date">What date is it?</param>
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

            x = this.Xpos + (float)((0.001 * Math.Sin(500 * y * (Math.PI / 180)) + 0.005)) * this.XLed;

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
