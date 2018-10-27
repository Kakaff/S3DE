using S3DE.Entities;
using S3DE.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Scenes
{
    public abstract class GameScene
    {
        List<GameEntity> activeEntities = new List<GameEntity>();
        List<GameEntity> inActiveEntities = new List<GameEntity>();


        List<GameEntity> entitiesToAdd = new List<GameEntity>();

        GameEntity[] frameStageEntities;

        Camera activeCamera;

        public Camera ActiveCamera
        {
            get => GetActiveCamera();
            set => activeCamera = value;
        }

        bool isStarted;

        internal bool IsStarted => isStarted;

        internal GameEntity[] ActiveEntities
        {
            get
            {
                InitFrameStage();
                return frameStageEntities;
            }
        }

        internal void AddEntity(GameEntity ge)
        {
            entitiesToAdd.Add(ge);
        }
        
        internal void RemoveEntity(GameEntity ge)
        {
            if (ge.IsActive && activeEntities.Contains(ge))
                activeEntities.Remove(ge);
            else if (inActiveEntities.Contains(ge))
                inActiveEntities.Remove(ge);
            else if (entitiesToAdd.Contains(ge))
                entitiesToAdd.Remove(ge);
        }

        internal void InitFrameStage() => frameStageEntities = activeEntities.ToArray();

        internal void UpdateScene()
        {
            for (int i = 0; i < entitiesToAdd.Count; i++)
            {
                GameEntity ge = entitiesToAdd[i];
                if (entitiesToAdd[i].IsActive)
                    activeEntities.Add(ge);
                else
                    inActiveEntities.Add(ge);
            }
            entitiesToAdd.Clear();

            Update();
            Render();
        }

        void Update()
        {
            InitFrameStage();
            foreach (GameEntity ge in frameStageEntities)
            {
                ge.InitComponents();
                ge.StartComponents();
                ge.EarlyUpdate();
                ge.Update();
                ge.LateUpdate();
            }
        }

        void Render()
        {
            InitFrameStage();
            foreach (GameEntity ge in frameStageEntities)
            {
                ge.PreDraw();
                ge.Draw();
                ge.PostDraw();
            }
        }

        internal void Start_Internal()
        {
            StartScene();
            isStarted = true;
        }

        internal GameEntity CreateGameEntity_Internal()
        {
            GameEntity ge = GameEntity.Create(this);
            AddEntity(ge);
            return ge;
        }

        Camera GetActiveCamera()
        {
            if (activeCamera == null)
            {
                GameEntity ge = GameEntity.Create(this);
                Camera c = ge.AddComponent<Camera>();
                activeCamera = c;
                activeEntities.Add(ge);
                Console.WriteLine("No camera set for this scene, creating a new one");
            }

            return activeCamera;
        }

        protected GameEntity CreateEntity() => CreateGameEntity_Internal();
        protected abstract void LoadScene();
        protected abstract void UnloadScene();
        protected abstract void StartScene();

        internal void Load_Internal() => LoadScene();
        internal void Unload_Internal() => UnloadScene();
    }
}
