using S3DE.Engine.Graphics.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL
{
    public interface IOpenGL_Texture : ITexture
    {
        uint Pointer {get;}
    }
}
