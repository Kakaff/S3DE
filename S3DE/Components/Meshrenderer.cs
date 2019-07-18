using S3DE.Graphics;
using S3DE.Graphics.Materials;
using S3DECore.Graphics;

namespace S3DE.Components
{
    public sealed class Meshrenderer : EntityComponent
    {
        public Mesh Mesh { get => m; set { m = value; UpdateDrawcall(); } }
        public Material Material { get => mat; set { mat = value; UpdateDrawcall(); } }
        //public Renderpass TargetRenderpass { get => trgRenderpass; set { trgRenderpass = value; UpdateDrawcall(); } }

        Drawcall drawcall;
        Material mat;
        Mesh m;

        void UpdateDrawcall()
        {
            if (drawcall.ParentContainer != null)
            {
                if (Material != null && 
                    (Material.RenderpassChanged || Material.ShaderProgramID != drawcall.ParentContainer.ShaderProgramID))
                    drawcall.RemoveFromRenderpass();
                else if (Material == null || Mesh == null)
                    drawcall.RemoveFromRenderpass();
            }

            if (Material != null && Mesh != null)
            {
                if (Material.TargetRenderpass == null)
                    throw new System.Exception("Material does not have a target renderpass!");

                drawcall.AddToRenderpass(Material.TargetRenderpass, Scene);
            }
            
        }

        internal void Draw()
        {
            if (Material != null && Mesh != null)
            {
                Material.UseMaterial();
                Material.SetTargetTransform(transform);
                Material.UpdateUniforms_Internal();
                Mesh.Draw();
            }
        }

        protected override void PreRender()
        {
            if (Material.RenderpassChanged)
                UpdateDrawcall();
        }

        protected override void OnCreation()
        {
            drawcall = new Drawcall(this);
        }
    }
}
