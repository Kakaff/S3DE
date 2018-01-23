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
        static Engine.Graphics.Window window;

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
            
            window = new Engine.Graphics.Window((int)Renderer.Resolution.x,
                                                (int)Renderer.Resolution.y,
                                                game.GameName());
            window.MakeCurrentContext();

            FrameSync fs = new FrameSync();
            game.InitGame();

            isRunning = true;

            while (!window.IsCloseRequested)
            {
                Renderer.Clear();
                SceneHandler.RunScenes();
                
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
