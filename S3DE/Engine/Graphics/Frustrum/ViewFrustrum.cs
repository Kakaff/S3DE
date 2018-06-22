using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.Frustrum
{
    public abstract class ViewFrustrum
    {
        public abstract bool Test(BoundingBox bb);
    }
}
