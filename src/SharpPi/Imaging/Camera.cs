using MMALSharp;
using MMALSharp.Components;
using MMALSharp.Native;
using MMALSharp.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MMALSharp.Common;
using MMALSharp.Callbacks;
using MMALSharp.Components;
using MMALSharp.Handlers;
using MMALSharp.Native;
using MMALSharp.Ports;
using MMALSharp.Ports.Outputs;

namespace SharpPi.Imaging
{
    public class Camera : IDisposable
    {
        /// <summary>
        /// Called when the a new frame has been captured and encoded.
        /// </summary>
        public event EventHandler<byte[]> FrameCaptured;

        public bool IsRecording { get; private set; }
        public bool IsDisposed { get; private set; }

        private MMALCamera Instance;
        private InMemoryCaptureHandler OutputHandler;
        private MMALVideoEncoder VideoEncoder;
        private CancellationTokenSource RecordToken;

        public Camera()
        {
            OutputHandler = new InMemoryCaptureHandler();
            VideoEncoder = new MMALVideoEncoder(OutputHandler);

            // Create our component pipeline. Here we are using the H.264 standard with a YUV420 pixel format. The video will be taken at 25Mb/s.
            Instance = MMALCamera.Instance;

            MMALPortConfig portConfig = new MMALPortConfig(MMALEncoding.H264, MMALEncoding.I420, 10, MMALVideoEncoder.MaxBitrateLevel4, null);
            Instance.ConfigureCameraSettings();
            VideoEncoder.ConfigureOutputPort(portConfig);
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
            if (IsRecording)
            {
                // Camera warm up time
                Thread.Sleep(2000);

                RecordToken = new CancellationTokenSource();
                Instance.ProcessAsync(Instance.Camera.VideoPort, RecordToken.Token);
            }
        }

        /// <summary>
        /// Stop recording on the camera.
        /// </summary>
        public void Stop()
        {
            RecordToken.Cancel();
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                // Only call when you no longer require the camera, i.e. on app shutdown.
                Instance.Cleanup();

                IsDisposed = true;
            }
        }
    }
}
