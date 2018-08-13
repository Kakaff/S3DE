using glfw3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using S3DE.Engine;
using S3DE.Engine.Graphics;
using S3DE.Engine.Scenes;
using S3DE.Engine.Debug.RenderingDebugging;
using S3DE.Engine.IO;
using S3DE.Engine.Collections;

namespace S3DE
{
    class EngineMain
    {
        static bool isRunning;
        static bool reziseWindow = false;
        static bool windowResized = false;

        static Game game;

        internal static Game RunningGame => game;

        internal static bool WindowResized
        {
            get => windowResized;
        }

        internal static bool ResizeWindow
        {
            get => reziseWindow;
            set => reziseWindow = value;
        }

        public static bool IsRunning => isRunning;

        public static void RunGame(Game game, bool singleFrame)
        {
            EngineMain.game = game;

            Console.WriteLine("Initializing Game");
            game.InitGame();

            Console.WriteLine("Starting Clock");
            Time.Start();
            Console.WriteLine("Initializing GLFW");
            Glfw.Init();
            
            
            Console.WriteLine($"Creating new Window");
            Engine.Graphics.Window.CreateWindow(game.GameName());
            Engine.Graphics.Window.MakeCurrentContext();

            Console.WriteLine($"Intiializing Renderer | Name: {Renderer.GetName()} | API: {Renderer.GetAPI}");
            Renderer.Init_Internal();

            Console.WriteLine($"Setting Renderer Capabilities");
            Renderer.SetCapabilities_Internal();
            Console.WriteLine("Initializing TextureUnits");
            TextureUnits.Initialize();
            Console.WriteLine("Intiializing BindingPoints");
            UniformBuffers.Initialize_BindingPoints();
            Console.WriteLine("Initializing RenderPipeline");
            RenderPipeline.Init_Internal();
            Console.WriteLine($"Vectors are {(System.Numerics.Vector.IsHardwareAccelerated ? "" : "not")} hardware accelerated.");
            Console.WriteLine($"Starting Game {game.GameName()}");
            game.StartGame();
            Console.WriteLine($"Game: {game.GameName()} started");
            isRunning = true;

            Console.WriteLine($"Creating the Main Rendercall");
            Renderer.CreateMainRenderCall();
            Console.WriteLine($"Creating ScreenQuad");
            ScreenQuad.Create();
            Time.UpdateDeltaTime(Time.CurrentTick);
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("----------------Intialization Completed---------------------");
            Console.WriteLine("------------------------------------------------------------");

            if (singleFrame)
            {
                MainLoop();
                Console.WriteLine("End of frame.");
                Console.WriteLine("Press Enter To Exit");
                Console.Read();
            }
            else
                while (!Engine.Graphics.Window.IsCloseRequested) MainLoop();
        }

        private static void MainLoop()
        {
            Engine.Graphics.Window.PollEvents();

            Input.PollInput();
            SceneHandler.RunScenes();

            Renderer.Finish_Internal();
            FrameSync.WaitForTargetFPS();
            Engine.Graphics.Window.SwapBuffer();

            Time.UpdateDeltaTime(Time.CurrentTick);

            windowResized = false;
            if (reziseWindow)
            {
                Engine.Graphics.Window.ResizeWindow();
                reziseWindow = false;
                windowResized = true;
            }
        }

        internal static void StopEngine(int exitCode)
        {
            isRunning = false;
            Environment.Exit(exitCode);
        }

        internal static void StopEngine_ConfirmationRequired(int exitCode)
        {
            isRunning = false;
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Environment.Exit(exitCode);
        }
    }
}
