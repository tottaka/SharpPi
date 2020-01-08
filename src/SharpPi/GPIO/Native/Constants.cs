namespace SharpPi.Native
{
    public abstract partial class NativeMethods
    {
        public abstract partial class Library
        {
            /// <summary>
            /// Default: libbcm2835.so
            /// </summary>
            public const string GPIO = "libbcm2835.so";
        }
    }
}
