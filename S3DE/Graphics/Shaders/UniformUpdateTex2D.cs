using S3DE.Graphics.Textures;
using S3DECore.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Shaders
{
    internal class UniformUpdateTex2D : UniformUpdate
    {

        public override UniformType UniformType => UniformType.TextureSampler2D;

        public RenderTexture2D Texture { get; set; }
        public int TextureIndex { get; private set; }

        public UniformUpdateTex2D(int location,int texIndex) : base(location) { TextureIndex = texIndex;  }

        public UniformUpdateTex2D(int location,int texIndex, RenderTexture2D tex) : base(location)
        {
            this.Texture = tex;
            TextureIndex = texIndex;
        }

        public override void Perform()
        {
            S3DECore.Graphics.Uniforms.SetUniform1i(UniformLocation, Texture.Bind());
        }
    }
}
