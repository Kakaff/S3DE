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
        static Vector2 currDisplayRes,currRenderRes,newDisplayRes,newRenderRes;
        static bool displayResolutionChanged, renderResolutionChanged,renderResChanged,displayResChanged;
        static bool vsync = false;
        static uint latestError = 0;
        public static bool NoError { get { latestError = Extern_CheckGLErrors(); return latestError == (uint)GL.NO_ERROR; } }
        public static uint LatestError => latestError;

        public static Vector2 DisplayResolution => currDisplayRes;
        public static Vector2 RenderResolution => currRenderRes;

        public static bool RenderResolutionChanged => renderResChanged;
        public static bool DisplayResolutionChanged => displayResChanged;
        public static bool Vsync => vsync;

        public delegate void ResolutionChanged(Vector2 oldRes, Vector2 newRes);
        public static event ResolutionChanged OnDisplayResolutionChanged;
        public static event ResolutionChanged OnRenderResolutionChanged;

        public static void SetDisplayResolution(Vector2 res)
        {
            if (DisplayResolution != res)
            {
                newDisplayRes = res;
                displayResolutionChanged = true;
            }
        }

        public static void SetRenderResolution(Vector2 res)
        {
            if (RenderResolution != res)
            {
                newRenderRes = res;
                renderResolutionChanged = true;
            }
        }

        public static void UpdateEvents()
        {
            renderResChanged = false;
            displayResChanged = false;

            if (renderResolutionChanged)
            {
                if (OnRenderResolutionChanged != null)
                    OnRenderResolutionChanged(currRenderRes, newRenderRes);

                currRenderRes = newRenderRes;
                renderResolutionChanged = false;

                renderResChanged = true;
                
            }

            if (displayResolutionChanged)
            {
                if (OnDisplayResolutionChanged != null)
                    OnDisplayResolutionChanged(currDisplayRes, newDisplayRes);

                currDisplayRes = newDisplayRes;
                displayResolutionChanged = false;

                displayResChanged = true;
            }
        }

        internal static void Init(Vector2 displayRes,Vector2 renderRes)
        {
            currRenderRes = renderRes;
            currDisplayRes = displayRes;
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

        public static void Enable_Vsync(bool v)
        {
            if (v != vsync)
            {
                Extern_SetSwapInterval(v ? 1 : 0);
            }
        }

        internal static void Clear(ClearBufferBit clearBuffer)
        {
            Extern_Clear(clearBuffer);
        }
    }
}
