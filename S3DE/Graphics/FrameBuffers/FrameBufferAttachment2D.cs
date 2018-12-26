using S3DE.Graphics.Textures;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.FrameBuffers
{
    public class FrameBufferAttachment2D : FrameBufferAttachment
    {
        RenderTexture2D internalTexture;

        internal override IRenderTexture InternalTexture => internalTexture;

        public FrameBufferAttachment2D(InternalFormat frmt,PixelFormat pxfrmt,PixelType pxType,Vector2 res) : base(frmt)
        {
            internalTexture = new RenderTexture2D((int)res.x, (int)res.y, frmt,pxfrmt,pxType);
            internalTexture.WrapMode = WrapMode.None;
            internalTexture.FilterMode = FilterMode.Nearest;

            internalTexture.Apply();
        }
    }
}
