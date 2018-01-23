using S3DE.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Scenes
{
    public abstract class GameScene
    {
        List<GameEntity> activeEntities = new List<GameEntity>();
        List<GameEntity> inActiveEntities = new List<GameEntity>();

        GameEntity[] frameStageEntities;

        internal void AddEntity(GameEntity ge)
        {
            if (ge.IsActive)
                activeEntities.Add(ge);
            else
                inActiveEntities.Add(ge);
        }
        
        internal void RemoveEntity(GameEntity ge)
        {
            if (ge.IsActive && activeEntities.Contains(ge))
                activeEntities.Remove(ge);
            else if (inActiveEntities.Contains(ge))
                inActiveEntities.Remove(ge);
        }

        internal void InitFrameStage() => frameStageEntities = activeEntities.ToArray();

        internal void EarlyUpdate()
        {
            InitFrameStage();

            foreach (GameEntity ge in frameStageEntities)
            {
                ge.InitComponents();
                ge.StartComponents();
                ge.EarlyUpdate();
            }
        }

        internal void Update()
        {
            InitFrameStage();
            foreach (GameEntity ge in frameStageEntities)
                ge.Update();
        }

        internal void LateUpdate()
        {
            InitFrameStage();
            foreach (GameEntity ge in frameStageEntities)
                ge.LateUpdate();
        }

        internal void Draw()
        {
            InitFrameStage();
            foreach (GameEntity ge in frameStageEntities)
                ge.Draw();
        }

        internal void PostDraw()
        {
            InitFrameStage();
            foreach (GameEntity ge in frameStageEntities)
                ge.PostDraw();
        }

        internal void Run(bool canDraw)
        {

            EarlyUpdate();
            Update();
            LateUpdate();

            if (canDraw)
            {
                Draw();
                PostDraw();
            }
        }

        internal void ReloadScene()
        {
            //Dispose the scene. (call Dispose() and dispose all entities and their components).
            //Load the scene.
        }
        protected abstract void LoadScene();
        protected abstract void UnloadScene();

        internal GameEntity _createGameEntity() => CreateGameEntity();
        protected GameEntity CreateGameEntity() {
            GameEntity ge = GameEntity.Create(this);
            AddEntity(ge);
            return ge;
        }

        internal void Load_Internal() => LoadScene();
        internal void Unload_Internal() => UnloadScene();
    }
}
