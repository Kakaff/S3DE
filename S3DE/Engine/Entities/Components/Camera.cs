﻿using S3DE.Engine.Graphics;
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
        static Camera mc;

        public static Camera MainCamera
        {
            get => GetMainCamera();
            set => mc = value;
        }

        public static Camera ActiveCamera => MainCamera;

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
            zNear = 0.1f;
            zFar = 2000f;

            RecalculateViewMatrix();
            RecalculateProjectionMatrix();
        }

        protected override void PreDraw()
        {
            if (gameEntity.transform.HasChanged)
            {
                Console.WriteLine("Camera has moved, new position is now: " + transform.Position);
                RecalculateViewMatrix();
            }
        }

        static Camera GetMainCamera()
        {

            if (mc == null)
            {
                Console.WriteLine("Creating new maincamera");
                mc = SceneHandler.ActiveScene.CreateEntityInternal().AddComponent<Camera>();
            }

            return mc;
        }

        public void RecalculateMatrices()
        {
            RecalculateViewMatrix();
            RecalculateProjectionMatrix();
        }
        void RecalculateViewMatrix() => viewMatrix = Matrix4x4.CreateViewMatrix(transform.Position, transform.Forward, transform.Up);

        void RecalculateProjectionMatrix() => projMatrix = Matrix4x4.CreateProjectionMatrix_FoV(fov, zNear, zFar, Window.AspectRatio);


    }
}
