using System.Runtime.InteropServices;

namespace S3DE
{
    static partial class Time
    {
        [DllImport("S3DECore.dll")]
        private static extern long Extern_Time_GetTick();
        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetYieldTime(long value);
        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetFreqCheckInterval(long value);
        [DllImport("S3DECore.dll")]
        private static extern long Extern_GetDeltaTime();
    }
}
