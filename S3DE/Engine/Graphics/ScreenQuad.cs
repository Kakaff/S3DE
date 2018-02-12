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

        public static RenderTexture2D Frame => instance.prevFrame;

        internal static void Render_Internal(RenderTexture2D frame)
        {
            instance.prevFrame = frame;
            instance.RenderFrameToScreen(frame);
        }

        protected abstract void RenderFrameToScreen(RenderTexture2D frame);

        internal static void Create() => instance = Renderer.CreateScreenQuad_Internal();
    }
}
