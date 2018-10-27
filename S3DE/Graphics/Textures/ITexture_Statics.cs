using S3DE.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Graphics.Textures
{
    public abstract partial class ITexture
    {
        static ITexture[] boundTextures;
        static LinkedQueueList<int> boundTextureUnits;
        static LinkedQueueList<int> unboundTextureUnits;
        static int Max_TextureUnits;
        public static int ActiveTextureUnit { get; private set; }
        static bool textureUnitsInitialized;

        static int GetAvailableTextureUnit()
        {
            int res = 0;
            if (unboundTextureUnits.Count > 0)
                res = unboundTextureUnits.Dequeue();
            else
            {
                res = boundTextureUnits.Dequeue();
                boundTextures[res].IsBound = false;
                boundTextures[res].BoundTexUnit = -1;
                boundTextures[res] = null;
            }
            
            return res;
        }

        internal static void SetActiveTexture(ITexture tex)
        {
            Extern_SetActiveTexture((uint)tex.BoundTexUnit);
            ActiveTextureUnit = tex.BoundTexUnit;
        }

        internal static int Bind(ITexture tex)
        {
            if (!tex.IsBound)
            {
                
                int texUnit = GetAvailableTextureUnit();
                Console.WriteLine($"Binding TextureUnit to {texUnit}");
                Extern_BindTexture(tex.Handle, (uint)texUnit);
                boundTextures[texUnit] = tex;
                tex.BoundTexUnit = texUnit;
                tex.IsBound = true;
                boundTextureUnits.Enqueue(texUnit);
            }

            return tex.BoundTexUnit;
        }

        internal static void UnBind(ITexture tex)
        {
            Extern_BindTexture(IntPtr.Zero, (uint)tex.BoundTexUnit);
            tex.IsBound = false;
            boundTextures[tex.BoundTexUnit] = null;
            unboundTextureUnits.Enqueue(tex.BoundTexUnit);
            tex.BoundTexUnit = -1;
            
        }

        internal static void InitTextures()
        {
            if (!textureUnitsInitialized)
            {
                Extern_GLGeti(GL.MAX_COMBINED_TEXTURE_IMAGE_UNITS,out Max_TextureUnits);
                boundTextures = new ITexture[Max_TextureUnits];
                unboundTextureUnits = new LinkedQueueList<int>();
                boundTextureUnits = new LinkedQueueList<int>();

                Console.WriteLine($"GPU supports {Max_TextureUnits} texture units");
                for (int i = 0; i < Max_TextureUnits; i++)
                    unboundTextureUnits.Enqueue(i);
            }
        }
    }
}
