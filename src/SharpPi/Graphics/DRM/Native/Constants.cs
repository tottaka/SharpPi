using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPi.Native
{
    public abstract partial class NativeMethods
    {
        public abstract partial class Library
        {
            /// <summary>
            /// Default: /usr/local/lib/arm-linux-gnueabihf/libdrm.so
            /// </summary>
            public const string DRM = "/usr/local/lib/arm-linux-gnueabihf/libdrm.so";
        }

        public abstract partial class Drm
        {
            public const int DRM_PROP_NAME_LEN = 32;
            public const int DRM_CONNECTOR_NAME_LEN = 32;
            public const int DRM_DISPLAY_MODE_LEN = 32;


            public enum drmModeConnection
            {
                DRM_MODE_CONNECTED = 1,
                DRM_MODE_DISCONNECTED = 2,
                DRM_MODE_UNKNOWNCONNECTION = 3
            }

            public enum drmModeSubPixel
            {
                DRM_MODE_SUBPIXEL_UNKNOWN = 1,
                DRM_MODE_SUBPIXEL_HORIZONTAL_RGB = 2,
                DRM_MODE_SUBPIXEL_HORIZONTAL_BGR = 3,
                DRM_MODE_SUBPIXEL_VERTICAL_RGB = 4,
                DRM_MODE_SUBPIXEL_VERTICAL_BGR = 5,
                DRM_MODE_SUBPIXEL_NONE = 6
            }

        }
    }
}
