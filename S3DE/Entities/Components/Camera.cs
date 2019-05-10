using S3DE.Graphics;
using S3DE.Maths;

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

            if (Renderer.RenderResolutionChanged)
                RecalculateProjectionMatrix();
        }
        
        public void RecalculateMatrices()
        {
            RecalculateViewMatrix();
            RecalculateProjectionMatrix();
        }

        public Matrix4x4 GetCameraMatrix()
        {
            return viewMatrix * projMatrix;
        }

        void RecalculateViewMatrix()
        {
            Matrix4x4 tm = Matrix4x4.CreateTranslationMatrix(-transform.Position);
            Matrix4x4 rM = transform.Rotation.Conjugate().ToRotationMatrix();
            viewMatrix =  tm * rM;
        }

        void RecalculateProjectionMatrix() =>
            projMatrix = Matrix4x4.CreatePerspectiveFieldOfView(fov, zNear, zFar, S3DE.Window.AspectRatio);


    }
}
