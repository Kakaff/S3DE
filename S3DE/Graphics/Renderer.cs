using S3DE.Maths;

namespace S3DE.Graphics
{

    public enum ClearBufferBit
    {
        COLOR = 0x00004000,
        DEPTH = 0x00000100,
        STENCIL = 0x00000400
    }

    public enum GlEnableCap
    {
        DepthTest = 0x0B71,
        AlphaTest = 0x0BC0,
        CullFace = 0x0B44
    }

    public static partial class Renderer
    {
        static Vector2 renderRes;
        public static Vector2 Resolution => renderRes;
        static uint latestError = 0;
        public static bool NoError { get { latestError = Extern_CheckGLErrors(); return latestError == (uint)GL.NO_ERROR; } }
        public static uint LatestError => latestError;

        public static Vector2 DisplayResolution => new Vector2(1280, 720);
        public static Vector2 RenderResolution => new Vector2(320, 180);

        internal static void Init(Vector2 renderRes)
        {
            Renderer.renderRes = renderRes;
            InitGlew();
        }

        public static void Set_ViewPortSize(uint width, uint height)
        {
            Extern_SetViewPortSize(width, height);
        }

        public static void Enable_FaceCulling(bool v)
        {
            if (v)
                Extern_Enable((uint)GlEnableCap.CullFace);
            else
                Extern_Disable((uint)GlEnableCap.CullFace);
        }

        public static void Enable_DepthTest(bool v)
        {
            if (v)
                Extern_Enable((uint)GlEnableCap.DepthTest);
            else
                Extern_Disable((uint)GlEnableCap.DepthTest);
        }

        internal static void Clear(ClearBufferBit clearBuffer)
        {
            Extern_Clear(clearBuffer);
        }
    }
}
