using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Sockets;

using MMALSharp;
using MMALSharp.Components;
using MMALSharp.Handlers;
using MMALSharp.Native;
using MMALSharp.Ports;
using MMALSharp.Processors;
using MMALSharp.Common.Utility;
using System.Numerics;

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
        public event EventHandler<byte[]> OnFrameCaptured;

        public bool IsRecording { get; private set; }
        public bool IsDisposed { get; private set; }

        private MMALCamera Instance;
        private MMALVideoEncoder VideoEncoder;
        private NetworkStreamCaptureHandler OutputHandler;
        private MMALNullSinkComponent nullSink;
        private CancellationTokenSource RecordToken;
        private Task RecordingTask;
        
        public Camera(NetworkStream outputStream, Vector2 resolution, int bitrate = 1300000, int frameRate = 25, int quality = 0, EventHandler<byte[]> frameCaptured = null)
        {
            OnFrameCaptured = frameCaptured;

            MMALCameraConfig.VideoResolution = new Resolution((int)resolution.X, (int)resolution.Y);
            MMALCameraConfig.VideoFramerate = new MMAL_RATIONAL_T(frameRate, 1);

            OutputHandler = new NetworkStreamCaptureHandler(outputStream);
            VideoEncoder = new MMALVideoEncoder();

            // Create our component pipeline. Here we are using the H.264 standard with a YUV420 pixel format. The video will be taken at 25Mb/s.
            Instance = MMALCamera.Instance;
            Instance.ConfigureCameraSettings();

            MMALPortConfig portConfig = new MMALPortConfig(MMALEncoding.H264, MMALEncoding.I420, quality, bitrate, null);
            VideoEncoder.ConfigureOutputPort(portConfig, OutputHandler);

            nullSink = new MMALNullSinkComponent();
            Instance.Camera.PreviewPort.ConnectTo(nullSink);
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
            }
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                // Only call when you no longer require the camera, i.e. on app shutdown.

                if (IsRecording)
                    Stop();

                OutputHandler.Dispose();
                VideoEncoder.Dispose();
                nullSink.Dispose();

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

        private void Encoder_FrameEncoded(byte[] frameData)
        {
            OnFrameCaptured?.Invoke(this, frameData);
        }

        public class NetworkStreamCaptureHandler : StreamCaptureHandler<NetworkStream>, IVideoCaptureHandler
        {
            private long imageCount = 0;

            public NetworkStreamCaptureHandler(NetworkStream outputStream)
            {
                CurrentStream = outputStream;
            }

            public override void Process(byte[] data, bool eos)
            {
                base.Process(data, eos);

                if (eos)
                {
                    imageCount++;
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
            }

            public void Split()
            {
                throw new NotImplementedException();
            }
        }
    }
}
