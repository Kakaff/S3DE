using S3DE.Engine.Entities;
using S3DE.Maths;
using S3DE.Engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static S3DE.Engine.Enums;

namespace S3DE.Engine.Graphics
{
    public abstract class Renderer
    {

        public abstract RenderingAPI GetRenderingAPI();

        public static RenderingAPI GetAPI => ActiveRenderer.GetRenderingAPI();

        private Vector2 displayResolution,renderResolution;
        
        private static Renderer activeRenderer;

        protected abstract void SetCapabilities();
        protected abstract void init();
        protected abstract Renderer_MeshRenderer createMeshRenderer();
        protected abstract Renderer_Material createMaterial(Type materialType);
        protected abstract Texture2D createTexture2D(int width, int height);
        
        protected abstract void clear();
        protected abstract void OnWindowResized();
        protected abstract void OnRenderResolutionChanged();

        internal static Renderer ActiveRenderer => activeRenderer;
        
        internal static T SetTargetRenderer<T>() where T : Renderer
        {
            if (ActiveRenderer == null)
                activeRenderer = InstanceCreator.CreateInstance<T>();

            ActiveRenderer.displayResolution = new Vector2(640, 480);
            return (T)ActiveRenderer;
        }

        internal static Renderer_MeshRenderer CreateMeshRenderer()
        {
            return ActiveRenderer.createMeshRenderer();
        }

        internal static Renderer_Material CreateMaterial(Type materialType)
        {
            return ActiveRenderer.createMaterial(materialType);
        }

        internal static Texture2D CreateTexture2D(int width, int height)
        {
            return ActiveRenderer.createTexture2D(width, height);
        }

        internal static void SetCapabilities_Internal() => ActiveRenderer.SetCapabilities();

        internal static void Init()
        {
            ActiveRenderer.init();
        }

        internal static void Clear()
        {
            ActiveRenderer.clear();
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

        internal static void OnWindowResized_Internal() => ActiveRenderer.OnWindowResized();
        internal static void OnRenderResolutionChanged_Internal() => ActiveRenderer.OnRenderResolutionChanged();
        internal static Vector2 DisplayResolution => ActiveRenderer.displayResolution;
        internal static Vector2 RenderResolution => ActiveRenderer.renderResolution;
    }
}
