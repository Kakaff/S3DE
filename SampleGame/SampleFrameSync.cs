using S3DE.Engine;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleGame
{
    //This is probably way more complicated than it needs to be. 
    //But we keep running into issues where the wait/yield goes haywire. 
    //Resulting in frames taking 0.1secs for some odd reason.
    //Or maybe it's down to the GLFW.SwapBuffers/PollInputs acting up?

    public class SampleFrameSync : FrameSync
    {
        public static long DynamicBreakTime => dynamicBreakTime;
        public static long DynamicYieldTime => dynamicYieldTime;
        public static long OverSleep => oversleep;

        static long oversleep = 0;
        static long oversleep_Remainder = 0;

        static long dynamicYieldTime = MIN_YIELD_TIME;
        static long dynamicBreakTime = MIN_BREAK_TIME;

        const long MAX_BREAK_ADJUSTMENT = 50;
        const long MIN_BREAK_ADJUSTMENT = 5;

        const long MAX_YIELD_ADJUSTMENT = 500;
        const long MIN_YIELD_ADJUSTMENT = 5;

        const long MAX_DIFF_OVERSLEEP_BREAK = 350;
        const long MAX_DIFF_UNDERSLEEP_BREAK = -150;
        const long MAX_DIFF_OVERSLEEP_YIELD = 150;
        const long MAX_DIFF_UNDERSLEEP_YIELD = -150;
        const long MAX_OVERSLEEP_ERR = 250;
        const long OVERSLEEP_STEP = 50;
        const long MIN_BREAK_TIME = 0;
        const long MIN_YIELD_TIME = 2500;

        const long MAX_BREAK_TIME = 5500;
        const long MAX_YIELD_TIME = 5500;

        const long MAX_FRAME_SKIPS = 1;
        const double MAX_FRAME_LAG = 1;

        
        protected override void WaitForTargetFrameDuration(long durationToWait)
        {
            long modifiedDurToWait = durationToWait;
            long eslapedTicks = 0;
            modifiedDurToWait -= oversleep;
            
            if (modifiedDurToWait <= 0)
            {
                oversleep_Remainder += modifiedDurToWait;
                modifiedDurToWait = 0;
                oversleep = 0;
            }

            while (true)
            {
                eslapedTicks = Time.CurrentTick - WaitStartTick;

                if (eslapedTicks >= modifiedDurToWait - dynamicBreakTime)
                {
                    oversleep = eslapedTicks - modifiedDurToWait;
                    
                    if (oversleep > MAX_DIFF_OVERSLEEP_YIELD)
                        dynamicYieldTime += EngineMath.Clamp(MIN_YIELD_ADJUSTMENT, MAX_YIELD_ADJUSTMENT, (long)(EngineMath.MultipleOf(oversleep * 0.9, 5)));

                    if (oversleep > MAX_DIFF_OVERSLEEP_BREAK)
                        dynamicBreakTime += EngineMath.Clamp(MIN_BREAK_ADJUSTMENT,MAX_BREAK_ADJUSTMENT,(long)(EngineMath.MultipleOf(oversleep * 0.25,5)));

                    if ((oversleep > 0 && oversleep <= MAX_DIFF_OVERSLEEP_YIELD) || oversleep < MAX_DIFF_UNDERSLEEP_YIELD)
                        dynamicYieldTime -= EngineMath.Clamp(MIN_YIELD_ADJUSTMENT, MAX_YIELD_ADJUSTMENT, (long)(EngineMath.MultipleOf(oversleep * 0.9,5)));

                    if (oversleep < MAX_DIFF_UNDERSLEEP_BREAK)
                        dynamicBreakTime -= EngineMath.Clamp(MIN_BREAK_ADJUSTMENT, MAX_BREAK_ADJUSTMENT, (long)(EngineMath.MultipleOf(oversleep * 0.25, 5)));

                    dynamicBreakTime = (dynamicBreakTime < MIN_BREAK_TIME) ? MIN_BREAK_TIME 
                        : (dynamicBreakTime > MAX_BREAK_TIME) ? MAX_BREAK_TIME : dynamicBreakTime;

                    dynamicYieldTime = (dynamicYieldTime < MIN_YIELD_TIME) ? MIN_YIELD_TIME
                        : (dynamicYieldTime > MAX_YIELD_TIME) ? MAX_YIELD_TIME : dynamicYieldTime;
                    
                    oversleep += oversleep_Remainder;
                    oversleep_Remainder = (long)(((oversleep / (double)OVERSLEEP_STEP) % 1d) * OVERSLEEP_STEP);
                    oversleep = (Math.Abs(oversleep) >= MAX_OVERSLEEP_ERR) ? (oversleep / OVERSLEEP_STEP) * OVERSLEEP_STEP : 0;
                    
                    break;
                }
                else
                {
                    long remainingTicks = modifiedDurToWait - eslapedTicks;

                    if (remainingTicks >  dynamicYieldTime + dynamicBreakTime)
                    {

                        long ticksToWait = (long)(remainingTicks * 0.8) - (dynamicYieldTime + dynamicBreakTime);

                        if (ticksToWait > 0)
                            Thread.Sleep(TimeSpan.FromTicks(ticksToWait));
                        else
                            Thread.Yield();
                    }
                    else
                        Thread.Yield();
                }
            }


        }
    }
}
