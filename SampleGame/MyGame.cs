using S3DE;
using S3DE.Engine;
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
            SetTargetFPS(60);
            SetStartScene<SampleScene>();
            Input.HideCursor();
            Resolution = new Vector2(1280, 720);

        }

        protected override void OnGameExit()
        {
            
        }

        protected override void SetClock()
        {
            SetEngineClock<S3DE.Engine.HighResClock>();
        }

        protected override void SetFrameSync()
        {
            SetEngineFrameSync<SampleFrameSync>();
        }

        protected override void SetRenderer()
        {
            SetTargetRenderer<Sample_OGL_Renderer.OpenGL_Renderer>();
        }
    }
}
