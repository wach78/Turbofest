using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    class GhostBusters : IEffect
    {
        private Sound snd;
        private int img;
        private int slime;
        private bool disposed = false;
        private string LastDate;

        private float x;
        private float y;

        private long tick;

        public GhostBusters(ref Sound sound)
        {
            img = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/ghostbusters.jpg");
            slime = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/slimer.jpg", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
         
            snd = sound;
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/GhostBuster.ogg", "GhostBusters");
            LastDate = string.Empty;

            this.x = -1.0f;
            this.y = 0.0f;
            this.tick = 0;

        }

        ~GhostBusters()
        {
            Dispose(false);
            System.GC.SuppressFinalize(this);
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
                    Util.DeleteTexture(ref img);
                    Util.DeleteTexture(ref slime);
                    
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");

                disposed = true;
            }
        }

        private void DrawImage()
        {
            
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, img);
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
            
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, slime);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(BeginMode.Quads);

            

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(1.5f + x, -0.50f + y, 0.4f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(1.0f + x, -0.50f + y, 0.4f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(1.0f + x, 0.0f + y, 0.4f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(1.5f + x, 0.0f + y, 0.4f); // top left 

           

            GL.End();
            GL.Disable(EnableCap.Blend);//
            GL.Disable(EnableCap.Texture2D);


        }//DrawImage

        private void updateImge()
        {
            this.tick++;
          
            x = (float)(0.004 * Math.Sin((this.tick / 50.0) * 3.1415) * 250);
            x -= 1.2f;
            y = (float)(0.004 * Math.Sin((this.tick / 42.1) * 3.1415) * 125);
            y += 0.37f;
        }

        private void Play(String Date)
        {
            if (LastDate != Date && snd.PlayingName() != "GhostBusters") // this will start once the last sound is done, ie looping.
            {
                snd.Play("GhostBusters");
                LastDate = Date;
            }
        }
        public void Draw(string Date)
        {
            Play(Date);
            updateImge();
            DrawImage();
        }//Draw
    }//class
}//namespace
