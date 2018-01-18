using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine
{
    public abstract class EngineClock
    {
        protected abstract long GetTick();

        protected abstract void StartClock();
        protected abstract void StopClock();

        protected abstract void OnCreation();

        internal void OnCreation_Internal()
        {
            OnCreation();
        }

        internal void StartClock_Internal()
        {
            StartClock();
        }


        internal long CurrentTick => GetTick();
    }
}
