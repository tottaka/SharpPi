using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.ES20;
using SharpPi.Native;

namespace SharpPi.Graphics
{
    public enum TextureCoordinate
    {
        S = TextureParameterName.TextureWrapS,
        T = TextureParameterName.TextureWrapT,
        R = TextureParameterName.TextureWrapR
    }

    public class Texture2D : IDisposable
    {
        public readonly int GLTexture;
        public readonly int Width, Height;
        public readonly PixelFormat PixelFormat;
        public readonly TextureUnit TextureSlot;
        public readonly TextureComponentCount ComponentCount;


        public Texture2D(int width, int height, PixelFormat pixelFormat, TextureComponentCount componentCount, TextureUnit slot)
        {
            Width = width;
            Height = height;
            PixelFormat = pixelFormat;
            TextureSlot = slot;
            ComponentCount = componentCount;
            GLTexture = GL.GenTexture();
            GLException.CheckError("Texture2D");
        }

        public Texture2D(int width, int height, PixelFormat pixelFormat, TextureComponentCount componentCount, TextureUnit slot, IntPtr data) : this(width, height, pixelFormat, componentCount, slot)
        {
            Copy(data);
            GLException.CheckError("Texture2D");
        }

        public void SetMinFilter(TextureMinFilter filter)
        {
            BindTexture();
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)filter);
            UnbindTexture();
        }

        public void SetMagFilter(TextureMagFilter filter)
        {
            BindTexture();
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)filter);
            UnbindTexture();
        }

        public void SetLod(int @base, int min, int max)
        {
            BindTexture();
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureLodBias, @base);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinLod, min);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLod, max);
            UnbindTexture();
        }

        public void SetWrap(TextureCoordinate coord, TextureWrapMode mode)
        {
            BindTexture();
            GL.TexParameter(TextureTarget.Texture2D, (TextureParameterName)coord, (int)mode);
            UnbindTexture();
        }

        public void SetPixelStore(PixelStoreParameter pixelStore, int param)
        {
            BindTexture();
            GL.PixelStore(PixelStoreParameter.UnpackRowLength, param);
            UnbindTexture();
        }

        public void Copy(IntPtr source)
        {
            BindTexture();
            GL.TexImage2D(TextureTarget2d.Texture2D, 0, ComponentCount, Width, Height, 0, PixelFormat, PixelType.UnsignedByte, source);
            GLException.CheckError("Copy(TexImage2D)");

            GL.TexSubImage2D(TextureTarget2d.Texture2D, 0, 0, 0, Width, Height, PixelFormat, PixelType.UnsignedByte, source);
            GLException.CheckError("Copy(TexSubImage2D)");
            UnbindTexture();
        }

        public void Dispose()
        {
            GL.DeleteTexture(GLTexture);
        }

        public void BindTexture()
        {
            GL.ActiveTexture(TextureSlot);
            GL.BindTexture(TextureTarget.Texture2D, GLTexture);
        }

        public void UnbindTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        /// <summary>
        /// Executes the supplied function while this <see cref="Texture2D"/> instance has context.
        /// The <see cref="Texture2D"/> instance is first bound before executing the function, then unbound after the provided function is finished executing.
        /// </summary>
        /// <typeparam name="T">The return value of the function.</typeparam>
        /// <param name="action">The function to execute with texture context.</param>
        /// <returns>The value returned by the provided <paramref name="action"/>.</returns>
        public T BindWhile<T>(Func<T> action)
        {
            BindTexture();
            T result = action();
            UnbindTexture();
            return result;
        }

        /// <summary>
        /// Executes the supplied action while this <see cref="Texture2D"/> instance has context.
        /// The <see cref="Texture2D"/> instance is first bound before executing the action, then unbound after the provided action is finished executing.
        /// </summary>
        /// <param name="action">The action to execute with texture context.</param>
        public void BindWhile(Action action)
        {
            BindTexture();
            action();
            UnbindTexture();
        }
    }
}
