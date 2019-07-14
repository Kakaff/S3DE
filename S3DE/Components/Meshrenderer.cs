using S3DE.Graphics.Materials;
using S3DECore.Graphics;

namespace S3DE.Components
{
    public sealed class Meshrenderer : EntityComponent
    {
        public Mesh Mesh { get; set; }
        public Material Material { get; set; }
        
        protected override void Render()
        {
            if (Material != null && Mesh != null)
            {
                Material.UseMaterial();
                Material.SetTargetTransform(transform);
                Material.UpdateUniforms_Internal();
                Mesh.Draw();
            }
        }


    }
}
