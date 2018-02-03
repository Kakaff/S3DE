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

        internal OpenGL_Texture2D(int width, int height)
        {
            size = new Vector2(width, height);
            pixels = new Color[width, height];
        }

        public override Vector2 Size => size;

        byte[] convertToByteArray(Color[,] pixels, Vector2 size)
        {
            List<byte> result = new List<byte>();
            for (int y = 0; y < size.y; y++)
                for (int x = 0; x < size.x; x++)
                    result.AddRange(pixels[x, y].ToArray());

            return result.ToArray();
        }

        public override void Apply()
        {
            if (pointer == 0)
                GenerateHandle();

            int repeat = Gl.REPEAT;
            int linear = Gl.LINEAR;

            Gl.BindTexture(TextureTarget.Texture2d, pointer);
            Gl.TexParameteri(TextureTarget.Texture2d,TextureParameterName.TextureWrapS, ref repeat);
            Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, ref repeat);
            Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, ref linear);
            Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, ref linear);

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

            Gl.ActiveTexture(TextureUnit.Texture0);
            Gl.BindTexture(TextureTarget.Texture2d, pointer);
            
        }

        public override Color GetPixel(int x, int y) => pixels[x, y];   

        public override void SetPixel(int x, int y, Color color) => pixels[x,y] = color;
    }
}
