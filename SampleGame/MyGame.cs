using S3DE;
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
            SetEngineClock<S3DE.Engine.StopwatchClock>();
            SetTargetRenderer<Sample_OGL_Renderer.OpenGL_Renderer>();
            SetStartScene<SampleScene>();
        }

        protected override void OnGameExit()
        {
            
        }
    }
}
