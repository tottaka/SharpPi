using System;
using System.Runtime.InteropServices;

namespace SharpPi.Native
{
    public abstract partial class NativeMethods
    {
        public abstract partial class VideoCore
        {
            /// <summary>
            /// The processor ID.
            /// </summary>
            [DllImport(Library.BcmHost)]
            public static extern int bcm_host_get_processor_id();

            /// <summary>
            /// Returns 1 if kms is active (dtoverlay=v3d-kms-vc4)
            /// </summary>
            [DllImport(Library.BcmHost)]
            public static extern int bcm_host_is_kms_active();

            /// <summary>
            /// Returns 1 if fkms is active (dtoverlay=v3d-fkms-vc4)
            /// </summary>
            [DllImport(Library.BcmHost)]
            public static extern int bcm_host_is_fkms_active();

            /// <summary>
            /// Returns 1 if model is Pi4
            /// </summary>
            [DllImport(Library.BcmHost)]
            public static extern int bcm_host_is_model_pi4();

            /// <summary>
            /// Gets the model of the Raspberry Pi board being used.
            /// </summary>
            [DllImport(Library.BcmHost)]
            public static extern int bcm_host_get_model_type();
        }
    }
}
