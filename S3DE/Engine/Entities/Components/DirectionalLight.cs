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
    public class DirectionalLight : EntityComponent, ILight, IDirectionalLight
    {
        public Color Color { get => lightColor; set => lightColor = value;}
        public float Intensity { get => intensity; set => intensity = value;}
        public Vector3 LightDirection { get => lightDir; set => lightDir = value;}
        public bool CastsShadows { get => shadows; set => shadows = value;}

        Vector3 lightDir;
        float intensity;
        Color lightColor;
        bool shadows;

        protected override void OnCreation()
        {
            Scene.AddDirectionalLight(this as IDirectionalLight);
            lightDir = Vector3.Down;
            intensity = 1;
            lightColor = Color.White;
            shadows = false;
        }

        protected override void OnDestruction()
        {
            Scene.RemoveDirectionalLight(this as IDirectionalLight);
        }

        protected override void OnEnable()
        {
            Scene.AddDirectionalLight(this as IDirectionalLight);
        }

        protected override void OnDisable()
        {
            Scene.RemoveDirectionalLight(this as IDirectionalLight);
        }
    }
}
