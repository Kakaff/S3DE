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

namespace S3DE
{
    class EngineMain
    {
        static bool isRunning;
        static Engine.Graphics.Window window;

        public static bool IsRunning => isRunning;

        public static void RunGame(Game game)
        {
            game.InitGame();
            Time.Start();
            while (Time.TimeSinceStart < TimeSpan.TicksPerSecond * 0.25)
                Thread.Yield();

            Renderer.Init();
            Glfw.Init();
            window = new Engine.Graphics.Window((int)Renderer.Resolution.x,
                                                (int)Renderer.Resolution.y,
                                                game.GameName());

            FrameSync fs = new FrameSync();

            isRunning = true;

            game.StartScene.Load_Internal();

            while (!window.IsCloseRequested)
            {
                Renderer.Clear();
                fs.WaitForTargetFPS();
                window.SwapBuffer();
                window.PollEvents();
            }

        }
        internal static void StopEngine(int exitCode)
        {
            isRunning = false;
            Environment.Exit(exitCode);
        }
    }
}
