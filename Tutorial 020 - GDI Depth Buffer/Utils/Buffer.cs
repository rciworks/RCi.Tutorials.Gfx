using System;
using System.Runtime.InteropServices;
using System.Security;

namespace RCi.Tutorials.Gfx.Utils
{
    public class Buffer
    {
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false), SuppressUnmanagedCodeSecurity]
        public static extern unsafe void* memcpy(void* dest, void* src, ulong count);

        protected Buffer() { }
    }

    public unsafe class Buffer<T> :
        Buffer,
        IDisposable
        where T : unmanaged
    {
        #region // storage

        public T[] Data { get; private set; }

        private GCHandle GCHandle { get; set; }

        public IntPtr Address { get; private set; }

        public T* Pointer { get; private set; }

        #endregion

        #region // ctor

        public Buffer(T[] data)
        {
            Data = data;
            GCHandle = GCHandle.Alloc(Data, GCHandleType.Pinned);
            Address = GCHandle.AddrOfPinnedObject();
            Pointer = (T*)Address;
        }

        public Buffer(int length) :
            this(new T[length])
        {
        }

        public virtual void Dispose()
        {
            Pointer = default;
            Address = default;
            if (GCHandle.IsAllocated)
            {
                GCHandle.Free();
            }
            GCHandle = default;
            Data = default;
        }

        #endregion

        #region // routines

        #region // write

        public void Write<U>(int index, U value)
            where U : unmanaged
        {
            *(U*)(Pointer + index) = value;
        }

        public void Write<U>(int index, U* source)
            where U : unmanaged
        {
            *(U*)(Pointer + index) = *source;
        }

        public void Write<U>(int index, U[] source, int sourceIndex, int count)
            where U : unmanaged
        {
            fixed (U* sourcePointer = source)
            {
                memcpy(Pointer + index, sourcePointer + sourceIndex, (ulong)(count * sizeof(U)));
            }
        }

        public void Write<U>(int index, U[] source)
            where U : unmanaged
        {
            fixed (U* sourcePointer = source)
            {
                memcpy(Pointer + index, sourcePointer, (ulong)(source.Length * sizeof(U)));
            }
        }

        public void Write<U>(int index, U* source, int count)
            where U : unmanaged
        {
            memcpy(Pointer + index, source, (ulong)(count * sizeof(U)));
        }

        #endregion

        #region // read

        public U Read<U>(int index)
            where U : unmanaged
        {
            return *(U*)(Pointer + index);
        }

        public void Read<U>(int index, U* destination)
            where U : unmanaged
        {
            *destination = *(U*)(Pointer + index);
        }

        public void Read<U>(int index, U[] destination, int destinationIndex, int count)
            where U : unmanaged
        {
            fixed (U* destinationPointer = destination)
            {
                memcpy(destinationPointer + destinationIndex, Pointer + index, (ulong)(count * sizeof(U)));
            }
        }

        public void Read<U>(int index, U[] destination)
            where U : unmanaged
        {
            fixed (U* destinationPointer = destination)
            {
                memcpy(destinationPointer, Pointer + index, (ulong)(destination.Length * sizeof(U)));
            }
        }

        public void Read<U>(int index, U* destination, int count)
            where U : unmanaged
        {
            memcpy(Pointer + index, destination, (ulong)(count * sizeof(U)));
        }

        #endregion

        #endregion
    }
}
