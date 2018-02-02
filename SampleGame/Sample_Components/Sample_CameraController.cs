﻿using S3DE.Engine;
using S3DE.Engine.Entities;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static S3DE.Engine.Enums;

namespace SampleGame.Sample_Components
{
    public class Sample_CameraController : EntityComponent
    {
        Vector3 movDir;

        float xRot = 0, yRot = 0;

        protected override void Update()
        {
            movDir = Vector3.Zero;

            if (Input.GetKey(KeyCode.W))
                movDir += transform.Forward;

            if (Input.GetKey(KeyCode.S))
                movDir += transform.Backward;

            if (Input.GetKey(KeyCode.A))
                movDir += transform.Left;

            if (Input.GetKey(KeyCode.D))
                movDir += transform.Right;

            if (movDir != Vector3.Zero)
                transform.Translate(movDir.normalized, 1f * DeltaTime);

            if (Input.CursorMoved)
            {
                xRot += Input.MouseDelta.x * 45f;
                yRot += Input.MouseDelta.y * 45f;

                Quaternion q1 = Quaternion.CreateFromAxisAngle(Vector3.Up, xRot);
                Quaternion q2 = Quaternion.CreateFromAxisAngle(Vector3.Right, yRot);
                
                transform.Rotation = q1 * q2;
            }


        }

        float Clamp(float min, float max, float val) => val > max ? max : val < min ? min : val;

        float Normalize(float start, float end, float value)
        {
            float width = end - start;
            float offset = value - start;

            return (float)(offset - (System.Math.Floor(offset / width) * width)) + start;
        }

        protected override void OnCreation()
        {
            
        }
    }
}