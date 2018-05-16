using S3DE.Engine.Graphics.Textures;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static S3DE.Engine.Enums;

namespace S3DE.Engine.Graphics
{
    public abstract class RenderTexture2D : ITexture
    {
        bool isBound;
        TextureUnit boundTexUnit;

        public abstract Vector2 Size { get; }
        public abstract Texture2D ToTexture2D();
        public TextureUnit BoundTextureUnit => boundTexUnit;
        public abstract FilterMode FilterMode {get;}
        public abstract InternalFormat InternalFormat {get;}
        public abstract PixelFormat PixelFormat {get;}
        public abstract PixelType PixelType{get;}
        public void Bind(TextureUnit tu) => TextureUnits.BindTextureUnit(this, tu);
        public TextureUnit Bind() => TextureUnits.BindTexture(this);
        public abstract bool Compare(ITexture tex1);

        public bool IsBound(out TextureUnit texUnit)
        {
            texUnit = boundTexUnit;
            return isBound;
        }

        public void SetIsBound(bool value, TextureUnit texUnit)
        {
            isBound = value;
            boundTexUnit = value ? texUnit : TextureUnit.Null;
        }
    }
}
