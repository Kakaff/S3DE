using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.Lights
{
    public class AmbientLight : ILight
    {
        Color c;
        float i;

        public Color Color {get => c; set => c = value;}
        public float Intensity {get => i; set => i = value;}

        public AmbientLight(Color color, float intensity)
        {
            Color = color;
            Intensity = intensity;
        }
    }
}
