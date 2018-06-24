using S3DE;
using S3DE.Engine;
using S3DE.Engine.Entities;
using S3DE.Engine.Entities.Components;
using S3DE.Engine.Graphics;
using S3DE.Engine.Graphics.Materials;
using S3DE.Engine.Scenes;
using S3DE.Maths;
using SampleGame.Sample_Components;
using S3DE.Engine.Graphics.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S3DE.Utility;

namespace SampleGame
{
    public class SampleScene : GameScene
    {
        protected override void LoadScene()
        {
            
            Material mat = new SimpleMaterial_UBO();
            Mesh m = Mesh.CreateCube(new System.Numerics.Vector3(1, 1, 1));
            
            const int s = 10;
            Console.WriteLine($"Testing {(s*2) * (s*2)} rotating cubes");

            

            for (int x = -s; x < s; x++)
                for (int y = -s; y < s; y++)
                {
                    GameEntity ge = CreateGameEntity();
                    ge.transform.Position = new System.Numerics.Vector3(x * 2, 0, y * 2);
                    MeshRenderer mr = ge.AddComponent<MeshRenderer>();
                    ge.AddComponent<Sample_ObjectRotator>();
                    mr.mesh = m;
                    mr.Material = mat;
                }
            
            
            GameEntity sun = CreateGameEntity();
            DirectionalLight sunLight1 = sun.AddComponent<DirectionalLight>();
            sun.AddComponent<Sample_TitleChanger>();
            sunLight1.LightDirection = new System.Numerics.Vector3(1, -1, 0).Normalized();
            
            MainCamera.gameEntity.AddComponent<Sample_CameraController>();
            //MainCamera.gameEntity.AddComponent<Sample_FrameMonitor>();


        }

        protected override void UnloadScene()
        {
            throw new NotImplementedException();
        }
    }
}
