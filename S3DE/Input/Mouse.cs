using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Input
{
    public enum CursorMode
    {
        Normal = 0,
        Hidden = 1,
        LockedAndHidden = 2
    }

    public static partial class Mouse
    {
        static double currX, currY;
        static double prevX, prevY;
        static double deltaX, deltaY;

        static double virtualX, virtualY;

        static bool hasMoved,isInsideWindow,isLocked;

        public static double RawX => virtualX;
        public static double RawY => virtualY;

        public static double RawDeltaX => deltaX;
        public static double RawDeltaY => deltaY;
        public static bool HasMoved => hasMoved;

        public static bool IsInsideWIndow => isInsideWindow;
        public static bool IsLocked => isLocked;

        public static void SetCursor(CursorMode cm)
        {
            if (cm == CursorMode.LockedAndHidden)
                isLocked = true;
            else
                isLocked = false;

            Extern_SetCursor(cm);
        }

        internal static void Update()
        {
            Extern_GetCursorPos(out double x, out double y);
            x -= Window.Resolution.X * 0.5f;
            y -= Window.Resolution.Y * 0.5f;

            x /= Window.Resolution.X * 0.5f;
            y /= Window.Resolution.Y * 0.5f;
            
            prevX = currX;
            prevY = currY;

            currX = x;
            currY = y;

            if (!isLocked && currX >= -1 && currX <= 1 && currY >= -1 && currY <= 1)
                isInsideWindow = true;
            else if (isLocked)
                isInsideWindow = true;
            else
                isInsideWindow = false;

            if (!Game.RegainedFocus && IsInsideWIndow)
            {
                deltaX = currX - prevX;
                deltaY = currY - prevY;
            }
            else
            {
                deltaX = 0;
                deltaY = 0;
            }

            hasMoved = deltaX != 0 || deltaY != 0;

            virtualX = EngineMath.Clamp(-1, 1, virtualX + deltaX);
            virtualY = EngineMath.Clamp(-1, 1, virtualY + deltaY);
        }
    }
}
