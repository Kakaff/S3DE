﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE
{
    public enum WindowHint
    {
        
        RED_BITS,
        GREEN_BITS,
        BLUE_BITS,
        ALPHA_BITS,
        DEPTH_BITS,
        STENCIL_BITS,
        ACCUM_RED_BITS,
        ACCUM_GREEN_BITS,
        ACCUM_BLUE_BITS,
        ACCUM_ALPHA_BITS,
        AUX_BUFFERS,
        SAMPLES = 0x0002100D,
        REFRESH_RATE,
        STEREO,
        SRGB_CAPABLE,
        DOUBLEBUFFER,
        CLIENT_API,
        CONTEXT_CREATION_API,
        CONTEXT_VERSION_MAJOR = 0x00022002,
        CONTEXT_VERSION_MINOR = 0x00022003,
        CONTEXT_ROBUSTNESS,
        CONTEXT_RELEASE_BEHAVIOR,
        OPENGL_FORWARD_COMPAT,
        OPENGL_DEBUG_CONTEXT,
        OPENGL_PROFILE = 0x00022008
    }

    public enum GLFW
    {
        True = 0x00000001,
        False = 0x00000000,
        DONT_CARE,
        OPENGL_API,
        OPENGL_ES,
        NO_API,
        NO_ROBUSTNESS,
        NO_RESET_NOTIFICATION,
        LOSE_CONTEXT_ON_RESET,
        ANY_RELEASE_BEHAVIOR,
        RELEASE_BEHAVIOR_FLUSH,
        RELEASE_BEHAVIOR_NONE,
        OPENGL_ANY_PROFILE,
        OPENGL_COMPAT_PROFILE,
        OPENGL_CORE_PROFILE = 0x00032001
    }
}
