using OpenGL;
using S3DE.Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S3DE.Maths;
using S3DE.Engine;
using S3DE.Engine.Graphics.Textures;

namespace S3DE.Engine.Graphics.OpGL
{
    class OpenGL_Texture2D : Texture2D, IOpenGL_Texture
    {
        int mipmapCount;
        uint pointer;
        Color[,] pixels;
        Vector2 size;
        FilterMode filterMode;
        AnisotropicSamples samples;
        WrapMode wrapMode;
        Enums.PixelFormat pixelFormat;
        Enums.InternalFormat internalFormat;
        Enums.PixelType pixelType;

        public override int MipMapLevels
        {
            get => mipmapCount;
            set => SetMipMapCount(value);
        }
        internal OpenGL_Texture2D(int width, int height)
        {
            size = new Vector2(width, height);
            pixels = new Color[width, height];
            internalFormat = Enums.InternalFormat.RGBA;
            pixelFormat = Enums.PixelFormat.RGBA;
            pixelType = Enums.PixelType.UByte;
        }

        public override Vector2 Resolution => size;

        public override FilterMode FilterMode { get => filterMode; set => filterMode = value;}
        public override AnisotropicSamples AnisotropicSamples { get => samples; set => samples = value; }
        public override WrapMode WrapMode { get => wrapMode; set => wrapMode = value; }
        public override Enums.InternalFormat InternalFormat { get => internalFormat; set => internalFormat = value; }
        public override Enums.PixelFormat PixelFormat { get => pixelFormat; set => pixelFormat = value;}
        public override Enums.PixelType PixelType { get => pixelType; set => pixelType = value;}

        public uint Pointer { get { if (pointer == 0) GenerateHandle(); return pointer; } }

        byte[] convertToByteArray(Color[,] pixels, Vector2 size)
        {
            List<byte> result = new List<byte>();
            for (int y = 0; y < size.y; y++)
                for (int x = 0; x < size.x; x++)
                    result.AddRange(pixels[x, y].ToArray());

            return result.ToArray();
        }

        void SetFilterMode()
        {
            Gl.Get(Gl.MAX_TEXTURE_MAX_ANISOTROPY, out float aniso);
            int minfilter = 0;
            int magfilter = 0;
            switch (filterMode) {
                case FilterMode.Nearest: minfilter = (mipmapCount > 0) ? Gl.NEAREST_MIPMAP_LINEAR : Gl.NEAREST; magfilter = Gl.NEAREST;  break;
                case FilterMode.Bilinear: minfilter = (mipmapCount > 0) ? Gl.LINEAR_MIPMAP_NEAREST : Gl.NEAREST; magfilter = Gl.LINEAR; break;
                case FilterMode.Trilinear: minfilter = (mipmapCount > 0) ? Gl.LINEAR_MIPMAP_LINEAR : Gl.LINEAR; magfilter = Gl.LINEAR;  break;
            }

            Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, ref minfilter);
            OpenGL_Renderer.TestForGLErrors();
            Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, ref magfilter);
            OpenGL_Renderer.TestForGLErrors();

            
            float samples = (float)AnisotropicSamples;
            samples = samples > 0 ? samples : 1;
            if (samples > aniso)
                throw new NotSupportedException($"The current system only supports a maxium of {aniso} Anisotropic samples.");
            Gl.TexParameterf(TextureTarget.Texture2d, (TextureParameterName)0x84FE, ref samples);
            OpenGL_Renderer.TestForGLErrors();
        }

        public override void Apply()
        {
            Console.WriteLine($"Sending {size.x * size.y * 4} bytes of texture data to GPU");

            if (pointer == 0)
                GenerateHandle();

            UploadToGPU_Tex();

            SetFilterMode();
            SetWrapMode();

            if (mipmapCount > 0)
            {
                Console.WriteLine($"Generating: {mipmapCount} mipmaps");
                Gl.GenerateMipmap(TextureTarget.Texture2d);
                OpenGL_Renderer.TestForGLErrors();
            }
        }

        void SetMipMapCount(int count)
        {
            int MaxMipMaps = CalcMaxNumberMipmaps(size);
            MaxMipMaps = (MaxMipMaps > Renderer.Max_Mipmap_Levels) ? Renderer.Max_Mipmap_Levels : MaxMipMaps;
            mipmapCount = (count < MaxMipMaps) ? count : MaxMipMaps;
        }

        void SetWrapMode()
        {
            if (WrapMode != WrapMode.None)
            {
                int repeat = Gl.REPEAT;
                Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, ref repeat);
                OpenGL_Renderer.TestForGLErrors();
                Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, ref repeat);
                OpenGL_Renderer.TestForGLErrors();
            }
        }

        void UploadToGPU_Tex()
        {
            Gl.BindTexture(TextureTarget.Texture2d, pointer);
            byte[] pixelData = convertToByteArray(pixels, size);
            using (MemoryLock ml = new MemoryLock(pixelData))
            Gl.TexImage2D(TextureTarget.Texture2d, 0, OpenGL_Utility.Convert(InternalFormat), 
               (int)size.x, (int)size.y, 0, OpenGL_Utility.Convert(PixelFormat), OpenGL_Utility.Convert(PixelType), ml.Address);

            Gl.TexParameterI(TextureTarget.Texture2d, TextureParameterName.TextureMaxLevel, new int[] {1 + mipmapCount});
            OpenGL_Renderer.TestForGLErrors();
        }

        void GenerateHandle()
        {
            pointer = Gl.GenTexture();

        }

        public override bool Compare(ITexture tex1)
        {
            return Pointer == ((IOpenGL_Texture)tex1).Pointer;
        }

        public override Color GetPixel(int x, int y) => pixels[x, y];   

        public override void SetPixel(int x, int y, Color color) => pixels[x,y] = color;
    }
}
