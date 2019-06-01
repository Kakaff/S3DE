using S3DE.Entities;
using S3DE.Components;
using System;
using System.Collections.Generic;
using S3DE.Graphics.FrameBuffers;
using S3DE.Graphics;
using S3DE.Graphics.Screen;
using S3DE.Graphics.Rendering;
using S3DE.Graphics.Textures;

namespace S3DE.Scenes
{
    public abstract class GameScene
    {
        List<GameEntity> activeEntities = new List<GameEntity>();
        List<GameEntity> inActiveEntities = new List<GameEntity>();
        
        List<GameEntity> entitiesToAdd = new List<GameEntity>();

        List<RenderPass> renderpasses = new List<RenderPass>();
        
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
            GameEntity ge;
            for (int i = 0; i < activeEntities.Count; i++)
            {
                ge = activeEntities[i];
                ge.PreDraw();
                ge.Draw();
            }

            for (int i = 0; i < renderpasses.Count; i++)
            {
                RenderPass rp = renderpasses[i];
                rp.BindFrameBuffer();
                if (rp.ClearBeforeRendering)
                    rp.ClearBuffers();

                renderpasses[i].Render();
            }

            for (int i = 0; i < activeEntities.Count; i++)
                activeEntities[i].PostDraw();
        }

        
        internal void PresentFrame()
        {
            FrameBuffer fb = renderpasses[renderpasses.Count - 1].GetFrameBuffer();
            fb.Unbind();

            Renderer.Clear(ClearBufferBit.COLOR | ClearBufferBit.DEPTH);
            Renderer.Disable(GlEnableCap.DepthTest);

            DefaultScreenQuadMaterial.Instance.Tex = 
                fb.GetAttachment(FrameBufferAttachmentLocation.COLOR0).InternalTexture as RenderTexture2D;

            ScreenQuad.Draw(DefaultScreenQuadMaterial.Instance);
            
            Renderer.Enable(GlEnableCap.DepthTest);
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
                GameEntity ge = GameEntity.Create(this);
                Camera c = ge.AddComponent<Camera>();
                activeCamera = c;
                activeEntities.Add(ge);
                Console.WriteLine("No camera set for this scene, creating a new one");
            }

            return activeCamera;
        }

        protected void AddRenderPass(RenderPass rp, int targetIndex)
        {
            if (targetIndex > renderpasses.Count)
                renderpasses.Add(rp);
            else
                renderpasses.Insert(targetIndex, rp);
        }

        public RenderPass GetRenderPass(int identifier)
        {
            return renderpasses.Find(x => x.Identifier == identifier);
        }

        /// <summary>
        /// Creates a Deferred, A Forward and a Blend renderpass.
        /// </summary>
        protected void CreateStandardRenderPasses()
        {
            throw new NotImplementedException();
            /*
            renderpasses.Add(new DeferredRenderPass(RenderPassType.Deferred),
                new ForwardRenderPass(RenderPassType.Forward),
                new BlendRenderPass(RenderPassType.Blend));
                */
        }

        protected GameEntity CreateEntity() => CreateGameEntity_Internal();
        protected abstract void LoadScene();
        protected abstract void UnloadScene();
        protected abstract void StartScene();

        internal void Load_Internal() => LoadScene();
        internal void Unload_Internal() => UnloadScene();
    }
}
