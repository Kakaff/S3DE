﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace S3DE
{
    static partial class Window
    {

        [DllImport("S3DECore.dll")]
        private static extern void InitGLFW();

        [DllImport("S3DECore.dll")]
        private static extern void CreateWindow();
        
        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetWindowHint(int hint, int value);

        [DllImport("S3DECore.dll")]
        private static extern void Extern_SetWindowSize(int width, int height);

        [DllImport("S3DECore.dll")]
        private static extern int Extern_GetAttribute(int attribute);
    }
}
