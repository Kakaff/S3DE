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
using S3DE.Engine.Collections;

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
        Equals,
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

        public static S3DE_Vector2 ViewportSize
        {
            get => ActiveRenderer.viewportSize;
            set {
                ActiveRenderer.SetViewportSize(value);
                ActiveRenderer.viewportSize = value;
            }
        }

        private S3DE_Vector2 displayResolution,renderResolution,viewportSize;
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
        protected abstract Renderer_Mesh CreateRendererMesh();
        protected abstract S3DE_UniformBuffer Create_UniformBuffer();

        protected abstract int MaxSupportedTextureUnits();
        protected abstract int MaxSupportedUniformBlockBindingPoints();
        protected abstract void Draw_Mesh(Renderer_Mesh m);
        protected abstract void Clear();
        protected abstract void OnWindowResized();
        protected abstract void OnRenderResolutionChanged();
        protected abstract void OnRefreshRateChanged();
        protected abstract void enable(Function func);
        protected abstract void disable(Function func);
        protected abstract void SetViewportSize(S3DE_Vector2 size);
        protected abstract void BindTexUnit(ITexture tex, TextureUnit tu);
        protected abstract void Bind_UniformBuffer(S3DE_UniformBuffer buffer, int bindingPoint);
        protected abstract void Unbind_UniformBuffer(S3DE_UniformBuffer buffer);
        protected abstract void UnbindTexUnit(TextureUnit tu);
        protected abstract void SetDrawBuffers(BufferAttachment[] buffers);
        protected abstract void FinalizePass();
        protected abstract void FinishFrame();

        protected void SetApiVersion(int version) => apiVer = version;

        internal static Renderer ActiveRenderer => activeRenderer;
        internal static RenderCall MainRenderCall => mainRenderCall;
        public static void Enable(Function func) => ActiveRenderer.enable(func);
        public static void Disable(Function func) => ActiveRenderer.disable(func);
        internal static void FinalizeRenderPass() => ActiveRenderer.FinalizePass();
        internal static void Finish_Internal() => ActiveRenderer.FinishFrame();

        internal static string GetName() => ActiveRenderer.GetType().Name;

        internal static T SetTargetRenderer<T>() where T : Renderer
        {
            Console.WriteLine($"Setting TargetRenderer as {typeof(T).Name}");
            activeRenderer = InstanceCreator.CreateInstance<T>();

            ActiveRenderer.displayResolution = new S3DE_Vector2(640, 480);
            ActiveRenderer.renderResolution = new S3DE_Vector2(640, 480);
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

        public static Renderer_Mesh CreateMesh() => ActiveRenderer.CreateRendererMesh();

        public static S3DE_UniformBuffer CreateUniformBuffer() => ActiveRenderer.Create_UniformBuffer();

        internal static void DrawMesh(Renderer_Mesh m) => ActiveRenderer.Draw_Mesh(m);

        internal static RenderTexture2D CreateRenderTexture2D_Internal(InternalFormat internalFormat, 
            PixelFormat pixelFormat, PixelType pixelType, FilterMode filter,int width, int height) => 
            ActiveRenderer.CreateRenderTexture2D(internalFormat,pixelFormat,pixelType,filter,width, height);

        internal static ScreenQuad CreateScreenQuad_Internal() => ActiveRenderer.CreateScreenQuad();

        internal static Framebuffer CreateFramebuffer_Internal(S3DE_Vector2 size) => ActiveRenderer.CreateFrameBuffer((int)size.X, (int)size.Y);
        
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

        internal static void SetDisplayResolution(S3DE_Vector2 res)
        {
            ActiveRenderer.displayResolution = res;
            EngineMain.ResizeWindow = true;
        }

        internal static void SetRenderResolution(S3DE_Vector2 res)
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
        internal static S3DE_Vector2 DisplayResolution => ActiveRenderer.displayResolution;
        internal static S3DE_Vector2 RenderResolution => ActiveRenderer.renderResolution;
        internal static int RefreshRate => ActiveRenderer.refreshRate;
        public static void UnbindTextureUnit(TextureUnit tu) => ActiveRenderer.UnbindTexUnit(tu);
        public static void BindTextureUnit(ITexture tex, TextureUnit tu) => ActiveRenderer.BindTexUnit(tex, tu);
        public static void BindUniformBuffer(S3DE_UniformBuffer buffer, int bindingPoint) => ActiveRenderer.Bind_UniformBuffer(buffer, bindingPoint);
        public static void UnbindUniformBuffer(S3DE_UniformBuffer buffer) => ActiveRenderer.Unbind_UniformBuffer(buffer);
        public static int TextureUnitCount { get => ActiveRenderer.MaxSupportedTextureUnits(); }
        public static int UniformBlockBindingPoints { get => ActiveRenderer.MaxSupportedUniformBlockBindingPoints(); }
    }
}
