using OpenGL;
using S3DE.Engine.Graphics;
using S3DE.Engine.Graphics.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static S3DE.Engine.Enums;

namespace S3DE.Engine.Graphics.OpGL
{
    internal static class OpenGL_Utility
    {

        internal static EnableCap Convert(Function func)
        {

            switch (func)
            {
                case Function.AlphaTest: return EnableCap.AlphaTest;
                case Function.DepthTest: return EnableCap.DepthTest;
            }

            throw new NotSupportedException($"No conversion exists for Function.{func} to OpenGL.EnableCap");
        }

        internal static (int MinFilter,int MagFilter) Convert(FilterMode filterMode,bool mipmap)
        {
            int min = 0;
            int mag = 0;

            switch (filterMode)
            {
                case FilterMode.Nearest: min = (mipmap) ? Gl.NEAREST_MIPMAP_LINEAR : Gl.NEAREST; mag = Gl.NEAREST; break;
                case FilterMode.Bilinear: min = (mipmap) ? Gl.LINEAR_MIPMAP_NEAREST : Gl.NEAREST; mag = Gl.NEAREST; break;
                case FilterMode.Trilinear: min = (mipmap) ? Gl.LINEAR_MIPMAP_LINEAR : Gl.LINEAR; mag = Gl.NEAREST; break;
            }

            return (min, mag);
        }
        internal static OpenGL.InternalFormat Convert(S3DE.Engine.Enums.InternalFormat internform)
        {
            switch (internform)
            {
                case S3DE.Engine.Enums.InternalFormat.R: return OpenGL.InternalFormat.Red;
                case S3DE.Engine.Enums.InternalFormat.R16F: return OpenGL.InternalFormat.R16f;
                case S3DE.Engine.Enums.InternalFormat.R16I: return OpenGL.InternalFormat.R16i;
                case S3DE.Engine.Enums.InternalFormat.R16UI: return OpenGL.InternalFormat.R16ui;
                case S3DE.Engine.Enums.InternalFormat.R16: return OpenGL.InternalFormat.R16;
                case S3DE.Engine.Enums.InternalFormat.RGB: return OpenGL.InternalFormat.Rgb;
                case S3DE.Engine.Enums.InternalFormat.RGBA: return OpenGL.InternalFormat.Rgba;
                case S3DE.Engine.Enums.InternalFormat.RGB8: return OpenGL.InternalFormat.Rgb8;
                case S3DE.Engine.Enums.InternalFormat.RGB10: return OpenGL.InternalFormat.Rgb10;
                case S3DE.Engine.Enums.InternalFormat.RGB10_A2: return OpenGL.InternalFormat.Rgb10A2;
                case S3DE.Engine.Enums.InternalFormat.RGB16F: return OpenGL.InternalFormat.Rgb16f;
                case S3DE.Engine.Enums.InternalFormat.RGBA16F: return OpenGL.InternalFormat.Rgba16f;
                case S3DE.Engine.Enums.InternalFormat.Depth24_Stencil8: return OpenGL.InternalFormat.Depth24Stencil8;
                case S3DE.Engine.Enums.InternalFormat.DepthComponent16: return OpenGL.InternalFormat.DepthComponent16;
                case S3DE.Engine.Enums.InternalFormat.DepthComponent24: return OpenGL.InternalFormat.DepthComponent24;
                case S3DE.Engine.Enums.InternalFormat.Depth_Component32: return OpenGL.InternalFormat.DepthComponent32f;
            }
            throw new NotSupportedException($"No conversion exists for InternalFormat.{internform} to OpenGL.InternalFormat");
        }

        internal static OpenGL.PixelFormat Convert(S3DE.Engine.Enums.PixelFormat pf)
        {
            switch (pf)
            {
                case S3DE.Engine.Enums.PixelFormat.DepthStencil: return OpenGL.PixelFormat.DepthStencil;
                case S3DE.Engine.Enums.PixelFormat.RGB: return OpenGL.PixelFormat.Rgb;
                case S3DE.Engine.Enums.PixelFormat.RGBA: return OpenGL.PixelFormat.Rgba;
                case S3DE.Engine.Enums.PixelFormat.Depth: return OpenGL.PixelFormat.DepthComponent;
                case S3DE.Engine.Enums.PixelFormat.Red: return OpenGL.PixelFormat.Red;
                case S3DE.Engine.Enums.PixelFormat.Uint: return OpenGL.PixelFormat.UnsignedInt;
            }
            throw new NotSupportedException($"No conversion exists for PixelFormat.{pf} to OpenGL.PixelFormat");
        }

        internal static OpenGL.PixelType Convert(S3DE.Engine.Enums.PixelType pt)
        {
            
            switch (pt)
            {
                case S3DE.Engine.Enums.PixelType.UByte: return OpenGL.PixelType.UnsignedByte;
                case S3DE.Engine.Enums.PixelType.Float16: return OpenGL.PixelType.HalfFloat;
                case S3DE.Engine.Enums.PixelType.Float32: return OpenGL.PixelType.Float;
                case S3DE.Engine.Enums.PixelType.UInt16: return OpenGL.PixelType.UnsignedShort;
                case S3DE.Engine.Enums.PixelType.UInt32: return OpenGL.PixelType.UnsignedInt;
                case S3DE.Engine.Enums.PixelType.UInt24_8: return OpenGL.PixelType.UnsignedByte;
            }
            throw new NotSupportedException($"No conversion exists for PixelType.{pt} to OpenGL.PixelType");
        }

        internal static OpenGL.AlphaFunction Convert(S3DE.Engine.Graphics.AlphaFunction func)
        {
            switch (func)
            {
                case S3DE.Engine.Graphics.AlphaFunction.Equals: return OpenGL.AlphaFunction.Equal;
                case S3DE.Engine.Graphics.AlphaFunction.Always: return OpenGL.AlphaFunction.Always;
                case S3DE.Engine.Graphics.AlphaFunction.GreaterThan: return OpenGL.AlphaFunction.Greater;
                case S3DE.Engine.Graphics.AlphaFunction.LessThan: return OpenGL.AlphaFunction.Less;
                case S3DE.Engine.Graphics.AlphaFunction.Never: return OpenGL.AlphaFunction.Never;
            }
            throw new NotSupportedException($"No conversion exists for AlphaFunction.{func} to OpenGL.AlphaFunction");
        }

        internal static FramebufferAttachment Convert(BufferAttachment attachment)
        {
            switch (attachment)
            {
                case BufferAttachment.Color0: return FramebufferAttachment.ColorAttachment0;
                case BufferAttachment.Color1: return FramebufferAttachment.ColorAttachment1;
                case BufferAttachment.Color2: return FramebufferAttachment.ColorAttachment2;
                case BufferAttachment.Color3: return FramebufferAttachment.ColorAttachment3;
                case BufferAttachment.Color4: return FramebufferAttachment.ColorAttachment4;
                case BufferAttachment.Color5: return FramebufferAttachment.ColorAttachment5;
                case BufferAttachment.Color6: return FramebufferAttachment.ColorAttachment6;
                case BufferAttachment.Color7: return FramebufferAttachment.ColorAttachment7;
                case BufferAttachment.Color8: return FramebufferAttachment.ColorAttachment8;
                case BufferAttachment.Color9: return FramebufferAttachment.ColorAttachment9;

                case BufferAttachment.Color10: return FramebufferAttachment.ColorAttachment10;
                case BufferAttachment.Color11: return FramebufferAttachment.ColorAttachment11;
                case BufferAttachment.Color12: return FramebufferAttachment.ColorAttachment12;
                case BufferAttachment.Color13: return FramebufferAttachment.ColorAttachment13;
                case BufferAttachment.Color14: return FramebufferAttachment.ColorAttachment14;
                case BufferAttachment.Color15: return FramebufferAttachment.ColorAttachment15;
                case BufferAttachment.Color16: return FramebufferAttachment.ColorAttachment16;
                case BufferAttachment.Color17: return FramebufferAttachment.ColorAttachment17;
                case BufferAttachment.Color18: return FramebufferAttachment.ColorAttachment18;
                case BufferAttachment.Color19: return FramebufferAttachment.ColorAttachment19;

                case BufferAttachment.Color20: return FramebufferAttachment.ColorAttachment20;
                case BufferAttachment.Color21: return FramebufferAttachment.ColorAttachment21;
                case BufferAttachment.Color22: return FramebufferAttachment.ColorAttachment22;
                case BufferAttachment.Color23: return FramebufferAttachment.ColorAttachment23;
                case BufferAttachment.Color24: return FramebufferAttachment.ColorAttachment24;
                case BufferAttachment.Color25: return FramebufferAttachment.ColorAttachment25;
                case BufferAttachment.Color26: return FramebufferAttachment.ColorAttachment26;
                case BufferAttachment.Color27: return FramebufferAttachment.ColorAttachment27;
                case BufferAttachment.Color28: return FramebufferAttachment.ColorAttachment28;
                case BufferAttachment.Color29: return FramebufferAttachment.ColorAttachment29;

                case BufferAttachment.Color30: return FramebufferAttachment.ColorAttachment30;
                case BufferAttachment.Color31: return FramebufferAttachment.ColorAttachment31;

                case BufferAttachment.Depth: return FramebufferAttachment.DepthAttachment;
                case BufferAttachment.MaxColor: return FramebufferAttachment.MaxColorAttachments;
                case BufferAttachment.Depth_Stencil: return (FramebufferAttachment)Gl.DEPTH24_STENCIL8;


            }

            throw new NotSupportedException($"No conversion exists for BufferAttachment.{attachment} to OpenGL.FramebufferAttachment");
        }
    }
}
