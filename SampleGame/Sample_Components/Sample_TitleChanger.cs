using S3DE.Engine;
using S3DE.Engine.Entities;
using S3DE.Engine.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGame.Sample_Components
{
    public class Sample_TitleChanger : EntityComponent, IUpdateLogic
    {
        long et = 0;
        int frames = 0;

        public void EarlyUpdate() { }
        public void LateUpdate() { }

        public void Update()
        {
            et += Time.DeltaTicks;
            frames++;

            if (et >= TimeSpan.TicksPerSecond)
            {
                MyGame.WindowTitle = $"{MyGame.GameTitle} | FPS:{frames}";
                et -= TimeSpan.TicksPerSecond;
                frames = 0;
            }
        }
    }
}
