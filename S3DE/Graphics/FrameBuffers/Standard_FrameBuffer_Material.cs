using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.FrameBuffers
{
    class Standard_FrameBuffer_Material : FrameBufferMaterial
    {
        internal override void PresentFrame(FrameBuffer fb)
        {
           DrawTexture(new Vector2(0,0),new Vector2(1,1),fb.GetAttachment(FrameBufferAttachmentLocation.COLOR0).InternalTexture);
        }
    }
}
