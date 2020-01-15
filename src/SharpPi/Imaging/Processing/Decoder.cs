using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPi.Imaging
{
    public class Decoder : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public Decoder()
        {

        }

        ~Decoder()
        {
            
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {


                IsDisposed = true;
            }
        }
    }
}
