using S3DE.Engine.Graphics.Textures;
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
    public enum TargetBuffer
    {
        Diffuse = BufferAttachment.Color0,
        Color = BufferAttachment.Color1,
        Normal = BufferAttachment.Color2,
        Position = BufferAttachment.Color3,
        Specular = BufferAttachment.Color4,
        Light_Color_Intensity = BufferAttachment.Color5,
        Depth = BufferAttachment.Depth,
        Depth_Stencil = BufferAttachment.Depth_Stencil,
    }

    public abstract class Framebuffer
    {

        protected BufferAttachment[] DrawBuffers;

        public abstract bool IsComplete { get; }
        public static Framebuffer Create(Vector2 size) => Renderer.CreateFramebuffer_Internal(size);
        public static Framebuffer ActiveFrameBuffer => activeFrameBuffer;
        static Framebuffer activeFrameBuffer;
        public abstract void Bind();
        public abstract void Unbind();
        public abstract void Clear();
        public abstract void Clear(bool color, bool depth, bool stencil);
        public abstract void Clear(params BufferAttachment[] attachments);

        public void Clear(params TargetBuffer[] targetBuffers)
        {
            BufferAttachment[] attachments = new BufferAttachment[targetBuffers.Length];
            for (int i = 0; i < targetBuffers.Length; i++)
                attachments[i] = (BufferAttachment)targetBuffers[i];

            Clear(attachments);

        }
        public virtual void SetDrawBuffers(params TargetBuffer[] buffers)
        {
            BufferAttachment[] attachments = new BufferAttachment[buffers.Length];
            for (int i = 0; i < buffers.Length; i++)
                attachments[i] = (BufferAttachment)buffers[i];

            SetDrawBuffers(attachments);
        }

        public virtual void SetDrawBuffers(params BufferAttachment[] attachments)
        {
            Bind();
            SetAsActive();
            Renderer.SetDrawBuffers_Internal(attachments);
            DrawBuffers = attachments;
            Unbind();
        }

        public void SetAsActive() => activeFrameBuffer = this;
        public RenderTexture2D GetBuffer(TargetBuffer buffer) => GetBuffer((BufferAttachment)buffer);
        public void AddBuffer(RenderTexture2D buffer, TargetBuffer target) => AddBuffer(buffer, (BufferAttachment)target);
        public void AddBuffer(InternalFormat internalFormat, PixelFormat pixelFormat, PixelType colorType, FilterMode filter, TargetBuffer target) =>
            AddBuffer(internalFormat,pixelFormat,colorType,filter, (BufferAttachment)target);
        public void AddBuffer(InternalFormat internalFormat, PixelFormat pixelFormat, PixelType colorType, FilterMode filter, TargetBuffer target, out RenderTexture2D renderTexture) =>
            AddBuffer(internalFormat, pixelFormat, colorType, filter, (BufferAttachment)target, out renderTexture);

        public abstract RenderTexture2D GetBuffer(BufferAttachment attachment);
        public abstract void AddBuffer(RenderTexture2D buffer, BufferAttachment attachment);
        public abstract void AddBuffer(InternalFormat internalFormat,PixelFormat pixelFormat, PixelType colorType, FilterMode filter, BufferAttachment attachment);
        public abstract void AddBuffer(InternalFormat internalFormat, PixelFormat pixelFormat, PixelType colorType, FilterMode filter, BufferAttachment attachment, out RenderTexture2D renderTexture);
    }
}
