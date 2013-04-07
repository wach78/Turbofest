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


        private float XA;
        private float XT;
        private float XW;

        private bool XTBack;

        public Intro(ref Sound sound, ref Text2D txt)
        {
            disposed = false;
            snd = sound;
            text = txt;

            ZA = 0.0f;
            ZT = 0.0f;
            ZW = 0.0f;


            XA = 0.0f;
            XT = 0.0f;
            XW = 0.0f;
            XTBack = false;

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
            if (!this.disposed)
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
        }
        private void Play()
        {
            if (snd.PlayingName() != "Intro") // this will start once the last sound is done, ie looping.
            {
                snd.Play("Intro");
            }
        }


        private void drawText()
        {
            text.Draw("KamikazE", Text2D.FontName.Coolfont, new Vector3(1.5f + XA, 1.0f, 4.0f - ZA), new OpenTK.Vector2(0.10f, 0.10f), new OpenTK.Vector2(0.0f, 0.0f), 4.0f);
            text.Draw("TURBOPHEST!", Text2D.FontName.Coolfont, new Vector3(1.5f + XT, 0.0f, 4.0f - ZT), new OpenTK.Vector2(0.10f, 0.10f), new OpenTK.Vector2(0.0f, 0.0f), 4.0f);
            text.Draw("WACH", Text2D.FontName.Coolfont, new Vector3(1.5f + XW, -1.0f, 4.0f - ZW), new OpenTK.Vector2(0.10f, 0.10f), new OpenTK.Vector2(0.0f, 0.0f), 4.0f);

            if (ZA <= 2.00f)
            {
                ZA += 0.01f;
            }

            if (ZA >= 2.00f && ZW <= 2.00f)
            {
                ZW += 0.01f;
            }

            if (ZW >= 2.00f && ZT <= 2.00f)
            {
                ZT += 0.01f;
            }

           // någon form av delay inann x ändras
            
            if (ZT >= 2.00f && XT  >= -4.70f && !XTBack)
            {
                 XT -= 0.01f;
               
            }

            if (XT <= -4.70f && XA <= 5.5f)
            {
                XA += 0.01f;
                XW += 0.01f;
                XTBack = true;
            }

            if (XT <= 0.6f && XA >= 5.5f && XTBack)
            {
                XT += 0.025f;
                ZT += 0.003f;
            }

         
           

        }

        public void Draw(string Date)
        {
            Play();
            drawText();
        }//Draw

    }//class
}//namspace
