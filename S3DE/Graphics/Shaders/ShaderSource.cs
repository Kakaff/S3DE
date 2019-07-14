using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Shaders
{
    public enum ShaderStage
    {
        Fragment = 0x8B30,
        Vertex = 0x8B31
    }

    public abstract class ShaderSource
    {
        public abstract ShaderStage Stage { get; }
        public abstract string Source { get; }
    }
}
