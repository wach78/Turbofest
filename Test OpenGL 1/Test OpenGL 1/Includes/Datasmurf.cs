using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Diagnostics;

namespace OpenGL
{
    class Datasmurf : IEffect
    {
        private Sound snd;
        private Text2D text;
        private int image;
        private float x;
        private float y;

        private bool disposed = false;
        private int tick;

        public Datasmurf(ref Sound sound, ref Text2D txt)
        {
            x = -1.0f;
            y = 0.0f;
            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/dataSmurf.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255,0,255));

            snd = sound;
            text = txt;
            //snd.CreateSound(Sound.FileType.WAV, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/datasmurf.wav", "Smurf");
            snd.CreateSound(Sound.FileType.Ogg, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/datasmurf.ogg", "Smurf");

            tick = 0;
        }

        ~Datasmurf()
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
                    Util.DeleteTexture(ref image);
                    text = null;
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");

                disposed = true;
            }
        }

      

        private void DrawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image);
            GL.Enable(EnableCap.Blend); //       
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); //

            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(2.0f + x, -0.75f + y, 0.4f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(1.1f + x, -0.75f + y, 0.4f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(1.1f + x, 0.10f + y, 0.4f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(2.0f + x, 0.10f + y, 0.4f); // top left 

            GL.End();
            GL.Disable(EnableCap.Blend);//
            GL.Disable(EnableCap.Texture2D);


        }//DrawImage

        private void moveImage()
        {
            
            this.tick++;
            
            x = (float)(Math.Tan((this.tick / 1.5) * Math.PI / 180));
            x = x * -1.0f;
            y = (float)(Math.Sin((this.tick / 1.5) * Math.PI / 180) * 0.6f);
            y += 0.25f;
         
        }//moveImage

        private void drawText()
        {
            text.Draw("Smurfar", Text2D.FontName.Coolfont, new Vector3(0.8f, 0.0f, 0.5f), new OpenTK.Vector2(0.10f, 0.10f), new OpenTK.Vector2(0.0f, 0.0f), 2.0f);
            text.Draw("Internet", Text2D.FontName.Coolfont, new Vector3(0.8f, -0.4f, 0.5f ), new OpenTK.Vector2(0.10f, 0.10f), new OpenTK.Vector2(0.0f, 0.0f), 2.0f);
        }

        private void Play()
        {
            if (snd.PlayingName() != "Smurf") // this will start once the last sound is done, ie looping.
            {
                snd.Play("Smurf");
            }
        }
        public void Draw(string Date)
        {
            Play();
            drawText();
            moveImage();
            DrawImage();
        }//Draw
    }//class
}//namespace 
