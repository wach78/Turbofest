using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    /// <summary>
    /// Zelda effect
    /// </summary>
    class Zelda : IEffect
    {
        private Sound snd;
        private Chess bakground;
        private int zelda;
        private int link;
        private int ganon;
        private bool disposed = false;
        private string LastDate;
        private long ticks;
        private long oldTicks;

        private bool showGanon;

        /// <summary>
        /// Constructor for Zelda effect
        /// </summary>
        /// <param name="sound">Sound system</param>
        /// <param name="chess">Chessboard</param>
        public Zelda(ref Sound sound,ref Chess chess)
        {
            zelda = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/Zelda.png", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            link = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/Link.png", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            ganon = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/Ganon.png", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            bakground = chess;
            snd = sound;
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/zeldaNES.ogg", "Zelda");
            LastDate = string.Empty;
            showGanon = false;

        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Zelda()
        {
            Dispose(false);
            System.GC.SuppressFinalize(this);
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
                    Util.DeleteTexture(ref zelda);
                    Util.DeleteTexture(ref link);
                    Util.DeleteTexture(ref ganon);
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");

                disposed = true;
            }
        }

        /// <summary>
        /// Draw image to screen
        /// </summary>
        private void DrawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, zelda);
            GL.Enable(EnableCap.Blend); //       
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); //

            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led

            GL.TexCoord2(0.0, 1.0); GL.Vertex3(1.8f, -0.8f, 1.0f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(1.0f, -0.8f, 1.0f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(1.0f, -0.00f, 1.0f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(1.8f, -0.00f, 1.0f); // top left 


            GL.End();

            
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, link);
            GL.Enable(EnableCap.Blend); //       
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); //

            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led

            GL.TexCoord2(0.0, 1.0); GL.Vertex3(-0.8f, -0.8f, 1.0f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-1.8f, -0.8f, 1.0f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-1.8f, -0.00f, 1.0f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(-0.8f, -0.00f, 1.0f); // top left 


            GL.End();

            if (showGanon)
            {
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, ganon);
                GL.Enable(EnableCap.Blend); //       
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); //

                GL.Begin(BeginMode.Quads);

                // x y z
                // alla i mitten Y-led  alla till vänster x-led

                GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.8f, -0.8f, 1.0f); // bottom left  
                GL.TexCoord2(1.0, 1.0); GL.Vertex3(-0.8f, -0.8f, 1.0f); // bottom right 
                GL.TexCoord2(1.0, 0.0); GL.Vertex3(-0.8f, 0.2f, 1.0f);// top right
                GL.TexCoord2(0.0, 0.0); GL.Vertex3(0.8f, 0.2f, 1.0f); // top left 

                GL.End();

                GL.Disable(EnableCap.Blend);//
                GL.Disable(EnableCap.Texture2D);
            }

        }//DrawImage


        public void updateImages()
        {
            ticks = System.DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;


            if (this.oldTicks != 0)
            {
                if (!showGanon)
                {
                    if ((this.ticks - this.oldTicks) > 52000)
                    {
                        showGanon = true;
                        oldTicks = ticks;
                    }//inner if
                }

                

            }//outer if

            if (oldTicks == 0)
                oldTicks = ticks;

        }

        /// <summary>
        /// Play sound
        /// </summary>
        /// <param name="Date">New date?</param>

        private void Play(String Date)
        {
            if (LastDate != Date && snd.PlayingName() != "Zelda") // this will start once the last sound is done, ie looping.
            {
                snd.Play("Zelda");
                LastDate = Date;
            }
        }

        /// <summary>
        /// Draw Zelda effect on screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            if (LastDate != Date)
            {
                showGanon = false;
               // LastDate = Date;
            }
            updateImages();
            Play(Date);
            bakground.Draw(Date, Chess.ChessColor.Triforce);
            DrawImage();
        }//Draw

    }//class
}//namespace
