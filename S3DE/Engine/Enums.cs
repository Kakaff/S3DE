using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine
{
    public static class Enums
    {
        public enum FilterMode
        {
            Nearest,
            Linear,
            Anisotropic
        }

        public enum Space
        {
            Local,
            World
        }

        public enum RenderingAPI
        {
            OpenGL,
            Vulkan,
            DirectX
        }
    }
}
