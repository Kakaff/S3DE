using S3DE.Maths;
using System;

namespace S3DE
{
    public static partial class Window
    {
        public enum WindowAttribute
        {
            FOCUSED = 0x00020001,
            ICONIFIED = 0x00020002,
            RESIZABLE = 0x00020003,
            VISIBLE = 0x00020004,
            DECORATED = 0x00020005,
            AUTO_ICONIFY = 0x00020006,
            FLOATING = 0x00020007,
            MAXIMIZED = 0x00020008,
        }
        static float aspect = 1;
        static bool isFocused,lostFocus,gainedFocus;

        internal static bool RegainedFocus => gainedFocus;
        internal static bool LostFocus => lostFocus;
        internal static bool IsFocused => isFocused;

        public static float AspectRatio => aspect;
        
        internal static void _CreateWindow(Vector2 displayRes,string title)
        {
            aspect = displayRes.x / displayRes.y;
            InitGLFW();
            Extern_SetWindowHint((int)WindowHint.SAMPLES, 0);
            Extern_SetWindowHint((int)WindowHint.CONTEXT_VERSION_MAJOR, 3);
            Extern_SetWindowHint((int)WindowHint.CONTEXT_VERSION_MINOR, 3);
            Extern_SetWindowHint((int)WindowHint.OPENGL_PROFILE, (int)GLFW.OPENGL_CORE_PROFILE);
            Extern_SetWindowHint((int)WindowAttribute.RESIZABLE, 0);
            CreateWindow();
        }
        

        public static void OnDisplayResChanged(Vector2 oldRes, Vector2 newRes)
        {
            Extern_SetWindowSize((int)newRes.x, (int)newRes.y);
        }

        internal static void UpdateFocus()
        {
            lostFocus = false;
            gainedFocus = false;

            bool foc = Extern_GetAttribute((int)WindowAttribute.FOCUSED) == 1;
            if (foc != isFocused)
            {
                isFocused = foc;
                if (foc)
                {
                    gainedFocus = true;
                    Console.WriteLine("Window regained focus!");
                }
                else
                {
                    lostFocus = true;
                    Console.WriteLine("Window lost focus!");
                }
            }
        }
    }
}
