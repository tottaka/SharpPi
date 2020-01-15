using MMALSharp;
using MMALSharp.Components;
using MMALSharp.Native;
using MMALSharp.Ports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPi.Imaging
{
    public class Decoder<T> : IDisposable where T : Stream
    {
        public bool IsDisposed { get; private set; }

        private GenericStreamCaptureHandler<T> InputCaptureHandler;
        private MMALVideoDecoder VideoDecoder;
        private OutputFrameCallback DecodeFrameCallback;
        private MMALStandalone Instance;

        public Decoder(T inputStream)
        {
            InputCaptureHandler = new GenericStreamCaptureHandler<T>(inputStream);
            VideoDecoder = new MMALVideoDecoder(InputCaptureHandler);

            MMALPortConfig inputPortConfig = new MMALPortConfig(MMALEncoding.H264, null, 1280, 720, 25, 0, 1300000, true, null);
            MMALPortConfig outputPortConfig = new MMALPortConfig(MMALEncoding.RGB16, null, 1280, 720, 25, 0, 1300000, true, null);
            VideoDecoder.ConfigureInputPort(inputPortConfig).ConfigureOutputPort(outputPortConfig);

            DecodeFrameCallback = new OutputFrameCallback(VideoDecoder.Outputs[0]);
            VideoDecoder.RegisterOutputCallback(DecodeFrameCallback);

            //Instance = MMALStandalone.Instance;

            //Instance.ProcessAsync(VideoDecoder);
        }

        ~Decoder()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                InputCaptureHandler.Dispose();
                VideoDecoder.Dispose();
                Instance = null;

                IsDisposed = true;
            }
        }
    }
}
