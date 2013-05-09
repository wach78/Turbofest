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
    class TurboLogo : IEffect
    {
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
        private Chess bakground;
        private int chessNumber;
        private int numPlayedSound = 0;
        private string LastPlayedDate = string.Empty;

        public TurboLogo(ref Sound sound, ref Chess chess,bool VT = true)
        {
   
            if (Util.Rnd.Next(0, 10) < 6)
            {
                moveUp = false;
            }
            else
	        {
                moveUp = true;
            }
            if (Util.Rnd.Next(0,10) < 6)
            {
                moveLeft = false;
            }
            else
	        {
                moveLeft = true;
            }

            bakground = chess;
            snd = sound;
            randomChess();

            //snd.CreateSound(Sound.FileType.WAV, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/roadrunner.wav", "roadrunner");
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/roadrunner.ogg", "roadrunner");

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
            if (!this.disposed)
            {
                if (disposing)
                {
                    // free managed resources
                    Util.DeleteTexture(ref texture);
                    snd = null;
                }
                // free native resources if there are any.
                disposed = true;
                System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
            }
        }

        public void randomChess()
        {
            chessNumber = Util.Rnd.Next(0, 6);
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
                randomChess();
            }
            //need to keep track of how many times this have played?!
            if (snd.PlayingName() != "roadrunner" && numPlayedSound < 3) // this will start once the last sound is done, ie looping.
            {
                snd.Play("roadrunner");
                numPlayedSound++;
            }
        }

        public void FrameUpdate(string Date)
        {
            if (Math.Round(X, 2) >= 1.15)
            {
                moveLeft = false;
            }
            else if (Math.Round(X, 1) <= -1.9)
            {
                moveLeft = true;
            }
            if (Math.Round(Y, 2) >= 1.13)
            {
                moveUp = false;
            }
            else if (Math.Round(Y, 1) <= -1.0)
            {
                moveUp = true;
            }

            X = X + (moveLeft ? 1 : -1) * moveX;
            Y = Y + (moveUp ? 1 : -1) * moveY;
        }

        public void Draw(string Date)
        {

            bakground.Draw(Date, (Chess.ChessColor)chessNumber);

            float[] viewport = Util.GetViewport();
            Matrix4 mvp = Util.GetMVP();
            // process drain....
            Vector3 pointVTopLeft = Vector3.Transform(new Vector3(X + 0.9f, Y + 0.1f, 1.0f), mvp); // need to change this with 0.1 or so to correct the screen, done mut still small misses
            Vector3 pointVBottomRight = Vector3.Transform(new Vector3(X - 0.2f, Y - 0.3f, 1.0f), mvp);
            //pointV.Normalize(); //can normalize it to get the same?!
            float screenX = ((pointVTopLeft.X / pointVTopLeft.Z) + 1) * viewport[2] / 2;
            float screenY = ((pointVTopLeft.Y / pointVTopLeft.Z) + 1) * viewport[3] / 2;
            Vector2 pointV2DTopLeft = new Vector2(screenX, screenY);
            screenX = ((pointVBottomRight.X / pointVBottomRight.Z) + 1) * viewport[2] / 2;
            screenY = ((pointVBottomRight.Y / pointVBottomRight.Z) + 1) * viewport[3] / 2;
            Vector2 pointV2DBottomRight = new Vector2(screenX, screenY);

            //System.Diagnostics.Debug.WriteLine(pointV2DTopLeft.X + ", " + pointV2DTopLeft.Y);
            

            PlaySound(Date);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            //GL.BlendColor(255, 0, 255, 255);
            //GL.BlendFunc(BlendingFactorSrc.ConstantColor, BlendingFactorDest.OneMinusConstantColor); //(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); //(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Disable(EnableCap.DepthTest);
            GL.Begin(BeginMode.Quads);
            //X += moveX;
            //Y += moveY;

            /*Vector4[] bounds = Util.GetFrustum(false);
            Vector4[] bounds2 = Util.GetFrustum(true);
            
            //aspect = width/height
            //fov_ = atan2(fov)
            //right = near * aspect * fov_
            //left = -right
            //top = near * fov_
            //bottom = -top
            // The equation for a plane is: Ax + By + Cz + D = 0, where A, B and C define the plane's normal vector, D is the distance from the origin to the plane,
            // and x, y and z are any points on the plane.. You can plug any point into the equation and if the result is 0 then the point lies on the plane. If the
            // result is greater than 0 then the point is in front of the plane, and if it's negative the point is behind the plane
            float dright = bounds[0].X * (-0.30f + X) + bounds[0].Y * (0.20f + Y) + bounds[0].Z * (1.00f) + bounds[0].W;
            float dleft = bounds[1].X * (1.00f + X) + bounds[1].Y * (0.20f + Y) + bounds[1].Z * (1.00f) + bounds[1].W;
            float dtop = bounds[3].X * (-0.30f + X) + bounds[3].Y * (0.20f + Y) + bounds[3].Z * (1.00f) + bounds[3].W;
            float dbottom = bounds[2].X * (1.00f + X) + bounds[2].Y * (-0.40f + Y) + bounds[2].Z * (1.00f) + bounds[2].W;
            */

            /*
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
            }*/

            if (pointV2DBottomRight.X >= viewport[2])
            {
                moveLeft = true;
            }
            else if (pointV2DTopLeft.X <= viewport[0])
            {
                moveLeft = false;
            }
            if (pointV2DTopLeft.Y >= viewport[3])
            {
                moveUp = false;
            }
            else if (pointV2DBottomRight.Y <= viewport[1])
            {
                moveUp = true;
            }

            X = X + (moveLeft ? 1 : -1) * moveX;
            Y = Y + (moveUp ? 1 : -1) * moveY;
            
            //FrameUpdate(Date);

            //System.Diagnostics.Debug.WriteLine(X + ", " + Y + ", " + moveUp + ", " + moveRight);
            GL.TexCoord2((moveLeft ? 0.5f : 0.0f), (VTColour ? 0.5f : 0.0f)); GL.Vertex3(1.00f + X, 0.20f + Y, 1.00f); // top left
            GL.TexCoord2((moveLeft ? 0.5f : 0.0f), (VTColour ? 1.0f : 0.5f)); GL.Vertex3(1.00f + X, -0.40f + Y, 1.00f); // bottom left
            GL.TexCoord2((moveLeft ? 1.0f : 0.5f), (VTColour ? 1.0f : 0.5f)); GL.Vertex3(-0.30f + X, -0.40f + Y, 1.00f); // bottom right
            GL.TexCoord2((moveLeft ? 1.0f : 0.5f), (VTColour ? 0.5f : 0.0f)); GL.Vertex3(-0.30f + X, 0.20f + Y, 1.00f); // top right
            

            GL.End();
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

            /*GL.PointSize(5.0f);
            GL.Begin(BeginMode.Points);
            GL.Vertex3(new Vector3(X + 1.0f, Y + 0.2f, 1.0f));
            GL.Vertex3(new Vector3(X - 0.3f, Y - 0.4f, 1.0f));
            GL.End();*/

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

    }
}
