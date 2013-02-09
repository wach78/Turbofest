using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    public class Chess : IEffect
    {
        private bool disposed = false;
        private int texture;
        private double m_scrollX;
        private double m_scrollY;
        private Vector3[] m_vec;

        public Chess()
        {
            this.m_vec = new Vector3[4]; // well well....
            this.m_vec[0] = new Vector3(-2.30f, -1.50f, -1.0f);
            this.m_vec[1] = new Vector3(-2.30f, 0.20f, 1.0f);
            this.m_vec[2] = new Vector3(2.30f, 0.20f, 1.0f);
            this.m_vec[3] = new Vector3(2.30f, -1.50f, -1.0f);

            //GL.GenBuffers(1, out this.texture);
            //this.texture = GL.GenTexture();

            //System.Drawing.Bitmap bitmapChess = new System.Drawing.Bitmap(System.IO.Path.GetFullPath(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/chess.gif"));


            texture = Util.LoadTexture(System.IO.Path.GetFullPath(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/chess.gif"), TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Repeat, TextureWrapMode.Repeat);
            /*GL.BindTexture(TextureTarget.Texture2D, texture);
            System.Drawing.Imaging.BitmapData data = bitmapChess.LockBits(new System.Drawing.Rectangle(0, 0, bitmapChess.Width, bitmapChess.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmapChess.UnlockBits(data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            */
            this.m_scrollX = 0.0;
            this.m_scrollY = 0.0;
        }

        ~Chess()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            //base.Finalize();
            //GL.DeleteBuffers(1, ref this.texture); 
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                Util.DeleteTexture(ref texture);
                texture = -1;
            }
            // free native resources if there are any.

            disposed = true;
        }

        public void Draw()
        {
            Draw(false, true);
        }

        public void Draw(bool scrollx, bool scrolly)
        {
            if (scrolly) this.m_scrollY += 0.008;
            if (scrollx) this.m_scrollX += 0.008;
            if (this.m_scrollY >= 1.0) this.m_scrollY = 0.0;
            if (this.m_scrollX >= 1.0) this.m_scrollX = 0.0;

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, this.texture);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0 + this.m_scrollX, 0.0 + this.m_scrollY); GL.Vertex3(-2.30f, -1.50f, -1.0f); // bottom right
            GL.TexCoord2(0.0 + this.m_scrollX, 5.0 + this.m_scrollY); GL.Vertex3(-2.30f, 0.20f, 1.0f); // top right
            GL.TexCoord2(5.0 + this.m_scrollX, 5.0 + this.m_scrollY); GL.Vertex3(2.30f, 0.20f, 1.0f); // top left
            GL.TexCoord2(5.0 + this.m_scrollX, 0.0 + this.m_scrollY); GL.Vertex3(2.30f, -1.50f, -1.0f); // bottom left
            GL.End();
            GL.Disable(EnableCap.Texture2D);

        }

    }
}
