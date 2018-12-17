using S3DE.Maths;
using System;

namespace S3DE.Graphics.FrameBuffers
{
    public enum FrameBufferAttachmentLocation
    {
        COLOR0 = 0x8CE0,
        COLOR1 = 0x8CE1,
        COLOR2 = 0x8CE2,
        COLOR3 = 0x8CE3,
        COLOR4 = 0x8CE4,
        COLOR5 = 0x8CE5,
        COLOR6 = 0x8CE6,
        COLOR7 = 0x8CE7,
        COLOR8 = 0x8CE8,
        COLOR9 = 0x8CE9,
        COLOR10 = 0x8CEA,
        COLOR11 = 0x8CEB,
        COLOR12 = 0x8CEC,
        COLOR13 = 0x8CED,
        COLOR14 = 0x8CEE,
        COLOR15 = 0x8CEF,
        DEPTH = 0x8D00,
        STENCIL = 0x8D20,
        DEPTH_STENCIL = 0x821A,
    }

    public sealed partial class FrameBuffer
    {
        int width, height;

        FramebufferAttachment[] attachments;

        private FrameBuffer() { }

        public FrameBuffer(int width, int height)
        {
            this.width = width;
            this.height = height;
            attachments = new FramebufferAttachment[19];
        }
        
        public void AddAttachment(FramebufferAttachment fba,FrameBufferAttachmentLocation fbal)
        {
            int tIndex = GetBufferIndex(fbal);

            //Might change this in the future. Since there's no reason to keep people who know what they are doing from
            //not using a whatever attachment they want for whatever buffer they want.
            if ((fbal == FrameBufferAttachmentLocation.DEPTH && !(fba is DepthAttachment)) ||
                (fbal == FrameBufferAttachmentLocation.STENCIL && !(fba is StencilAttachment)) ||
                (fbal == FrameBufferAttachmentLocation.DEPTH_STENCIL && !(fba is Depth_StencilAttachment)) ||
                (EngineMath.IsInrange(tIndex, 0, 15) && !(fba is ColorAttachment)))
            {
                throw new ArgumentException();
            }


        }

        public void BindAttachmentToTextureUnit()
        {

        }

        public void Clear()
        {

        }

        public void Bind()
        {

        }

        static int GetBufferIndex(FrameBufferAttachmentLocation fbal)
        {
            switch (fbal)
            {
                case FrameBufferAttachmentLocation.COLOR0: return 0;
                case FrameBufferAttachmentLocation.COLOR1: return 1;
                case FrameBufferAttachmentLocation.COLOR2: return 2;
                case FrameBufferAttachmentLocation.COLOR3: return 3;
                case FrameBufferAttachmentLocation.COLOR4: return 4;
                case FrameBufferAttachmentLocation.COLOR5: return 5;
                case FrameBufferAttachmentLocation.COLOR6: return 6;
                case FrameBufferAttachmentLocation.COLOR7: return 7;
                case FrameBufferAttachmentLocation.COLOR8: return 8;
                case FrameBufferAttachmentLocation.COLOR9: return 9;

                case FrameBufferAttachmentLocation.COLOR10: return 10;
                case FrameBufferAttachmentLocation.COLOR11: return 11;
                case FrameBufferAttachmentLocation.COLOR12: return 12;
                case FrameBufferAttachmentLocation.COLOR13: return 13;
                case FrameBufferAttachmentLocation.COLOR14: return 14;
                case FrameBufferAttachmentLocation.COLOR15: return 15;

                case FrameBufferAttachmentLocation.DEPTH: return 16;
                case FrameBufferAttachmentLocation.STENCIL: return 17;
                case FrameBufferAttachmentLocation.DEPTH_STENCIL: return 18;

                default: throw new ArgumentException("Unkown framebuffer attachment location.");
            }
        }

    }
}
