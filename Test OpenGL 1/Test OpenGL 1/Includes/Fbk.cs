using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    class Fbk :IDisposable
    {
        private int image;
        private System.Media.SoundPlayer player;

        public Fbk()
        {
            image = 0;
            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\gfx\\fbk2.png");

            player = new System.Media.SoundPlayer();
            player.SoundLocation = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\samples\\fbk.wav";
            player.Play();
        }

        public void Dispose()
        {
            GL.DeleteBuffers(1, ref image);
            this.image = -1;
            System.GC.SuppressFinalize(this);
        }

        private void drawImage()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image);
            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.8f, -0.8f, 1.0f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-0.8f, -0.8f, 1.0f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-.8f, -0.00f, 1.0f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(0.8f, -0.00f, 1.0f); // top left 

            GL.End();
            GL.Disable(EnableCap.Texture2D);

        }//DrawImage

        public void Play()
        {
            player.Play();
        }
        public void Stop()
        {
            player.Stop();
        }
        public void Draw()
        {
            drawImage();
        }//Draw

    }//class
}//namespace
