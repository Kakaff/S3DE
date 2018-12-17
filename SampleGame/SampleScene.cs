using S3DE.Components;
using S3DE.Entities;
using S3DE.Graphics;
using S3DE.Graphics.Textures;
using S3DE.Maths;
using S3DE.Scenes;
using SampleGame.Debug;
using System;

namespace SampleGame
{
    class SampleScene : GameScene
    {
        const int s = 15;
        
        protected override void LoadScene()
        {
            
        }

        protected override void StartScene()
        {
            int w = 16, h = 16;

            Console.WriteLine("Creating texture 1");
            Texture2D tex = new Texture2D(w, h, ColorFormat.RGBA,InternalFormat.RGBA16F);
            Console.WriteLine("Creating texture 2");
            Texture2D tex2 = new Texture2D(w, h, ColorFormat.RGBA, InternalFormat.RGBA16F);

            float mul = 0;
            float xMod = 0, yMod = 0;

            float wMod = (w - 1) / 2f;
            float hMod = (h - 1) / 2f;
            float half = wMod * wMod + hMod * hMod;
            for (int x = 0; x < w; x++)
            {
                xMod = (-w+1) / 2f + x;
                for (int y = 0; y < h; y++)
                {
                    yMod = (-h+1) / 2f + y;
                    mul = (float)Math.Pow((xMod * xMod + yMod * yMod) / half,2);
                    tex[x, y] = new Color(0, 0, (byte)(255 * mul));
                    tex2[x,y] = new Color((byte)(255 * mul), (byte)(255 * mul), 0);
                }
            }

            tex.FilterMode = FilterMode.Nearest;
            tex.WrapMode = WrapMode.Clamp;
            tex2.FilterMode = FilterMode.Nearest;
            tex2.WrapMode = WrapMode.Clamp;
            tex.Apply();
            tex2.Apply();
            
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
            SimpleMat mat2 = new SimpleMat();
            mat.Texture = tex;
            mat2.Texture = tex2;

            CreateSquareOfEntities_2Mat(s, mat,mat2,m);

            
            Camera c = ActiveCamera;

            c.transform.Position = new Vector3(0, 0f, 0f);
            
            c.Entity.AddComponent<SimpleCameraController>();
            c.FoV = 95f;
            //c.Entity.AddComponent<Debug_FrameMonitor>();
        }

        protected override void UnloadScene()
        {
            
        }

        void CreateSquareOfEntities_2Mat(float size, Material mat, Material mat2, Mesh m)
        {
            for (int x = -s; x <= s; x++)
                for (int z = -s; z <= s; z++)
                {
                    GameEntity ge = CreateEntity();
                    ge.AddComponent<ObjectRotator>()
                        .Entity.AddComponent<MeshRenderer>()
                        .SetMaterial((z % 2) * (x % 2) != 0 ? mat : mat2)
                        .SetMesh(m);

                    ge.transform.Position = new Vector3(x * 2, 0, z * 2);
                }
        }

        void CreateSquareOfEntities(float size,Material mat,Mesh m)
        {
            for (int x = -s; x <= s; x++)
                for (int z = -s; z <= s; z++)
                {
                    GameEntity ge = CreateEntity();
                    ge.AddComponent<ObjectRotator>().Entity.AddComponent<MeshRenderer>().SetMaterial(mat).SetMesh(m);
                    ge.transform.Position = new Vector3(x * 2, 0, z * 2);
                }
        }
    }
}
