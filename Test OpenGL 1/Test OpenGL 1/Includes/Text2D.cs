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
        private bool _disposed = false;
        string m_strText;
        string[] AllowedChars;
        char[] splitChars;
        int[] texture;
        float[,] textureSize;
        float[,] FontSize;
        System.Drawing.Font m_font;
        int[] m_drawSize;
        //string oldDate;

        public enum FontName { Coolfont=0, CandyBlue, CandyGreen, CandyGrey, CandyPink, CandyPurple, CandyYellow, TypeFont, Other };

        public Text2D()
        {
            texture = new int[9];
            textureSize = new float[9, 2];
            FontSize = new float[9, 2];
            AllowedChars = new string[9];
            //use the enum or not that is the question :D
                                 
            texture[(int)FontName.Coolfont] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/coolfont_db.bmp", out textureSize[0, 0], out textureSize[0, 1], TextureMinFilter.Nearest, TextureMagFilter.Nearest, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(0, 0, 0));
            texture[(int)FontName.CandyBlue] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/scroller2_db_blue.bmp", out textureSize[1, 0], out textureSize[1, 1], TextureMinFilter.Nearest, TextureMagFilter.Nearest, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(0, 0, 0));
            texture[(int)FontName.CandyGreen] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/scroller2_db_green.bmp", out textureSize[2, 0], out textureSize[2, 1], TextureMinFilter.Nearest, TextureMagFilter.Nearest, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(0, 0, 0));
            texture[(int)FontName.CandyGrey] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/scroller2_db_grey.bmp", out textureSize[3, 0], out textureSize[3, 1], TextureMinFilter.Nearest, TextureMagFilter.Nearest, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(0, 0, 0));
            texture[(int)FontName.CandyPink] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/scroller2_db_pink.bmp", out textureSize[4, 0], out textureSize[4, 1], TextureMinFilter.Nearest, TextureMagFilter.Nearest, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(0, 0, 0));
            texture[(int)FontName.CandyPurple] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/scroller2_db_purple.bmp", out textureSize[5, 0], out textureSize[5, 1], TextureMinFilter.Nearest, TextureMagFilter.Nearest, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(0, 0, 0));
            texture[(int)FontName.CandyYellow] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/scroller2_db_yellow.bmp", out textureSize[6, 0], out textureSize[6, 1], TextureMinFilter.Nearest, TextureMagFilter.Nearest, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(0, 0, 0));
            texture[(int)FontName.TypeFont] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/typefont15x25.bmp", out textureSize[7, 0], out textureSize[7, 1], TextureMinFilter.Nearest, TextureMagFilter.Nearest, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(0, 0, 0));
            texture[(int)FontName.Other] = Util.GenTextureID(); // used for builtintext generation

            FontSize[0, 0] = FontSize[0, 1] = 32; // 32 x 32 size of char
            FontSize[1, 0] = FontSize[1, 1] = FontSize[2, 0] = FontSize[2, 1] = FontSize[3, 0] = FontSize[3, 1] = FontSize[4, 0] = FontSize[4, 1] = FontSize[5, 0] = FontSize[5, 1] = FontSize[6, 0] = FontSize[6, 1] = 32; // 31 x 31 size of char
            FontSize[7, 0] = 15;
            FontSize[7, 1] = 25;

            AllowedChars[0] = "ABCDEFGHIJKLMNOPQRSTUVWXYZ!?:;0123456789\"()-ÅÄÖ* ";
            AllowedChars[1] = AllowedChars[2] = AllowedChars[3] = AllowedChars[4] = AllowedChars[5] = AllowedChars[6] = "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ!?- ";
            AllowedChars[7] = " ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖabcdefghijklmnopqrstuvwxyzåäö0123456789()!:;.,-+\"'@&*?";
            AllowedChars[8] = "";

            splitChars = new char[] { ' ', '.', '-', '\n' }; // set this to each Font as not all have them..
        }

        ~Text2D()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            //base.Finalize();
            //GL.DeleteBuffers(2, this.m_tex); // this might bug out or?
            //this.m_tex = null; // protects for miss use if GC haven't been collecting...
            
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // If you need thread safety, use a lock around these  
            // operations, as well as in your methods that use the resource. 
            if (!_disposed)
            {
                if (disposing)
                {
                    if (texture != null)
                    {
                        //do a loop so taht we don't need to add as we add new textures...
                        Util.DeleteTexture(ref texture[0]);
                        Util.DeleteTexture(ref texture[1]);
                        Util.DeleteTexture(ref texture[2]);
                        Util.DeleteTexture(ref texture[3]);
                        Util.DeleteTexture(ref texture[4]);
                        Util.DeleteTexture(ref texture[5]);
                        Util.DeleteTexture(ref texture[6]);
                        Util.DeleteTexture(ref texture[7]);
                        Util.DeleteTexture(ref texture[8]);
                    }
                    System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
                }

                // Indicate that the instance has been disposed.
                texture = null;
                _disposed = true;
            }
        }


        public Vector2[] TextureCoordinates(char Character, FontName Font)
        {
            Vector2[] texVec = new Vector2[4];
            float Row = 0, Column = 0;
            int intFont = (int)Font;

            if (Character == '?')
            {
                ;
            }

            texVec[0] = texVec[1] = texVec[2] = texVec[3] = Vector2.Zero;
            if (AllowedChars[intFont].ToLower().Contains(Character) || AllowedChars[intFont].Contains(Character))
            {
                texVec[0] = texVec[1] = texVec[2] = texVec[3] = Vector2.Zero;
                Column = AllowedChars[intFont].IndexOf(Character) * FontSize[intFont, 0];
                if (Column < 0) // dirty way but works....
                {
                    Column = AllowedChars[intFont].ToLower().IndexOf(Character) * FontSize[intFont, 0];
                }
                while (Column >= textureSize[intFont, 0])
                {
                    Row += FontSize[intFont, 1];
                    Column -= textureSize[intFont, 0];
                    /*if (intFont >= 1 && intFont <= 6)
                    {
                        Row -= -5;
                    }*/
                }

                // top left
                texVec[3].X = Column / textureSize[intFont, 0];
                texVec[3].Y = (Row + FontSize[intFont, 1] - (intFont >= 1 && intFont <= 6? 5:0)) / textureSize[intFont, 1];
                
                // bottom left
                texVec[2].X = Column / textureSize[intFont, 0];
                texVec[2].Y = Row / textureSize[intFont, 1];
                
                // Bottom right
                texVec[1].X = (Column + FontSize[intFont, 0]) / textureSize[intFont, 0];
                texVec[1].Y = Row / textureSize[intFont, 1];
                
                // top right
                texVec[0].X = (Column + FontSize[intFont, 0]) / textureSize[intFont, 0];
                texVec[0].Y = (Row + FontSize[intFont, 1] - (intFont >= 1 && intFont <= 6 ? 5 : 0)) / textureSize[intFont, 1];
            }
            /*else // this make alot of crashes if not handled
            {
                texVec = null;
            }*/
            return texVec;
        }

        public Vector2[,] TextureCoordinates(string Text, FontName Font)
        {
            Vector2[,] texVec = new Vector2[Text.Length, 4];
            Vector2[] tmpVec = new Vector2[4];
            for (int i = 0; i < Text.Length; i++)
            {
                tmpVec = TextureCoordinates(Text[i], Font);
                texVec[i, 0] = tmpVec[0];
                texVec[i, 1] = tmpVec[1];
                texVec[i, 2] = tmpVec[2];
                texVec[i, 3] = tmpVec[3];
            }
            return texVec;
        }

        public Vector3[] TextVertix(string Text, FontName Font, float Size, Vector3 StartPosition = new Vector3(), Vector2 WidthHeight = new Vector2())
        {
            Vector3[] TextVec = new Vector3[Text.Length*4];
            Vector3[] tmpVec = new Vector3[4];
            List<string> strRow = new List<string>();
            int intFont = (int)Font;
            float X, Y;
            X = StartPosition.X;
            Y = StartPosition.Y;
             // if we find this we can try to use does to split the line.
            //var fSize = FontSize.GetValue(intFont);
            //var tSize = textureSize.GetValue(intFont);

            //Text = Text.Replace("\r", "");

            // Measure string
            //string[] RealRow = Text.Replace("\r", "").Split(new char[] {'\n'}, StringSplitOptions.None);
            List<string> RealRow = new List<string>();
            RealRow.AddRange(Text.Replace("\r", "").Split(new char[] { '\n' }, StringSplitOptions.None));
            float width;
            if (/*StartPosition != Vector3.Zero &&*/ WidthHeight != Vector2.Zero)
            {
                System.Diagnostics.Debug.WriteLine("We can get size of text area");
                // Nasty stuff :D
                for (int y = 0; y < RealRow.Count; y++)
                {
                    width = MeasureString(RealRow[y], Size);
                    if (width > WidthHeight.X) // this needs to be calculated from the parameters left and right
                    {
                        int LastSplitIndex = RealRow[y].LastIndexOfAny(splitChars);

                        if (LastSplitIndex == -1)
                        {
                            // split at max with and test again and repeat until fit
                            string NewSplit = RealRow[y].Substring(0, RealRow[y].Length / 2);
                            string NewSplit2 = RealRow[y].Substring(RealRow[y].Length / 2);
                            RealRow[y] = NewSplit;
                            RealRow.Insert(y + 1, NewSplit2);
                            y--;
                        }
                        else
                        {
                            string[] newRows = SplitFitString(RealRow[y], Size, WidthHeight.X);
                            RealRow.InsertRange(y + 1, newRows);
                            RealRow.RemoveAt(y);
                            y += newRows.Length - 1;
                        }
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("We can print text all over the place :)");
                
            }
            

            int Vint = 0;
            for (int y = 0; y < RealRow.Count; y++)
            {
                Y = y * Size;
                for (int x = 0; x < RealRow[y].Length; x++)
                {
                    TextVec[Vint] = new Vector3(X - ((x + 1) * Size), Y - Size, StartPosition.Z);
                    TextVec[++Vint] = new Vector3(X - ((x + 1) * Size), Y, StartPosition.Z);
                    TextVec[++Vint] = new Vector3(X - (x * Size), Y, StartPosition.Z);
                    TextVec[++Vint] = new Vector3(X - (x * Size), Y - Size, StartPosition.Z);
                    Vint++;
                }
            }

            return TextVec;
        }

        public float MeasureString(string Text, float Size)
        {
            return Text.Length * Size;
        }

        public string[] SplitFitString(string Text, float Size, float MaxWidth=0.0f)
        {
            List<string> RealRow = new List<string>();
            List<string> Rows = new List<string>();
            //Rows.InsertRange(0, Text.Split(splitChars));
            if (Text.IndexOf('\n') != -1)
            {
                Rows.AddRange(Text.Replace("\r", "").Split(new char[] { '\n' }, StringSplitOptions.None));    
            }
            else
            {
                Rows.Add(Text);
            }

            if (MaxWidth == 0.0f)
            {
                RealRow.AddRange(Rows);
            }
            else
            {
                for (int i = 0; i < Rows.Count; i++)
                {
                    string escReg = System.Text.RegularExpressions.Regex.Escape(".");
                    string[] split = System.Text.RegularExpressions.Regex.Split(Rows[i], "( |-|,|" + escReg + ")");
                    int ii = i;
                    string newRow = "";
                    for (int y = 0; y < split.Length; y++)
                    {
                        if (split[y] == "")
                        {
                            continue;
                        }
                        if (MeasureString(newRow + split[y], Size) >= MaxWidth && newRow != "")
                        {
                            RealRow.Add(newRow.Trim());
                            y--;
                            newRow = "";
                        }
                        else
                        {
                            newRow += split[y];
                        }
                    }
                    /*if (newRow != "")
                    {*/
                        RealRow.Add(newRow.Trim());
                        newRow = "";
                    //}
                }
            }
            return RealRow.ToArray<string>();
        }

        /// <summary>
        /// 2D text to be writen to the screen
        /// </summary>
        /// <param name="width">Width of the area to write on</param>
        /// <param name="height">Height of the area to write on</param>
        public void BuiltinText(int width, int height, string ToPrint)
        {
            m_strText = ToPrint;
            m_font = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 12.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            m_drawSize = new int[2] { width, height };

            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(m_drawSize[0], m_drawSize[1]);
            GL.BindTexture(TextureTarget.Texture2D, texture[0]);
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

        /*public void Draw(string Text, FontName Font, float X, float Y, float Z, float Scale = 1.0f)
        {
            Draw(Text, Font, new Vector3(X, Y, Z), new Vector2(-2.0f, 0.0f), new Vector2(-2.0f, 0.0f));
        }*/

        private Vector2 ScaleVector2(Vector2 ToScale, float Scale)
        {
            return Vector2.Multiply(ToScale, Scale);
        }

        public void Draw(string Text, FontName Font, Vector3 Position, Vector2 CharSize, Vector2 MaxSize, float Scale = 1.0f)
        {
            CharSize = Vector2.Multiply(CharSize, Scale); // scale the char size
            Vector2[] texVec = new Vector2[4];
            float X, Y;
            string[] splitText = SplitFitString(Text, CharSize.X, MaxSize.X); // Split the text to fit in MaxSize

            

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); //(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
            GL.BindTexture(TextureTarget.Texture2D, texture[(int)Font]);
            GL.Begin(BeginMode.Quads);
            
            X = Position.X;
            Y = Position.Y;
            for (int y = 0; y < splitText.Length; y++)
            {
                for (int i = 0; i < splitText[y].Length; i++)
                {
                    texVec = TextureCoordinates(splitText[y][i], Font);
                    GL.TexCoord2(texVec[0]); GL.Vertex3(X - ((i + 1) * CharSize.X), Y - CharSize.Y, Position.Z); // bottom right
                    GL.TexCoord2(texVec[1]); GL.Vertex3(X - ((i + 1) * CharSize.X), Y, Position.Z); // top right
                    GL.TexCoord2(texVec[2]); GL.Vertex3(X - (i * CharSize.X), Y, Position.Z); // top left
                    GL.TexCoord2(texVec[3]); GL.Vertex3(X - (i * CharSize.X), Y - CharSize.Y, Position.Z); // bottom left
                }
                //Y -= 0.1f;
                Y -= CharSize.Y;
            }

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Draw(string Date)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); //(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
            GL.BindTexture(TextureTarget.Texture2D, texture[0]);
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
