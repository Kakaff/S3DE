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

            Console.WriteLine("Creating window");
            Window._CreateWindow(game.Internal_LoadDisplayResolution, game.GameName);
            Console.WriteLine("Initializing Renderer");
            Renderer.Init(game.Internal_LoadRenderResolution);
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
            if (Window.PendingChanges)
                Window.ApplyChanges();

            if (FrameBuffer.DefaultFrameBuffer == null)
                FrameBuffer.DefaultFrameBuffer = FrameBuffer.Create_Standard_FrameBuffer(Renderer.RenderResolution);

            if (FrameBuffer.ActiveFrameBuffer == null)
            {
                FrameBuffer.DefaultFrameBuffer.Bind();
                FrameBuffer.ActiveFrameBuffer.Clear(ClearBufferBit.COLOR | ClearBufferBit.DEPTH);
            }
            
            SceneHandler.UpdateActiveScene();
            //Error while drawing scene!
            FrameBuffer.ActiveFrameBuffer.PresentFrame();
            
            
            if (Window.ResolutionChanged && Window.ChangesApplied)
                Window.ResolutionChanged = false;

        }
    }
}
