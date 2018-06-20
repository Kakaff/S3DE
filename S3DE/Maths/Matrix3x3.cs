using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Maths
{
    public class Matrix3x3
    {
        float[][] mv;

        public float this[int x, int y] {
            get => mv[x][y];
            set => mv[x][y] = value;
        }

        public Matrix3x3()
        {
            mv = new float[3][];
            SetIdentity();
        }

        public void SetIdentity()
        {
            mv[0] = new float[] { 1, 0, 0 };
            mv[1] = new float[] { 0, 1, 0 };
            mv[2] = new float[] { 0, 0, 1 };
        }

        public static Matrix3x3 Identity => new Matrix3x3();

        public float[] ToArray()
        {
            float[] res = new float[9];
            res[0] = this[0, 0];
            res[1] = this[1, 0];
            res[2] = this[2, 0];

            res[3] = this[0, 1];
            res[4] = this[1, 1];
            res[5] = this[2, 1];

            res[6] = this[0, 2];
            res[7] = this[1, 2];
            res[8] = this[2, 2];

            return res;
        }
    }
}
