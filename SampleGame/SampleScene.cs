using S3DE.Engine.Entities;
using S3DE.Engine.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGame
{
    public class SampleScene : GameScene
    {
        protected override void LoadScene()
        {
            GameEntity ge = CreateGameEntity();

            MeshRenderer mr = ge.AddComponent<MeshRenderer>();
        }

        protected override void UnloadScene()
        {
            throw new NotImplementedException();
        }
    }
}
