using System;
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
        public readonly TextureUnit TextureSlot;

        public Texture2D(int GLTex, int width, int height, TextureUnit slot)
        {
            GLTexture = GLTex;
            Width = width;
            Height = height;
            TextureSlot = slot;
        }

        public Texture2D(int width, int height, TextureUnit slot, IntPtr data)
        {
            Width = width;
            Height = height;
            TextureSlot = slot;

            GLTexture = GL.GenTexture();
            BindTexture();

            // we might not need to pin this
            using (PinnedObject<IntPtr> texHandle = new PinnedObject<IntPtr>(data))
                GL.TexImage2D(TextureTarget2d.Texture2D, 0, TextureComponentCount.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);

            SetWrap(TextureCoordinate.S, TextureWrapMode.Repeat);
            SetWrap(TextureCoordinate.T, TextureWrapMode.Repeat);

            UnbindTexture();
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
    }
}
