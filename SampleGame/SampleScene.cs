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

        const int s = 120;
        const int p = 8;
        const TestCase test = TestCase.Arms;

        protected override void LoadScene()
        {
            
        }

        protected override void StartScene()
        {
            
            Mesh m = StandardMesh.CreateCube(new Vector3(1, 1, 1));
            NewSimpleMaterial mat = new NewSimpleMaterial();
            
            ActiveCamera.FoV = 80;
            ActiveCamera.Entity.AddComponent<SimpleCameraController>();
            ActiveCamera.transform.Position = new Vector3(0, 0, 5);

            CreateSpinningArmNewMR(120, GetPointsInCircle(8), new Material[] { mat }, m);
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
