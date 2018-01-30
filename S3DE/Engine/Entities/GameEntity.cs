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
        EntityComponent[] frameStageComponents;
        bool isActive;

        Transform trans;

        GameScene parentScene;

        public Transform transform => trans;

        public bool IsActive => isActive;

        void InitStage() => frameStageComponents = components.Where(ec => ec.IsActive && ec.IsStarted).ToArray();

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
            InitStage();
            foreach (EntityComponent ec in frameStageComponents)
                ec.EarlyUpdate_Internal();
        }

        internal void Update()
        {
            InitStage();
            foreach (EntityComponent ec in frameStageComponents)
                ec.Update_Internal();
        }

        internal void LateUpdate()
        {
            InitStage();
            foreach (EntityComponent ec in frameStageComponents)
                ec.LateUpdate_Internal();
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
