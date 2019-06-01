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
        
        public static Vector2 DisplayResolution => Renderer.DisplayResolution;
        public static Vector2 RenderResolution => Renderer.RenderResolution;

        public void Run() => EngineMain.RunGame(this);
        internal void InitGame() => Initialize();
        internal void StartGame() => Start();

        protected void LoadStartScene<Scene>() where Scene : GameScene
        {
            GameScene gs = SceneHandler.LoadScene<Scene>();
            SceneHandler.SetActiveScene(gs);
        }

        public static void LogWarning(string s)
        {
            Console.WriteLine($"WARNING: {s}");
        }

        protected abstract void Initialize();
        protected abstract void Start();

        protected abstract Vector2 LoadDisplayResolution();
        protected abstract Vector2 LoadRenderResolution();

        internal Vector2 Internal_LoadDisplayResolution => LoadDisplayResolution();
        internal Vector2 Internal_LoadRenderResolution => LoadRenderResolution();

        public abstract String GameName { get; }

        public static void Enable_Powersaving(bool b) => S3DECore.Time.EngineClock.SetPowerSaving(b);// EngineMain.Extern_EnablePowerSaving(b);
        public static void Set_TargetFramerate(uint value) => S3DECore.Time.EngineClock.SetTargetFramerate(value);// EngineMain.Extern_SetTargetFramerate((int)value);
    }
}
