using S3DE.Engine.Entities;
using S3DE.Engine.Entities.Components;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGame.Sample_Components
{
    public class Sample_ObjectRotator : EntityComponent, IUpdateLogic
    {
        protected override void OnCreation() { }

        public void EarlyUpdate() { }
        public void Update()
        {
            transform.Rotate(VectorExtensions.Vec3_Up, 45f * DeltaTime);
        }

        public void LateUpdate() { }
    }
}
