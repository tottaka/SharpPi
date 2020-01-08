using System;

namespace SharpPi.Native
{
    using DISPMANX_DISPLAY_HANDLE_T = System.UInt32;
    using DISPMANX_ELEMENT_HANDLE_T = System.UInt32;
    using DISPMANX_PROTECTION_T = System.UInt32;
    using DISPMANX_RESOURCE_HANDLE_T = System.UInt32;
    using DISPMANX_UPDATE_HANDLE_T = System.UInt32;

    public abstract partial class NativeMethods
    {
        public abstract partial class Library
        {
            /// <summary>
            /// Default: /opt/vc/lib/libbcm_host.so
            /// </summary>
            public const string VideoCore = "/opt/vc/lib/libbcm_host.so";
        }

        public abstract partial class VideoCore
        {
            public delegate void DISPMANX_CALLBACK_FUNC_T(DISPMANX_UPDATE_HANDLE_T u, IntPtr arg);
        }
    }
}
