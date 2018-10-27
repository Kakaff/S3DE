using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Entities.Components
{
    public interface IRenderLogic
    {
        void PreRender();
        void Render();
        void PostRender();
    }
}
