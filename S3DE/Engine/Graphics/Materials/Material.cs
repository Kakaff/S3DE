using S3DE.Engine.Collections;
using S3DE.Engine.Entities;
using S3DE.Engine.Entities.Components;
using S3DE.Engine.Graphics.Lights;
using S3DE.Engine.Graphics.Shaders;
using S3DE.Engine.Graphics.Textures;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.Materials
{
    public enum ShaderStage
    {
        Vertex,
        Tessalation,
        Tessalation_Evaluation,
        Geometry,
        Fragment
    }

    public abstract class Material
    {
        //Dictionary for shaders.

        //On creation, see if the shaders already exist.
        ShaderProgram deferredShader;
        ShaderProgram forwardShader;

        ShaderProgram activeShader;

        public ShaderProgram Shader => activeShader;

        MeshRenderer activeMeshRenderer;
        protected Transform transform => activeMeshRenderer.transform;

        protected abstract ShaderSource[] GetShaderSources(RenderPass pass);

        bool isCreated;
        
        protected Material()
        {

        }
        
        
        void CheckIsCreated()
        {
            if (!isCreated)
            {
                Console.WriteLine($"Creating RendererMaterials for {this.GetType().Name} in {this.GetType().Namespace}");
                deferredShader = Renderer.Create_ShaderProgram();
                foreach (ShaderSource ss in GetShaderSources(RenderPass.Deferred))
                    deferredShader.SetSource(ss.Stage, ss.Source);

                deferredShader.Compile();
                isCreated = true;
            }

        }

        public void UseMaterial(RenderPass pass,MeshRenderer mr)
        {
            activeMeshRenderer = mr;
            CheckIsCreated();
            deferredShader.Bind();
            activeShader = deferredShader;
            UpdateUniforms(pass);
        }
        
        protected abstract void UpdateUniforms(RenderPass pass);
        
    }
}
