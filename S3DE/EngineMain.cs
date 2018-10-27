using S3DE.Graphics;
using S3DE.Graphics.Shaders;
using S3DE.Graphics.Textures;
using S3DE.Input;
using S3DE.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

            Console.WriteLine("Creating window");
            Window._CreateWindow(game.Internal_LoadDisplayResolution, game.GameName);
            Console.WriteLine("Initializing Renderer");
            Renderer.Init(game.Internal_LoadRenderResolution);
            Console.WriteLine("Registering update callback");
            delegateInstance = UpdateFrame;
            RegisterUpdateFunc(delegateInstance);
            
            Time.InitTime();
            ITexture.InitTextures();
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
            if (Window.PendingChanges)
                Window.ApplyChanges();

            SceneHandler.UpdateActiveScene();

            if (Window.ResolutionChanged && Window.ChangesApplied)
                Window.ResolutionChanged = false;

        }
    }
}
