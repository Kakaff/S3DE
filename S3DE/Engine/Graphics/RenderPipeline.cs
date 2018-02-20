using S3DE.Engine.Entities;
using S3DE.Engine.Graphics.Materials;
using S3DE.Engine.Scenes;
using S3DE.Engine.Utility;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics
{
    public abstract class RenderPipeline
    {
        static RenderPipeline instance;

        public static RenderPipeline ActivePipeline => instance;

        internal static void CreatePipeline<T>() where T: RenderPipeline => instance = InstanceCreator.CreateInstance<T>();

        internal static RenderCall CreateRenderCall_Internal() => ActivePipeline.CreateRenderCall(Renderer.RenderResolution);

        protected abstract RenderCall CreateRenderCall(Vector2 resolution);

        protected abstract void RenderScene(GameScene scene, RenderCall renderCall);

        protected abstract void Init();

        internal static void Init_Internal() => ActivePipeline.Init();
        internal static void RenderScene_Internal(GameScene scene, RenderCall renderCall)
        {
            Renderer.ViewportSize = renderCall.Resolution;
            ActivePipeline.RenderScene(scene, renderCall);
            ScreenQuad.Render_Internal(renderCall.GetBuffer(FrameBufferTarget.Final, TargetBuffer.Diffuse));
        }

        protected void DrawScene(GameScene scene) => scene.Draw();
    }
}
