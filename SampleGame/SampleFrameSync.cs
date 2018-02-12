using S3DE.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleGame
{
    public class SampleFrameSync : FrameSync
    {
        public static long DynamicBreakTime = 0;
        public static long DynamicYieldTime = 0;

        long yieldTime = (long)(TimeSpan.TicksPerMillisecond * 1.15);

        long dynamicYieldTime = 0;
        long dynamicBreakTime = 0;

        const long MAX_DIFF_OVERSLEEP_BREAK = 75;
        const long MAX_DIFF_UNDERSLEEP_BREAK = -75;
        const long MAX_DIFF_OVERSLEEP_YIELD = 150;
        const long MAX_DIFF_UNDERSLEEP_YIELD = -150;
        const long MIN_BREAK_TIME = 0;
        const long MIN_YIELD_TIME = 0;

        const long MAX_BREAK_TIME = 11500;
        const long MAX_YIELD_TIME = 3500;

        protected override void WaitForTargetFrameDuration(long durationToWait)
        {
            
            long eslapedTicks = 0;

            while (true)
            {
                eslapedTicks = Time.CurrentTick - WaitStartTick;
                
                if (eslapedTicks >= durationToWait - dynamicBreakTime)
                {
                    long oversleep = eslapedTicks - durationToWait;

                    if (oversleep > MAX_DIFF_OVERSLEEP_YIELD)
                        dynamicYieldTime += 100;
                    
                    if (oversleep > MAX_DIFF_OVERSLEEP_BREAK)
                        dynamicBreakTime += 100;

                    if ((oversleep > 0 && oversleep <= MAX_DIFF_OVERSLEEP_YIELD) || oversleep < MAX_DIFF_UNDERSLEEP_YIELD)
                        dynamicYieldTime -= 50;

                    if (oversleep < MAX_DIFF_UNDERSLEEP_BREAK)
                        dynamicBreakTime -= 50;

                    
                    if (dynamicBreakTime < MIN_BREAK_TIME)
                        dynamicBreakTime = MIN_BREAK_TIME;
                    if (dynamicBreakTime > MAX_BREAK_TIME)
                        dynamicBreakTime = MAX_BREAK_TIME;

                    if (dynamicYieldTime < MIN_YIELD_TIME)
                        dynamicYieldTime = MIN_YIELD_TIME;
                    if (dynamicYieldTime > MAX_YIELD_TIME)
                        dynamicYieldTime = MAX_YIELD_TIME;

                    DynamicBreakTime = dynamicBreakTime;
                    DynamicYieldTime = dynamicYieldTime;
                    break;
                } else
                    if (durationToWait - eslapedTicks > yieldTime + dynamicYieldTime + dynamicBreakTime)
                        Thread.Sleep(1);
                    else
                        Thread.Yield();

            }


        }
    }
}
