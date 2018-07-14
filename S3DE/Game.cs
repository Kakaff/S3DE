using S3DE.Engine;
using S3DE.Engine.Graphics;
using S3DE.Engine.Scenes;
using S3DE.Engine.Utility;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE
{
    public abstract class Game
    {
        public static bool IsFocused => Window.IsFocused;
        public static bool RegainedFocus => Window.RegainedFocus;
        public static bool LostFocus => Window.LostFocus;

        public static S3DE_Vector2 DisplayResolution {
           get => Renderer.DisplayResolution;
           set => Renderer.SetDisplayResolution(value);
        }

        public static S3DE_Vector2 RenderResolution
        {
            get => Renderer.RenderResolution;
            set => Renderer.SetRenderResolution(value);
        }

        public static int RefreshRate
        {
            get => Renderer.RefreshRate;
            set => Renderer.SetRefreshRate(value);
        }

        public static bool IsFullScreen
        {
            get => Window.IsFullScreen;
        }

        public static bool ResolutionChanged => EngineMain.WindowResized;
        public static string WindowTitle {set => Engine.Graphics.Window.SetTitle(value);}
        public static string GameTitle => EngineMain.RunningGame.GameName();

        protected void SetStartScene<T>() where T : GameScene => SceneHandler.SetMainScene<T>();
        
        protected void SetTargetRenderer<T>() where T : Renderer => Renderer.SetTargetRenderer<T>();
        protected void SetEngineClock<T>() where T : EngineClock => Time.SetEngineClock<T>();
        protected void SetEngineFrameSync<T>() where T : FrameSync => FrameSync.SetFrameSync = InstanceCreator.CreateInstance<T>();
        protected void SetTargetRenderPipleline<T>() where T : RenderPipeline => RenderPipeline.CreatePipeline<T>();

        protected void SetTargetFPS(uint fps) => FrameSync.SetTargetFPS(fps);
        public void Run(bool singleFrame) => EngineMain.RunGame(this,singleFrame);
        protected void SetVSync(bool value) => Window.SetVSync(value);
        internal void InitGame() => Initialize();
        internal void StartGame() => Start();

        protected abstract void Initialize();
        protected abstract void Start();

        public abstract String GameName();

        protected abstract void OnGameExit();

        internal void exitGame() => OnGameExit();

        public static void Exit(int exitCode) {
            EngineMain.StopEngine(exitCode);
        }

        public static void SetFullScreen(bool fullscreen) => Window.SetFullScreen(fullscreen);
    }
}
