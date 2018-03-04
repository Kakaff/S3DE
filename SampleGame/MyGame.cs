using S3DE;
using S3DE.Engine;
using S3DE.Engine.Graphics;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGame
{
    public class MyGame : Game
    {

        public static void Main(String[] args)
        {
            new MyGame().Run();
        }

        public override string GameName()
        {
            return "Sample Game";
        }

        protected override void Initialize()
        {
            SetEngineClock<HighResClock>();
            SetEngineFrameSync<SampleFrameSync>();
            SetTargetRenderer<Sample_OGL_Renderer.OpenGL_Renderer>();
            SetTargetRenderPipleline<S3DE.Engine.Graphics.OpenGL.StandardPipeline>();

            SetTargetFPS(60);
        }

        protected override void Start()
        {
            DisplayResolution = new Vector2(1280, 720);
            RenderResolution = new Vector2(1280, 720);
            RefreshRate = 60;
            Input.LockCursor();
            SetStartScene<SampleScene>();
        }

        protected override void OnGameExit()
        {
            
        }
    }
}
