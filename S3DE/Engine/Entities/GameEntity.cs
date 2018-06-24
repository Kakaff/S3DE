using S3DE.Engine.Data;
using S3DE.Engine.Entities.Components;
using S3DE.Engine.Scenes;
using S3DE.Engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Entities
{
    public sealed class GameEntity
    {

        List<EntityComponent> components;
        DualList<EntityComponent> componentsToStart;
        List<EntityComponent> inactiveComponents;
        List<EntityComponent> activeComponents;

        EntityComponent[] frameStageComponents;
        IUpdateLogic[] updateStageComponents;

        bool isActive;

        Transform trans;

        GameScene parentScene;

        public GameScene Scene => parentScene;

        public Transform transform => trans;

        public bool IsActive => isActive;

        //Init stage causing shit performance.
        void InitStage() => frameStageComponents = components.Where(ec => ec.IsActive && ec.IsStarted).ToArray();
        void InitUpdateStage() => updateStageComponents = components.Where(ec => ec.IsActive && ec.IsStarted && ec is IUpdateLogic).Cast<IUpdateLogic>().ToArray();

        internal void InitComponents()
        {
            componentsToStart.SwapAndClear();
            foreach (EntityComponent ec in componentsToStart)
                ec.Init_Internal();
        }

        internal void StartComponents()
        {
            foreach (EntityComponent ec in componentsToStart)
            {
                ec.Start_Internal();
                activeComponents.Add(ec);
            }
        }


        internal void EarlyUpdate()
        {
            foreach (EntityComponent ec in activeComponents)
                if (ec is IUpdateLogic)
                    ((IUpdateLogic)ec).EarlyUpdate();
        }

        internal void Update()
        {
            foreach (EntityComponent ec in activeComponents)
                if (ec is IUpdateLogic)
                    ((IUpdateLogic)ec).Update();
        }

        internal void LateUpdate()
        {
            foreach (EntityComponent ec in activeComponents)
                if (ec is IUpdateLogic)
                    ((IUpdateLogic)ec).LateUpdate();
        }

        internal void PreDraw()
        {
            foreach (EntityComponent ec in activeComponents)
                ec.PreDraw_Internal();
        }

        internal void Draw()
        {
            foreach (EntityComponent ec in activeComponents)
                ec.Draw_Internal();
        }

        internal void PostDraw()
        {
            foreach (EntityComponent ec in activeComponents)
                ec.PostDraw_Internal();
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

        internal static GameEntity Create(GameScene scene)
        {
            //Create Transform.
            GameEntity ge = new GameEntity();
            ge.parentScene = scene;
            ge.trans = ge.AddComponent<Transform>();
            ge.isActive = true;
            return ge;
        }
    }
}
