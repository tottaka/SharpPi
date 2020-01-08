using System;
using System.Runtime.InteropServices;

namespace SharpPi.Native
{
    using VCHI_INSTANCE_T = System.IntPtr;
    using VCHI_MEM_HANDLE_T = System.Int32;

    /// <summary>
    /// Generic native methods
    /// https://docs.microsoft.com/en-us/dotnet/framework/interop/marshaling-classes-structures-and-unions
    /// https://limbioliong.wordpress.com/2011/06/03/passing-structures-between-managed-and-unmanaged-code/
    /// https://www.codeproject.com/Articles/66244/Marshaling-with-C-Chapter-2-Marshaling-Simple-Type
    /// </summary>
    public abstract partial class NativeMethods
    {
        public abstract partial class Library
        {
            /// <summary>
            /// Default: libdl
            /// </summary>
            public const string DynamicLink = "libdl";
        }

        /// <summary>
        /// Dynamic Link Library (libdl)
        /// </summary>
        public abstract class DL
        {
            /// <summary>
            /// I think this means force load all the symbols even if we're not using them yet.
            /// </summary>
            public const int FLAGS_RTLD_NOW = 2;

            /// <summary>
            /// Loads the dynamic library file named by the null-terminated string filename and returns an opaque "handle" for the dynamic library.
            /// </summary>
            [DllImport(Library.DynamicLink, EntryPoint = "dlopen")]
            public static extern IntPtr Open(string fileName, int flags);

            /// <summary>
            /// TBI
            /// </summary>
            [DllImport(Library.DynamicLink, EntryPoint = "dlsym")]
            public static extern IntPtr Sym(IntPtr handle, string funcName);
        }
    }

    public class PinnedObject : IDisposable
    {
        public GCHandle Handle { get; private set; }
        public bool IsDisposed { get; private set; }

        public PinnedObject(object obj)
        {
            Handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
        }

        /// <summary>
        /// Address of the pinned object in memory.
        /// </summary>
        public IntPtr Address => Handle.IsAllocated ? Handle.AddrOfPinnedObject() : IntPtr.Zero;

        /// <summary>
        /// Releases all resource used by the <see cref="MemoryLock"/> object.
        /// </summary>
        /// <remarks>
        /// Call <see cref="Dispose"/> when you are finished using the <see cref="MemoryLock"/>.
        /// </remarks>
        public void Dispose()
        {
            if (!IsDisposed)
            {
                if (Handle.IsAllocated)
                    Handle.Free();

                IsDisposed = true;
            }
        }
    }
}
