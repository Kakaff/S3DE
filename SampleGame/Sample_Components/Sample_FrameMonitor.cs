using S3DE.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGame.Sample_Components
{
    public class Sample_FrameMonitor : EntityComponent
    {

        float t;
        int count;
        float low, high;

        protected override void Update()
        {
            count++;
            t += DeltaTime;

            if (low > DeltaTime)
                low = DeltaTime;

            if (high < DeltaTime)
                high = DeltaTime;

            if (t >= 1)
            {
                Console.WriteLine($"FPS: {count} | FrameTimeLow: {low} | FrameTimeHigh: {high}");
                t -= 1;
                high = 0;
                low = 1;
                count = 0;
            }
        }
        protected override void OnCreation()
        {
        }
    }
}
