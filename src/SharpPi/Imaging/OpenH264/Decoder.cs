using System;
using System.Runtime.InteropServices;
using SharpPi.Native;

namespace SharpPi.Imaging.OpenH264
{
    /// <summary>
    /// Provides methods for decoding with Cisco's OpenH264 software codec.
    /// </summary>
    public class Decoder : IDisposable
    {
        public bool Initialized { get; private set; }
        public bool IsDisposed { get; private set; }

        public NativeMethods.H264.ISVCDecoder decoder;

        /// <summary>
        /// Creates a new <see cref="H264Decoder"/> instance.
        /// </summary>
        public Decoder()
        {
            
            Initialized = true;
            NativeMethods.H264.ISVCDecoder dec = null;
            int rc = NativeMethods.H264.CreateDecoderFunc(ref dec);
            if (rc != 0)
                throw new DllNotFoundException("Unable to call WelsCreateSVCDecoder function.");

            decoder = dec;
            rc = Setup();
            if (rc != 0)
                throw new InvalidOperationException("Error occurred during initializing decoder.");
        }

        /// <summary>
        /// Ensures this <see cref="H264Decoder"/> instance is disposed by the garbage collector, if not disposed manually before then.
        /// </summary>
        ~Decoder()
        {
            Dispose();
        }

        /// <summary>
        /// Releases and cleans up all resources used by this <see cref="H264Decoder"/> instance.
        /// </summary>
        public void Dispose()
        {
            if (!IsDisposed)
            {
                if (!Initialized) return;
                decoder.Uninitialize();
                NativeMethods.H264.DestroyDecoderFunc(decoder);
                decoder = null;

                IsDisposed = true;
            }
        }

        private int Setup()
        {
            NativeMethods.H264.SDecodingParam decParam = new NativeMethods.H264.SDecodingParam
            {
                //memset(&decParam, 0, Marshal.SizeOf<NativeMethods.H264.SDecodingParam>());
                uiTargetDqLayer = NativeMethods.H264.UCHAR_MAX,
                eEcActiveIdc = NativeMethods.H264.ErrorConIDC.ERROR_CON_SLICE_COPY,
                sVideoProperty = new NativeMethods.H264.SVideoProperty()
            };
            decParam.sVideoProperty.eVideoBsType = NativeMethods.H264.VideoBitstreamType.VIDEO_BITSTREAM_DEFAULT;

            if (decoder == null)
                throw new Exception("DECODER IS NULL?!?!?!");

            return decoder.Initialize(ref decParam);
        }
    }
}
