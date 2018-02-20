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
        void Bind();
        void Bind(int textureUnit);
        void Unbind();
        InternalFormat InternalFormat {get;}
        PixelFormat PixelFormat {get;}
        PixelType PixelType {get;}
    }
}
