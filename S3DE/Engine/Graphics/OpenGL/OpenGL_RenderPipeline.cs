using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S3DE.Engine.Scenes;
using S3DE.Maths;
using static S3DE.Engine.Enums;
using S3DE.Engine.Graphics.Textures;

namespace S3DE.Engine.Graphics.OpenGL
{
    public class OpenGL_RenderPipeline : RenderPipeline
    {
        protected override RenderCall CreateRenderCall(Vector2 resolution)
        {
            RenderCall rc = new RenderCall(resolution);

            #region Deferred Geometry
            Framebuffer def_geo_fb = Framebuffer.Create(resolution);
            //Diffuse The final texture with light applied.
            def_geo_fb.AddBuffer(InternalFormat.RGBA, PixelFormat.RGBA, PixelType.UByte, FilterMode.Nearest, TargetBuffer.Diffuse);
            //Color
            def_geo_fb.AddBuffer(InternalFormat.RGB8, PixelFormat.RGB, PixelType.UByte, FilterMode.Nearest, TargetBuffer.Color);
            //Normal & Specular
            def_geo_fb.AddBuffer(InternalFormat.RGBA16F, PixelFormat.RGBA, PixelType.Float16, FilterMode.Nearest, TargetBuffer.Normal_Specular);
            //Position
            def_geo_fb.AddBuffer(InternalFormat.RGB16F, PixelFormat.RGB, PixelType.Float16, FilterMode.Nearest, TargetBuffer.Position);
            //Light Color & Intensity Alpha = Intensity
            def_geo_fb.AddBuffer(InternalFormat.RGBA16F, PixelFormat.RGBA, PixelType.Float16, FilterMode.Nearest, TargetBuffer.Light_Color_Intensity);
            //Depth
            def_geo_fb.AddBuffer(InternalFormat.DepthComponent16, PixelFormat.Depth, PixelType.Float16, FilterMode.Nearest, TargetBuffer.Depth);

            rc.AddFrameBuffer(def_geo_fb,FrameBufferTarget.Deferred_Geometry);
            bool complete = def_geo_fb.IsComplete;
            def_geo_fb.SetDrawBuffers(TargetBuffer.Color,
                TargetBuffer.Normal_Specular, TargetBuffer.Position);

            #endregion

            #region Final
            Framebuffer fin = Framebuffer.Create(resolution);

            fin.AddBuffer(def_geo_fb.GetBuffer(TargetBuffer.Color), TargetBuffer.Diffuse);
            rc.AddFrameBuffer(fin, FrameBufferTarget.Final);

            #endregion

            return rc;
        }

        protected override void Init()
        {
        }

        protected override void RenderScene(GameScene scene, RenderCall renderCall)
        {
            RenderDeferredGeo(scene, renderCall.GetFrameBuffer(FrameBufferTarget.Deferred_Geometry));
        }

        void RenderDeferredGeo(GameScene scene, Framebuffer fb)
        {
            fb.Bind();
            fb.Clear();
            Renderer.Enable(Function.DepthTest);
            Renderer.Enable(Function.AlphaTest);
            Renderer.AlphaFunction(AlphaFunction.Never, 0f);
            DrawScene(scene);
            fb.Unbind();
        }
    }
}
