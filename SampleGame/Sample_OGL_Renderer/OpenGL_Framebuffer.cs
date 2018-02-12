using OpenGL;
using S3DE.Engine.Graphics;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static S3DE.Engine.Enums;

namespace SampleGame.Sample_OGL_Renderer
{
    internal class OpenGL_Framebuffer : Framebuffer
    {
        uint pointer;
        Dictionary<BufferAttachment, OpenGL_RenderTexture2D> attachments;
        Vector2 resolution;

        internal OpenGL_Framebuffer(int width, int height)
        {
            Console.WriteLine($"Creating new Framebuffer with resolution: {width}x{height}");
            resolution = new Vector2(width, height);
            attachments = new Dictionary<BufferAttachment, OpenGL_RenderTexture2D>();
        }

        public override bool IsComplete => checkStatus();

        public override void Bind()
        {
            if (pointer == 0)
                CreateHandle();
            Gl.BindFramebuffer(FramebufferTarget.Framebuffer, pointer);
            OpenGL_Renderer.TestForGLErrors();
        }

        public override void Unbind()
        {
            Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            OpenGL_Renderer.TestForGLErrors();
        }

        public override RenderTexture2D GetBuffer(BufferAttachment attachment)
        {
            attachments.TryGetValue(attachment, out OpenGL_RenderTexture2D tex);
            return tex;
        }

        public override void AddBuffer(S3DE.Engine.Enums.InternalFormat internalFormat, S3DE.Engine.Enums.PixelFormat pixelFormat, S3DE.Engine.Enums.PixelType pixelType, FilterMode filter,BufferAttachment attachment, out RenderTexture2D renderTexture)
        {
            AddBuffer(internalFormat,pixelFormat,pixelType, filter,attachment);
            renderTexture = GetBuffer(attachment);
        }

        public override void AddBuffer(S3DE.Engine.Enums.InternalFormat internalFormat, S3DE.Engine.Enums.PixelFormat pixelFormat, S3DE.Engine.Enums.PixelType pixelType, FilterMode filter,BufferAttachment attachment)
        {
            //Create a new rendertexture.
            OpenGL_RenderTexture2D tex = new OpenGL_RenderTexture2D(
                internalFormat,
                pixelFormat,
                pixelType,
                filter,
                (int)resolution.x,(int)resolution.y);

            //Attach it to the framebuffer.
            this.Bind();
            tex.Bind();
            Gl.FramebufferTexture2D(FramebufferTarget.Framebuffer,
                OpenGL_Utility.Convert(attachment), 
                TextureTarget.Texture2d, 
                tex.Pointer, 0);
            OpenGL_Renderer.TestForGLErrors();
            tex.Unbind();
            this.Unbind();
            attachments.Add(attachment, tex);
            //Do the thing.
        }

        public override void Clear()
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
        void CreateHandle()
        {
            pointer = Gl.GenFramebuffer();
            OpenGL_Renderer.TestForGLErrors();
        }

        bool checkStatus()
        {
            this.Bind();
            FramebufferStatus status = Gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (status == FramebufferStatus.FramebufferComplete)
                return true;
            else
                throw new Exception("Framebuffer is not complete | " + status);
        }
    }
}
