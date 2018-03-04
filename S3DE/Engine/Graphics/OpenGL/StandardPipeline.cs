using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S3DE.Engine.Scenes;
using S3DE.Maths;
using static S3DE.Engine.Enums;
using S3DE.Engine.Graphics.Textures;
using S3DE.Engine.Graphics.Materials;

namespace S3DE.Engine.Graphics.OpenGL
{
    public class StandardPipeline : RenderPipeline
    {
        Deferred_Ambient_Material ambientMat;

        class Deferred_Vertex_Source : MaterialSource
        {
            public Deferred_Vertex_Source()
            {
                SetStage(ShaderStage.Vertex);
                SetSource(
                  "#version 330 core " + '\n'
                + "layout(location = 0)in vec3 position; " + '\n'
                + "layout(location = 1)in vec2 uvs;" + '\n'
                + "out Frag { " + '\n'
                + "vec2 uv;" + '\n'
                + "} frag;" + '\n'

                + "void main() " + '\n'
                + "{ " + '\n'
                + "gl_Position = vec4(position,1.0);" + '\n'
                + "frag.uv = uvs;" + '\n'
                + "}");
            }
        }

        class Deferred_Ambient_Material : Material
        {
            class Deferred_Ambient_Fragment_Source : MaterialSource
            {
                public Deferred_Ambient_Fragment_Source()
                {
                    SetStage(ShaderStage.Fragment);
                    SetSource(
                        "#version 330 core " + '\n'
                      + "uniform sampler2D color;" + '\n'
                      + "layout(location = 0) out vec4 gDiffuse; " + '\n'
                      + "layout(location = 1) out vec4 gLightColorIntensity; " + '\n'

                      + "struct AmbientLight " + '\n'
                      + "{ " + '\n'
                      + "vec3 color; " + '\n'
                      + "float intensity; " + '\n'
                      + "}; " + '\n'

                      + "uniform AmbientLight Ambient;" + '\n'

                      + "in Frag { " + '\n'
                      + "vec2 uv;" + '\n'
                      + "} frag;" + '\n'

                      + "void main() {" + '\n'
                      + "vec4 f = texture(color,frag.uv);" + '\n'
                      + "gDiffuse = vec4(f.rgb * Ambient.color * Ambient.intensity,f.a);" + '\n'
                      + "gLightColorIntensity = vec4(Ambient.color,Ambient.intensity);" + '\n'
                      + "}" + '\n'
                        );
                }
            }

            public Deferred_Ambient_Material() : base()
            {
                UsesProjectionMatrix = false;
                UsesRotationMatrix = false;
                UsesTransformMatrix = false;
                UsesViewMatrix = false;
            }

            protected override string[] GetUniforms()
            {
                return new string[] { "Ambient.color","Ambient.intensity","color"};
            }

            protected override MaterialSource GetSource(ShaderStage stage)
            {
                switch (stage)
                {
                    case ShaderStage.Vertex:
                        {
                            return new Deferred_Vertex_Source();
                        }
                    case ShaderStage.Fragment:
                        {
                            return new Deferred_Ambient_Fragment_Source();
                        }
                }

                throw new NotSupportedException($"ShaderStage: {stage.ToString()} is not supported by the deferred light pass of the StandardPipeline");
            }

            protected override void UpdateUniforms()
            {
                SetUniform("Ambient", SceneHandler.ActiveScene.Ambient);
                SetUniform("color", 1);
            }
        }


        //Deferred Lighting Material For AmbientLight.
        //Deferred Lighting Material For DirectionalLights.
        //Deferred Lighting Material For PointLight.

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

            #region Deferred Light
            Framebuffer def_li_fb = Framebuffer.Create(resolution);

            def_li_fb.AddBuffer(def_geo_fb.GetBuffer(TargetBuffer.Diffuse), TargetBuffer.Diffuse);
            def_li_fb.AddBuffer(def_geo_fb.GetBuffer(TargetBuffer.Light_Color_Intensity), TargetBuffer.Light_Color_Intensity);
            def_li_fb.AddBuffer(def_geo_fb.GetBuffer(TargetBuffer.Depth), TargetBuffer.Depth);

            def_li_fb.SetDrawBuffers(TargetBuffer.Diffuse, TargetBuffer.Light_Color_Intensity);
            rc.AddFrameBuffer(def_li_fb, FrameBufferTarget.Deferred_Light);
            #endregion

            #region Final
            Framebuffer fin = Framebuffer.Create(resolution);

            //Create a new RenderTexture2D for the Finals Diffuse buffer, as the final will be the blended result of the deferred and forward passes.
            fin.AddBuffer(def_geo_fb.GetBuffer(TargetBuffer.Color), TargetBuffer.Diffuse);
            fin.AddBuffer(def_geo_fb.GetBuffer(TargetBuffer.Depth), TargetBuffer.Depth);
            rc.AddFrameBuffer(fin, FrameBufferTarget.Final);

            #endregion

            return rc;
        }

        protected override void Init()
        {
            ambientMat = new Deferred_Ambient_Material();
        }

        protected override void RenderScene(GameScene scene, RenderCall renderCall)
        {
            RenderDeferredGeo(scene, renderCall.GetFrameBuffer(FrameBufferTarget.Deferred_Geometry));
            renderCall.GetFrameBuffer(FrameBufferTarget.Deferred_Geometry).GetBuffer(TargetBuffer.Color).Bind(1);
            RenderDeferredLight(scene, renderCall.GetFrameBuffer(FrameBufferTarget.Deferred_Light));
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

        void RenderDeferredLight(GameScene scene, Framebuffer fb)
        {
            fb.Bind();
            Renderer.Disable(Function.DepthTest);
            Renderer.Disable(Function.AlphaTest);
            ambientMat.UseMaterial();
            ScreenQuad.RenderToScreenQuad();
            //Bind the Deferred_Ambient_Material.
            //Set the ambientlight in the material.
            //Draw a screenquad using the material.
            
            //Bind the Deferred_DirectionalLight_Material
            //Do pretty much the same as in the ambientlight pass, might need to loop if too many directional lights.
            fb.Unbind();
        }
    }
}
