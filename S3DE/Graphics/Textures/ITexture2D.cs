namespace S3DE.Graphics.Textures
{
    public interface ITexture2D
    {
        int Width {get;}
        int Height {get;}
        int MipmapCount {get;}

        PixelFormat PixelFormat {get;}
        InternalFormat InternalFormat {get;}
        int BoundTexUnit {get;}
        bool IsBound { get;}
        int Bind();
    }
}
