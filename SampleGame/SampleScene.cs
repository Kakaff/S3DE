using S3DE.Engine.Entities;
using S3DE.Engine.Graphics;
using S3DE.Engine.Scenes;
using S3DE.Maths;
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
            Mesh m = new Mesh();
            m.Vertices = new Vector3[] {new Vector3(0, 0, 0), new Vector3(1f, 0, 0), new Vector3(0, 1f, 0)};
            m.Triangles = new int[] {0, 1, 2};
            mr.mesh = m;
            
            Material mat = new SimpleMaterial();
            mr.material = mat;
        }

        protected override void UnloadScene()
        {
            throw new NotImplementedException();
        }
    }
}
