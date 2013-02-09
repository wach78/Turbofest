using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL
{
    interface IEffect : IDisposable
    {
        //void Dispose(bool disposing);
        //void Dispose();
        void Draw();
    }
}
