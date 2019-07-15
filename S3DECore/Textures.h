#pragma once
#include "Renderer.h"
#include "Collections.h"
#include "EngineTypes.h"
#include <vector>

using namespace std;
using namespace S3DECore::Collections;


namespace S3DECore {
	namespace Graphics {

		public value class Color {
		public:
			Color(uint8_t red, uint8_t green, uint8_t blue, uint8_t alpha) {
				r = red;
				g = green;
				b = blue;
				a = alpha;
			}
			property uint8_t Red {
				uint8_t get() { return r; }
				void set(uint8_t v) { r = v; }
			}
			property uint8_t Green {
				uint8_t get() { return g; }
				void set(uint8_t v) { g = v; }
			}
			property uint8_t Blue {
				uint8_t get() { return b; }
				void set(uint8_t v) { b = v; }
			}
			property uint8_t Alpha {
				uint8_t get() { return a; }
				void set(uint8_t v) { a = v; }
			}
			property uint32_t AsUInt32 {
				uint32_t get() { return (r << 24) + (g << 16) + (b << 8) + a; }
				void set(uint32_t v) { 
					r = v >> 24;
					g = (v >> 16) & 0xFF;
					b = (v >> 8) & 0xFF;
					a = v & 0xFF;
				}
			}
			virtual String^ ToString() override {
				return String::Format("({0},{1},{2},{3})", r, g, b,a);
			}
		private:
			uint8_t r, g, b, a;
		};

		namespace Textures {

			public enum class AnisotropicSamples : int {
				x1,
				x2,
				x4,
				x8,
				x16
			};

			public enum class TextureParameter : int
			{
				DEPTH_STENCIL_TEXTURE_MODE = 0x90EA,
				TEXTURE_MIN_LOD = 0x813A,
				TEXTURE_MAX_LOD = 0x813B,
				TEXTURE_BASE_LEVEL = 0x813C,
				TEXTURE_MAX_LEVEL = 0x813D,
				TEXTURE_COMPARE_MODE = 0x884C,
				TEXTURE_COMPARE_FUNC = 0x884D,
				TEXTURE_LOD_BIAS = 0x8501,
				TEXTURE_MAG_FILTER = 0x2800,
				TEXTURE_MIN_FILTER = 0x2801,
				TEXTURE_SWIZZLE_R = 0x8E42,
				TEXTURE_SWIZZLE_G = 0x8E43,
				TEXTURE_SWIZZLE_B = 0x8E44,
				TEXTURE_SWIZZLE_A = 0x8E45,
				TEXTURE_SWIZZLE_RGBA = 0x8E46,
				TEXTURE_WRAP_S = 0x2802,
				TEXTURE_WRAP_T = 0x2803,
				TEXTURE_WRAP_R = 0x8072,
				TEXTURE_BORDER_COLOR = 0x1004,
				TEXTURE_MAX_ANISOTROPY = 0x84FE,
			};

			public enum class WrapMode : int {
				None,
				Clamp,
				Wrap,
				
			};

			public enum class FilterMode : int {
				Nearest,
				BiLinear,
				TriLinear,
			};

			public enum class InternalFilterMode : int {
				NEAREST = 0x2600,
				LINEAR = 0x2601,
				NEAREST_MIPMAP_NEAREST = 0x2700,
				LINEAR_MIPMAP_NEAREST = 0x2701,
				NEAREST_MIPMAP_LINEAR = 0x2702,
				LINEAR_MIPMAP_LINEAR = 0x2703,
			};

			public enum class TextureType : int {
				Texture1D = GL_TEXTURE_1D,
				Texture2D = GL_TEXTURE_2D,
				Texture3D = GL_TEXTURE_3D,

				Texture_Rectangle = GL_TEXTURE_RECTANGLE,
				Texture_Buffer = GL_TEXTURE_BUFFER,
				Texture_CubeMap = GL_TEXTURE_CUBE_MAP,

				Texture1D_Array = GL_TEXTURE_1D_ARRAY,
				Texture2D_Array = GL_TEXTURE_2D_ARRAY,
				Texture_CubeMap_Array = GL_TEXTURE_CUBE_MAP_ARRAY,

				Texture2D_MultiSample = GL_TEXTURE_2D_MULTISAMPLE,
				Texture2D_MultiSample_Array = GL_TEXTURE_2D_MULTISAMPLE_ARRAY
			};

			public enum class PixelFormat : int {
				RED = 0x1903,
				RG = 0x8227,
				RGB = 0x1907,
				RGBA = 0x1908,
				DEPTH_COMPONENT = 0x1902,
				DEPTH_STENCIL = 0x84F9
			};

			public enum class PixelType : int {
				BYTE = 0x1400,
				UNSIGNED_BYTE = 0x1401,
				SHORT = 0x1402,
				UNSIGNED_SHORT = 0x1403,
				INT = 0x1404,
				UNSIGNED_INT = 0x1405,
				FLOAT = 0x1406,

				UNSIGNED_INT_24_8 = 0x84FA,
				UNSIGNED_BYTE_3_3_2 = 0x8032,
				UNSIGNED_SHORT_4_4_4_4 = 0x8033,
				UNSIGNED_SHORT_5_5_5_1 = 0x8034,
				UNSIGNED_INT_8_8_8_8 = 0x8035,
				UNSIGNED_INT_10_10_10_2 = 0x8036,
				UNSIGNED_BYTE_2_3_3_REV = 0x8362,
				UNSIGNED_SHORT_5_6_5 = 0x8363,
				UNSIGNED_SHORT_5_6_5_REV = 0x8364,
				UNSIGNED_SHORT_4_4_4_4_REV = 0x8365,
				UNSIGNED_SHORT_1_5_5_5_REV = 0x8366,
				UNSIGNED_INT_8_8_8_8_REV = 0x8367
			};

			public enum class InternalTextureFormat : int {
				RED = 0x1903,
				RG = 0x8227,
				RGB = 0x1907,
				RGBA = 0x1908,
				DEPTH_COMPONENT = 0x1902,
				DEPTH_STENCIL = 0x84F9,
				STENCIL_INDEX = 0x1901,
				STENCIL_INDEX1 = 0x8D46,
				STENCIL_INDEX4 = 0x8D47,
				STENCIL_INDEX8 = 0x8D48,
				STENCIL_INDEX16 = 0x8D49,
				DEPTH24_STENCIL8 = 0x88F0,

				R8 = 0x8229,
				R16 = 0x822A,
				RG8 = 0x822B,
				RG16 = 0x822C,
				RGB4 = 0x804F,
				RGB5 = 0x8050,
				RGB8 = 0x8051,
				RGB10 = 0x8052,
				RGB12 = 0x8053,
				RGB16 = 0x8054,
				RGBA2 = 0x8055,
				RGBA4 = 0x8056,
				RGB5_A1 = 0x8057,
				RGBA8 = 0x8058,
				RGB10_A2 = 0x8059,
				RGBA12 = 0x805A,
				RGBA16 = 0x805B,

				R16F = 0x822D,
				R32F = 0x822E,
				RG16F = 0x822F,
				RG32F = 0x8230,
				RGB16F = 0x881B,
				RGB32F = 0x8815,
				RGBA16F = 0x881A,
				RGBA32F = 0x8814,

				R8I = 0x8231,
				R16I = 0x8233,
				R32I = 0x8235,
				RG8I = 0x8237,
				RG16I = 0x8239,
				RG32I = 0x823B,
				RGB8I = 0x8D8F,
				RGB16I = 0x8D89,
				RGB32I = 0x8D83,
				RGBA8I = 0x8D8E,
				RGBA16I = 0x8D88,
				RGBA32I = 0x8D82,

				R8UI = 0x8232,
				R16UI = 0x8234,
				R32UI = 0x8236,
				RG8UI = 0x8238,
				RG16UI = 0x823A,
				RG32UI = 0x823C,
				RGB8UI = 0x8D7D,
				RGB16UI = 0x8D77,
				RGB32UI = 0x8D71,
				RGBA8UI = 0x8D7C,
				RGBA16UI = 0x8D76,
				RGBA32UI = 0x8D70
			};

			ref class Texture;
			private ref class TextureUnits abstract sealed {
			public:
				static void InitTextureUnits();
				static uint BindTexture(Texture^ tex);
				static uint BindTexture(Texture^ tex, uint texUnit);
				static Texture^ GetTexture(uint texUnit);
				static void UnbindTexture(Texture^ tex);
				static void UnbindTexture(uint textureUnit);
				static uint GetActiveTextureUnit();
				static void SetActiveTextureUnit(uint texUnit);
			private:
				static uint availableTextureUnits;
				static uint activeTextureUnit;
				static bool isInitialized;
				static uint GetAvailableTextureUnit();
				static cli::array<Texture^>^ textureUnits;
				static LinkedQueueList<int> ^boundTexUnits, ^unboundTexUnits;
			};

			public ref class Texture abstract {
			public:
				~Texture();
				!Texture();
				void SetIsBound(bool b);
				bool IsBound();
				uint GetIdentifier();
				uint GetBoundTexUnit();
				uint GetInstanceID();
				uint Bind(uint textureUnit);
				uint Bind(); 
				void Unbind();
				void Apply();
				TextureType GetTextureType();
			internal:
				void SetBoundTexUnit(uint tu);
			protected:
				Texture(TextureType texType);
				virtual void OnApply() abstract;
				virtual void UploadPixelData() abstract;
				void SetTexParameteri(int param, int val);
				void SetTexParameterf(int param, float val);
				FilterMode filterMode;
				WrapMode wrapMode;
				AnisotropicSamples anisoSamples;
				bool filterModeChanged,wrapModeChanged,anisoChanged;
			private:
				uint gl_TexIdentifier;
				uint boundTexUnit;
				uint instanceID;
				TextureType target;
				bool isBound;
				static uint instanceCntr;
			};

			public ref class RenderTexture2D : public Texture {
			public:
				RenderTexture2D(PixelType pt, PixelFormat pf, InternalTextureFormat itf, uint width, uint height);
				~RenderTexture2D();
				!RenderTexture2D();
				uint GetWidth();
				uint GetHeight();
				property WrapMode Wrap {
					WrapMode get() {
						return wrapMode;
					}
					void set(WrapMode wm) {
						if (wm != wrapMode) {
							wrapMode = wm;
							wrapModeChanged = true;
						}
					}
				}

				property FilterMode Filter {
					FilterMode get() {
						return filterMode;
					}
					void set(FilterMode fm) {
						if (fm != filterMode) {
							filterMode = fm;
							filterModeChanged = true;
						}
					}
				}

				property AnisotropicSamples Anisotropic {
					AnisotropicSamples get() {
						return anisoSamples;
					}
					void set(AnisotropicSamples as) {
						if (as != anisoSamples) {
							anisoSamples = as;
							anisoChanged = true;
						}
					}
				}

				property bool AutoGenerateMipMaps {
					void set(bool b) {
						genMipMaps = b;
					}
				}

				void GenerateMipMaps();
			protected:
				uint width;
				uint height;
				PixelType pixType;
				PixelFormat pixFrmt;
				InternalTextureFormat intTexFrmt;
				virtual void UploadPixelData() override;
				virtual void OnApply() override;
				bool genMipMaps;
				bool hasMipMaps;
			private:
				void InitTexture();
			};

			public ref class Texture2D sealed : public RenderTexture2D {
			public:
				Texture2D(uint width, uint height);

				property Color default[int, int]{ //Shows as error, but there's no error, still compiles and runs fine.
					Color get(int x, int y) {
						int pos = ((y * width) + x);
						switch (pixFrmt) {
						case PixelFormat::RGBA: {
								pos *= 4;
								return Color(bytes[pos], bytes[pos + 1], bytes[pos + 2], bytes[pos + 3]);
							}
						default: throw gcnew System::ArgumentOutOfRangeException("PixelFormat not supported");
						}
					}
					void set(int x, int y, Color c) {
						int pos = ((y * width) + x);
						switch (pixFrmt) {
						case PixelFormat::RGBA: {
							pos *= 4;
							bytes[pos] = c.Red;
							bytes[pos + 1] = c.Green;
							bytes[pos + 2] = c.Blue;
							bytes[pos + 3] = c.Alpha;
							break;
						}
						default: throw gcnew System::ArgumentOutOfRangeException("PixelFormat not supported");
						}
					}
				}
			protected:
				virtual void UploadPixelData() override;
			private:
				cli::array<uint8_t>^ bytes;
			};
		}
	}
}