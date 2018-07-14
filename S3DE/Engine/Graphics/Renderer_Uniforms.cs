using S3DE.Engine.Graphics.Lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics
{
    public abstract partial class Renderer
    {

        protected abstract uint GetUniform_BlockLocation(string uniformBlockName);
        protected abstract int GetUniform_Location(string uniformName);

        protected abstract void SetUniform(int loc, int value);
        protected abstract void SetUniform(int loc, int[] values);
        protected abstract void SetUniform(int loc, float value);
        protected abstract void SetUniform(int loc, float[] values);
        protected abstract void SetUniform(int loc, Maths.Matrix4x4 matrix);
        protected abstract void SetUniform(int loc, System.Numerics.Vector3 vector);
        protected abstract void SetUniform(int loc, ILight light);
        protected abstract void SetUniform(int loc, IDirectionalLight dirLight);
        protected abstract void SetUniform(int loc, Color color);

        protected abstract void SetUniformBlock(int loc, UniformBuffer buff);
        protected abstract void SetUniformBlocks(int[] locations, UniformBuffer[] buffers);
        
        protected abstract void SetUniform(string name, int value);
        protected abstract void SetUniform(string name, int[] values);
        protected abstract void SetUniform(string name, float value);
        protected abstract void SetUniform(string name, float[] values);
        protected abstract void SetUniform(string name, Maths.Matrix4x4 matrix);
        protected abstract void SetUniform(string name, System.Numerics.Vector3 vector);
        protected abstract void SetUniform(string name, ILight light);
        protected abstract void SetUniform(string name, IDirectionalLight dirLight);
        protected abstract void SetUniform(string name, Color color);

        protected abstract void SetUniformBlock(string name, UniformBuffer buff);
        protected abstract void SetUniformBlocks(string[] names, UniformBuffer[] buffers);

        public static void Set_Uniform(int loc, int value) => ActiveRenderer.SetUniform(loc, value);
        public static void Set_Uniform(int loc, int[] values) => ActiveRenderer.SetUniform(loc, values);
        public static void Set_Uniform(int loc, float value) => ActiveRenderer.SetUniform(loc, value);
        public static void Set_Uniform(int loc, float[] values) => ActiveRenderer.SetUniform(loc, values);
        public static void Set_Uniform(int loc, S3DE.Maths.Matrix4x4 matrix) => ActiveRenderer.SetUniform(loc, matrix);
        public static void Set_Uniform(int loc, System.Numerics.Vector3 vector) => ActiveRenderer.SetUniform(loc, vector);
        public static void Set_Uniform(int loc, ILight light) => ActiveRenderer.SetUniform(loc, light);
        public static void Set_Uniform(int loc, IDirectionalLight dirLight) => ActiveRenderer.SetUniform(loc, dirLight);
        public static void Set_Uniform(int loc, Color color) => ActiveRenderer.SetUniform(loc, color);

        public static void Set_Uniform(string name, int value) => ActiveRenderer.SetUniform(name, value);
        public static void Set_Uniform(string name, int[] values) => ActiveRenderer.SetUniform(name, values);
        public static void Set_Uniform(string name, float value) => ActiveRenderer.SetUniform(name, value);
        public static void Set_Uniform(string name, float[] values) => ActiveRenderer.SetUniform(name, values);
        public static void Set_Uniform(string name, S3DE.Maths.Matrix4x4 matrix) => ActiveRenderer.SetUniform(name, matrix);
        public static void Set_Uniform(string name, System.Numerics.Vector3 vector) => ActiveRenderer.SetUniform(name, vector);
        public static void Set_Uniform(string name, ILight light) => ActiveRenderer.SetUniform(name, light);
        public static void Set_Uniform(string name, IDirectionalLight dirLight) => ActiveRenderer.SetUniform(name, dirLight);
        public static void Set_Uniform(string name, Color color) => ActiveRenderer.SetUniform(name, color);


        public static void Set_UniformBlock(int loc, UniformBuffer buff) => ActiveRenderer.SetUniformBlock(loc, buff);
        public static void Set_UniformBlock(string name, UniformBuffer buff) => ActiveRenderer.SetUniformBlock(name, buff);
        public static void Set_UniformBlocks(int[] locations, UniformBuffer[] buffers) => ActiveRenderer.SetUniformBlocks(locations, buffers);
        public static void Set_UniformBlocks(string[] names, UniformBuffer[] buffers) => ActiveRenderer.SetUniformBlocks(names, buffers);

        public static uint GetUniformBlockLocation(string uniformBlockName) => ActiveRenderer.GetUniform_BlockLocation(uniformBlockName);
        public static int GetUniformLocation(string uniformName) => ActiveRenderer.GetUniform_Location(uniformName);
    }
}
