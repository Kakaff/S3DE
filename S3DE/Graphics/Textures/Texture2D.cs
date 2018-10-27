using S3DE.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Textures
{
    public sealed partial class Texture2D : ITexture
    {

        static FilterMode dfltFm = FilterMode.TriLinear;
        static WrapMode dfltWm = WrapMode.Repeat;
        static AnisotropicSamples dfltAs = AnisotropicSamples.x16;

        public static FilterMode DefaultTextureFiltering
        {
            get => dfltFm;
            set => dfltFm = value;
        }

        public static WrapMode DefaultWrapMode
        {
            get => dfltWm;
            set => dfltWm = value;
        }

        public static AnisotropicSamples DefaultAnisotropicFiltering
        {
            get => dfltAs;
            set => dfltAs = value;
        }

        ColorFormat frmt;
        FilterMode filterMode;
        WrapMode wrapMode;
        AnisotropicSamples anisoSamples;
        
        byte[] data; 
        int width, height,mipmapCount,boundTexUnit;
        
        bool isBound,hasChanged,resChanged, dataChanged, wrapChanged, filterChanged, anisoChanged, mipmapChanged;

        IntPtr handle;

        protected override IntPtr Handle => handle;

        public override int BoundTexUnit {
            get => boundTexUnit;
            protected set => boundTexUnit = value;
        }

        public override bool IsBound {
            get => isBound;
            protected set => isBound = value;
        }

        public override int Width => width;

        public override int Height => height;

        public override ColorFormat ColorFormat => frmt;

        public override byte[] PixelData => data;

        public FilterMode FilterMode
        {
            get => filterMode;
            set
            {
                filterChanged = true;
                hasChanged = true;
                filterMode = value;
            }
        }

        public WrapMode WrapMode
        {
            get => wrapMode;
            set
            {
                wrapChanged = true;
                hasChanged = true;
                wrapMode = value;
            }
        }

        public AnisotropicSamples AnisotropicSamples
        {
            get => anisoSamples;
            set
            {
                hasChanged = true;
                anisoChanged = true;
                anisoSamples = value;

            }
        }

        private Texture2D() { }

        public Texture2D(int width, int height, ColorFormat colorFormat)
        {
            frmt = colorFormat;
            this.width = width;
            this.height = height;
            data = new byte[(width * height) * (int)colorFormat];
            dataChanged = true;
            handle = Extern_CreateTexture(TextureTarget.TEXTURE_2D);
            AnisotropicSamples = DefaultAnisotropicFiltering;
            WrapMode = DefaultWrapMode;
            FilterMode = DefaultTextureFiltering;
        }

        public void SetPixel(int x, int y,Color c)
        {
            int frstIndx = (x + (y * width)) * (int)ColorFormat;
            
            switch (ColorFormat)
            {
                case ColorFormat.Red: { data[frstIndx] = c.R; break; }
                case ColorFormat.RGB: { data[frstIndx] = c.R; data[frstIndx + 1] = c.G; data[frstIndx + 2] = c.B; break; }
                case ColorFormat.RGBA: { data[frstIndx] = c.R; data[frstIndx + 1] = c.G; data[frstIndx + 2] = c.B; data[frstIndx + 3] = c.A; break;}
            }
            dataChanged = true;
        }

        public Color GetPixel(int x, int y)
        {
            int frstIndx = (x + (y * width)) * (int)ColorFormat;
            switch (ColorFormat)
            {
                case ColorFormat.Red: return new Color(data[frstIndx], 0, 0);
                case ColorFormat.RGB: return new Color(data[frstIndx], data[frstIndx + 1], data[frstIndx + 2]);
                case ColorFormat.RGBA: return new Color(data[frstIndx], data[frstIndx + 1], data[frstIndx + 2], data[frstIndx + 3]);
            }
            throw new ArgumentException("Texture2D has a unknown/unsupported ColorFormat");
        }

        public void Clear()
        {
            Array.Clear(data, 0, data.Length);
        }

        public void Apply()
        {
            if (hasChanged)
            {
                if (!isBound)
                    Bind();
                if (boundTexUnit != ITexture.ActiveTextureUnit)
                    SetActiveTexture(this);

                if (dataChanged)
                    UploadPixelData();

                if (mipmapChanged)
                {
                    throw new NotImplementedException("Mipmaps not yet implemented");
                    if (mipmapCount > 0)
                    {
                        
                    }
                }

                
                hasChanged = false;

                Console.WriteLine("Applying filtermode");
                if (filterChanged)
                    ApplyFilterMode();
                Console.WriteLine("Applying anisotropic filtering");
                if (anisoChanged)
                    ApplyAnisoSettings();
                Console.WriteLine("Applying wrapmode");
                if (wrapChanged)
                    ApplyWrapMode();
            }
        }

        void UploadPixelData()
        {
            Console.WriteLine("Uploading pixeldata");
            PixelFormat pxfrmt;

            switch (ColorFormat)
            {
                case ColorFormat.Red: pxfrmt = PixelFormat.RED; break;
                case ColorFormat.RG: pxfrmt = PixelFormat.RG; break;
                case ColorFormat.RGB: pxfrmt = PixelFormat.RGB; break;
                case ColorFormat.RGBA: pxfrmt = PixelFormat.RGBA; break;
                default: throw new NotSupportedException("Texture2D has a unkown/unsupported ColorFormat");
            }
            
            using (PinnedMemory pm = new PinnedMemory(data))
            {
                Extern_SetTexImage2D_Data(handle, Texture2DTarget.TEXTURE_2D, 0,
                    InternalFormat.RGB, width, height, 0,
                    pxfrmt, PixelType.UNSIGNED_BYTE, data);
            }

            dataChanged = false;
        }

        void ApplyAnisoSettings()
        {
            SetTexParameter(TextureParameter.TEXTURE_MAX_ANISOTROPY, (float)anisoSamples);
            anisoChanged = false;
        }

        void ApplyFilterMode()
        {
            int min = 0, max = 0;
            switch (FilterMode)
            {
                case FilterMode.Nearest:
                    {
                        min = (int)((mipmapCount > 0) ?
                            InternalFilterMode.NEAREST_MIPMAP_LINEAR :
                            InternalFilterMode.NEAREST
                            );
                        max = (int)InternalFilterMode.NEAREST;
                        break;
                    }
                case FilterMode.BiLinear:
                    {
                        min = (int)((mipmapCount > 0) ?
                            InternalFilterMode.LINEAR_MIPMAP_NEAREST :
                            InternalFilterMode.NEAREST
                            );
                        max = (int)InternalFilterMode.LINEAR;
                        break;
                    }
                case FilterMode.TriLinear:
                    {
                        min = (int)((mipmapCount > 0) ?
                            InternalFilterMode.LINEAR_MIPMAP_NEAREST :
                            InternalFilterMode.LINEAR
                            );
                        max = (int)InternalFilterMode.LINEAR;
                        break;
                    }
            }
            SetTexParameter(TextureParameter.TEXTURE_MIN_FILTER, min);
            SetTexParameter(TextureParameter.TEXTURE_MAG_FILTER, max);
            filterChanged = false;
        }
    

        void ApplyWrapMode()
        {
            switch (wrapMode)
            {
                case WrapMode.None: { break; }
                case WrapMode.Clamp:
                    {
                        SetTexParameter(TextureParameter.TEXTURE_WRAP_S, (int)WrapMode.Clamp);
                        SetTexParameter(TextureParameter.TEXTURE_WRAP_T, (int)WrapMode.Clamp);
                        break;
                    }
                case WrapMode.Repeat:
                    {
                        SetTexParameter(TextureParameter.TEXTURE_WRAP_S, (int)WrapMode.Repeat);
                        SetTexParameter(TextureParameter.TEXTURE_WRAP_T, (int)WrapMode.Repeat);
                        break;
                    }
            }
            wrapChanged = false;
        }

        void CreateHandle()
        {
            handle = Extern_CreateTexture(TextureTarget.TEXTURE_2D);
        }

        void SetTexParameter(TextureParameter parameter, int value)
        {
            Extern_SetTexParameteri(handle,parameter, value);
        }

        void SetTexParameter(TextureParameter parameter, float value)
        {
            Extern_SetTexParameterf(handle, parameter, value);
        }

        void Bind()
        {
            ITexture.Bind(this);
        }
    }
}
