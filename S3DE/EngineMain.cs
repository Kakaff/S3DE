﻿using glfw3;
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

        static void CheckContext()
        {
            if (Engine.Graphics.Window.IsCurrentContext())
                Console.WriteLine("Renderer has a context");
            else
                Console.WriteLine("Renderer does not have a context");
        }
        public static void RunGame(Game game)
        {
            EngineMain.game = game;

            game.InitGame();

            Time.Start();
            Glfw.Init();

            /*
             * On 2 of the 3 gpu's i've tested, the renderer has to be initialized before creating a window.
             * and making the window context current.
             */

            Engine.Graphics.Window.CreateWindow(game.GameName());
            Engine.Graphics.Window.MakeCurrentContext();

            Renderer.Init_Internal();
            Renderer.SetCapabilities_Internal();
            TextureUnits.Initialize();
            RenderPipeline.Init_Internal();
            game.StartGame();
            isRunning = true;
            
            Renderer.CreateMainRenderCall();
            ScreenQuad.Create();
            Time.UpdateDeltaTime(Time.CurrentTick);
            
            MainLoop();
        }

        private static void MainLoop()
        {
            while (!Engine.Graphics.Window.IsCloseRequested)
            {
                Engine.Graphics.Window.PollEvents();
                
                Input.PollInput();
                SceneHandler.RunScenes();
                
                Engine.Graphics.Window.SwapBuffer();
                FrameSync.WaitForTargetFPS();
                Time.UpdateDeltaTime(Time.CurrentTick);
                
                windowResized = false;
                if (reziseWindow)
                {
                    reziseWindow = false;
                    windowResized = true;
                    Engine.Graphics.Window.ResizeWindow();
                }
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
            Console.Read();
            Environment.Exit(exitCode);
        }
    }
}
