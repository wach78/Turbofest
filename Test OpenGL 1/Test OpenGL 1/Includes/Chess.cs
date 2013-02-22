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
        #region Suppress 414 warning variable set but not used
#pragma warning disable 414
        #endregion
        public enum ChessColor {BlackWhite, BlueWhite, PurpleGreen, BlackGreen, BlackLightGreen, BlackPurple, BlackRed}; // this is after the constructor index...
        private bool disposed = false;
        private int[] texture;
        private double m_scrollX;
        private double m_scrollY;
        private Vector3[] m_vec;

        //private string vertShader;
        //private string fragShader;

        public Chess()
        {
            m_vec = new Vector3[4]; // well well....
            m_vec[0] = new Vector3(-2.30f, -1.50f, 0.0f);
            m_vec[1] = new Vector3(-2.30f, 0.20f, 5.0f);
            m_vec[2] = new Vector3(2.30f, 0.20f, 5.0f);
            m_vec[3] = new Vector3(2.30f, -1.50f, 0.0f);
            texture = new int[7];

            //GL.GenBuffers(1, out this.texture);
            //this.texture = GL.GenTexture();

            //System.Drawing.Bitmap bitmapChess = new System.Drawing.Bitmap(System.IO.Path.GetFullPath(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/chess.gif"));


            texture[0] = Util.LoadTexture(System.IO.Path.GetFullPath(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/chess_bw.gif"), TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Repeat, TextureWrapMode.Repeat);
            texture[1] = Util.LoadTexture(System.IO.Path.GetFullPath(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/chess_blue.gif"), TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Repeat, TextureWrapMode.Repeat);
            texture[2] = Util.LoadTexture(System.IO.Path.GetFullPath(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/chess_fbk.gif"), TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Repeat, TextureWrapMode.Repeat);
            texture[3] = Util.LoadTexture(System.IO.Path.GetFullPath(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/chess_green.gif"), TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Repeat, TextureWrapMode.Repeat);
            texture[4] = Util.LoadTexture(System.IO.Path.GetFullPath(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/chess_lightgreen.gif"), TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Repeat, TextureWrapMode.Repeat);
            texture[5] = Util.LoadTexture(System.IO.Path.GetFullPath(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/chess_purple.gif"), TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Repeat, TextureWrapMode.Repeat);
            texture[6] = Util.LoadTexture(System.IO.Path.GetFullPath(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/chess_red.gif"), TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Repeat, TextureWrapMode.Repeat);
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

            /*vertShader = @"#version 130
                void main() {			
	                // Set the front color to the color passed through with glColor*f
	                gl_FrontColor = gl_Color;
	                // Set the position of the current vertex 
	                gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
                }
            ";
            fragShader = @"#version 130
                void main() {
	                // Set the output color of our current pixel
	                gl_FragColor = gl_Color;
                }
            ";*/
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
                for (int i = 0; i < texture.Length; i++)
                {
                    Util.DeleteTexture(ref texture[i]);    
                }
                texture = null;
            }
            // free native resources if there are any.

            disposed = true;
        }
        public void Draw(string date)
        {
            Draw(false, true, ChessColor.BlackWhite);
        }

        public void Draw(string date, ChessColor CC)
        {
            Draw(false, true, CC);
        }

        public void Draw(bool scrollx, bool scrolly, ChessColor CC)
        {
            if (scrolly) this.m_scrollY += 0.008;
            if (scrollx) this.m_scrollX += 0.008;
            if (this.m_scrollY >= 1.0) this.m_scrollY = 0.0;
            if (this.m_scrollX >= 1.0) this.m_scrollX = 0.0;

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texture[(int)CC]);
            GL.Begin(BeginMode.Quads);
            //GL.TexCoord2(0.0 + this.m_scrollX, 0.0 + this.m_scrollY); GL.Vertex3(-2.30f, -1.50f, -1.0f); // bottom right
            //GL.TexCoord2(0.0 + this.m_scrollX, 5.0 + this.m_scrollY); GL.Vertex3(-2.30f, 0.20f, 1.0f); // top right
            //GL.TexCoord2(5.0 + this.m_scrollX, 5.0 + this.m_scrollY); GL.Vertex3(2.30f, 0.20f, 1.0f); // top left
            //GL.TexCoord2(5.0 + this.m_scrollX, 0.0 + this.m_scrollY); GL.Vertex3(2.30f, -1.50f, -1.0f); // bottom left

            
            
            GL.Color4(System.Drawing.Color.White); GL.TexCoord2(0.0 + this.m_scrollX, 5.0 + this.m_scrollY); GL.Vertex3(1.50f, -1.50f, 0.0f); // bottom left
            GL.Color4(System.Drawing.Color.Yellow); GL.TexCoord2(5.0 + this.m_scrollX, 5.0 + this.m_scrollY); GL.Vertex3(-1.50f, -1.50f, 0.0f); // bottom right
            GL.Color4(System.Drawing.Color.Blue); GL.TexCoord2(5.0 + this.m_scrollX, 0.0 + this.m_scrollY); GL.Vertex3(-3.00f, 0.50f, 6.0f); // top right
            GL.Color4(System.Drawing.Color.Red); GL.TexCoord2(0.0 + this.m_scrollX, 0.0 + this.m_scrollY); GL.Vertex3(3.00f, 0.50f, 6.0f); // top left
            
            
            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);
        }

    }
}
