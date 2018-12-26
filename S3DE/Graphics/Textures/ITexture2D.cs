namespace S3DE.Graphics.Textures
{
    public interface ITexture2D : IRenderTexture
    {
        void SetPixel(int x, int y, Color c);
        Color GetPixel(int x, int y);
    }
}
