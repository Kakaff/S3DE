using S3DE.Engine.Graphics;
using S3DE.Engine.Graphics.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Entities.Components
{
    public sealed class DrawCall_MeshRenderer : EntityComponent
    {

        Renderer_MeshRenderer api_mr;
        Mesh m;

        Material mat;
        uint passes = 0;

        bool deferred = true;
        bool forward = false;
        bool shadows = false;

        public Renderer_MeshRenderer API_MeshRenderer => api_mr;
        public bool UseDeferredRendering
        {
            get => deferred;
            set => SetDeferredRenderer(value);
        }

        public bool UseForwardRendering
        {
            get => forward;
            set => SetForwardRenderer(value);
        }

        public bool CastShadows
        {
            get => shadows;
            set => SetCastsShadows(value);
        }

        public Mesh mesh
        {
            set
            {
                m = value;
                api_mr.SetMesh_Internal(value);
            }
        }

        public Material Material
        {
            set => mat = value;
            get => mat;

        }

        protected override void PreRender() => api_mr.Prepare_Internal();

        protected override void Render()
        {
            if (Uses(Renderer.CurrentRenderPass))
            {
                if (mat != null && m != null && mat.SupportsRenderPass(Renderer.CurrentRenderPass))
                {
                    mat.UseMaterial(Renderer.CurrentRenderPass);
                    
                    if (mat.UsesTransformMatrix)
                        //mat.SetTransformMatrix_DC(gameEntity.transform.WorldTransformMatrix);
                    if (mat.UsesViewMatrix)
                        //mat.SetViewMatrix_DC(Scene.ActiveCamera.ViewMatrix);
                    if (mat.UsesProjectionMatrix)
                        //mat.SetProjectionMatrix_DC(Scene.ActiveCamera.ProjectionMatrix);
                    if (mat.UsesRotationMatrix)
                        //mat.SetRotationMatrix_DC(gameEntity.transform.WorldRotationMatrix);
                    
                    api_mr.Render_DC_Internal();
                }
                else if (mat != null && !mat.SupportsRenderPass(Renderer.CurrentRenderPass))
                {
                    throw new InvalidOperationException($"Material {mat.GetType().Name} does not support " +
                        $"{Renderer.CurrentRenderPass} but is attached to a MeshRenderer set to use this RenderPass");
                }
            }
        }

        protected override void OnCreation()
        {
            api_mr = Renderer.CreateMeshRenderer_Internal();
            passes ^= (uint)RenderPass.Deferred;
        }

        void SetDeferredRenderer(bool value)
        {
            if (value != UseDeferredRendering)
            {
                deferred = value;
                passes ^= (uint)RenderPass.Deferred;
            }
        }

        void SetForwardRenderer(bool value)
        {
            if (value != UseForwardRendering)
            {
                forward = value;
                passes ^= (uint)RenderPass.Forward;
            }
        }

        void SetCastsShadows(bool value)
        {
            if (value != CastShadows)
            {
                shadows = value;
                passes ^= (uint)RenderPass.ShadowMap;
            }
        }

        public bool Uses(RenderPass pass) => (passes & (uint)pass) == (uint)pass && (uint)pass > 0;
    }
}
