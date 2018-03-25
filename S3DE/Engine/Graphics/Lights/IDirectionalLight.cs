using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.Lights
{
    public interface IDirectionalLight : ILight
    {
        Vector3 LightDirection {get;}
        bool CastsShadows {get;}
    }
}
