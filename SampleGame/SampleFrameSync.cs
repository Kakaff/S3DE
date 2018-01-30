using S3DE.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleGame
{
    class SampleFrameSync : FrameSync
    {
        long yieldTime = (long)(TimeSpan.TicksPerMillisecond * 1.15);

        long dynamicYieldTime = 0;
        long dynamicBreakTime = 0;

        const long MAX_DIFF_OVERSLEEP = 75;
        const long MAX_DIFF_UNDERSLEEP = -75;

        const long MIN_BREAK_TIME = 0;
        const long MIN_YIELD_TIME = 2500;

        const long MAX_BREAK_TIME = 5000;
        const long MAX_YIELD_TIME = 8500;

        protected override void WaitForTargetFrameDuration(long durationToWait)
        {
            
            long eslapedTicks = 0;

            while (true)
            {
                eslapedTicks = Time.CurrentTick - WaitStartTick;
                
                if (eslapedTicks >= durationToWait - dynamicBreakTime)
                {
                    long oversleep = eslapedTicks - durationToWait;

                    if (oversleep > MAX_DIFF_OVERSLEEP)
                    {
                        dynamicBreakTime += 25;
                        dynamicYieldTime += 50;
                    }
                    else if (oversleep > 0)
                        dynamicYieldTime -= 50;

                    if (oversleep < MAX_DIFF_UNDERSLEEP)
                        dynamicBreakTime -= 25;

                    if (dynamicBreakTime < MIN_BREAK_TIME)
                        dynamicBreakTime = MIN_BREAK_TIME;
                    if (dynamicBreakTime > MAX_BREAK_TIME)
                        dynamicBreakTime = MAX_BREAK_TIME;

                    if (dynamicYieldTime < MIN_YIELD_TIME)
                        dynamicYieldTime = MIN_YIELD_TIME;
                    if (dynamicYieldTime > MAX_YIELD_TIME)
                        dynamicYieldTime = MAX_YIELD_TIME;
                    
                    break;
                } else
                    if (durationToWait - eslapedTicks > yieldTime + dynamicYieldTime)
                        Thread.Sleep(1);
                    else
                        Thread.Yield();

            }


        }
    }
}
