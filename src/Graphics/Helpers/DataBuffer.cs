using System;
using OpenTK.Graphics.OpenGL;

namespace Polynano.Graphics.Helpers
{
    /// <summary>
    // Wrapper around the OpenGL Buffer 
    // that owns the buffer and therefore is responsible
    // for deleting it.
    /// </summary>
    public class DataBuffer : IDisposable
    {
        public int Handle { get; private set; }

        public DataBuffer()
        {
            Handle = GL.GenBuffer();
        }

        /// <summary>
        /// Send the data to the buffer
        /// </summary>
        /// <param name="target">buffer target type</param>
        /// <param name="data">data to store in the buffer</param>
        /// <param name="totalSizeInBytes">total size in bytes of the data</param>
        /// <param name="usageHint">usage hint of how the buffer will be used</param>
        public void BufferData<T>(
            BufferTarget target,
            T[] data,
            int totalSizeInBytes,
            BufferUsageHint usageHint = BufferUsageHint.DynamicDraw)
            where T : struct
        {
            GL.BindBuffer(target, Handle);
            GL.BufferData<T>(target, new IntPtr(totalSizeInBytes), data, usageHint);
            GL.BindBuffer(target, 0);
        }

        /// <summary>
        /// Update the data in the buffer
        /// </summary>
        public void BufferSubData<T>(
            BufferTarget target,
            T[] data,
            int totalSizeInBytes,
            IntPtr offset,
            BufferUsageHint usageHint = BufferUsageHint.DynamicDraw
            )
            where T : struct
        {
            GL.BindBuffer(target, Handle);
            GL.BufferSubData<T>(target, offset, totalSizeInBytes, data);
            GL.BindBuffer(target, 0);
        }

        ~DataBuffer()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (Handle != 0)
            {
                GL.DeleteBuffer(Handle);
                Handle = 0;
            }
        }
    }
}
