using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL.DC
{
    class DrawCall
    {
        //Treelike structure, based on ShaderProgramID
        //Each ShaderProgramID then stores what Textures it uses.
        //Each TextureUsage container also has a container for other textures used by it.
        //And finally at the end is a DrawCall stored, which stores which entity to draw, which uniforms to set and such.
        //This way we minimize how often we need to rebind shaders and textures. so that Objects using the same shaderprogram and textures
        //All get drawn after eachother.
        //When Bla bla draw thingie is called the DrawCall is deemed finished and inserted into the tree.
        //So basicly when setting a uniform/binding a texture/etc. lump it into a Drawcall. Which is then performed on Renderer.FinalizeRenderStage()
        //But only when doing it from a Meshrenderer. Not when binding textures outside of that.

        OpenGL_Mesh mesh;
        OpenGL_ShaderProgram shaderProg;
        List<IDrawCallCommand> commands;
        List<IOpenGL_Texture> Textures;

        internal OpenGL_Mesh Mesh => mesh;
        internal int TextureBindings => Textures.Count;
        internal uint ShaderProgramIdentifier => shaderProg.Pointer;
        internal uint MeshIdentifier => mesh.VAO_Pointer;

        internal DrawCall()
        {
            commands = new List<IDrawCallCommand>();
            Textures = new List<IOpenGL_Texture>();
        }

        internal void AddTexture(IOpenGL_Texture tex) => Textures.Add(tex);
        internal void SetShaderProg(OpenGL_ShaderProgram shad) => shaderProg = shad;
        internal void SetMesh(OpenGL_Mesh m) => mesh = m;

        internal void Perform()
        {
            foreach (IDrawCallCommand com in commands)
            {
                com.Perform();
            }
        }

        internal void AddCommand(IDrawCallCommand com)
        {
            commands.Add(com);
            com.OnAdd(this);
        }

        internal void Dispose()
        {
            mesh = null;
            shaderProg = null;

            foreach (IDrawCallCommand dcc in commands)
                dcc.Dispose();

            commands.Clear();
            commands = null;
        }


        internal int GetTextureBindingIdentifier(int index)
        {
            if (index > TextureBindings - 1)
                throw new ArgumentOutOfRangeException();

            return (int)Textures[index].Pointer;
        }
    }
}
