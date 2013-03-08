using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    class Semla : IEffect
    {
        private int image;
        private float x;
        private float y;

        private bool leftBorder;
        private bool rightBorder;
        private bool topBorder;
        private bool bottomBorder;

        public Semla()
        {
            x = -1.0f;
            y = 0.0f;
            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\semla.bmp",TextureMinFilter.Linear,TextureMagFilter.Linear,TextureWrapMode.Clamp,TextureWrapMode.Clamp,System.Drawing.Color.FromArgb(255, 0, 255));

            leftBorder = true;
            rightBorder = false;
            topBorder = true;
            bottomBorder = false;
        }

        public void Dispose()
        {
            Util.DeleteTexture(ref image);
            this.image = -1;
            System.GC.SuppressFinalize(this);
        }

        private void DrawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image);
            GL.Enable(EnableCap.Blend); //       
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); //

            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(1.6f + x, -0.85f + y, 1.0f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(1.0f + x, -0.85f + y, 1.0f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(1.0f + x, 0.10f + y, 1.0f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(1.6f + x, 0.10f + y, 1.0f); // top left 

            GL.End();
            GL.Disable(EnableCap.Blend);//
            GL.Disable(EnableCap.Texture2D);
            

        }//DrawImage

        private void moveImage()
        {
            if (Math.Round(x, 2) == -2.66)
            {
                rightBorder = true;
                leftBorder = false;
            }

            if (Math.Round(x, 2) == 0.08)
            {
                leftBorder = true;
                rightBorder = false;
            }

            if (Math.Round(y, 2) == 1.20)
            {
                topBorder = true;
                bottomBorder = false;
            }

            if (Math.Round(y, 2) == -0.48)
            {
                bottomBorder = true;
                topBorder = false;
            }

            if (!topBorder)
            {
                y += 0.010f;
            }

            if (!bottomBorder)
            {
                y -= 0.010f;
            }

            if (!rightBorder)
            {
                x -= 0.010f;
            }

            if (!leftBorder)
            {
                x += 0.010f;
            }

        }//moveImage

        public void Draw(string Date)
        {
            moveImage();
            DrawImage();
        }//Draw

    }//class
}//namespace
