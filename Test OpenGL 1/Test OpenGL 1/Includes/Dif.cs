using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    class Dif : IEffect
    {
        private Chess bakground; 
        private int image;
        private float x;
        private float y;

        private bool leftBorder;
        private bool rightBorder;
        private bool topBorder;
        private bool bottomBorder;

        private bool disposed = false;

        public Dif(ref Chess chess)
        {
            bakground = chess;
            x = -1.0f;
            y = 0.0f;
            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\dif2.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
           
            leftBorder = true;
            rightBorder = false;
            topBorder = true;
            bottomBorder = false;
        }

        ~Dif()
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
                    Util.DeleteTexture(ref image);
                    image = 0;
                    bakground = null;
                }
                // free native resources if there are any.
                Console.WriteLine(this.GetType().ToString() + " disposed.");
                disposed = true;
            }
        }

        private void DrawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image);
            GL.Enable(EnableCap.Blend);       
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);  

            GL.Begin(BeginMode.Quads);
           
            // x y z
            // alla i mitten Y-led  alla till vänster x-led //
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(1.6f+x, -0.55f+y, 0.8f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(1.0f+x, -0.55f+y, 0.8f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(1.0f+x, 0.10f+y, 0.8);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(1.6f+x, 0.10f+y, 0.8f); // top left 

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

        }//DrawImage
   
        private void moveImage()
        {        
            if (x < -2.90f)
            {
                rightBorder = true; 
                leftBorder = false;
            }

            if (x > 0.3f)
            {
                leftBorder = true; 
                rightBorder = false;
            }

            if (y >1.10f)
            {
                topBorder = true; 
                bottomBorder = false;
            }

            if (y < 0.0f && y < -0.48f)
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
            bakground.Draw(Date, Chess.ChessColor.WhiteRed);
            moveImage();
            DrawImage();
            
        }//Draw

    }//class
}//namespace
