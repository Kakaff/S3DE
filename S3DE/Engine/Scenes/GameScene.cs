using S3DE.Engine.Entities;
using S3DE.Engine.Entities.Components;
using S3DE.Engine.Graphics;
using S3DE.Engine.Graphics.Lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Scenes
{
    public abstract class GameScene
    {
        List<GameEntity> activeEntities = new List<GameEntity>();
        List<GameEntity> inActiveEntities = new List<GameEntity>();

        GameEntity[] frameStageEntities;

        Camera mainCamera;
        Camera activeCamera;
        AmbientLight ambient = new AmbientLight(Color.White,0.225f);
        List<IDirectionalLight> directionalLights = new List<IDirectionalLight>();

        /// <summary>
        /// The Main camera of this scene.
        /// </summary>
        public Camera MainCamera {
            get => GetMainCamera();
            set => mainCamera = value;
        }
        /// <summary>
        /// The camera that is currently rendering.
        /// </summary>
        public Camera ActiveCamera
        {
            get => activeCamera;
            internal set => activeCamera = value;
        }

        internal GameEntity[] ActiveEntities
        {
            get
            {
                InitFrameStage();
                return frameStageEntities;
            }
        }

        public AmbientLight Ambient
        {
            get => ambient;
            set => ambient = value;
        }

        public IDirectionalLight[] DirectionalLights => directionalLights.ToArray();

        Camera GetMainCamera()
        {
            if (mainCamera == null)
                mainCamera = CreateGameEntity().AddComponent<Camera>();
            return mainCamera;
        }

        internal void AddEntity(GameEntity ge)
        {
            if (ge.IsActive)
                activeEntities.Add(ge);
            else
                inActiveEntities.Add(ge);
        }
        
        internal void RemoveEntity(GameEntity ge)
        {
            if (ge.IsActive && activeEntities.Contains(ge))
                activeEntities.Remove(ge);
            else if (inActiveEntities.Contains(ge))
                inActiveEntities.Remove(ge);
        }

        internal void InitFrameStage() => frameStageEntities = activeEntities.ToArray();

        internal void EarlyUpdate()
        {
            InitFrameStage();

            foreach (GameEntity ge in frameStageEntities)
            {
                ge.InitComponents();
                ge.StartComponents();
                ge.EarlyUpdate();
            }
        }

        internal void Update()
        {
            InitFrameStage();
            foreach (GameEntity ge in frameStageEntities)
                ge.Update();
        }

        internal void LateUpdate()
        {
            InitFrameStage();
            foreach (GameEntity ge in frameStageEntities)
                ge.LateUpdate();
        }

        /// <summary>
        /// Gets called before GameScene.Draw() and is mainly used to setup everything to be ready for drawing.
        /// </summary>
        internal void PreDraw()
        {
            InitFrameStage();
            foreach (GameEntity ge in frameStageEntities)
                ge.PreDraw();
        }
        /// <summary>
        /// Renders the scene.
        /// </summary>
        internal void Draw()
        {
            InitFrameStage();
            foreach (GameEntity ge in frameStageEntities)
                ge.Draw();
        }
        /// <summary>
        /// Is called after GameScene.Draw() and is mainly used to cleanup from drawing and to ready the next frame.
        /// </summary>
        internal void PostDraw()
        {
            InitFrameStage();
            foreach (GameEntity ge in frameStageEntities)
                ge.PostDraw();
        }
        /// <summary>
        /// Updates this GameScene 1 frame.
        /// </summary>
        /// <param name="canDraw">Specifies wether or not this scene will be drawn</param>
        internal void Run(bool canDraw)
        {
            EarlyUpdate();
            Update();
            LateUpdate();

            if (canDraw)
            {
                
                //Queue up a render call for the main camera.
                ActiveCamera = MainCamera;
                PreDraw();
                RenderPipeline.RenderScene_Internal(this, Renderer.MainRenderCall);
                //Draw ScreenQuad
                //Draw Ui
                //Loop through all queued rendercalls. Then perform the scenes main render call.
                //Then draw the main render call to a quad and present it on screen.
                //The draw method is called for each rendercall. (it can in other words be called several times per frame).
                //So when the rendercall looks at an entity it calls the GameEntity.Draw();
                PostDraw();
                
                
            }
            
        }

        internal void ReloadScene()
        {
            //Dispose the scene. (call Dispose() and dispose all entities and their components).
            //Load the scene.
        }

        protected abstract void LoadScene();
        protected abstract void UnloadScene();

        internal GameEntity CreateEntityInternal() => CreateGameEntity();

        protected GameEntity CreateGameEntity() {
            GameEntity ge = GameEntity.Create(this);
            AddEntity(ge);
            return ge;
        }

        internal void Load_Internal() => LoadScene();
        internal void Unload_Internal() => UnloadScene();

        public void AddDirectionalLight(IDirectionalLight dirLight) => directionalLights.Add(dirLight);
        public void RemoveDirectionalLight(IDirectionalLight dirLight) => directionalLights.Remove(dirLight);
    }
}
