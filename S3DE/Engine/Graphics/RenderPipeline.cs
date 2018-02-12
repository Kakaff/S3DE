using S3DE.Engine.Entities;
using S3DE.Engine.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics
{
    internal static class RenderPipeline
    {
        public static void RenderSceneToFrameBuffer(RenderPass pass,GameScene scene, Framebuffer framebuffer)
        {
            switch (pass)
            {
                case RenderPass.ShadowMap:
                    {
                        Draw_Shadow_Depth(framebuffer, scene);
                        Draw_Shadow_Color(framebuffer, scene);
                        break;
                    }
                case RenderPass.Deferred:
                    {
                        Draw_Deferred_Geometry(framebuffer, scene);
                        Draw_Deferred_Light(framebuffer, scene);
                        break;
                    }
                case RenderPass.Forward:
                    {
                        Draw_Forward(framebuffer, scene);
                        break;
                    }
                case RenderPass.Blend:
                    {
                        //We could possibly make it supported if we add a way to assign which two framebuffers to blend.
                        throw new NotSupportedException("Drawing only RenderPass.Blend() is not supported, use a RenderCall");
                    }
            }
        }

        public static void RenderSceneToFrameBuffer(GameScene scene, Framebuffer framebuffer)
        {

        }

        public static void RenderSceneToRenderCall(GameScene scene, Rendercall renderCall)
        {
            Renderer.ViewportSize = renderCall.Resolution;
            Renderer.Enable(Function.DepthTest);
            Framebuffer fb = renderCall.GetFrameBuffer(RenderPass.Deferred);
            fb.Bind();
            fb.Clear();

            if (renderCall.Uses(RenderPass.Deferred))
            {
                Draw_Deferred_Geometry(fb,scene);
                Draw_Deferred_Light(fb, scene);
            }

            fb.Unbind();
                
        }

        

        internal static void Render(Rendercall rc)
        {
            
        }

        static void Draw_Shadow_Depth(Framebuffer frameBuffer,GameScene scene)
        {
            Renderer.CurrentRenderPass = RenderPass.ShadowMap;
            //Draw all Meshrenders, Check that alpha is 1.0 to write to the depth buffer
            //Disable the color buffer.
        }

        static void Draw_Shadow_Color(Framebuffer frameBuffer, GameScene scene)
        {
            Renderer.CurrentRenderPass = RenderPass.ShadowMap;
            //Use the same framebuffer as for Shadow_Depth but disable depth writing.
            //Get shadowcasters that cast colored shadows.
            //Try using Material.Use(RenderPass.Shadow_Depth);
            //If that for some reason fails, fallback to (Renderpass.Deferred) (We only want the albedo afterall.
        }

        static void Draw_Deferred_Geometry(Framebuffer frameBuffer, GameScene scene)
        {
            Renderer.CurrentRenderPass = RenderPass.Deferred;
            //Bind the frameBuffer.
            GameEntity[] entities = scene.ActiveEntities;
            foreach (GameEntity ge in entities)
                ge.Draw();
            
        }

        static void Draw_Deferred_Light(Framebuffer frameBuffer, GameScene scene)
        {
            Renderer.CurrentRenderPass = RenderPass.Deferred;
        }

        static void Draw_Forward(Framebuffer frameBuffer, GameScene scene)
        {
            Renderer.CurrentRenderPass = RenderPass.Forward;
            //Doesn't use the shadow color map.

        }

        static void Draw_Blend(Framebuffer frameBuffer, GameScene scene)
        {
            Renderer.CurrentRenderPass = RenderPass.Blend;
        }
    }
}
