using S3DE.Collections;
using S3DE.Components;
using S3DE.Scenes;
using S3DE.Utility;
using System.Collections.Generic;
using System.Linq;

namespace S3DE.Entities
{
    public sealed class GameEntity
    {
        List<EntityComponent> components;
        DualList<EntityComponent> componentsToStart;
        List<EntityComponent> inactiveComponents;
        List<EntityComponent> activeComponents;
        
        GameScene scene;
        bool isActive;

        Transform trans;

        public Transform transform => trans;
        public GameScene ParentScene => scene;
        public bool IsActive => isActive;
        
        internal void InitComponents()
        {
            if (!componentsToStart.IsEmpty)
            {
                componentsToStart.SwapAndClear();
                foreach (EntityComponent ec in componentsToStart)
                    ec.Init_Internal();
            }
        }

        internal void StartComponents()
        {
            if (!componentsToStart.IsEmpty)
                foreach (EntityComponent ec in componentsToStart)
                {
                    ec.Start_Internal();
                    activeComponents.Add(ec);
                }
        }

        internal void EarlyUpdate()
        {
            for (int i = 0; i < activeComponents.Count; i++)
                activeComponents[i].EarlyUpdate_Internal();
        }

        internal void Update()
        {
            for (int i = 0; i < activeComponents.Count; i++)
                activeComponents[i].Update_Internal();
        }

        internal void LateUpdate()
        {
            for (int i = 0; i < activeComponents.Count; i++)
                activeComponents[i].LateUpdate_Internal();
        }

        internal void PreDraw()
        {
            for (int i = 0; i < activeComponents.Count; i++)
                activeComponents[i].PreDraw_Internal();
        }

        internal void Draw()
        {
            for (int i = 0; i < activeComponents.Count; i++)
                activeComponents[i].Draw_Internal();
        }

        internal void PostDraw()
        {

            for (int i = 0; i < activeComponents.Count; i++)
                activeComponents[i].PostDraw_Internal();
        }

        private GameEntity() { components = new List<EntityComponent>(); componentsToStart = new DualList<EntityComponent>(); activeComponents = new List<EntityComponent>(); inactiveComponents = new List<EntityComponent>(); }

        public T AddComponent<T>() where T : EntityComponent
        {
            T ec = InstanceCreator.CreateInstance<T>();
            
            components.Add(ec);
            componentsToStart.Add(ec);
            ec.SetParentEntity(this);
            ec.OnCreation_Internal();
            return ec;
        }

        public void RemoveComponent(EntityComponent ec)
        {
            if (components.Contains(ec))
            {
                components.Remove(ec);
                if (ec.IsActive)
                    activeComponents.Remove(ec);
                else
                    inactiveComponents.Remove(ec);
            }
        }

        internal void ChangeComponentActivity(EntityComponent ec, bool newStatus)
        {
            if (ec.IsStarted)
            {
                if (newStatus)
                {
                    activeComponents.Add(ec);
                    inactiveComponents.Remove(ec);
                }else
                {
                    inactiveComponents.Add(ec);
                    activeComponents.Remove(ec);
                }
                    
            }
        }

        internal static GameEntity Create(GameScene parentScene)
        {
            GameEntity ge = new GameEntity();
            ge.scene = parentScene;
            ge.trans = ge.AddComponent<Transform>();
            ge.isActive = true;
            return ge;
        }
    }
}
