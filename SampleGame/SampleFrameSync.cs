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
        
        const long MAX_DIFF_OVERSLEEP_BREAK = 350;
        const long MAX_DIFF_UNDERSLEEP_BREAK = -150;
        const long MAX_DIFF_OVERSLEEP_YIELD = 150;
        const long MAX_DIFF_UNDERSLEEP_YIELD = -150;
        const long MAX_OVERSLEEP_ERR = 250;
        const long OVERSLEEP_STEP = 150;
        const long MIN_BREAK_TIME = MAX_OVERSLEEP_ERR;
        const long MIN_YIELD_TIME = 2500;

        const long MAX_BREAK_TIME = 5500;
        const long MAX_YIELD_TIME = 5500;

        const long MAX_FRAME_SKIPS = 1;
        const long MAX_FRAME_LAG = 1;

        protected override void WaitForTargetFrameDuration(long durationToWait)
        {
            long modifiedDurToWait = durationToWait;
            long eslapedTicks = 0;
            modifiedDurToWait -= oversleep;
            modifiedDurToWait = (modifiedDurToWait > -TargetFrameDuration * MAX_FRAME_LAG) ? modifiedDurToWait : -TargetFrameDuration * MAX_FRAME_LAG;
            modifiedDurToWait = (modifiedDurToWait < durationToWait + (TargetFrameDuration * MAX_FRAME_SKIPS)) ? modifiedDurToWait : durationToWait + (TargetFrameDuration * MAX_FRAME_SKIPS);

            while (true)
            {
                eslapedTicks = Time.CurrentTick - WaitStartTick;

                if (eslapedTicks >= modifiedDurToWait - dynamicBreakTime)
                {
                    oversleep = eslapedTicks - modifiedDurToWait;

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
                        dynamicBreakTime = EngineMath.Normalize(MIN_BREAK_TIME, MAX_BREAK_TIME, dynamicBreakTime);

                    if (dynamicYieldTime < MIN_YIELD_TIME)
                        dynamicYieldTime = MIN_YIELD_TIME;
                    if (dynamicYieldTime > MAX_YIELD_TIME)
                        dynamicYieldTime = EngineMath.Normalize(MIN_YIELD_TIME, MAX_YIELD_TIME, dynamicYieldTime);

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
