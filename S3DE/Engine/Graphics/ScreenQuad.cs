using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics
{
    public abstract class ScreenQuad
    {
        static ScreenQuad instance;
        RenderTexture2D prevFrame;

        public static RenderTexture2D PreviousFrame => instance.prevFrame;

        public static void RenderToScreenQuad() => instance.Render();

        internal static void Present_Frame(RenderTexture2D frame)
        {
            Framebuffer.ActiveFrameBuffer.Unbind();
            Renderer.Disable(Function.DepthTest);
            Renderer.Disable(Function.AlphaTest);
            Renderer.Clear_Internal();
            Renderer.ViewportSize = Renderer.DisplayResolution;
            instance.RenderFrameToScreen(frame);
            instance.prevFrame = frame;
        }

        internal static void BindMesh_Internal() => instance.BindMesh();

        protected abstract void Render();
        protected abstract void RenderFrameToScreen(RenderTexture2D frame);
        protected abstract void BindMesh();
        protected abstract void UnbindMesh();
        internal static void Create() => instance = Renderer.CreateScreenQuad_Internal();
    }
}
