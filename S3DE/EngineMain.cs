using glfw3;
using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using S3DE.Engine;
using S3DE.Engine.Graphics;
using S3DE.Engine.Scenes;

namespace S3DE
{
    class EngineMain
    {
        static bool isRunning;
        static bool changeRes = false;
        static bool resChanged = false;

        static Game game;

        internal static bool ResolutionChanged
        {
            get => resChanged;
        }

        internal static bool ChangeResolution
        {
            get => changeRes;
            set => changeRes = value;
        }

        public static bool IsRunning => isRunning;

        public static void RunGame(Game game)
        {
            EngineMain.game = game;
            EngineMain.game.SetRenderer_Internal();
            EngineMain.game.SetClock_Internal();

            Time.Start();
            Renderer.Init();
            Glfw.Init();

            Engine.Graphics.Window.SetResolution(640, 480);
            Engine.Graphics.Window.CreateWindow(game.GameName());
            Engine.Graphics.Window.MakeCurrentContext();

            Renderer.SetCapabilities_Internal();
            EngineMain.game.SetFrameSync_Internal();
            EngineMain.game.InitGame();
            isRunning = true;
            Time.UpdateDeltaTime(Time.CurrentTick);

            MainLoop();

        }

        private static void MainLoop()
        {
            while (!Engine.Graphics.Window.IsCloseRequested && !changeRes)
            {
                Input.PollInput();
                Renderer.Clear();
                SceneHandler.RunScenes();
                Engine.Graphics.Window.SwapBuffer();
                FrameSync.WaitForTargetFPS();
                Time.UpdateDeltaTime(Time.CurrentTick);
                Engine.Graphics.Window.PollEvents();

                if (resChanged)
                    resChanged = false;
            }

            if (changeRes)
            {
                changeRes = false;
                resChanged = true;
                Engine.Graphics.Window.ResizeWindow();
                MainLoop();
            }
        }

        internal static void StopEngine(int exitCode)
        {
            isRunning = false;
            Environment.Exit(exitCode);
        }
    }
}
