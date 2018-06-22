using S3DE.Engine.Graphics;
using S3DE.Engine.Graphics.Textures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static S3DE.Engine.Enums;

namespace S3DE.Engine.IO
{
    public static class ImageLoader
    {
        public static Texture2D LoadFromFile(string path)
        {
            Bitmap bmp = (Bitmap)Image.FromFile(path);
            
            Texture2D tex = Texture2D.Create(bmp.Width, bmp.Height);

            tex.FilterMode = FilterMode.Trilinear;
            tex.PixelFormat = PixelFormat.RGBA;
            tex.PixelType = PixelType.UByte;
            tex.InternalFormat = InternalFormat.RGB16F;
            tex.CalculateMipMapCount();
            tex.AnisotropicSamples = AnisotropicSamples.x16;
            for (int x = 0; x < bmp.Width; x++)
                for (int y = 0; y < bmp.Width; y++)
                    tex.SetPixel(x, y, bmp.GetPixel(x, y));
            
            tex.Apply();
            bmp.Dispose();
            return tex;
        }

        
    }
}
