using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace S3DE.Engine
{
    public class StopwatchClock : EngineClock
    {
        Stopwatch watch;

        protected override long GetTick() => watch.ElapsedTicks;

        protected override void OnCreation() => watch = new Stopwatch();

        protected override void StartClock() => watch.Start();

        protected override void StopClock() => watch.Stop();
    }
}
