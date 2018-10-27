using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace S3DE
{
    static partial class Time
    {
        [DllImport("S3DECore.dll")]
        private static extern long Extern_Time_GetTick();
        [DllImport("S3DECore.dll")]
        private static extern void Extern_EnableOversleepAdjustment(bool flag);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetYieldTime(long value);
        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetSleepTime(long value);
        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetPowerSaveTime(long value);
        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetFreqCheckInterval(long value);
    }
}
