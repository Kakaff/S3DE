using S3DE.Engine.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Entities
{
    public abstract class EntityComponent
    {
        bool isActive;
        GameEntity entity;
        
        public bool IsActive
        {
            get
            {
                return isActive & entity.IsActive;
            }
        }
        public GameEntity gameEntity
        {
            get
            {
                return entity;
            }
        }

        public Transform transform
        {
            get
            {
                return entity.transform;
            }
        }

        protected abstract void OnCreation();

        protected virtual void Init() { }
        protected virtual void Start() { }

        protected virtual void EarlyUpdate() { }
        protected virtual void Update() { }
        protected virtual void LateUpdate() { }

        protected virtual void Draw() { }
        protected virtual void PostDraw() { }

        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }

        internal void OnCreation_Internal()
        {
            OnCreation();
        }

        internal void Init_Internal()
        {
            Init();
        }

        internal void Start_Internal()
        {
            Start();
        }

        internal void EarlyUpdate_Internal()
        {
            EarlyUpdate();
        }

        internal void Update_Internal()
        {
            Update();
        }

        internal void LateUpdate_Internal()
        {
            LateUpdate();
        }

        internal void Draw_Internal()
        {
            Draw();
        }
        
        internal void PostDraw_Internal()
        {
            PostDraw();
        }

        internal void SetParentEntity(GameEntity parent)
        {
            entity = parent;
        }
    }
}
