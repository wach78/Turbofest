using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio.OpenAL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;

namespace OpenGL
{
    /// <summary>
    /// Gummi Bears effect
    /// </summary>
    class GummiBears : IEffect
    {
        /// <summary>
        /// Bear class
        /// </summary>
        class Bear : IEffect
        {
            private bool disposed = false;
            private SizeF fSize;
            private Vector2 SpeedXY;
            private Vector3 StartPostition;
            private int Texture;
            private float X;
            private float Y;
            private bool MoveLeft;

            /// <summary>
            /// Constructor for a bear
            /// </summary>
            /// <param name="Width">Width of the bear</param>
            /// <param name="Height">Height of the bear</param>
            /// <param name="X">X-position</param>
            /// <param name="Y">Y-position</param>
            /// <param name="Z">Z-position</param>
            /// <param name="SpeedX">Speed in Y-axis</param>
            /// <param name="SpeedY">Speed in X-axis</param>
            /// <param name="TextureID">TextureID to use</param>
            public Bear(float Width, float Height, float X, float Y, float Z, float SpeedX, float SpeedY, int TextureID)
            {
                Texture = TextureID;
                fSize = new SizeF(Width, Height);
                SpeedXY = new Vector2(SpeedX, SpeedY);
                StartPostition = new Vector3(X, Y, Z);
                this.X = X;
                this.Y = Y;
                MoveLeft = (Util.Rnd.Next(0, 100) < 50? true:false);
            }

            /// <summary>
            /// Constructor for a bear
            /// </summary>
            /// <param name="size">Size of the bear</param>
            /// <param name="start">Start position</param>
            /// <param name="SpeedXY">Speed in X- and Y-axis</param>
            /// <param name="TextureID">TextureID to use</param>
            public Bear(SizeF size, Vector3 start, Vector2 SpeedXY, int TextureID)
            {
                Texture = TextureID;
                fSize = size;
                this.SpeedXY = SpeedXY;
                StartPostition = start;
                X = StartPostition.X;
                Y = StartPostition.Y;
                MoveLeft = (Util.Rnd.Next(0, 1) == 1 ? true : false);
            }

            /// <summary>
            /// Destructor
            /// </summary>
            ~Bear()
            {
                Dispose(false);
            }

            /// <summary>
            /// Dispose method
            /// </summary>
            public void Dispose()
            {
                //base.Finalize();
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
                        
                    }
                    // free native resources if there are any.
                    disposed = true;
                    Debug.WriteLine(this.GetType().ToString() + " disposed.");

                }
            }

            /// <summary>
            /// Move bear
            /// </summary>
            public void Update()
            {
                
                if (X >= (1.6f - fSize.Width))
                {
                    MoveLeft = true;
                }
                else if (X <= -1.62f)
                {
                    MoveLeft = false;
                }

                X += (MoveLeft? -1:1) * SpeedXY.X;

                Y = (float)Math.Sin(X*700 / 42.1f) * 0.4f - fSize.Height * 1.45f;
            }

            /// <summary>
            /// Draw Bear on screen
            /// </summary>
            /// <param name="Date">Current date</param>
            public void Draw(string Date)
            {
                GL.BindTexture(TextureTarget.Texture2D, Texture);
                GL.Enable(EnableCap.Texture2D);
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

                GL.Begin(BeginMode.Quads);
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(X + fSize.Width, Y, StartPostition.Z);
                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(X, Y, StartPostition.Z);
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(X, Y + fSize.Height, StartPostition.Z);
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(X + fSize.Width, Y + fSize.Height, StartPostition.Z);
                GL.End();

                GL.Disable(EnableCap.Blend);
                GL.Disable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, 0);
                Update();
            }
        }

        private bool disposed = false;
        private int[] textures;
        private int currentSound;
        private string[] songName;
        private Bear[] Bears;
        private Sound snd;
        private string LastPlayedDate;
        private List<int> PlayedSongs;

        /// <summary>
        /// Constructor for GummiBears effect
        /// </summary>
        /// <param name="sound">Sound system</param>
        public GummiBears(ref Sound sound)
        {
            PlayedSongs = new List<int>();
            snd = sound;

            songName = new string[8];
            songName[0] = "GummiSwe";
            songName[1] = "GummiDan";
            songName[2] = "GummiEng";
            songName[3] = "GummiGer";
            songName[4] = "GummiJap";
            songName[5] = "GummiNor";
            songName[6] = "GummiPol";
            songName[7] = "GummiRus";

            textures = new int[7]; // there are 7 in the "newer" gummi bears, 6 in the older (Gusto is a new one)...
            textures[0] = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/zummi.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            textures[1] = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/grammi.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            textures[2] = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/tummi.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            textures[3] = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/gruffi.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            textures[4] = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/sunni.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            textures[5] = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/cubbi.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            textures[6] = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/gusto.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/GummiBears-Swedish.ogg", songName[0]);
            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/GummiBears-Danish.ogg", songName[1]);
            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/GummiBears-English.ogg", songName[2]);
            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/GummiBears-German.ogg", songName[3]);
            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/GummiBears-Japanese.ogg", songName[4]);
            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/GummiBears-Norwegian.ogg", songName[5]);
            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/GummiBears-Polish.ogg", songName[6]);
            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/GummiBears-Russian.ogg", songName[7]);

            currentSound = Util.Rnd.Next(0, songName.Length); // do this random and add to list of played?
            PlayedSongs.Add(currentSound);
            LastPlayedDate = string.Empty;
            Bears = new Bear[7];
            // left max = 1.2, right max = -1.7,
            Bears[0] = new Bear(0.4f, 0.4f, 1.0f, -0.5f, 0.40006f, Util.Rnd.Next(2000, 6500) / 1000000.0f, 0.0045f, textures[0]);
            Bears[1] = new Bear(0.4f, 0.4f, 0.75f, 0.0f, 0.40005f, Util.Rnd.Next(2000, 6500) / 1000000.0f, 0.0045f, textures[1]);
            Bears[2] = new Bear(0.4f, 0.4f, 0.5f, 0.0f, 0.40004f, Util.Rnd.Next(2000, 6500) / 1000000.0f, 0.0045f, textures[2]);
            Bears[3] = new Bear(0.4f, 0.4f, 0.0f, 0.0f, 0.40003f, Util.Rnd.Next(2000, 6500) / 1000000.0f, 0.0045f, textures[3]);
            Bears[4] = new Bear(0.25f, 0.35f, -0.5f, 0.0f, 0.40002f, Util.Rnd.Next(2000, 6500) / 1000000.0f, 0.0045f, textures[4]);
            Bears[5] = new Bear(0.3f, 0.3f, -1.0f, 0.0f, 0.40001f, Util.Rnd.Next(2000, 6500) / 1000000.0f, 0.0045f, textures[5]);
            Bears[6] = new Bear(0.4f, 0.4f, -1.5f, 0.0f, 0.40000f, Util.Rnd.Next(2000, 6500) / 1000000.0f, 0.0045f, textures[6]);

        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~GummiBears()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            //base.Finalize();
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
                    for (int i = 0; i < textures.Length; i++)
                    {
                        Util.DeleteTexture(ref textures[i]);
                        if (Bears[i] != null) Bears[i].Dispose();
                    }
                    snd = null;
                }
                // free native resources if there are any.
                disposed = true;
                Debug.WriteLine(this.GetType().ToString() + " disposed.");
            }
        }

        /// <summary>
        /// Play sound
        /// </summary>
        /// <param name="Date">New date?</param>
        public void PlaySound(string Date)
        {
            if (LastPlayedDate != Date && !songName.Contains( snd.PlayingName()))
            {
                LastPlayedDate = Date;
                snd.Play(songName[currentSound]);
                currentSound = Util.Rnd.Next(0, songName.Length);
                PlayedSongs.Add(currentSound); // make sure that we have this stored and reseted if we have all in it.
                /*currentSound++;
                if (currentSound > 7)
                {
                    currentSound = 0;
                }*/
            }
        }

        /// <summary>
        /// Update bears position
        /// </summary>
        public void UpdateSceen()
        {
            foreach (Bear item in Bears)
            {
                item.Update();
            }
        }

        /// <summary>
        /// Draw GummiBears effect on screen
        /// </summary>
        /// <param name="Date"></param>
        public void Draw(string Date)
        {
            PlaySound(Date);

            foreach (Bear item in Bears)
            {
                item.Draw(Date);
            }
        }

    }
}
