using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S3DE.Maths;

namespace S3DE.Engine.Graphics
{
    public abstract class Texture2D
    {
        public abstract Vector2 Size { get; }

        public abstract void Apply();
        
        public abstract void Bind(int textureunit);

        public abstract Color GetPixel(int x, int y);

        public abstract void SetPixel(int x, int y, Color color);

        protected Texture2D() { }

        public static Texture2D Create(int width, int height) => Renderer.CreateTexture2D(width, height);
    }
}
