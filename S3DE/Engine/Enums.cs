using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine
{
    public static class Enums
    {
        public enum PixelFormat
        {
            Red,
            RGB,
            RGBA,
            DepthStencil,
            Depth,
            Uint
        }

        public enum PixelType
        {
            Float16,
            Float32,
            UByte,
            UInt16,
            UInt32,
            UInt24_8
        }
        
        public enum InternalFormat
        {
            R,
            R16,
            R16UI,
            R16F,
            R16I,
            RGB,
            RGB8,
            RGB10,
            RGB16F,
            RGBA,
            RGB5_A1,
            RGB10_A2,
            Depth24_Stencil8,
            Depth32F_Stencil8,
            DepthComponent16,
            DepthComponent24,
            Depth_Component32
        }

        public enum Space
        {
            Local,
            World
        }

        public enum RenderingAPI
        {
            OpenGL,
        }
    }
}
