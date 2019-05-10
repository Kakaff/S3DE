using S3DE.Graphics.Textures;
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
        static FrameBuffer activeFrameBuffer;
        public static FrameBuffer ActiveFrameBuffer => activeFrameBuffer;

        bool isBound, isCleared;

        FrameBufferAttachment[] attachments;
        IntPtr handle;
        Vector2 res;
        uint id;
        static uint idcntr = 0;
        public bool IsBound => isBound;
        public bool IsCleared => isCleared;
        public uint ID => id;

        private FrameBuffer() { }

        public FrameBuffer(Vector2 res)
        {
            attachments = new FrameBufferAttachment[19];
            isBound = false;
            isCleared = false;
            this.res = res;
            handle = Extern_FrameBuffer_Create();
            id = idcntr;
            idcntr++;
        }
        
        public void AddAttachment(FrameBufferAttachment fba,FrameBufferAttachmentLocation fbal)
        {
            if (fba.InternalTexture.Height != (int)res.y || fba.InternalTexture.Width != (int)res.x)
                throw new Exception("Error: The framebuffer attachment must have the same resolution as the framebuffer!");

            int tIndex = GetBufferIndex(fbal);

            if (attachments[tIndex] != null)
                throw new ArgumentException($"{fbal.ToString()} is already set for this FrameBuffer!");

            if (!isBound)
                Bind();

            Extern_FrameBuffer_AddTextureAttachment2D(fba.InternalTexture.Handle, (uint)fbal, 0);
            if (!Renderer.NoError)
                throw new Exception("Error attaching texture!");
            attachments[tIndex] = fba;

        }

        public int BindAttachmentToTextureUnit(FrameBufferAttachmentLocation attachment)
        {
            FrameBufferAttachment fba = attachments[GetBufferIndex(attachment)];

            if (fba == null)
                throw new ArgumentNullException();

            return fba.InternalTexture.Bind();
        }

        public FrameBufferAttachment GetAttachment(FrameBufferAttachmentLocation attachmentLocation)
        {
            return (attachments[GetBufferIndex(attachmentLocation)]);
        }

        /// <summary>
        /// Binds the FBO if it is not bound, and clears it. Will not unbind the FBO when done.
        /// </summary>
        /// <param name="clearbit"></param>
        public void Clear(ClearBufferBit clearbit)
        {
            if (!isBound)
                Bind();

            Renderer.Clear(clearbit);
            if (!Renderer.NoError)
                throw new Exception($"Error clearing framebuffer! Error: {Renderer.LatestError}");
        }

        /// <summary>
        /// Binds the FBO
        /// </summary>
        public void Bind()
        {
            Extern_FrameBuffer_Bind(handle);
            Renderer.Set_ViewPortSize((uint)res.x, (uint)res.y);
            isBound = true;
            if (activeFrameBuffer != null)
                activeFrameBuffer.isBound = false;

            activeFrameBuffer = this;
        }

        /// <summary>
        /// Checks if the FBO is a complete FBO.
        /// </summary>
        /// <returns></returns>
        public bool CheckIsComplete()
        {
            if (!isBound)
                Bind();
            
            bool res = Extern_FrameBuffer_IsComplete(handle);
            Console.WriteLine("FrameBuffer status: " + Extern_FrameBuffer_CheckStatus());

            return res;
        }

        /// <summary>
        /// Unbinds the FBO
        /// </summary>
        public void Unbind()
        {
            if (isBound)
            {
                Extern_FrameBuffer_Unbind();
                Renderer.Set_ViewPortSize((uint)Renderer.DisplayResolution.x, (uint)Renderer.DisplayResolution.y);
                isBound = false;
                activeFrameBuffer = null;
            } else
            {
                throw new Exception($"Framebuffer {id} isn't bound!");
            }
        }

        /// <summary>
        /// Creates a FBO with a RGBA Color0 and Depth_Component Depth Attachment.
        /// </summary>
        /// <param name="res">The desired resolution of this FBO</param>
        /// <returns></returns>
        public static FrameBuffer Create_Standard_FrameBuffer(Vector2 res)
        {
            Console.WriteLine("Creating standard framebuffer, resolution: " + res.ToString());
            FrameBuffer fb = new FrameBuffer(res);
            Console.WriteLine("Attaching Color0 attachment");
            fb.AddAttachment(new FrameBufferAttachment2D(InternalFormat.RGBA,PixelFormat.RGBA,PixelType.FLOAT, res),
                FrameBufferAttachmentLocation.COLOR0);
            Console.WriteLine("Attaching depth buffer");
            fb.AddAttachment(new FrameBufferAttachment2D(InternalFormat.DEPTH_COMPONENT, PixelFormat.DEPTH_COMPONENT,PixelType.FLOAT, res),
                FrameBufferAttachmentLocation.DEPTH);

            if (!fb.CheckIsComplete())
                throw new Exception("FrameBuffer is not complete!");

            Console.WriteLine("Framebuffer is complete!");
            return fb;
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
