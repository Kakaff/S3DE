using S3DE.Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S3DE.Maths;
using OpenGL;
using static S3DE.Engine.Enums;
using S3DE.Engine;

namespace SampleGame.Sample_OGL_Renderer
{
    internal class OpenGL_RenderTexture2D : RenderTexture2D
    {
        Vector2 size;
        uint pointer;

        internal uint Pointer {
            get {
                if (pointer == 0)
                    GeneratePointer();
                return pointer;
            }
        }
        Enums.InternalFormat internalFormat;
        Enums.PixelFormat pixelFormat;
        Enums.PixelType pixelType;
        FilterMode filterMode;

        internal OpenGL_RenderTexture2D(Enums.InternalFormat internalFormat, Enums.PixelFormat pixelFormat, Enums.PixelType pixelType, FilterMode filterMode,int width, int height)
        {
            Console.WriteLine($"Creating new RenderTexture2D " +
                $"| Resolution {width}x{height} | " +
                $"InternalFormat {internalFormat} | " +
                $"PixelFormat {pixelFormat} | " +
                $"PixelType {pixelType}");
            size = new Vector2(width, height);
            this.internalFormat = internalFormat;
            this.pixelFormat = pixelFormat;
            this.pixelType = pixelType;
            this.filterMode = filterMode;
        }

        public override Vector2 Size => throw new NotImplementedException();

        public override FilterMode FilterMode => filterMode;
        public override Enums.InternalFormat InternalFormat => internalFormat;
        public override Enums.PixelFormat PixelFormat => pixelFormat;
        public override Enums.PixelType PixelType => pixelType;

        public override void Bind(int textureunit)
        {
            Gl.ActiveTexture(TextureUnit.Texture0 + textureunit);
            Gl.BindTexture(TextureTarget.Texture2d, pointer);
        }

        public override void Bind()
        {
            Gl.BindTexture(TextureTarget.Texture2d, pointer);
            OpenGL_Renderer.TestForGLErrors();
        }

        public override void Unbind()
        {
            Gl.BindTexture(TextureTarget.Texture2d, 0);
        }

        void GeneratePointer()
        {
            pointer = Gl.GenTexture();
            Bind();

            (int MinFilter, int MagFilter) t = OpenGL_Utility.Convert(FilterMode, false);
            Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, ref t.MinFilter);
            OpenGL_Renderer.TestForGLErrors();
            Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, ref t.MagFilter);
            OpenGL_Renderer.TestForGLErrors();

            Gl.TexImage2D(TextureTarget.Texture2d, 0,
                OpenGL_Utility.Convert(InternalFormat), (int)size.x, (int)size.y, 0, 
                OpenGL_Utility.Convert(PixelFormat), 
                OpenGL_Utility.Convert(PixelType), 
                IntPtr.Zero);

            
        }

        public override Texture2D ToTexture2D()
        {
            throw new NotImplementedException();
        }
    }
}
