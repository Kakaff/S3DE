using glfw3;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine
{
    /// <summary>
    /// Scancodes representing individual keys on a US layout keyboard.
    /// </summary>
    public enum KeyCode
    {
        Space = glfw3.Key.Space,
        Apostrophe = glfw3.Key.Apostrophe,
        Comma = glfw3.Key.Comma,
        Minus = glfw3.Key.Minus,
        Period = glfw3.Key.Period,
        Slash = glfw3.Key.Slash,
        _0 = glfw3.Key._0,
        _1 = glfw3.Key._1,
        _2 = glfw3.Key._2,
        _3 = glfw3.Key._3,
        _4 = glfw3.Key._4,
        _5 = glfw3.Key._5,
        _6 = glfw3.Key._6,
        _7 = glfw3.Key._7,
        _8 = glfw3.Key._8,
        _9 = glfw3.Key._9,
        Semicolon = glfw3.Key.Semicolon,
        Equal = glfw3.Key.Equal,
        A = glfw3.Key.A,
        B = glfw3.Key.B,
        C = glfw3.Key.C,
        D = glfw3.Key.D,
        E = glfw3.Key.E,
        F = glfw3.Key.F,
        G = glfw3.Key.G,
        H = glfw3.Key.H,
        I = glfw3.Key.I,
        J = glfw3.Key.J,
        K = glfw3.Key.K,
        L = glfw3.Key.L,
        M = glfw3.Key.M,
        N = glfw3.Key.N,
        O = glfw3.Key.O,
        P = glfw3.Key.P,
        Q = glfw3.Key.Q,
        R = glfw3.Key.R,
        S = glfw3.Key.S,
        T = glfw3.Key.T,
        U = glfw3.Key.U,
        V = glfw3.Key.V,
        W = glfw3.Key.W,
        X = glfw3.Key.X,
        Y = glfw3.Key.Y,
        Z = glfw3.Key.Z,
        LeftBracket = glfw3.Key.LeftBracket,
        Backslash = glfw3.Key.Backslash,
        RightBracket = glfw3.Key.RightBracket,
        GraveAccent = glfw3.Key.GraveAccent,
        World1 = glfw3.Key.World1,
        World2 = glfw3.Key.World2,
        Escape = glfw3.Key.Escape,
        Enter = glfw3.Key.Enter,
        Tab = glfw3.Key.Tab,
        Backspace = glfw3.Key.Backspace,
        Insert = glfw3.Key.Insert,
        Delete = glfw3.Key.Delete,
        Right = glfw3.Key.Right,
        Left = glfw3.Key.Left,
        Down = glfw3.Key.Down,
        Up = glfw3.Key.Up,
        PageUp = glfw3.Key.PageUp,
        PageDown = glfw3.Key.PageDown,
        Home = glfw3.Key.Home,
        End = glfw3.Key.End,
        CapsLock = glfw3.Key.CapsLock,
        ScrollLock = glfw3.Key.ScrollLock,
        NumLock = glfw3.Key.NumLock,
        PrintScreen = glfw3.Key.PrintScreen,
        Pause = glfw3.Key.Pause,
        F1 = glfw3.Key.F1,
        F2 = glfw3.Key.F2,
        F3 = glfw3.Key.F3,
        F4 = glfw3.Key.F4,
        F5 = glfw3.Key.F5,
        F6 = glfw3.Key.F6,
        F7 = glfw3.Key.F7,
        F8 = glfw3.Key.F8,
        F9 = glfw3.Key.F9,
        F10 = glfw3.Key.F10,
        F11 = glfw3.Key.F11,
        F12 = glfw3.Key.F12,
        F13 = glfw3.Key.F13,
        F14 = glfw3.Key.F14,
        F15 = glfw3.Key.F15,
        F16 = glfw3.Key.F16,
        F17 = glfw3.Key.F17,
        F18 = glfw3.Key.F18,
        F19 = glfw3.Key.F19,
        F20 = glfw3.Key.F20,
        F21 = glfw3.Key.F21,
        F22 = glfw3.Key.F22,
        F23 = glfw3.Key.F23,
        F24 = glfw3.Key.F24,
        F25 = glfw3.Key.F25,
        Kp0 = glfw3.Key.Kp0,
        Kp1 = glfw3.Key.Kp1,
        Kp2 = glfw3.Key.Kp2,
        Kp3 = glfw3.Key.Kp3,
        Kp4 = glfw3.Key.Kp4,
        Kp5 = glfw3.Key.Kp5,
        Kp6 = glfw3.Key.Kp6,
        Kp7 = glfw3.Key.Kp7,
        Kp8 = glfw3.Key.Kp8,
        Kp9 = glfw3.Key.Kp9,
        KpDecimal = glfw3.Key.KpDecimal,
        KpDivide = glfw3.Key.KpDivide,
        KpMultiply = glfw3.Key.KpMultiply,
        KpSubtract = glfw3.Key.KpSubtract,
        KpAdd = glfw3.Key.KpAdd,
        KpEnter = glfw3.Key.KpEnter,
        KpEqual = glfw3.Key.KpEqual,
        LeftShift = glfw3.Key.LeftShift,
        LeftControl = glfw3.Key.LeftControl,
        LeftAlt = glfw3.Key.LeftAlt,
        LeftSuper = glfw3.Key.LeftSuper,
        RightShift = glfw3.Key.RightShift,
        RightControl = glfw3.Key.RightControl,
        RightAlt = glfw3.Key.RightAlt,
        RightSuper = glfw3.Key.RightSuper,
        Menu = glfw3.Key.Menu,
        Last = glfw3.Key.Last,
        Unknown = glfw3.Key.Unknown,
    }

    public static class Input
    {
        static Dictionary<KeyCode, bool> keyUp = new Dictionary<KeyCode, bool>(); //The key was released the prev frame
        static Dictionary<KeyCode, bool> keyDown = new Dictionary<KeyCode, bool>(); //The key was pressed the prev frame.
        static Dictionary<KeyCode, bool> Key = new Dictionary<KeyCode, bool>(); //The key is pressed.

        static bool cursorHidden;
        static bool cursorLocked;
        static bool cursorHasMoved;
        static S3DE_Vector2 mouseDeltaRaw = S3DE_Vector2.Zero;
        static S3DE_Vector2 mouseDelta = S3DE_Vector2.Zero;
        static S3DE_Vector2 mousePos = S3DE_Vector2.Zero;
        static S3DE_Vector2 prevMousePos = S3DE_Vector2.Zero;

        public static S3DE_Vector2 MouseDeltaRaw => mouseDeltaRaw;
        public static S3DE_Vector2 MouseDelta => mouseDelta;

        public static bool CursorMoved => cursorHasMoved;

        public static bool HiddenCursor
        {
            get => cursorHidden;
            set
            {
                if (value)
                    HideCursor();
                else
                    ShowCursor();
                cursorHidden = value;
            }
        }

        public static bool LockedCursor
        {
            get => cursorLocked;
            set
            {
                cursorLocked = value;
                if (value)
                    LockCursor();
                else
                    ShowCursor();
            }
        }

        internal static void PollInput()
        {
            //Mouse stuff
            if (Game.IsFocused)
            {
                double x = 0, y = 0;
                Glfw.GetCursorPos(S3DE.Engine.Graphics.Window.window, ref x, ref y);

                mousePos = new S3DE_Vector2((float)(x > Game.DisplayResolution.X ? Game.DisplayResolution.X : x < 0 ? 0 : x),
                                       (float)(y > Game.DisplayResolution.Y ? Game.DisplayResolution.Y : y < 0 ? 0 : y));

                mouseDeltaRaw = new S3DE_Vector2((float)x - prevMousePos.X, (float)y - prevMousePos.Y);
                prevMousePos = new S3DE_Vector2((float)x, (float)y);

                if (!LockedCursor && !CursorIsInsideWindow((int)x, (int)y))
                {
                    mouseDeltaRaw = S3DE_Vector2.Zero;
                    prevMousePos = mousePos;
                }
                
                //Prevents the game from spazzing out if the user clicks somewhere on screen to get focus on the game again.
                if (Game.RegainedFocus)
                    mouseDeltaRaw = S3DE_Vector2.Zero;

                if (mouseDeltaRaw != S3DE_Vector2.Zero)
                    cursorHasMoved = true;
                else
                    cursorHasMoved = false;

                mouseDelta = new S3DE_Vector2(mouseDeltaRaw.X / Game.DisplayResolution.X, mouseDeltaRaw.Y / Game.DisplayResolution.X);
            } else
            {
                mouseDelta = S3DE_Vector2.Zero;
                mouseDeltaRaw = S3DE_Vector2.Zero;
            }

            int[] values = (int[])Enum.GetValues(typeof(KeyCode));
            foreach (int i in values)
            {
                int v = Glfw.GetKey(S3DE.Engine.Graphics.Window.window, i);
                bool isDown = GetKey((KeyCode)i);
                KeyCode key = (KeyCode)i;

                if (GetKeyReleased(key))
                    keyUp.Remove(key);

                if (v == 1)
                {
                    if (isDown)
                    {
                        if (GetKeyDown(key))
                            keyDown.Remove(key);
                    }
                    else
                    {
                        keyDown.Add(key, true);
                        Key.Add(key, true);
                    }
                }
                else if (isDown)
                {
                    //Dirty fix for keys not being removed.
                    if (GetKeyReleased(key))
                        keyUp.Remove(key);

                    keyUp.Add(key, true);
                    Key.Remove(key);
                    if (GetKeyDown(key))
                        keyDown.Remove(key);
                }
            }
        }

        /// <summary>
        /// Locks and Hides the cursor.
        /// </summary>
        public static void LockCursor()
        {
            cursorLocked = true;
            Glfw.SetInputMode(S3DE.Engine.Graphics.Window.window, (int)glfw3.State.Cursor, (int)glfw3.State.CursorDisabled);
        }

        public static void ReleaseCursor()
        {
            cursorLocked = false;
            ShowCursor();
        }

        public static void HideCursor()
        {
            Glfw.SetInputMode(S3DE.Engine.Graphics.Window.window, (int)glfw3.State.Cursor, (int)glfw3.State.CursorHidden);
            
        }

        public static void ShowCursor()
        {
            Glfw.SetInputMode(S3DE.Engine.Graphics.Window.window, (int)glfw3.State.Cursor, (int)glfw3.State.CursorNormal);
        }

        public static string GetLocalizedKeyName(KeyCode key) => Glfw.GetKeyName((int)key, (int)key);

        /// <summary>
        /// Returns a bool indicating wether or not the key was pressed this frame.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetKeyDown(KeyCode key) => keyDown.TryGetValue(key, out bool tmp);

        /// <summary>
        /// Returns a bool indicating wether or not the key was released this frame.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetKeyReleased(KeyCode key) => keyUp.TryGetValue(key, out bool tmp);

        /// <summary>
        /// Returns a bool indicating wether or not the key is currently being pressed.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetKey(KeyCode key) => Key.TryGetValue(key, out bool tmp);

        private static bool CursorIsInsideWindow(int x, int y)
        {
            if (x > 0 && x < Game.DisplayResolution.X && y > 0 && y < Game.DisplayResolution.Y)
                return true;
            return false;
        }
    }
}
