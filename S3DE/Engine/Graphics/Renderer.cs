using S3DE.Engine.Entities;
using S3DE.Maths;
using S3DE.Engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static S3DE.Engine.Enums;
using S3DE.Engine.Graphics.Materials;
using S3DE.Engine.Graphics.Textures;

namespace S3DE.Engine.Graphics
{
    public enum RenderPass
    {
        //Still in planning phase.
        ShadowMap = 0x01, //Only operates on objects within range of light.
        Deferred = 0x02, //Only operates on objects that use deferred rendering.
        Forward = 0x04, //Only operates on objects that use forward rendering, is sorted from far to near.
    }

    public enum FrameBufferTarget
    {
        Deferred_Geometry,
        Deferred_Light,
        Forward,
        Final
    }

    public enum AlphaFunction
    {
        GreaterThan,
        LessThan,
        Never,
        Always,
    }

    public enum Function
    {
        AlphaTest,
        DepthTest,

    }
    public abstract class Renderer
    {
        static RenderPass renderPass = RenderPass.Deferred;
        static RenderCall mainRenderCall;

        public static RenderPass CurrentRenderPass
        {
            get => renderPass;
            internal set => renderPass = value;
        }

        public abstract RenderingAPI GetRenderingAPI();

        public static RenderingAPI GetAPI => ActiveRenderer.GetRenderingAPI();
        public static int Max_Mipmap_Levels
        {
            get => ActiveRenderer.maxMipMapLevels;
            set => ActiveRenderer.maxMipMapLevels = value;
        }

        public static bool AlphaTesting
        {
            get => activeRenderer.alphaTesting;
            set
            {
                if (value)
                    ActiveRenderer.enable(Function.AlphaTest);
                else
                    ActiveRenderer.disable(Function.AlphaTest);

                activeRenderer.alphaTesting = value;
            }
        }

        public static int API_Version => ActiveRenderer.apiVer;

        public static Vector2 ViewportSize
        {
            get => ActiveRenderer.viewportSize;
            set {
                ActiveRenderer.SetViewportSize(value);
                ActiveRenderer.viewportSize = value;
            }
        }

        private Vector2 displayResolution,renderResolution,viewportSize;
        private int apiVer,refreshRate;
        private int maxMipMapLevels = 1000;
        private bool alphaTesting = false;

        private static Renderer activeRenderer;

        protected abstract void SetCapabilities();
        protected abstract void Init();
        protected abstract void SetAlphaFunction(AlphaFunction function, float value);
        protected abstract Renderer_MeshRenderer CreateMeshRenderer();
        protected abstract Renderer_Material CreateMaterial(Type materialType,RenderPass pass);
        protected abstract Texture2D CreateTexture2D(int width, int height);
        protected abstract RenderTexture2D CreateRenderTexture2D(InternalFormat internalFormat, PixelFormat pixelFormat, PixelType pixelType, FilterMode filter,int width, int height);
        protected abstract Framebuffer CreateFrameBuffer(int width, int height);
        protected abstract ScreenQuad CreateScreenQuad();

        protected abstract void Clear();
        protected abstract void OnWindowResized();
        protected abstract void OnRenderResolutionChanged();
        protected abstract void OnRefreshRateChanged();
        protected abstract void enable(Function func);
        protected abstract void disable(Function func);
        protected abstract void SetViewportSize(Vector2 size);
        protected abstract void UnbindTexUnit(int textureUnit);
        protected abstract void SetDrawBuffers(BufferAttachment[] buffers);
        protected abstract void Sync();
        protected void SetApiVersion(int version) => apiVer = version;

        internal static Renderer ActiveRenderer => activeRenderer;
        internal static RenderCall MainRenderCall => mainRenderCall;
        internal static void Sync_Internal() => ActiveRenderer.Sync();
        public static void Enable(Function func) => ActiveRenderer.enable(func);
        public static void Disable(Function func) => ActiveRenderer.disable(func);

        internal static T SetTargetRenderer<T>() where T : Renderer
        {
            if (ActiveRenderer == null)
                activeRenderer = InstanceCreator.CreateInstance<T>();

            ActiveRenderer.displayResolution = new Vector2(640, 480);
            ActiveRenderer.renderResolution = new Vector2(640, 480);
            ActiveRenderer.refreshRate = 60;
            return (T)ActiveRenderer;
        }

        internal static Renderer_MeshRenderer CreateMeshRenderer_Internal()
        {
            return ActiveRenderer.CreateMeshRenderer();
        }

        internal static Renderer_Material CreateMaterial_Internal(Type materialType,RenderPass pass)
        {
            return ActiveRenderer.CreateMaterial(materialType,pass);
        }

        internal static Texture2D CreateTexture2D_Internal(int width, int height)
        {
            return ActiveRenderer.CreateTexture2D(width, height);
        }

        internal static RenderTexture2D CreateRenderTexture2D_Internal(InternalFormat internalFormat, 
            PixelFormat pixelFormat, PixelType pixelType, FilterMode filter,int width, int height) => 
            ActiveRenderer.CreateRenderTexture2D(internalFormat,pixelFormat,pixelType,filter,width, height);

        internal static ScreenQuad CreateScreenQuad_Internal() => ActiveRenderer.CreateScreenQuad();

        internal static Framebuffer CreateFramebuffer_Internal(Vector2 size) => ActiveRenderer.CreateFrameBuffer((int)size.x, (int)size.y);
        
        public static void AlphaFunction(AlphaFunction function, float value) => ActiveRenderer.SetAlphaFunction(function, value);

        internal static void SetCapabilities_Internal() => ActiveRenderer.SetCapabilities();

        internal static void Init_Internal()
        {
            ActiveRenderer.Init();
        }

        internal static void Clear_Internal()
        {
            ActiveRenderer.Clear();
        }

        internal static void SetDisplayResolution(Vector2 res)
        {
            ActiveRenderer.displayResolution = res;
            EngineMain.ResizeWindow = true;
        }

        internal static void SetRenderResolution(Vector2 res)
        {
            ActiveRenderer.renderResolution = res;
            OnRenderResolutionChanged_Internal();
        }

        internal static void CreateMainRenderCall()
        {
            mainRenderCall = RenderPipeline.CreateRenderCall_Internal();
        }

        internal static void SetRefreshRate(int r)
        {
            ActiveRenderer.refreshRate = r;
            OnRefreshRateChanged_Internal();
        }

        internal static void SetDrawBuffers_Internal(params BufferAttachment[] buffers) => ActiveRenderer.SetDrawBuffers(buffers);

        internal static void OnWindowResized_Internal() => ActiveRenderer.OnWindowResized();
        internal static void OnRenderResolutionChanged_Internal() => ActiveRenderer.OnRenderResolutionChanged();
        internal static void OnRefreshRateChanged_Internal() => ActiveRenderer.OnRefreshRateChanged();
        internal static Vector2 DisplayResolution => ActiveRenderer.displayResolution;
        internal static Vector2 RenderResolution => ActiveRenderer.renderResolution;
        internal static int RefreshRate => ActiveRenderer.refreshRate;
        public static void UnbindTextureUnit(int textureUnit) => ActiveRenderer.UnbindTexUnit(textureUnit); 
        public static void UnbindTextureUnit(params int[] textureUnits)
        {
            foreach (int i in textureUnits)
                ActiveRenderer.UnbindTexUnit(i);
        }
    }
}
