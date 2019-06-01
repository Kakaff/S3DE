using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Textures
{
    public interface IRenderTexture
    {
        
        int Width { get; }
        int Height { get; }
        int MipmapCount { get; }

        PixelFormat PixelFormat { get; }
        InternalFormat InternalFormat { get; }
        PixelType PixelType { get; }
        int BoundTexUnit { get; }
        bool IsBound { get; }
        int Bind();
        int GetInstanceID();
        IntPtr Handle { get; }
    }
}
