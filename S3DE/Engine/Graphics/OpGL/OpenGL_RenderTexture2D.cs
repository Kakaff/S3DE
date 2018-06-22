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
using S3DE.Engine.Graphics.Textures;

namespace S3DE.Engine.Graphics.OpGL
{
    internal class OpenGL_RenderTexture2D : RenderTexture2D, IOpenGL_Texture
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

        uint IOpenGL_Texture.Pointer => Pointer;

        void GeneratePointer()
        {
            pointer = Gl.GenTexture();
            Bind(TextureUnit._0);

            (int MinFilter, int MagFilter) t = OpenGL_Utility.Convert(FilterMode, false);
            Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, ref t.MinFilter);
            OpenGL_Renderer.TestForGLErrors();
            Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, ref t.MagFilter);
            OpenGL_Renderer.TestForGLErrors();

            Gl.TexImage2D(TextureTarget.Texture2d, 0,
                OpenGL_Utility.Convert(InternalFormat), (int)size.X, (int)size.Y, 0, 
                OpenGL_Utility.Convert(PixelFormat), 
                OpenGL_Utility.Convert(PixelType), 
                IntPtr.Zero);

            
        }

        public override Texture2D ToTexture2D()
        {
            throw new NotImplementedException();
        }

        public override bool Compare(ITexture tex1)
        {
            return ((IOpenGL_Texture)tex1).Pointer == Pointer;
        }
    }
}
