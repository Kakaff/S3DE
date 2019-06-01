using System;

namespace S3DE.Graphics.Textures
{
    internal enum TextureTarget
    {
        TEXTURE_1D = 0x0DE0,
        TEXTURE_1D_ARRAY = 0x8C18,
        TEXTURE_2D = 0x0DE1,
        TEXTURE_2D_ARRAY = 0x8C1A,
        TEXTURE_2D_MULTISAMPLE = 0x9100,
        TEXTURE_2D_MULTISAMPLE_ARRAY = 0x9102,
        TEXTURE_3D = 0x806F,
        TEXTURE_CUBE_MAP = 0x8513,
        TEXTURE_CUBE_MAP_ARRAY = 0x9009,
        TEXTURE_RECTANGLE = 0x84F5
    }

    public enum TextureParameter
    {
        DEPTH_STENCIL_TEXTURE_MODE = 0x90EA,
        TEXTURE_MIN_LOD = 0x813A,
        TEXTURE_MAX_LOD = 0x813B,
        TEXTURE_BASE_LEVEL = 0x813C,
        TEXTURE_MAX_LEVEL = 0x813D,
        TEXTURE_COMPARE_MODE = 0x884C,
        TEXTURE_COMPARE_FUNC = 0x884D,
        TEXTURE_LOD_BIAS = 0x8501,
        TEXTURE_MAG_FILTER = 0x2800,
        TEXTURE_MIN_FILTER = 0x2801,
        TEXTURE_SWIZZLE_R = 0x8E42,
        TEXTURE_SWIZZLE_G = 0x8E43,
        TEXTURE_SWIZZLE_B = 0x8E44,
        TEXTURE_SWIZZLE_A = 0x8E45,
        TEXTURE_SWIZZLE_RGBA = 0x8E46,
        TEXTURE_WRAP_S = 0x2802,
        TEXTURE_WRAP_T = 0x2803,
        TEXTURE_WRAP_R = 0x8072,
        TEXTURE_BORDER_COLOR = 0x1004,
        TEXTURE_MAX_ANISOTROPY = 0x84FE,
    }

    internal enum InternalFilterMode
    {
        NEAREST = 0x2600,
        LINEAR = 0x2601,
        NEAREST_MIPMAP_NEAREST = 0x2700,
        LINEAR_MIPMAP_NEAREST = 0x2701,
        NEAREST_MIPMAP_LINEAR = 0x2702,
        LINEAR_MIPMAP_LINEAR = 0x2703,
    }

    public enum FilterMode
    {
        Nearest,
        BiLinear,
        TriLinear
    }

    public enum AnisotropicSamples
    {
        x1 = 0x0,
        x2 = 0x2,
        x4 = 0x4,
        x8 = 0x8,
        x16 = 0x10
    }

    public enum WrapMode
    {
        None,
        Clamp = 0x2900,
        Repeat = 0x2901,
    }

    public enum InternalFormat
    {
        RED = 0x1903,
        RG = 0x8227,
        RGB = 0x1907,
        RGBA = 0x1908,
        DEPTH_COMPONENT = 0x1902,
        DEPTH_STENCIL = 0x84F9,
        STENCIL_INDEX = 0x1901,
        STENCIL_INDEX1 = 0x8D46,
        STENCIL_INDEX4 = 0x8D47,
        STENCIL_INDEX8 = 0x8D48,
        STENCIL_INDEX16 = 0x8D49,
        DEPTH24_STENCIL8 = 0x88F0,

        R8 = 0x8229,
        R16 = 0x822A,
        RG8 = 0x822B,
        RG16 = 0x822C,
        RGB4 = 0x804F,
        RGB5 = 0x8050,
        RGB8 = 0x8051,
        RGB10 = 0x8052,
        RGB12 = 0x8053,
        RGB16 = 0x8054,
        RGBA2 = 0x8055,
        RGBA4 = 0x8056,
        RGB5_A1 = 0x8057,
        RGBA8 = 0x8058,
        RGB10_A2 = 0x8059,
        RGBA12 = 0x805A,
        RGBA16 = 0x805B,

        R16F = 0x822D,
        R32F = 0x822E,
        RG16F = 0x822F,
        RG32F = 0x8230,
        RGB16F = 0x881B,
        RGB32F = 0x8815,
        RGBA16F = 0x881A,
        RGBA32F = 0x8814,

        R8I = 0x8231,
        R16I = 0x8233,
        R32I = 0x8235,
        RG8I = 0x8237,
        RG16I = 0x8239,
        RG32I = 0x823B,
        RGB8I = 0x8D8F,
        RGB16I = 0x8D89,
        RGB32I = 0x8D83,
        RGBA8I = 0x8D8E,
        RGBA16I = 0x8D88,
        RGBA32I = 0x8D82,

        R8UI = 0x8232,
        R16UI = 0x8234,
        R32UI = 0x8236,
        RG8UI = 0x8238,
        RG16UI = 0x823A,
        RG32UI = 0x823C,
        RGB8UI = 0x8D7D,
        RGB16UI = 0x8D77,
        RGB32UI = 0x8D71,
        RGBA8UI = 0x8D7C,
        RGBA16UI = 0x8D76,
        RGBA32UI = 0x8D70,
    }

    public enum PixelType
    {
        BYTE = 0x1400,
        UNSIGNED_BYTE = 0x1401,
        SHORT = 0x1402,
        UNSIGNED_SHORT = 0x1403,
        INT = 0x1404,
        UNSIGNED_INT = 0x1405,
        FLOAT = 0x1406,

        UNSIGNED_INT_24_8 = 0x84FA,
        UNSIGNED_BYTE_3_3_2 = 0x8032,
        UNSIGNED_SHORT_4_4_4_4 = 0x8033,
        UNSIGNED_SHORT_5_5_5_1 = 0x8034,
        UNSIGNED_INT_8_8_8_8 = 0x8035,
        UNSIGNED_INT_10_10_10_2 = 0x8036,
        UNSIGNED_BYTE_2_3_3_REV = 0x8362,
        UNSIGNED_SHORT_5_6_5 = 0x8363,
        UNSIGNED_SHORT_5_6_5_REV = 0x8364,
        UNSIGNED_SHORT_4_4_4_4_REV = 0x8365,
        UNSIGNED_SHORT_1_5_5_5_REV = 0x8366,
        UNSIGNED_INT_8_8_8_8_REV = 0x8367
    }

    public enum Texture2DTarget
    {
        TEXTURE_1D_ARRAY = 0x8C18,
        TEXTURE_2D = 0x0DE1,
        TEXTURE_RECTANGLE = 0x84F5,
        TEXTURE_CUBE_MAP_POSITIVE_X = 0x8515,
        TEXTURE_CUBE_MAP_NEGATIVE_X = 0x8516,
        TEXTURE_CUBE_MAP_POSITIVE_Y = 0x8517,
        TEXTURE_CUBE_MAP_NEGATIVE_Y = 0x8518,
        TEXTURE_CUBE_MAP_POSITIVE_Z = 0x8519,
        TEXTURE_CUBE_MAP_NEGATIVE_Z = 0x851A,
    }

    public enum PixelFormat
    {
        RED = 0x1903,
        RG = 0x8227,
        RGB = 0x1907,
        RGBA = 0x1908,
        DEPTH_COMPONENT = 0x1902,
        DEPTH_STENCIL = 0x84F9,
    }

    public enum ColorFormat
    {
        Red = 0x1,
        RG = 0x2,
        RGB = 0x3,
        RGBA = 0x4,
    }

    public abstract partial class Texture
    {
        static int instanceCount = 0;
        int instanceID;

        public int GetInstanceID() => instanceID;
        
        public Texture()
        {
            instanceID = instanceCount;
            instanceCount++;
        }

        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract InternalFormat InternalFormat { get; }
        public abstract PixelFormat PixelFormat {get;}
        public abstract PixelType PixelType { get; }
        public abstract IntPtr Handle {get;}
        public abstract int BoundTexUnit {get; protected set;}
        public abstract bool IsBound {get; protected set;}

        public int Bind() =>
            Texture.Bind(this);
        
        public void Bind(uint TextureUnit) =>
            Texture.Bind(this, TextureUnit);

        protected void SetActive() =>
            Texture.SetActiveTextureUnit((uint)BoundTexUnit);
    }
}
