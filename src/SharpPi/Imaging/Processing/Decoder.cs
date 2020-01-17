using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using MMALSharp;
using MMALSharp.Components;
using MMALSharp.Callbacks;
using MMALSharp.Handlers;
using MMALSharp.Native;
using MMALSharp.Ports;
using System.Collections.Generic;
using MMALSharp.Processors;
using System.Net.Sockets;

namespace SharpPi.Imaging
{
    public class Decoder : IDisposable
    {
        public bool IsDisposed { get; private set; }

        private SharpInputHandler InputCaptureHandler;
        private SharpCaptureHandler OutputCaptureHandler;
        private MMALVideoDecoder VideoDecoder;
        private MMALStandalone Instance;
        private Task DecoderTask;
        private CancellationTokenSource DecoderToken;

        public Decoder(Stream inputStream)
        {
            Instance = MMALStandalone.Instance;

            InputCaptureHandler = new SharpInputHandler(inputStream);
            OutputCaptureHandler = new SharpCaptureHandler(frame => {
                Console.WriteLine("I have a full frame. Clearing working data. DISPLAY THE IMAGE!!! " + frame.Length);
            });
            VideoDecoder = new MMALVideoDecoder();


            MMALPortConfig inputPortConfig = new MMALPortConfig(MMALEncoding.H264, null, 1280, 720, 25, 0, 1300000, true, null);
            MMALPortConfig outputPortConfig = new MMALPortConfig(MMALEncoding.RGB16, null, 1280, 720, 25, 0, 1300000, true, null);

            // Create our component pipeline.
            VideoDecoder.ConfigureInputPort(inputPortConfig, InputCaptureHandler).ConfigureOutputPort(outputPortConfig, OutputCaptureHandler);

            DecoderToken = new CancellationTokenSource();
            DecoderTask = Instance.ProcessAsync(VideoDecoder, DecoderToken.Token).ContinueWith(Decoder_ProcessedFinished);
        }

        ~Decoder()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                DecoderToken.Cancel();
                DecoderTask.Wait();
                DecoderToken.Dispose();
                DecoderTask.Dispose();

                OutputCaptureHandler.Dispose();
                VideoDecoder.Dispose();
                Instance = null;

                IsDisposed = true;
            }
        }

        private void Decoder_ProcessedFinished(Task t)
        {
            if (t.IsFaulted)
                throw t.Exception?.InnerException;

            Console.WriteLine("Decoder stopped.");

            t.Dispose();
        }

        internal class SharpInputHandler : InputCaptureHandler
        {
            public SharpInputHandler(Stream inputStream) : base(inputStream)
            {
                
            }

            public override ProcessResult Process(uint allocSize)
            {
                ProcessResult result = base.Process(allocSize);
                Console.WriteLine("InputProcess: " + result.DataLength + " EOF: " + result.EOF);
                return result;
            }

            public void Split()
            {
                throw new NotImplementedException();
            }
        }

        internal class SharpCaptureHandler : OutputCaptureHandler, IVideoCaptureHandler
        {
            public List<byte> WorkingData;
            private readonly Action<byte[]> FrameDecodedCallback;
            private long imageCount = 0;

            public SharpCaptureHandler(Action<byte[]> OnFrameDecoded)
            {
                WorkingData = new List<byte>();
                FrameDecodedCallback = OnFrameDecoded;
            }

            public override void Process(byte[] data, bool eos)
            {
                base.Process(data, eos);
                WorkingData.AddRange(data);
                Console.WriteLine("Process: " + data.Length);
                if (eos)
                {
                    imageCount++;
                    FrameDecodedCallback?.Invoke(WorkingData.ToArray());
                    WorkingData.Clear();
                }
            }

            public override void PostProcess()
            {
                if (OnManipulate != null && ImageContext != null)
                {
                    ImageContext.Data = WorkingData.ToArray();
                    OnManipulate(new FrameProcessingContext(ImageContext));
                    WorkingData = new List<byte>(ImageContext.Data);
                }
            }

            public override string TotalProcessed()
            {
                return $"{imageCount}";
            }

            public override void Dispose()
            {
                Console.WriteLine($"Successfully processed {imageCount} images.");
                WorkingData.Clear();
            }

            public void Split()
            {
                throw new NotImplementedException();
            }
        }
    }
}
