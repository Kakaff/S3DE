using S3DECore.Graphics;
using S3DECore.Math;

namespace S3DE.Components
{
    public sealed class Camera : EntityComponent
    {
        public Matrix4x4 ViewMatrix { get {
                if (transform.HasChanged && !isUpdated)
                {
                    RecalculateViewMatrix();
                    isUpdated = true;
                }

                return viewMatrix;
            }
        }

        public Matrix4x4 ProjectionMatrix => projMatrix;

        Matrix4x4 viewMatrix, projMatrix;
        float zNear, zFar, fov;
        bool isUpdated = false; //Change this to be set to false whenever the parent transform changes. (via delegate)

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

            Renderer.OnRenderResolutionChanged += RecalculateProjectionMatrix;
        }

        protected override void PreRender()
        {
            if (Entity.transform.HasChanged)
                RecalculateViewMatrix();
        }

        protected override void PostRender()
        {
            isUpdated = false;
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
            viewMatrix = Matrix4x4.CreateTranslationMatrix(transform.Position.Inverse()) *
                Matrix4x4.CreateRotationMatrix(transform.Rotation.Conjugate());
        }

        void RecalculateProjectionMatrix(Vector2 oldRes, Vector2 newRes) => RecalculateProjectionMatrix();

        void RecalculateProjectionMatrix()
        {
            Vector2 v = Renderer.RenderResolution;
            float aspect = v.x / v.y;
            projMatrix = Matrix4x4.CreateFoVPerspectiveMatrix(fov, zNear, zFar, aspect);
        }


    }
}
