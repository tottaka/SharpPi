using System;
using SharpPi.Native;

namespace SharpPi.Imaging.OpenH264
{
    /// <summary>
    /// Provides methods for encoding with Cisco's OpenH264 software codec.
    /// </summary>
    public class Encoder : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public Encoder()
        {

        }

        ~Encoder()
        {
            Dispose();
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
