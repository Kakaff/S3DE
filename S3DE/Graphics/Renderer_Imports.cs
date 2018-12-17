using System.Runtime.InteropServices;

namespace S3DE.Graphics
{
    static partial class Renderer
    {
        [DllImport("S3DECore.dll")]
        private static extern void InitGlew();
    }
}
