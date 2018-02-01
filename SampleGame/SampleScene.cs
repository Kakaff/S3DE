using S3DE.Engine;
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
            ge.transform.Position = new Vector3(0f,0,0f);
            ge.AddComponent<Sample_ObjectRotator>();
            Material mat = new SimpleMaterial();
            mr.material = mat;
            Camera.MainCamera.transform.Position = new Vector3(0,3f,-4);

            Camera.MainCamera.transform.LookAt(ge.transform.Position);

            GameEntity ge2 = CreateGameEntity();
            MeshRenderer mr2 = ge2.AddComponent<MeshRenderer>();
            mr2.mesh = Mesh.CreateCube(new Vector3(1, 1, 1));
            mr2.material = new SimpleMaterial();
            ge2.transform.SetParent(ge.transform);
            ge2.transform.SetPosition(new Vector3(0, 0, 2f), S3DE.Engine.Enums.Space.World);

            GameEntity ge3 = CreateGameEntity();
            MeshRenderer mr3 = ge3.AddComponent<MeshRenderer>();
            mr3.mesh = Mesh.CreateCube(new Vector3(1, 1, 1));
            mr3.material = new SimpleMaterial();
            ge3.transform.Position = new Vector3(3, 0, 3);

            Sample_LookAt la = ge2.AddComponent<Sample_LookAt>();
            la.Target = ge3.transform.Position;

            Camera.MainCamera.gameEntity.AddComponent<Sample_CameraController>();
            
        }

        protected override void UnloadScene()
        {
            throw new NotImplementedException();
        }
    }
}
