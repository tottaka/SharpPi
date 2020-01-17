using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using MMALSharp;
using MMALSharp.Common;
using MMALSharp.Callbacks;
using MMALSharp.Components;
using MMALSharp.Handlers;
using MMALSharp.Native;
using MMALSharp.Ports;
using MMALSharp.Ports.Outputs;
using System.Net.Sockets;
using System.Collections.Generic;
using MMALSharp.Processors;

namespace SharpPi.Imaging
{
    /// <summary>
    /// Record camera output to a <see cref="Stream"/> of the given type.
    /// </summary>
    /// <typeparam name="T">The stream type</typeparam>
    public class Camera : IDisposable
    {
        /// <summary>
        /// Called when the a new frame has been captured and encoded.
        /// </summary>
        //public event EventHandler<byte[]> FrameCaptured;

        public bool IsRecording { get; private set; }
        public bool IsDisposed { get; private set; }

        private MMALCamera Instance;
        //private MMALNullSinkComponent nullSink;
        private NetworkStreamCaptureHandler OutputHandler;
        private MMALVideoEncoder VideoEncoder;
        private CancellationTokenSource RecordToken;
        private Task RecordingTask;

        public Camera(NetworkStream outputStream)
        {
            //MMALCameraConfig.Debug = true;
            OutputHandler = new NetworkStreamCaptureHandler(outputStream);
            VideoEncoder = new MMALVideoEncoder();

            // Create our component pipeline. Here we are using the H.264 standard with a YUV420 pixel format. The video will be taken at 25Mb/s.
            Instance = MMALCamera.Instance;
            Instance.ConfigureCameraSettings();
            MMALCameraConfig.Debug = true;

            MMALPortConfig portConfig = new MMALPortConfig(MMALEncoding.H264, MMALEncoding.I420, 10, MMALVideoEncoder.MaxBitrateLevel4, null);
            VideoEncoder.ConfigureOutputPort(portConfig, OutputHandler);

            //nullSink = new MMALNullSinkComponent();
            //Instance.Camera.PreviewPort.ConnectTo(nullSink);
            Instance.Camera.VideoPort.ConnectTo(VideoEncoder);
        }

        ~Camera()
        {
            Dispose();
        }

        /// <summary>
        /// Start recording on the camera.
        /// </summary>
        public void Start()
        {
            if (!IsRecording)
            {
                RecordToken = new CancellationTokenSource();
                RecordingTask = Instance.ProcessAsync(Instance.Camera.VideoPort, RecordToken.Token).ContinueWith(Instance_RecordingStopped);
                Console.WriteLine("Recording started!");
                IsRecording = true;
            }
        }

        /// <summary>
        /// Stop recording on the camera.
        /// </summary>
        public void Stop()
        {
            if (IsRecording)
            {
                RecordToken.Cancel();
                RecordingTask.Wait();
                RecordingTask.Dispose();
                Console.WriteLine("Recording stopped.");
            }
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                // Only call when you no longer require the camera, i.e. on app shutdown.

                if (IsRecording)
                    Stop();

                //nullSink.Dispose();
                OutputHandler.Dispose();
                VideoEncoder.Dispose();
                
                Instance.Cleanup();

                IsDisposed = true;
            }
        }

        private void Instance_RecordingStopped(Task t)
        {
            IsRecording = false;
            RecordToken.Dispose();

            if (t.IsFaulted)
                throw t.Exception?.InnerException;

            t.Dispose();
        }

        public class NetworkStreamCaptureHandler : OutputCaptureHandler, IVideoCaptureHandler
        {
            public List<byte> WorkingData;

            private NetworkStream OutputStream;
            private long imageCount = 0;

            public NetworkStreamCaptureHandler(NetworkStream outputStream)
            {
                OutputStream = outputStream;
                WorkingData = new List<byte>();
            }

            public override void Process(byte[] data, bool eos)
            {
                //base.Process(data, eos);
                WorkingData.AddRange(data);
                if (eos)
                {
                    imageCount++;
                    OutputStream.Write(WorkingData.ToArray(), 0, WorkingData.Count);
                    Console.WriteLine("Write: " + WorkingData.Count + " Frame Count: " + imageCount);
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
