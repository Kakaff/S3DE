namespace S3DE.Components
{
    public interface IUpdateLogic
    {
        void EarlyUpdate();
        void Update();
        void LateUpdate();
    }
}
