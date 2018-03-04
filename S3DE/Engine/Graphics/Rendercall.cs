using S3DE.Engine.Entities.Components;
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
    public class RenderCall
    {
        Dictionary<int, Framebuffer> frameBuffers;
        Vector2 res;

        private RenderCall() { }

        public RenderCall(Vector2 res)
        {
            this.res = res;
            frameBuffers = new Dictionary<int, Framebuffer>();
        }

        public Vector2 Resolution
        {
            get => res;
            protected set => res = value;
        }

        public Framebuffer GetFrameBuffer(int target)
        {
            Framebuffer fb = null;
            frameBuffers.TryGetValue(target, out fb);
            return fb;
        }

        public Framebuffer GetFrameBuffer(FrameBufferTarget target) => GetFrameBuffer((int)target);

        public void AddFrameBuffer(Framebuffer fb, int target) => frameBuffers.Add(target, fb);
        public void AddFrameBuffer(Framebuffer fb, FrameBufferTarget target) => AddFrameBuffer(fb, (int)target);

        public void AddFrameBuffer(FrameBufferTarget target, out Framebuffer fb) => AddFrameBuffer(target, out fb);

        public RenderTexture2D GetBuffer(FrameBufferTarget target, TargetBuffer buffer) => GetFrameBuffer(target).GetBuffer(buffer);

        
    }
}
