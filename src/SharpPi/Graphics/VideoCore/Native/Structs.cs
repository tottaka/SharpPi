using System;
using System.Runtime.InteropServices;

namespace SharpPi.Native
{
    using DISPMANX_DISPLAY_HANDLE_T = System.UInt32;
    using DISPMANX_ELEMENT_HANDLE_T = System.UInt32;
    using DISPMANX_PROTECTION_T = System.UInt32;
    using DISPMANX_RESOURCE_HANDLE_T = System.UInt32;
    using DISPMANX_UPDATE_HANDLE_T = System.UInt32;

    public abstract partial class NativeMethods
    {
        public abstract partial class VideoCore
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct VC_RECT_T
            {
                public VC_RECT_T(int x, int y, int w, int h)
                {
                    this.x = x;
                    this.y = y;
                    this.width = w;
                    this.height = h;
                }

                public Int32 x;

                public Int32 y;

                public Int32 width;

                public Int32 height;

                public override string ToString() => string.Format("VC_RECT_T={{ x={0} y={1} w={2} h={3} }}", x, y, width, height);
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct DISPMANX_MODEINFO_T
            {
                Int32 width;
                Int32 height;
                DISPMANX_TRANSFORM_T transform;
                DISPLAY_INPUT_FORMAT_T input_format;
                UInt32 display_num;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct VC_DISPMANX_ALPHA_T
            {
                DISPMANX_FLAGS_ALPHA_T flags;
                UInt32 opacity;
                DISPMANX_RESOURCE_HANDLE_T mask;
            }

            [StructLayout(LayoutKind.Explicit)]
            public struct DISPMANX_CLAMP_KEYS_T
            {
                [FieldOffset(0)]
                Byte yuv_yy_upper;
                [FieldOffset(1)]
                Byte yuv_yy_lower;
                [FieldOffset(2)]
                Byte yuv_cr_upper;
                [FieldOffset(3)]
                Byte yuv_cr_lower;
                [FieldOffset(4)]
                Byte yuv_cb_upper;
                [FieldOffset(5)]
                Byte yuv_cb_lower;

                [FieldOffset(0)]
                Byte rgb_red_upper;
                [FieldOffset(1)]
                Byte rgb_red_lower;
                [FieldOffset(2)]
                Byte rgb_blue_upper;
                [FieldOffset(3)]
                Byte rgb_blue_lower;
                [FieldOffset(4)]
                Byte rgb_green_upper;
                [FieldOffset(5)]
                Byte rgb_green_lower;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct DISPMANX_CLAMP_T
            {
                DISPMANX_FLAGS_CLAMP_T mode;
                DISPMANX_FLAGS_KEYMASK_T key_mask;
                DISPMANX_CLAMP_KEYS_T key_value;
                UInt32 replace_value;
            }

            /// <summary>
            /// The structure expected by eglCreateWindowSurface function.
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public struct EGL_DISPMANX_WINDOW_T
            {
                public UInt32 element;
                public int width;
                public int height;
            }
        }
    }
}
