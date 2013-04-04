using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    class Intro : IEffect
    {
        private bool disposed;
        private Sound snd;
        private Text2D text;
        private float ZA;
        private float ZT;
        private float ZW;

        private long ticks;
        private long oldTicks;

        private float XA;
        private float XT;
        private float XW;

        public Intro(ref Sound sound, ref Text2D txt)
        {
            disposed = false;
            snd = sound;
            text = txt;

            ZA = 0.0f;
            ZT = 0.0f;
            ZW = 0.0f;

            ticks = 0;
            oldTicks = 0;

            XA = 0.0f;
            XT = 0.0f;
            XW = 0.0f;

        }

         ~Intro()
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
            if (disposing)
            {
                // free managed resources
            
                snd = null;
                text = null;
            }
            // free native resources if there are any.
            Console.WriteLine(this.GetType().ToString() + " disposed.");
            disposed = true;
        }
        private void Play()
        {
            if (snd.PlayingName() != "Intro") // this will start once the last sound is done, ie looping.
            {
                snd.Play("Intro");
            }
        }

        public bool  delay()
        {
            ticks = System.DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            bool b = false;

            if (this.oldTicks != 0)
            {
                if ((this.ticks - this.oldTicks) > 3000)
                {
                    b = true; 
                    oldTicks = ticks;
                }//inner if
            }//outer if

            if (oldTicks == 0)
                oldTicks = ticks;

            return b;

        }

        private void drawText()
        {
            text.Draw("ANDREAS", Text2D.FontName.Coolfont, new Vector3(0.5f + XA, 1.0f, 4.0f - ZA), new OpenTK.Vector2(0.10f, 0.10f), new OpenTK.Vector2(0.0f, 0.0f));
            text.Draw("TURBOPHEST!", Text2D.FontName.Coolfont, new Vector3(0.5f + XT, 0.0f, 4.0f - ZT), new OpenTK.Vector2(0.10f, 0.10f), new OpenTK.Vector2(0.0f, 0.0f));
            text.Draw("WACH", Text2D.FontName.Coolfont, new Vector3(0.5f + XW, -1.0f, 4.0f - ZW), new OpenTK.Vector2(0.10f, 0.10f), new OpenTK.Vector2(0.0f, 0.0f));

            if (Math.Round(ZA, 2) <= 2.00f)
            {
                ZA += 0.01f;
            }

            if (Math.Round(ZA, 2) >= 2.00f && Math.Round(ZW, 2) <= 2.00f)
            {
                ZW += 0.01f;
            }

            if (Math.Round(ZW, 2) >= 2.00f && Math.Round(ZT, 2) <= 2.00f)
            {
                ZT += 0.01f;
            }

           // någon form av delay inann x ändras
            
            if (Math.Round(ZT, 2) >= 2.00f && Math.Round(XT, 2) >= -3.70f)
            {
                 XT -= 0.025f;
            }

            if (Math.Round(XT, 2) <= -3.70f && Math.Round(XA, 2) <= 3.5f)
            {
                XA += 0.01f;
                XW += 0.01f;
            }
            

           

        }

        public void Draw(string Date)
        {
            Play();
            drawText();
        }//Draw

    }//class
}//namspace
