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

            ~Bear()
            {
                Dispose(false);
            }

            public void Dispose()
            {
                //base.Finalize();
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
                        
                    }
                    // free native resources if there are any.
                    disposed = true;
                    Debug.WriteLine(this.GetType().ToString() + " disposed.");

                }
            }

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

        public GummiBears(ref Sound sound)
        {
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
            textures[0] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/zummi.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            textures[1] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/grammi.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            textures[2] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/tummi.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            textures[3] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/gruffi.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            textures[4] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/sunni.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            textures[5] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/cubbi.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            textures[6] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/gusto.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/GummiBears-Swedish.ogg", songName[0]);
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/GummiBears-Danish.ogg", songName[1]);
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/GummiBears-English.ogg", songName[2]);
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/GummiBears-German.ogg", songName[3]);
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/GummiBears-Japanese.ogg", songName[4]);
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/GummiBears-Norwegian.ogg", songName[5]);
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/GummiBears-Polish.ogg", songName[6]);
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/GummiBears-Russian.ogg", songName[7]);

            currentSound = 0; // do this random and add to list of played?

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

        ~GummiBears()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            //base.Finalize();
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
                    for (int i = 0; i < textures.Length; i++)
                    {
                        Util.DeleteTexture(ref textures[i]);
                    }
                    snd = null;
                }
                // free native resources if there are any.
                disposed = true;
                Debug.WriteLine(this.GetType().ToString() + " disposed.");
            }
        }

        public void PlaySound(string Date)
        {
            if (LastPlayedDate != Date && !songName.Contains( snd.PlayingName()))
            {
                LastPlayedDate = Date;
                snd.Play(songName[currentSound]);
                currentSound++;
                if (currentSound > 7)
                {
                    currentSound = 0;
                }
            }
        }

        public void UpdateSceen()
        {
            foreach (Bear item in Bears)
            {
                item.Update();
            }
        }

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
