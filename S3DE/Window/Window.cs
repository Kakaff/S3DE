using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        static S3DE_Vector2 currRes,newRes;
        static float aspect = 1;
        static bool resChanged = true,chApplied = true;
        static bool isFocused,lostFocus,gainedFocus;

        internal static bool RegainedFocus => gainedFocus;
        internal static bool LostFocus => lostFocus;
        internal static bool IsFocused => isFocused;

        public static float AspectRatio => aspect;

        public static bool PendingChanges => !chApplied;

        public static bool ChangesApplied
        {
            get => chApplied;
            internal set => chApplied = value;
        }

        public static bool ResolutionChanged
        {
            get => resChanged;
            internal set => resChanged = value;
        }

        public static S3DE_Vector2 Resolution => currRes;

        internal static void _CreateWindow(S3DE_Vector2 displayRes,string title)
        {
            Window.currRes = displayRes;
            aspect = displayRes.X / displayRes.Y;
            InitGLFW();
            Extern_SetWindowHint((int)WindowHint.SAMPLES, 0);
            Extern_SetWindowHint((int)WindowHint.CONTEXT_VERSION_MAJOR, 3);
            Extern_SetWindowHint((int)WindowHint.CONTEXT_VERSION_MINOR, 3);
            Extern_SetWindowHint((int)WindowHint.OPENGL_PROFILE, (int)GLFW.OPENGL_CORE_PROFILE);
            Extern_SetWindowHint((int)WindowAttribute.RESIZABLE, 0);
            CreateWindow();
        }

        internal static void SetResolution(int width, int height)
        {
            newRes = new S3DE_Vector2(width, height);
            chApplied = false;
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

        internal static void ApplyChanges()
        {
            if (!chApplied)
            {
                chApplied = true;
                resChanged = true;
                Extern_SetWindowSize((int)newRes.X, (int)newRes.Y);
                currRes = newRes;
            }
        }

    }
}
