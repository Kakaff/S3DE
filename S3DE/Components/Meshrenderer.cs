using S3DE.Graphics;
using S3DE.Graphics.Materials;
using S3DECore.Graphics;

namespace S3DE.Components
{
    public sealed class Meshrenderer : EntityComponent
    {
        public Mesh Mesh { get => m; set { m = value; UpdateDrawcall(); } }
        public Material Material { get => mat; set { mat = value; UpdateDrawcall(); } }
        public Renderpass TargetRenderpass { get => trgRenderpass; set { trgRenderpass = value; UpdateDrawcall(); } }

        Drawcall drawcall;
        Renderpass trgRenderpass;
        Material mat;
        Mesh m;

        void UpdateDrawcall()
        {
            if (trgRenderpass == null && drawcall.ParentContainer != null)
                drawcall.RemoveFromRenderpass();
            else if (trgRenderpass != null)
            {
                if (drawcall.ParentContainer != null)
                {
                    //Check if Material or Renderpass changed.
                    //Or if Material or Mesh has been set to null.

                    if (drawcall.ParentContainer.RenderpassID != trgRenderpass.ID)
                        drawcall.RemoveFromRenderpass();
                    else if (Material != null && Material.ShaderProgramID != drawcall.ParentContainer.ShaderProgramID)
                        drawcall.RemoveFromRenderpass();
                    else if (Material == null || Mesh == null)
                        drawcall.RemoveFromRenderpass();
                }

                if (Mesh != null && Material != null)
                    drawcall.AddToRenderpass(trgRenderpass);
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

        protected override void Render()
        {

        }

        protected override void OnCreation()
        {
            drawcall = new Drawcall(this);
        }
    }
}
