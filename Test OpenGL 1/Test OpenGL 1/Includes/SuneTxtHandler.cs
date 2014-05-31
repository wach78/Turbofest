using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    /// <summary>
    /// SuneTxt effect
    /// </summary>
    class SuneTxtHandler : IEffect
    {
        private static List<string> listquotes;
        private static List<int> indexList;
        private static int maxIndexValue;
        private Bitmap textBmp;
        int textTexture;
        private Font font;
        private Text2D text;
        private string currentString;
        private bool builtInFont;
        private bool disposed = false;

        /// <summary>
        /// Constructor for SuneTxt effect
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="BuiltInFont"></param>
        public SuneTxtHandler(ref Text2D Text, bool BuiltInFont)
        {
            listquotes = new List<string>();
            indexList = new List<int>();
            maxIndexValue = 0;
            text = Text;
            builtInFont = BuiltInFont;
           
            readFromXml();
            drawInit(builtInFont);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~SuneTxtHandler()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing">Is it disposing?</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // free managed resources
                    Util.DeleteTexture(ref textTexture);
                    if (textBmp != null) textBmp.Dispose();
                    if (font != null) font.Dispose();
                    text = null;
                    listquotes.Clear();
                    indexList.Clear();
                }
                // free native resources if there are any.
                System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
                disposed = true;
            }
        }
    
        /// <summary>
        /// Read data from XML-file
        /// </summary>
        public static void readFromXml()
        {
            string path = Util.CurrentExecutionPath + "/XMLFiles/Sune/sune.xml" ;
            

            System.Xml.Linq.XDocument xDoc = System.Xml.Linq.XDocument.Load(path);
         
            var quotes = (
                        from x in xDoc.Elements("quotes")
                        select x
                        ).FirstOrDefault();

            foreach (var x in quotes.Elements("quote"))
            {
                listquotes.Add(x.Element("date").Value + "\n\n" + x.Element("txt").Value);
                maxIndexValue++;
            }
        }//readFromXml

        /// <summary>
        /// Get a random qoute
        /// </summary>
        /// <returns>String with random qoute</returns>
        public string getOneRandomQuote()
        {
            int index = 0;
            bool again = true;

            do
            {
                if (indexList.Count >= maxIndexValue)
                {
                    indexList.Clear();
                }

                index = Util.Rnd.Next(0, maxIndexValue);

                if (!indexList.Contains(index))
                {
                    indexList.Add(index);
                    again = false;
                }      
     
            }while(again);

            return listquotes[index]; 
        }//getOneRandomQuote

        /// <summary>
        /// Draw setup
        /// </summary>
        /// <param name="BuiltIn">To use built in font or not</param>
        public void drawInit(bool BuiltIn)
        {
            if (BuiltIn)
            {
                textBmp = new Bitmap(256, 300);
                font = new Font("times new roman", 20.0f, FontStyle.Bold);

                //textTexture = GL.GenTexture();
                textTexture = Util.GenTextureID();
                GL.BindTexture(TextureTarget.Texture2D, textTexture);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, textBmp.Width, textBmp.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero); 
                Graphics gfx = Graphics.FromImage(textBmp);
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                gfx.Clear(Color.Transparent);
                gfx.DrawString(getOneRandomQuote(), font, System.Drawing.Brushes.White, new System.Drawing.Rectangle(0, 0, textBmp.Width, textBmp.Height));
                gfx.Dispose();

                BitmapData data = textBmp.LockBits(new Rectangle(0, 0, textBmp.Width, textBmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 256, 300, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                textBmp.UnlockBits(data);
                data = null;
                textBmp.Dispose();
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
            else
            {
                currentString = getOneRandomQuote();
            }
        }

        /// <summary>
        /// Draw SuneTxt effect on screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            if (builtInFont)
            {
                GL.BindTexture(TextureTarget.Texture2D, textTexture);
                GL.Enable(EnableCap.Texture2D);
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
                //  GL.Color3(Color.White);
                GL.Begin(BeginMode.Quads);

                GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.8f, -0.85f, 1.0f); // bottom left // x y z
                GL.TexCoord2(1.0, 1.0); GL.Vertex3(-1.0f, -0.85f, 1.0f); // bottom right
                GL.TexCoord2(1.0, 0.0); GL.Vertex3(-1.0f, 0.10f, 1.0f);// top right
                GL.TexCoord2(0.0, 0.0); GL.Vertex3(0.8f, 0.10f, 1.0f); // top left 

                GL.End();

                GL.Disable(EnableCap.Blend);
                GL.Disable(EnableCap.Texture2D);
            }
            else
            {
                text.Draw(currentString, Text2D.FontName.TypeFont, new OpenTK.Vector3(0.8f, 0.10f, 1.0f), new OpenTK.Vector2(0.20f, 0.25f), new OpenTK.Vector2(2.8f, 2.0f), 0.5f);
            }
        }
    }//class
}//namespace
