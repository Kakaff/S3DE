using S3DE.Components;
using S3DE.Graphics.Materials;
using S3DE.Graphics.Meshes;
using S3DE.Graphics.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Components
{
    public sealed class Meshrenderer : EntityComponent
    {
        public Mesh Mesh { get; set; }
        public Material Material { get; set; }

        Drawcall _drawcall;
        
        protected override void Render()
        {
            if (_drawcall == null)
                _drawcall = new Drawcall(this);
            

            if (_drawcall.Validate()) //If the drawcall has a non empty mesh and a material, it is considered valid.
            {
                Material.SetTargetDrawcall(_drawcall);
                Material.SetTargetTransform(transform);
                Material.UpdateUniforms_Internal();
                
                if (_drawcall.TexturesChanged || _drawcall.MaterialChanged)
                    Scene.GetRenderPass(Material.TargetRenderPass).AddDrawcall(_drawcall);
                    
                //Material.UseMaterial();
                //_drawcall.PerformUniformUpdates();
                //Mesh.Draw();
                

            }
        }


    }
}
