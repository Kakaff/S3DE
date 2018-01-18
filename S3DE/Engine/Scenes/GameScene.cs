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
        List<GameEntity> activeEntities;
        List<GameEntity> inActiveEntities;

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

        protected abstract void LoadScene();
        protected abstract void UnloadScene();

        internal GameEntity _createGameEntity() => CreateGameEntity();
        protected GameEntity CreateGameEntity() => GameEntity.Create(this);

        internal void Load_Internal() => LoadScene();
        internal void Unload_Internal() => UnloadScene();
    }
}
