using S3DE.Engine;
using S3DE.Engine.Graphics;
using S3DE.Engine.Scenes;
using S3DE.Engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE
{
    public abstract class Game
    {
        protected void SetStartScene<T>() where T : GameScene => SceneHandler.SetMainScene<T>();
        
        protected void SetTargetRenderer<T>() where T : Renderer => Renderer.SetTargetRenderer<T>();

        protected void SetEngineClock<T>() where T : EngineClock => Time.SetEngineClock<T>();

        public void Run() => EngineMain.RunGame(this);

        internal void InitGame() => InitializeGame();

        internal void SetRenderer_Internal() => SetRenderer();
        internal void SetClock_Internal() => SetClock();

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
    }
}
