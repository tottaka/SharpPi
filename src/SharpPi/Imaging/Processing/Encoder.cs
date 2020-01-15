using System;
using System.IO;
using MMALSharp;
using MMALSharp.Common;
using MMALSharp.Callbacks;
using MMALSharp.Components;
using MMALSharp.Handlers;
using MMALSharp.Native;
using MMALSharp.Ports;
using MMALSharp.Ports.Outputs;

namespace SharpPi.Imaging
{
    public class Encoder<T> : IDisposable where T : Stream
    {
        public bool IsDisposed { get; private set; }

        private Stream InputStream;
        private GenericStreamCaptureHandler<T> InputCaptureHandler;
        private MMALVideoEncoder VideoEncoder;
        private OutputFrameCallback EncodeCallback;

        public Encoder(T inputStream)
        {
            InputStream = inputStream;

            MMALStandalone standalone = MMALStandalone.Instance;

            InputCaptureHandler = new GenericStreamCaptureHandler<T>((T)InputStream);
            VideoEncoder = new MMALVideoEncoder(InputCaptureHandler);

            MMALPortConfig inputPortConfig = new MMALPortConfig(MMALEncoding.RGB16, null, 1280, 720, 25, 0, 1300000, true, null);
            MMALPortConfig outputPortConfig = new MMALPortConfig(MMALEncoding.H264, MMALEncoding.RGB16, 1280, 720, 25, 0, 1300000, true, null);
            VideoEncoder.ConfigureInputPort(inputPortConfig).ConfigureOutputPort(outputPortConfig);

            EncodeCallback = new OutputFrameCallback(VideoEncoder.Outputs[0]);
            VideoEncoder.RegisterOutputCallback(EncodeCallback);
        }

        ~Encoder()
        {
            Dispose();
        }

        public void WriteFrame(byte[] frameData)
        {
            InputCaptureHandler.WriteFrame(frameData);
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                InputCaptureHandler.Dispose();
                VideoEncoder.Dispose();
                

                IsDisposed = true;
            }
        }
    }


    public class OutputFrameCallback : DefaultOutputCallbackHandler
    {
        public OutputFrameCallback(OutputPortBase outputPort) : base(outputPort)
        {
            
        }

        public override void Callback(MMALBufferImpl buffer)
        {
            base.Callback(buffer);
            Console.WriteLine("Output: " + buffer.Length);
        }
    }

    public class GenericStreamCaptureHandler<T> : StreamCaptureHandler<T> where T : Stream
    {
        public GenericStreamCaptureHandler(T stream)
        {
            CurrentStream = stream;
        }

        public void WriteFrame(byte[] data)
        {
            CurrentStream.Write(data, 0, data.Length);
        }

        public override void Process(byte[] data)
        {
            base.Process(data);
            Console.WriteLine("Input: " + data.Length);
        }
    }
}