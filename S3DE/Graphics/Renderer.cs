using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics
{
    
    public static partial class Renderer
    {
        static S3DE_Vector2 renderRes;
        public static S3DE_Vector2 Resolution => renderRes;
        internal static void Init(S3DE_Vector2 renderRes)
        {
            Renderer.renderRes = renderRes;
            InitGlew();
        }
    }
}
