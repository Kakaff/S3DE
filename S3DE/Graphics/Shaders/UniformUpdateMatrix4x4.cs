using S3DE.Maths;
using S3DECore.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Shaders
{
    internal sealed unsafe class UniformUpdateMatrix4x4 : UniformUpdate
    { 
        public Matrix4x4 Value { get => matr; set => matr = value; }

        Matrix4x4 matr;
        public override UniformType UniformType => UniformType.Matrixf4x4;

        public UniformUpdateMatrix4x4(int uniformLocation) : base(uniformLocation) { }

        public UniformUpdateMatrix4x4(int uniformLocation, Matrix4x4 matr) : base(uniformLocation) { Value = matr; }

        public override void Perform()
        {
            unsafe
            {
                fixed (float* m = &matr.m00) {
                    Uniforms.SetUniformMatrixf4v((uint)UniformLocation, 1, true, m);
                }
            }
        }
    }
}
