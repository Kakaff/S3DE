using S3DE.Engine.Entities;
using S3DE.Engine.Entities.Components;
using S3DE.Engine.Graphics;
using S3DE.Engine.Scenes;
using S3DE.Maths;
using SampleGame.Sample_Components;
using SampleGame.Sample_OGL_Renderer.Shaders;
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
            mr.mesh = Mesh.CreateCube(new Vector3(1,1,1));
            ge.transform.Position = new Vector3(0f, 0f, 2f);
            ge.AddComponent<Sample_ObjectRotator>();
            Material mat = new SimpleMaterial();
            mr.material = mat;
            Camera.MainCamera.gameEntity.transform.Position = new Vector3(0, 0, 0f);

        }

        protected override void UnloadScene()
        {
            throw new NotImplementedException();
        }
    }
}
