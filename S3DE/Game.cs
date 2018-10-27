using S3DE.Graphics;
using S3DE.Graphics.Textures;
using S3DE.Maths;
using S3DE.Scenes;
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
        public static bool LostFocus => Window.LostFocus;
        public static bool RegainedFocus => Window.RegainedFocus;

        public static bool ResolutionChanged => Window.ResolutionChanged;
        public static S3DE_Vector2 DisplayResolution => Window.Resolution;
        public static S3DE_Vector2 RenderResolution => Renderer.Resolution;
        
        public void Run() => EngineMain.RunGame(this);
        internal void InitGame() => Initialize();
        internal void StartGame() => Start();

        protected void LoadStartScene<Scene>() where Scene : GameScene
        {
            GameScene gs = SceneHandler.LoadScene<Scene>();
            SceneHandler.SetActiveScene(gs);
        }

        protected abstract void Initialize();
        protected abstract void Start();

        protected abstract S3DE_Vector2 LoadDisplayResolution();
        protected abstract S3DE_Vector2 LoadRenderResolution();

        internal S3DE_Vector2 Internal_LoadDisplayResolution => LoadDisplayResolution();
        internal S3DE_Vector2 Internal_LoadRenderResolution => LoadRenderResolution();

        public abstract String GameName { get; }
    }
}
