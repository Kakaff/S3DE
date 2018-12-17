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

        internal static void UpdateDeltaTime()
        {
            deltaTime = (float)((double)(Extern_GetDeltaTime()) / (double)1000000) * Timescale;
        }

        public static void SetClock(ClockSetting setting, long value)
        {
            switch (setting)
            {
                case ClockSetting.YieldTime: {Extern_SetYieldTime(value); break; }
                case ClockSetting.FrequencyUpdateInterval: { Extern_SetFreqCheckInterval(value);break;}
            }
        }
    }
}
