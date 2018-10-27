using S3DE;
using S3DE.Components;
using S3DE.Entities;
using S3DE.Graphics;
using S3DE.Graphics.Textures;
using S3DE.Scenes;
using SampleGame.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SampleGame
{
    class SampleScene : GameScene
    {
        const int s = 30;

        protected override void LoadScene()
        {
            
        }

        protected override void StartScene()
        {

            Texture2D tex = new Texture2D(8, 8, ColorFormat.RGBA);
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                {
                    tex.SetPixel(x, y, new Color(255, 0, 0));
                }

            tex.Apply();
            Mesh m = new Mesh();
            m.SetVertexAttribute(0, 3, GLType.FLOAT, false, 20, 0);
            m.SetVertexAttribute(1, 2, GLType.FLOAT, false, 20, 12);
            m.EnableVertexAttribute(0);
            m.EnableVertexAttribute(1);

            m.Vertices = new Vector3[] {new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f),new Vector3(0.5f,-0.5f,-0.5f),
                                        new Vector3(-0.5f,0.5f,-0.5f),new Vector3(-0.5f,0.5f,0.5f),new Vector3(0.5f,0.5f,0.5f),new Vector3(0.5f,0.5f,-0.5f),
                                        new Vector3(0.5f,-0.5f,-0.5f),new Vector3(0.5f,0.5f,-0.5f),new Vector3(0.5f,0.5f,0.5f),new Vector3(0.5f,-0.5f,0.5f),
                                        new Vector3(-0.5f,-0.5f,0.5f),new Vector3(-0.5f,0.5f,0.5f),new Vector3(-0.5f,0.5f,-0.5f),new Vector3(-0.5f,-0.5f,-0.5f),
                                        new Vector3(0.5f,-0.5f,0.5f),new Vector3(0.5f,0.5f,0.5f),new Vector3(-0.5f,0.5f,0.5f),new Vector3(-0.5f,-0.5f,0.5f),
                                        new Vector3(-0.5f,-0.5f,0.5f),new Vector3(-0.5f,-0.5f,-0.5f),new Vector3(0.5f,-0.5f,-0.5f),new Vector3(0.5f,-0.5f,0.5f)};
            
            m.UVs = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
                                    new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
                                    new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
                                    new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
                                    new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
                                    new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0)};
                                    

            m.Indicies = new ushort[] { 0,1,2,2,3,0,
                                        4,5,6,6,7,4,
                                        8,9,10,10,11,8,
                                        12,13,14,14,15,12,
                                        16,17,18,18,19,16,
                                        20,21,22,22,23,20};
            m.Apply();
            SimpleMat mat = new SimpleMat();
            mat.Texture = tex;

            for (int x = -s; x <= s; x++)
            {
                for (int z = -s; z <= s; z++)
                {
                    GameEntity ge = CreateEntity();
                    MeshRenderer mr = ge.AddComponent<MeshRenderer>();
                    mr.Mesh = m;
                    mr.Material = mat;

                    ge.transform.Position = new Vector3(x * 2, 0, z * 2);
                }
            }
            
            Camera c = ActiveCamera;

            c.transform.Position = new Vector3(0, 0.5f, -1);
            c.Entity.AddComponent<SimpleCameraController>();
            c.FoV = 85f;
            c.Entity.AddComponent<Debug_FrameMonitor>();
        }

        protected override void UnloadScene()
        {
            
        }
    }
}
