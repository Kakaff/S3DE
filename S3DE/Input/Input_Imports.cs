using System.Runtime.InteropServices;

namespace S3DE.Input
{
    static partial class Keyboard
    {
        [DllImport("S3DECore.dll")]
        private static extern KeyState Extern_GetKey(KeyCode key);
    }

    static partial class Mouse
    {
        [DllImport("S3DECore.dll")]
        private static extern void Extern_GetCursorPos(out double x, out double y);
        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetCursorPos(double x, double y);
        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetCursor(CursorMode mode);
    }

    static partial class Input_Handler
    {
        
    }
}
