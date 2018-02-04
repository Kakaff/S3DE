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
        uint pointer;
        Color[,] pixels;
        Vector2 size;
        FilterMode filterMode;
        AnisotropicSamples samples;
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
            Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, ref magfilter);
            

            float samples = (float)AnisotropicSamples;
            if (samples > aniso)
                throw new NotSupportedException();
            Gl.TexParameterf(TextureTarget.Texture2d, (TextureParameterName)0x84FE, ref samples);
            Console.WriteLine((TextureParameterName)0x84FE);
            
        }

        public override void Apply()
        {
            if (pointer == 0)
                GenerateHandle();

            int repeat = Gl.REPEAT;
            

            Gl.BindTexture(TextureTarget.Texture2d, pointer);
            Gl.TexParameteri(TextureTarget.Texture2d,TextureParameterName.TextureWrapS, ref repeat);
            Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, ref repeat);

            SetFilterMode();
            

            OpenGL_Renderer.TestForGLErrors();
            //Convert pixels to byte array.
            byte[] byteData = convertToByteArray(pixels, size);
            Console.WriteLine($"Sending {byteData.Length} bytess of texture data to GPU");

            using (MemoryLock ml = new MemoryLock(byteData))
            {
                Gl.TexImage2D(TextureTarget.Texture2d, 0,
                InternalFormat.Rgba8, (int)Size.x, (int)Size.y, 0,
                PixelFormat.Rgba, PixelType.UnsignedByte, ml.Address);
            }

            OpenGL_Renderer.TestForGLErrors();

            Gl.GenerateMipmap(TextureTarget.Texture2d);
            OpenGL_Renderer.TestForGLErrors();
            //Upload the changes to the GPU.
        }

        void GenerateHandle()
        {
            pointer = Gl.GenTexture();
            OpenGL_Renderer.TestForGLErrors();
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
