using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Utility
{
    public sealed class PinnedMemory : IDisposable
    {
        private PinnedMemory() { }

        GCHandle handle;

        public IntPtr Adress => handle.AddrOfPinnedObject();

        public PinnedMemory(Object o)
        {
            handle = GCHandle.Alloc(o,GCHandleType.Pinned);
        }

        public void Dispose()
        {
            handle.Free();
        }
    }
}
