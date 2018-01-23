using S3DE.Engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Scenes
{
    internal static class SceneHandler
    {
        //A main scene, the one currently being rendered.
        static GameScene mainScene;


        static Dictionary<Type, GameScene> loadedScenes = new Dictionary<Type, GameScene>();

        //Scenes to load async
        static List<GameScene> scenesToLoadAsync = new List<GameScene>();

        internal static void RunScenes()
        {
            mainScene.Run(true);
            //Check if we have any scenes to load.
            if (scenesToLoadAsync.Count > 0) { }
                //Send the scenes to the async scene loader.

            //Check if we have any scenes that have completed loading async.
        }

        internal static void LoadScene<T>() where T : GameScene
        {
            if (mainScene != null)
                loadedScenes.Add(mainScene.GetType(),mainScene);

            if (loadedScenes.TryGetValue(typeof(T), out mainScene))
                mainScene.ReloadScene();
            else
            {
                GameScene gs = InstanceCreator.CreateInstance<T>();
                gs.Load_Internal();
                mainScene = gs;
            }
            //Loads a scene on the mainthread and sets it as the mainscene.
        }

        internal static void SetMainScene<T>() where T : GameScene
        {
            GameScene gs;

            if (!loadedScenes.TryGetValue(typeof(T),out gs)) {
                LoadScene<T>();
            } else
            {
                loadedScenes.Add(typeof(T), mainScene);
                mainScene = gs;
            }
        }

        internal static void LoadSceneAsync()
        {

        }

        internal static void UnloadScene()
        {

        }
    }
}
