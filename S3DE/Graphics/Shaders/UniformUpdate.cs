using S3DECore.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Shaders
{
    internal abstract class UniformUpdate
    {
        int location = -1;
        public abstract UniformType UniformType { get; }
        protected int UniformLocation => location;

        private UniformUpdate() { }
        protected UniformUpdate(int location) { this.location = location; }

        public abstract void Perform();
    }
}
