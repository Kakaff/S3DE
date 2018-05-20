using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics.OpGL.DC
{
    class DrawCallContainer
    {
        internal enum ContainerType
        {
            ShaderProgram,
            Texture,
        }

        int identifier;
        int depth;
        List<DrawCallContainer> subDrawCallContainers;
        List<DrawCall> drawCalls;
        ContainerType containerType;


        internal int Identifier => identifier;

        public void Dispose()
        {
            foreach (DrawCallContainer dcc in subDrawCallContainers)
                dcc.Dispose();

            subDrawCallContainers.Clear();
            subDrawCallContainers = null;
            foreach (DrawCall dc in drawCalls)
                dc.Dispose();

            drawCalls.Clear();
            drawCalls = null;
        }

        private DrawCallContainer() { }
        internal DrawCallContainer(ContainerType containerType,int depth, int identifier)
        {
            this.identifier = identifier;
            subDrawCallContainers = new List<DrawCallContainer>();
            drawCalls = new List<DrawCall>();
            this.depth = depth;
            this.containerType = containerType;
        }

        internal void IssueDrawCalls()
        {
            foreach (DrawCall dc in drawCalls)
                dc.Perform();

            foreach (DrawCallContainer dcc in subDrawCallContainers)
                dcc.IssueDrawCalls();
        }

        DrawCallContainer FindSubContainer(int identifier)
        {
            return subDrawCallContainers.Find(dcc => dcc.identifier == identifier);
        }

        internal void AddDrawCall(DrawCall dc)
        {
            switch(containerType)  {
                case ContainerType.ShaderProgram:
                    {
                        if (dc.TextureBindings > 0)
                        {
                            //Find which SubContainer we should add to.
                            int id = dc.GetTextureBindingIdentifier(depth - 1);
                            DrawCallContainer dcc = FindSubContainer(id);
                            if (dcc != null)
                                dcc.AddDrawCall(dc);
                            else
                            {
                                dcc = new DrawCallContainer(ContainerType.Texture, depth + 1, id);
                                dcc.drawCalls.Add(dc);
                                subDrawCallContainers.Add(dcc);
                            }

                        } else
                        {
                            drawCalls.Add(dc);
                        }

                        break;
                    }
                case ContainerType.Texture:
                    {
                        if (dc.TextureBindings > depth + 1)
                        {
                            int id = dc.GetTextureBindingIdentifier(depth - 1);
                            DrawCallContainer dcc = FindSubContainer(id);
                            if (dcc == null)
                                dcc = new DrawCallContainer(ContainerType.Texture, depth + 1, id);

                            dcc.AddDrawCall(dc);
                            //Check if SubContainer exists, otherwise create it.
                        } else
                        {
                            drawCalls.Add(dc);
                        }
                        break;
                    }
            }
        }
    }

    internal static class DrawCallSorter
    {
        static List<DrawCallContainer> QueuedDrawCalls = new List<DrawCallContainer>();
        static DrawCall CurrentDrawCall; //The draw call that is currently being modified.

        internal static void SetCurrentDrawCall(DrawCall dc)
        {
            if (CurrentDrawCall != null)
                EnqueueCurrentDrawCall();

            CurrentDrawCall = dc;
        }

        

        internal static void FlushContainers()
        {
            foreach (DrawCallContainer dcc in QueuedDrawCalls)
                dcc.Dispose();

            QueuedDrawCalls.Clear();
            //throw new NotImplementedException();
            //Delete all containers
            //in the future we should store them instead. and only remove those that have changed.
            //otherwise we'll waste quite a bit of cpu performance on needlessly rebuilding the collection
            //And probably reuse objects aswell to cut down on the GC.
            //every single frame.
        }

        internal static void AddCommandToCurrent(IDrawCallCommand com)
        {
            CurrentDrawCall.AddCommand(com);
            //Get MeshBinding command sorted and DrawIndexedElements sorted out.
        }

        internal static void EnqueueCurrentDrawCall()
        {
            if (CurrentDrawCall != null)
            {
                DrawCallContainer dcc = QueuedDrawCalls.Find(t => t.Identifier == CurrentDrawCall.MaterialIdentifier);
                if (dcc == null)
                {
                    dcc = new DrawCallContainer(DrawCallContainer.ContainerType.ShaderProgram, 0, (int)CurrentDrawCall.MaterialIdentifier);
                    QueuedDrawCalls.Add(dcc);
                }

                dcc.AddDrawCall(CurrentDrawCall);
                CurrentDrawCall = null;
            }
        }

        internal static void IssueDrawCalls()
        {
            foreach (DrawCallContainer dcc in QueuedDrawCalls)
                dcc.IssueDrawCalls();
        }
    }
}
