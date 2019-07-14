using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S3DE.Graphics.Shaders;
using S3DE.Graphics.Shaders.Sources;

namespace S3DE.Graphics.Screen
{
    public sealed class SolidColorScreenQuadMaterial : ScreenQuadMaterial
    {
        static SolidColorScreenQuadMaterial instance;

        public static SolidColorScreenQuadMaterial GetInstance() {
            if (instance == null)
                instance = new SolidColorScreenQuadMaterial();

            return instance;
        }

        protected override ShaderSource[] ShaderSources => new ShaderSource[]{
            new SolidColorScreenQuadMaterialVertexSource(),
            new SolidColorScreenQuadMaterialFragmentSource()
        };

        protected override void OnCompilationSuccess()
        {

        }

        protected override void UpdateUniforms()
        {

        }
    }
}
