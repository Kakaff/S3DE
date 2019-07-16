using S3DE.Components;
using S3DE.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics
{
    internal class DrawcallContainer
    {
        List<Drawcall> drawcalls = new List<Drawcall>();

        public int ShaderProgramID { get; private set; }
        public int RenderpassID { get; private set; }

        internal DrawcallContainer(int spID,int rpID)
        {
            ShaderProgramID = spID;
        }

        public void Draw()
        {
            for (int i = 0; i < drawcalls.Count; i++)
                drawcalls[i].MR.Draw();
        }

        public void AddDrawcall(Drawcall dc)
        {
            dc.ParentContainer = this;
            dc.Index = drawcalls.Count;
            drawcalls.Add(dc);
        }

        public void RemoveDrawcall(Drawcall dc)
        {
            for (int i = dc.Index + 1; i < drawcalls.Count; i++)
                drawcalls[i].Index--;

            drawcalls.RemoveAt(dc.Index);
        }
    }

    internal class Drawcall
    {
        DrawcallContainer parentContainer;
        Meshrenderer mr;
        int index;
        
        internal Meshrenderer MR => mr;
        internal int Index { get => index; set => index = value; }
        internal DrawcallContainer ParentContainer { get => parentContainer; set => parentContainer = value;}


        public Drawcall(Meshrenderer mr)
        {
            this.mr = mr;
        }

        public void AddToRenderpass(Renderpass rp)
        {
            rp.AddDrawcall(this);
        }

        public void RemoveFromRenderpass()
        {
            parentContainer.RemoveDrawcall(this);
            parentContainer = null;
            index = -1;
        }
    }

    public abstract class Renderpass
    {
        Dictionary<int,DrawcallContainer> drawcallContainers = new Dictionary<int, DrawcallContainer>();
        static int cntr = 0;

        public GameScene Scene { get; internal set; }
        public int Index { get; internal set; }
        public int ID { get => id; }
        bool isInitiated;
        int id;

        protected Renderpass() { id = cntr; cntr++; }

        internal void Draw()
        {
            if (!isInitiated)
            {
                Init();
                isInitiated = true;
            }

            PreDraw();
            OnDraw();
            PostDraw();
        }

        internal DrawcallContainer GetContainer(int shaderProgID)
        {
            DrawcallContainer dcc;
            drawcallContainers.TryGetValue(shaderProgID, out dcc);
            return dcc;
        }

        internal void AddContainer(int shaderProgID)
        {
            drawcallContainers.Add(shaderProgID, new DrawcallContainer(shaderProgID,id));
        }

        internal void AddDrawcall(Drawcall dc)
        {
            if (drawcallContainers.TryGetValue(dc.MR.Material.ShaderProgramID, out DrawcallContainer dcc))
                dcc.AddDrawcall(dc);
            else
            {
                dcc = new DrawcallContainer(dc.MR.Material.ShaderProgramID,id);
                dcc.AddDrawcall(dc);
                drawcallContainers.Add(dc.MR.Material.ShaderProgramID, dcc);
            }
        }

        protected void DrawMeshes()
        {
            foreach (KeyValuePair<int, DrawcallContainer> kvp in drawcallContainers)
                kvp.Value.Draw();
        }

        protected abstract void Init();
        protected abstract void PreDraw();
        protected abstract void OnDraw();
        protected abstract void PostDraw();
    }
}
