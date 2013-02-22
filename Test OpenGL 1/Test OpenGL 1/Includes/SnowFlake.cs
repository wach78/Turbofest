using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    class SnowFlake
    {
        private float x;
        private float y;
        private float speedY; // Y led gravity 
        private float speedX;
        private float borderLeft;
        private float borderRight;

        private bool flakesLeft = true;
        private bool flakesRight = false;

        private int snowImage;

        Vector2[] vecTex;
        Vector3[] vecPos;

        public SnowFlake(float x, float speedY, float speedX,float borderLeft,
            float borderRight, int snowImage, Vector2[] vecTex)
        {
            this.x = x;
            this.y = 1.4f;
            this.speedY = speedY;
            this.speedX = speedX;
            this.borderLeft = x - borderLeft;
            this.borderRight = x + borderRight;
            this.snowImage = snowImage;
            this.vecTex = vecTex;
            //this.vecPos = vecPos;

            this.vecPos = new Vector3[] {
                                         new Vector3(x + 0.0f,y  -0.05f,1.1f),
                                         new Vector3(x - 0.05f,y  -0.05f,1.1f),
                                         new Vector3(x - 0.05f,y  + 0.0f,1.1f),
                                         new Vector3(x + 0.0f,y  +0.0f,1.1f)
                                        };

        }

        public void Draw()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, snowImage);
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



         
            y -= speedY;

            vecPos[0].Y = y - 0.05f;
            vecPos[1].Y = y - 0.05f;
            vecPos[2].Y = y;
            vecPos[3].Y = y;

            snowFlakesXpos();
            vecPos[0].X = x;
            vecPos[1].X = x - 0.05f;
            vecPos[2].X = x - 0.05f;
            vecPos[3].X = x;

        }


        private void snowFlakesXpos()
        {
            if (Math.Round((x), 2) == Math.Round(borderRight,2))
            {
                flakesLeft = true;
                flakesRight = false;
            }

            if (Math.Round((x), 2) == Math.Round(borderLeft, 2))
            {
                flakesRight = true;
                flakesLeft = false;
            }

            if (!flakesRight)
            {
                x+= 0.005f;
            }

            if (!flakesLeft)
            {
                x -= 0.005f;
            }


        }
    }//class
}//namespase
