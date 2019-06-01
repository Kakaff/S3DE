﻿using System;
using System.Runtime.InteropServices;

namespace S3DE.Maths
{
    [StructLayout(LayoutKind.Sequential, Pack = 16)]
    public struct Quaternion
    {
        public float x, y, z, w;
        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static Quaternion Identity => new Quaternion(0, 0, 0, 1);

        public Quaternion Conjugate() =>
            new Quaternion(-x, -y, -z, w);

        public Quaternion Normalized()
        {
            double l = Math.Sqrt(x * 2 + y * 2 + z * 2 + w * 2);
            return new Quaternion((float)(x / l), (float)(y / l), (float)(z / l), (float)(w / l));
        }

        public static Quaternion Inverse(Quaternion q)
        {
            Quaternion r;

            float ls = q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w;
            float invNorm = 1.0f / ls;

            r.x = -q.x * invNorm;
            r.y = -q.y * invNorm;
            r.z = -q.z * invNorm;
            r.w = q.w * invNorm;

            return r;
        }

        public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle)
        {
            Quaternion r;
            angle =  (float)(angle * Constants.ToRadians);
            float halfAngle = angle * 0.5f;
            float s = (float)Math.Sin(halfAngle);
            float c = (float)Math.Cos(halfAngle);

            r.x = axis.x * s;
            r.y = axis.y * s;
            r.z = axis.z * s;
            r.w = c;

            return r;
        }

        public Vector3 Transform(Vector3 v) => Transform(v, this);

        public static Vector3 Transform(Vector3 v, Quaternion q)
        {
            Quaternion res = (q * new Quaternion(v.x, v.y, v.z, 0)) * q.Conjugate();

            return new Vector3(res.x, res.y,res.z);
                
        }

        public static Quaternion operator * (Quaternion q1, Quaternion q2)
        {
            Quaternion ans;

            float q1x = q1.x;
            float q1y = q1.y;
            float q1z = q1.z;
            float q1w = q1.w;

            float q2x = q2.x;
            float q2y = q2.y;
            float q2z = q2.z;
            float q2w = q2.w;

            // cross(av, bv)
            float dot = q1x * q2x + q1y * q2y + q1z * q2z;
            
            float cx = q1y * q2z - q1z * q2y;
            float cy = q1z * q2x - q1x * q2z;
            float cz = q1x * q2y - q1y * q2x;

            ans.x = q1x * q2w + q2x * q1w + cx;
            ans.y = q1y * q2w + q2y * q1w + cy;
            ans.z = q1z * q2w + q2z * q1w + cz;
            ans.w = q1w * q2w + 0   * 1   - dot;
            
            return ans;
            
        }

        public Matrix4x4 ToRotationMatrix() => Matrix4x4.CreateRotationMatrix(this);

        public override string ToString()
        {
            return $"({x},{y},{z},{w})";
        }

    }
}
