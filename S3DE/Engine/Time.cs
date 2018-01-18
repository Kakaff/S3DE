using S3DE.Engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine
{
    public static class Time
    {
        static EngineClock clock;
        static long deltaTicks;
        static float deltaTime;
        static long startTick;

        public static long CurrentTick => clock.CurrentTick;

        public static long TimeSinceStart => clock.CurrentTick - startTick;

        internal static void SetEngineClock<T>() where T : EngineClock
        {
            clock = InstanceCreator.CreateInstance<T>();
            clock.OnCreation_Internal();

        }

        internal static void Start()
        {
            clock.StartClock_Internal();
            startTick = clock.CurrentTick;
        }
    }
}
