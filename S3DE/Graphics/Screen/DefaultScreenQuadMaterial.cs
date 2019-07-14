using S3DECore.Math;
using S3DECore.Graphics.Shaders;
using S3DE.Graphics.Shaders;
using S3DE.Graphics.Shaders.Sources;
using S3DECore.Graphics.Textures;

namespace S3DE.Graphics.Screen
{
    /// <summary>
    /// A simple material that draws a texture to the screen
    /// </summary>
    public sealed class DefaultScreenQuadMaterial : ScreenQuadMaterial
    {
        static DefaultScreenQuadMaterial instance;
        public static DefaultScreenQuadMaterial Instance {
            get { if (instance == null) instance = new DefaultScreenQuadMaterial(); return instance; } }

        private DefaultScreenQuadMaterial() { }
        
        public RenderTexture2D Tex { set; private get; }

        int texLoc;
        
        protected override ShaderSource[] ShaderSources => new ShaderSource[] {
            new ScreenQuadVertexSource(),new ScreenQuadFragmentSource()};

        protected override void UpdateUniforms()
        {
            SetUniform(texLoc, Tex);
        }

        protected override void OnCompilationSuccess()
        {
            texLoc = GetUniformLocation("tex");
        }
    }
}
