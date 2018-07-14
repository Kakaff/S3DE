using S3DE.Engine.Graphics.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.Shaders
{
    public abstract class ShaderSource
    {
        ShaderStage stage;
        string source;

        public ShaderStage Stage => stage;
        public string Source { get => source; protected set => source = value; }

        private ShaderSource() { }

        protected ShaderSource(ShaderStage stage) => this.stage = stage;

        protected void SetSource(params string[] src)
        {
            source = String.Join("\n", src);
        }
    }
}
