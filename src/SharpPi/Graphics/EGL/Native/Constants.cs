namespace SharpPi.Native
{
    public abstract partial class NativeMethods
    {
        public abstract partial class Library
        {
            /// <summary>
            /// Default: /opt/vc/lib/libbrcmEGL.so
            /// </summary>
            public const string EGL = "/opt/vc/lib/libbrcmEGL.so";
        }

        public abstract partial class Egl
        {
            // EGL Enumerants. Bitmasks and other exceptional cases aside, most enums are assigned unique values starting at 0x3000.
            /// <summary>
            /// [EGL] Value of EGL_DEFAULT_DISPLAY symbol.
            /// </summary>
            public const int DEFAULT_DISPLAY = 0;

            /// <summary>
            /// [EGL] Value of EGL_MULTISAMPLE_RESOLVE_BOX_BIT symbol.
            /// </summary>
            public const int MULTISAMPLE_RESOLVE_BOX_BIT = 0x0200;

            /// <summary>
            /// <para>
            /// [EGL] Egl.QuerySurface: Returns the filter used when resolving the multisample buffer. The filter may be either 
            /// Egl.MULTISAMPLE_RESOLVE_DEFAULT or Egl.MULTISAMPLE_RESOLVE_BOX, as described for Egl.SurfaceAttrib.
            /// </para>
            /// <para>
            /// [EGL] Egl.SurfaceAttrib: Specifies the filter to use when resolving the multisample buffer (this may occur when swapping 
            /// or copying the surface, or when changing the client API context bound to the surface). A value of 
            /// Egl.MULTISAMPLE_RESOLVE_DEFAULT chooses the default implementation-defined filtering method, while 
            /// Egl.MULTISAMPLE_RESOLVE_BOX chooses a one-pixel wide box filter placing equal weighting on all multisample values. The 
            /// initial value of Egl.MULTISAMPLE_RESOLVE is Egl.MULTISAMPLE_RESOLVE_DEFAULT.
            /// </para>
            /// </summary>
            public const int MULTISAMPLE_RESOLVE = 0x3099;

            /// <summary>
            /// [EGL] Value of EGL_MULTISAMPLE_RESOLVE_DEFAULT symbol.
            /// </summary>
            public const int MULTISAMPLE_RESOLVE_DEFAULT = 0x309A;

            /// <summary>
            /// [EGL] Value of EGL_MULTISAMPLE_RESOLVE_BOX symbol.
            /// </summary>
            public const int MULTISAMPLE_RESOLVE_BOX = 0x309B;

            /// <summary>
            /// [EGL] Value of EGL_OPENGL_API symbol.
            /// </summary>
            public const int OPENGL_API = 0x30A2;

            /// <summary>
            /// [EGL] Value of EGL_OPENGL_BIT symbol.
            /// </summary>
            public const int OPENGL_BIT = 0x0008;

            /// <summary>
            /// [EGL] Value of EGL_SWAP_BEHAVIOR_PRESERVED_BIT symbol.
            /// </summary>
            public const int SWAP_BEHAVIOR_PRESERVED_BIT = 0x0400;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by a bitmask indicating which types of client API contexts created with respect 
            /// to the frame buffer configuration config must pass the required conformance tests for that API. Mask bits include: 
            /// Egl.OPENGL_BIT Config supports creating OpenGL contexts. Egl.OPENGL_ES_BIT Config supports creating OpenGL ES 1.0 and/or 
            /// 1.1 contexts. Egl.OPENGL_ES2_BIT Config supports creating OpenGL ES 2.0 contexts. Egl.OPENVG_BIT Config supports 
            /// creating OpenVG contexts. For example, if the bitmask is set to Egl.OPENGL_ES_BIT, only frame buffer configurations that 
            /// support creating conformant OpenGL ES contexts will match. The default value is zero. Most EGLConfigs should be 
            /// conformant for all supported client APIs, and it is rarely desirable to select a nonconformant config. Conformance 
            /// requirements limit the number of non-conformant configs that an implementation can define.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns a bitmask indicating which client API contexts created with respect to this config 
            /// are conformant.
            /// </para>
            /// </summary>
            public const int CONFORMANT = 0x3042;

            /// <summary>
            /// <para>
            /// [EGL] Egl.CreateContext: Must be followed by an integer that determines which version of an OpenGL ES context to create. 
            /// A value of 1 specifies creation of an OpenGL ES 1.x context. An attribute value of 2 specifies creation of an OpenGL ES 
            /// 2.x context. The default value is 1. This attribute can only be specified when creating a OpenGL ES context (e.g. when 
            /// the current rendering API is Egl.OPENGL_ES_API).
            /// </para>
            /// <para>
            /// [EGL] Egl.QueryContext: Returns the version of the client API which the context supports, as specified at context 
            /// creation time. The resulting value is only meaningful for an OpenGL ES context.
            /// </para>
            /// </summary>
            public const int CONTEXT_CLIENT_VERSION = 0x3098;

            /// <summary>
            /// [EGL] Egl.ChooseConfig: Must be followed by the handle of a valid native pixmap, cast to EGLint, or Egl.NONE. If the 
            /// value is not Egl.NONE, only configs which support creating pixmap surfaces with this pixmap using 
            /// Egl.CreatePixmapSurface will match this attribute. If the value is Egl.NONE, then configs are not matched for this 
            /// attribute. The default value is Egl.NONE. Egl.MATCH_NATIVE_PIXMAP was introduced due to the difficulty of determining an 
            /// EGLConfig compatibile with a native pixmap using only color component sizes.
            /// </summary>
            public const int MATCH_NATIVE_PIXMAP = 0x3041;

            /// <summary>
            /// [EGL] Value of EGL_OPENGL_ES2_BIT symbol.
            /// </summary>
            public const int OPENGL_ES2_BIT = 0x0004;

            /// <summary>
            /// <para>
            /// [EGL] Egl.CreatePbufferSurface: Specifies how alpha values are interpreted by OpenVG when rendering to the surface. If 
            /// its value is Egl.VG_ALPHA_FORMAT_NONPRE, then alpha values are not premultipled. If its value is 
            /// Egl.VG_ALPHA_FORMAT_PRE, then alpha values are premultiplied. The default value of Egl.VG_ALPHA_FORMAT is 
            /// Egl.VG_ALPHA_FORMAT_NONPRE.
            /// </para>
            /// <para>
            /// [EGL] Egl.CreatePixmapSurface: Specifies how alpha values are interpreted by OpenVG when rendering to the surface. If 
            /// its value is Egl.VG_ALPHA_FORMAT_NONPRE, then alpha values are not premultipled. If its value is 
            /// Egl.VG_ALPHA_FORMAT_PRE, then alpha values are premultiplied. The default value of Egl.VG_ALPHA_FORMAT is 
            /// Egl.VG_ALPHA_FORMAT_NONPRE.
            /// </para>
            /// <para>
            /// [EGL] Egl.CreateWindowSurface: Specifies how alpha values are interpreted by OpenVG when rendering to the surface. If 
            /// its value is Egl.VG_ALPHA_FORMAT_NONPRE, then alpha values are not premultipled. If its value is 
            /// Egl.VG_ALPHA_FORMAT_PRE, then alpha values are premultiplied. The default value of Egl.VG_ALPHA_FORMAT is 
            /// Egl.VG_ALPHA_FORMAT_NONPRE.
            /// </para>
            /// </summary>
            public const int VG_ALPHA_FORMAT = 0x3088;

            /// <summary>
            /// [EGL] Value of EGL_VG_ALPHA_FORMAT_NONPRE symbol.
            /// </summary>
            public const int VG_ALPHA_FORMAT_NONPRE = 0x308B;

            /// <summary>
            /// [EGL] Value of EGL_VG_ALPHA_FORMAT_PRE symbol.
            /// </summary>
            public const int VG_ALPHA_FORMAT_PRE = 0x308C;

            /// <summary>
            /// [EGL] Value of EGL_VG_ALPHA_FORMAT_PRE_BIT symbol.
            /// </summary>
            public const int VG_ALPHA_FORMAT_PRE_BIT = 0x0040;

            /// <summary>
            /// <para>
            /// [EGL] Egl.CreatePbufferSurface: Specifies the color space used by OpenVG when rendering to the surface. If its value is 
            /// Egl.VG_COLORSPACE_sRGB, then a non-linear, perceptually uniform color space is assumed, with a corresponding 
            /// VGImageFormat of form Egl.*. If its value is Egl.VG_COLORSPACE_LINEAR, then a linear color space is assumed, with a 
            /// corresponding VGImageFormat of form Egl.*. The default value of Egl.VG_COLORSPACE is Egl.VG_COLORSPACE_sRGB.
            /// </para>
            /// <para>
            /// [EGL] Egl.CreatePixmapSurface: Specifies the color space used by OpenVG when rendering to the surface. If its value is 
            /// Egl.VG_COLORSPACE_sRGB, then a non-linear, perceptually uniform color space is assumed, with a corresponding 
            /// VGImageFormat of form Egl.*. If its value is Egl.VG_COLORSPACE_LINEAR, then a linear color space is assumed, with a 
            /// corresponding VGImageFormat of form Egl.*. The default value of Egl.VG_COLORSPACE is Egl.VG_COLORSPACE_sRGB.
            /// </para>
            /// <para>
            /// [EGL] Egl.CreateWindowSurface: Specifies the color space used by OpenVG when rendering to the surface. If its value is 
            /// Egl.VG_COLORSPACE_sRGB, then a non-linear, perceptually uniform color space is assumed, with a corresponding 
            /// VGImageFormat of form Egl.*. If its value is Egl.VG_COLORSPACE_LINEAR, then a linear color space is assumed, with a 
            /// corresponding VGImageFormat of form Egl.*. The default value of Egl.VG_COLORSPACE is Egl.VG_COLORSPACE_sRGB.
            /// </para>
            /// </summary>
            public const int VG_COLORSPACE = 0x3087;

            /// <summary>
            /// [EGL] Value of EGL_VG_COLORSPACE_LINEAR_BIT symbol.
            /// </summary>
            public const int VG_COLORSPACE_LINEAR_BIT = 0x0020;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by a nonnegative integer that indicates the desired alpha mask buffer size, in 
            /// bits. The smallest alpha mask buffers of at least the specified size are preferred. The default value is zero. The alpha 
            /// mask buffer is used only by OpenGL and OpenGL ES client APIs.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the number of bits in the alpha mask buffer.
            /// </para>
            /// </summary>
            public const int ALPHA_MASK_SIZE = 0x303E;

            /// <summary>
            /// [EGL] Value of EGL_BUFFER_PRESERVED symbol.
            /// </summary>
            public const int BUFFER_PRESERVED = 0x3094;

            /// <summary>
            /// [EGL] Value of EGL_BUFFER_DESTROYED symbol.
            /// </summary>
            public const int BUFFER_DESTROYED = 0x3095;

            /// <summary>
            /// [EGL] Egl.QueryString: Returns a string describing which client rendering APIs are supported. The string contains a 
            /// space-separate list of API names. The list must include at least one of OpenGL, OpenGL_ES, or OpenVG. These strings 
            /// correspond respectively to values Egl.OPENGL_API, Egl.OPENGL_ES_API, and Egl.OPENVG_API of the Egl.BindAPI, api 
            /// argument.
            /// </summary>
            public const int CLIENT_APIS = 0x308D;

            /// <summary>
            /// [EGL] Value of EGL_COLORSPACE_sRGB symbol.
            /// </summary>
            public const int COLORSPACE_sRGB = 0x3089;

            /// <summary>
            /// [EGL] Value of EGL_COLORSPACE_LINEAR symbol.
            /// </summary>
            public const int COLORSPACE_LINEAR = 0x308A;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by one of Egl.RGB_BUFFER or Egl.LUMINANCE_BUFFER. Egl.RGB_BUFFER indicates an 
            /// RGB color buffer; in this case, attributes Egl.RED_SIZE, Egl.GREEN_SIZE and Egl.BLUE_SIZE must be non-zero, and 
            /// Egl.LUMINANCE_SIZE must be zero. Egl.LUMINANCE_BUFFER indicates a luminance color buffer. In this case Egl.RED_SIZE, 
            /// Egl.GREEN_SIZE, Egl.BLUE_SIZE must be zero, and Egl.LUMINANCE_SIZE must be non-zero. For both RGB and luminance color 
            /// buffers, Egl.ALPHA_SIZE may be zero or non-zero.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the color buffer type. Possible types are Egl.RGB_BUFFER and Egl.LUMINANCE_BUFFER.
            /// </para>
            /// </summary>
            public const int COLOR_BUFFER_TYPE = 0x303F;

            /// <summary>
            /// [EGL] Egl.QueryContext: Returns the type of client API which the context supports (one of Egl.OPENGL_API, 
            /// Egl.OPENGL_ES_API, or Egl.OPENVG_API).
            /// </summary>
            public const int CONTEXT_CLIENT_TYPE = 0x3097;

            /// <summary>
            /// [EGL] Value of EGL_DISPLAY_SCALING symbol.
            /// </summary>
            public const int DISPLAY_SCALING = 10000;

            /// <summary>
            /// [EGL] Egl.QuerySurface: Returns the horizontal dot pitch of the display on which a window surface is visible. The value 
            /// returned is equal to the actual dot pitch, in pixels/meter, multiplied by the constant value Egl.DISPLAY_SCALING.
            /// </summary>
            public const int HORIZONTAL_RESOLUTION = 0x3090;

            /// <summary>
            /// [EGL] Value of EGL_LUMINANCE_BUFFER symbol.
            /// </summary>
            public const int LUMINANCE_BUFFER = 0x308F;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by a nonnegative integer that indicates the desired size of the luminance 
            /// component of the color buffer, in bits. If this value is zero, color buffers with the smallest luminance component size 
            /// are preferred. Otherwise, color buffers with the largest luminance component of at least the specified size are 
            /// preferred. The default value is zero.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the number of bits of luminance stored in the luminance buffer.
            /// </para>
            /// </summary>
            public const int LUMINANCE_SIZE = 0x303D;

            /// <summary>
            /// [EGL] Value of EGL_OPENGL_ES_BIT symbol.
            /// </summary>
            public const int OPENGL_ES_BIT = 0x0001;

            /// <summary>
            /// [EGL] Value of EGL_OPENVG_BIT symbol.
            /// </summary>
            public const int OPENVG_BIT = 0x0002;

            /// <summary>
            /// [EGL] Value of EGL_OPENGL_ES_API symbol.
            /// </summary>
            public const int OPENGL_ES_API = 0x30A0;

            /// <summary>
            /// [EGL] Value of EGL_OPENVG_API symbol.
            /// </summary>
            public const int OPENVG_API = 0x30A1;

            /// <summary>
            /// [EGL] Value of EGL_OPENVG_IMAGE symbol.
            /// </summary>
            public const int OPENVG_IMAGE = 0x3096;

            /// <summary>
            /// [EGL] Egl.QuerySurface: Returns the aspect ratio of an individual pixel (the ratio of a pixel's width to its height). 
            /// The value returned is equal to the actual aspect ratio multiplied by the constant value Egl.DISPLAY_SCALING.
            /// </summary>
            public const int PIXEL_ASPECT_RATIO = 0x3092;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by a bitmask indicating which types of client API contexts the frame buffer 
            /// configuration must support creating with Egl.CreateContext). Mask bits are the same as for attribute Egl.CONFORMANT. The 
            /// default value is Egl.OPENGL_ES_BIT.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns a bitmask indicating the types of supported client API contexts.
            /// </para>
            /// </summary>
            public const int RENDERABLE_TYPE = 0x3040;

            /// <summary>
            /// <para>
            /// [EGL] Egl.CreateWindowSurface: Specifies which buffer should be used for client API rendering to the window. If its 
            /// value is Egl.SINGLE_BUFFER, then client APIs should render directly into the visible window. If its value is 
            /// Egl.BACK_BUFFER, then client APIs should render into the back buffer. The default value of Egl.RENDER_BUFFER is 
            /// Egl.BACK_BUFFER. Client APIs may not be able to respect the requested rendering buffer. To determine the actual buffer 
            /// being rendered to by a context, call Egl.QueryContext.
            /// </para>
            /// <para>
            /// [EGL] Egl.QueryContext: Returns the buffer which client API rendering via the context will use. The value returned 
            /// depends on properties of both the context, and the surface to which the context is bound: If the context is bound to a 
            /// pixmap surface, then Egl.SINGLE_BUFFER will be returned. If the context is bound to a pbuffer surface, then 
            /// Egl.BACK_BUFFER will be returned. If the context is bound to a window surface, then either Egl.BACK_BUFFER or 
            /// Egl.SINGLE_BUFFER may be returned. The value returned depends on both the buffer requested by the setting of the 
            /// Egl.RENDER_BUFFER property of the surface (which may be queried by calling eglQuerySurface), and on the client API (not 
            /// all client APIs support single-buffer rendering to window surfaces). If the context is not bound to a surface, such as 
            /// an OpenGL ES context bound to a framebuffer object, then Egl.NONE will be returned.
            /// </para>
            /// <para>
            /// [EGL] Egl.QuerySurface: Returns the buffer which client API rendering is requested to use. For a window surface, this is 
            /// the same attribute value specified when the surface was created. For a pbuffer surface, it is always Egl.BACK_BUFFER. 
            /// For a pixmap surface, it is always Egl.SINGLE_BUFFER. To determine the actual buffer being rendered to by a context, 
            /// call Egl.QueryContext.
            /// </para>
            /// </summary>
            public const int RENDER_BUFFER = 0x3086;

            /// <summary>
            /// [EGL] Value of EGL_RGB_BUFFER symbol.
            /// </summary>
            public const int RGB_BUFFER = 0x308E;

            /// <summary>
            /// [EGL] Value of EGL_SINGLE_BUFFER symbol.
            /// </summary>
            public const int SINGLE_BUFFER = 0x3085;

            /// <summary>
            /// <para>
            /// [EGL] Egl.QuerySurface: Returns the effect on the color buffer when posting a surface with Egl.SwapBuffers. Swap 
            /// behavior may be either Egl.BUFFER_PRESERVED or Egl.BUFFER_DESTROYED, as described for Egl.SurfaceAttrib.
            /// </para>
            /// <para>
            /// [EGL] Egl.SurfaceAttrib: Specifies the effect on the color buffer of posting a surface with Egl.SwapBuffers. A value of 
            /// Egl.BUFFER_PRESERVED indicates that color buffer contents are unaffected, while Egl.BUFFER_DESTROYED indicates that 
            /// color buffer contents may be destroyed or changed by the operation. The initial value of Egl.SWAP_BEHAVIOR is chosen by 
            /// the implementation.
            /// </para>
            /// </summary>
            public const int SWAP_BEHAVIOR = 0x3093;

            /// <summary>
            /// [EGL] Value of EGL_UNKNOWN symbol.
            /// </summary>
            public const int UNKNOWN = -1;

            /// <summary>
            /// [EGL] Egl.QuerySurface: Returns the vertical dot pitch of the display on which a window surface is visible. The value 
            /// returned is equal to the actual dot pitch, in pixels/meter, multiplied by the constant value Egl.DISPLAY_SCALING.
            /// </summary>
            public const int VERTICAL_RESOLUTION = 0x3091;

            /// <summary>
            /// [EGL] Value of EGL_BACK_BUFFER symbol.
            /// </summary>
            public const int BACK_BUFFER = 0x3084;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by Egl.DONT_CARE, Egl.TRUE, or Egl.FALSE. If Egl.TRUE is specified, then only 
            /// frame buffer configurations that support binding of color buffers to an OpenGL ES RGB texture will be considered. 
            /// Currently only frame buffer configurations that support pbuffers allow this. The default value is Egl.DONT_CARE.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns Egl.TRUE if color buffers can be bound to an RGB texture, Egl.FALSE otherwise.
            /// </para>
            /// </summary>
            public const int BIND_TO_TEXTURE_RGB = 0x3039;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by one of Egl.DONT_CARE, Egl.TRUE, or Egl.FALSE. If Egl.TRUE is specified, then 
            /// only frame buffer configurations that support binding of color buffers to an OpenGL ES RGBA texture will be considered. 
            /// Currently only frame buffer configurations that support pbuffers allow this. The default value is Egl.DONT_CARE.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns Egl.TRUE if color buffers can be bound to an RGBA texture, Egl.FALSE otherwise.
            /// </para>
            /// </summary>
            public const int BIND_TO_TEXTURE_RGBA = 0x303A;

            /// <summary>
            /// [EGL] Egl.GetError: A power management event has occurred. The application must destroy all contexts and reinitialise 
            /// OpenGL ES state and objects to continue rendering.
            /// </summary>
            public const int CONTEXT_LOST = 0x300E;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by a integer that indicates the minimum value that can be passed to 
            /// eglSwapInterval. The default value is Egl.DONT_CARE.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the minimum value that can be passed to eglSwapInterval.
            /// </para>
            /// </summary>
            public const int MIN_SWAP_INTERVAL = 0x303B;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by a integer that indicates the maximum value that can be passed to 
            /// Egl.SwapInterval. The default value is Egl.DONT_CARE.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the maximum value that can be passed to eglSwapInterval.
            /// </para>
            /// </summary>
            public const int MAX_SWAP_INTERVAL = 0x303C;

            /// <summary>
            /// <para>
            /// [EGL] Egl.CreatePbufferFromClientBuffer: Specifies whether storage for mipmaps should be allocated. Space for mipmaps 
            /// will be set aside if the attribute value is Egl.TRUE and Egl.TEXTURE_FORMAT is not Egl.NO_TEXTURE. The default value is 
            /// Egl.FALSE.
            /// </para>
            /// <para>
            /// [EGL] Egl.CreatePbufferSurface: Specifies whether storage for mipmaps should be allocated. Space for mipmaps will be set 
            /// aside if the attribute value is Egl.TRUE and Egl.TEXTURE_FORMAT is not Egl.NO_TEXTURE. The default value is Egl.FALSE.
            /// </para>
            /// <para>
            /// [EGL] Egl.QuerySurface: Returns Egl.TRUE if texture has mipmaps, Egl.FALSE otherwise.
            /// </para>
            /// </summary>
            public const int MIPMAP_TEXTURE = 0x3082;

            /// <summary>
            /// <para>
            /// [EGL] Egl.QuerySurface: Returns which level of the mipmap to render to, if texture has mipmaps.
            /// </para>
            /// <para>
            /// [EGL] Egl.SurfaceAttrib: For mipmap textures, the Egl.MIPMAP_LEVEL attribute indicates which level of the mipmap should 
            /// be rendered. If the value of this attribute is outside the range of supported mipmap levels, the closest valid mipmap 
            /// level is selected for rendering. The default value is Egl..
            /// </para>
            /// </summary>
            public const int MIPMAP_LEVEL = 0x3083;

            /// <summary>
            /// [EGL] Value of EGL_NO_TEXTURE symbol.
            /// </summary>
            public const int NO_TEXTURE = 0x305C;

            /// <summary>
            /// [EGL] Value of EGL_TEXTURE_2D symbol.
            /// </summary>
            public const int TEXTURE_2D = 0x305F;

            /// <summary>
            /// <para>
            /// [EGL] Egl.CreatePbufferFromClientBuffer: Specifies the format of the texture that will be created when a pbuffer is 
            /// bound to a texture map. Possible values are Egl.NO_TEXTURE, Egl.TEXTURE_RGB, and Egl.TEXTURE_RGBA. The default value is 
            /// Egl.NO_TEXTURE.
            /// </para>
            /// <para>
            /// [EGL] Egl.CreatePbufferSurface: Specifies the format of the texture that will be created when a pbuffer is bound to a 
            /// texture map. Possible values are Egl.NO_TEXTURE, Egl.TEXTURE_RGB, and Egl.TEXTURE_RGBA. The default value is 
            /// Egl.NO_TEXTURE.
            /// </para>
            /// <para>
            /// [EGL] Egl.QuerySurface: Returns format of texture. Possible values are Egl.NO_TEXTURE, Egl.TEXTURE_RGB, and 
            /// Egl.TEXTURE_RGBA.
            /// </para>
            /// </summary>
            public const int TEXTURE_FORMAT = 0x3080;

            /// <summary>
            /// [EGL] Value of EGL_TEXTURE_RGB symbol.
            /// </summary>
            public const int TEXTURE_RGB = 0x305D;

            /// <summary>
            /// [EGL] Value of EGL_TEXTURE_RGBA symbol.
            /// </summary>
            public const int TEXTURE_RGBA = 0x305E;

            /// <summary>
            /// <para>
            /// [EGL] Egl.CreatePbufferFromClientBuffer: Specifies the target for the texture that will be created when the pbuffer is 
            /// created with a texture format of Egl.TEXTURE_RGB or Egl.TEXTURE_RGBA. Possible values are Egl.NO_TEXTURE, or 
            /// Egl.TEXTURE_2D. The default value is Egl.NO_TEXTURE.
            /// </para>
            /// <para>
            /// [EGL] Egl.CreatePbufferSurface: Specifies the target for the texture that will be created when the pbuffer is created 
            /// with a texture format of Egl.TEXTURE_RGB or Egl.TEXTURE_RGBA. Possible values are Egl.NO_TEXTURE, or Egl.TEXTURE_2D. The 
            /// default value is Egl.NO_TEXTURE.
            /// </para>
            /// <para>
            /// [EGL] Egl.QuerySurface: Returns type of texture. Possible values are Egl.NO_TEXTURE, or Egl.TEXTURE_2D.
            /// </para>
            /// </summary>
            public const int TEXTURE_TARGET = 0x3081;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by a nonnegative integer that indicates the desired size of the alpha component 
            /// of the color buffer, in bits. If this value is zero, color buffers with the smallest alpha component size are preferred. 
            /// Otherwise, color buffers with the largest alpha component of at least the specified size are preferred. The default 
            /// value is zero.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the number of bits of alpha stored in the color buffer.
            /// </para>
            /// </summary>
            public const int ALPHA_SIZE = 0x3021;

            /// <summary>
            /// [EGL] Egl.GetError: EGL cannot access a requested resource (for example a context is bound in another thread).
            /// </summary>
            public const int BAD_ACCESS = 0x3002;

            /// <summary>
            /// [EGL] Egl.GetError: EGL failed to allocate resources for the requested operation.
            /// </summary>
            public const int BAD_ALLOC = 0x3003;

            /// <summary>
            /// [EGL] Egl.GetError: An unrecognized attribute or attribute value was passed in the attribute list.
            /// </summary>
            public const int BAD_ATTRIBUTE = 0x3004;

            /// <summary>
            /// [EGL] Egl.GetError: An EGLConfig argument does not name a valid EGL frame buffer configuration.
            /// </summary>
            public const int BAD_CONFIG = 0x3005;

            /// <summary>
            /// [EGL] Egl.GetError: An EGLContext argument does not name a valid EGL rendering context.
            /// </summary>
            public const int BAD_CONTEXT = 0x3006;

            /// <summary>
            /// [EGL] Egl.GetError: The current surface of the calling thread is a window, pixel buffer or pixmap that is no longer 
            /// valid.
            /// </summary>
            public const int BAD_CURRENT_SURFACE = 0x3007;

            /// <summary>
            /// [EGL] Egl.GetError: An EGLDisplay argument does not name a valid EGL display connection.
            /// </summary>
            public const int BAD_DISPLAY = 0x3008;

            /// <summary>
            /// [EGL] Egl.GetError: Arguments are inconsistent (for example, a valid context requires buffers not supplied by a valid 
            /// surface).
            /// </summary>
            public const int BAD_MATCH = 0x3009;

            /// <summary>
            /// [EGL] Egl.GetError: A NativePixmapType argument does not refer to a valid native pixmap.
            /// </summary>
            public const int BAD_NATIVE_PIXMAP = 0x300A;

            /// <summary>
            /// [EGL] Egl.GetError: A NativeWindowType argument does not refer to a valid native window.
            /// </summary>
            public const int BAD_NATIVE_WINDOW = 0x300B;

            /// <summary>
            /// [EGL] Egl.GetError: One or more argument values are invalid.
            /// </summary>
            public const int BAD_PARAMETER = 0x300C;

            /// <summary>
            /// [EGL] Egl.GetError: An EGLSurface argument does not name a valid surface (window, pixel buffer or pixmap) configured for 
            /// GL rendering.
            /// </summary>
            public const int BAD_SURFACE = 0x300D;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by a nonnegative integer that indicates the desired size of the blue component 
            /// of the color buffer, in bits. If this value is zero, color buffers with the smallest blue component size are preferred. 
            /// Otherwise, color buffers with the largest blue component of at least the specified size are preferred. The default value 
            /// is zero.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the number of bits of blue stored in the color buffer.
            /// </para>
            /// </summary>
            public const int BLUE_SIZE = 0x3022;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by a nonnegative integer that indicates the desired color buffer size, in bits. 
            /// The smallest color buffers of at least the specified size are preferred. The default value is zero. The color buffer 
            /// size is the sum of Egl.RED_SIZE, Egl.GREEN_SIZE, Egl.BLUE_SIZE, and Egl.ALPHA_SIZE, and does not include any padding 
            /// bits which may be present in the pixel format. It is usually preferable to specify desired sizes for these color 
            /// components individually.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the depth of the color buffer. It is the sum of Egl.RED_SIZE, Egl.GREEN_SIZE, 
            /// Egl.BLUE_SIZE, and Egl.ALPHA_SIZE.
            /// </para>
            /// </summary>
            public const int BUFFER_SIZE = 0x3020;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by Egl.DONT_CARE, Egl.NONE, Egl.SLOW_CONFIG, or Egl.NON_CONFORMANT_CONFIG. If 
            /// Egl.DONT_CARE is specified, then configs are not matched for this attribute. The default value is Egl.DONT_CARE. If 
            /// Egl.NONE is specified, then configs are matched for this attribute, but only configs with no caveats (neither 
            /// Egl.SLOW_CONFIG or Egl.NON_CONFORMANT_CONFIG) will be considered. If Egl.SLOW_CONFIG is specified, then only slow 
            /// configs configurations will be considered. The meaning of``slow'' is implementation-dependent, but typically indicates a 
            /// non-hardware-accelerated (software) implementation. If Egl.NON_CONFORMANT_CONFIG is specified, then only configs 
            /// supporting non-conformant OpenGL ES contexts will be considered. If the EGL version is 1.3 or later, caveat 
            /// Egl.NON_CONFORMANT_CONFIG is obsolete, since the same information can be specified via the Egl.CONFORMANT attribute on a 
            /// per-client-API basis, not just for OpenGL ES.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the caveats for the frame buffer configuration. Possible caveat values are Egl.NONE, 
            /// Egl.SLOW_CONFIG, and Egl.NON_CONFORMANT.
            /// </para>
            /// </summary>
            public const int CONFIG_CAVEAT = 0x3027;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by a valid integer ID that indicates the desired EGL frame buffer 
            /// configuration. When a Egl.CONFIG_ID is specified, all other attributes are ignored. The default value is Egl.DONT_CARE. 
            /// The meaning of config IDs is implementation-dependent. They are used only to uniquely identify different frame buffer 
            /// configurations.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the ID of the frame buffer configuration.
            /// </para>
            /// <para>
            /// [EGL] Egl.QueryContext: Returns the ID of the EGL frame buffer configuration with respect to which the context was 
            /// created.
            /// </para>
            /// <para>
            /// [EGL] Egl.QuerySurface: Returns the ID of the EGL frame buffer configuration with respect to which the surface was 
            /// created.
            /// </para>
            /// </summary>
            public const int CONFIG_ID = 0x3028;

            /// <summary>
            /// [EGL] Value of EGL_CORE_NATIVE_ENGINE symbol.
            /// </summary>
            public const int CORE_NATIVE_ENGINE = 0x305B;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by a nonnegative integer that indicates the desired depth buffer size, in bits. 
            /// The smallest depth buffers of at least the specified size is preferred. If the desired size is zero, frame buffer 
            /// configurations with no depth buffer are preferred. The default value is zero. The depth buffer is used only by OpenGL 
            /// and OpenGL ES client APIs.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the number of bits in the depth buffer.
            /// </para>
            /// </summary>
            public const int DEPTH_SIZE = 0x3025;

            /// <summary>
            /// [EGL] Value of EGL_DONT_CARE symbol.
            /// </summary>
            public const int DONT_CARE = -1;

            /// <summary>
            /// [EGL] Value of EGL_DRAW symbol.
            /// </summary>
            public const int DRAW = 0x3059;

            /// <summary>
            /// [EGL] Egl.QueryString: Returns a space separated list of supported extensions to EGL.
            /// </summary>
            public const int EXTENSIONS = 0x3055;

            /// <summary>
            /// [EGL] Value of EGL_FALSE symbol.
            /// </summary>
            public const int FALSE = 0;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by a nonnegative integer that indicates the desired size of the green component 
            /// of the color buffer, in bits. If this value is zero, color buffers with the smallest green component size are preferred. 
            /// Otherwise, color buffers with the largest green component of at least the specified size are preferred. The default 
            /// value is zero.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the number of bits of green stored in the color buffer.
            /// </para>
            /// </summary>
            public const int GREEN_SIZE = 0x3023;

            /// <summary>
            /// <para>
            /// [EGL] Egl.CreatePbufferSurface: Specifies the required height of the pixel buffer surface. The default value is Egl..
            /// </para>
            /// <para>
            /// [EGL] Egl.QuerySurface: Returns the height of the surface in pixels.
            /// </para>
            /// </summary>
            public const int HEIGHT = 0x3056;

            /// <summary>
            /// <para>
            /// [EGL] Egl.CreatePbufferSurface: Requests the largest available pixel buffer surface when the allocation would otherwise 
            /// fail. Use Egl.QuerySurface to retrieve the dimensions of the allocated pixel buffer. The default value is Egl.FALSE.
            /// </para>
            /// <para>
            /// [EGL] Egl.QuerySurface: Returns the same attribute value specified when the surface was created with 
            /// Egl.CreatePbufferSurface. For a window or pixmap surface, value is not modified.
            /// </para>
            /// </summary>
            public const int LARGEST_PBUFFER = 0x3058;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by an integer buffer level specification. This specification is honored 
            /// exactly. Buffer level zero corresponds to the default frame buffer of the display. Buffer level one is the first overlay 
            /// frame buffer, level two the second overlay frame buffer, and so on. Negative buffer levels correspond to underlay frame 
            /// buffers. The default value is zero. Most imlementations do not support overlay or underlay planes (buffer levels other 
            /// than zero).
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the frame buffer level. Level zero is the default frame buffer. Positive levels 
            /// correspond to frame buffers that overlay the default buffer and negative levels correspond to frame buffers that 
            /// underlay the default buffer.
            /// </para>
            /// </summary>
            public const int LEVEL = 0x3029;

            /// <summary>
            /// [EGL] Egl.GetConfigAttrib: Returns the maximum height of a pixel buffer surface in pixels.
            /// </summary>
            public const int MAX_PBUFFER_HEIGHT = 0x302A;

            /// <summary>
            /// [EGL] Egl.GetConfigAttrib: Returns the maximum size of a pixel buffer surface in pixels.
            /// </summary>
            public const int MAX_PBUFFER_PIXELS = 0x302B;

            /// <summary>
            /// [EGL] Egl.GetConfigAttrib: Returns the maximum width of a pixel buffer surface in pixels.
            /// </summary>
            public const int MAX_PBUFFER_WIDTH = 0x302C;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by Egl.DONT_CARE, Egl.TRUE, or Egl.FALSE. If Egl.TRUE is specified, then only 
            /// frame buffer configurations that allow native rendering into the surface will be considered. The default value is 
            /// Egl.DONT_CARE.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns Egl.TRUE if native rendering APIs can render into the surface, Egl.FALSE otherwise.
            /// </para>
            /// </summary>
            public const int NATIVE_RENDERABLE = 0x302D;

            /// <summary>
            /// [EGL] Egl.GetConfigAttrib: Returns the ID of the associated native visual.
            /// </summary>
            public const int NATIVE_VISUAL_ID = 0x302E;

            /// <summary>
            /// [EGL] Egl.GetConfigAttrib: Returns the type of the associated native visual.
            /// </summary>
            public const int NATIVE_VISUAL_TYPE = 0x302F;

            /// <summary>
            /// [EGL] Value of EGL_NONE symbol.
            /// </summary>
            public const int NONE = 0x3038;

            /// <summary>
            /// [EGL] Value of EGL_NON_CONFORMANT_CONFIG symbol.
            /// </summary>
            public const int NON_CONFORMANT_CONFIG = 0x3051;

            /// <summary>
            /// [EGL] Egl.GetError: EGL is not initialized, or could not be initialized, for the specified EGL display connection.
            /// </summary>
            public const int NOT_INITIALIZED = 0x3001;

            /// <summary>
            /// [EGL] Value of EGL_NO_CONTEXT symbol.
            /// </summary>
            public const int NO_CONTEXT = 0;

            /// <summary>
            /// [EGL] Value of EGL_NO_DISPLAY symbol.
            /// </summary>
            public const int NO_DISPLAY = 0;

            /// <summary>
            /// [EGL] Value of EGL_NO_SURFACE symbol.
            /// </summary>
            public const int NO_SURFACE = 0;

            /// <summary>
            /// [EGL] Value of EGL_PBUFFER_BIT symbol.
            /// </summary>
            public const int PBUFFER_BIT = 0x0001;

            /// <summary>
            /// [EGL] Value of EGL_PIXMAP_BIT symbol.
            /// </summary>
            public const int PIXMAP_BIT = 0x0002;

            /// <summary>
            /// [EGL] Value of EGL_READ symbol.
            /// </summary>
            public const int READ = 0x305A;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by a nonnegative integer that indicates the desired size of the red component 
            /// of the color buffer, in bits. If this value is zero, color buffers with the smallest red component size are preferred. 
            /// Otherwise, color buffers with the largest red component of at least the specified size are preferred. The default value 
            /// is zero.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the number of bits of red stored in the color buffer.
            /// </para>
            /// </summary>
            public const int RED_SIZE = 0x3024;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by the minimum number of samples required in multisample buffers. 
            /// Configurations with the smallest number of samples that meet or exceed the specified minimum number are preferred. Note 
            /// that it is possible for color samples in the multisample buffer to have fewer bits than colors in the main color 
            /// buffers. However, multisampled colors maintain at least as much color resolution in aggregate as the main color buffers.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the number of samples per pixel.
            /// </para>
            /// </summary>
            public const int SAMPLES = 0x3031;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by the minimum acceptable number of multisample buffers. Configurations with 
            /// the smallest number of multisample buffers that meet or exceed this minimum number are preferred. Currently operation 
            /// with more than one multisample buffer is undefined, so only values of zero or one will produce a match. The default 
            /// value is zero.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the number of multisample buffers.
            /// </para>
            /// </summary>
            public const int SAMPLE_BUFFERS = 0x3032;

            /// <summary>
            /// [EGL] Value of EGL_SLOW_CONFIG symbol.
            /// </summary>
            public const int SLOW_CONFIG = 0x3050;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by a nonnegative integer that indicates the desired stencil buffer size, in 
            /// bits. The smallest stencil buffers of at least the specified size are preferred. If the desired size is zero, frame 
            /// buffer configurations with no stencil buffer are preferred. The default value is zero. The stencil buffer is used only 
            /// by OpenGL and OpenGL ES client APIs.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the number of bits in the stencil buffer.
            /// </para>
            /// </summary>
            public const int STENCIL_SIZE = 0x3026;

            /// <summary>
            /// [EGL] Egl.GetError: The last function succeeded without error.
            /// </summary>
            public const int SUCCESS = 0x3000;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by a bitmask indicating which EGL surface types and capabilities the frame 
            /// buffer configuration must support. Mask bits include: Egl.MULTISAMPLE_RESOLVE_BOX_BIT Config allows specifying box 
            /// filtered multisample resolve behavior with Egl.SurfaceAttrib. Egl.PBUFFER_BIT Config supports creating pixel buffer 
            /// surfaces. Egl.PIXMAP_BIT Config supports creating pixmap surfaces. Egl.SWAP_BEHAVIOR_PRESERVED_BIT Config allows setting 
            /// swap behavior for color buffers with Egl.SurfaceAttrib. Egl.VG_ALPHA_FORMAT_PRE_BIT Config allows specifying OpenVG 
            /// rendering with premultiplied alpha values at surface creation time (see Egl.CreatePbufferSurface, 
            /// Egl.CreatePixmapSurface, and Egl.CreateWindowSurface). Egl.VG_COLORSPACE_LINEAR_BIT Config allows specifying OpenVG 
            /// rendering in a linear colorspace at surface creation time (see Egl.CreatePbufferSurface, Egl.CreatePixmapSurface, and 
            /// Egl.CreateWindowSurface). Egl.WINDOW_BIT Config supports creating window surfaces. For example, if the bitmask is set to 
            /// Egl.WINDOW_BIT | Egl.PIXMAP_BIT, only frame buffer configurations that support both windows and pixmaps will be 
            /// considered. The default value is Egl.WINDOW_BIT.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns a bitmask indicating the types of supported EGL surfaces.
            /// </para>
            /// </summary>
            public const int SURFACE_TYPE = 0x3033;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by an integer value indicating the transparent blue value. The value must be 
            /// between zero and the maximum color buffer value for blue. Only frame buffer configurations that use the specified 
            /// transparent blue value will be considered. The default value is Egl.DONT_CARE. This attribute is ignored unless 
            /// Egl.TRANSPARENT_TYPE is included in attrib_list and specified as Egl.TRANSPARENT_RGB.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the transparent blue value.
            /// </para>
            /// </summary>
            public const int TRANSPARENT_BLUE_VALUE = 0x3035;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by an integer value indicating the transparent green value. The value must be 
            /// between zero and the maximum color buffer value for green. Only frame buffer configurations that use the specified 
            /// transparent green value will be considered. The default value is Egl.DONT_CARE. This attribute is ignored unless 
            /// Egl.TRANSPARENT_TYPE is included in attrib_list and specified as Egl.TRANSPARENT_RGB.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the transparent green value.
            /// </para>
            /// </summary>
            public const int TRANSPARENT_GREEN_VALUE = 0x3036;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by an integer value indicating the transparent red value. The value must be 
            /// between zero and the maximum color buffer value for red. Only frame buffer configurations that use the specified 
            /// transparent red value will be considered. The default value is Egl.DONT_CARE. This attribute is ignored unless 
            /// Egl.TRANSPARENT_TYPE is included in attrib_list and specified as Egl.TRANSPARENT_RGB.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the transparent red value.
            /// </para>
            /// </summary>
            public const int TRANSPARENT_RED_VALUE = 0x3037;

            /// <summary>
            /// [EGL] Value of EGL_TRANSPARENT_RGB symbol.
            /// </summary>
            public const int TRANSPARENT_RGB = 0x3052;

            /// <summary>
            /// <para>
            /// [EGL] Egl.ChooseConfig: Must be followed by one of Egl.NONE or Egl.TRANSPARENT_RGB. If Egl.NONE is specified, then only 
            /// opaque frame buffer configurations will be considered. If Egl.TRANSPARENT_RGB is specified, then only transparent frame 
            /// buffer configurations will be considered. The default value is Egl.NONE. Most implementations support only opaque frame 
            /// buffer configurations.
            /// </para>
            /// <para>
            /// [EGL] Egl.GetConfigAttrib: Returns the type of supported transparency. Possible transparency values are: Egl.NONE, and 
            /// Egl.TRANSPARENT_RGB.
            /// </para>
            /// </summary>
            public const int TRANSPARENT_TYPE = 0x3034;

            /// <summary>
            /// [EGL] Value of EGL_TRUE symbol.
            /// </summary>
            public const int TRUE = 1;

            /// <summary>
            /// [EGL] Egl.QueryString: Returns the company responsible for this EGL implementation. This name does not change from 
            /// release to release.
            /// </summary>

            public const int VENDOR = 0x3053;

            /// <summary>
            /// [EGL] Egl.QueryString: Returns a version or release number. The Egl.VERSION string is laid out as 
            /// follows:major_version.minor_version space vendor_specific_info
            /// </summary>
            public const int VERSION = 0x3054;

            /// <summary>
            /// <para>
            /// [EGL] Egl.CreatePbufferSurface: Specifies the required width of the pixel buffer surface. The default value is Egl..
            /// </para>
            /// <para>
            /// [EGL] Egl.QuerySurface: Returns the width of the surface in pixels.
            /// </para>
            /// </summary>
            public const int WIDTH = 0x3057;

            /// <summary>
            /// [EGL] Value of EGL_WINDOW_BIT symbol.
            /// </summary>
            public const int WINDOW_BIT = 0x0004;
        }
    }
}
