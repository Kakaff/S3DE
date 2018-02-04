using OpenGL;
using S3DE.Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S3DE.Maths;

namespace SampleGame.Sample_OGL_Renderer
{
    class OpenGL_Texture2D : Texture2D
    {
        int mipmapCount;
        uint pointer;
        Color[,] pixels;
        Vector2 size;
        FilterMode filterMode;
        AnisotropicSamples samples;

        public override int MipMapLevels
        {
            get => mipmapCount;
            set => SetMipMapCount(value);
        }
        internal OpenGL_Texture2D(int width, int height)
        {
            size = new Vector2(width, height);
            pixels = new Color[width, height];
        }

        public override Vector2 Size => size;

        public override FilterMode FilterMode { get => filterMode; set => filterMode = value;}
        public override AnisotropicSamples AnisotropicSamples { get => samples; set => samples = value; }

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
                case FilterMode.Nearest: minfilter = Gl.NEAREST_MIPMAP_LINEAR; magfilter = Gl.NEAREST;  break;
                case FilterMode.Bilinear: minfilter = Gl.LINEAR_MIPMAP_NEAREST; magfilter = Gl.LINEAR; break;
                case FilterMode.Trilinear: minfilter = Gl.LINEAR_MIPMAP_LINEAR; magfilter = Gl.LINEAR;  break;
            }

            Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, ref minfilter);
            OpenGL_Renderer.TestForGLErrors();
            Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, ref magfilter);
            OpenGL_Renderer.TestForGLErrors();

            float samples = (float)AnisotropicSamples;
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

            if (Renderer.API_Version >= 420)
                UploadToGPU_TexStorage();
            else
                UploadToGPU_Tex();

            SetFilterMode();
            SetWrapMode();
            Gl.GenerateMipmap(TextureTarget.Texture2d);
            OpenGL_Renderer.TestForGLErrors();
        }

        void SetMipMapCount(int count)
        {
            int MaxMipMaps = CalcMaxNumberMipmaps(size);
            MaxMipMaps = (MaxMipMaps > Renderer.Max_Mipmap_Levels) ? Renderer.Max_Mipmap_Levels : MaxMipMaps;
            mipmapCount = (count < MaxMipMaps) ? count : MaxMipMaps;
        }

        void SetWrapMode()
        {
            int repeat = Gl.REPEAT;
            Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, ref repeat);
            OpenGL_Renderer.TestForGLErrors();
            Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, ref repeat);
            OpenGL_Renderer.TestForGLErrors();
        }

        void UploadToGPU_Tex()
        {
            Gl.BindTexture(TextureTarget.Texture2d, pointer);
            byte[] pixelData = convertToByteArray(pixels, size);
            using (MemoryLock ml = new MemoryLock(pixelData))
            Gl.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba8, 
               (int)size.x, (int)size.y, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ml.Address);

            Gl.TexParameterI(TextureTarget.Texture2d, TextureParameterName.TextureMaxLevel, new int[] {1 + mipmapCount});
            OpenGL_Renderer.TestForGLErrors();
        }

        void UploadToGPU_TexStorage()
        {
            Gl.BindTexture(TextureTarget.Texture2d, pointer);
            OpenGL_Renderer.TestForGLErrors();

            byte[] pixelData = convertToByteArray(pixels, size);
            using (MemoryLock ml = new MemoryLock(pixelData))
                Gl.TexSubImage2D(TextureTarget.Texture2d, 0, 0, 0, (int)size.x, (int)size.y, 
                    PixelFormat.Rgba, PixelType.UnsignedByte, ml.Address);

            OpenGL_Renderer.TestForGLErrors();
        }

        void GenerateHandle()
        {
            pointer = Gl.GenTexture();

            if (Renderer.API_Version >= 420)
            {
                Gl.BindTexture(TextureTarget.Texture2d, pointer);
                Gl.TexStorage2D(TextureTarget.Texture2d,
                    1 + mipmapCount,
                    InternalFormat.Rgba8, (int)size.x, (int)size.y);
            }

        }

        public override void Bind(int textureunit)
        {
            if (pointer == 0)
                GenerateHandle();
            
            Gl.ActiveTexture(TextureUnit.Texture0 + textureunit);
            Gl.BindTexture(TextureTarget.Texture2d, pointer);
            
        }

        public override Color GetPixel(int x, int y) => pixels[x, y];   

        public override void SetPixel(int x, int y, Color color) => pixels[x,y] = color;
    }
}
