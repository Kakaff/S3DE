using S3DE.Graphics.Textures;

namespace S3DE.Graphics.FrameBuffers
{
    public abstract class FramebufferAttachment
    {
        InternalFormat intfrmt;

        private FramebufferAttachment()
        {

        }

        internal FramebufferAttachment(InternalFormat frmt)
        {
            intfrmt = frmt;
        }
    }

    public class ColorAttachment : FramebufferAttachment
    {
        public ColorAttachment(InternalFormat format) : base(format)
        {

        }
    }

    public class DepthAttachment : FramebufferAttachment
    {
        public DepthAttachment() : base(InternalFormat.DEPTH_COMPONENT)
        {
            
        }
    }

    public class StencilAttachment : FramebufferAttachment
    {
        public StencilAttachment() : base(InternalFormat.STENCIL_INDEX)
        {

        }
    }

    public class Depth_StencilAttachment : FramebufferAttachment
    {
        public Depth_StencilAttachment() : base(InternalFormat.DEPTH24_STENCIL8)
        {

        }
    }
}
