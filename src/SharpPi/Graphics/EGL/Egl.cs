using OpenTK.Graphics.ES20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpPi.Native;

namespace SharpPi.Graphics
{
    public abstract class Egl
    {
        /// <summary>
        /// Initialize EGL display connection.
        /// </summary>
        /// <param name="display">The display handle</param>
        public static bool Initialize(IntPtr display, int major = 2, int minor = 1) => NativeMethods.Egl.Initialize(display, ref major, ref minor);

        /// <summary>
        /// Get an EGL display connection.
        /// </summary>
        public static IntPtr GetDisplay(IntPtr displayId) => NativeMethods.Egl.GetDisplay(displayId);

        /// <summary>
        /// Terminate an EGL connection.
        /// </summary>
        public static bool Terminate(IntPtr display) => NativeMethods.Egl.Terminate(display);

        /// <summary>
        /// Destroy an EGL surface.
        /// </summary>
        public static bool DestroySurface(IntPtr display, IntPtr surface) => NativeMethods.Egl.DestroySurface(display, surface);

        /// <summary>
        /// Creates a new EGL window surface.
        /// </summary>
        public static IntPtr CreateWindowSurface(IntPtr display, IntPtr config, IntPtr window, int[] attrs) => NativeMethods.Egl.CreateWindowSurface(display, config, window, attrs);
    }

    internal class EGLContext : IDisposable
    {
        public const string gl_lib = "/usr/local/lib/arm-linux-gnueabihf/libGLESv2.so";

        public bool IsDisposed { get; private set; }

        //public static IntPtr GetCurrentContext() => Egl.GetCurrentContext();
        /// <summary>
        /// The native surface used by this device context.
        /// </summary>
        private NativeSurface _NativeSurface;

        /// <summary>
        /// Get the display connection.
        /// </summary>
        internal IntPtr Display => _NativeSurface?._Display ?? IntPtr.Zero;

        /// <summary>
        /// Get the EGL surface handle.
        /// </summary>
        private IntPtr EglSurface => _NativeSurface?.Handle ?? new IntPtr(NativeMethods.Egl.NO_DISPLAY);

        /// <summary>
        /// The frame buffer configuration.
        /// </summary>
        private IntPtr _Config;

        /// <summary>
		/// Get the flag indicating whether this DeviceContext has a pixel format defined.
		/// </summary>
		public bool IsPixelFormatSet { get; protected set; }

        private int[] DefaultConfigAttribs
        {
            get
            {
                List<int> configAttribs = new List<int>();

                configAttribs.AddRange(new[] { NativeMethods.Egl.RENDERABLE_TYPE, NativeMethods.Egl.OPENGL_ES2_BIT });
                configAttribs.AddRange(new[] { NativeMethods.Egl.SURFACE_TYPE, NativeMethods.Egl.PBUFFER_BIT | NativeMethods.Egl.WINDOW_BIT });
                configAttribs.AddRange(new[] {
                    NativeMethods.Egl.RED_SIZE, 8,
                    NativeMethods.Egl.GREEN_SIZE, 8,
                    NativeMethods.Egl.BLUE_SIZE, 8
                });
                configAttribs.Add(NativeMethods.Egl.NONE);

                return configAttribs.ToArray();
            }
        }

        private int[] DefaultContextAttribs
        {
            get
            {
                List<int> contextAttribs = new List<int>();
                contextAttribs.AddRange(new[] { NativeMethods.Egl.CONTEXT_CLIENT_VERSION, 2 });
                contextAttribs.Add(NativeMethods.Egl.NONE);
                return contextAttribs.ToArray();
            }
        }

        /// <summary>
        /// Determine whether the hosting platform is able to create a P-Buffer.
        /// </summary>
        public static readonly bool IsPBufferSupported = true;

        private static bool GL_Initialized { get; set; }
        public static NativeMethods.DynamicLibrary GLESv2_Lib { get; private set; }

        /// <summary>
		/// Default constructor.
		/// </summary>
		private EGLContext()
        {
            if (!GL_Initialized)
            {
                GLESv2_Lib = NativeMethods.DynamicLibrary.Load(gl_lib, NativeMethods.DynamicLibrary.FLAGS_RTLD_NOW);
                GL_Initialized = true;
            }
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="DeviceContextEGL"/> class.
		/// </summary>
		/// <param name='windowHandle'>
		/// A <see cref="IntPtr"/> that specifies the window handle used to create the device context.  If it is <see cref="IntPtr.Zero"/>
		/// the surface referenced by this NativeDeviceContext is a minimal PBuffer, or no surface at all in case EGL_KHR_surfaceless_context
		/// </param>
		/// <exception cref='InvalidOperationException'>
		/// Is thrown when an operation cannot be performed.
		/// </exception>
		public EGLContext(IntPtr windowHandle) : this(NativeSurface.DefaultDisplay, windowHandle)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceContextEGL"/> class.
        /// </summary>
        /// <param name="display">
        /// A <see cref="IntPtr"/> that specifies the display handle used to create <paramref name="windowHandle"/>.
        /// </param>
        /// <param name='windowHandle'>
        /// A <see cref="IntPtr"/> that specifies the window handle used to create the device context. If it is <see cref="IntPtr.Zero"/>
        /// the surface referenced by this NativeDeviceContext is a minimal PBuffer, or no surface at all in case EGL_KHR_surfaceless_context
        /// is supported.
        /// </param>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when an operation cannot be performed.
        /// </exception>
        public EGLContext(IntPtr display, IntPtr windowHandle) : this()
        {
            _NativeSurface = new NativeWindow(display, windowHandle);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceContextEGL"/> class.
        /// </summary>
        /// <param name='nativeBuffer'>
        /// A <see cref="INativePBuffer"/> that specifies the P-Buffer used to create the device context.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if <paramref name="nativeBuffer"/> is null.
        /// </exception>
        public EGLContext(INativePBuffer nativeBuffer) : this()
        {
            if (nativeBuffer == null)
                throw new ArgumentNullException(nameof(nativeBuffer));

            NativePBuffer nativePBuffer = nativeBuffer as NativePBuffer;
            _NativeSurface = nativePBuffer ?? throw new ArgumentException("INativePBuffer not created with DeviceContext.CreatePBuffer");
            IsPixelFormatSet = true;
        }

        /// <summary>
        /// Create a simple context.
        /// </summary>
        /// <returns>
        /// A <see cref="IntPtr"/> that represents the handle of the created context. If the context cannot be
        /// created, it returns IntPtr.Zero.
        /// </returns>
        internal IntPtr CreateSimpleContext()
        {
            IntPtr ctx;

            int[] configAttribs = DefaultConfigAttribs;
            int[] configCount = new int[1];
            IntPtr[] configs = new IntPtr[8];

            if (!NativeMethods.Egl.ChooseConfig(Display, configAttribs, configs, configs.Length, configCount))
                throw new InvalidOperationException("unable to choose configuration");

            if (configCount[0] == 0)
                throw new InvalidOperationException("no available configuration");

            int[] contextAttribs = DefaultContextAttribs;
            int[] surfaceAttribs = { NativeMethods.Egl.NONE };

            if (!NativeMethods.Egl.BindAPI(NativeMethods.Egl.OPENGL_ES_API))
                throw new InvalidOperationException("no ES API");

            if ((ctx = NativeMethods.Egl.CreateContext(Display, configs[0], IntPtr.Zero, contextAttribs)) == IntPtr.Zero)
                throw new InvalidOperationException("unable to create context");

            if (_NativeSurface.Handle == IntPtr.Zero)
            {
                List<int> pbufferAttribs = new List<int>(surfaceAttribs);

                pbufferAttribs.RemoveAt(pbufferAttribs.Count - 1);
                pbufferAttribs.AddRange(new[] { NativeMethods.Egl.WIDTH, 1, NativeMethods.Egl.HEIGHT, 1 });
                pbufferAttribs.Add(NativeMethods.Egl.NONE);

                _NativeSurface.CreateHandle(configs[0], pbufferAttribs.ToArray());
            }

            return ctx;
        }

        /// <summary>
        /// Creates a context.
        /// </summary>
        /// <param name="sharedContext">
        /// A <see cref="IntPtr"/> that specify a context that will share objects with the returned one. If
        /// it is IntPtr.Zero, no sharing is performed.
        /// </param>
        /// <returns>
        /// A <see cref="IntPtr"/> that represents the handle of the created context. If the context cannot be
        /// created, it returns IntPtr.Zero.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Exception thrown in the case <paramref name="sharedContext"/> is different from IntPtr.Zero, and the objects
        /// cannot be shared with it.
        /// </exception>
        public IntPtr CreateContext(IntPtr sharedContext)
        {
            return CreateContextAttrib(sharedContext, DefaultContextAttribs);
        }

        /// <summary>
        /// Creates a context, specifying attributes.
        /// </summary>
        /// <param name="sharedContext">
        /// A <see cref="IntPtr"/> that specify a context that will share objects with the returned one. If
        /// it is IntPtr.Zero, no sharing is performed.
        /// </param>
        /// <param name="attribsList">
        /// A <see cref="T:Int32[]"/> that specifies the attributes list.
        /// </param>
        /// <param name="api">
        /// A <see cref="KhronosVersion"/> that specifies the API to be implemented by the returned context. It can be null indicating the
        /// default API for this DeviceContext implementation. If it is possible, try to determine the API version also.
        /// </param>
        /// <returns>
        /// A <see cref="IntPtr"/> that represents the handle of the created context. If the context cannot be
        /// created, it returns IntPtr.Zero.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if <paramref name="attribsList"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Exception thrown if <paramref name="attribsList"/> length is zero or if the last item of <paramref name="attribsList"/>
        /// is not zero.
        /// </exception>
        public IntPtr CreateContextAttrib(IntPtr sharedContext, int[] attribsList)
        {
            if (attribsList == null)
                throw new ArgumentNullException(nameof(attribsList));
            if (attribsList.Length == 0)
                throw new ArgumentException("zero length array", nameof(attribsList));
            if (attribsList[attribsList.Length - 1] != NativeMethods.Egl.NONE)
                throw new ArgumentException("not EGL_NONE-terminated array", nameof(attribsList));

            IntPtr context;

            // Select surface pixel format automatically
            if (_NativeSurface != null && _NativeSurface.Handle != IntPtr.Zero)
            {
                int[] configId = new int[1];

                if (NativeMethods.Egl.QuerySurface(Display, EglSurface, NativeMethods.Egl.CONFIG_ID, configId) == false)
                    throw new InvalidOperationException("unable to query EGL surface config ID");

                _Config = ChoosePixelFormat(Display, configId[0]);
            }

            if (!NativeMethods.Egl.BindAPI(NativeMethods.Egl.OPENGL_ES_API))
                throw new InvalidOperationException("Unable to bind GLES API");

            // Create context
            if ((context = NativeMethods.Egl.CreateContext(Display, _Config, sharedContext, attribsList)) == IntPtr.Zero)
                throw new EglException(NativeMethods.Egl.GetError());

            // Create native surface (pixel format pending)
            // @todo Back-buffer?
            if (_NativeSurface != null && _NativeSurface.Handle == IntPtr.Zero)
                _NativeSurface.CreateHandle(_Config, new[] { NativeMethods.Egl.NONE });

            return context;
        }

        /// <summary>
		/// Makes the context current on the calling thread.
		/// </summary>
		/// <param name="ctx">
		/// A <see cref="IntPtr"/> that specify the context to be current on the calling thread, bound to
		/// thise device context. It can be IntPtr.Zero indicating that no context will be current.
		/// </param>
		/// <returns>
		/// It returns a boolean value indicating whether the operation was successful.
		/// </returns>
		/// <exception cref="NotSupportedException">
		/// Exception thrown if the current platform is not supported.
		/// </exception>
		public bool MakeCurrent(IntPtr context)
        {
            // Basic implementation
            bool current = MakeCurrentCore(context);

            // Link OpenGL procedures on Gl
            if (context == IntPtr.Zero || !current)
                return current;

            //GL.BindAPI();
            return true;
        }

        /// <summary>
        /// Makes the context current on the calling thread.
        /// </summary>
        /// <param name="ctx">
        /// A <see cref="IntPtr"/> that specify the context to be current on the calling thread, bound to
        /// thise device context. It can be IntPtr.Zero indicating that no context will be current.
        /// </param>
        /// <returns>
        /// It returns a boolean value indicating whether the operation was successful.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Exception thrown if the current platform is not supported.
        /// </exception>
        protected bool MakeCurrentCore(IntPtr context)
        {
            if (context != IntPtr.Zero)
            {
                int contextClientType = 0;
                if (NativeMethods.Egl.QueryContext(Display, context, NativeMethods.Egl.CONTEXT_CLIENT_TYPE, ref contextClientType))
                {
                    if (!NativeMethods.Egl.BindAPI((uint)contextClientType))
                        throw new InvalidOperationException("no ES API");
                }

                return NativeMethods.Egl.MakeCurrent(Display, EglSurface, EglSurface, context);
            }
            else
            {
                return NativeMethods.Egl.MakeCurrent(Display, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Deletes a context.
        /// </summary>
        /// <param name="ctx">
        /// A <see cref="IntPtr"/> that specify the context to be deleted.
        /// </param>
        /// <returns>
        /// It returns a boolean value indicating whether the operation was successful. If it returns false,
        /// query the exception by calling <see cref="GetPlatformException"/>.
        /// </returns>
        /// <remarks>
        /// <para>The context <paramref name="ctx"/> must not be current on any thread.</para>
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Exception thrown if <paramref name="ctx"/> is IntPtr.Zero.
        /// </exception>
        public bool DeleteContext(IntPtr context)
        {
            if (context == IntPtr.Zero)
                throw new ArgumentNullException("ctx");

            return NativeMethods.Egl.DestroyContext(Display, context);
        }

        /// <summary>
        /// Swap the buffers of a device.
        /// </summary>
        public void SwapBuffers()
        {
            NativeMethods.Egl.SwapBuffers(Display, EglSurface);
        }

        /// <summary>
        /// Control the the buffers swap of a device.
        /// </summary>
        /// <param name="interval">
        /// A <see cref="Int32"/> that specifies the minimum number of video frames that are displayed
        /// before a buffer swap will occur.
        /// </param>
        /// <returns>
        /// It returns a boolean value indicating whether the operation was successful.
        /// </returns>
        public bool SwapInterval(int interval)
        {
            return NativeMethods.Egl.SwapInterval(Display, interval);
        }

        /// <summary>
        /// Get the pixel formats supported by this device.
        /// </summary>
        public DevicePixelFormatCollection PixelsFormats
        {
            get
            {
                // Use cached pixel formats
                if (_PixelFormatCache != null)
                    return _PixelFormatCache;

                _PixelFormatCache = new DevicePixelFormatCollection();

                // Get the number of pixel formats
                int configCount;
                if (NativeMethods.Egl.GetConfigs(Display, null, 0, out configCount) == false)
                    throw new InvalidOperationException("unable to get configurations count");

                IntPtr[] configs = new IntPtr[configCount];
                if (NativeMethods.Egl.GetConfigs(Display, configs, configs.Length, out configCount) == false)
                    throw new InvalidOperationException("unable to get configurations");

                foreach (IntPtr config in configs)
                {
                    DevicePixelFormat pixelFormat = new DevicePixelFormat();
                    int param;

                    if (NativeMethods.Egl.GetConfigAttrib(Display, config, NativeMethods.Egl.CONFIG_ID, out pixelFormat.FormatIndex) == false)
                        throw new InvalidOperationException("unable to get configuration parameter CONFIG_ID");

                    // Defaults to RGBA
                    pixelFormat.RgbaUnsigned = true;
                    pixelFormat.RenderWindow = true;
                    pixelFormat.RenderBuffer = false;

                    if (NativeMethods.Egl.GetConfigAttrib(Display, config, NativeMethods.Egl.BUFFER_SIZE, out param) == false)
                        throw new InvalidOperationException("unable to get configuration parameter BUFFER_SIZE");
                    pixelFormat.ColorBits = param;

                    if (NativeMethods.Egl.GetConfigAttrib(Display, config, NativeMethods.Egl.RED_SIZE, out pixelFormat.RedBits) == false)
                        throw new InvalidOperationException("unable to get configuration parameter RED_SIZE");
                    if (NativeMethods.Egl.GetConfigAttrib(Display, config, NativeMethods.Egl.GREEN_SIZE, out pixelFormat.GreenBits) == false)
                        throw new InvalidOperationException("unable to get configuration parameter GREEN_SIZE");
                    if (NativeMethods.Egl.GetConfigAttrib(Display, config, NativeMethods.Egl.BLUE_SIZE, out pixelFormat.BlueBits) == false)
                        throw new InvalidOperationException("unable to get configuration parameter BLUE_SIZE");
                    if (NativeMethods.Egl.GetConfigAttrib(Display, config, NativeMethods.Egl.ALPHA_SIZE, out pixelFormat.AlphaBits) == false)
                        throw new InvalidOperationException("unable to get configuration parameter ALPHA_SIZE");
                    if (NativeMethods.Egl.GetConfigAttrib(Display, config, NativeMethods.Egl.ALPHA_MASK_SIZE, out param) == false)
                        throw new InvalidOperationException("unable to get configuration parameter ALPHA_MASK_SIZE");

                    if (NativeMethods.Egl.GetConfigAttrib(Display, config, NativeMethods.Egl.DEPTH_SIZE, out param) == false)
                        throw new InvalidOperationException("unable to get configuration parameter DEPTH_SIZE");
                    pixelFormat.DepthBits = param;

                    if (NativeMethods.Egl.GetConfigAttrib(Display, config, NativeMethods.Egl.STENCIL_SIZE, out param) == false)
                        throw new InvalidOperationException("unable to get configuration parameter STENCIL_SIZE");
                    pixelFormat.StencilBits = param;

                    if (NativeMethods.Egl.GetConfigAttrib(Display, config, NativeMethods.Egl.SAMPLES, out param) == false)
                        throw new InvalidOperationException("unable to get configuration parameter SAMPLES");
                    pixelFormat.MultisampleBits = param;

                    if (NativeMethods.Egl.GetConfigAttrib(Display, config, NativeMethods.Egl.CONFIG_CAVEAT, out param) == false)
                        throw new InvalidOperationException("unable to get configuration parameter CONFIG_CAVEAT");
                    switch (param)
                    {
                        case NativeMethods.Egl.NONE:
                            break;
                        case NativeMethods.Egl.SLOW_CONFIG:
                            continue;           // Skip software implementations?
                        case NativeMethods.Egl.NON_CONFORMANT_CONFIG:
                            break;
                    }

                    // EGL 1.2 attributes
                    if (NativeMethods.Egl.GetConfigAttrib(Display, config, NativeMethods.Egl.COLOR_BUFFER_TYPE, out param) == false)
                        throw new InvalidOperationException("unable to get configuration parameter COLOR_BUFFER_TYPE");
                    switch (param)
                    {
                        case NativeMethods.Egl.RGB_BUFFER:
                            break;
                        case NativeMethods.Egl.LUMINANCE_BUFFER:
                            if (NativeMethods.Egl.GetConfigAttrib(Display, config, NativeMethods.Egl.LUMINANCE_SIZE, out param) == false)
                                throw new InvalidOperationException("unable to get configuration parameter LUMINANCE_SIZE");
                            // Overrides color bits
                            pixelFormat.ColorBits = param;

                            // ATM do not support luminance buffers
                            continue;
                    }

                    if (NativeMethods.Egl.GetConfigAttrib(Display, config, NativeMethods.Egl.MAX_SWAP_INTERVAL, out param) == false)
                        throw new InvalidOperationException("unable to get configuration parameter MAX_SWAP_INTERVAL");
                    if (NativeMethods.Egl.GetConfigAttrib(Display, config, NativeMethods.Egl.MIN_SWAP_INTERVAL, out param) == false)
                        throw new InvalidOperationException("unable to get configuration parameter MIN_SWAP_INTERVAL");

                    // EGL 1.3 attributes
                    if (NativeMethods.Egl.GetConfigAttrib(Display, config, NativeMethods.Egl.CONFORMANT, out param) == false)
                        throw new InvalidOperationException("unable to get configuration parameter CONFORMANT");

                    if ((param & NativeMethods.Egl.OPENGL_ES2_BIT) != 0)
                    {

                    }
                    else continue;

                    if ((param & NativeMethods.Egl.OPENGL_ES_BIT) != 0)
                    {

                    }

                    if ((param & NativeMethods.Egl.OPENVG_BIT) != 0)
                    {

                    }

                    if ((param & NativeMethods.Egl.OPENGL_BIT) != 0)
                    {

                    }

                    // Not implemented by ANGLE
                    //if (Egl.GetConfigAttrib(_Display, config, Egl.MATCH_NATIVE_PIXMAP, param) == false)
                    //	throw new InvalidOperationException("unable to get configuration parameter MATCH_NATIVE_PIXMAP");

                    // EGL 1.4 attributes
                    if (NativeMethods.Egl.GetConfigAttrib(Display, config, NativeMethods.Egl.SURFACE_TYPE, out param) == false)
                        throw new InvalidOperationException("unable to get configuration parameter SURFACE_TYPE");

                    if ((param & NativeMethods.Egl.MULTISAMPLE_RESOLVE_BOX_BIT) != 0) { }
                    if ((param & NativeMethods.Egl.PBUFFER_BIT) != 0) { }
                    if ((param & NativeMethods.Egl.PIXMAP_BIT) != 0) { }
                    if ((param & NativeMethods.Egl.SWAP_BEHAVIOR_PRESERVED_BIT) != 0) { }
                    if ((param & NativeMethods.Egl.VG_ALPHA_FORMAT_PRE_BIT) != 0) { }
                    if ((param & NativeMethods.Egl.VG_COLORSPACE_LINEAR_BIT) != 0) { }
                    
                    if ((param & NativeMethods.Egl.WINDOW_BIT) == 0)
                        pixelFormat.RenderWindow = false;

                    // Double buffer and swap method can be determined only later, once the pixel format is set
                    pixelFormat.DoubleBuffer = true;
                    pixelFormat.SwapMethod = 0;

                    _PixelFormatCache.Add(pixelFormat);
                }

                return _PixelFormatCache;
            }
        }

        /// <summary>
        /// Set the device pixel format.
        /// </summary>
        /// <param name="pixelFormat">
        /// A <see cref="DevicePixelFormat"/> that specifies the pixel format to set.
        /// </param>
        public void ChoosePixelFormat(DevicePixelFormat pixelFormat)
        {
            if (_NativeSurface == null)
                return; // Support EGL_KHR_surfaceless_context

            if (_NativeSurface.Handle != IntPtr.Zero)
                throw new InvalidOperationException("pixel format already set");
            _Config = ChoosePixelFormat(Display, pixelFormat);

            IsPixelFormatSet = true;
        }

        /// <summary>
        /// Set the device pixel format.
        /// </summary>
        /// <param name="pixelFormat">
        /// A <see cref="DevicePixelFormat"/> that specifies the pixel format to set.
        /// </param>
        private static IntPtr ChoosePixelFormat(IntPtr display, DevicePixelFormat pixelFormat)
        {
            if (pixelFormat == null)
                throw new ArgumentNullException(nameof(pixelFormat));

            List<int> configAttribs = new List<int>();
            int[] configCount = new int[1];
            IntPtr[] configs = new IntPtr[8];
            int surfaceType = 0;

            configAttribs.AddRange(new[] { NativeMethods.Egl.RENDERABLE_TYPE, NativeMethods.Egl.OPENGL_ES2_BIT });

            if (pixelFormat.RenderWindow)
                surfaceType |= NativeMethods.Egl.WINDOW_BIT;
            if (pixelFormat.RenderPBuffer)
                surfaceType |= NativeMethods.Egl.PBUFFER_BIT;
            if (surfaceType != 0)
                configAttribs.AddRange(new[] { NativeMethods.Egl.SURFACE_TYPE, surfaceType });

            switch (pixelFormat.ColorBits)
            {
                case 24:
                    configAttribs.AddRange(new[] { NativeMethods.Egl.RED_SIZE, 8, NativeMethods.Egl.GREEN_SIZE, 8, NativeMethods.Egl.BLUE_SIZE, 8 });
                    break;
                case 32:
                    configAttribs.AddRange(new[] { NativeMethods.Egl.RED_SIZE, 8, NativeMethods.Egl.GREEN_SIZE, 8, NativeMethods.Egl.BLUE_SIZE, 8, NativeMethods.Egl.ALPHA_SIZE, 8 });
                    break;
                default:
                    configAttribs.AddRange(new[] { NativeMethods.Egl.BUFFER_SIZE, pixelFormat.ColorBits });
                    break;
            }
            if (pixelFormat.DepthBits > 0)
                configAttribs.AddRange(new[] { NativeMethods.Egl.DEPTH_SIZE, pixelFormat.DepthBits });
            if (pixelFormat.StencilBits > 0)
                configAttribs.AddRange(new[] { NativeMethods.Egl.STENCIL_SIZE, pixelFormat.StencilBits });

            configAttribs.Add(NativeMethods.Egl.NONE);

            if (NativeMethods.Egl.ChooseConfig(display, configAttribs.ToArray(), configs, configs.Length, configCount) == false)
                throw new InvalidOperationException("unable to choose configuration");
            if (configCount[0] == 0)
                throw new InvalidOperationException("no available configuration");

            return configs[0];
        }

        private static IntPtr ChoosePixelFormat(IntPtr display, int configId)
        {
            List<int> configAttribs = new List<int>();
            int[] configCount = new int[1];
            IntPtr[] configs = new IntPtr[8];

            configAttribs.AddRange(new[] { NativeMethods.Egl.CONFIG_ID, configId });
            configAttribs.Add(NativeMethods.Egl.NONE);

            if (NativeMethods.Egl.ChooseConfig(display, configAttribs.ToArray(), configs, configs.Length, configCount) == false)
                throw new InvalidOperationException("unable to choose configuration");
            if (configCount[0] == 0)
                throw new InvalidOperationException("no available configuration");

            return configs[0];
        }

        /// <summary>
        /// Set the device pixel format.
        /// </summary>
        /// <param name="pixelFormat">
        /// A <see cref="DevicePixelFormat"/> that specifies the pixel format to set.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if <paramref name="pixelFormat"/> is null.
        /// </exception>
        public void SetPixelFormat(DevicePixelFormat pixelFormat)
        {
            if (pixelFormat == null)
                throw new ArgumentNullException(nameof(pixelFormat));
            if (_NativeSurface == null)
                return; // Support EGL_KHR_surfaceless_context
            if (_NativeSurface.Handle != IntPtr.Zero)
                throw new InvalidOperationException("pixel format already set");

            List<int> configAttribs = new List<int>();

            configAttribs.AddRange(new[] { NativeMethods.Egl.RENDERABLE_TYPE, NativeMethods.Egl.OPENGL_ES2_BIT });
            configAttribs.AddRange(new[] {
                NativeMethods.Egl.CONFIG_ID, pixelFormat.FormatIndex
            });
            configAttribs.Add(NativeMethods.Egl.NONE);

            int[] configCount = new int[1];
            IntPtr[] configs = new IntPtr[1];

            if (NativeMethods.Egl.ChooseConfig(Display, configAttribs.ToArray(), configs, 1, configCount) == false)
                throw new InvalidOperationException("unable to choose configuration");
            if (configCount[0] == 0)
                throw new InvalidOperationException("no available configuration");

            _Config = configs[0];

            IsPixelFormatSet = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting managed/unmanaged resources.
        /// </summary>
        /// <param name="disposing">
        /// A <see cref="System.Boolean"/> indicating whether the disposition is requested explictly.
        /// </param>
        protected void Dispose(bool disposing)
        {
            // Note: display must be disposed after device context disposition
            if (disposing)
            {
                if (_NativeSurface != null)
                {
                    _NativeSurface.Dispose();
                    _NativeSurface = null;
                }
            }
        }

        /// <summary>
        /// Pixel formats available on this DeviceContext (cache).
        /// </summary>
        private DevicePixelFormatCollection _PixelFormatCache;

        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Basic native EGL surface.
        /// </summary>
        /// <remarks>
        /// Holds EGL display and version.
        /// </remarks>
        public abstract class NativeSurface : IDisposable
        {
            #region Constructors

            /// <summary>
            /// Default constructor.
            /// </summary>
            /// <param name="display">
            /// A <see cref="IntPtr"/> that specifies the display handle to be passed to <see cref="Egl.GetDisplay(IntPtr)"/>.
            /// </param>
            protected NativeSurface(IntPtr display)
            {
                if ((_Display = Egl.GetDisplay(display)) == IntPtr.Zero)
                    throw new InvalidOperationException("unable to get display handle");
                if (!Egl.Initialize(_Display, 2, 1))
                    throw new InvalidOperationException("unable to initialize the display");
            }

            #endregion

            #region Handles

            /// <summary>
            /// The default display handle.
            /// </summary>
            public static readonly IntPtr DefaultDisplay = new IntPtr(NativeMethods.Egl.DEFAULT_DISPLAY);

            /// <summary>
            /// Get the native surface handle.
            /// </summary>
            public abstract IntPtr Handle { get; }

            /// <summary>
            /// Create the native surface handle.
            /// </summary>
            /// <param name="configId">
            /// A <see cref="IntPtr"/> that specifies the configuration ID.
            /// </param>
            /// <param name="attribs">
            /// A <see cref="T:int[]"/> that lists the handle attributes.
            /// </param>
            /// <exception cref="InvalidOperationException">
            /// Exception thrown if the handle is already created.
            /// </exception>
            public abstract void CreateHandle(IntPtr configId, int[] attribs);

            /// <summary>
            /// Get the display handle associated this instance.
            /// </summary>
            protected internal IntPtr _Display;

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public virtual void Dispose()
            {
                if (_Display != IntPtr.Zero)
                {
                    Egl.Terminate(_Display);
                    _Display = IntPtr.Zero;
                }
            }

            #endregion
        }

        public interface INativeWindow : IDisposable
        {
            /// <summary>
            /// Get the display handle associated this instance.
            /// </summary>
            IntPtr Display { get; }

            /// <summary>
            /// Get the native window handle.
            /// </summary>
            IntPtr Handle { get; }
        }

        /// <summary>
        /// Native P-Buffer interface.
        /// </summary>
        public interface INativePBuffer : IDisposable
        {

        }

        /// <summary>
        /// Native window implementation for Windows.
        /// </summary>
        public sealed class NativeWindow : NativeSurface, INativeWindow
        {
            #region Constructors

            /// <summary>
            /// Construct a NativeWindow.
            /// </summary>
            /// <param name="display">
            /// A <see cref="IntPtr"/> that specifies the display handle to be passed to <see cref="Egl.GetDisplay(IntPtr)"/>.
            /// </param>
            //public NativeWindow(IntPtr display) : this(display, GL.NativeWindow.Handle)
            //{

            //}

            /// <summary>
            /// Construct a NativeWindow on an OS window
            /// </summary>
            /// <param name="display">
            /// A <see cref="IntPtr"/> that specifies the display handle to be passed to <see cref="Egl.GetDisplay(IntPtr)"/>.
            /// </param>
            /// <param name="windowHandle">
            /// A <see cref="IntPtr"/> that specifies the handle of the OS window.
            /// </param>
            public NativeWindow(IntPtr display, IntPtr windowHandle) : this(display, windowHandle, null)
            {
                
            }

            /// <summary>
            /// Construct a NativeWindow on an OS window.
            /// </summary>
            /// <param name="display">
            /// A <see cref="IntPtr"/> that specifies the display handle to be passed to <see cref="Egl.GetDisplay(IntPtr)"/>.
            /// </param>
            /// <param name="windowHandle">
            /// A <see cref="IntPtr"/> that specifies the handle of the OS window.
            /// </param>
            /// <param name="pixelFormat">
            /// A <see cref="DevicePixelFormat"/> used for choosing the NativeWindow pixel format configuration. It can
            /// be null; in this case the pixel format will be set elsewhere.
            /// </param>
            public NativeWindow(IntPtr display, IntPtr windowHandle, DevicePixelFormat pixelFormat) : base(display)
            {
                try
                {
                    // Hold the window handle in case pixel format will be set later
                    _WindowHandle = windowHandle;

                    // Choose appropriate pixel format
                    if (pixelFormat != null)
                    {
                        pixelFormat.RenderWindow = true;

                        IntPtr configId = ChoosePixelFormat(_Display, pixelFormat);
                        List<int> attribs = new List<int>();

                        if (pixelFormat.DoubleBuffer)
                            attribs.AddRange(new[] { NativeMethods.Egl.RENDER_BUFFER, NativeMethods.Egl.BACK_BUFFER });
                        attribs.Add(NativeMethods.Egl.NONE);

                        CreateHandle(configId, attribs.ToArray());
                    }
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            #endregion

            #region Handles

            /// <summary>
            /// The EGL window handle.
            /// </summary>
            private IntPtr _Handle;

            /// <summary>
            /// The OS window handle.
            /// </summary>
            private readonly IntPtr _WindowHandle;

            #endregion

            /// <summary>
            /// Get the EGL window handle.
            /// </summary>
            public override IntPtr Handle => _Handle;

            /// <summary>
            /// Create the native surface handle.
            /// </summary>
            /// <param name="configId">
            /// A <see cref="IntPtr"/> that specifies the configuration ID.
            /// </param>
            /// <param name="attribs">
            /// A <see cref="T:int[]"/> that lists the handle attributes.
            /// </param>
            /// <exception cref="InvalidOperationException">
            /// Exception thrown if the handle is already created.
            /// </exception>
            public override void CreateHandle(IntPtr configId, int[] attribs)
            {
                if (_Handle != IntPtr.Zero)
                    throw new InvalidOperationException("handle already created");

                if (_WindowHandle != IntPtr.Zero)
                {
                    if ((_Handle = Egl.CreateWindowSurface(_Display, configId, _WindowHandle, attribs)) == IntPtr.Zero)
                        throw new InvalidOperationException("unable to create window surface");
                }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public override void Dispose()
            {
                if (_Handle != IntPtr.Zero)
                {
                    bool res = Egl.DestroySurface(_Display, _Handle);
                    _Handle = IntPtr.Zero;
                }

                // Base implementation
                base.Dispose();
            }

            /// <summary>
            /// Get the display handle associated this instance.
            /// </summary>
            IntPtr INativeWindow.Display => _Display;

            /// <summary>
            /// Get the native window handle.
            /// </summary>
            IntPtr INativeWindow.Handle => _Handle;
        }

        /// <summary>
        /// P-Buffer implementation for EGL.
        /// </summary>
        public sealed class NativePBuffer : NativeSurface, INativePBuffer, INativeWindow
        {
            #region Constructors

            /// <summary>
            /// Construct a NativePBuffer with a specific pixel format and size.
            /// </summary>
            /// <param name="pixelFormat">
            /// A <see cref="DevicePixelFormat"/> that specifies the pixel format and the ancillary buffers required.
            /// </param>
            /// <param name="width">
            /// A <see cref="UInt32"/> that specifies the width of the P-Buffer, in pixels.
            /// </param>
            /// <param name="height">
            /// A <see cref="UInt32"/> that specifies the height of the P-Buffer, in pixels.
            /// </param>
            public NativePBuffer(DevicePixelFormat pixelFormat, uint width, uint height) : this(DefaultDisplay, pixelFormat, width, height)
            {

            }

            /// <summary>
            /// Construct a NativePBuffer with a specific pixel format and size.
            /// </summary>
            /// <param name="display">
            /// 
            /// </param>
            /// <param name="pixelFormat">
            /// A <see cref="DevicePixelFormat"/> that specifies the pixel format and the ancillary buffers required.
            /// </param>
            /// <param name="width">
            /// A <see cref="UInt32"/> that specifies the width of the P-Buffer, in pixels.
            /// </param>
            /// <param name="height">
            /// A <see cref="UInt32"/> that specifies the height of the P-Buffer, in pixels.
            /// </param>
            public NativePBuffer(IntPtr display, DevicePixelFormat pixelFormat, uint width, uint height) : base(display)
            {
                if (pixelFormat == null)
                    throw new ArgumentNullException(nameof(pixelFormat));

                try
                {
                    // Choose appropriate pixel format
                    pixelFormat.RenderWindow = false;
                    pixelFormat.RenderPBuffer = true;

                    IntPtr configId = ChoosePixelFormat(_Display, pixelFormat);
                    List<int> attribs = new List<int>();

                    attribs.AddRange(new[] { NativeMethods.Egl.WIDTH, (int)width });
                    attribs.AddRange(new[] { NativeMethods.Egl.HEIGHT, (int)height });
                    attribs.Add(NativeMethods.Egl.NONE);

                    CreateHandle(configId, attribs.ToArray());
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            #endregion

            #region Handles

            /// <summary>
            /// The P-Buffer handle.
            /// </summary>
            private IntPtr _Handle;

            #endregion

            #region NativeSurface Overrides

            /// <summary>
            /// Get the EGL window handle.
            /// </summary>
            public override IntPtr Handle => _Handle;

            /// <summary>
            /// Create the native surface handle.
            /// </summary>
            /// <param name="configId">
            /// A <see cref="IntPtr"/> that specifies the configuration ID.
            /// </param>
            /// <param name="attribs">
            /// A <see cref="T:int[]"/> that lists the handle attributes.
            /// </param>
            /// <exception cref="InvalidOperationException">
            /// Exception thrown if the handle is already created.
            /// </exception>
            public override void CreateHandle(IntPtr configId, int[] attribs)
            {
                if (_Handle != IntPtr.Zero)
                    throw new InvalidOperationException("handle already created");

                if ((_Handle = NativeMethods.Egl.CreatePbufferSurface(_Display, configId, attribs)) == IntPtr.Zero)
                    throw new InvalidOperationException("unable to create window surface");
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public override void Dispose()
            {
                if (_Handle != IntPtr.Zero)
                {
                    bool res = Egl.DestroySurface(_Display, _Handle);
                    _Handle = IntPtr.Zero;
                }

                // Base implementation
                base.Dispose();
            }

            #endregion

            #region INativeWindow Implementation

            /// <summary>
            /// Get the display handle associated this instance.
            /// </summary>
            IntPtr INativeWindow.Display => _Display;

            /// <summary>
            /// Get the native window handle.
            /// </summary>
            IntPtr INativeWindow.Handle => _Handle;

            #endregion
        }

    }

    /// <summary>
	/// Exception thrown by Egl class.
	/// </summary>
	public sealed class EglException : Exception
    {
        public int ErrorCode = -1;

        /// <summary>
        /// Construct a EglException.
        /// </summary>
        /// <param name="errorCode">
        /// A <see cref="Int32"/> that specifies the error code.
        /// </param>
        public EglException(int errorCode) : base(GetErrorMessage(errorCode))
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Returns a description of the error code.
        /// </summary>
        /// <param name="errorCode">
        /// A <see cref="Int32"/> that specifies the error code.
        /// </param>
        /// <returns>
        /// It returns a description of <paramref name="errorCode"/>.
        /// </returns>
        private static string GetErrorMessage(int errorCode)
        {
            switch (errorCode)
            {
                default:
                    return $"unknown error code 0x{errorCode:X8}";
                case NativeMethods.Egl.SUCCESS:
                    return "no error";
                case NativeMethods.Egl.NOT_INITIALIZED:
                    return "EGL is not initialized, or could not be initialized, for the specified EGL display connection";
                case NativeMethods.Egl.BAD_ACCESS:
                    return "EGL cannot access a requested resource";
                case NativeMethods.Egl.BAD_ALLOC:
                    return "EGL failed to allocate resources for the requested operation";
                case NativeMethods.Egl.BAD_ATTRIBUTE:
                    return "an unrecognized attribute or attribute value was passed in the attribute list";
                case NativeMethods.Egl.BAD_CONTEXT:
                    return "an EGLContext argument does not name a valid EGL rendering context";
                case NativeMethods.Egl.BAD_CONFIG:
                    return "an EGLConfig argument does not name a valid EGL frame buffer configuration";
                case NativeMethods.Egl.BAD_CURRENT_SURFACE:
                    return "the current surface of the calling thread is a window, pixel buffer or pixmap that is no longer valid";
                case NativeMethods.Egl.BAD_DISPLAY:
                    return "an EGLDisplay argument does not name a valid EGL display connection";
                case NativeMethods.Egl.BAD_SURFACE:
                    return "an EGLSurface argument does not name a valid surface configured for GL rendering";
                case NativeMethods.Egl.BAD_MATCH:
                    return "arguments are inconsistent";
                case NativeMethods.Egl.BAD_PARAMETER:
                    return "one or more argument values are invalid";
                case NativeMethods.Egl.BAD_NATIVE_PIXMAP:
                    return "a NativePixmapType argument does not refer to a valid native pixmap";
                case NativeMethods.Egl.BAD_NATIVE_WINDOW:
                    return "a NativeWindowType argument does not refer to a valid native window";
                case NativeMethods.Egl.CONTEXT_LOST:
                    return "a power management event has occurred";
            }
        }
    }

    /// <summary>
	/// Exception thrown by Egl class.
	/// </summary>
	public sealed class GLException : Exception
    {
        public ErrorCode errorCode = ErrorCode.NoError;

        /// <summary>
        /// Construct a GLException.
        /// </summary>
        /// <param name="errorCode">
        /// A <see cref="Int32"/> that specifies the error code.
        /// </param>
        public GLException(ErrorCode error) : this(error, GetErrorMessage(error)) { }
        public GLException(ErrorCode error, string message) : base(message)
        {
            errorCode = error;
        }

        /// <summary>
        /// Returns a description of the error code.
        /// </summary>
        /// <param name="errorCode">
        /// A <see cref="Int32"/> that specifies the error code.
        /// </param>
        /// <returns>
        /// It returns a description of <paramref name="errorCode"/>.
        /// </returns>
        private static string GetErrorMessage(ErrorCode errorCode)
        {
            switch (errorCode)
            {
                default:
                case ErrorCode.NoError:
                    return $"No error";
                case ErrorCode.OutOfMemory:
                    return $"Out of memory";
                case ErrorCode.StackOverflow:
                    return $"Stack overflow";
                case ErrorCode.StackUnderflow:
                    return $"Stack underflow";
                case ErrorCode.TableTooLarge:
                    return $"Table too large";
                case ErrorCode.TextureTooLargeExt:
                    return $"Texture too large";
                case ErrorCode.InvalidValue:
                    return $"Invalid value";
                case ErrorCode.InvalidOperation:
                    return $"Invalid operation";
                case ErrorCode.InvalidFramebufferOperation:
                    return $"Invalid framebuffer operation";
                case ErrorCode.InvalidEnum:
                    return $"Invalid enum";
                case ErrorCode.ContextLost:
                    return $"Context lost";
            }
        }

        public static void CheckError(string context = "")
        {
            ErrorCode code = GL.GetError();
            if (code != ErrorCode.NoError)
            {
                if (string.IsNullOrWhiteSpace(context))
                    throw new GLException(code);
                else throw new GLException(code, string.Format("{0}: {1}", context, GetErrorMessage(code)));
            }
        }
    }

    public sealed class DevicePixelFormat
    {
        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public DevicePixelFormat()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colorBits"></param>
        public DevicePixelFormat(int colorBits)
        {
            RgbaUnsigned = true;
            RenderWindow = true;
            ColorBits = colorBits;
        }

        #region Format Identification

        /// <summary>
        /// Pixel format index.
        /// </summary>
        public int FormatIndex;

        #endregion

        #region Pixel Type

        /// <summary>
        /// Flag indicating whether this pixel format provide canonical (normalized) unsigned integer RGBA color.
        /// </summary>
        public bool RgbaUnsigned;

        /// <summary>
        /// Flag indicating whether this pixel format provide RGBA color composed by single-precision floating-point.
        /// </summary>
        public bool RgbaFloat;

        #endregion

        #region Surfaces

        /// <summary>
        /// Pixel format can be used for rendering on windows.
        /// </summary>
        public bool RenderWindow;

        /// <summary>
        /// Pixel format can be used for rendering on memory buffers.
        /// </summary>
        public bool RenderBuffer;

        /// <summary>
        /// Pixel format can be used for rendering on pixel buffer objects.
        /// </summary>
        public bool RenderPBuffer;

        #endregion

        #region Double Buffering

        /// <summary>
        /// Pixel format support double buffering.
        /// </summary>
        public bool DoubleBuffer;

        /// <summary>
        /// Method used for swapping back buffers (WGL only).
        /// </summary>
        /// <remarks>
        /// It can assume the values Wgl.SWAP_EXCHANGE, SWAP_COPY, or SWAP_UNDEFINED in the case DoubleBuffer is false.
        /// </remarks>
        public int SwapMethod;

        /// <summary>
        /// Pixel format support stereo buffering.
        /// </summary>
        public bool StereoBuffer;

        #endregion

        #region Buffers bits

        /// <summary>
        /// Color bits (without alpha).
        /// </summary>
        public int ColorBits;

        /// <summary>
        /// Red bits.
        /// </summary>
        public int RedBits;

        /// <summary>
        /// Green bits.
        /// </summary>
        public int GreenBits;

        /// <summary>
        /// Blue bits.
        /// </summary>
        public int BlueBits;

        /// <summary>
        /// Alpha bits.
        /// </summary>
        public int AlphaBits;

        /// <summary>
        /// Depth buffer bits.
        /// </summary>
        public int DepthBits;

        /// <summary>
        /// Stencil buffer bits.
        /// </summary>
        public int StencilBits;

        /// <summary>
        /// Multisample bits.
        /// </summary>
        public int MultisampleBits;

        /// <summary>
        /// sRGB conversion capability.
        /// </summary>
        public bool SRGBCapable;

        #endregion

        /// <summary>
        /// Copy this DevicePixelFormat
        /// </summary>
        /// <returns>
        /// It returns a <see cref="DevicePixelFormat"/> equals to this DevicePixelFormat.
        /// </returns>
        public DevicePixelFormat Copy()
        {
            return (DevicePixelFormat)MemberwiseClone();
        }

        /// <summary>
        /// Represent this object with a String.
        /// </summary>
        /// <returns>
        /// Guess it.
        /// </returns>
        public override string ToString()
        {
            StringBuilder pixelType = new StringBuilder();

            if (RgbaUnsigned) pixelType.Append("U");
            if (RgbaFloat) pixelType.Append("F");
            if (SRGBCapable) pixelType.Append("s");

            StringBuilder surfaceType = new StringBuilder();

            if (RenderWindow) surfaceType.Append("W");
            if (RenderBuffer) surfaceType.Append("B");
            if (RenderPBuffer) surfaceType.Append("P");

            return $"Idx={FormatIndex} Pixel={pixelType} Color={ColorBits} Depth={DepthBits} Stencil={StencilBits} Ms={MultisampleBits} DB={DoubleBuffer}, Surface={surfaceType}";
        }
    }

    public class DevicePixelFormatCollection : List<DevicePixelFormat>
    {
        /// <summary>
        /// Choose a <see cref="DevicePixelFormat"/>
        /// </summary>
        /// <param name="pixelFormat">
        /// A <see cref="DevicePixelFormat"/> that specify the minimum requirements
        /// </param>
        /// <returns></returns>
        public List<DevicePixelFormat> Choose(DevicePixelFormat pixelFormat)
        {
            if (pixelFormat == null)
                throw new ArgumentNullException(nameof(pixelFormat));

            List<DevicePixelFormat> pixelFormats = new List<DevicePixelFormat>(this);

            pixelFormats.RemoveAll(delegate (DevicePixelFormat item) {

                if (pixelFormat.RgbaUnsigned != item.RgbaUnsigned)
                    return true;
                if (pixelFormat.RgbaFloat != item.RgbaFloat)
                    return true;

                if (pixelFormat.RenderWindow && !item.RenderWindow)
                    return true;
                if (pixelFormat.RenderPBuffer && !item.RenderPBuffer)
                    return true;
                if (pixelFormat.RenderBuffer && !item.RenderBuffer)
                    return true;

                if (item.ColorBits < pixelFormat.ColorBits)
                    return true;
                if (item.RedBits < pixelFormat.RedBits)
                    return true;
                if (item.GreenBits < pixelFormat.GreenBits)
                    return true;
                if (item.BlueBits < pixelFormat.BlueBits)
                    return true;
                if (item.AlphaBits < pixelFormat.AlphaBits)
                    return true;

                if (item.DepthBits < pixelFormat.DepthBits)
                    return true;
                if (item.StencilBits < pixelFormat.StencilBits)
                    return true;
                if (item.MultisampleBits < pixelFormat.MultisampleBits)
                    return true;

                if (pixelFormat.DoubleBuffer && !item.DoubleBuffer)
                    return true;

                if (pixelFormat.SRGBCapable && !item.SRGBCapable)
                    return true;

                return false;
            });

            List<DevicePixelFormat> pixelFormatsCopy = pixelFormats.Select(devicePixelFormat => devicePixelFormat.Copy()).ToList();

            // Sort (ascending by resource occupation)
            pixelFormatsCopy.Sort(delegate (DevicePixelFormat x, DevicePixelFormat y) {
                int compare;

                if ((compare = x.ColorBits.CompareTo(y.ColorBits)) != 0)
                    return compare;
                if ((compare = x.DepthBits.CompareTo(y.DepthBits)) != 0)
                    return compare;
                if ((compare = x.StencilBits.CompareTo(y.StencilBits)) != 0)
                    return compare;
                if ((compare = x.MultisampleBits.CompareTo(y.MultisampleBits)) != 0)
                    return compare;

                if ((compare = y.DoubleBuffer.CompareTo(x.DoubleBuffer)) != 0)
                    return compare;

                return 0;
            });

            return pixelFormatsCopy;
        }

        /// <summary>
        /// Try to guess why <see cref="Choose(DevicePixelFormat)"/> is not returning any pixel format.
        /// </summary>
        /// <param name="pixelFormat">
        /// A <see cref="DevicePixelFormat"/> that specify the minimum requirements
        /// </param>
        /// <returns>
        /// It returns a string indicating the actual reason behind a failure in pixel format selection using <paramref name="pixelFormat"/>.
        /// </returns>
        public string GuessChooseError(DevicePixelFormat pixelFormat)
        {
            if (pixelFormat == null)
                throw new ArgumentNullException(nameof(pixelFormat));

            List<DevicePixelFormat> pixelFormats = new List<DevicePixelFormat>(this);

            pixelFormats.RemoveAll(delegate (DevicePixelFormat item) {
                if (pixelFormat.RgbaUnsigned != item.RgbaUnsigned)
                    return true;
                if (pixelFormat.RgbaFloat != item.RgbaFloat)
                    return true;

                return false;
            });
            if (pixelFormats.Count == 0)
                return $"no RGBA pixel type matching (RGBAui={pixelFormat.RgbaUnsigned}, RGBAf={pixelFormat.RgbaFloat})";

            pixelFormats.RemoveAll(delegate (DevicePixelFormat item) {
                if (pixelFormat.RenderWindow && !item.RenderWindow)
                    return true;
                if (pixelFormat.RenderPBuffer && !item.RenderPBuffer)
                    return true;
                if (pixelFormat.RenderBuffer && !item.RenderBuffer)
                    return true;

                return false;
            });

            if (pixelFormats.Count == 0)
                return
                    $"no surface matching (Window={pixelFormat.RenderWindow}, PBuffer={pixelFormat.RenderPBuffer}, RenderBuffer={pixelFormat.RenderBuffer})";

            pixelFormats.RemoveAll(delegate (DevicePixelFormat item) {
                if (item.ColorBits < pixelFormat.ColorBits)
                    return true;
                if (item.RedBits < pixelFormat.RedBits)
                    return true;
                if (item.GreenBits < pixelFormat.GreenBits)
                    return true;
                if (item.BlueBits < pixelFormat.BlueBits)
                    return true;
                if (item.AlphaBits < pixelFormat.AlphaBits)
                    return true;

                return false;
            });

            if (pixelFormats.Count == 0)
                return
                    $"no color bits combination matching ({pixelFormat.ColorBits} bits, {{{pixelFormat.RedBits}|{pixelFormat.BlueBits}|{pixelFormat.GreenBits}|{pixelFormat.AlphaBits}}})";

            pixelFormats.RemoveAll(item => item.DepthBits < pixelFormat.DepthBits);
            if (pixelFormats.Count == 0)
                return $"no depth bits matching (Depth >= {pixelFormat.DepthBits})";

            pixelFormats.RemoveAll(item => item.StencilBits < pixelFormat.StencilBits);
            if (pixelFormats.Count == 0)
                return $"no stencil bits matching (Bits >= {pixelFormat.StencilBits})";

            pixelFormats.RemoveAll(item => item.MultisampleBits < pixelFormat.MultisampleBits);
            if (pixelFormats.Count == 0)
                return $"no multisample bits matching (Samples >= {pixelFormat.MultisampleBits})";

            pixelFormats.RemoveAll(item => pixelFormat.DoubleBuffer && !item.DoubleBuffer);
            if (pixelFormats.Count == 0)
                return $"no double-buffer matching (DB={pixelFormat.DoubleBuffer})";

            pixelFormats.RemoveAll(item => pixelFormat.SRGBCapable && !item.SRGBCapable);
            if (pixelFormats.Count == 0)
                return $"no sRGB matching (sRGB={pixelFormat.SRGBCapable})";

            return "no error";
        }

        /// <summary>
        /// Copy this DevicePixelFormatCollection.
        /// </summary>
        /// <returns>
        /// It returns a <see cref="DevicePixelFormatCollection"/> equivalent to this DevicePixelFormatCollection.
        /// </returns>
        public DevicePixelFormatCollection Copy()
        {
            DevicePixelFormatCollection pixelFormats = new DevicePixelFormatCollection();

            pixelFormats.AddRange(this.Select(devicePixelFormat => devicePixelFormat.Copy()));

            return pixelFormats;
        }
    }
}