using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
namespace OpenGL
{
    class Drink
    {
        private Sound snd;
        private bool disposed;
        private string LastDate;
        private Beer b;

        public Drink(ref Sound sound)
        { 
            snd = sound;

            snd.CreateSound(Sound.FileType.Ogg, Util.CurrentExecutionPath + "/Samples/Drink.ogg", "Drink");
            b = new Beer();
            disposed = false;
            LastDate = string.Empty;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Drink()
        {
            Dispose(false);
        }

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
                   
                    b.Dispose();
                }
                // free native resources if there are any.
                Debug.WriteLine(this.GetType().ToString() + " disposed.");
                disposed = true;
            }
        }

        /// <summary>
        /// Play sound
        /// </summary>
        public void Play()
        {
            if (snd.PlayingName() != "Drink") // this will start once the last sound is done, ie looping.
            {
                snd.Play("Drink");
            }
        }
        public void Draw(string Date)
        {
            Play();
            b.Draw(Date);
        }//Draw

    }//class

    
}//namespace
