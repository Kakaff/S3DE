using S3DE.Engine.Graphics.Materials;
using S3DE.Engine.Graphics.OpGL.DC;
using S3DE.Engine.Graphics.Textures;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL
{
    class OpenGL_Material_DC : Renderer_Material {

    OpenGL_ShaderProgram prog;

    MaterialSource VertexShader, FragmentShader;
    Dictionary<string, int> Uniforms;

    protected override void Compile()
    {
        prog = new OpenGL_ShaderProgram(OpenGL_Shader.Create(VertexShader), OpenGL_Shader.Create(FragmentShader));
        prog.Compile();
        identifier = prog.Pointer;
    }

    internal OpenGL_Material_DC()
    {
        Uniforms = new Dictionary<string, int>();
    }

    protected override void UseMaterial()
    {
        DrawCallSorter.SetCurrentDrawCall(new DrawCall());
        DrawCallSorter.AddCommandToCurrent(new UseRendererMaterialCommand(this));
    }

    protected override void SetSource(MaterialSource source)
    {
        switch (source.Stage)
        {
            case ShaderStage.Vertex:
                {
                    VertexShader = source;
                    break;
                }
            case ShaderStage.Fragment:
                {
                    FragmentShader = source;
                    break;
                }
            default:
                {
                    throw new NotImplementedException(source.Stage + " is not yet implemented/supported");
                }
        }
    }

        protected override void SetUniformf(string uniformName, float[] value) =>
                 DrawCallSorter.AddCommandToCurrent(new SetFloatArrayCommand((uint)prog.GetUniformLocation(uniformName), value));

        protected override void SetUniformf(string uniformName, float value) =>
                 DrawCallSorter.AddCommandToCurrent(new SetFloatCommand((uint)prog.GetUniformLocation(uniformName), value));

        protected override void SetUniformi(string uniformName, int value) =>
                DrawCallSorter.AddCommandToCurrent(new SetIntegerCommand((uint)prog.GetUniformLocation(uniformName), value));

        protected override void SetUniform(string uniformName, Matrix4x4 m) =>
                DrawCallSorter.AddCommandToCurrent(new SetMatrix4x4Command((uint)prog.GetUniformLocation(uniformName), m));

        protected override void SetUniform(string uniformName, Vector3 v) =>
                DrawCallSorter.AddCommandToCurrent(new SetVector3Command((uint)prog.GetUniformLocation(uniformName), v));

    protected override void AddUniform(string uniformName) => prog.AddUniform(uniformName);

    protected override void SetTexture(string uniformName, TextureUnit textureUnit, ITexture texture)
    {
        DrawCallSorter.AddCommandToCurrent(new SetTextureSamplerCommand((uint)prog.GetUniformLocation(uniformName), texture as IOpenGL_Texture));
    }
}
}
