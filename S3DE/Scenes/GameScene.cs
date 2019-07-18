using S3DE.Entities;
using S3DE.Components;
using System;
using System.Collections.Generic;
using S3DECore.Graphics.Framebuffers;
using S3DE.Graphics.Screen;
using S3DECore.Graphics;
using S3DECore.Graphics.Textures;
using S3DE.Graphics;

namespace S3DE.Scenes
{
    public abstract class GameScene
    {
        List<GameEntity> activeEntities = new List<GameEntity>();
        List<GameEntity> inActiveEntities = new List<GameEntity>();
        
        List<GameEntity> entitiesToAdd = new List<GameEntity>();

        Dictionary<Type, Renderpass> renderPasses = new Dictionary<Type, Renderpass>();
        List<Renderpass> renderPassList = new List<Renderpass>();
        Camera activeCamera;

        public Camera ActiveCamera
        {
            get => GetActiveCamera();
            set => activeCamera = value;
        }

        bool isStarted;

        internal bool IsStarted => isStarted;
        
        internal void AddEntity(GameEntity ge)
        {
            entitiesToAdd.Add(ge);
        }
        
        internal void RemoveEntity(GameEntity ge)
        {
            if (ge.IsActive && activeEntities.Contains(ge))
                activeEntities.Remove(ge);
            else if (inActiveEntities.Contains(ge))
                inActiveEntities.Remove(ge);
            else if (entitiesToAdd.Contains(ge))
                entitiesToAdd.Remove(ge);
        }
        
        internal void UpdateScene()
        {
            for (int i = 0; i < entitiesToAdd.Count; i++)
            {
                GameEntity ge = entitiesToAdd[i];
                if (entitiesToAdd[i].IsActive)
                    activeEntities.Add(ge);
                else
                    inActiveEntities.Add(ge);
            }
            entitiesToAdd.Clear();
            Init();
            Update();
            Render();
        }

        void Init()
        {
            GameEntity ge;
            for (int i = 0; i < activeEntities.Count; i++)
            {
                ge = activeEntities[i];
                ge.InitComponents();
                ge.StartComponents();
            }
        }

        void Update()
        {
            GameEntity ge;
            for (int i = 0; i < activeEntities.Count; i++)
            {
                ge = activeEntities[i];
                ge.EarlyUpdate();
                ge.Update();
                ge.LateUpdate();
            }
        }

        void Render()
        {
            Renderer.Clear(ClearBufferBit.Color | ClearBufferBit.Depth);

            GameEntity ge;
            for (int i = 0; i < activeEntities.Count; i++)
            {
                ge = activeEntities[i];
                ge.PreDraw();
                ge.Draw();
            }

            for (int i = 0; i < renderPasses.Count; i++)
                renderPassList[i].Draw();

            PresentFrame();
            for (int i = 0; i < activeEntities.Count; i++)
                activeEntities[i].PostDraw();
            
        }

        
        internal void PresentFrame()
        {
            if (renderPassList.Count == 0)
                throw new Exception("Scene does not contain any Renderpasses!");

            Framebuffer fb = renderPassList[renderPassList.Count - 1].FinalFramebuffer;

            if (fb == null)
                throw new Exception("The final Framebuffer for the final Renderpass is null!");

            /* Unbind fb.
             * Draw Color0 to the screen.
             */
            
        }

        internal void Start_Internal()
        {
            StartScene();
            isStarted = true;
        }

        internal GameEntity CreateGameEntity_Internal()
        {
            GameEntity ge = GameEntity.Create(this);
            AddEntity(ge);
            return ge;
        }

        Camera GetActiveCamera()
        {
            if (activeCamera == null)
            {
                Console.WriteLine("No camera set for this scene, creating a new one");
                GameEntity ge = GameEntity.Create(this);
                Camera c = ge.AddComponent<Camera>();
                activeCamera = c;
                activeEntities.Add(ge);
            }

            return activeCamera;
        }

        
        protected T AddRenderpass<T>() where T : Renderpass
        {
            if (renderPasses.TryGetValue(typeof(T), out Renderpass rp))
                throw new Exception($"Scene already contains a renderpass of type {rp.GetType().Name}");

            rp = Activator.CreateInstance<T>();

            renderPasses.Add(typeof(T), rp);
            rp.Index = renderPassList.Count;
            rp.Scene = this;
            renderPassList.Add(rp);
            return (T)rp;
        }
        

        internal Renderpass AddRenderpass(Type t)
        {
            if (renderPasses.TryGetValue(t, out Renderpass rp))
                throw new Exception($"Scene already contains a renderpass of type {t.Name}");
            
            rp = (Renderpass)Activator.CreateInstance(t);

            renderPasses.Add(t, rp);
            rp.Index = renderPassList.Count;
            rp.Scene = this;
            renderPassList.Add(rp);
            return rp;
        }

        
        protected T AddRenderpass<T>(int targetIndex) where T : Renderpass
        {
            T rp = AddRenderpass<T>();
            ChangeRenderpassIndex(rp, targetIndex);
            return rp;
        }
        

        public Renderpass GetRenderpass(int index)
        {
            return renderPassList[index];
        }
        
        internal Renderpass GetRenderpass(Type t)
        {
            if (!renderPasses.TryGetValue(t, out Renderpass rp))
                rp = AddRenderpass(t);
                
            return rp;
        }

        
        public void ChangeRenderpassIndex(Renderpass rp, int newIndex)
        {
            for (int i = rp.Index + 1; i <= newIndex; i++)
                renderPassList[i].Index--;

            for (int i = newIndex; i < rp.Index; i++)
                renderPassList[i].Index++;

            renderPassList.Insert(newIndex, rp);
            renderPassList.RemoveAt(rp.Index);
            rp.Index = newIndex;
        }
        

        public void RemoveRenderpass(Renderpass rp)
        {
            for (int i = rp.Index + 1; i < renderPasses.Count; i++)
                renderPassList[i].Index--;

            renderPassList.RemoveAt(rp.Index);
        }

        protected GameEntity CreateEntity() => CreateGameEntity_Internal();
        protected abstract void LoadScene();
        protected abstract void UnloadScene();
        protected abstract void StartScene();

        internal void Load_Internal() => LoadScene();
        internal void Unload_Internal() => UnloadScene();
        
    }
}
