using S3DE.Utility;

namespace S3DE.Scenes
{
    internal static class SceneHandler
    {
        static GameScene activeScene;

        public static GameScene ActiveScene => activeScene;

        internal static void SetActiveScene(GameScene gs)
        {
            activeScene = gs;
        }

        internal static GameScene LoadScene<Scene>() where Scene : GameScene
        {
            GameScene gs = InstanceCreator.CreateInstance<Scene>();
            gs.Load_Internal();
            return gs;
        }

        internal static void UpdateActiveScene()
        {
            if (!activeScene.IsStarted)
                activeScene.Start_Internal();

            activeScene.UpdateScene();
        }

        
    }
}
