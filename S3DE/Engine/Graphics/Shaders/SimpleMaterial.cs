using S3DE.Engine.Graphics;
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
    public class SimpleMaterial : Material
    {
        Texture2D texture,normal;

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

                    "uniform mat4 view;",
                    "uniform mat4 projection;",
                    "uniform mat4 transform;",
                    "uniform mat4 rotation;",

                    "uniform CameraMatrices {",
                    "mat4 view;",
                    "mat4 projection;",
                    "} Camera;",

                    "uniform TransformMatrices {",
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
                    "frag.pos = (transform * vec4(position,1.0)).xyz;",
                    "frag.uv = uvs;",
                    "frag.TBN = mat3(rotation) * mat3(tangent,bitangent,normal);",
                    "gl_Position = (projection * view) * vec4(frag.pos,1.0);",
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

                    "uniform sampler2D tex;",
                    "uniform sampler2D normalMap;",

                    "void main() {",
                    "gFragColor = vec3(texture(tex,frag.uv).rgb);",
                    "gPosition = frag.pos;",
                    "gNormal = frag.TBN * (texture(normalMap,frag.uv).rgb * 2 - 1);",
                    "gSpecular = vec4(vec3(length(gFragColor) * 0.5773502691896258f),0.025f);",
                    "}");
            }
        }

        public SimpleMaterial() : base()
        {
            UsesProjectionMatrix = true;
            UsesViewMatrix = true;
            UsesTransformMatrix = true;
            UsesRotationMatrix = true;
            SupportsDeferredRendering = true;
            normal = ImageLoader.LoadFromFile(Environment.CurrentDirectory + @"\brickwall_normal.jpg");
            texture = ImageLoader.LoadFromFile(Environment.CurrentDirectory + @"\brickwall.jpg");
        }

        Texture2D createSampleTexture(S3DE_Vector2 resolution)
        {
            Texture2D tex = Texture2D.Create((int)resolution.X, (int)resolution.Y);
            float xMod = 255 / resolution.X;
            float yMod = 255 / resolution.Y;

            for (int x = 0; x < resolution.X; x++)
                for (int y = 0; y < resolution.Y; y++)
                    tex.SetPixel(x, y, new Color((byte)(x * xMod), (byte)(y * yMod), (byte)(((x * xMod) + (y * yMod)) / 2), 255));

            tex.FilterMode = FilterMode.Trilinear;
            tex.AnisotropicSamples = AnisotropicSamples.x16;
            tex.CalculateMipMapCount();
            tex.Apply();
            return tex;
        }

        protected override MaterialSource GetSource(ShaderStage stage,RenderPass pass)
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
            return new string[] {"tex", "normalMap"};
        }

        protected override void UpdateUniforms(RenderPass pass)
        {
            SetTextures(new string[] { "tex", "normalMap" }, new ITexture[] { texture, normal });
            //SetTexture("tex",texture);
            //SetTexture("normalMap", normal);
        }
    }
}
