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

        public delegate void OnFrameUpdate();
        private static OnFrameUpdate delegateInstance;

        public static void RunGame(Game game)
        {
            EngineMain.game = game;

            Renderer.OnDisplayResolutionChanged += Window.OnDisplayResChanged;

            Console.WriteLine("Creating window");
            Window._CreateWindow(game.Internal_LoadDisplayResolution, game.GameName);
            Console.WriteLine("Initializing Renderer");
            Renderer.Init(game.Internal_LoadDisplayResolution,game.Internal_LoadRenderResolution);
            Console.WriteLine("Registering update callback");
            delegateInstance = UpdateFrame;
            RegisterUpdateFunc(delegateInstance);
            
            Texture.InitTextures();
            game.InitGame();
            game.StartGame();
            Console.WriteLine("Running engine core");
            RunEngine();
        }

        static void UpdateFrame()
        {
            Time.UpdateDeltaTime();

            Window.UpdateFocus();
            Input_Handler.PollInput();
            
            SceneHandler.UpdateActiveScene();
            SceneHandler.ActiveScene.PresentFrame();
            
            Renderer.UpdateEvents();

            

        }
    }
}
