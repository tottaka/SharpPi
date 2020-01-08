using System;
using System.Runtime.InteropServices;

using OpenTK;
using OpenTK.Graphics.ES20;
using GraphicsContext = OpenTK.Graphics.GraphicsContext;

using ImGuiNET;

namespace SharpPi.Graphics
{
    /// <summary>
    /// Return codes, non-zero indicates failure.
    /// </summary>
    public enum StatusCode { SUCCESS = 0, FAIL = -1 }

    /// <summary>
    /// Derived from the WM DRM levels, 101-300
    /// </summary>
    public enum ProtectionLevel { MAX = 0x0f, NONE = 0, HDCP = 11 }

    public abstract class VideoCore
    {
        public static bool Initialized { get; private set; }
        public static OpenTK.Graphics.Color4 ClearColor = OpenTK.Graphics.Color4.CornflowerBlue;
        public static double TargetFramerate = 30.0;

        private static DateTime startTime = DateTime.Now;
        private static DateTime lastFrameTime = DateTime.Now;
        private static TimeSpan frameInterval = TimeSpan.FromSeconds(1 / TargetFramerate);
        private static TimeSpan timeTillNextFrame = TimeSpan.Zero;

        private static int frameCount = 0;
        private static float dt = 0.0f;
        private static float updateRate = 4.0f;
        private static readonly object ThreadLock = new object();
        private static readonly object RenderLock = new object();
        internal static readonly ContextHandle DummyHandle = new ContextHandle(new IntPtr(0xDEADBEEF));
        internal static ContextHandle GetContext() => DummyHandle;
        internal static IntPtr GetProcAddress(string procName) => NativeMethods.DL.Sym(EGLContext.GLESv2_Lib, procName);

        /// <summary>
        /// Initialize the VideoCore driver. Call this before doing anything with VideoCore/OpenGL.
        /// </summary>
        public static void Initialize()
        {
            lock (ThreadLock)
            {
                if (!Initialized)
                {
                    NativeMethods.VideoCore.Initialize();
                    Initialized = true;
                }
            }
        }

        /// <summary>
        /// Shutdown the VideoCore driver. Call this after you are done using VideoCore/OpenGL.
        /// </summary>
        public static void Uninitialize()
        {
            lock (ThreadLock)
            {
                if (Initialized)
                {
                    NativeMethods.VideoCore.Uninitialize();
                    Initialized = false;
                }
            }
        }

        /// <summary>
        /// Begin rendering a <see cref="VideoCoreWindow"/> instance.
        /// </summary>
        public static void Run(VideoCoreWindow window)
        {
            lock (RenderLock)
            {
                // Just incase...
                Initialize();

                GL.ClearColor(ClearColor);
                GLException.CheckError("ClearColor");

                bool running = true;
                DateTime lastFrameTime = DateTime.Now;

                while (running)
                {
                    DateTime timestamp = DateTime.Now;
                    Time.deltaTime = (float)(timestamp - lastFrameTime).TotalMilliseconds / 1000.0f;
                    Time.time = (float)(timestamp - startTime).TotalMilliseconds / 1000.0f;

                    // Calculate average framerate
                    frameCount++;
                    dt += Time.deltaTime;
                    if (dt > 1.0f / updateRate)
                    {
                        Time.averageFramerate = frameCount / dt;
                        frameCount = 0;
                        dt -= 1.0f / updateRate;
                    }

                    // Clear the screen and depth buffer
                    GL.Enable(EnableCap.StencilTest);
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit); //  | ClearBufferMask.StencilBufferBit
                    GL.Viewport(0, 0, window.Width, window.Height);

                    window.InternalUpdate();

                    lastFrameTime = DateTime.Now;
                    if (TargetFramerate > 0)
                    {
                        // Limit frames per second
                        timeTillNextFrame = timestamp + frameInterval - lastFrameTime;
                        if (timeTillNextFrame > TimeSpan.Zero)
                            Time.Delay(timeTillNextFrame.Milliseconds);
                    }

                    GLException.CheckError("Update");
                }

                window.InternalClose();

                Uninitialize();
            }
        }
    }

    /// <summary>
    /// OpenGL renderer based on VideoCore IV API on RaspberryPi.
    /// </summary>
    public class VideoCoreWindow : IDisposable
    {
        #region Handles
        /// <summary>
        /// The display handle associated this instance. This property is always <see cref="IntPtr.Zero"/>.
        /// </summary>
        public IntPtr DisplayHandle => IntPtr.Zero;

        /// <summary>
        /// The native window handle.
        /// </summary>
        public IntPtr NativeWindowHandle => _NativeWindowLock.Address;
        #endregion

        private GraphicsContext GLContext;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool IsDisposed { get; private set; }

        #region Events
        /// <summary>
        /// Called before <see cref="Update"/>, this is where any pre-processsing should be done.
        /// </summary>
        public event EventHandler PreUpdate;

        /// <summary>
        /// Called before <see cref="UpdateGUI"/>, this is where 3D drawing and frame-by-frame updates should be done.
        /// </summary>
        public event EventHandler Update;

        /// <summary>
        /// Called after <see cref="Update"/> and before <see cref="UpdateGUI"/>, this is where any post-processing should be done.
        /// </summary>
        public event EventHandler PostUpdate;

        /// <summary>
        /// Called after <see cref="Update"/> and <see cref="PostUpdate"/>, this is where ImGUI & other 2d drawing should be done.
        /// </summary>
        public event EventHandler UpdateGUI;
        #endregion

        /// <summary>
        /// The structure expected by eglCreateWindowSurface function.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct EGL_DISPMANX_WINDOW_T
        {
            public uint element;
            public int width;
            public int height;
        }

        private readonly object ThreadLock = new object();
        private uint DispmanHandle;
        private uint _ElementHandle;
        private IntPtr glContext;
        private EGL_DISPMANX_WINDOW_T _NativeWindow;
        private PinnedObject _NativeWindowLock;
        private EGLContext Context;

        private ImGuiController ImGUIController;

        /// <summary>
        /// Construct a fullscreen window.
        /// </summary>
        public VideoCoreWindow()
        {
            VideoCore.Initialize();
            if (NativeMethods.VideoCore.GetDisplaySize(DisplayID.HDMI, out int width, out int height) < 0)
                throw new InvalidOperationException("Unable to get HDMI display size");

            Width = width;
            Height = height;

            NativeMethods.VideoCore.VC_RECT_T dstRect = new NativeMethods.VideoCore.VC_RECT_T(0, 0, width, height);
            NativeMethods.VideoCore.VC_RECT_T srcRect = new NativeMethods.VideoCore.VC_RECT_T(0, 0, width << 16, height << 16);

            if ((DispmanHandle = NativeMethods.VideoCore.vc_dispmanx_display_open((uint)DisplayID.HDMI)) == 0)
                throw new InvalidOperationException("Unable to open display");

            // Update - Add element
            uint updateHandle = NativeMethods.VideoCore.vc_dispmanx_update_start(0);
            _ElementHandle = NativeMethods.VideoCore.vc_dispmanx_element_add(updateHandle, DispmanHandle, 0, ref dstRect, 0, ref srcRect, 0, IntPtr.Zero, IntPtr.Zero, NativeMethods.VideoCore.DISPMANX_TRANSFORM_T.DISPMANX_NO_ROTATE);

            // check if this returns an error code??
            NativeMethods.VideoCore.vc_dispmanx_update_submit_sync(updateHandle);

            // Native window
            _NativeWindow = new EGL_DISPMANX_WINDOW_T
            {
                element = _ElementHandle,
                width = width,
                height = height
            };

            // Keep native window pinned so GC doesn't mess with it...
            _NativeWindowLock = new PinnedObject(_NativeWindow);

            // Create EGL context
            Context = new EGLContext(DisplayHandle, NativeWindowHandle);
            Context.ChoosePixelFormat(new DevicePixelFormat(32));

            glContext = Context.CreateContext(IntPtr.Zero);
            Context.MakeCurrent(glContext);
            //Context.SwapInterval(1);

            // Create OpenGL context
            GLContext = new GraphicsContext(VideoCore.DummyHandle, VideoCore.GetProcAddress, VideoCore.GetContext);

            GLException.CheckError();
            ImGUIController = new ImGuiController(Width, Height);
        }

        internal void InternalUpdate()
        {
            OnPreUpdate(EventArgs.Empty);

            OnUpdate(EventArgs.Empty);

            OnPostUpdate(EventArgs.Empty);

            ImGUIController.Update();

            OnGUI(EventArgs.Empty);

            ImGUIController.Render();

            Context.SwapBuffers();
        }

        internal void InternalClose()
        {
            // Gui.Dispose();
            Context.DeleteContext(glContext);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="VideoCoreWindow"/> object.
        /// </summary>
        public void Dispose()
        {
            lock (ThreadLock)
            {
                if (!IsDisposed)
                    throw new ObjectDisposedException(typeof(VideoCoreWindow).Name);

                //InternalClose();
                GLContext.Dispose();
                Context.Dispose();
                if (_ElementHandle != 0)
                {
                    uint updateHandle = NativeMethods.VideoCore.vc_dispmanx_update_start(0);
                    NativeMethods.VideoCore.vc_dispmanx_element_remove(updateHandle, _ElementHandle);
                    NativeMethods.VideoCore.vc_dispmanx_update_submit_sync(updateHandle);
                    _ElementHandle = 0;
                }

                if (DispmanHandle != 0)
                {
                    NativeMethods.VideoCore.vc_dispmanx_display_close(DispmanHandle);
                    DispmanHandle = 0;
                }

                if (_NativeWindowLock != null)
                {
                    _NativeWindowLock.Dispose();
                    _NativeWindowLock = null;
                }

                // Dispose of everything here
                IsDisposed = true;
            }
        }

        protected virtual void OnPreUpdate(EventArgs args)
        {
            PreUpdate?.Invoke(this, args);
        }

        protected virtual void OnUpdate(EventArgs args)
        {
            Update?.Invoke(this, args);
        }

        protected virtual void OnPostUpdate(EventArgs args)
        {
            PostUpdate?.Invoke(this, args);
        }

        protected virtual void OnGUI(EventArgs args)
        {
            UpdateGUI?.Invoke(this, args);
        }
    }
}
