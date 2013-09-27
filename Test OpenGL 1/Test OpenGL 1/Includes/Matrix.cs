using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace OpenGL 
{
    class Matrix : IEffect
    {
        private int textureID;
        private bool disposed;
        private int slideshowImage;

        public Matrix(ref Text2D txt)
        {
            textureID = txt.getTexture(OpenGL.Text2D.FontName.TypeFont);
            disposed = false;

            string randomChars ="";
            string valuesChar = txt.getAllowedChars(OpenGL.Text2D.FontName.TypeFont);

            //byt ut 10 mot random 20-40 5 steg
            for (int i = 0; i < 10; i++)
            {
                randomChars += valuesChar[Util.Rnd.Next(0, valuesChar.Length)];
            }

            createImage(randomChars, ref txt);
        }

        ~Matrix()
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
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");

                disposed = true;
            }
        }

        private void createImage(string randomChars, ref Text2D txt)
        {
            Bitmap bmSprite = new Bitmap(20, 25 * randomChars.Length);
            Graphics g = Graphics.FromImage(bmSprite);
            g.FillRectangle(Brushes.Black, 0, 0, bmSprite.Width, bmSprite.Height);

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int Y = 0;
            for (int i = 0; i < randomChars.Length; i++)
            {
                g.DrawString(randomChars[i].ToString(), new Font("Tahoma", 8.0f, FontStyle.Italic | FontStyle.Bold), Brushes.White, 2, Y);
                Y += 25;
            }

            bmSprite.Save("Matrix.bmp", ImageFormat.Bmp);
            g.Dispose();

            System.Drawing.Imaging.BitmapData data = bmSprite.LockBits(new System.Drawing.Rectangle(0, 0, bmSprite.Width, bmSprite.Height),
            ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Util.GenTextureID(out slideshowImage);
            GL.BindTexture(TextureTarget.Texture2D, slideshowImage);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);


            bmSprite.UnlockBits(data);
            bmSprite.Dispose();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        public void DrawImage()
        {
            GL.Enable(EnableCap.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, slideshowImage);
          
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, -1.25f, 1.0f); // bottom left // x y z alla i mitten Y-led 
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.5f, -1.25f, 1.0f); // bottom right // alla till vänster x-led
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-1.5f, 1.0f, 1.0f);// top right
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.0f, 1.0f, 1.0f); // top left 

            GL.End();
            GL.Disable(EnableCap.Texture2D);

        }

        public void Draw(string Date)
        {
            DrawImage();
        }
            
    }//class
}//namespace
