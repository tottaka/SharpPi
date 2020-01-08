using System;
using System.Runtime.InteropServices;

namespace SharpPi.Native
{
    // EGL Types, EGLint is defined in eglplatform.h
    using EGLBoolean = System.Boolean;
    using EGLenum = System.UInt32;
    using EGLConfig = System.IntPtr;
    using EGLDisplay = System.IntPtr;
    using EGLSurface = System.IntPtr;
    using EGLClientBuffer = System.IntPtr;

    public abstract partial class NativeMethods
    {
        /// <summary>
        /// EGL_VERSION_1_4
        /// </summary>
        public abstract partial class Egl
        {
            /// <summary>
            /// [EGL] eglGetCurrentContext: return the current EGL rendering context
            /// </summary>
            [DllImport(Library.EGL, EntryPoint = "eglGetCurrentContext")]
            public static extern IntPtr GetCurrentContext();

            /// <summary>
            /// [EGL] eglBindAPI: Set the current rendering API
            /// </summary>
            /// <param name="api">
            /// Specifies the client API to bind, one of Egl.OPENGL_API, Egl.OPENGL_ES_API, or Egl.OPENVG_API.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglBindAPI")]
            public static extern EGLBoolean BindAPI(uint api);

            /// <summary>
            /// [EGL] eglQueryAPI: Query the current rendering API
            /// </summary>
            [DllImport(Library.EGL, EntryPoint = "eglQueryAPI")]
            public static extern uint QueryAPI();

            /// <summary>
            /// [EGL] eglCreatePbufferFromClientBuffer: create a new EGL pixel buffer surface bound to an OpenVG image
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="buftype">
            /// Specifies the type of client API buffer to be bound. Must be Egl.OPENVG_IMAGE, corresponding to an OpenVG VGImage 
            /// buffer.
            /// </param>
            /// <param name="buffer">
            /// Specifies the OpenVG VGImage handle of the buffer to be bound.
            /// </param>
            /// <param name="config">
            /// Specifies the EGL frame buffer configuration that defines the frame buffer resource available to the surface.
            /// </param>
            /// <param name="attrib_list">
            /// Specifies pixel buffer surface attributes. May be Egl. or empty (first attribute is Egl.NONE).
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglCreatePbufferFromClientBuffer")]
            public static extern IntPtr CreatePbufferFromClientBuffer(EGLDisplay display, uint buftype, IntPtr buffer, IntPtr config, int[] attrib_list);

            /// <summary>
            /// [EGL] eglReleaseThread: Release EGL per-thread state
            /// </summary>
            [DllImport(Library.EGL, EntryPoint = "eglReleaseThread")]
            public static extern EGLBoolean ReleaseThread();

            /// <summary>
            /// [EGL] eglWaitClient: Complete client API execution prior to subsequent native rendering calls
            /// </summary>
            [DllImport(Library.EGL, EntryPoint = "eglWaitClient")]
            public static extern EGLBoolean WaitClient();

            /// <summary>
            /// [EGL] eglBindTexImage: Defines a two-dimensional texture image
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="surface">
            /// Specifies the EGL surface.
            /// </param>
            /// <param name="buffer">
            /// Specifies the texture image data.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglBindTexImage")]
            public static extern EGLBoolean BindTexImage(EGLDisplay display, IntPtr surface, int buffer);

            /// <summary>
            /// [EGL] eglReleaseTexImage: Releases a color buffer that is being used as a texture
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="surface">
            /// Specifies the EGL surface.
            /// </param>
            /// <param name="buffer">
            /// Specifies the texture image data.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglReleaseTexImage")]
            public static extern EGLBoolean ReleaseTexImage(EGLDisplay display, IntPtr surface, int buffer);

            /// <summary>
            /// [EGL] eglSurfaceAttrib: set an EGL surface attribute
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="surface">
            /// Specifies the EGL surface.
            /// </param>
            /// <param name="attribute">
            /// Specifies the EGL surface attribute to set.
            /// </param>
            /// <param name="value">
            /// Specifies the attributes required value.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglSurfaceAttrib")]
            public static extern EGLBoolean SurfaceAttrib(EGLDisplay display, IntPtr surface, int attribute, int value);

            /// <summary>
            /// [EGL] eglSwapInterval: specifies the minimum number of video frame periods per buffer swap for the window associated 
            /// with the current context.
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="interval">
            /// Specifies the minimum number of video frames that are displayed before a buffer swap will occur.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglSwapInterval")]
            public static extern EGLBoolean SwapInterval(EGLDisplay display, int interval);

            /// <summary>
            /// [EGL] eglChooseConfig: return a list of EGL frame buffer configurations that match specified attributes
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="attrib_list">
            /// Specifies attributes required to match by configs.
            /// </param>
            /// <param name="configs">
            /// Returns an array of frame buffer configurations.
            /// </param>
            /// <param name="config_size">
            /// Specifies the size of the array of frame buffer configurations.
            /// </param>
            /// <param name="num_config">
            /// Returns the number of frame buffer configurations returned.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglChooseConfig")]
            public static extern EGLBoolean ChooseConfig(EGLDisplay display, int[] attrib_list, IntPtr[] configs, int config_size, int[] num_config);

            /// <summary>
            /// [EGL] eglCopyBuffers: copy EGL surface color buffer to a native pixmap
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="surface">
            /// Specifies the EGL surface whose color buffer is to be copied.
            /// </param>
            /// <param name="target">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglCopyBuffers")]
            public static extern EGLBoolean CopyBuffers(EGLDisplay display, IntPtr surface, IntPtr target);

            /// <summary>
            /// [EGL] eglCreateContext: create a new EGL rendering context
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="config">
            /// Specifies the EGL frame buffer configuration that defines the frame buffer resource available to the rendering context.
            /// </param>
            /// <param name="share_context">
            /// Specifies another EGL rendering context with which to share data, as defined by the client API corresponding to the 
            /// contexts. Data is also shared with all other contexts with which <paramref name="share_context"/> shares data. 
            /// Egl.NO_CONTEXT indicates that no sharing is to take place.
            /// </param>
            /// <param name="attrib_list">
            /// Specifies attributes and attribute values for the context being created. Only the attribute Egl.CONTEXT_CLIENT_VERSION 
            /// may be specified.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglCreateContext")]
            public static extern IntPtr CreateContext(EGLDisplay display, IntPtr config, IntPtr share_context, int[] attrib_list);

            /// <summary>
            /// [EGL] eglCreatePbufferSurface: create a new EGL pixel buffer surface
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="config">
            /// Specifies the EGL frame buffer configuration that defines the frame buffer resource available to the surface.
            /// </param>
            /// <param name="attrib_list">
            /// Specifies pixel buffer surface attributes. May be Egl. or empty (first attribute is Egl.NONE).
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglCreatePbufferSurface")]
            public static extern EGLSurface CreatePbufferSurface(EGLDisplay display, IntPtr config, int[] attrib_list);

            /// <summary>
            /// [EGL] eglCreatePixmapSurface: create a new EGL pixmap surface
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="config">
            /// Specifies the EGL frame buffer configuration that defines the frame buffer resource available to the surface.
            /// </param>
            /// <param name="pixmap">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="attrib_list">
            /// Specifies pixmap surface attributes. May be Egl. or empty (first attribute is Egl.NONE).
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglCreatePixmapSurface")]
            public static extern EGLSurface CreatePixmapSurface(EGLDisplay display, IntPtr config, IntPtr pixmap, int[] attrib_list);

            /// <summary>
            /// [EGL] eglCreateWindowSurface: create a new EGL window surface
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="config">
            /// Specifies the EGL frame buffer configuration that defines the frame buffer resource available to the surface.
            /// </param>
            /// <param name="win">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="attrib_list">
            /// Specifies window surface attributes. May be Egl. or empty (first attribute is Egl.NONE).
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglCreateWindowSurface")]
            public static extern EGLSurface CreateWindowSurface(EGLDisplay display, IntPtr config, IntPtr win, int[] attrib_list);

            /// <summary>
            /// [EGL] eglDestroyContext: destroy an EGL rendering context
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="ctx">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglDestroyContext")]
            public static extern EGLBoolean DestroyContext(EGLDisplay display, IntPtr ctx);

            /// <summary>
            /// [EGL] eglDestroySurface: destroy an EGL surface
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="surface">
            /// Specifies the EGL surface to be destroyed.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglDestroySurface")]
            public static extern EGLBoolean DestroySurface(EGLDisplay display, IntPtr surface);

            /// <summary>
            /// [EGL] eglGetConfigAttrib: return information about an EGL frame buffer configuration
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="config">
            /// Specifies the EGL frame buffer configuration to be queried.
            /// </param>
            /// <param name="attribute">
            /// Specifies the EGL rendering context attribute to be returned.
            /// </param>
            /// <param name="value">
            /// Returns the requested value.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglGetConfigAttrib")]
            public static extern EGLBoolean GetConfigAttrib(EGLDisplay display, IntPtr config, int attribute, [Out] int[] value);

            /// <summary>
            /// [EGL] eglGetConfigAttrib: return information about an EGL frame buffer configuration
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="config">
            /// Specifies the EGL frame buffer configuration to be queried.
            /// </param>
            /// <param name="attribute">
            /// Specifies the EGL rendering context attribute to be returned.
            /// </param>
            /// <param name="value">
            /// Returns the requested value.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglGetConfigAttrib")]
            public static extern EGLBoolean GetConfigAttrib(EGLDisplay display, EGLConfig config, int attribute, out int value);

            /// <summary>
            /// [EGL] eglGetConfigs: return a list of all EGL frame buffer configurations for a display
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="configs">
            /// Returns a list of configs.
            /// </param>
            /// <param name="config_size">
            /// Specifies the size of the list of configs.
            /// </param>
            /// <param name="num_config">
            /// Returns the number of configs returned.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglGetConfigs")]
            public static extern EGLBoolean GetConfigs(EGLDisplay display, [Out] EGLConfig[] configs, int config_size, [Out] int[] num_config);

            /// <summary>
            /// [EGL] eglGetConfigs: return a list of all EGL frame buffer configurations for a display
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="configs">
            /// Returns a list of configs.
            /// </param>
            /// <param name="config_size">
            /// Specifies the size of the list of configs.
            /// </param>
            /// <param name="num_config">
            /// Returns the number of configs returned.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglGetConfigs")]
            public static extern EGLBoolean GetConfigs(EGLDisplay display, [Out] EGLConfig[] configs, int config_size, out int num_config);

            /// <summary>
            /// [EGL] eglGetCurrentDisplay: return the display for the current EGL rendering context
            /// </summary>
            [DllImport(Library.EGL, EntryPoint = "eglGetCurrentDisplay")]
            public static extern EGLDisplay GetCurrentDisplay();

            /// <summary>
            /// [EGL] eglGetCurrentSurface: return the read or draw surface for the current EGL rendering context
            /// </summary>
            /// <param name="readdraw">
            /// Specifies whether the EGL read or draw surface is to be returned.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglGetCurrentSurface")]
            public static extern EGLSurface GetCurrentSurface(int readdraw);

            /// <summary>
            /// [EGL] eglGetDisplay: return an EGL display connection
            /// </summary>
            /// <param name="display_id">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglGetDisplay")]
            public static extern EGLDisplay GetDisplay(EGLDisplay display_id);

            /// <summary>
            /// [EGL] eglGetError: return error information
            /// </summary>
            [DllImport(Library.EGL, EntryPoint = "eglGetError")]
            public static extern int GetError();

            /// <summary>
            /// [EGL] eglGetProcAddress: return a GL or an EGL extension function
            /// </summary>
            /// <param name="procname">
            /// Specifies the name of the function to return.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglGetProcAddress")]
            public static extern IntPtr GetProcAddress(string procname);

            /// <summary>
            /// [EGL] eglInitialize: initialize an EGL display connection
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="major">
            /// Returns the major version number of the EGL implementation. May be Egl..
            /// </param>
            /// <param name="minor">
            /// Returns the minor version number of the EGL implementation. May be Egl..
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglInitialize")]
            public static extern EGLBoolean Initialize(EGLDisplay dpy, ref int major, ref int minor);

            /// <summary>
            /// [EGL] eglMakeCurrent: attach an EGL rendering context to EGL surfaces
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="draw">
            /// Specifies the EGL draw surface.
            /// </param>
            /// <param name="read">
            /// Specifies the EGL read surface.
            /// </param>
            /// <param name="ctx">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglMakeCurrent")]
            public static extern EGLBoolean MakeCurrent(EGLDisplay display, IntPtr draw, IntPtr read, IntPtr context);

            /// <summary>
            /// [EGL] eglQueryContext: return EGL rendering context information
            /// </summary>
            /// <param name="display">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="context">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="attribute">
            /// Specifies the EGL rendering context attribute to be returned.
            /// </param>
            /// <param name="value">
            /// Returns the requested value.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglQueryContext")]
            public static extern EGLBoolean QueryContext(EGLDisplay display, IntPtr context, int attribute, ref int value);

            /// <summary>
            /// [EGL] eglQueryString: return a string describing an EGL display connection
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="name">
            /// Specifies a symbolic constant, one of Egl.CLIENT_APIS, Egl.VENDOR, Egl.VERSION, or Egl.EXTENSIONS.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglQueryString")]
            public static extern string QueryString(EGLDisplay display, int name);

            /// <summary>
            /// [EGL] eglQuerySurface: return EGL surface information
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="surface">
            /// Specifies the EGL surface to query.
            /// </param>
            /// <param name="attribute">
            /// Specifies the EGL surface attribute to be returned.
            /// </param>
            /// <param name="value">
            /// Returns the requested value.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglQuerySurface")]
            public static extern EGLBoolean QuerySurface(EGLDisplay display, EGLSurface surface, int attribute, int[] value);

            /// <summary>
            /// [EGL] eglSwapBuffers: post EGL surface color buffer to a native window
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            /// <param name="surface">
            /// Specifies the EGL drawing surface whose buffers are to be swapped.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglSwapBuffers")]
            public static extern EGLBoolean SwapBuffers(EGLDisplay display, EGLSurface surface);

            /// <summary>
            /// [EGL] eglTerminate: terminate an EGL display connection
            /// </summary>
            /// <param name="dpy">
            /// A <see cref="T:IntPtr"/>.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglTerminate")]
            public static extern EGLBoolean Terminate(EGLDisplay display);

            /// <summary>
            /// [EGL] eglWaitGL: Complete GL execution prior to subsequent native rendering calls
            /// </summary>
            [DllImport(Library.EGL, EntryPoint = "eglWaitGL")]
            public static extern EGLBoolean WaitGL();

            /// <summary>
            /// [EGL] eglWaitNative: complete native execution prior to subsequent GL rendering calls
            /// </summary>
            /// <param name="engine">
            /// Specifies a particular marking engine to be waited on. Must be Egl.CORE_NATIVE_ENGINE.
            /// </param>
            [DllImport(Library.EGL, EntryPoint = "eglWaitNative")]
            public static extern EGLBoolean WaitNative(int engine);
        }
    }
}
