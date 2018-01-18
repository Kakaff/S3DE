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
        private GameScene startScene;

        internal GameScene StartScene => startScene;

        protected void SetStartScene<T>() where T : GameScene => startScene = InstanceCreator.CreateInstance<T>();
        
        protected void SetTargetRenderer<T>() where T : Renderer => Renderer.SetTargetRenderer<T>();

        protected void SetEngineClock<T>() where T : EngineClock => Time.SetEngineClock<T>();

        public void Run() => EngineMain.RunGame(this);

        internal void InitGame() => InitializeGame();

        private void InitializeGame()
        {
            Initialize();
        }

        protected abstract void Initialize();
        public abstract String GameName();

        protected abstract void OnGameExit();


        internal void exitGame() => OnGameExit();
    }
}
