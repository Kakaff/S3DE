﻿using S3DE.Engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics
{
    public abstract class ShaderSource
    {
        string @source;
        ShaderStage stage;

        public string @Source => source;
        public ShaderStage Stage => stage;

        protected void SetStage(ShaderStage stage) => this.stage = stage;
        protected void SetSource(string source) => this.source = source;
    }
}