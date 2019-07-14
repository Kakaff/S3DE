using S3DE.Entities;
using S3DE.Scenes;

namespace S3DE.Components
{
    public abstract class EntityComponent
    {
        bool isActive = true,isStarted = false;
        GameEntity entity;

        public bool IsActive
        {
            get => isActive & entity.IsActive;
            set
            {
                if (isActive != value) {
                    isActive = value;
                    entity.ChangeComponentActivity(this, value);
                }
            }
        }
        internal bool IsStarted => isStarted;

        public GameEntity Entity => entity;
        public GameScene Scene => entity.ParentScene;
        public Transform transform => entity.transform;

        protected static double DeltaTime => S3DECore.Time.EngineClock.DeltaTime;
        protected virtual void OnCreation() { }
        protected virtual void OnDestruction() { }

        protected virtual void Init() { }
        protected virtual void Start() { }
        
        protected virtual void EarlyUpdate() { }
        protected virtual void Update() { }
        protected virtual void LateUpdate() { }

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

        protected GameEntity CreateEntity() => Scene.CreateGameEntity_Internal();

        internal void EarlyUpdate_Internal() => EarlyUpdate();
        internal void Update_Internal() => Update();
        internal void LateUpdate_Internal() => LateUpdate();

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
