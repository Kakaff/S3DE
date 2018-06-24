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
    public class DirectionalLight : EntityComponent, ILight, IDirectionalLight, IEnableLogic
    {
        public Color Color { get => lightColor; set => lightColor = value;}
        public float Intensity { get => intensity; set => intensity = value;}
        public System.Numerics.Vector3 LightDirection { get => lightDir; set => lightDir = value;}
        public bool CastsShadows { get => shadows; set => shadows = value;}

        System.Numerics.Vector3 lightDir;
        float intensity;
        Color lightColor;
        bool shadows;

        protected override void OnCreation()
        {
            Scene.AddDirectionalLight(this as IDirectionalLight);
            lightDir = VectorExtensions.Vec3_Down;
            intensity = 1;
            lightColor = Color.White;
            shadows = false;
        }

        protected override void OnDestruction()
        {
            Scene.RemoveDirectionalLight(this as IDirectionalLight);
        }
        
        public void OnEnable()
        {
            Scene.AddDirectionalLight(this as IDirectionalLight);
        }

        public void OnDisable()
        {
            Scene.RemoveDirectionalLight(this as IDirectionalLight);
        }

    }
}
