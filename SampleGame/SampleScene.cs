using S3DE.Components;
using S3DE.Entities;
using S3DE.Graphics.Materials;
using S3DECore.Graphics.Meshes;
using S3DECore.Graphics.Textures;
using S3DECore.Graphics;
using S3DECore.Math;
using S3DE.Scenes;
using SampleGame.Debug;
using System;

namespace SampleGame
{
    class SampleScene : GameScene
    {
        enum TestCase
        {
            Square,
            Square_NoRender,
            Arms,
        }

        const int s = 75;
        const int p = 4;
        const TestCase test = TestCase.Arms;

        protected override void LoadScene()
        {
            
        }

        protected override void StartScene()
        {
            
            Mesh m = StandardMesh.CreateCube(new Vector3(1, 1, 1));
            NewSimpleMaterial mat = new NewSimpleMaterial();
            NewTexturedMaterial mat1 = new NewTexturedMaterial();

            Texture2D tex = new Texture2D(32, 32);
            tex.AutoGenerateMipMaps = true;
            tex.Anisotropic = AnisotropicSamples.x16;
            tex.Filter = FilterMode.TriLinear;

            float xMod = 1 / (float)tex.GetWidth();
            float yMod = 1 / (float)tex.GetHeight();
            float cMod = 1 / (new Vector2(tex.GetWidth(),tex.GetHeight()).LengthSquared());

            for (int x = 0; x < tex.GetWidth(); x++)
                for (int y = 0; y < tex.GetHeight(); y++)
                {
                    tex[x, y] = new Color(
                        (byte)(255 * (xMod * x)), 
                        (byte)(255 * (cMod * new Vector2(x,y).LengthSquared())), 
                        (byte)(255 * (yMod * y)), 255);
                }
            

            tex.Apply();
            mat1.Texture = tex;
            ActiveCamera.FoV = 80;
            ActiveCamera.Entity.AddComponent<SimpleCameraController>();
            ActiveCamera.transform.Position = new Vector3(0, 0, 5);

            CreateSpinningArmNewMR(s, GetPointsInCircle(p), new Material[] { mat,mat1 }, m);
        }

        protected override void UnloadScene()
        {
            
        }


        Vector3[] GetPointsInCircle(int points)
        {
            Vector3[] res = new Vector3[points];
            float rotMod = 360f / (float)points;

            for (int i = 0; i < points; i++)
                res[i] = Vector3.Forward.Transform(Quaternion.CreateFromAxisAngle(Vector3.Up, rotMod * i));

            return res;
        }

        void CreateSpinningArmNewMR(int length, Vector3[] armDirections, Material[] mats, Mesh m)
        {
            int matCntr = 0;
            GameEntity[] prevEnts = new GameEntity[armDirections.Length];
            prevEnts[0] = CreateEntity();
            Meshrenderer nmr = prevEnts[0].AddComponent<ObjectRotator>().
                Entity.AddComponent<Meshrenderer>();

            nmr.Material = mats[0];
            nmr.Mesh = m;

            double scaleMod = 1f / (length - 1);
            for (int i = 1; i < armDirections.Length; i++)
                prevEnts[i] = prevEnts[0];

            for (int i = 1; i < length; i++)
            {

                matCntr = (int)MathFun.Normalize(0, mats.Length, matCntr + 1);
                float scale = (float)((double)(length - (i - 1)) * scaleMod);
                float posMod = 2 * i * (1 + (0.05f * i));

                for (int j = 0; j < armDirections.Length; j++)
                {
                    GameEntity ge = CreateEntity();

                    Meshrenderer cnmr = ge.AddComponent<Meshrenderer>();
                    cnmr.Mesh = m;
                    cnmr.Material = mats[matCntr];
                    ge.AddComponent<ObjectRotator>();

                    Vector3 pos = armDirections[j] * posMod;
                    ge.transform.Position = armDirections[j] * posMod;
                    ge.transform.SetParent(prevEnts[j].transform);
                    ge.transform.SetScale(new Vector3(scale), S3DE.Space.World);
                    prevEnts[j] = ge;
                }
            }
        }
    }
}
