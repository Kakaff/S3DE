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
        EntityComponent[] frameComponents;
        bool isActive;

        Transform trans;

        GameScene parentScene;

        public Transform transform => trans;

        public bool IsActive => isActive;

        internal void InitFrame()
        {
            frameComponents = components.Where(ec => ec.IsActive).ToArray();
            EntityComponent[] _t = components.Where(ec => !ec.IsActive).ToArray();
            StartComponents();
            if (_t.Length > 0)
                componentsToStart.AddRange(_t);
        }

        void StartComponents()
        {
            EntityComponent[] ects = componentsToStart.ToArray();

            for(int i = 0; i < ects.Length; i++)
            {
                ects[i].Start_Internal();
            }

            componentsToStart.Clear();
        }

        internal void EarlyUpdate()
        {

        }

        internal void Update()
        {

        }

        internal void LateUpdate()
        {

        }

        internal void Draw()
        {

        }

        internal void PostDraw()
        {

        }

        private GameEntity() { components = new List<EntityComponent>(); componentsToStart = new List<EntityComponent>(); }

        public T AddComponent<T>() where T : EntityComponent
        {
            T ec = InstanceCreator.CreateInstance<T>();
            components.Add(ec);
            componentsToStart.Add(ec);

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
