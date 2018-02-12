using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine
{
    public sealed class AutoAdjustingStopWatchClock : EngineClock
    {
        Stopwatch watch;
        const long StartupPeriod = TimeSpan.TicksPerMillisecond * 50;
        double adjustment = 1d;
        long adjustmentPeriod = TimeSpan.TicksPerMillisecond * 1500;
        long lastAdjustment = 0;
        protected override long GetTick()
        {
            long eslaped = (long)(watch.ElapsedTicks * adjustment);

            if (eslaped - lastAdjustment >= adjustmentPeriod)
            {
                long acutalEslaped = DateTime.Now.Ticks - lastAdjustment;
                adjustment *= (double)acutalEslaped / (double)eslaped;
                lastAdjustment = (long)(watch.ElapsedTicks * adjustment);
            }

            return (long)(watch.ElapsedTicks * adjustment);
        }

        protected override void OnCreation()
        {
            watch = new Stopwatch();
        }

        protected override void StartClock()
        {
            if (Stopwatch.IsHighResolution)
                Console.WriteLine("Clock is high resolution!");
            
            long eslapedTicks = 0;
            long startTick = DateTime.Now.Ticks;

            watch.Start();
            while (true)
            {
                eslapedTicks = watch.ElapsedTicks;
                if (eslapedTicks >= StartupPeriod)
                    break;
            }

            long endTick = DateTime.Now.Ticks;
            long eslaped = endTick - startTick;
            adjustment = (double)eslaped / (double)StartupPeriod;
            lastAdjustment = DateTime.Now.Ticks;
            Console.WriteLine($"The clock needs to be changed by " + adjustment);
        }

        protected override void StopClock()
        {
            watch.Stop();
        }
    }
}
