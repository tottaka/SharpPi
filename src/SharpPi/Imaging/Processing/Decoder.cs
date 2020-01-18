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
using MMALSharp.Ports.Outputs;

namespace SharpPi.Imaging
{
    public class Decoder : IDisposable
    {
        public bool IsDisposed { get; private set; }

        private SharpInputHandler InputCaptureHandler;
        private NetworkStreamCaptureHandler OutputCaptureHandler;
        private MMALImageDecoder VideoDecoder;
        private MMALStandalone Instance;
        private Task DecoderTask;
        private CancellationTokenSource DecoderToken;

        public Decoder(Stream inputStream)
        {
            //MMALCameraConfig.Debug = true;
            Instance = MMALStandalone.Instance;

            InputCaptureHandler = new SharpInputHandler(inputStream);
            OutputCaptureHandler = new NetworkStreamCaptureHandler(frame => {
                Console.WriteLine("I have a full frame. Clearing working data. DISPLAY THE IMAGE!!! " + frame.Length);
            });
            VideoDecoder = new MMALImageDecoder();


            //MMALPortConfig inputPortConfig = new MMALPortConfig(MMALEncoding.H264, null, 1280, 720, 30, 10, MMALVideoEncoder.MaxBitrateLevel4, true, null);
            //MMALPortConfig outputPortConfig = new MMALPortConfig(MMALEncoding.RGB16, null, 1280, 720, 30, 10, MMALVideoEncoder.MaxBitrateLevel4, true, null);

            MMALPortConfig inputPortConfig = new MMALPortConfig(MMALEncoding.JPEG, MMALEncoding.I420, 2560, 1920, 0, 0, 0, true, null);
            MMALPortConfig outputPortConfig = new MMALPortConfig(MMALEncoding.I420, null, 2560, 1920, 0, 0, 0, true, null);

            // Create our component pipeline.
            VideoDecoder.ConfigureInputPort(inputPortConfig, InputCaptureHandler).ConfigureOutputPort<FileEncodeOutputPort>(0, outputPortConfig, OutputCaptureHandler);

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
                DecoderTask.Dispose();

                InputCaptureHandler.Dispose();
                OutputCaptureHandler.Dispose();
                VideoDecoder.Dispose();
                Instance = null;

                IsDisposed = true;
            }
        }

        private void Decoder_ProcessedFinished(Task t)
        {
            Console.WriteLine("Decoder stopped.");
            DecoderToken.Dispose();
            if (t.IsFaulted)
                throw t.Exception?.InnerException;

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
                Console.WriteLine("[Input] Process: {0}, EOF = {1}", result.DataLength, result.EOF);
                return result;
            }

            public void Split()
            {
                throw new NotImplementedException();
            }
        }

        internal class NetworkStreamCaptureHandler : StreamCaptureHandler<NetworkStream>, IOutputCaptureHandler
        {
            public List<byte> WorkingData;
            private readonly Action<byte[]> FrameDecodedCallback;
            private long imageCount = 0;

            public NetworkStreamCaptureHandler(Action<byte[]> OnFrameDecoded)
            {
                
            }

            public override void Process(byte[] data, bool eos)
            {
                //base.Process(data, eos);
                WorkingData.AddRange(data);
                Console.WriteLine("Process: " + data.Length);
                if (eos)
                {
                    imageCount++;
                    FrameDecodedCallback?.Invoke(WorkingData.ToArray());
                    WorkingData.Clear();
                }
            }
        }

        /*
        internal class NetworkStreamCaptureHandler : StreamCaptureHandler<NetworkStream>, IOutputCaptureHandler
        {
            public List<byte> WorkingData;
            private readonly Action<byte[]> FrameDecodedCallback;
            private long imageCount = 0;

            public NetworkStreamCaptureHandler(Action<byte[]> OnFrameDecoded)
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
        */
    }
}
