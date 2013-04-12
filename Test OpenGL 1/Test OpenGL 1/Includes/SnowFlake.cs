using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;


namespace OpenGL
{
    class SnowFlake : IEffect
    {
        private float x;
        private float y;
        private float speedY; // Y led gravity 

        private float z;
        private float Xpos;
        private float XLed;

        private int snowImage;

        private bool disposed = false;

        private Vector2[] vecTex;
        private Vector3[] vecPos;

        public SnowFlake(float x, float y, float speedY, int snowImage, Vector2[] vecTex, float XLed, float z)
        {
            this.x = x;
            this.y = y;
            this.Xpos = x;
            this.speedY = speedY;
            this.z = z;
            this.snowImage = snowImage;
            this.vecTex = vecTex;
            this.XLed = XLed;
            

            this.vecPos = new Vector3[] {
                                         new Vector3(x + 0.0f,y  -0.05f,this.z),
                                         new Vector3(x - 0.05f,y  -0.05f,this.z),
                                         new Vector3(x - 0.05f,y  + 0.0f,this.z),
                                         new Vector3(x + 0.0f,y  +0.0f,this.z)
                                        };

        }
        ~SnowFlake()
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
                    snowImage = -1;
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");
                disposed = true;
            }
        }



        public void Draw(string Date)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, snowImage);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(BeginMode.Quads);

            // x y z
            // alla i mitten Y-led  alla till vänster x-led

            GL.TexCoord2(vecTex[0]); GL.Vertex3(vecPos[0]); // bottom left  
            GL.TexCoord2(vecTex[1]); GL.Vertex3(vecPos[1]); // bottom right 
            GL.TexCoord2(vecTex[2]); GL.Vertex3(vecPos[2]);// top right
            GL.TexCoord2(vecTex[3]); GL.Vertex3(vecPos[3]); // top left 

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);


            if (this.y < -1.4f)
            {
                this.y = 1.4f;
            }
            else
            {
                this.y -= speedY;
            }


           // x = x + 0.001f * (float)Math.Sin(y * 50000 * speedX) + 0.005f;

            x = this.Xpos + (float)((0.001 * Math.Sin(500 * y * (Math.PI /180)) + 0.005)  ) * this.XLed;

            
            //x = A*sin((10+m)*y)+1+k
            //Där A är random mellan 0,5 och 1,5
            //m är mellan -5 och 5 och k är mellan -2 och 2

          //  x = x + (float)(0.001 * Math.Sin(y * 872) + 0.005);

           // x = (float)(1000 * Math.Pow(y,2) + 0.001);

           // x = x + (float)Math.Sin((y / 20) * speedX) * (float)0.05f + 1;

        //    x = (float)Math.Sin(y);

         //   x =  + (float)(0.001 * Math.Sin(10000 * y) + 0.005) * 500.0f;
           

             //Prova detta 0.001*sin (5000*x) + 0.005
         //   3*sin(x+)+x+ 

            // r.Next(1, 10) / 1000.0f
         // x = A * sin(y + m);

            

          //  typ x=Asin(y+k) + b Där du låter A, k och b vara random tal???
            /*   I ekvationen jag skrev, så kommer A bestämma hur långt ut snöflingan 
               åker från centrum, k bestämmer långden på varje topp/dal medans b förskjuter
               kurvan gentemot andra kurvor.....
              om du inte vill att dom ska falla rakt ner så kan du alltid lägga till 
              en extra ekvation så kommer sinuskurvan åka runt den ekvationen
             typ x = A * sin(y + k) + m * y + b
            Detta kommer få sinuskurvan att variera i en linje som bestäms av m som då kan simulera en vind i sidled
             * 
             *  Jag tänkte något i stil med x = A * sin(y+m) + k * x
                Där A är random mellan -1,5 och 1,5. m mellan 0 och 2 och k mellan är ett fast värde runt 1
                Ja då.... Med ekvationen jag precis skrev så får du en variation i x-led..... sen har du en annan ekvation där du bestämmer hastigheten i y-led
                Om du vill ha snöflingorna rakt neråt så sätt bara k = 0
             */

            vecPos[0].Y = y - 0.05f;
            vecPos[1].Y = y - 0.05f;
            vecPos[2].Y = y;
            vecPos[3].Y = y;

            //snowFlakesXpos();
            vecPos[0].X = x;
            vecPos[1].X = x - 0.05f;
            vecPos[2].X = x - 0.05f;
            vecPos[3].X = x;

        }



    }//class
}//namespase
