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
            set => api_mr.SetMesh_Internal(value);
        }

        protected override void Draw()
        {
            mat.SetTransformMatrix(gameEntity.transform.WorldTransformMatrix);
            mat.SetViewMatrix(Camera.ActiveCamera.ViewMatrix);
            mat.SetProjectionMatrix(Camera.ActiveCamera.ProjectionMatrix);

            base.Draw();
        }
        protected override void OnCreation() => api_mr = Renderer.CreateMeshRenderer();
    }
}
