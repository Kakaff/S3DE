using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Shaders
{
    public abstract class ShaderSource
    {
        public abstract ShaderStage Stage { get; }
        public abstract string Source { get; }
    }
}
