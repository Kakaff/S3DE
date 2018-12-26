using System;
using System.Runtime.InteropServices;

namespace S3DE.Graphics.Textures
{
    sealed partial class Texture2D
    {
        
    }

    partial class RenderTexture2D
    {
        [DllImport("S3DECore.dll")]
        protected static extern void Extern_SetTexImage2D_Data(IntPtr tex, Texture2DTarget target, int level, InternalFormat internalFormat,
            int widht, int height, int border, PixelFormat textureDataFormat, PixelType textureDataType, IntPtr data);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_InitTexImage2D_Data(IntPtr tex, Texture2DTarget target, int level, InternalFormat internalFormat,
            int widht, int height, int border, PixelFormat textureDataFormat, PixelType textureDataType);

        [DllImport("S3DECore.dll")]
        private static extern IntPtr Extern_Texture2D_Create();
        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetTexParameteri(IntPtr tex, TextureParameter param, int value);
        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetTexParameterf(IntPtr tex, TextureParameter param, float value);
    }

    partial class Texture
    {
        [DllImport("S3DECore.dll")]
        private static extern void Extern_GLGeti(GL param, out int i);
        [DllImport("S3DECore.dll")]
        private static extern void Extern_BindTexture(IntPtr tex, uint textureunit);
        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetActiveTextureUnit(uint textureunit);
    }
}
