using System;
using System.Collections.Generic;
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

        private static Dictionary<string, DynamicLibrary> CachedLibraries = new Dictionary<string, DynamicLibrary>();

        /// <summary>
        /// Dynamic Link Library (libdl)
        /// </summary>
        public class DynamicLibrary : IDisposable
        {
            /// <summary>
            /// I think this means force load all the symbols even if we're not using them yet.
            /// </summary>
            public const int FLAGS_RTLD_NOW = 2;

            /// <summary>
            /// The handle to the loaded library.
            /// </summary>
            public IntPtr Location { get; private set; }
            public bool IsDisposed { get; private set; }

            private Dictionary<string, IntPtr> FunctionLocations;

            private DynamicLibrary(IntPtr loc)
            {
                Location = loc;
                FunctionLocations = new Dictionary<string, IntPtr>();
            }

            ~DynamicLibrary()
            {
                Dispose();
            }

            public IntPtr GetFunctionLocation(string procAddress)
            {
                if (FunctionLocations.ContainsKey(procAddress))
                    return FunctionLocations[procAddress];

                IntPtr location = Sym(Location, procAddress);
                FunctionLocations.Add(procAddress, location);
                return FunctionLocations[procAddress];
            }

            public void Dispose()
            {
                if (!IsDisposed)
                {
                    if (Close(Location) != 0)
                        throw new Exception("DynamicLink Error: " + GetError());
                    IsDisposed = true;
                }
            }

            public static DynamicLibrary Load(string fileName, int flags)
            {
                if (CachedLibraries.ContainsKey(fileName))
                    return CachedLibraries[fileName];

                IntPtr location = Open(fileName, flags);
                if (location == IntPtr.Zero)
                    throw new DllNotFoundException("Could not open library '" + fileName + "'.");

                CachedLibraries.Add(fileName, new DynamicLibrary(location));
                return CachedLibraries[fileName];
            }

            /// <summary>
            /// Loads the dynamic library file named by the null-terminated string filename and returns an opaque "handle" for the dynamic library.
            /// </summary>
            [DllImport(Library.DynamicLink, EntryPoint = "dlopen")]
            private static extern IntPtr Open(string fileName, int flags);

            /// <summary>
            /// TBI
            /// </summary>
            [DllImport(Library.DynamicLink, EntryPoint = "dlsym")]
            private static extern IntPtr Sym(IntPtr handle, string funcName);

            /// <summary>
            /// Decrements the reference count on the dynamic library handle handle.
            /// If the reference count drops to zero and no other loaded libraries use symbols in it, then the dynamic library is unloaded.
            /// </summary>
            /// <returns>0 on success, and nonzero on error.</returns>
            [DllImport(Library.DynamicLink, EntryPoint = "dlclose")]
            private static extern int Close(IntPtr handle);

            /// <summary>
            /// Returns a human readable string describing the most recent error that occurred from dlopen(), dlsym() or dlclose() since the last call to dlerror().
            /// It returns <see cref="null"/> if no errors have occurred since initialization or since it was last called.
            /// </summary>
            /// <returns></returns>
            [DllImport(Library.DynamicLink, EntryPoint = "dlerror")]
            private static extern string GetError();
        }
    }

    public class PinnedObject<T> : IDisposable
    {
        public GCHandle Handle { get; private set; }
        //public int Size { get; private set; }
        public bool IsDisposed { get; private set; }

        public PinnedObject(T obj)
        {
            Handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
            //Size = Marshal.SizeOf<T>();
        }

        /// <summary>
        /// Address of the pinned object in memory.
        /// </summary>
        public IntPtr Address => Handle.IsAllocated ? Handle.AddrOfPinnedObject() : IntPtr.Zero;

        ~PinnedObject()
        {
            Dispose();
        }

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

    public class PinnedBuffer : IDisposable
    {
        public byte[] Bits;
        public bool IsDisposed { get; private set; }
        public IntPtr BitsPointer { get; private set; }
        protected GCHandle BitsHandle { get; private set; }

        public PinnedBuffer(int size)
        {
            Bits = new byte[size];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            BitsPointer = BitsHandle.AddrOfPinnedObject();
        }

        public PinnedBuffer(byte[] buffer)
        {
            Bits = buffer;
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            BitsPointer = BitsHandle.AddrOfPinnedObject();
        }

        ~PinnedBuffer()
        {
            Dispose();
        }

        public void Overwrite(byte[] data)
        {
            lock (Bits)
            {
                if (data.Length > Bits.Length)
                    throw new InvalidOperationException("Data length cannot be greater than the size of the buffer. (Data: " + data.Length + " / Total: " + Bits.Length + ")");

                Marshal.Copy(data, 0, BitsPointer, data.Length);
            }
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                BitsHandle.Free();
                IsDisposed = true;
            }
        }
    }
}
