using S3DE.Engine.Entities.Components;
using S3DE.Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Entities
{
    public class MeshRenderer : EntityComponent
    {
        Renderer_MeshRenderer api_mr;
        Mesh m;

        Material mat;
        uint passes = 0;

        bool deferred = true;
        bool forward = false;
        bool shadows = false;

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
                if (mat != null && m != null)
                {
                    mat.UseMaterial();
                    if (mat.UsesTransformMatrix)
                        mat.SetTransformMatrix(gameEntity.transform.WorldTransformMatrix);
                    if (mat.UsesViewMatrix)
                        mat.SetViewMatrix(Scene.ActiveCamera.ViewMatrix);
                    if (mat.UsesProjectionMatrix)
                        mat.SetProjectionMatrix(Scene.ActiveCamera.ProjectionMatrix);
                    if (mat.UsesRotationMatrix)
                        mat.SetRotationMatrix(gameEntity.transform.WorldRotationMatrix);
                    mat.UpdateUniforms_Internal();
                    api_mr.Render_Internal();
                }
            }
        }

        protected override void OnCreation() {
            api_mr = Renderer.CreateMeshRenderer_Internal();
            passes ^= (uint)RenderPass.Deferred;
        }

        protected override void OnEnable()
        {
            //Add to scene meshrenderer collection.
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            //Remove from scene meshrenderer collection.
            base.OnDisable();
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
