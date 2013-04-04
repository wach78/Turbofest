using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    class Valentine : IEffect
    {
        private int image;
        private int image2;

        private Sound snd;

        public Valentine(ref Sound sound)
        {
            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\valentine.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            image2 = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\barseback3.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));

            snd = sound;
            snd.CreateSound(Sound.FileType.WAV, System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\samples\\valentine.wav", "Valentine");
         
        }

        public void Dispose()
        {
            Util.DeleteTexture(ref image);
            Util.DeleteTexture(ref image2);
            snd = null;
            System.GC.SuppressFinalize(this);
        }

        private void drawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image);
            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.0f, -1.1f, 0.8f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-1.6f, -1.1f, 0.8f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-1.6f, 0.1f, 0.8f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(0.0f, 0.1f, 0.8f); // top left 

            GL.End();

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image2);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(1.8f, -1.0f, 1.0f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(0.2f, -1.0f, 1.0f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(0.2f, -0.0f , 1.0f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(1.8f, -0.0f, 1.0f); // top left 

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
      

        }//DrawImage

        public void Play()
        {
            if (snd.PlayingName() != "Valentine") // this will start once the last sound is done, ie looping.
            {
                snd.Play("Valentine");
            }
        }
        public void Draw(string Date)
        {
            Play();
            drawImage();
        }//Draw
    }//class
}//namespace
