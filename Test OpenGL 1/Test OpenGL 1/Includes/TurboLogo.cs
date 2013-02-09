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
    class TurboLogo
    {
        #region Suppress 414 warning variable set but not used
#pragma warning disable 414
        #endregion
        int texture;
        bool moveUp;
        bool moveLeft;
        float moveX;
        float moveY;
        float X;
        float Y;

        public TurboLogo()
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

            //OpenGL.Sound snd = new Sound();
            
            //Sound.CreateSound(Sound.FileType.WAV, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/fbk.wav", "FBK");
            //Sound.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/free.ogg", "free");
            Sound.CreateSound(Sound.FileType.WAV, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/roadrunner.wav", "roadrunner");
            //snd.Play("FBK");

            moveX = 0.0025f;
            moveY = 0.0025f;
            X = Y = 0.0f;
            /*int ab = AL.GenBuffer();
            int sb = AL.GenSource();
            Vector3 v3 = new Vector3(1, 1, 1);
            AL.Source(ab, ALSource3f.Position, ref v3);*/

            Bitmap bitmap = new Bitmap(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/roadrunner.bmp");
            // change yellow (255, 214, 0) to blue and ?
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color col = bitmap.GetPixel(x, y);
                    if (col.R == 255 && col.G == 214 && col.B == 0)
                    {
                        //Console.WriteLine("X:" + x + ", Y:" + y + ", " + col);
                        bitmap.SetPixel(x, y, Color.FromArgb(0, 187, 255));
                    }
                }
            }
            
            bitmap.MakeTransparent(Color.Magenta);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            
            Util.GenTextureID(out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

            bitmap.UnlockBits(data);
            //data = null;
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        bool xx = false;
        public void PlaySound()
        {
            if (!xx)
            {
                Sound.SetAudioBuffer("roadrunner");
                xx = true;
            }
        }

        public void Draw()
        {
            PlaySound();
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
            if (Math.Round(Y,1) >= 1.2)
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
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.00f + X, 0.20f + Y, 1.00f); // top left
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.00f + X, -0.40f + Y, 1.00f); // bottom left
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-0.30f + X, -0.40f + Y, 1.00f); // bottom right
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-0.30f + X, 0.20f + Y, 1.00f); // top right
            

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

    }
}
