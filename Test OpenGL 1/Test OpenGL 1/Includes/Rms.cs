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
    /// Richard Stalman.
    /// </summary>
    class RMS : IEffect
    {
        private bool disposed = false;
        private int texture;
        private string[] SongText;
        private int CurrentRow;
        private long lastTick;
        private Sound snd;
        private Text2D txt;
        private int DelayRow;
        private string LastPlayedDate;

        public RMS(ref Sound sound, ref Text2D text)
        {
            snd = sound;
            txt = text;
            texture = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/rms.jpg", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.Yellow);
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/free.ogg", "rms");
            SongText = new string[9];
            CurrentRow = -1;
            DelayRow = 3500; // milisecond
            LastPlayedDate = string.Empty;
            SongText[0] = "Join us now and share the software. You'll be free, hackers, you'll be free.";
            SongText[1] = "Join us now and share the software. You'll be free, hackers, you'll be free.";
            SongText[2] = "Hoarders can get piles of money, That is true, hackers, that is true.";
            SongText[3] = "But they cannot help their neighbors; That's not good, hackers, that's not good.";
            SongText[4] = "When we have enough free software At our call, hackers, at our call.";
            SongText[5] = "We'll kick out those dirty licenses Ever more, hackers, ever more.";
            SongText[6] = "Join us now and share the software. You'll be free, hackers, you'll be free.";
            SongText[7] = "Join us now and share the software. You'll be free, hackers, you'll be free.";
            //SongText[8] = "Richard Stallman - a true hacker's birthday!";
            SongText[8] = "Richard Stallman - a true hacker!";
            lastTick = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond; // lastTick is in milisecond
        }

        ~RMS()
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
                    Util.DeleteTexture(ref texture);
                    snd = null;
                }
                // free native resources if there are any.
                disposed = true;
                Debug.WriteLine(this.GetType().ToString() + " disposed.");

            }
        }

        public void PlaySound(string Date)
        {
            if (LastPlayedDate != Date && snd.PlayingName() != "rms")
            {
                LastPlayedDate = Date;
                snd.Play("rms");
                CurrentRow = -1;
            }
        }

        public void Draw(string Date)
        {
            PlaySound(Date);
            if ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) > lastTick + DelayRow && CurrentRow < 8)
            {
                lastTick = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                CurrentRow++;
            }
            switch (CurrentRow)
            {
                case 0:
                    DelayRow = 12000;
                    break;
                case 1:
                    DelayRow = (27 - 15) * 1000;
                    break;
                case 2:
                    DelayRow = (39 - 27) * 1000;
                    break;
                case 3:
                    DelayRow = (52 - 39) * 1000;
                    break;
                case 4:
                    DelayRow = (63 - 52) * 1000;
                    break;
                case 5:
                    DelayRow = (76 - 63) * 1000;
                    break;
                case 6:
                    DelayRow = (87 - 76) * 1000;
                    break;
                case 7:
                    DelayRow = 6000;
                    break;
                default:
                    
                    break;
            }

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(2.2f, -1.15f, 1.0f); // bottom left // x y z alla i mitten Y-led 
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, -1.15f, 1.0f); // bottom right // alla till vänster x-led
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, 0.10f, 1.0f);// top right
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(2.2f, 0.10f, 1.0f); // top left 

            GL.End();
            GL.Disable(EnableCap.Texture2D);

            txt.Draw(SongText[CurrentRow == -1 ? 8 : CurrentRow], Text2D.FontName.TypeFont, new OpenTK.Vector3(0.9f, 0.10f, 1.0f), new OpenTK.Vector2(0.10f, 0.10f), new OpenTK.Vector2(2.9f, 2.0f), 1.8f);

            //System.Diagnostics.Debug.WriteLine(SongText[CurrentRow]);
        }

    }
}
