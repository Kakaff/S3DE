using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Textures
{
    sealed partial class Texture2D
    {
        [DllImport("S3DECore.dll")]
        private static extern IntPtr Extern_CreateTexture(TextureTarget targ);
        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetTexImage2D_Data(IntPtr tex,Texture2DTarget target, int level, InternalFormat internalFormat,
            int widht, int height, int border, PixelFormat textureDataFormat, PixelType textureDataType, byte[] data);
        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetTexParameteri(IntPtr tex,TextureParameter param, int value);
        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetTexParameterf(IntPtr tex, TextureParameter param, float value);
    }

    partial class ITexture
    {
        [DllImport("S3DECore.dll")]
        private static extern void Extern_GLGeti(GL param, out int i);
        [DllImport("S3DECore.dll")]
        private static extern void Extern_BindTexture(IntPtr tex, uint textureunit);
        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetActiveTexture(uint textureUnit);
    }
}
