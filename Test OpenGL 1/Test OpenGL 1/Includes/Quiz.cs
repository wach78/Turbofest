using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;

namespace OpenGL 
{
    class Quiz : IEffect
    {
        private static List<string> listquotes;
        private static List<int> indexList;
        private static int maxIndexValue;

        private Bitmap textBmp;
        int textTexture;
        private Font font;
        private Text2D text;
        private Sound snd;
        private string currentString;
        private bool builtInFont;

        private string LastPlayedDate;
        private string LastDate;

        public Quiz(ref Text2D Text, bool BuiltInFont, ref Sound sound)
        {
            listquotes = new List<string>();
            indexList = new List<int>();
            maxIndexValue = 0;
            text = Text;
            builtInFont = BuiltInFont;
            snd = sound;
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/Yoda.ogg", "YODA");

            readFromXml();
            drawInit(builtInFont);
        }

        public void Dispose()
        {
            //base.Finalize();
            if (textTexture > 0)
                Util.DeleteTexture(ref textTexture);
            System.GC.SuppressFinalize(this);
            System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
        }

        public static void readFromXml()
        {
            string path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/XMLFiles/Quiz/quiz.xml";


            System.Xml.Linq.XDocument xDoc = System.Xml.Linq.XDocument.Load(path);

            var quotes = (
                        from x in xDoc.Elements("questions")
                        select x
                        ).FirstOrDefault();

            foreach (var x in quotes.Elements("question"))
            {
                listquotes.Add(x.Element("id").Value + "\n" + x.Element("txt").Value);
                maxIndexValue++;
            }
        }//readFromXml

        public string getOneRandomquestion()
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

            } while (again);

            return listquotes[index];
        }//getOneRandomquestion

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
                gfx.DrawString(getOneRandomquestion(), font, System.Drawing.Brushes.White, new System.Drawing.Rectangle(0, 0, textBmp.Width, textBmp.Height));
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
                currentString = getOneRandomquestion();
            }
        }

        private void random(string Date)
        {
            if (LastPlayedDate != Date)
            {
                drawInit(false);
                LastPlayedDate = Date;
            }

        }

        public void Play(String Date)
        {
            if (LastDate != Date && snd.PlayingName() != "YODA")
            {
                snd.Play("YODA");
                LastDate = Date;
            }
        }

        public void Draw(string Date)
        {
            Play(Date);
            random(Date);   

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
                text.Draw(currentString, Text2D.FontName.TypeFont, new OpenTK.Vector3(1.4f, 0.10f, 1.0f), new OpenTK.Vector2(0.10f, 0.10f), new OpenTK.Vector2(3.0f, 2.0f), 1.3f);
            }
        }
            
    }//class
}//namespace
