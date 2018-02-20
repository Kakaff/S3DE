using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Maths
{
    public struct Vector4
    {
        float _x, _y, _z, _w;

        public float x
        {
            get => _x;
            set => _x = value;
        }

        public float y
        {
            get => _y;
            set => _y = value;
        }

        public float z
        {
            get => _z;
            set => _z = value;
        }

        public float w
        {
            get => _w;
            set => _w = value;
        }

        public Vector3 xyz => new Vector3(_x, _y, _z);

        public Vector4(float x, float y, float z, float w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        public static implicit operator Vector4(Vector3 v) => new Vector4(v.x,v.y,v.z,0);

        public float[] ToArray() => new float[] { _x, _y, _z, _w };
    }
}
