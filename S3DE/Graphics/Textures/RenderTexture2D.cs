using System;

namespace S3DE.Graphics.Textures
{
    public partial class RenderTexture2D : Texture,IRenderTexture
    {
        static FilterMode dfltFm = FilterMode.TriLinear; //default filtermode
        static WrapMode dfltWm = WrapMode.Repeat; //default wrapmode
        static AnisotropicSamples dfltAs = AnisotropicSamples.x16; //default aniso.

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

        protected InternalFormat internfrmt;
        protected PixelFormat pxfrmt;
        protected PixelType pxType;
        protected FilterMode filterMode;
        protected WrapMode wrapMode;
        protected AnisotropicSamples anisoSamples;
        
        int width, height, mipmapCount, boundTexUnit;

        bool isBound, dataChanged,hasChanged, resChanged, wrapChanged, filterChanged, anisoChanged, mipmapChanged;

        IntPtr handle;

        public override IntPtr Handle => handle;
        
        public override int BoundTexUnit
        {
            get => boundTexUnit;
            protected set => boundTexUnit = value;
        }

        public override bool IsBound
        {
            get => isBound;
            protected set => isBound = value;
        }

        public bool HasChanged { get => hasChanged; protected set => hasChanged = value;}
        protected bool ResChanged { get => resChanged; set => resChanged = value;}
        protected bool WrapChanged { get => wrapChanged; set => wrapChanged = value;}
        protected bool FilterChanged { get => filterChanged; set => filterChanged = value;}
        protected bool AnisoChanged { get => anisoChanged; set => anisoChanged = value;}
        protected bool MipmapChanged { get => mipmapChanged; set => mipmapChanged = value;}
        public bool DataChanged { get => dataChanged; protected set => dataChanged = value;}

        public override int Width => width;

        public override int Height => height;

        public int MipmapCount => mipmapCount;
        public override InternalFormat InternalFormat => internfrmt;
        public override PixelFormat PixelFormat => pxfrmt;
        public override PixelType PixelType => pxType;
        

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

        private RenderTexture2D() { }

        public RenderTexture2D(int width, int height, InternalFormat internalFormat, PixelFormat pixelFormat,PixelType pixelType)
        {
            pxType = pixelType;
            pxfrmt = pixelFormat;
            internfrmt = internalFormat;
            this.width = width;
            this.height = height;
            dataChanged = true;
            
            handle = Extern_Texture2D_Create();
            if (!Renderer.NoError)
                throw new Exception("Error creating texture2d");

            SetIsBound(this, ActiveTextureUnit);

            AnisotropicSamples = DefaultAnisotropicFiltering;
            WrapMode = DefaultWrapMode;
            FilterMode = DefaultTextureFiltering;
        }

        public void Apply()
        {
            if (hasChanged)
            {
                if (!isBound || boundTexUnit != ActiveTextureUnit)
                    if (!isBound)
                        Bind();
                    else if (isBound && boundTexUnit != ActiveTextureUnit)
                        SetActive();

                if (dataChanged)
                {
                    UploadPixelData();
                    if (!Renderer.NoError)
                        throw new Exception($"Error uploading pixeldata! Error:{Renderer.LatestError}");
                    dataChanged = false;
                }

                if (mipmapChanged)
                {
                    throw new NotImplementedException("Mipmaps not yet implemented");
                    if (mipmapCount > 0)
                    {

                    }
                }

                Console.WriteLine("Applying filtermode");
                if (filterChanged)
                    ApplyFilterMode();
                Console.WriteLine("Applying anisotropic filtering");
                if (anisoChanged)
                    ApplyAnisoSettings();
                Console.WriteLine("Applying wrapmode");
                if (wrapChanged)
                    ApplyWrapMode();

                hasChanged = false;
            }
        }

        protected virtual void UploadPixelData()
        {
            Console.WriteLine($"Setting rendertexture2d data  res: {width} {height}");
            Extern_SetTexImage2D_Data(handle, Texture2DTarget.TEXTURE_2D, 0,
                InternalFormat, width, height, 0, PixelFormat, PixelType,IntPtr.Zero);
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
        
        protected void SetTexParameter(TextureParameter parameter, int value)
        {
            Extern_SetTexParameteri(handle, parameter, value);
            if (!Renderer.NoError)
                throw new Exception("Error setting TexParameteri");
        }

        protected void SetTexParameter(TextureParameter parameter, float value)
        {
            Extern_SetTexParameterf(handle, parameter, value);

            if (!Renderer.NoError)
                throw new Exception("Error setting TexParameterf");
        }
    }
}
