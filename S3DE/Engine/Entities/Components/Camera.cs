using S3DE.Engine.Data;
using S3DE.Engine.Graphics;
using S3DE.Engine.Scenes;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Entities.Components
{
    public sealed class Camera : EntityComponent
    {
        public Matrix4x4 ViewMatrix => viewMatrix;

        public Matrix4x4 ProjectionMatrix => projMatrix;

        bool updateUBO;

        Matrix4x4 viewMatrix, projMatrix;
        float zNear, zFar, fov;
        S3DE_UniformBuffer ubo;

        public S3DE_UniformBuffer UniformBuffer => ubo;

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
            updateUBO = true;

        }

        protected override void PreRender()
        {

            if (gameEntity.transform.HasChanged)
            {
                RecalculateViewMatrix();
                updateUBO = true;
            }

            if (Game.ResolutionChanged)
            {
                RecalculateProjectionMatrix();
                updateUBO = true;
            }

            if (updateUBO)
            {
                if (ubo == null)
                    ubo = S3DE_UniformBuffer.Create();
                ubo.SetData(Matrix4x4.ToByteArray(ViewMatrix, ProjectionMatrix));

                updateUBO = false;
            }
        }
        
        public void RecalculateMatrices()
        {
            RecalculateViewMatrix();
            RecalculateProjectionMatrix();
            updateUBO = true;
        }

        void RecalculateViewMatrix()
        {
            viewMatrix = Matrix4x4.CreateViewMatrix(transform.Position, transform.Forward, transform.Up);
            updateUBO = true;
        }

        void RecalculateProjectionMatrix()
        {
            projMatrix = Matrix4x4.CreateProjectionMatrix_FoV(fov, zNear, zFar, Window.AspectRatio);
            updateUBO = true;
        }


    }
}
