using S3DE.Graphics;

namespace S3DE.Components
{
    public class MeshRenderer : EntityComponent
    {
        Mesh m;
        Material mat;

        public Mesh Mesh
        {
            get => m;
            set => m = value;
        }

        public Material Material
        {
            get => mat;
            set => mat = value;
        }

        protected override void Render()
        {
            if (m != null && mat != null)
            {
                mat.SetTransform(transform);
                mat.UpdateUniforms_Internal();
                m.Draw();
                mat.SetTransform(null);
            }
        }

        public MeshRenderer SetMaterial(Material mat)
        {
            this.mat = mat;
            return this;
        }

        public MeshRenderer SetMesh(Mesh m)
        {
            this.m = m;
            return this;
        }
    }
}
