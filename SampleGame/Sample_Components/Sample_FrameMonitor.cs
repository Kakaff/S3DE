using S3DE;
using S3DE.Engine;
using S3DE.Engine.Entities;
using S3DE.Engine.Graphics.OpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGame.Sample_Components
{
    public class Sample_FrameMonitor : EntityComponent
    {
        List<long> frames = new List<long>();
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

                frames.Add(Time.DeltaTicks);

                if (t >= 1)
                {
                    frames.Sort();
                    long min10 = 0;
                    long max10 = 0;

                    int c = frames.Count / 10;

                    c = c == 0 ? 1 : c;

                    for (int i = 0; i < c; i++)
                    {
                        min10 += frames[i];
                        max10 += frames[frames.Count - (i + 1)];
                    }

                    min10 /= c;
                    max10 /= c;


                    Console.WriteLine($"FPS: {count} " +
                        $"| AvgFrameTime: {(float)(((double)t / (double)count)) * 1000}ms " +
                        $"| FrameTimeLow10%: {(float)((double)min10 / (double)TimeSpan.TicksPerMillisecond)}ms " +
                        $"| FrameTimeHigh10%: {(float)((double)max10 / (double)TimeSpan.TicksPerMillisecond)}ms");
                    Console.WriteLine($"DynamicYieldTime: {SampleFrameSync.DynamicYieldTime} | DynamicBreakTime: {SampleFrameSync.DynamicBreakTime}");
                    Console.WriteLine($"Oversleep: {SampleFrameSync.OverSleep}");
                
                    t -= 1;
                    high = 0;
                    low = 1;
                    count = 0;
                frames.Clear();
                }
            
                
        }
        protected override void OnCreation()
        {
        }
    }
}
