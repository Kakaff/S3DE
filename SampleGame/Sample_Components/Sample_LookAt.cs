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
    public class Sample_LookAt : EntityComponent, IUpdateLogic
    {
        Vector3 target;

        public Vector3 Target
        {
            get => target;
            set => target = value;
        }

        protected override void OnCreation() { target = Vector3.Zero; }

        public void EarlyUpdate() { }
        public void LateUpdate() { }
        public void Update()
        {
            transform.LookAt(target);
        }
    }
}
