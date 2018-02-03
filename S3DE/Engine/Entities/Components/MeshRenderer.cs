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

        public Mesh mesh
        {
            set
            {
                m = value;
                api_mr.SetMesh_Internal(value);
            }
        }

        public Material material
        {
            set => mat = value;
            get => mat;
            
        }

        protected override void PreDraw() => api_mr.Prepare_Internal();

        protected override void Draw()
        {
            if (mat != null && m != null)
            {
                mat.UseMaterial();
                if (mat.UsesTransformMatrix)
                    mat.SetTransformMatrix(gameEntity.transform.WorldTransformMatrix);
                if (mat.UsesViewMatrix)
                    mat.SetViewMatrix(Camera.ActiveCamera.ViewMatrix);
                if (mat.UsesProjectionMatrix)
                    mat.SetProjectionMatrix(Camera.ActiveCamera.ProjectionMatrix);

                mat.UpdateUniforms_Internal();
                api_mr.Render_Internal();
            }
        }

        protected override void OnCreation() {
            api_mr = Renderer.CreateMeshRenderer();
        }
    }
}
