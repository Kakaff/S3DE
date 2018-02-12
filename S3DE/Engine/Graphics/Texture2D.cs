using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S3DE.Maths;
using static S3DE.Engine.Enums;

namespace S3DE.Engine.Graphics
{
    public enum FilterMode
    {
        Nearest,
        Bilinear,
        Trilinear
    }

    public enum WrapMode
    {
        None,
        Wrap,
    }

    public enum AnisotropicSamples
    {
        x0 = 1,
        x2 = 2,
        x4 = 4,
        x8 = 8,
        x16 = 16,
    }

    public abstract class Texture2D
    {

        public abstract Vector2 Size { get; }

        public abstract FilterMode FilterMode { get; set; }
        public abstract AnisotropicSamples AnisotropicSamples { get; set;}
        public abstract WrapMode WrapMode {get; set;}
        public abstract PixelFormat PixelFormat {get; set;}
        public abstract PixelType PixelType { get; set;}
        public abstract InternalFormat InternalFormat {get; set;}
        public abstract int MipMapLevels { get; set; }
        public abstract void Apply();
        
        public abstract void Bind(int textureunit);

        public abstract Color GetPixel(int x, int y);

        public abstract void SetPixel(int x, int y, Color color);

        public virtual void CalculateMipMapCount() => MipMapLevels = CalcMaxNumberMipmaps(Size);

        protected Texture2D() { }

        public static Texture2D Create(int width, int height) => Renderer.CreateTexture2D_Internal(width, height);

        protected int CalcMaxNumberMipmaps(Vector2 size)
        {
            //Floor size to power of two.
            Vector2 floored = new Vector2(EngineMath.FloorToPowerOfTwo((int)size.x), EngineMath.FloorToPowerOfTwo((int)size.y));
            int smallest = (int)(floored.x > floored.y ? floored.x : floored.y);

            Console.WriteLine($"Smallest: {smallest}");
            return (int)(Math.Log(smallest) / Math.Log(2)) - 4;
        }
    }
}
