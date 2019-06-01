using S3DE.Components;
using S3DE.Graphics.Shaders;
using S3DE.Graphics.Textures;
using S3DE.Maths;
using S3DECore.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static S3DE.Graphics.Rendering.RenderPass;

namespace S3DE.Graphics.Rendering
{
    internal sealed class Drawcall
    {
        public bool IsValid { get; private set; }
        public bool TexturesChanged { get; private set; }
        public bool MaterialChanged { get; private set; }

        public int MaterialID => shadProgID;
        public int TargetRenderPass => trgRenderPass;

        DrawCallContainer parentContainer;

        Meshrenderer mr;
        int shadProgID;
        int trgRenderPass;

        UniformUpdate[] uniformUpdates;
        int[] textures;

        private Drawcall() { }

        public Drawcall(Meshrenderer mr)
        {
            this.mr = mr;
            TexturesChanged = true;
            MaterialChanged = true;
            shadProgID = -1;
            trgRenderPass = -1;
        }

        public bool Validate()
        {
            TexturesChanged = false;
            MaterialChanged = false;

            if (mr.Material == null)
            {
                IsValid = false;
                shadProgID = 0;
                Array.Clear(uniformUpdates, 0, uniformUpdates.Length);
                if (parentContainer != null)
                    parentContainer.RemoveDrawCall(this);
                return false;
            }
            else if (mr.Material.ShaderProgramID != shadProgID)
            {
                MaterialChanged = true;
                shadProgID = mr.Material.ShaderProgramID;
                (uniformUpdates,textures) = mr.Material.GetUniforms();
            }
            IsValid = true;
            return true;
        }

        public void PerformUniformUpdates()
        {
            for (int i = 0; i < uniformUpdates.Length; i++)
                if (uniformUpdates[i] != null)
                    uniformUpdates[i].Perform();
        }

        public void Draw()
        {
            mr.Material.UseMaterial();
            PerformUniformUpdates();
            mr.Mesh.Draw();
        }

        public void SetUniformUpdateMatrixf4(int location, ref Matrix4x4 matr)
        {
            UniformUpdate uu = uniformUpdates[location];
            if (uu != null && uu.UniformType == UniformType.Matrixf4x4)
                ((UniformUpdateMatrix4x4)uu).Value = matr;
            else if (uu == null)
                ThrowNullUniformException(location);
            else if (uu.UniformType != UniformType.Matrixf4x4)
                ThrowInvalidValueException(uu, location, UniformType.Matrixf4x4);
        }
        
        static void ThrowNullUniformException(int location)
        {
            throw new NullReferenceException($"Uniform {location} does not exist!");
        }

        static void ThrowInvalidValueException(UniformUpdate uu,int location, UniformType ut)
        {
            throw new InvalidOperationException($"Uniform {location} expects a {Enum.GetName(typeof(UniformType), uu.UniformType)} but received a {Enum.GetName(typeof(UniformType),ut)}");
        }

        public void SetUniformUpdateTex2D(int location, RenderTexture2D tex)
        {
            UniformUpdate uu = uniformUpdates[location];
            if (uu != null && uu.UniformType == UniformType.TextureSampler2D)
            {
                UniformUpdateTex2D uut2d = uu as UniformUpdateTex2D;
                if (textures[uut2d.TextureIndex] == tex.GetInstanceID() && uut2d.Texture != null)
                    return;
                else if (uut2d.Texture == null || uut2d.Texture.GetInstanceID() != tex.GetInstanceID())
                {
                    if (!TexturesChanged)
                    {
                        if (parentContainer != null)
                            parentContainer.RemoveDrawCall(this);
                        TexturesChanged = true;
                    }
                    textures[uut2d.TextureIndex] = tex.GetInstanceID();
                    uut2d.Texture = tex;
                }
            }
            else if (uu == null)
                ThrowNullUniformException(location);
            else if (uu.UniformType != UniformType.TextureSampler2D)
                ThrowInvalidValueException(uu, location, UniformType.TextureSampler2D);
        }
    }
}
