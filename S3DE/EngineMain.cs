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
using S3DE.Engine.Data;

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

        public static void RunGame(Game game)
        {
            EngineMain.game = game;

            game.InitGame();

            Time.Start();
            Glfw.Init();


            Renderer.Init_Internal();

            Engine.Graphics.Window.CreateWindow(game.GameName());
            Engine.Graphics.Window.MakeCurrentContext();

            Renderer.SetCapabilities_Internal();

            TextureUnits.Initialize();
            S3DE_UniformBuffer.Initialize_BindingPoints();
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
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Environment.Exit(exitCode);
        }
    }
}
