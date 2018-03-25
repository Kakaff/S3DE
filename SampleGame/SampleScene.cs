﻿using S3DE;
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
            mr.Material = mat;
            
            MainCamera.transform.Position = new Vector3(0,0,-1);
            
            GameEntity ge2 = CreateGameEntity();
            MeshRenderer mr2 = ge2.AddComponent<MeshRenderer>();
            mr2.mesh = Mesh.CreateCube(new Vector3(1, 1, 1));
            mr2.Material = mr.Material;
            ge2.transform.SetParent(ge.transform);
            ge2.transform.SetPosition(new Vector3(0, 1, 2f), Enums.Space.World);

            GameEntity ge3 = CreateGameEntity();
            MeshRenderer mr3 = ge3.AddComponent<MeshRenderer>();
            mr3.mesh = Mesh.CreateCube(new Vector3(1, 1, 1));
            mr3.Material = mr.Material;
            ge3.transform.Position = new Vector3(3, 0, 3);

            Sample_LookAt la = ge2.AddComponent<Sample_LookAt>();
            la.Target = ge3.transform.Position;

            GameEntity sun = CreateGameEntity();
            DirectionalLight sunLight1 = sun.AddComponent<DirectionalLight>();

            sunLight1.LightDirection = new Vector3(1, -1, 0).normalized;
            
            MainCamera.gameEntity.AddComponent<Sample_CameraController>();
            //MainCamera.gameEntity.AddComponent<Sample_FrameMonitor>();

            
        }

        protected override void UnloadScene()
        {
            throw new NotImplementedException();
        }
    }
}
