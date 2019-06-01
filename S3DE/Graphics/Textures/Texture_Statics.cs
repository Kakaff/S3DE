using S3DE.Collections;
using System;

namespace S3DE.Graphics.Textures
{
    public abstract partial class Texture
    {
        static Texture activeTexture;
        static Texture[] boundTextures;
        static LinkedQueueList<int> boundTextureUnits;
        static LinkedQueueList<int> unboundTextureUnits;
        static int Max_TextureUnits;
        static bool textureUnitsInitialized;
        static uint activeTextureUnit;

        protected static uint ActiveTextureUnit => activeTextureUnit;

        static int GetAvailableTextureUnit()
        {
            int res = 0;
            if (unboundTextureUnits.Count > 0)
                res = unboundTextureUnits.Head;
            else
            {
                res = boundTextureUnits.Head;
            }
            
            return res;
        }

        internal static void SetActiveTextureUnit(uint TextureUnit)
        {
            if (activeTextureUnit != TextureUnit)
            {
                Console.WriteLine($"Setting TextureUnit : {TextureUnit} as the ActiveTextureUnit");
                Extern_SetActiveTextureUnit(TextureUnit);
                if (!Renderer.NoError)
                    throw new Exception("Error setting active texture unit!");
                activeTextureUnit = TextureUnit;
            }
        }

        internal static void Bind(Texture tex, uint TextureUnit)
        {
            if (tex.BoundTexUnit != TextureUnit)
            {
                Console.WriteLine($"Binding texture to TextureUnit {TextureUnit}");
                Extern_BindTexture(tex.Handle, TextureUnit);
                if (!Renderer.NoError)
                    Console.WriteLine("Error binding texture!");
                Console.WriteLine($"TextureUnit : {TextureUnit} is now the ActiveTextureUnit");
                activeTextureUnit = TextureUnit;
                SetIsBound(tex, TextureUnit);
            }
            
        }
        
        internal static int Bind(Texture tex)
        {
            if (!tex.IsBound)
                Bind(tex, (uint)GetAvailableTextureUnit());

            return tex.BoundTexUnit;
        }

        internal static void InitTextures()
        {
            if (!textureUnitsInitialized)
            {
                Extern_GLGeti(GL.MAX_COMBINED_TEXTURE_IMAGE_UNITS,out Max_TextureUnits);
                if (!Renderer.NoError)
                    throw new Exception("Error getting TextureUnits!");
                boundTextures = new Texture[Max_TextureUnits];
                unboundTextureUnits = new LinkedQueueList<int>();
                boundTextureUnits = new LinkedQueueList<int>();

                Console.WriteLine($"GPU supports {Max_TextureUnits} texture units");
                for (int i = 0; i < Max_TextureUnits; i++)
                    unboundTextureUnits.Enqueue(i);
            }
        }

        internal static void SetIsBound(Texture tex, uint TextureUnit)
        {
            if (tex.IsBound && tex.BoundTexUnit != TextureUnit)
            {
                unboundTextureUnits.Enqueue(tex.BoundTexUnit);
                boundTextures[tex.BoundTexUnit] = null;
                boundTextureUnits.Remove(tex.BoundTexUnit);
            }
            else if (tex.IsBound && tex.BoundTexUnit == TextureUnit)
                return;

            if (boundTextures[TextureUnit] != null)
            {
                Texture t = boundTextures[TextureUnit];
                t.IsBound = false;
                t.BoundTexUnit = -1;
                boundTextureUnits.Remove((int)TextureUnit);
            }
            else
                unboundTextureUnits.Remove((int)TextureUnit);

            tex.IsBound = true;
            tex.BoundTexUnit = (int)TextureUnit;
            boundTextures[TextureUnit] = tex;
            boundTextureUnits.Enqueue((int)TextureUnit);
        }
    }
}
