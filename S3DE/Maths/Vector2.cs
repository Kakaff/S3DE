using System.Runtime.InteropServices;

namespace S3DE.Maths
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2
    {
        public float x { get; set; }
        public float y { get; set; }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }


        public static bool operator == (Vector2 v1, Vector2 v2)
        {
            return v1.x == v2.x && v1.y == v2.y;
        }

        public static bool operator !=(Vector2 v1, Vector2 v2) => !(v1 == v2);

        public override string ToString()
        {
            return $"({x},{y})";
        }
    }
}
