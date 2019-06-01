using S3DE.Utility;
using System;

namespace S3DE.Graphics.Textures
{
    public sealed partial class Texture2D : RenderTexture2D, ITexture2D
    {
        byte[] data;

        ColorFormat colfrmt;

        public ColorFormat ColorFormat => colfrmt;

        public byte[] PixelData => data;

        public Color this[int x, int y]
        {
            get => GetPixel(x, y);
            set => SetPixel(x, y,value);
        }

        /// <summary>
        /// Creates a new Texture2D with the same settings as another Texture2D.
        /// </summary>
        /// <param name="tex"></param>
        public Texture2D(Texture2D tex) : base(tex.Width,tex.Height,tex.InternalFormat,tex.PixelFormat,tex.PixelType)
        {
            colfrmt = tex.ColorFormat;
            data = new byte[(tex.Width * tex.Height) * (int)tex.ColorFormat];
            wrapMode = tex.WrapMode;
            anisoSamples = tex.AnisotropicSamples;
            filterMode = tex.FilterMode;
        }

        public Texture2D(int width, int height, ColorFormat colorFormat) 
            : base(width,height,GetInternalFormat(colorFormat),GetPixelFormat(colorFormat),PixelType.UNSIGNED_BYTE)
        {
            colfrmt = colorFormat;
            data = new byte[(width * height) * (int)colorFormat];
        }

        public Texture2D(int width, int height, ColorFormat colorFormat, InternalFormat internalFormat) 
            : base(width,height,internalFormat,GetPixelFormat(colorFormat),PixelType.UNSIGNED_BYTE)
        {
            colfrmt = colorFormat;
            data = new byte[(width * height) * (int)colorFormat];
        }
        
        public void SetPixel(int x, int y,Color c)
        {
            int frstIndx = (x + (y * Width)) * (int)ColorFormat;
            
            switch (ColorFormat)
            {
                case ColorFormat.Red: { data[frstIndx] = c.R; break; }
                case ColorFormat.RG: { data[frstIndx] = c.R; data[frstIndx + 1] = c.G; break; }
                case ColorFormat.RGB: { data[frstIndx] = c.R; data[frstIndx + 1] = c.G; data[frstIndx + 2] = c.B; break; }
                case ColorFormat.RGBA: { data[frstIndx] = c.R; data[frstIndx + 1] = c.G; data[frstIndx + 2] = c.B; data[frstIndx + 3] = c.A; break;}
            }

            DataChanged = true;
        }

        public Color GetPixel(int x, int y)
        {
            int frstIndx = (x + (y * Width)) * (int)ColorFormat;
            switch (ColorFormat)
            {
                case ColorFormat.Red: return new Color(data[frstIndx], 0, 0);
                case ColorFormat.RG: return new Color(data[frstIndx], data[frstIndx + 1],0);
                case ColorFormat.RGB: return new Color(data[frstIndx], data[frstIndx + 1], data[frstIndx + 2]);
                case ColorFormat.RGBA: return new Color(data[frstIndx], data[frstIndx + 1], data[frstIndx + 2], data[frstIndx + 3]);
            }
            throw new ArgumentException("Texture2D has a unknown/unsupported ColorFormat");
        }

        public void Clear()
        {
            Array.Clear(data, 0, data.Length);
        }

        
        protected override void UploadPixelData()
        {
            Console.WriteLine("Uploading pixeldata");
            
            using (PinnedMemory pm = new PinnedMemory(data))
            {
                Extern_SetTexImage2D_Data(Handle, Texture2DTarget.TEXTURE_2D, 0,
                    InternalFormat, Width, Height, 0,
                    PixelFormat, PixelType, pm.Adress);
            }

            if (!Renderer.NoError)
                throw new Exception("Error uploading texture data!");
        }

        static InternalFormat GetInternalFormat(ColorFormat colfrmt)
        {
            switch (colfrmt)
            {
                case ColorFormat.Red: return InternalFormat.RED;
                case ColorFormat.RG: return InternalFormat.RG;
                case ColorFormat.RGB: return InternalFormat.RGB;
                case ColorFormat.RGBA: return InternalFormat.RGBA;
                default: throw new NotSupportedException("Texture2D has a unkown/unsupported ColorFormat");
            }
        }

        static PixelFormat GetPixelFormat(ColorFormat colfrmt)
        {
            switch (colfrmt)
            {
                case ColorFormat.Red: return PixelFormat.RED;
                case ColorFormat.RG: return PixelFormat.RG;
                case ColorFormat.RGB: return PixelFormat.RGB;
                case ColorFormat.RGBA: return PixelFormat.RGBA;
                default: throw new NotSupportedException("Texture2D has a unkown/unsupported ColorFormat");
            }
        }

        public static Texture2D Copy(Texture2D tex)
        {
            Texture2D rTex = new Texture2D(tex);
            Buffer.BlockCopy(tex.data, 0, rTex.data, 0, tex.data.Length);

            return rTex;
        }
    }
}
