using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace S3DE.Engine
{
    internal sealed class FrameSync
    {
        internal FrameSync()
        {
            lastUpdate = Time.CurrentTick;
            overSleep = 0;
            targetFrameRate = 60;
            targetFrameDuration = CalculateTargetFrameDuration();
        }

        int targetFrameRate;
        long targetFrameDuration;
        const long MAX_DIFF = 2;
        const long MaxYieldTimeAdjustment = 20;
        long lastUpdate;
        long overSleep;
        long dynamicYieldTime = (long)(0.75 * TimeSpan.TicksPerMillisecond);
        bool allowOverSleepAdjustment = true;

        int currentFPS;

        internal void SetTargetFPS(int fps)
        {
            targetFrameRate = fps;
            targetFrameDuration = CalculateTargetFrameDuration();
        }


        long CalculateTargetFrameDuration()
        {
            if (targetFrameRate == 0)
                return TimeSpan.TicksPerMillisecond / 2;
            else
                return TimeSpan.TicksPerSecond / targetFrameRate;
        }

        internal void WaitForTargetFPS()
        {
            long currTime;
            long sleepDuration;
            long diff;
            int fps;
            
            long modifiedTargetSleepDuration = Math.Max(targetFrameDuration - overSleep, 0);
            long targetSleepTime = lastUpdate + modifiedTargetSleepDuration;
            
            long yieldTime = Math.Min(modifiedTargetSleepDuration, dynamicYieldTime + (long)(TimeSpan.TicksPerMillisecond * 1.5));
            
            while (true)
            {
                currTime = Time.CurrentTick;
                
                if (currTime < targetSleepTime - yieldTime)
                    Thread.Sleep(1);
                else if (currTime < targetSleepTime)
                    Thread.Yield();
                else
                {
                    
                    sleepDuration = modifiedTargetSleepDuration + (currTime - targetSleepTime);
                    overSleep = (allowOverSleepAdjustment) ? currTime - targetSleepTime : 0;
                    overSleep = (overSleep < 0) ? 0 : overSleep;
                    diff = overSleep - dynamicYieldTime;

                    if (overSleep > dynamicYieldTime && diff > MAX_DIFF)
                        dynamicYieldTime = Math.Min((long)(1.5 * TimeSpan.TicksPerMillisecond),
                                    dynamicYieldTime + Math.Max(MaxYieldTimeAdjustment, diff / 4));
                    if (overSleep < dynamicYieldTime && diff < -MAX_DIFF)
                        dynamicYieldTime = Math.Max((long)(0.1 * TimeSpan.TicksPerMillisecond),
                                    dynamicYieldTime + Math.Min(-MaxYieldTimeAdjustment, diff / 4));
                    break;
                }
            }
            lastUpdate = Time.CurrentTick;
        }
    }
}
