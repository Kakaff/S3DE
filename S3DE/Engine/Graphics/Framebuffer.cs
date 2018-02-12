using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static S3DE.Engine.Enums;

namespace S3DE.Engine.Graphics
{
    public enum BufferAttachment
    {
        Depth,
        Depth_Stencil,
        Stencil,
        MaxColor,
        Color0,
        Color1,
        Color2,
        Color3,
        Color4,
        Color5,
        Color6,
        Color7,
        Color8,
        Color9,
        Color10,
        Color11,
        Color12,
        Color13,
        Color14,
        Color15,
        Color16,
        Color17,
        Color18,
        Color19,
        Color20,
        Color21,
        Color22,
        Color23,
        Color24,
        Color25,
        Color26,
        Color27,
        Color28,
        Color29,
        Color30,
        Color31
    }

    public abstract class Framebuffer
    {
        public abstract bool IsComplete { get; }
        public static Framebuffer Create(Vector2 size) => Renderer.CreateFramebuffer_Internal(size);
        public abstract void Bind();
        public abstract void Unbind();
        public abstract void Clear();

        public void SetDrawBuffers(params BufferAttachment[] attachments)
        {
            Bind();
            Renderer.SetDrawBuffers_Internal(attachments);
            Unbind();
        }

        public abstract RenderTexture2D GetBuffer(BufferAttachment attachment);
        public abstract void AddBuffer(InternalFormat internalFormat,PixelFormat pixelFormat, PixelType colorType, FilterMode filter, BufferAttachment attachment);
        public abstract void AddBuffer(InternalFormat internalFormat, PixelFormat pixelFormat, PixelType colorType, FilterMode filter, BufferAttachment attachment, out RenderTexture2D renderTexture);
    }
}
