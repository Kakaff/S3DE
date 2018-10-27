using S3DE.Maths;
using S3DE.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Components
{
    public sealed class Camera : EntityComponent
    {
        public Matrix4x4 ViewMatrix => viewMatrix;

        public Matrix4x4 ProjectionMatrix => projMatrix;

        Matrix4x4 viewMatrix, projMatrix;
        float zNear, zFar, fov;
        
        public float ZNear
        {
            get => zNear;
            set
            {
                zNear = value;
                RecalculateProjectionMatrix();
            }
        }

        public float ZFar
        {
            get => zFar;
            set
            {
                zFar = value;
                RecalculateProjectionMatrix();
            }
        }

        public float FoV
        {
            get => fov;
            set
            {
                fov = value;
                RecalculateProjectionMatrix();
            }
        }
        protected override void OnCreation() {
            fov = 75f;
            zNear = 0.01f;
            zFar = 2000f;

            RecalculateMatrices();
        }

        protected override void PreRender()
        {
            if (Entity.transform.HasChanged)
                RecalculateViewMatrix();

            if (Game.ResolutionChanged)
                RecalculateProjectionMatrix();
        }
        
        public void RecalculateMatrices()
        {
            RecalculateViewMatrix();
            RecalculateProjectionMatrix();
        }

        void RecalculateViewMatrix() =>
            viewMatrix = Matrix4x4.CreateViewMatrix(transform.Position, transform.Forward, transform.Up);

        void RecalculateProjectionMatrix() =>
            projMatrix = Matrix4x4.CreateProjectionMatrix_FoV(fov, zNear, zFar, S3DE.Window.AspectRatio);


    }
}
