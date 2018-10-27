using S3DE.Engine.Entities.Components;
using S3DE.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
