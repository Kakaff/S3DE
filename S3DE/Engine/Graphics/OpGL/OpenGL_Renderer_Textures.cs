using OpenGL;
using S3DE.Engine.Graphics.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL
{
    public sealed partial class OpenGL_Renderer : Renderer
    {
        int textureUnits;

        protected override void BindTexUnit(ITexture tex, TextureUnit tu)
        {
            Gl.ActiveTexture(OpenGL.TextureUnit.Texture0 + (int)tu);
            TestForGLErrors();
            Gl.BindTexture(TextureTarget.Texture2d, ((IOpenGL_Texture)tex).Pointer);
            TestForGLErrors();
        }
        protected override void UnbindTexUnit(TextureUnit textureUnit)
        {
            Gl.ActiveTexture(OpenGL.TextureUnit.Texture0 + (int)textureUnit);
            TestForGLErrors();
            Gl.BindTexture(TextureTarget.Texture2d, 0);
            TestForGLErrors();
        }
    }
}
