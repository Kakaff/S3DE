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
        List<EntityComponent> componentsToStart;
        List<EntityComponent> inactiveComponents; 

        EntityComponent[] frameStageComponents;
        IUpdateLogic[] updateStageComponents;

        bool isActive;

        Transform trans;

        GameScene parentScene;

        public GameScene Scene => parentScene;

        public Transform transform => trans;

        public bool IsActive => isActive;

        void InitStage() => frameStageComponents = components.Where(ec => ec.IsActive && ec.IsStarted).ToArray();
        void InitUpdateStage() => updateStageComponents = components.Where(ec => ec.IsActive && ec.IsStarted && ec is IUpdateLogic).Cast<IUpdateLogic>().ToArray();
        void GetComponentsToInitAndStart() => componentsToStart.AddRange(components.Where(ec => ec.IsActive && !ec.IsStarted));

        internal void InitComponents()
        {
            GetComponentsToInitAndStart();
            foreach (EntityComponent ec in componentsToStart)
                ec.Init_Internal();
        }

        internal void StartComponents()
        {
            foreach (EntityComponent ec in componentsToStart)
                ec.Start_Internal();
        }


        internal void EarlyUpdate()
        {
            InitUpdateStage();
            foreach (IUpdateLogic iul in updateStageComponents)
                iul.EarlyUpdate();
        }

        internal void Update()
        {
            InitUpdateStage();
            foreach (IUpdateLogic iul in updateStageComponents)
                iul.Update();
        }

        internal void LateUpdate()
        {
            InitUpdateStage();
            foreach (IUpdateLogic iul in updateStageComponents)
                iul.LateUpdate();
        }

        internal void PreDraw()
        {
            InitStage();
            foreach (EntityComponent ec in frameStageComponents)
                ec.PreDraw_Internal();
        }

        internal void Draw()
        {
            InitStage();
            foreach (EntityComponent ec in frameStageComponents)
                ec.Draw_Internal();
        }

        internal void PostDraw()
        {
            InitStage();
            foreach (EntityComponent ec in frameStageComponents)
                ec.PostDraw_Internal();
        }

        private GameEntity() { components = new List<EntityComponent>(); componentsToStart = new List<EntityComponent>(); }

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
                components.Remove(ec);
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
