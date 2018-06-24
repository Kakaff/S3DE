using S3DE.Engine.Data;
using S3DE.Engine.Graphics.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics
{
    public enum TextureUnit
    {
        Null = -1,
        _0,
        _1,
        _2,
        _3,
        _4,
        _5,
        _6,
        _7,
        _8,
        _9,
        _10,
        _11,
        _12,
        _13,
        _14,
        _15,
        _16,
        _17,
        _18,
        _19,
        _20,
        _21,
        _22,
        _23,
        _24,
        _25,
        _26,
        _27,
        _28,
        _29,
        _30,
        _31,
    }

    public static class TextureUnits
    {
        static ITexture[] Textures;
        static int _MaxSupportedTextureUnits;
        static LinkedQueueList<TextureUnit> UnboundTextureUnits, BoundTextureUnits;

        internal static void Initialize()
        {
            UnboundTextureUnits = new LinkedQueueList<TextureUnit>();
            BoundTextureUnits = new LinkedQueueList<TextureUnit>();
            _MaxSupportedTextureUnits = Renderer.TextureUnitCount;
            Textures = new ITexture[_MaxSupportedTextureUnits];

            for (int i = 0; i < _MaxSupportedTextureUnits; i++)
                UnboundTextureUnits.Enqueue((TextureUnit)i);

            Console.WriteLine($"GPU supports {_MaxSupportedTextureUnits} textureunits");
        }

        public static ITexture GetTextureUnit(TextureUnit texUnit)
        {
            if ((int)texUnit + 1 <= _MaxSupportedTextureUnits)
                return Textures[(int)texUnit];
            else
            {
                throw new ArgumentOutOfRangeException($"The GPU only supports {_MaxSupportedTextureUnits} texture units!");
            }
        }

        public static void BindTextureUnit(ITexture tex, TextureUnit texUnit)
        {
            if ((int)texUnit + 1 <= _MaxSupportedTextureUnits)
            {
                ITexture t = Textures[(int)texUnit];
                if (t == null || !t.Compare(tex))
                {
                    if (t == null)
                        UnboundTextureUnits.Remove(texUnit);
                    else
                    {
                        BoundTextureUnits.Remove(texUnit);
                        ITexture t2 = Textures[(int)texUnit];
                        t2.SetIsBound(false, TextureUnit.Null);
                    }

                    Renderer.BindTextureUnit(tex, texUnit);
                    Textures[(int)texUnit] = tex;
                    tex.SetIsBound(true, texUnit);
                    BoundTextureUnits.Enqueue(texUnit);
                }
            }
        }

        public static TextureUnit BindTexture(ITexture tex)
        {
            TextureUnit t;
            if (!tex.IsBound(out t))
            {
                if (UnboundTextureUnits.Count > 0)
                    t = UnboundTextureUnits.Dequeue();
                else
                    t = BoundTextureUnits.Dequeue();

                tex.Bind(t);
            }
            return t;
        }

        public static void BindTextures(params ITexture[] tex)
        {
            //Find multiple free textureunits.
            //or bind over other textureunits.
            //keep track of which ones we've already bound to so we don't overwrite the textures we are trying to bind.
            //Also skip binding if the texture is already bound.
        }

        public static void UnbindTextureUnit(TextureUnit texUnit)
        {
            if ((int)texUnit + 1 <= _MaxSupportedTextureUnits && Textures[(int)texUnit] != null)
            {
                Renderer.UnbindTextureUnit(texUnit);
                Textures[(int)texUnit].SetIsBound(false, TextureUnit.Null);
                Textures[(int)texUnit] = null;
                BoundTextureUnits.Remove(texUnit);
                UnboundTextureUnits.Enqueue(texUnit);
                Renderer.UnbindTextureUnit(texUnit);
            }
        }
    }
}
