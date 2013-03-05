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
    class SuneTxtHandler : IEffect
    {
        private static List<string> listquotes;
        private static List<int> indexList;
        private static Random rand;
        private static int maxIndexValue;

        private Bitmap textBmp;
        int textTexture;
        private Font font;
        private Text2D text;
        private string currentString;
        private bool builtInFont;

        public SuneTxtHandler(ref Text2D Text, bool BuiltInFont)
        {
            listquotes = new List<string>();
            indexList = new List<int>();
            rand = new Random();
            maxIndexValue = 0;
            text = Text;
            builtInFont = BuiltInFont;
           
            readFromXml();
            drawInit(builtInFont);
        }

        public void Dispose()
        {
            //base.Finalize();
            Util.DeleteTexture(ref textTexture);
            System.GC.SuppressFinalize(this);
        }
    
        public static void readFromXml()
        {
            string path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\XMLFiles\\Sune\\sune.xml" ;
            

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

                index = rand.Next(0, maxIndexValue);

                if (!indexList.Contains(index))
                {
                    indexList.Add(index);
                    again = false;
                }      
     
            }while(again);

            return listquotes[/*index*/60]; // 60, 100 bra test för bryttningar...
        }//getOneRandomQuote


   
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
                text.Draw(currentString, Text2D.FontName.TypeFont, new OpenTK.Vector3(0.8f, 0.10f, 1.0f), new OpenTK.Vector2(0.10f, 0.10f), new OpenTK.Vector2(2.8f, 2.0f), 1.0f);
            }
        }
    }//class
}//namespace
