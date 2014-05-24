using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL
{
    /// <summary>
    /// Interface for Effects
    /// </summary>
    interface IEffect : IDisposable
    {
        //void Dispose(bool disposing);
        //void Dispose();
        /// <summary>
        /// Method for drawing the effect on screen
        /// </summary>
        /// <param name="Date">What is the date</param>
        void Draw(string Date);
    }
}
