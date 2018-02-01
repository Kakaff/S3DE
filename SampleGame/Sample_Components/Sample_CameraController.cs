using S3DE.Engine;
using S3DE.Engine.Entities;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGame.Sample_Components
{
    public class Sample_CameraController : EntityComponent
    {
        Vector3 movDir;

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

        }
        protected override void OnCreation()
        {
            
        }
    }
}
