using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using OpenTK;

namespace OpenGL
{

    class FireWorks
    {

        private class particle
        {
            public float x;
            public float y;
            public float sx;
            public float sy;

            public float z;

            public particle()
            {

            }

        }//class


        private float fade;
        private float sfade;
        private float xv;
        float angel;

        private int image;

        private bool disposed = false;

        private const int NFW = 15;

        particle[] part;

        public FireWorks(int image)
        {

            this.image = image;
            this.fade = 1.1f;
            float z = 0.4f;
            angel = 360.0f /(float)NFW;
            angel = (float) (angel * (Math.PI/180) );

            part = new particle[NFW];

            sfade = (0.5f + ( Util.Rnd.Next() / (Int32.MaxValue + 1.0f))) / 50.0f;

            float x = Util.Rnd.Next(-18,18)/10;
            float y = Util.Rnd.Next(4,18)/10;

            for (int i = 0; i < NFW; i++)
            {
                part[i] = new particle();
                part[i].x = x;
                part[i].y = y;
                part[i].z = z;
                //v = (float)Math.PI * 2.0f * (Util.Rnd.Next() / (Int32.MaxValue + 1.0f));
                xv =  0.7f + 0.1f * (Util.Rnd.Next() / (Int32.MaxValue + 1.0f));

             

                part[i].sx = 0.005f * (float)Math.Cos(angel * i) * xv;
                part[i].sy = 0.005f * (float)Math.Sin(angel * i) * xv;
                z -= 0.0001f;

            }


        }

        ~FireWorks()
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
                    image = -1;
                }
                // free native resources if there are any.
               // Debug.WriteLine(this.GetType().ToString() + " disposed.");

                disposed = true;
            }
        }

        public void Draw(string Date)
        {
            GL.PushAttrib(AttribMask.CurrentBit);

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, image);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(BeginMode.Quads);

            
            GL.Color4(1.0, 1.0, 1.0, this.fade);
            // x y z
            // alla i mitten Y-led  alla till vänster x-led

            for (int i = 0; i < NFW; i++)
            {

                GL.TexCoord2(0.0, 0.0); GL.Vertex3(part[i].x, part[i].y, part[i].z); // bottom left  
                GL.TexCoord2(1.0, 0.0); GL.Vertex3(part[i].x + 0.1f, part[i].y, part[i].z); // bottom right 
                GL.TexCoord2(1.0, 1.0); GL.Vertex3(part[i].x + 0.1f, part[i].y - 0.1f, part[i].z);// top right
                GL.TexCoord2(0.0, 1.0); GL.Vertex3(part[i].x, part[i].y - 0.1f, part[i].z); // top left 
            }
          
            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.PopAttrib();
            if (this.fade > 0.0)
            {
                for (int i = 0; i < NFW; i++)
                {
                    part[i].x += part[i].sx;
                    part[i].y += part[i].sy;
                    part[i].sy -= 0.0005f;
                }
                this.fade -= this.sfade;  
            }
            else
            {
                float x = Util.Rnd.Next(-15, 15) / 10.0f;
                float y = Util.Rnd.Next(4, 15) / 10.0f;

                for (int i = 0; i < NFW; i++)
                {
                    part[i].x = x;
                    part[i].y = y;
                    part[i].sy = (0.005f * (float)Math.Sin(angel * i)) * (0.7f + 0.1f * (Util.Rnd.Next() / (Int32.MaxValue + 1.0f)));
                }

                this.fade = 1.1f;
            }
             


        }
    }//class

    class MoreFireWorks
    {
        private int fw1;
        private int fw2;
        private int fw3;
        private int fw4;
        private int fw5;

       private FireWorks[] fw;

       private bool disposed = false;

        public MoreFireWorks()
        {
            fw1 = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/part1.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.Black);
            fw2 = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/part2.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.Black);
            fw3 = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/part3.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.Black);
            fw4 = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/part4.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.Black);
            fw5 = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/part5.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, System.Drawing.Color.Black);

            fw = new FireWorks[5];
            fw[0] = new FireWorks(fw1);
            fw[1] = new FireWorks(fw2);
            fw[2] = new FireWorks(fw3);
            fw[3] = new FireWorks(fw4);
            fw[4] = new FireWorks(fw5);
        }

        ~MoreFireWorks()
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
                    Util.DeleteTexture(ref fw1);
                    Util.DeleteTexture(ref fw2);
                    Util.DeleteTexture(ref fw3);
                    Util.DeleteTexture(ref fw4);
                    Util.DeleteTexture(ref fw5);
                         
                }
                // free native resources if there are any.
              //  Debug.WriteLine(this.GetType().ToString() + " disposed.");

                disposed = true;
            }
        }

        public void Draw(String Date)
        {
            for (int i = 0; i < fw.Length; i++)
            {
                fw[i].Draw(Date);
            }
        }
    }//class  MoreFireWorks
}//namespace
