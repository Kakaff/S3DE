using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL.DC
{
    class UseShaderProgramCommand : IDrawCallCommand
    {
        private UseShaderProgramCommand() { }

        OpenGL_ShaderProgram shad;

        internal UseShaderProgramCommand(OpenGL_ShaderProgram s)
        {
            shad = s;
        }

        public void OnAdd(DrawCall dc) => dc.SetShaderProg(shad);
        public void Dispose() => shad = null;
        public void Perform() => shad.UseProgram();
    }
}
