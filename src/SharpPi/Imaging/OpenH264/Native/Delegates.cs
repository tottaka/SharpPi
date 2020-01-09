using System;
using System.Runtime.InteropServices;

namespace SharpPi.Native
{
    public abstract partial class NativeMethods
    {
        public abstract partial class H264
        {
            public static WelsCreateDecoderFunc CreateDecoderFunc { get; private set; }
            public static WelsDestroyDecoderFunc DestroyDecoderFunc { get; private set; }
            public static InitializeDecoderFunc DecoderInitializeFunc { get; private set; }

            public delegate int WelsCreateDecoderFunc([In, Out]ref ISVCDecoder ppDecoder);
            public delegate void WelsDestroyDecoderFunc([In]ISVCDecoder ppDecoder);
            public delegate int InitializeDecoderFunc(ref SDecodingParam pParam);

            private static bool DecoderInitialized = false;
            private static DynamicLibrary hLib;

            public static void LoadDecoder()
            {
                if (!DecoderInitialized)
                {
                    // Load DLL, this needs to be static
                    hLib = DynamicLibrary.Load(Library.OpenH264, DynamicLibrary.FLAGS_RTLD_NOW);

                    CreateDecoderFunc = Marshal.GetDelegateForFunctionPointer<WelsCreateDecoderFunc>(hLib.GetFunctionLocation("WelsCreateDecoder"));
                    if (CreateDecoderFunc == null)
                        throw new DllNotFoundException("Unable to load WelsCreateDecoder function.");

                    DestroyDecoderFunc = Marshal.GetDelegateForFunctionPointer<WelsDestroyDecoderFunc>(hLib.GetFunctionLocation("WelsDestroyDecoder"));
                    if (DestroyDecoderFunc == null)
                        throw new DllNotFoundException("Unable to load WelsDestroyDecoder function.");

                    /*
                    DecoderInitializeFunc = Marshal.GetDelegateForFunctionPointer<InitializeDecoderFunc>();
                    if (DecoderInitializeFunc == null)
                        throw new DllNotFoundException("Unable to load WelsDestroyDecoder function.");
                    */
                    DecoderInitialized = true;
                }
            }
        }
    }
}
