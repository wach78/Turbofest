using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using OpenTK;

namespace OpenGL
{
    class Scroller : IEffect
    {
        private Chess c;
        private Starfield sf;
        private Text2D text;
        private bool disposed = false;

        private float x;
        private float y;
        private float z;
        private float startY;
        private long tick;

        private static List<string> listScrollers;
        private static List<int> indexList;
        private static Random rand;
        private static int maxIndexValue;
        private int chessNumber;
        private int randomBackground;
        private int randomScrollMove;
        
        private string strScroll;

        public Scroller(ref Chess chess, ref Starfield star, ref Text2D txt)
        {
            c = chess;
            sf = star;
            text = txt;
            listScrollers = new List<string>();
            indexList = new List<int>();
            rand = new Random();
            maxIndexValue = 0;
            randomBackground = 0;
            randomScrollMove =  0;
            x = 0.0f;
            y = 0.0f;
            z = 0.0f;
            startY = 0.0f;
            tick = 0;


            readFromXml();
            getRandomScrollerStuff();

        }

        ~Scroller()
        {
            Dispose(false);
        }

        public void Dispose()
        {
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
                    c = null;
                    sf = null;
                    
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");
                disposed = true;
            }
        }

        public void getRandomScrollerStuff()
        {
            chessNumber = rand.Next(0, 6);
            randomBackground = rand.Next(0, 6);
            randomScrollMove = rand.Next(0, 6);
            strScroll = getOneRandomScrollers();
        }

        private static void readFromXml()
        {
            string path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\XMLFiles\\Scrollers\\Scrollers.xml";

            System.Xml.Linq.XDocument xDoc = System.Xml.Linq.XDocument.Load(path);

            var quotes = (
                        from x in xDoc.Elements("scrollers")
                        select x
                        ).FirstOrDefault();

            foreach (var x in quotes.Elements("scroller"))
            {
                listScrollers.Add(x.Element("name").Value);
                maxIndexValue++;
            }
        }//readFromXml

        private string getOneRandomScrollers()
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

            } while (again);

            return listScrollers[index]; 
        }//getOneRandomScrollers


        private void DrawText()
        {
            this.tick++;
            if (randomScrollMove == 0 || randomScrollMove == 1)
            {
                x += 0.01f;
                startY = 0.0f;
                if (x > 3.0f + (0.10f * 1.5f * (float)strScroll.Length))
                    x = -1.5f;

            }
            else if (randomScrollMove == 2)
            {
                x += 0.01f;
                startY = -0.4f;
                y = (float)Math.Abs(0.001 * Math.Sin((this.tick / 42.1) * 3.1415) * 500);

                if (x > 3.0f + (0.10f * 1.5f * (float)strScroll.Length))
                    x = -1.5f;
               
            }

            else if (randomScrollMove >= 3)
            {
                if (strScroll.Length < 12)
                {
                    x = (float)(0.004 * Math.Sin((this.tick / 50.0) * 3.1415) * 200);

                    if (strScroll.Length < 5)
                        x += 1.8f;
                    else
                        x += 2.2f;

                    y = (float)(0.004 * Math.Sin((this.tick / 42.1) * 3.1415) * 150);
                    y += 0.0f;
                }
                else
                {
                    x += 0.01f;
                    startY = -0.4f;
                    y = (float)Math.Abs(0.001 * Math.Sin((this.tick / 42.1) * 3.1415) * 500);

                    if (x > 3.0f + (0.10f * 1.5f * (float)strScroll.Length))
                        x = -1.5f;

                }


            }

            Debug.WriteLine("randomScrollMove");
            Debug.Write(randomScrollMove);
            text.Draw(strScroll, Text2D.FontName.CandyBlue, new Vector3(-1.5f + x,startY + y, 0.4f + z), new OpenTK.Vector2(0.10f, 0.10f), new OpenTK.Vector2(0.0f, 0.0f), 1.5f);
        }



       public void Draw(string Date)
       {
           if (randomBackground == 0 || randomBackground == 1)
           {
               sf.Draw(Date);
           }

           else if (randomBackground >= 2)
           {
               c.Draw(Date, (Chess.ChessColor)chessNumber);
           }

           DrawText();

       }//Draw
    }//class
}//namespace
