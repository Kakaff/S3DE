using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics
{
    static partial class Renderer
    {
        [DllImport("S3DECore.dll")]
        private static extern void InitGlew();
    }
}
