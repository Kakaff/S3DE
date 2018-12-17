using System.Runtime.InteropServices;

namespace S3DE
{
    partial class EngineMain
    {
        [DllImport("S3DECore.dll")]
        private static extern void RunEngine();

        [DllImport("S3DECore.dll")]
        private static extern void RegisterUpdateFunc(OnFrameUpdate func);

        [DllImport("S3DECore.dll")]
        internal static extern void Extern_EnablePowerSaving(bool val);

        [DllImport("S3DECore.dll")]
        internal static extern void Extern_SetTargetFramerate(int value);
    }
}
