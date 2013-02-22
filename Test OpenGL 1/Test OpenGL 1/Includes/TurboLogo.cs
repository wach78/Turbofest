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
    /// The logo of the Turbo party.
    /// create a dynamic image that have the logo mirrord and triger it when bouncing the right direction and write the correct name and year on the dynamic image.
    /// </summary>
    class TurboLogo //: IEffect
    {
        #region Suppress 414 warning variable set but not used
#pragma warning disable 414
        #endregion
        private bool disposed = false;
        private int texture;
        private bool moveUp;
        private bool moveLeft;
        private bool VTColour;
        private float moveX;
        private float moveY;
        private float X;
        private float Y;
        private Sound snd;
        private int numPlayedSound = 0;
        private string LastPlayedDate = string.Empty;

        public TurboLogo(ref Sound sound, bool VT=true)
        {
            Random rnd = new Random();
            if (rnd.Next(0, 10) < 6)
            {
                moveUp = false;
            }
            else
	        {
                moveUp = true;
            }
            if (rnd.Next(0,10) < 6)
            {
                moveLeft = false;
            }
            else
	        {
                moveLeft = true;
            }

            rnd = null;
            snd = sound;

            snd.CreateSound(Sound.FileType.WAV, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/roadrunner.wav", "roadrunner");

            VTColour = VT;

            moveX = 0.0075f;
            moveY = 0.0075f;
            X = Y = 0.0f;

            //Bitmaps for making the 4 different layouts
            Bitmap bitmap = new Bitmap(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/roadrunner.bmp");
            Bitmap bmSprite = new Bitmap(bitmap.Width*2,bitmap.Height*2);

            Graphics g = Graphics.FromImage(bmSprite);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            g.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
            g.DrawImage(bitmap, 0, bitmap.Height, bitmap.Width, bitmap.Height);
            bitmap.RotateFlip(RotateFlipType.Rotate180FlipY);
            g.DrawImage(bitmap, bitmap.Width, 0, bitmap.Width, bitmap.Height);
            g.DrawImage(bitmap, bitmap.Width, bitmap.Height, bitmap.Width, bitmap.Height);

            // verify that this is not missing as it will crash if not possible to find...
            // this makes bad resolution on text, it can get distorted...
            // top left
            g.DrawString("Turbophesten", new Font("Tahoma", 19.0f, FontStyle.Italic | FontStyle.Bold), Brushes.Black, 75, 135);
            g.DrawString("Örebro 2013", new Font("Tahoma", 19.0f, FontStyle.Italic | FontStyle.Bold), Brushes.Black, 72, 160);
            // top right
            g.DrawString("Turbophesten", new Font("Tahoma", 19.0f, FontStyle.Italic | FontStyle.Bold), Brushes.Black, bitmap.Width + 35, 135);
            g.DrawString("Örebro 2013", new Font("Tahoma", 19.0f, FontStyle.Italic | FontStyle.Bold), Brushes.Black, bitmap.Width + 55, 160);
            // bottom left
            g.DrawString("Turbophesten", new Font("Tahoma", 18.0f, FontStyle.Italic | FontStyle.Bold), Brushes.Black, 75, bitmap.Height + 135);
            g.DrawString("Örebro 2013", new Font("Tahoma", 18.0f, FontStyle.Italic | FontStyle.Bold), Brushes.Black, 72, bitmap.Height + 160);
            //bottom right
            g.DrawString("Turbophesten", new Font("Tahoma", 18.0f, FontStyle.Italic | FontStyle.Bold), Brushes.Black, bitmap.Width + 35, bitmap.Height + 135);
            g.DrawString("Örebro 2013", new Font("Tahoma", 18.0f, FontStyle.Italic | FontStyle.Bold), Brushes.Black, bitmap.Width + 55, bitmap.Height + 160);
            
            g.Dispose();
            //End Bitmap creation and edits
            bmSprite.MakeTransparent(Color.Magenta);
            // change yellow (255, 214, 0) to blue and ?
            for (int y = bitmap.Height; y < bmSprite.Height; y++)
            {
                for (int x = 0; x < bmSprite.Width; x++)
                {
                    Color col = bmSprite.GetPixel(x, y);
                    if (col.R == 255 && col.G == 214 && col.B == 0)
                    {
                        bmSprite.SetPixel(x, y, Color.FromArgb(0, 187, 255));
                    }
                }
            }
            
            BitmapData data = bmSprite.LockBits(new System.Drawing.Rectangle(0, 0, bmSprite.Width, bmSprite.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            
            Util.GenTextureID(out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

            bmSprite.UnlockBits(data);
            bmSprite.Dispose();
            bitmap.Dispose();
            data = null;
            bmSprite = null;
            bitmap = null;
            
            //data = null;
            GL.BindTexture(TextureTarget.Texture2D, 0);

           

        }

        ~TurboLogo()
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
            if (disposing)
            {
                // free managed resources
                Util.DeleteTexture(ref texture);
                snd = null;
            }
            // free native resources if there are any.
            disposed = true;
        }

        

        /*public void toPlay(string Date)
        {
            if (LastPlayedDate != Date)
            {
                LastPlayedDate = Date;
                numPlayedSound = 0;
            }
        }*/

        public void PlaySound(string Date)
        {
            if (LastPlayedDate != Date)
            {
                LastPlayedDate = Date;
                numPlayedSound = 0;
            }
            //need to keep track of how many times this have played?!
            if (snd.PlayingName() != "roadrunner" && numPlayedSound < 3) // this will start once the last sound is done, ie looping.
            {
                snd.Play("roadrunner");
                numPlayedSound++;
            }
        }

        public void Draw(string Date)
        {
            /*
            // Get bounds of view.
            float[] viewport = new float[4];
            float[] projectiofM = new float[16];
            float[] modelviewfM = new float[16];
            Matrix4 projectionM = new Matrix4();
            Matrix4 modelviewM = new Matrix4();
            Matrix4 projMultimodel;// = new Matrix4();
            Matrix4 ScreenFrustum = new Matrix4(); // all 4 used
            Vector4[] NearFarFrustum = new Vector4[2]; // only 0-1 used, 2-3 is zero
            GL.GetFloat(GetPName.Viewport, viewport);
            GL.GetFloat(GetPName.ProjectionMatrix, out projectionM);
            GL.GetFloat(GetPName.ModelviewMatrix, out modelviewM);
            projMultimodel = Matrix4.Mult(projectionM, modelviewM);

            Vector4 rPlane = new Vector4(projMultimodel.Column0.W - projMultimodel.Column0.X,
                    projMultimodel.Column1.W - projMultimodel.Column1.X,
                    projMultimodel.Column2.W - projMultimodel.Column2.X,
                    projMultimodel.Column3.W - projMultimodel.Column3.X); 
            rPlane.Normalize();

            Vector4 rPlaneManual = new Vector4(projMultimodel.M14 - projMultimodel.M11,
                    projMultimodel.M24 - projMultimodel.M21,
                    projMultimodel.M34 - projMultimodel.M31,
                    projMultimodel.M44 - projMultimodel.M41);
            rPlaneManual.Normalize();
            */

            /*Vector4 rPlaneManual2;
            unsafe
            {
                float* clip1 = (float*)(&projMultimodel);
                rPlaneManual2 = new Vector4(clip1[3] - clip1[0], clip1[7] - clip1[4], clip1[11] - clip1[8], clip1[15] - clip1[12]);
                rPlaneManual2.Normalize();
            }
            */

            /*Vector4 lPlane = new Vector4(projMultimodel.Column0.W + projMultimodel.Column0.X,
                    projMultimodel.Column1.W + projMultimodel.Column1.X,
                    projMultimodel.Column2.W + projMultimodel.Column2.X,
                    projMultimodel.Column3.W + projMultimodel.Column3.X);
            lPlane.Normalize();
            Vector4 bPlane = new Vector4(projMultimodel.Column0.W - projMultimodel.Column0.Y,
                    projMultimodel.Column1.W - projMultimodel.Column1.Y,
                    projMultimodel.Column2.W - projMultimodel.Column2.Y,
                    projMultimodel.Column3.W - projMultimodel.Column3.Y);
            bPlane.Normalize();
            Vector4 tPlane = new Vector4(projMultimodel.Column0.W + projMultimodel.Column0.Y,
                    projMultimodel.Column1.W + projMultimodel.Column1.Y,
                    projMultimodel.Column2.W + projMultimodel.Column2.Y,
                    projMultimodel.Column3.W + projMultimodel.Column3.Y); 
            tPlane.Normalize();

            ScreenFrustum = new Matrix4(rPlane, lPlane, bPlane, tPlane);
            */


            PlaySound(Date);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            //GL.BlendColor(255, 0, 255, 255);
            //GL.BlendFunc(BlendingFactorSrc.ConstantColor, BlendingFactorDest.OneMinusConstantColor); //(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); //(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
            
            GL.Begin(BeginMode.Quads);
            //X += moveX;
            //Y += moveY;

            if (Math.Round(X, 2) >= 1.15)
            {
                moveLeft = false;
            }
            else if (Math.Round(X, 1) <= -1.9)
            {
                moveLeft = true;
            }
            if (Math.Round(Y,2) >= 1.13)
            {
                moveUp = false;
            }
            else if (Math.Round(Y, 1) <= -1.0)
            {
                moveUp = true;
            }

            X = X + (moveLeft ? 1 : -1) * moveX;
            Y = Y + (moveUp ? 1 : -1) * moveY;

            //Console.WriteLine(X + ", " + Y + ", " + moveUp + ", " + moveRight);
            GL.TexCoord2((moveLeft ? 0.5f : 0.0f), (VTColour ? 0.5f : 0.0f)); GL.Vertex3(1.00f + X, 0.20f + Y, 1.00f); // top left
            GL.TexCoord2((moveLeft ? 0.5f : 0.0f), (VTColour ? 1.0f : 0.5f)); GL.Vertex3(1.00f + X, -0.40f + Y, 1.00f); // bottom left
            GL.TexCoord2((moveLeft ? 1.0f : 0.5f), (VTColour ? 1.0f : 0.5f)); GL.Vertex3(-0.30f + X, -0.40f + Y, 1.00f); // bottom right
            GL.TexCoord2((moveLeft ? 1.0f : 0.5f), (VTColour ? 0.5f : 0.0f)); GL.Vertex3(-0.30f + X, 0.20f + Y, 1.00f); // top right
            

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

    }
}
