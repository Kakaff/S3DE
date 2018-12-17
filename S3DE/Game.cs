using S3DE.Graphics;
using S3DE.Maths;
using S3DE.Scenes;
using System;

namespace S3DE
{
    public abstract class Game
    {
        public static bool IsFocused => Window.IsFocused;
        public static bool LostFocus => Window.LostFocus;
        public static bool RegainedFocus => Window.RegainedFocus;

        public static bool ResolutionChanged => Window.ResolutionChanged;
        public static Vector2 DisplayResolution => Window.Resolution;
        public static Vector2 RenderResolution => Renderer.Resolution;
        
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

        protected abstract Vector2 LoadDisplayResolution();
        protected abstract Vector2 LoadRenderResolution();

        internal Vector2 Internal_LoadDisplayResolution => LoadDisplayResolution();
        internal Vector2 Internal_LoadRenderResolution => LoadRenderResolution();

        public abstract String GameName { get; }

        public static void Enable_Powersaving(bool b) => EngineMain.Extern_EnablePowerSaving(b);
        public static void Set_TargetFramerate(uint value) => EngineMain.Extern_SetTargetFramerate((int)value);
    }
}
