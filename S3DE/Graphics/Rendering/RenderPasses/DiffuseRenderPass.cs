using S3DE.Graphics.FrameBuffers;
using S3DE.Graphics.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Rendering.RenderPasses
{
    public sealed class DiffuseRenderPass : RenderPass
    {
        public override bool ClearBeforeRendering => true;
        public override int Identifier => (int)RenderPassType.Deferred;


        
        public override void ClearBuffers()
        {
            Renderer.Clear(ClearBufferBit.COLOR | ClearBufferBit.DEPTH);
        }

        protected override void CreateFramebuffer()
        {
            framebuffer = new FrameBuffer(Game.RenderResolution);

            framebuffer.AddAttachment(
                new FrameBufferAttachment2D(
                    InternalFormat.RGB, 
                    PixelFormat.RGB, 
                    PixelType.FLOAT, 
                    Game.RenderResolution
                    ),
                FrameBufferAttachmentLocation.COLOR0);

            framebuffer.AddAttachment(
                new FrameBufferAttachment2D(
                    InternalFormat.DEPTH_COMPONENT,
                    PixelFormat.DEPTH_COMPONENT,
                    PixelType.FLOAT, 
                    Game.RenderResolution
                    ),
                FrameBufferAttachmentLocation.DEPTH);

            if (!framebuffer.CheckIsComplete())
                throw new Exception("Framebuffer is not complete!");
            
        }

        protected override void OnRender()
        {

        }

        protected override void PostDrawCallRender()
        {

        }
    }
}
