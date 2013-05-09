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
            private Vector3 StartPostition;
            private int Texture;

            public Bear(float Width, float Height, float X, float Y, float Z, int TextureID)
            {
                Texture = TextureID;
                fSize = new SizeF(Width, Height);
                StartPostition = new Vector3(X, Y, Z);
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

            public void Draw(string Date)
            {
                GL.BindTexture(TextureTarget.Texture2D, Texture);
                GL.Begin(BeginMode.Quads);
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.2f, 0.0f, 0.3f);
                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(0.7f, 0.0f, 0.3f);
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(0.7f, 0.70f, 0.3f);
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.2f, 0.70f, 0.3f);
                GL.End();
            }
        }

        private bool disposed = false;
        private int[] textures;
        private Bear[] Bears;
        private Sound snd;
        private string LastPlayedDate;

        public GummiBears(ref Sound sound)
        {
            snd = sound;
            textures = new int[7]; // there are 7 in the "newer" gummi bears, 6 in the older (Gusto is a new one)...
            textures[0] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/zummi.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            textures[1] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/grammi.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            textures[2] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/tummi.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            textures[3] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/gruffi.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            textures[4] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/sunni.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            textures[5] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/cubbi.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            textures[6] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/gusto.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/free.ogg", "Gummi");
            LastPlayedDate = string.Empty;
            Bears = new Bear[7];
            Bears[0] = new Bear(0.4f, 0.4f, 0.0f, 0.0f, 0.0f, textures[0]);
            Bears[1] = new Bear(0.4f, 0.4f, 0.0f, 0.0f, 0.0f, textures[1]);
            Bears[2] = new Bear(0.4f, 0.4f, 0.0f, 0.0f, 0.0f, textures[2]);
            Bears[3] = new Bear(0.4f, 0.4f, 0.0f, 0.0f, 0.0f, textures[3]);
            Bears[4] = new Bear(0.3f, 0.35f, 0.0f, 0.0f, 0.0f, textures[4]);
            Bears[5] = new Bear(0.3f, 0.3f, 0.0f, 0.0f, 0.0f, textures[5]);
            Bears[6] = new Bear(0.4f, 0.4f, 0.0f, 0.0f, 0.0f, textures[6]);

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
            if (LastPlayedDate != Date && snd.PlayingName() != "Gummi")
            {
                LastPlayedDate = Date;
                //snd.Play("Gummi");
            }
        }

        public void Draw(string Date)
        {
            PlaySound(Date);

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            /*
            GL.BindTexture(TextureTarget.Texture2D, textures[0]);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.5f, 0.0f, 0.3f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.1f, 0.0f, 0.3f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.1f, 0.40f, 0.3f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.5f, 0.40f, 0.3f);
            GL.End();
            
            GL.BindTexture(TextureTarget.Texture2D, textures[1]);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(0.8f, 0.0f, 0.3f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(0.4f, 0.0f, 0.3f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(0.4f, 0.40f, 0.3f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(0.8f, 0.40f, 0.3f);
            GL.End();
            
            GL.BindTexture(TextureTarget.Texture2D, textures[2]);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(0.2f, 0.0f, 0.3f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-0.2f, 0.0f, 0.3f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-0.2f, 0.40f, 0.3f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(0.2f, 0.40f, 0.3f);
            GL.End();
            
            GL.BindTexture(TextureTarget.Texture2D, textures[3]);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-0.5f, 0.0f, 0.3f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-0.9f, 0.0f, 0.3f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-0.9f, 0.40f, 0.3f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-0.5f, 0.40f, 0.3f);
            GL.End();
            
            GL.BindTexture(TextureTarget.Texture2D, textures[4]);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.1f, -0.45f, 0.3f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(0.9f, -0.45f, 0.3f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(0.9f, -0.1f, 0.3f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.1f, -0.1f, 0.3f);
            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, textures[5]);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(0.6f, -0.4f, 0.3f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(0.3f, -0.4f, 0.3f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(0.3f, -0.1f, 0.3f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(0.6f, -0.1f, 0.3f);
            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, textures[6]);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-0.2f, -0.5f, 0.3f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-0.6f, -0.5f, 0.3f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-0.6f, -0.1f, 0.3f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-0.2f, -0.1f, 0.3f);
            GL.End();
            */
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
        }

    }
}
