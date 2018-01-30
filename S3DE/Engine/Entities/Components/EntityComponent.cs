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
        bool isActive = true,isStarted = false;
        GameEntity entity;

        protected float DeltaTime => Time.DeltaTime;

        public bool IsActive
        {
            get => isActive & entity.IsActive;
            set
            {
                if (isActive != value)
                    if (value)
                        OnEnable();
                    else
                        OnDisable();
            }
        }
        internal bool IsStarted => isStarted;

        public GameEntity gameEntity => entity;

        public Transform transform => entity.transform;

        protected abstract void OnCreation();

        protected virtual void Init() { }
        protected virtual void Start() { }

        protected virtual void EarlyUpdate() { }
        protected virtual void Update() { }
        protected virtual void LateUpdate() { }

        protected virtual void PreDraw() { }
        protected virtual void Draw() { }
        protected virtual void PostDraw() { }

        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }

        internal void OnCreation_Internal() => OnCreation();

        internal void Init_Internal() => Init();

        internal void Start_Internal()
        {
            Start();
            isStarted = true;
        }

        internal void EarlyUpdate_Internal() => EarlyUpdate();

        internal void Update_Internal() => Update();

        internal void LateUpdate_Internal() => LateUpdate();

        internal void PreDraw_Internal() => PreDraw();

        internal void Draw_Internal() => Draw();

        internal void PostDraw_Internal() => PostDraw();

        internal void SetParentEntity(GameEntity parent) => entity = parent;
    }
}
