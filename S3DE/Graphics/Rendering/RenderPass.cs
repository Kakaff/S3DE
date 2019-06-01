using S3DE.Graphics.FrameBuffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Rendering
{
    public enum RenderPassType
    {
        Deferred,
        Forward,
        Blend,
    }

    public abstract class RenderPass
    {

        Dictionary<int, DrawCallContainer> drawCallContainers;

        protected FrameBuffer framebuffer;
        public abstract bool ClearBeforeRendering { get; }
        public abstract int Identifier { get; }
        public FrameBuffer GetFrameBuffer() => framebuffer;

        public abstract void ClearBuffers();
        protected abstract void CreateFramebuffer();

        protected RenderPass() { drawCallContainers = new Dictionary<int, DrawCallContainer>(); CreateFramebuffer(); framebuffer.Unbind(); }

        /// <summary>
        /// This method is called before the RenderPass starts rendering its drawcalls.
        /// </summary>
        protected abstract void OnRender();
        /// <summary>
        /// This method is called after the RenderPass has rendered all its drawcalls.
        /// </summary>
        protected abstract void PostDrawCallRender();

        internal void BindFrameBuffer()
        {
            framebuffer.Bind();
        }

        public void Render() {
            
            OnRender();
            foreach (KeyValuePair<int, DrawCallContainer> p in drawCallContainers)
                p.Value.Draw();
            
            //Get all the drawCallContainers and issue the drawcalls stored in them.
            PostDrawCallRender();
        }

        internal void AddDrawcall(Drawcall dc)
        {
            DrawCallContainer dcc;
            if (!drawCallContainers.TryGetValue(dc.MaterialID,out dcc))
            {
                dcc = new DrawCallContainer(0,this);
                drawCallContainers.Add(dc.MaterialID, dcc);
            }

            dcc.AddDrawcall(dc);
        }

        internal void RemoveContainer(DrawCallContainer dcc)
        {
            throw new NotImplementedException();
        }

        internal class DrawCallContainer
        {
            List<Drawcall> dcs;
            int level;
            DrawCallContainer parentContainer;
            RenderPass parentRenderPass;

            Dictionary<int, DrawCallContainer> subContainers;

            private DrawCallContainer() { }

            public DrawCallContainer(int level,RenderPass parentRenderPass)
            {
                this.level = level;
                dcs = new List<Drawcall>();
                subContainers = new Dictionary<int, DrawCallContainer>();

                this.parentRenderPass = parentRenderPass;
            }

            public DrawCallContainer(int level,RenderPass parentRenderPass, DrawCallContainer parent) : this(level,parentRenderPass)
            {
                parentContainer = parent;
            }

            public void Draw()
            {
                for (int i = 0; i < dcs.Count; i++)
                    dcs[i].Draw();
            }

            public void AddDrawcall(Drawcall dc)
            {
                dcs.Add(dc);
            }

            public void RemoveDrawCall(Drawcall dc)
            {
                if (dcs.Contains(dc))
                    dcs.Remove(dc);

                if (dcs.Count == 0 /*&& subContainers.Count == 0*/)
                    if (level > 0)
                        parentContainer.RemoveContainer(this);
                    else
                        parentRenderPass.RemoveContainer(this);

                if (level > 0 && dcs.Count == 0 /*&& subContainers.Count == 0*/)
                    parentContainer.RemoveContainer(this);
            }

            public void RemoveContainer(DrawCallContainer dcc)
            {
                //Check the level, get the correct texture id. Remove subContainer with textureID key at dcc.Level - 1.
                throw new NotImplementedException();
            }
        }
    }
}
