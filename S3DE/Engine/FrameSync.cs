using S3DE.Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace S3DE.Engine
{
    public abstract class FrameSync
    {
        long prevUpdate;
        long targetDuration;
        long waitStartTick;

        static FrameSync frameSync;

        internal static FrameSync SetFrameSync
        {
            set => frameSync = value;
        }

        protected long WaitStartTick => waitStartTick;
        protected long TargetFrameDuration => targetDuration;
        
        internal static void SetTargetFPS(uint fps)
        {
            if (fps != 0)
                frameSync.targetDuration = TimeSpan.TicksPerSecond / (long)fps;
            else
                frameSync.targetDuration = 0;
        }

        protected abstract void WaitForTargetFrameDuration(long durationToWait);
        protected virtual void OnSkip() { }
        static internal void WaitForTargetFPS() => frameSync._waitForTargetFPS();

        internal void _waitForTargetFPS()
        {
            

            long target = prevUpdate + targetDuration; //On what tick we should start the next frame.
            
            //Adjust for what the time is as of right now.
            long duration = target - Time.CurrentTick;
            
            waitStartTick = Time.CurrentTick;

            if (prevUpdate == 0)
                duration = 0;

            WaitForTargetFrameDuration(duration);
            prevUpdate = Time.CurrentTick;
        }

        
    }
}
