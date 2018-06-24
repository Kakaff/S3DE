using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.Textures
{
    interface ITexture2D : ITexture
    {
        S3DE_Vector2 Resolution {get;}
        FilterMode FilterMode {get;}
        int MipMapLevels {get;}
    }
}
