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

        /*
        internal void GenerateFrameBuffers()
        {
            #region Deferred_Framebuffers
            Framebuffer fb_Deferred = Framebuffer.Create(res);
            //Diffuse The final texture with light applied.
            fb_Deferred.AddBuffer(InternalFormat.RGBA, PixelFormat.RGBA, PixelType.UByte, FilterMode.Nearest, TargetBuffer.Diffuse);
            //Color
            fb_Deferred.AddBuffer(InternalFormat.RGB8, PixelFormat.RGB, PixelType.UByte, FilterMode.Nearest, TargetBuffer.Color);
            //Normal & Specular
            fb_Deferred.AddBuffer(InternalFormat.RGBA16F, PixelFormat.RGBA, PixelType.Float16, FilterMode.Nearest, TargetBuffer.Normal_Specular);
            //Position
            fb_Deferred.AddBuffer(InternalFormat.RGB16F, PixelFormat.RGB, PixelType.Float16, FilterMode.Nearest, TargetBuffer.Position);
            //Light Color & Intensity Alpha = Intensity
            fb_Deferred.AddBuffer(InternalFormat.RGBA16F, PixelFormat.RGBA, PixelType.Float16, FilterMode.Nearest, TargetBuffer.Light_Color_Intensity);
            //Depth
            fb_Deferred.AddBuffer(InternalFormat.DepthComponent16, PixelFormat.Depth, PixelType.Float16, FilterMode.Nearest, TargetBuffer.Depth);

            frameBuffers.Add(FrameBufferTarget.Deferred_Geometry, fb_Deferred);
            bool complete = fb_Deferred.IsComplete;
            fb_Deferred.SetDrawBuffers(TargetBuffer.Color,
                TargetBuffer.Normal_Specular, TargetBuffer.Position,TargetBuffer.Light_Color_Intensity);

            //Deferred_Light
            Framebuffer fb_DefLight = Framebuffer.Create(res);
            fb_DefLight.AddBuffer(fb_Deferred.GetBuffer(TargetBuffer.Diffuse), TargetBuffer.Diffuse);
            fb_DefLight.AddBuffer(fb_Deferred.GetBuffer(TargetBuffer.Color), TargetBuffer.Color);
            fb_DefLight.AddBuffer(fb_Deferred.GetBuffer(TargetBuffer.Normal_Specular), TargetBuffer.Normal_Specular);
            fb_DefLight.AddBuffer(fb_Deferred.GetBuffer(TargetBuffer.Position), TargetBuffer.Position);
            fb_DefLight.AddBuffer(fb_Deferred.GetBuffer(TargetBuffer.Light_Color_Intensity), TargetBuffer.Light_Color_Intensity);
            fb_DefLight.AddBuffer(fb_Deferred.GetBuffer(TargetBuffer.Depth), TargetBuffer.Depth);

            //Can only write into the Light Color & Intensity Map. (Color4).
            frameBuffers.Add(FrameBufferTarget.Deferred_Light, fb_DefLight);
            complete = fb_Deferred.IsComplete;
            fb_DefLight.SetDrawBuffers(TargetBuffer.Light_Color_Intensity);
            #endregion
        }
        */
    }
}
