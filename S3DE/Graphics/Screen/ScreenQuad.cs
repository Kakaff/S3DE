using S3DE.Graphics.Meshes;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Screen
{
    /// <summary>
    /// A simple quad that covers the entire screen from -1,-1 to 1,1
    /// </summary>
    public class ScreenQuad
    {
        static ScreenQuad instance;
        StandardMesh mesh;

        static ScreenQuad Instance { get { if (instance == null) instance = new ScreenQuad(); return instance; } }

        private ScreenQuad()
        {
            mesh = new StandardMesh();
            mesh.Vertices = new Vector3[] { new Vector3(-1, -1), new Vector3(-1, 1), new Vector3(1, 1), new Vector3(1, -1) };
            mesh.Uvs = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
            mesh.Indicies = new ushort[] { 3, 2, 1, 1, 0, 3 };
            mesh.Apply();
        }

        /// <summary>
        /// Draws the ScreenQuad onto the currently active FBO
        /// </summary>
        /// <param name="mat"></param>
        public static void Draw(Material mat)
        {
            mat.UpdateUniforms_Internal();
            
            Instance.mesh.Draw();
        }

        
    }
}
