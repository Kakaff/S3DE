using S3DE.Engine.Graphics.Lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S3DE.Engine.Graphics;
using S3DE.Maths;

namespace S3DE.Engine.Entities.Components
{
    public abstract class DirectionalLight : EntityComponent, ILight, IDirectionalLight
    {
        public Color Color { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Intensity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Vector3 LightDirection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
