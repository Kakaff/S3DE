using S3DE.Engine.Entities;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGame.Sample_Components
{
    public class Sample_ObjectRotator : EntityComponent
    {
        protected override void OnCreation() { }

        int count = 0;
        float t;

        protected override void Update()
        {
            t += DeltaTime;

            if (t >= 1f)
            {
                t -= 1f;
                Console.WriteLine($"[{System.DateTime.Now}]FPS: {count}");
                count = 0;
            }

            count++;

            transform.Rotate(Vector3.Up, 45f * DeltaTime);
        }
    }
}
