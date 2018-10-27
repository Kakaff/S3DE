using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace S3DE
{
    partial class EngineMain
    {
        [DllImport("S3DECore.dll")]
        private static extern void RunEngine();

        [DllImport("S3DECore.dll")]
        private static extern void RegisterUpdateFunc(OnFrameUpdate func);
    }
}
