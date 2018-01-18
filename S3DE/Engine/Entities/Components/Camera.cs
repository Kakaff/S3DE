using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Entities.Components
{
    public sealed class Camera : EntityComponent
    {
        public static Camera MainCamera => null;
        public static Camera ActiveCamera => null;

        public Matrix4x4 ViewMatrix => null;

        public Matrix4x4 ProjectionMatrix => null;

        protected override void OnCreation()
        {
            throw new NotImplementedException();
        }
    }
}
