using S3DE.Engine.Entities.Components;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static S3DE.Engine.Enums;

namespace S3DE.Engine.Graphics
{
    public sealed class Rendercall
    {
        uint passes;

        Dictionary<RenderPass, Framebuffer> frameBuffers;
        Vector2 res;

        private Rendercall() { }

        public Rendercall(Vector2 res, params RenderPass[] Passes)
        {
            frameBuffers = new Dictionary<RenderPass, Framebuffer>();
            this.res = res;
            passes = 0;
            foreach (RenderPass rp in Passes)
                passes = passes | (uint)rp;

            GenerateFrameBuffers();
        }

        RenderPass[] GetPasses()
        {
            List<RenderPass> result = new List<RenderPass>();
            
            return result.ToArray();
        }

        public bool Uses(RenderPass pass) => (passes & (uint)pass) == (uint)pass;
        public RenderPass[] Passes => GetPasses();
        public Vector2 Resolution => res;

        public Framebuffer GetFrameBuffer(RenderPass pass)
        {
            Framebuffer fb = null;
            if (Uses(pass))
                frameBuffers.TryGetValue(pass, out fb);

            return fb;
        }

        public RenderTexture2D GetDiffuseMap(RenderPass pass)
        {
            if (Uses(pass))
            {
                frameBuffers.TryGetValue(pass, out Framebuffer fb);
                return fb.GetBuffer(BufferAttachment.Color0);
            }
            return null;
        }

        public RenderTexture2D GetNormalMap(RenderPass pass)
        {
            if (Uses(pass))
            {
                frameBuffers.TryGetValue(pass, out Framebuffer fb);
                return fb.GetBuffer(BufferAttachment.Color1);
            }
            return null;
        }

        public RenderTexture2D GetPositionMap(RenderPass pass)
        {
            if (Uses(pass))
            {
                frameBuffers.TryGetValue(pass, out Framebuffer fb);
                return fb.GetBuffer(BufferAttachment.Color3);
            }
            return null;
        }
        public RenderTexture2D GetSpecularMap(RenderPass pass)
        {
            //Color2
            throw new NotImplementedException();
        }
        public RenderTexture2D GetLightMap(RenderPass pass)
        {
            //Color3
            throw new NotImplementedException();
        }

        public RenderTexture2D GetDepth_Stencil(RenderPass pass)
        {
            if (Uses(pass))
            {
                frameBuffers.TryGetValue(pass, out Framebuffer fb);
                return fb.GetBuffer(BufferAttachment.Depth_Stencil);
            }

            return null;
        }

        internal void GenerateFrameBuffers()
        {
            //Get all the passes we are going to use.
            if (Uses(RenderPass.Deferred))
            {
                Framebuffer fb = Framebuffer.Create(res);
                //Albedo
                fb.AddBuffer(InternalFormat.RGBA,PixelFormat.RGBA,PixelType.UByte,FilterMode.Nearest,BufferAttachment.Color0); //Albedo
                //Normal
                fb.AddBuffer(InternalFormat.RGB16F, PixelFormat.RGB, PixelType.Float16, FilterMode.Nearest, BufferAttachment.Color1);
                //Specular
                fb.AddBuffer(InternalFormat.R16F, PixelFormat.Red, PixelType.Float16, FilterMode.Nearest, BufferAttachment.Color2);
                //Position
                fb.AddBuffer(InternalFormat.RGB16F, PixelFormat.RGB, PixelType.Float16, FilterMode.Nearest, BufferAttachment.Color3);
                //Depth
                fb.AddBuffer(InternalFormat.Depth_Component32, PixelFormat.Depth, PixelType.Float32, FilterMode.Nearest, BufferAttachment.Depth); // Depth

                frameBuffers.Add(RenderPass.Deferred, fb);
                bool complete = fb.IsComplete;
                fb.SetDrawBuffers(BufferAttachment.Color0,BufferAttachment.Color1,BufferAttachment.Color2,BufferAttachment.Color3);
                
            }
            //Generate framebuffers for each.
        }
    }
}
