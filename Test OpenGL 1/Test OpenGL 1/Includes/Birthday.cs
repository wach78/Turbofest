using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace OpenGL
{
    /// <summary>
    /// Birthday effect
    /// </summary>
    class Birthday : IEffect
    {
        private int image;
        private int ballonsImage;
        private Sound snd;
        private Text2D text;
        private Chess chess;
        private int currentImage;
        private Ballons[] b;
        private const int NUMBEROFBALLONS = 20;
        private bool disposed = false;
        private int chessNumber;
        private long tick;
        private float y;
        private float x;
        private bool leftborder;
        private bool rightborder;
        private string LastPlayedDate;
        private int randomFontt;

        /// <summary>
        /// Constructor for Birthday effect
        /// </summary>
        /// <param name="sound">Sound system</param>
        /// <param name="txt">Text printing done with this</param>
        /// <param name="chess">Chessboard</param>
        public Birthday(ref Sound sound, ref Text2D txt, ref Chess chess)
        {
            image = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/tarta.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            ballonsImage = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/ballons2.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            snd = sound;
            text = txt;
            this.chess = chess;
            x = 0.0f;
            leftborder = false;
            rightborder = true;
            randomFontt = 0;
            randomFont();

            //snd.CreateSound(Sound.FileType.WAV, Util.CurrentExecutionPath + "/Samples/birthday.wav", "Birthday");
            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/birthday.ogg", "Birthday");
            currentImage = 0;
            
            b = new Ballons[NUMBEROFBALLONS];
            float z = 0.4f;

            for (int i = 0; i < NUMBEROFBALLONS; i++)
            {
                b[i] = new Ballons((Util.Rnd.Next(-18, 15)) / 10.0f, (Util.Rnd.Next(-10, 20) * -1) / 10.0f, Util.Rnd.Next(2, 8) / 1000.0f, ballonsImage,
                    new Vector2[] {  new Vector2(0.0f + (currentImage * 0.2f), 1.0f),
                                     new Vector2(0.2f + (currentImage * 0.2f), 1.0f),
                                     new Vector2(0.2f + (currentImage * 0.2f), 0.0f),
                                     new Vector2(0.0f + (currentImage * 0.2f), 0.0f)}, Util.Rnd.Next(5, 10) * 10.0f, z);


                z -= 0.00001f;
                currentImage++;

                if (currentImage == 4)
                    currentImage = 0;

            }//for
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Birthday()
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
                    Util.DeleteTexture(ref image);
                    Util.DeleteTexture(ref ballonsImage);
                    for (int i = 0; i < NUMBEROFBALLONS; i++)
                    {
                        b[i].Dispose();
                        b[i] = null;
                    }
                }
                // free native resources if there are any.

                System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");

                Debug.WriteLine(this.GetType().ToString() + " disposed.");

                disposed = true;
            }
        }

        /// <summary>
        /// Randomizes the font used
        /// </summary>
        private void randomFont()
        {
            randomFontt = Util.Rnd.Next(0, 6);
            chessNumber = Util.Rnd.Next(0, 6);
        }

        /// <summary>
        /// Draw image on screen
        /// </summary>
        private void drawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            /*
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(-0.8f, -1.2f, 0.8f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-1.8f, -1.2f, 0.8f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-1.8f, -0.2f, 0.8f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(-0.8f, -0.2f, 0.8f); // top left 
            */
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(-0.8f + x, -1.2f + y, 0.8f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-1.8f + x, -1.2f + y, 0.8f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-1.8f + x, -0.2f + y, 0.8f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(-0.8f + x, -0.2f + y, 0.8f); // top left 

            GL.End();

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

            this.tick++;

            if (x > 2.7)
            {
                leftborder = false;
                rightborder = true;
            }

            if (x < -0.1)
            {
                leftborder = true;
                rightborder = false;
            }

            if (!leftborder) 
                 x -= 0.02f;
            else if (!rightborder)
                x += 0.02f;

        //   y = (float) Math.Abs( ((0.001 * Math.Sin(500  * (Math.PI / 180)) + 0.005)) * 200);
            y = (float)Math.Abs(0.001 * Math.Sin((this.tick / 42.1) * 3.1415) * 500);
            //y = 400 - y;

            
            for (int i = 0; i < NUMBEROFBALLONS; i++)
            {
                b[i].Draw("");
            }
            

        }//DrawImage

        /// <summary>
        /// Play sound
        /// </summary>
        /// <param name="Date">Is it a new day?</param>
        public void Play(string Date)
        {
  
            if (LastPlayedDate != Date && snd.PlayingName() != "Birthday")
            {
                LastPlayedDate = Date;
                snd.Play("Birthday");
                randomFont();
            }
        }

        /// <summary>
        /// Draw text on screen
        /// </summary>
        /// <param name="name">String to be printed on screen</param>
        private void drawText(string name)
        {
            float y = 0.0f;
            string[] names = name.Split('\n');

            foreach (var n in names)
            {
                float middle = n.Length / 2.0f;
                text.Draw(n, (Text2D.FontName)randomFontt, new Vector3(middle * 0.2f, 0.2f-y, 1.0f), new Vector2(0.2f, 0.2f), new Vector2());
                y += 0.15f;
            }
        }

       /// <summary>
       /// Draw Birthday on screen
       /// </summary>
       /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            Play(Date);
            chess.Draw(Date, (Chess.ChessColor)chessNumber);
            drawText("Grattis!");
            drawImage();
        }//Draw

        /// <summary>
        /// Draw Birthday on screen
        /// </summary>
        /// <param name="Date">Current date</param>
        /// <param name="names">String to be printed on screen</param>
        public void Draw(string Date, string names)
        {       
            Play(Date); 
            chess.Draw(Date, (Chess.ChessColor)chessNumber);
            drawText("Grattis!\n\n" + names);
            drawImage();
        }//Draw

    }//class
}//namespace
