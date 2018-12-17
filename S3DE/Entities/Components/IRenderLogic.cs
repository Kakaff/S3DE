namespace S3DE.Engine.Entities.Components
{
    public interface IRenderLogic
    {
        void PreRender();
        void Render();
        void PostRender();
    }
}
