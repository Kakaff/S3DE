using S3DE.Entities;
using S3DE.Components;
using System;
using System.Collections.Generic;
using S3DE.Graphics.FrameBuffers;
using S3DE.Graphics;
using S3DE.Graphics.Screen;

namespace S3DE.Scenes
{
    public abstract class GameScene
    {
        List<GameEntity> activeEntities = new List<GameEntity>();
        List<GameEntity> inActiveEntities = new List<GameEntity>();
        
        List<GameEntity> entitiesToAdd = new List<GameEntity>();
        
        FrameBuffer sceneFBO;
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
            sceneFBO.Clear(ClearBufferBit.COLOR | ClearBufferBit.DEPTH);

            GameEntity ge;
            for (int i = 0; i < activeEntities.Count; i++)
            {
                ge = activeEntities[i];
                ge.PreDraw();
                ge.Draw();
                ge.PostDraw();
            }
        }

        /// <summary>
        /// Draw the SceneFBO onto the window.
        /// </summary>
        internal void PresentFrame()
        {
            sceneFBO.Unbind();

            Renderer.Clear(ClearBufferBit.COLOR | ClearBufferBit.DEPTH);
            Renderer.Enable_DepthTest(false);

            DefaultScreenQuadMaterial.Instance.Tex = 
                sceneFBO.GetAttachment(FrameBufferAttachmentLocation.COLOR0).InternalTexture;

            ScreenQuad.Draw(DefaultScreenQuadMaterial.Instance);

            Renderer.Enable_DepthTest(true);
        }

        internal void Start_Internal()
        {
            StartScene();
            if (sceneFBO == null)
            {
                Console.WriteLine("Creating new Scene FBO");
                sceneFBO = FrameBuffer.Create_Standard_FrameBuffer(Game.RenderResolution);
            }
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
                GameEntity ge = GameEntity.Create(this);
                Camera c = ge.AddComponent<Camera>();
                activeCamera = c;
                activeEntities.Add(ge);
                Console.WriteLine("No camera set for this scene, creating a new one");
            }

            return activeCamera;
        }

        protected GameEntity CreateEntity() => CreateGameEntity_Internal();
        protected abstract void LoadScene();
        protected abstract void UnloadScene();
        protected abstract void StartScene();

        internal void Load_Internal() => LoadScene();
        internal void Unload_Internal() => UnloadScene();
    }
}
