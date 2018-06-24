using S3DE.Engine.Entities.Components;
using S3DE.Engine.Scenes;
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

        public GameScene Scene => entity.Scene;

        protected float DeltaTime => Time.DeltaTime;

        public bool IsActive
        {
            get => isActive & entity.IsActive;
            set
            {
                IEnableLogic l = (IEnableLogic)this;
                if (isActive != value && l != null) {
                    if (value)
                        l.OnEnable();
                    else
                        l.OnDisable();

                    isActive = value;
                    entity.ChangeComponentActivity(this, value);
                }
            }
        }
        internal bool IsStarted => isStarted;

        public GameEntity gameEntity => entity;

        public Transform transform => entity.transform;

        protected virtual void OnCreation() { }
        protected virtual void OnDestruction() { }

        protected virtual void Init() { }
        protected virtual void Start() { }
        
        protected virtual void PreRender() { }
        protected virtual void Render() { }
        protected virtual void PostRender() { }

        internal void OnCreation_Internal() => OnCreation();

        internal void OnDestruction_Internal()
        {
            OnDestruction();

            if (entity != null)
            {
                entity.RemoveComponent(this);
                entity = null;
            }
            isActive = false;
            isStarted = false;
        }

        public void Destroy() => OnDestruction_Internal();

        internal void Init_Internal() => Init();

        internal void Start_Internal()
        {
            Start();
            isStarted = true;
        }

        internal void PreDraw_Internal() => PreRender();

        internal void Draw_Internal() => Render();

        internal void PostDraw_Internal() => PostRender();

        internal void SetParentEntity(GameEntity parent) => entity = parent;

        ~EntityComponent()
        {
            Destroy();
        }
    }
}
