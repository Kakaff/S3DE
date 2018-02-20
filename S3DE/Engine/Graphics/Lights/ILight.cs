using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.Lights
{
    public interface ILight
    {
        Color Color {get;}
        float Intensity {get;}
    }
}
