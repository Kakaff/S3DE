﻿using S3DE.Components;
using S3DE.Entities;
using S3DE.Graphics;
using S3DE.Graphics.Materials;
using S3DE.Graphics.Meshes;
using S3DE.Graphics.Rendering.RenderPasses;
using S3DE.Graphics.Textures;
using S3DE.Maths;
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

        const int s = 500;
        const int p = 8;
        const TestCase test = TestCase.Arms;

        protected override void LoadScene()
        {
            
        }

        protected override void StartScene()
        {
            AddRenderPass(new DiffuseRenderPass(),0);


            int w = 16, h = 16;
            Console.WriteLine("Creating texture 1");
            Texture2D tex = new Texture2D(w, h, ColorFormat.RGB,InternalFormat.RGBA);
            tex.FilterMode = FilterMode.TriLinear;
            tex.WrapMode = WrapMode.Clamp;
            Console.WriteLine("Creating texture 2");
            Texture2D tex2 = new Texture2D(tex);

            float mul = 0;
            float xMod = 0, yMod = 0;

            float wMod = (w - 1) / 2f;
            float hMod = (h - 1) / 2f;
            float half = wMod * wMod + hMod * hMod;
            byte colVal = 0;

            for (int x = 0; x < w; x++)
            {
                xMod = (-w+1) / 2f + x;
                for (int y = 0; y < h; y++)
                {
                    yMod = (-h+1) / 2f + y;
                    mul = (float)Math.Pow((xMod * xMod + yMod * yMod) / half,2);
                    colVal = (byte)(255 * mul);
                    tex[x, y] = new Color(0, 0, colVal);
                    tex2[x,y] = new Color(0, colVal, colVal);
                }
            }

            
            tex.Apply();
            tex2.Apply();

            Mesh m = StandardMesh.CreateCube(new Vector3(1, 1, 1));
            
            
           if (test == TestCase.Arms){
                NewTexturedMaterial ntm = new NewTexturedMaterial();
                NewTexturedMaterial ntm2 = new NewTexturedMaterial();
                ntm.Texture = tex;
                ntm2.Texture = tex2;

                Material[] mats = {
                    new NewSimpleMaterial(),ntm,ntm2
                };
                CreateSpinningArmNewMR(s,
                    GetPointsInCircle(p),
                    mats, m);
            }

            Camera c = ActiveCamera;

            c.transform.Position = new Vector3(0, 20f, 0f);
            c.transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Right, -90f);
            c.Entity.AddComponent<SimpleCameraController>();
            c.FoV = 95f;
            //c.Entity.AddComponent<Debug_FrameMonitor>();
        }

        protected override void UnloadScene()
        {
            
        }


        Vector3[] GetPointsInCircle(int points)
        {
            Vector3[] res = new Vector3[points];
            float rotMod = 360f / (float)points;

            for (int i = 0; i < points; i++)
                res[i] = Quaternion.CreateFromAxisAngle(Vector3.Up, rotMod * i).Transform(Vector3.Forward);

            return res;
        }

        void CreateSpinningArmNewMR(int length, Vector3[] armDirections, Material[] mats,Mesh m)
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

                matCntr = (int)EngineMath.Normalize(0, mats.Length, matCntr + 1);
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
