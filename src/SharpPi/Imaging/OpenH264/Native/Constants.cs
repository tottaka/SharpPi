namespace SharpPi.Native
{
    public abstract partial class NativeMethods
    {
        /// <summary>
        /// Contains static dependency library locations.
        /// </summary>
        public abstract partial class Library
        {
            /// <summary>
            /// The path to Cisco's OpenH264 library file.
            /// Default: /home/pi/scripts/libopenh264.so
            /// </summary>
            public const string OpenH264 = "/home/pi/scripts/libopenh264.so";
        }

        public abstract partial class H264
        {
            /// <summary>
            /// UCHAR_MAX
            /// </summary>
            public const int UCHAR_MAX = 0xff;
        }
    }
}
