using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    class Text2D
    {
        string m_strText;
        int texture;
        System.Drawing.Font m_font;
        int[] m_drawSize;

        /// <summary>
        /// 2D text to be writen to the screen
        /// </summary>
        /// <param name="width">Width of the area to write on</param>
        /// <param name="height">Height of the area to write on</param>
        public Text2D(int width, int height)
        {
            m_strText = "This is what will be shown";
            m_font = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 12.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            m_drawSize = new int[2] { width, height };
            //Console.WriteLine(m_font);
            texture = GL.GenTexture();

            //Console.WriteLine( System.Drawing.Graphics.FromImage(new System.Drawing.Bitmap(1,1)).MeasureString(m_strText[0].ToString(), m_font).Width );

            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(m_drawSize[0], m_drawSize[1]);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bm.Width, bm.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bm);
            
            /*float stringWidth = g.MeasureString(m_strText.ToString(), m_font).Width;
            List<string> str = new List<string>();

            if (stringWidth >= width)
            {
                int lastSpace = m_strText.LastIndexOf(' ');
                if (lastSpace == -1)
                {
                    throw new Exception("Can't fit String in image.");
                }
                //string tmp = m_strText.Substring(lastSpace).Trim();
                str.Add(m_strText.Substring(0,lastSpace).Trim());
                str.Add(m_strText.Substring(lastSpace).Trim());
            }*/

            g.Clear(System.Drawing.Color.Transparent);
            /*for (int i = 0; i < str.Count; i++)
            {
                g.DrawString(str[i], m_font, System.Drawing.Brushes.White, new System.Drawing.PointF(0, (0 + i * m_font.Height)));    
            }*/
            //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
            //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit; // default?
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            g.DrawString(m_strText, m_font, System.Drawing.Brushes.White, /*new System.Drawing.PointF(0, 0)*/ new System.Drawing.Rectangle(0, 0, m_drawSize[0], m_drawSize[1]));    
            g.Dispose();

            System.Drawing.Imaging.BitmapData data = bm.LockBits(new System.Drawing.Rectangle(0, 0, bm.Width, bm.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, m_drawSize[0], m_drawSize[1], 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bm.UnlockBits(data);
            data = null;
            bm.Dispose();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }


        public void Draw()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); //(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Begin(BeginMode.Quads);
            /*GL.TexCoord2(0f, 1f); GL.Vertex3(0f, 0f, 1.0f);
            GL.TexCoord2(1f, 1f); GL.Vertex3(1f, 0f, 1.0f);
            GL.TexCoord2(1f, 0f); GL.Vertex3(1f, 1f, 1.0f);
            GL.TexCoord2(0f, 0f); GL.Vertex3(0f, 1f, 1.0f);*/

            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-1.00f, -1.50f, 1.0f); // bottom right
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-1.00f, 0.20f, 1.0f); // top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(1.00f, 0.20f, 1.0f); // top left
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(1.00f, -1.50f, 1.0f); // bottom left

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}
