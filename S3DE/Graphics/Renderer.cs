using S3DE.Maths;

namespace S3DE.Graphics
{

    public static partial class Renderer
    {
        static Vector2 renderRes;
        public static Vector2 Resolution => renderRes;
        internal static void Init(Vector2 renderRes)
        {
            Renderer.renderRes = renderRes;
            InitGlew();
        }
    }
}
