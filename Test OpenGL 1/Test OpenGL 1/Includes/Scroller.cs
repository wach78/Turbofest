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
    /// <summary>
    /// Scroller effect
    /// Moving/eolling/bouncy text over the screen
    /// </summary>
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
        private static int maxIndexValue;
        private int chessNumber;
        private int randomBackground;
        private int randomScrollMove;
        private int randomFont;
        private string LastPlayedDate; 
        private string strScroll;

        /// <summary>
        /// Constructor for Scroller effect
        /// </summary>
        /// <param name="chess">Chessboard</param>
        /// <param name="star">Starfield</param>
        /// <param name="txt">Text printer</param>
        public Scroller(ref Chess chess, ref Starfield star, ref Text2D txt)
        {
            c = chess;
            sf = star;
            text = txt;
            listScrollers = new List<string>();
            indexList = new List<int>();
            maxIndexValue = 0;
            randomBackground = 0;
            randomScrollMove =  0;
            randomFont = 0;
            x = 0.0f;
            y = 0.0f;
            z = 0.0f;
            startY = 0.0f;
            tick = 0;
            LastPlayedDate = string.Empty;

            readFromXml();
            getRandomScrollerStuff();

        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Scroller()
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
                    c = null;
                    sf = null;
                    
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");
                disposed = true;
            }
        }

        /// <summary>
        /// Setup scroller effect
        /// </summary>
        public void getRandomScrollerStuff()
        {
            chessNumber = Util.Rnd.Next(0, 6);
            randomBackground = Util.Rnd.Next(0, 6);
            randomScrollMove = Util.Rnd.Next(0, 6);
            randomFont = Util.Rnd.Next(0, 6);
            strScroll = getOneRandomScrollers();

            x = 0.0f;
            y = 0.0f;
            z = 0.0f;
            tick = 0;
        }

        /// <summary>
        /// Read in data from a XML-file
        /// </summary>
        private static void readFromXml()
        {
            string path = Util.CurrentExecutionPath + "/XMLFiles/Scrollers/Scrollers.xml";

            // Quick and dirty to show that we are missing files!
            if (!System.IO.File.Exists(path))
            {
                throw new Exception("Missing XMLFiles/Scrollers/Scrollers.xml!");
            }

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

        /// <summary>
        /// Get random scroller text
        /// </summary>
        /// <returns>String with the text to show</returns>
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

                index = Util.Rnd.Next(0, maxIndexValue);

                if (!indexList.Contains(index))
                {
                    indexList.Add(index);
                    again = false;
                }

            } while (again);

            return listScrollers[index]; 
        }//getOneRandomScrollers

        /// <summary>
        /// Draw text to screen
        /// </summary>
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
                    startY = 0.0f;
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

            /*Debug.WriteLine("randomScrollMove");
            Debug.Write(randomScrollMove);*/
            text.Draw(strScroll, (Text2D.FontName)randomFont , new Vector3(-1.5f + x,startY + y, 0.4f + z), new OpenTK.Vector2(0.10f, 0.10f), new OpenTK.Vector2(0.0f, 0.0f), 1.5f);
        }

        /// <summary>
        /// Reset scroller with random stuff
        /// </summary>
        /// <param name="Date">New date?</param>
        private void allRandom(string Date)
        {
            if (LastPlayedDate != Date)
            {
                getRandomScrollerStuff();
                LastPlayedDate = Date;
            }
        }

        /// <summary>
        /// Draw Scroller effect on screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            allRandom(Date);

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
