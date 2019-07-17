using S3DECore.Graphics;
using S3DECore.Graphics.Framebuffers;
using S3DECore.Graphics.Textures;
using S3DECore.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics
{
    public sealed class DeferredRenderpass : Renderpass
    {
        public override Framebuffer FinalFramebuffer => Framebuffers[0];

        protected override void Init() {
            Framebuffer fb = new Framebuffer();
            
            fb.AddTextureAttachment2D(
                new TextureAttachment2D(
                    new Vector2(1280,720),
                    PixelType.UNSIGNED_BYTE,
                    PixelFormat.RGB,
                    InternalFormat.RGB
                    ),
                AttachmentLocation.Color0
                );

            
            fb.AddTextureAttachment2D(
                new TextureAttachment2D(
                    new Vector2(1280,720),
                    PixelType.FLOAT,
                    PixelFormat.DEPTH_COMPONENT,
                    InternalFormat.DEPTH_COMPONENT
                    ),
                AttachmentLocation.Depth
                );
                
            if (!fb.IsComplete())
                throw new Exception("Framebuffer is not complete!");
                
            Framebuffers = new Framebuffer[] { new Framebuffer() };
            Framebuffers[0] = fb;

            fb.Unbind();
        }

        protected override void OnDraw()
        {
            Framebuffers[0].Bind();
            Renderer.Clear(ClearBufferBit.Color | ClearBufferBit.Depth);
            DrawMeshes();
            
        }

        protected override void PostDraw()
        {
            Framebuffers[0].Unbind();
        }

        protected override void PreDraw()
        {
            
        }
    }
}
