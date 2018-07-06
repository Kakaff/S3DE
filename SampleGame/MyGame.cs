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
            SetTargetRenderer<S3DE.Engine.Graphics.OpGL.OpenGL_Renderer>();
            SetTargetRenderPipleline<S3DE.Engine.Graphics.OpGL.OpenGL_StandardPipeline>();
            
            SetTargetFPS(60);
        }

        protected override void Start()
        {
            DisplayResolution = new S3DE_Vector2(1600, 900);
            RenderResolution = new S3DE_Vector2(800, 450);
            RefreshRate = 60;
            SetTargetFPS(60);
            SetVSync(false);
            Input.LockCursor();
            SetStartScene<SampleScene>();
        }

        protected override void OnGameExit()
        {
            
        }
    }
}
