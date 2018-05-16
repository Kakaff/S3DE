using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static S3DE.Engine.Enums;

namespace S3DE.Engine.Graphics.Textures
{
    public interface ITexture
    {
        void Bind(TextureUnit tu);
        TextureUnit Bind();
        bool IsBound(out TextureUnit texUnit);
        void SetIsBound(bool value, TextureUnit texUnit);
        bool Compare(ITexture tex1);

        TextureUnit BoundTextureUnit { get; }
        InternalFormat InternalFormat {get;}
        PixelFormat PixelFormat {get;}
        PixelType PixelType {get;}
    }
}
