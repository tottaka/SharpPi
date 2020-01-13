using System;

using MMALSharp;
using MMALSharp.Common;
using MMALSharp.Handlers;

namespace SharpPi.Imaging
{
    public class Encoder : IDisposable
    {
        public bool IsDisposed { get; private set; }

        private MemoryStreamCaptureHandler CaptureHandler;

        public Encoder()
        {
            
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {



                IsDisposed = true;

            }
        }



        public async Task DecodeVideoFromFilestream()
        {
            MMALStandalone standalone = MMALStandalone.Instance;

            using (var CaptureHandler = new MemoryStreamCaptureHandler())
            using (var outputCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/", "raw"))
            using (var vidDecoder = new MMALVideoDecoder())
            {
                var inputPortConfig = new MMALPortConfig(MMALEncoding.H264, null, 1280, 720, 25, 0, 1300000, true, null);
                var outputPortConfig = new MMALPortConfig(MMALEncoding.RGB16, null, 1280, 720, 25, 0, 1300000, true, null);

                // Create our component pipeline.
                vidDecoder.ConfigureInputPort(inputPortConfig, inputCaptureHandler)
                    .ConfigureOutputPort<FileEncodeOutputPort>(0, outputPortConfig, outputCaptureHandler);

                await standalone.ProcessAsync(vidDecoder);
            }

            // Only call when you no longer require the MMAL library, i.e. on app shutdown.
            standalone.Cleanup();
        }
    }
}
