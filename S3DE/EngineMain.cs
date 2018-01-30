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

        public static bool IsRunning => isRunning;

        public static void RunGame(Game game)
        {
            game.SetRenderer_Internal();
            game.SetClock_Internal();

            Time.Start();
            while (Time.TimeSinceStart < TimeSpan.TicksPerSecond * 0.25)
                Thread.Yield();

            Renderer.Init();
            Glfw.Init();

            Engine.Graphics.Window.CreateWindow((int)Renderer.Resolution.x, (int)Renderer.Resolution.y, game.GameName());
            Engine.Graphics.Window.MakeCurrentContext();
            Renderer.SetCapabilities_Internal();
            game.SetFrameSync_Internal();
            game.InitGame();
            isRunning = true;
            Time.UpdateDeltaTime(Time.CurrentTick);

            while (!Engine.Graphics.Window.IsCloseRequested)
            {
                Renderer.Clear();
                SceneHandler.RunScenes();
                Engine.Graphics.Window.SwapBuffer();
                Engine.Graphics.Window.PollEvents();
                FrameSync.WaitForTargetFPS();
                Time.UpdateDeltaTime(Time.CurrentTick);
            }

        }
        internal static void StopEngine(int exitCode)
        {
            isRunning = false;
            Environment.Exit(exitCode);
        }
    }
}
