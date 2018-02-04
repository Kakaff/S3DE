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

        public static Vector2 DisplayResolution {
           get => Renderer.DisplayResolution;
           set => Renderer.SetDisplayResolution(value);
        }

        public static Vector2 RenderResolution
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

        protected void SetStartScene<T>() where T : GameScene => SceneHandler.SetMainScene<T>();
        
        protected void SetTargetRenderer<T>() where T : Renderer => Renderer.SetTargetRenderer<T>();
        protected void SetEngineClock<T>() where T : EngineClock => Time.SetEngineClock<T>();
        protected void SetEngineFrameSync<T>() where T : FrameSync => FrameSync.SetFrameSync = InstanceCreator.CreateInstance<T>();

        protected void SetTargetFPS(uint fps) => FrameSync.SetTargetFPS(fps);
        public void Run() => EngineMain.RunGame(this);

        internal void InitGame() => InitializeGame();

        internal void SetRenderer_Internal() => SetRenderer();
        internal void SetClock_Internal() => SetClock();
        internal void SetFrameSync_Internal() => SetFrameSync();

        protected abstract void SetFrameSync();
        protected abstract void SetRenderer();
        protected abstract void SetClock();

        private void InitializeGame()
        {
            Initialize();
        }

        protected abstract void Initialize();
        public abstract String GameName();

        protected abstract void OnGameExit();

        internal void exitGame() => OnGameExit();

        public static void Exit(int exitCode) {
            EngineMain.StopEngine(exitCode);
        }

        public static void SetFullScreen(bool fullscreen) => Window.SetFullScreen(fullscreen);
    }
}
