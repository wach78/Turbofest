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
    class Easter : IEffect
    {
        private int image;
        private int image2;
        private int eggImage;
        private int curruntEggImage;
        private int currentImage;

        private long ticks;
        private long oldTicks;

        private float x;
        private float y;
        private float xc;

        private bool disposed = false;
        private int tick;

        private Eggs[] egg;
        private const int NUMBEROFFEGGS = 40;

        private string LastDate;

        public Easter()
        {
          
            image = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/gladpask.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            image2 = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/chicken.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));
            eggImage = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/eggs.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.FromArgb(255, 0, 255));

            currentImage = 0;
            curruntEggImage = 0;
            ticks = 0;
            oldTicks = 0;

            xc = 0;


            LastDate = string.Empty;
            tick = 0;

            x = 1;
            y = 0;

            egg = new Eggs[NUMBEROFFEGGS];

            float z = 0.4f;

            for (int i = 0; i < NUMBEROFFEGGS; i++)
            {

                egg[i] = new Eggs((Util.Rnd.Next(-18, 15)) / 10.0f, (Util.Rnd.Next(-10, 20) * -1) / 10.0f, Util.Rnd.Next(2, 8) / 1000.0f, eggImage,
                    new Vector2[] {  new Vector2(0.0f + (curruntEggImage * 0.2f), 1.0f),
                                     new Vector2(0.2f + (curruntEggImage * 0.2f), 1.0f),
                                     new Vector2(0.2f + (curruntEggImage * 0.2f), 0.0f), 
                                     new Vector2(0.0f + (curruntEggImage * 0.2f), 0.0f)}, Util.Rnd.Next(5, 10) * 10.0f, z);


                z -= 0.00001f;
                curruntEggImage++;

                if (curruntEggImage == 4)
                    curruntEggImage = 0;


            }
        }

        ~Easter()
        {
            Dispose(false);
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
                   Util.DeleteTexture(ref image2);
                   Util.DeleteTexture(ref eggImage);

                   for (int i = 0; i < NUMBEROFFEGGS; i++)
                   {
                       egg[i].Dispose();
                       egg[i] = null;
                   }
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");

                disposed = true;
            }
        }

        private void drawImage()
        {
          
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.8f + x, -0.8f + y, 0.45f); // bottom left  
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(-0.8f + x, -0.8f + y, 0.45f); // bottom right 
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(-.8f + x, -0.0f + y, 0.45f);// top right
            GL.TexCoord2(0.0, 0.0); GL.Vertex3(0.8f + x, -0.0f + y, 0.45f); // top left 

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image2);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led
            GL.TexCoord2(0.0f + (currentImage * 0.5f), 1.0f); GL.Vertex3(-1.6f + xc, -1.0f, 0.44f); // bottom left // x y z alla i mitten Y-led 
            GL.TexCoord2(0.5f + (currentImage * 0.5f), 1.0f); GL.Vertex3(-2.5f + xc, -1.0f, 0.44f); // bottom right // alla till vänster x-led
            GL.TexCoord2(0.5f + (currentImage * 0.5f), 0.0f); GL.Vertex3(-2.5f + xc, -0.2f, 0.44f);// top right
            GL.TexCoord2(0.0f + (currentImage * 0.5f), 0.0f); GL.Vertex3(-1.6f + xc, -0.2f, 0.44f); // top left 

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

            

            for (int i = 0; i < NUMBEROFFEGGS; i++)
            {
                egg[i].Draw("");
            }
            

        }//DrawImage
        private void moveImage()
        {

            this.tick++;
            /*
            x = (float)(Math.Sin((this.tick * 1.5) * Math.PI / 180)) * 0.8f;
            x += 0.0f;
            y = (float)(Math.Sin((this.tick * 1.5) * Math.PI / 180) * 0.5f);
            y += 0.34f;
            */
            x = (float)(0.004 * Math.Sin((this.tick / 50.0) * 3.1415) * 200);

            y = (float)(0.004 * Math.Sin((this.tick / 42.1) * 3.1415) * 150);
            y += 0.37f;

        }//moveImage

        public void updateImages()
        {
            ticks = System.DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            if (this.oldTicks != 0)
            {
                if ((this.ticks - this.oldTicks) > 300)
                {
                    currentImage++;

                    if (currentImage > 1)
                        currentImage = 0;

                    oldTicks = ticks;
                }//inner if
            }//outer if

            if (oldTicks == 0)
                oldTicks = ticks;

            xc += 0.001f;

        }

        public void Draw(string Date)
        {
            updateImages();
            moveImage();

            drawImage();

        }//Draw

    }//class
}//namespace
