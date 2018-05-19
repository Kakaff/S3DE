using OpenGL;
using S3DE.Engine.Graphics;
using S3DE.Engine.Graphics.Textures;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static S3DE.Engine.Enums;

namespace S3DE.Engine.Graphics.OpGL
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
            /* Not sure if we should allow for binding buffer attachments while writing to the same framebuffer. Sounds kinda stupid.
            foreach (RenderTexture2D tex in attachments.Values)
                tex.Unbind();
                */
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
            tex.Bind(TextureUnit._0);
            Gl.FramebufferTexture2D(FramebufferTarget.Framebuffer,
                OpenGL_Utility.Convert(attachment), 
                TextureTarget.Texture2d, 
                tex.Pointer, 0);
            OpenGL_Renderer.TestForGLErrors();
            this.Unbind();
            TextureUnits.UnbindTextureUnit(TextureUnit._0);
            attachments.Add(attachment, tex);
            
        }

        public override void AddBuffer(RenderTexture2D buffer, BufferAttachment attachment)
        {
            this.Bind();
            buffer.Bind(TextureUnit._0);
            Gl.FramebufferTexture2D(FramebufferTarget.Framebuffer,
                OpenGL_Utility.Convert(attachment),
                TextureTarget.Texture2d,
                ((OpenGL_RenderTexture2D)buffer).Pointer, 0);
            TextureUnits.UnbindTextureUnit(TextureUnit._0);
            this.Unbind();
            attachments.Add(attachment, (OpenGL_RenderTexture2D)buffer);
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

        public override void Clear(bool color, bool depth, bool stencil)
        {
            int clearBit = 0;
            if (color)
                clearBit |= (int)OpenGL.ClearBufferMask.ColorBufferBit;
            if (depth)
                clearBit |= (int)OpenGL.ClearBufferMask.DepthBufferBit;
            if (stencil)
                clearBit |= (int)OpenGL.ClearBufferMask.StencilBufferBit;
            
            Gl.Clear((ClearBufferMask)clearBit);
        }

        public override void Clear(params BufferAttachment[] attachments)
        {
            throw new NotImplementedException();
        }
    }
}
