using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE
{
    public enum ClockSetting
    {
        YieldTime,
        SleepTime,
        PowerSaveThreshold,
        FrequencyUpdateInterval,
    }

    public static partial class Time
    {

        static long prevFrame;
        static long currFrame;
        static float timescale = 1;
        static float deltaTime;


        public static float DeltaTime => deltaTime * timescale;
        public static float Timescale
        {
            get => timescale;
            set => timescale = value;
        }

        public static long Tick => GetTick();

        internal static long GetTick()
        {
            return Extern_Time_GetTick();
        }

        internal static void InitTime()
        {

            currFrame = GetTick();
        }

        public static void EnableOversleepAdjustment(bool val)
        {
            Extern_EnableOversleepAdjustment(val);
        }

        internal static void UpdateDeltaTime()
        {
            prevFrame = currFrame;
            currFrame = GetTick();
            deltaTime = (float)((currFrame - prevFrame) / (double)10000000) * Timescale;
        }

        public static void SetClock(ClockSetting setting, long value)
        {
            switch (setting)
            {
                case ClockSetting.YieldTime: { Extern_SetYieldTime(value); break; }
                case ClockSetting.SleepTime: { Extern_SetSleepTime(value); break; }
                case ClockSetting.PowerSaveThreshold: { Extern_SetPowerSaveTime(value); break; }
                case ClockSetting.FrequencyUpdateInterval: { Extern_SetFreqCheckInterval(value);break; }
            }
        }
    }
}
