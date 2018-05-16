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
using S3DE.Engine.Graphics.Lights;

namespace S3DE.Engine.Graphics.OpGL
{
    public class StandardPipeline : RenderPipeline
    {
        static long passDuration,def_geo_Dur,def_li_dur;

        public static long RenderDuration => passDuration;
        public static long Deferred_Geometry_Duration => def_geo_Dur;
        public static long Deferred_Light_Duration => def_li_dur;

        Deferred_Ambient_Material ambientMat;
        Deferred_Directional_Material dirLightMat;

        //Deferred Lighting Material For AmbientLight.
        //Deferred Lighting Material For DirectionalLights.
        //Deferred Lighting Material For PointLight.

        protected override RenderCall CreateRenderCall(Vector2 resolution)
        {
            RenderCall rc = new RenderCall(resolution);

            #region Deferred Geometry
            Framebuffer def_geo_fb = Framebuffer.Create(resolution);
            def_geo_fb.AddBuffer(InternalFormat.RGB8, PixelFormat.RGB, PixelType.UByte, FilterMode.Nearest, TargetBuffer.Color);
            //Normal
            def_geo_fb.AddBuffer(InternalFormat.RGB16F, PixelFormat.RGB, PixelType.Float16, FilterMode.Nearest, TargetBuffer.Normal);
            //Specular
            def_geo_fb.AddBuffer(InternalFormat.RGBA, PixelFormat.RGBA, PixelType.UByte, FilterMode.Nearest, TargetBuffer.Specular);
            //Position
            def_geo_fb.AddBuffer(InternalFormat.RGB16F, PixelFormat.RGB, PixelType.Float16, FilterMode.Nearest, TargetBuffer.Position);
            //Depth
            def_geo_fb.AddBuffer(InternalFormat.DepthComponent16, PixelFormat.Depth, PixelType.Float16, FilterMode.Nearest, TargetBuffer.Depth);

            rc.AddFrameBuffer(def_geo_fb,FrameBufferTarget.Deferred_Geometry);
            bool complete = def_geo_fb.IsComplete;
            def_geo_fb.SetDrawBuffers(TargetBuffer.Color,
                TargetBuffer.Normal, TargetBuffer.Position,TargetBuffer.Specular);

            #endregion

            #region Deferred Light
            DualFrameBuffer def_li_fb = DualFrameBuffer.Create(resolution);
            //Diffuse
            def_li_fb.AddBuffer(InternalFormat.RGBA, PixelFormat.RGBA, PixelType.UByte, FilterMode.Nearest, TargetBuffer.Diffuse);
            //Light Color & Intensity Alpha = Intensity
            def_li_fb.AddBuffer(InternalFormat.RGBA16F, PixelFormat.RGBA, PixelType.Float16, FilterMode.Nearest, TargetBuffer.Light_Color_Intensity);
            def_li_fb.AddBuffer(def_geo_fb.GetBuffer(TargetBuffer.Depth), TargetBuffer.Depth);

            def_li_fb.SetDrawBuffers(TargetBuffer.Diffuse, TargetBuffer.Light_Color_Intensity);
            rc.AddFrameBuffer(def_li_fb, FrameBufferTarget.Deferred_Light);
            #endregion

            #region Final
            Framebuffer fin = Framebuffer.Create(resolution);

            //Create a new RenderTexture2D for the Finals Diffuse buffer, as the final will be the blended result of the deferred and forward passes.
            fin.AddBuffer(def_li_fb.GetBuffer(TargetBuffer.Diffuse), TargetBuffer.Diffuse);
            fin.AddBuffer(def_geo_fb.GetBuffer(TargetBuffer.Depth), TargetBuffer.Depth);
            rc.AddFrameBuffer(fin, FrameBufferTarget.Final);

            #endregion

            return rc;
        }

        class DualFrameBuffer : Framebuffer
        {
            int currentBuffer;
            Framebuffer[] fbs;

            public override bool IsComplete => throw new NotImplementedException();

            public override void AddBuffer(RenderTexture2D buffer, BufferAttachment attachment)
            {
                fbs[0].AddBuffer(buffer, attachment);
                fbs[1].AddBuffer(buffer, attachment);
            }

            public override void AddBuffer(InternalFormat internalFormat, PixelFormat pixelFormat, PixelType colorType, FilterMode filter, BufferAttachment attachment)
            {
                fbs[0].AddBuffer(internalFormat, pixelFormat, colorType, filter, attachment);
                fbs[1].AddBuffer(internalFormat, pixelFormat, colorType, filter, attachment);
            }

            public override void AddBuffer(InternalFormat internalFormat, PixelFormat pixelFormat, PixelType colorType, FilterMode filter, BufferAttachment attachment, out RenderTexture2D renderTexture)
            {
                throw new NotSupportedException("Method not supported by DualFrameBuffer, consider using a normal framebuffer.");
            }

            public override void Bind()
            {
                fbs[currentBuffer].Bind();
                fbs[currentBuffer].SetAsActive();
            }

            public override void Clear()
            {
                fbs[currentBuffer].Clear();
                Swap();
                Bind();
                fbs[currentBuffer].Clear();
                Swap();
                Bind();
            }

            public override void Clear(bool color, bool depth, bool stencil)
            {
                fbs[currentBuffer].Clear(color, depth, stencil);
                Swap();
                Bind();
                fbs[currentBuffer].Clear(color, depth, stencil);
                Swap();
                Bind();
            }

            public override void Clear(params BufferAttachment[] attachments)
            {
                fbs[currentBuffer].Clear(attachments);
                Swap();
                Bind();
                fbs[currentBuffer].Clear(attachments);
                Swap();
                Bind();
            }

            public override RenderTexture2D GetBuffer(BufferAttachment attachment)
            {
                return fbs[currentBuffer].GetBuffer(attachment);
            }

            public RenderTexture2D GetAlternativeBuffer(BufferAttachment attachment)
            {
                return fbs[currentBuffer == 0 ? 1 : 0].GetBuffer(attachment);
            }

            public RenderTexture2D GetAlternativeBuffer(TargetBuffer targetBuffer)
            {
                return fbs[currentBuffer == 0 ? 1 : 0].GetBuffer(targetBuffer);
            }

            public void Swap()
            {
                currentBuffer = (currentBuffer > 0) ? 0 : 1;
            }

            public override void Unbind()
            {
                fbs[currentBuffer].Unbind();
            }

            public override void SetDrawBuffers(params TargetBuffer[] buffers)
            {
                BufferAttachment[] attachments = new BufferAttachment[buffers.Length];
                for (int i = 0; i < buffers.Length; i++)
                    attachments[i] = (BufferAttachment)buffers[i];

                SetDrawBuffers(attachments);
            }

            public override void SetDrawBuffers(params BufferAttachment[] attachments)
            {
                fbs[0].SetDrawBuffers(attachments);
                fbs[1].SetDrawBuffers(attachments);
            }

            DualFrameBuffer(Vector2 res)
            {
                fbs = new Framebuffer[2];
                fbs[0] = Framebuffer.Create(res);
                fbs[1] = Framebuffer.Create(res);
            }

            //I'm lazy so this will suffice.
            public static new DualFrameBuffer Create(Vector2 resolution)
            {
                DualFrameBuffer dfb = new DualFrameBuffer(resolution);
                return dfb;
            }
        }

        protected override void Init()
        {
            ambientMat = new Deferred_Ambient_Material();
            dirLightMat = new Deferred_Directional_Material();
        }

        protected override void RenderScene(GameScene scene, RenderCall renderCall)
        {
            SetCurrentRenderPass(RenderPass.Deferred);
            RenderDeferredGeo(scene, renderCall.GetFrameBuffer(FrameBufferTarget.Deferred_Geometry));
            renderCall.GetFrameBuffer(FrameBufferTarget.Deferred_Geometry).GetBuffer(TargetBuffer.Color).Bind();
            renderCall.GetFrameBuffer(FrameBufferTarget.Deferred_Geometry).GetBuffer(TargetBuffer.Normal).Bind();
            renderCall.GetFrameBuffer(FrameBufferTarget.Deferred_Geometry).GetBuffer(TargetBuffer.Position).Bind();
            renderCall.GetFrameBuffer(FrameBufferTarget.Deferred_Geometry).GetBuffer(TargetBuffer.Specular).Bind();
            
            RenderDeferredLight(scene, renderCall.GetFrameBuffer(FrameBufferTarget.Deferred_Light));
        }

        void RenderDeferredGeo(GameScene scene, Framebuffer fb)
        {
            fb.Bind();
            fb.SetAsActive();
            fb.Clear();
            Renderer.Enable(Function.DepthTest);
            Renderer.Enable(Function.AlphaTest);
            Renderer.AlphaFunction(AlphaFunction.Never, 0f);
            DrawScene(scene);
            FinalizeRenderPass();
        }

        void RenderDeferredLight(GameScene scene, Framebuffer fb)
        {
            ambientMat.ColorTex = Framebuffer.ActiveFrameBuffer.GetBuffer(TargetBuffer.Color);
            dirLightMat.Color = Framebuffer.ActiveFrameBuffer.GetBuffer(TargetBuffer.Color);
            dirLightMat.Normal = Framebuffer.ActiveFrameBuffer.GetBuffer(TargetBuffer.Normal);
            dirLightMat.Position = Framebuffer.ActiveFrameBuffer.GetBuffer(TargetBuffer.Position);
            dirLightMat.Specular = Framebuffer.ActiveFrameBuffer.GetBuffer(TargetBuffer.Specular);
            DualFrameBuffer dfb = (DualFrameBuffer)fb;
            dfb.Bind();
            dfb.SetAsActive();
            dfb.Clear(true,false,false);
            Renderer.Disable(Function.DepthTest);
            Renderer.Disable(Function.AlphaTest);
             
            ambientMat.UseMaterial(RenderPass.Deferred);
            ScreenQuad.RenderToScreenQuad();
            FinalizeRenderPass();
            //DirectionalLightPass
            IDirectionalLight[] dirLights = scene.DirectionalLights;
            foreach (IDirectionalLight dl in dirLights)
            {
                dfb.Swap();
                dfb.Bind();
                dirLightMat.Diffuse = dfb.GetAlternativeBuffer(TargetBuffer.Diffuse);
                dirLightMat.DirectionalLight = dl;
                dirLightMat.UseMaterial(RenderPass.Deferred);
                ScreenQuad.RenderToScreenQuad();
                FinalizeRenderPass();
            }
            
            dfb.Unbind();
        }

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
            public RenderTexture2D ColorTex;

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
                SupportsDeferredRendering = true;
            }

            protected override string[] GetUniforms()
            {
                return new string[] {"Ambient.color", "Ambient.intensity", "color" };
            }

            protected override MaterialSource GetSource(ShaderStage stage, RenderPass pass)
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

            protected override void UpdateUniforms(RenderPass pass)
            {
                SetUniform("Ambient", SceneHandler.ActiveScene.Ambient);
                SetTexture("color", ColorTex);
            }
        }

        class Deferred_Directional_Material : Material
        {
            public RenderTexture2D Color, Normal, Diffuse, Position, Specular;

            class Deferred_Directional_Fragment_Source : MaterialSource
            {
                public Deferred_Directional_Fragment_Source()
                {
                    SetStage(ShaderStage.Fragment);
                    SetSource(
                        "#version 330 core " + '\n'
                      + "uniform sampler2D color;" + '\n'
                      + "uniform sampler2D normal;" + '\n'
                      + "uniform sampler2D specular;" + '\n'
                      + "uniform sampler2D diffuse;" + '\n'
                      + "uniform sampler2D position;" + '\n'
                      + "uniform vec3 camPos;" + '\n'
                      + "layout(location = 0) out vec4 gDiffuse; " + '\n'
                      + "layout(location = 1) out vec4 gLightColorIntensity; " + '\n'

                      + "struct DirectionalLight " + '\n'
                      + "{ " + '\n'
                      + "vec3 color; " + '\n'
                      + "float intensity; " + '\n'
                      + "vec3 direction; " + '\n'
                      + "}; " + '\n'

                      + "uniform DirectionalLight DirLight;" + '\n'

                      + "in Frag { " + '\n'
                      + "vec2 uv;" + '\n'
                      + "} frag;" + '\n'

                      + "float calcSpecular(float specularity,vec3 normal, vec3 fragpos, vec3 camPos, vec3 lightDir) {" + '\n'
                      + "vec3 viewDir = normalize(camPos - fragpos);" + '\n'
                      + "vec3 reflectDir = reflect(lightDir,normal);" + '\n'
                      + "float d = dot(viewDir,reflectDir);" + '\n'
                      + "if (d < 0) return 0;" + '\n'
                      + "return min(pow(d,int(specularity * 255)),0.15f + (0.85f * specularity) * d);" + '\n'
                      + "}" + '\n'

                      + "void main() {" + '\n'
                      + "vec4 f = texture(color,frag.uv);" + '\n'
                      + "vec3 norm = texture(normal,frag.uv).xyz;" + '\n'
                      + "vec4 specMap = texture(specular,frag.uv);" + '\n'
                      + "vec3 pos = texture(position,frag.uv).xyz;" + '\n'

                      + "float intensity = max(dot(norm,-DirLight.direction),0.0) * DirLight.intensity;" + '\n'
                      + "vec3 spec = specMap.xyz * calcSpecular(specMap.w,norm,pos,camPos,DirLight.direction);" + '\n'
                      
                      + "vec4 diffColor = vec4((texture(color,frag.uv).rgb * DirLight.color) * intensity,0);" + '\n'
                      + "gDiffuse = texture(diffuse,frag.uv) + diffColor + "
                      + "vec4(spec * DirLight.color * intensity,0);" + '\n'
                      + "" + '\n'
                      + "}" + '\n'
                        );
                }
            }

            IDirectionalLight dirLight;

            public IDirectionalLight DirectionalLight
            {
                get => dirLight;
                set => dirLight = value;
            }

            public Deferred_Directional_Material()
            {
                UsesProjectionMatrix = false;
                UsesRotationMatrix = false;
                UsesTransformMatrix = false;
                UsesViewMatrix = false;
                SupportsDeferredRendering = true;
            }

            class TestDirLight : IDirectionalLight
            {
                public Vector3 LightDirection => new Vector3(-0.35f,-0.5f,0.5f).normalized;

                public bool CastsShadows => false;

                public Color Color => Color.White;

                public float Intensity => 1f;
            }

            protected override MaterialSource GetSource(ShaderStage stage,RenderPass pass)
            {
                switch (stage)
                {
                    case ShaderStage.Vertex: return new Deferred_Vertex_Source();
                    case ShaderStage.Fragment: return new Deferred_Directional_Fragment_Source();
                }
                throw new NotImplementedException();
            }

            protected override void UpdateUniforms(RenderPass pass)
            {
                SetTexture("color", Color);
                SetTexture("normal", Normal);
                SetTexture("diffuse", Diffuse);
                SetTexture("position", Position);
                SetTexture("specular", Specular);
                SetUniform("DirLight", dirLight);
                SetUniform("camPos", SceneHandler.ActiveScene.ActiveCamera.transform.Position);
            }

            protected override string[] GetUniforms()
            {
                return new string[] {"color", "normal","diffuse","position","specular","camPos", "DirLight.intensity", "DirLight.color", "DirLight.direction" };
            }
        }
    }
}
