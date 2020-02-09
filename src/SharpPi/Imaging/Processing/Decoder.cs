using System;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using MMALSharp;
using MMALSharp.Ports;
using MMALSharp.Native;
using MMALSharp.Handlers;
using MMALSharp.Processors;
using MMALSharp.Components;
using MMALSharp.Ports.Outputs;
using System.Net.Sockets;
using SharpPi.Native;

namespace SharpPi.Imaging
{
    public class Decoder : IDisposable
    {
        public Vector2 Resolution { get; private set; }
        public int Framerate { get; private set; }
        public int Quality { get; private set; }
        public int Bitrate { get; private set; }
        public bool IsDisposed { get; private set; }

        public Action<Decoder, byte[]> OnFrameReceived;
        public Action<Decoder, byte[]> OnFrameDecoded;

        private SharpInputHandler InputCaptureHandler;
        private SharpOutputHandler OutputCaptureHandler;
        private MMALVideoDecoder VideoDecoder;
        private MMALStandalone Instance;
        private Task DecoderTask;
        private CancellationTokenSource DecoderToken;

        private List<byte> WorkingData = new List<byte>();

        public Decoder(NetworkStream inputStream, Vector2 resolution, int framerate = 25, int quality = 0, int bitrate = 1300000)
        {
            Resolution = resolution;
            Framerate = framerate;
            Quality = quality;
            Bitrate = bitrate;
            Instance = MMALStandalone.Instance;

            InputCaptureHandler = new SharpInputHandler(inputStream, Decoder_FrameReceived);
            OutputCaptureHandler = new SharpOutputHandler(Decoder_FrameDecoded);
            VideoDecoder = new MMALVideoDecoder();
            
            MMALPortConfig inputPortConfig = new MMALPortConfig(MMALEncoding.H264, MMALEncoding.I420, (int)Resolution.X, (int)Resolution.Y, Framerate, Quality, Bitrate, true, null);
            MMALPortConfig outputPortConfig = new MMALPortConfig(MMALEncoding.RGB16, MMALEncoding.RGB16, (int)Resolution.X, (int)Resolution.Y, Framerate, Quality, Bitrate, true, null);

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
                WorkingData.Clear();
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

        private void Decoder_FrameReceived(ProcessResult result)
        {
            // should be used to write encoded frame data to file if desired
            WorkingData.AddRange(result.BufferFeed);
            if (result.EOF)
                OnFrameReceived?.Invoke(this, WorkingData.ToArray());
        }

        private void Decoder_FrameDecoded(byte[] frame)
        {
            // should be used to display decoded frame data on screen or whatever
            OnFrameDecoded?.Invoke(this, frame);
        }

        internal class SharpInputHandler : InputCaptureHandler
        {
            public readonly Action<ProcessResult> inputCallback;

            public SharpInputHandler(NetworkStream inputStream, Action<ProcessResult> callback = null) : base(inputStream)
            {
                inputCallback = callback;
            }

            public override ProcessResult Process(uint allocSize)
            {
                NetworkStream stream = (NetworkStream)CurrentStream;
                ProcessResult result = stream.DataAvailable ? base.Process(allocSize) : new ProcessResult() { BufferFeed = new byte[1] { 0 }, DataLength = 0, Success = true };
                inputCallback?.Invoke(result);

                return result;
            }
        }

        internal class SharpOutputHandler : OutputCaptureHandler, IVideoCaptureHandler
        {
            public List<byte> WorkingData;
            private readonly Action<byte[]> FrameDecodedCallback;
            private long imageCount = 0;

            public SharpOutputHandler(Action<byte[]> OnFrameDecoded)
            {
                WorkingData = new List<byte>();
                FrameDecodedCallback = OnFrameDecoded;
            }

            public override void Process(byte[] data, bool eos)
            {
                base.Process(data, eos);
                WorkingData.AddRange(data);
                if (eos)
                {
                    imageCount++;
                    FrameDecodedCallback?.Invoke(WorkingData.ToArray());
                    WorkingData.Clear();
                }
            }

            public override void PostProcess()
            {

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

        public static PinnedBuffer Rgb16ToRgb24(byte[] imageData, int width, int height)
        {
            int imageSize = width * height * 3;
            PinnedBuffer buffer = new PinnedBuffer(imageSize);
            ushort a;

            for (int i = 0; i < imageSize / 3; i++)
            {
                a = (ushort)(imageData[i * 2 + 1] << 8 | imageData[i * 2]);
                buffer.Bits[i * 3] = (byte)((0x001F & a) * (256 / (1 << 5)));
                buffer.Bits[i * 3 + 1] = (byte)(((0x03E0 & a) >> 5) * (256 / (1 << 5)));
                buffer.Bits[i * 3 + 2] = (byte)(((0x7C00 & a) >> 10) * (256 / (1 << 5)));
            }

            return buffer;
        }
    }
}
