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
        private int[] matrixTexture;
        private int columns;
        private Font writeFont;
        private List<float> textureCoords;
        private List<float> textureCoordsStart;
        private List<float> textureCoordsSpeed;

        public Matrix(ref Text2D txt)
        {
            writeFont = new Font("Tahoma", 8.0f, FontStyle.Bold);
            textureID = txt.getTexture(OpenGL.Text2D.FontName.TypeFont);
            matrixTexture = new int[5];
            matrixTexture[0] = Util.GenTextureID();
            matrixTexture[1] = Util.GenTextureID();
            matrixTexture[2] = Util.GenTextureID();
            matrixTexture[3] = Util.GenTextureID();
            matrixTexture[4] = Util.GenTextureID();
            disposed = false;
            columns = 19;
            textureCoords = new List<float>();
            textureCoordsStart = new List<float>();
            textureCoordsSpeed = new List<float>();
            string[] randomChars = new string[5];
            string valuesChar = txt.getAllowedChars(OpenGL.Text2D.FontName.TypeFont);

            //byt ut 10 mot random 20-40 5 steg
            for (int j = 0; j < randomChars.Length; j++)
            {
                for (int i = 0; i < 20; i++)
                {
                    randomChars[j] += valuesChar[Util.Rnd.Next(0, valuesChar.Length)];
                }
            }

            for (int i = 0; i < columns; i++)
            {
                textureCoordsSpeed.Add((Util.Rnd.Next(100, 1000) / 75000.0f) * -1.0f);
                textureCoordsStart.Add(0.0f);
                textureCoords.Add(0.0f);
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
                    Util.DeleteTexture(ref matrixTexture[0]);
                    Util.DeleteTexture(ref matrixTexture[1]);
                    Util.DeleteTexture(ref matrixTexture[2]);
                    Util.DeleteTexture(ref matrixTexture[3]);
                    Util.DeleteTexture(ref matrixTexture[4]);
                    textureID = 0;
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");

                disposed = true;
            }
        }

        private void createImage(string[] randomChars, ref Text2D txt)
        {
            for (int i = 0; i < randomChars.Length; i++)
            {
                Bitmap bmSprite = new Bitmap(18, (((int)writeFont.GetHeight() + 1) * randomChars[i].Length));
                SolidBrush mgreen = new SolidBrush(Color.FromArgb(255, 0, 204, 0));
                Graphics g = Graphics.FromImage(bmSprite);
                g.FillRectangle(Brushes.Black, 0, 0, bmSprite.Width, bmSprite.Height);

                //SizeF sizeString = g.MeasureString(randomChars[i], writeFont);

                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                int Y = 0;
                for (int j = 0; j < randomChars[i].Length; j++)
                {
                    g.DrawString(randomChars[i][j].ToString(), writeFont, mgreen, 2, Y);
                    Y += (int)writeFont.GetHeight() + 1;
                }

                //bmSprite.Save("Matrix"+i+".bmp", ImageFormat.Bmp);
                g.Dispose();

                System.Drawing.Imaging.BitmapData data = bmSprite.LockBits(new System.Drawing.Rectangle(0, 0, bmSprite.Width, bmSprite.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.BindTexture(TextureTarget.Texture2D, matrixTexture[i]);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

                bmSprite.UnlockBits(data);
                bmSprite.Dispose();
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
        }

        public void DrawImage()
        {
            for (int i = 0; i < columns; i++)
            {
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, matrixTexture[i % 5]);
                GL.Begin(BeginMode.Quads);
                GL.TexCoord2(0.0f, 1.0f + textureCoords[i]); GL.Vertex3(1.4f - (i * 0.15f), -1.0f, 0.2f); // bottom left // x y z alla i mitten Y-led 
                GL.TexCoord2(1.0f, 1.0f + textureCoords[i]); GL.Vertex3(1.25f - (i * 0.15f), -1.0f, 0.2f); // bottom right // alla till vänster x-led
                GL.TexCoord2(1.0f, 0.0f + textureCoords[i]); GL.Vertex3(1.25f - (i * 0.15f), 1.0f, 0.2f);// top right
                GL.TexCoord2(0.0f, 0.0f + textureCoords[i]); GL.Vertex3(1.4f - (i * 0.15f), 1.0f, 0.2f); // top left 
                GL.End();
                GL.Disable(EnableCap.Texture2D);

                textureCoords[i] += textureCoordsSpeed[i];
                if (textureCoords[i] <= -1.0f)
                {
                    textureCoords[i] = 0.0f;
                }
            }
        }

        public void Draw(string Date)
        {
            DrawImage();
        }
    }//class
}//namespace
