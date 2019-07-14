using S3DECore.Graphics;
using S3DECore.Graphics.Framebuffers;
using S3DECore.Graphics.Textures;
using S3DECore.Input;
using S3DECore.Math;
using S3DECore;
using S3DE.Scenes;
using System;

namespace S3DE
{
    partial class EngineMain
    {
        static Game game;
        
        public static void RunGame(Game game)
        {
            EngineMain.game = game;

            //Renderer.OnDisplayResolutionChanged += Window.OnDisplayResChanged;
            
            Console.WriteLine("Initializing GLFW");
            if (!GLFW.Init())
                throw new Exception("Error initializing GLFW");

            Console.WriteLine("GLFW successfully initialized");

            Console.WriteLine("Creating window");
            if (!Window.CreateWindow(game.Internal_LoadDisplayResolution))
                throw new Exception("Error creating window!");

            Console.WriteLine("Window successfully created");

            Console.WriteLine("Initializing Renderer");
            Renderer.Init(game.Internal_LoadDisplayResolution, game.Internal_LoadRenderResolution);
            Console.WriteLine("Renderer successfully initialized");

            Console.WriteLine("Initializing Input");
            Input.Init();
            
            Console.WriteLine("Initializing Game");
            game.InitGame();

            Console.WriteLine("Starting Game");
            game.StartGame();
            RunEngine();
        }

        static void RunEngine()
        {
            S3DECore.Time.EngineClock.SetTargetFramerate(60);
            S3DECore.Graphics.Renderer.Enable(S3DECore.Graphics.RendererCapability.DepthTest);
            S3DECore.Graphics.Renderer.Enable(S3DECore.Graphics.RendererCapability.CullFace);

            while (!S3DECore.Window.IsCloseRequested())
            {
                //Time.UpdateDeltaTime();
                S3DECore.Input.Input.PollEvents();
                S3DECore.Window.UpdateFocus();

                SceneHandler.UpdateActiveScene();
                SceneHandler.ActiveScene.PresentFrame();

                Window.SwapBuffers();
                Renderer.UpdateEvents();
                S3DECore.Time.EngineClock.WaitForNextFrame();
                
            }
        }
        
    }
}
