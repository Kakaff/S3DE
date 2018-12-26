using S3DE.Graphics.Textures;

namespace S3DE.Graphics.FrameBuffers
{
    public abstract class FrameBufferAttachment
    {
        InternalFormat intfrmt;

        internal abstract IRenderTexture InternalTexture {get;}

        private FrameBufferAttachment()
        {

        }

        internal FrameBufferAttachment(InternalFormat frmt)
        {
            intfrmt = frmt;
        }
    }

}
