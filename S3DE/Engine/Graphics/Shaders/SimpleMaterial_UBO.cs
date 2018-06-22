using S3DE.Engine.Data;
using S3DE.Engine.Graphics.Materials;
using S3DE.Engine.Graphics.Textures;
using S3DE.Engine.IO;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.Shaders
{
    public class SimpleMaterial_UBO : Material
    {

        Texture2D diffuse, normal;

        private class SimpleVertSource : MaterialSource
        {
            public SimpleVertSource()
            {
                SetStage(ShaderStage.Vertex);
                SetSource(
                    "#version 330",

                    "layout(location = 0)in vec3 position;",
                    "layout(location = 1)in vec2 uvs;",
                    "layout(location = 2)in vec3 normal;",
                    "layout(location = 3)in vec3 tangent;",
                    "layout(location = 4)in vec3 bitangent;",
                    
                    "layout(std140) uniform CameraMatrices {",
                    "mat4 view;",
                    "mat4 projection;",
                    "} Camera;",

                    "layout(std140) uniform TransformMatrices {",
                    "mat4 transform;",
                    "mat4 translation;",
                    "mat4 rotation;",
                    "mat4 scale;",
                    "} Transform;",

                    "out Frag {",
                    "vec3 pos;",
                    "vec2 uv;",
                    "mat3 TBN;",
                    "} frag;",

                    "void main() {",
                    "frag.pos = (Transform.transform * vec4(position,1.0)).xyz;",
                    "frag.uv = uvs;",
                    "frag.TBN = mat3(Transform.rotation) * mat3(tangent,bitangent,normal);",
                    "gl_Position = (Camera.projection * Camera.view) * vec4(frag.pos,1.0);",
                    "}");
            }
        }

        private class SimpleFragSource : MaterialSource
        {
            public SimpleFragSource()
            {
                SetStage(ShaderStage.Fragment);

                SetSource(
                    "#version 330",

                    "layout(location = 0) out vec3 gFragColor;",
                    "layout(location = 1) out vec3 gNormal;",
                    "layout(location = 2) out vec3 gPosition;",
                    "layout(location = 3) out vec4 gSpecular;",

                    "in Frag {",
                    "vec3 pos;",
                    "vec2 uv;",
                    "mat3 TBN;",
                    "} frag;",

                    "uniform sampler2D diffuse;",
                    "uniform sampler2D normal;",

                    "void main() {",
                    "gFragColor = vec3(texture(diffuse,frag.uv).rgb);",
                    "gPosition = frag.pos;",
                    "gNormal = frag.TBN * (texture(normal,frag.uv).rgb * 2 - 1);",
                    "gSpecular = vec4(1,1,1,0.075f);",
                    "}");
                //vec3(length(gFragColor) * 0.5773502691896258f)
            }
        }

        public SimpleMaterial_UBO() : base()
        {
            UsesTransformMatrices = true;
            UsesCameraMatrices = true;
            CameraUniformBlockName = "CameraMatrices";
            TransformUniformBlockName = "TransformMatrices";

            SupportsDeferredRendering = true;
            normal = ImageLoader.LoadFromFile(Environment.CurrentDirectory + @"\brickwall_normal.jpg");
            diffuse = ImageLoader.LoadFromFile(Environment.CurrentDirectory + @"\brickwall.jpg");
        }

        protected override MaterialSource GetSource(ShaderStage stage, RenderPass pass)
        {
            switch (stage)
            {
                case ShaderStage.Vertex:
                    {
                        return new SimpleVertSource();
                    }
                case ShaderStage.Fragment:
                    {
                        return new SimpleFragSource();
                    }
                default:
                    {
                        throw new NotSupportedException();
                    }
            }
        }

        protected override string[] GetUniforms()
        {
            return new string[] { "diffuse", "normal" };
        }

        protected override void UpdateUniforms(RenderPass pass)
        {
            SetTexture("diffuse", diffuse);
            SetTexture("normal", normal);
        }
    }
}
