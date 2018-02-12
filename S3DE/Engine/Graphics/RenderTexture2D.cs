using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static S3DE.Engine.Enums;

namespace S3DE.Engine.Graphics
{
    public abstract class RenderTexture2D
    {
        public abstract Vector2 Size { get; }
        public abstract Texture2D ToTexture2D();
        public abstract FilterMode FilterMode {get;}
        public abstract InternalFormat InternalFormat {get;}
        public abstract PixelFormat PixelFormat {get;}
        public abstract PixelType PixelType{get;}
        public abstract void Bind(int TextureUnit);
        public abstract void Bind();
        public abstract void Unbind();
    }
}
