using OpenGL;
using Khronos;

using S3DE.Engine.Entities;
using S3DE.Engine.Graphics;
using S3DE.Engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S3DE;
using static S3DE.Engine.Enums;
using S3DE.Maths;
using S3DE.Engine.Graphics.Materials;
using S3DE.Engine.Graphics.Textures;
using S3DE.Engine.Graphics.OpGL.DC;

namespace S3DE.Engine.Graphics.OpGL
{

    public sealed class OpenGL_Renderer : Renderer
    {
        public override RenderingAPI GetRenderingAPI() => RenderingAPI.OpenGL;

        Dictionary<Type, OpenGL_Material> ShaderPrograms;

        protected override void Clear()
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit | 
                     ClearBufferMask.DepthBufferBit);
        }

        protected override Renderer_Material CreateMaterial(Type materialType,RenderPass pass)
        {
            if (!ShaderPrograms.TryGetValue(materialType,out OpenGL_Material mat))
            {
                mat = new OpenGL_Material();
                ShaderPrograms.Add(materialType, mat);
            }

            return mat;
        }

        protected override Renderer_MeshRenderer CreateMeshRenderer()
        {
            return new OpenGL_MeshRenderer();
        }

        protected override void Draw_Mesh(Renderer_Mesh m)
        {
            OpenGL_Mesh mesh = m as OpenGL_Mesh;
            Draw_Mesh(mesh);
        }

        void Draw_Mesh(OpenGL_Mesh m)
        {
            m.Bind();
            Gl.DrawElements(PrimitiveType.Triangles, m.Indicies, DrawElementsType.UnsignedShort, IntPtr.Zero);
        }

        protected override void Init()
        {
            Console.WriteLine("Initializing OpenGL_Renderer");
            ShaderPrograms = new Dictionary<Type, OpenGL_Material>();

            Console.WriteLine("Creating dummy DeviceContext");
            DeviceContext dc = DeviceContext.Create();
            Gl.Initialize();
            dc.Dispose();
        }

        protected override void SetCapabilities()
        {
            Console.WriteLine("Setting Renderer Capabilities");
            string s = $"GPU | Vendor: {Gl.GetString(StringName.Vendor)} | Model: {Gl.GetString(StringName.Renderer)}";
            TestForGLErrors();
            Console.WriteLine("GPU: " + s);
            Gl.Get(Gl.MINOR_VERSION, out int minor);
            TestForGLErrors();
            Gl.Get(Gl.MAJOR_VERSION, out int major);
            TestForGLErrors();
            SetApiVersion((major * 100) + (minor * 10));

            Console.WriteLine($"Supported OpenGL Version: " + Renderer.API_Version);
            
            Gl.Enable(EnableCap.CullFace);
            TestForGLErrors();
            Gl.FrontFace(FrontFaceDirection.Cw);
            TestForGLErrors();
            Gl.Enable(EnableCap.DepthTest);
            TestForGLErrors();
            Gl.CullFace(CullFaceMode.Back);
            TestForGLErrors();
            Gl.FrontFace(FrontFaceDirection.Cw);
            TestForGLErrors();

        }

        protected override void SetDrawBuffers(BufferAttachment[] buffers)
        {
            //Convert attachments to glattachments.
            int[] vals = new int[buffers.Length];
            for (int i = 0; i < buffers.Length; i++)
                vals[i] = (int)OpenGL_Utility.Convert(buffers[i]);

            Gl.DrawBuffers(vals);
            TestForGLErrors();
        }

        internal static void TestForGLErrors()
        {
            ErrorCode err = Gl.GetError();
            if (err != ErrorCode.NoError)
            {
                throw new Exception("GL Operation Failed, Error: " + err);
            }
        }

        protected override void Sync()
        {
            Gl.Finish();
            TestForGLErrors();
        }

        protected override void OnWindowResized()
        {
            //This needs to be removed. As the viewport will now be set from the framebuffer currently being drawn. (Framebuffer.Resolution)
            //The screenquad will also set the viewport to the appropriate size when drawn. (DisplayResolution)
            //So there's no need to change the viewport when we resize the window.
            Gl.Viewport(0, 0, (int)Game.DisplayResolution.x, (int)Game.DisplayResolution.y);
            TestForGLErrors();
        }

        protected override void OnRenderResolutionChanged()
        {
            
        }

        protected override void SetViewportSize(Vector2 size)
        {
            Gl.Viewport(0, 0, (int)size.x, (int)size.y);
            TestForGLErrors();
        }

        protected override Texture2D CreateTexture2D(int width, int height) => new OpenGL_Texture2D(width, height);

        protected override void OnRefreshRateChanged()
        {
            
        }

        protected override Framebuffer CreateFrameBuffer(int width, int height) => new OpenGL_Framebuffer(width,height);

        protected override Renderer_Mesh CreateRendererMesh() => new OpenGL_Mesh();

        protected override RenderTexture2D CreateRenderTexture2D(S3DE.Engine.Enums.InternalFormat internalFormat, S3DE.Engine.Enums.PixelFormat pixelFormat, S3DE.Engine.Enums.PixelType pixelType,FilterMode filter, int width, int height) => 
            new OpenGL_RenderTexture2D(internalFormat,pixelFormat,pixelType,filter,width, height);

        protected override void BindTexUnit(ITexture tex, TextureUnit tu)
        {
            Gl.ActiveTexture(OpenGL.TextureUnit.Texture0 + (int)tu);
            Gl.BindTexture(TextureTarget.Texture2d, ((IOpenGL_Texture)tex).Pointer);
            TestForGLErrors();
        }
        protected override void UnbindTexUnit(TextureUnit textureUnit)
        {
            Gl.ActiveTexture(OpenGL.TextureUnit.Texture0 + (int)textureUnit);
            Gl.BindTexture(TextureTarget.Texture2d,0);
            TestForGLErrors();
        }
        protected override void SetAlphaFunction(S3DE.Engine.Graphics.AlphaFunction function, float value)
        {
            Gl.AlphaFunc(OpenGL_Utility.Convert(function), value);
            TestForGLErrors();
        }

        protected override ScreenQuad CreateScreenQuad() => new OpenGL_ScreenQuad();

        protected override void enable(Function func)
        {
            Gl.Enable(OpenGL_Utility.Convert(func));
            TestForGLErrors();
        }

        protected override void disable(Function func)
        {
            Gl.Disable(OpenGL_Utility.Convert(func));
            TestForGLErrors();
        }

        protected override int MaxSupportedTextureUnits()
        {
            OpenGL.Gl.Get(OpenGL.Gl.MAX_TEXTURE_IMAGE_UNITS,out int c);
            return c;
        }

        protected override void FinalizePass()
        {
            DrawCallSorter.IssueDrawCalls();
            DrawCallSorter.FlushContainers();
        }
    }
}