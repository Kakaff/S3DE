using S3DE.Graphics;
using S3DE.Graphics.FrameBuffers;
using S3DE.Graphics.Textures;
using S3DE.Input;
using S3DE.Maths;
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

            Renderer.OnDisplayResolutionChanged += Window.OnDisplayResChanged;
            
            Console.WriteLine("Initializing GLFW");
            if (!S3DECore.GLFW.Init())
                throw new Exception("Error initializing GLFW");

            Console.WriteLine("Creating window");
            if (!Window.CreateWindow(game.Internal_LoadDisplayResolution, game.GameName))
                throw new Exception("Error creating window!");
            
            Console.WriteLine("Initializing Renderer");
            if (!Renderer.Init(game.Internal_LoadDisplayResolution,game.Internal_LoadRenderResolution))
                throw new Exception("Error initializing renderer");

            Console.WriteLine("Initializing Textures");
            Texture.InitTextures();
            Console.WriteLine("Initializing Game");
            game.InitGame();

            Console.WriteLine("Starting Game");
            game.StartGame();
            RunEngine();
        }

        static void RunEngine()
        {
            S3DECore.Graphics.Renderer.SetViewportSize(0, 0,(int)Renderer.DisplayResolution.x, (int)Renderer.DisplayResolution.y);
            S3DECore.Time.EngineClock.SetTargetFramerate(60);
            Renderer.Enable(GlEnableCap.DepthTest);
            Renderer.Enable(GlEnableCap.CullFace);

            while (!S3DECore.Window.IsCloseRequested())
            {
                Time.UpdateDeltaTime();
                S3DECore.Input.Keyboard.PollEvents();
                Window.UpdateFocus();
                Input_Handler.PollInput();

                SceneHandler.UpdateActiveScene();
                SceneHandler.ActiveScene.PresentFrame();
                Window.SwapBuffers();
                Renderer.UpdateEvents();
                S3DECore.Time.EngineClock.WaitForNextFrame(Renderer.Vsync);
                
            }
        }
        
    }
}
