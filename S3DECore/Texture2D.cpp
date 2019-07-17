#include "Textures.h"

namespace S3DECore {
	namespace Graphics {
		namespace Textures {

			Texture2D::Texture2D(uint width, uint height) :
				RenderTexture2D(PixelType::UNSIGNED_BYTE, PixelFormat::RGBA, InternalFormat::RGBA, width, height) {
				bytes = gcnew cli::array<uint8_t>((width * height) * 4);
			}

			void Texture2D::UploadPixelData() {
				pin_ptr<uint8_t> pxData = &bytes[0];
				glTexImage2D(
					(int)GetTextureType(),
					0,
					(int)intTexFrmt,
					width,
					height,
					0,
					(int)pixFrmt,
					(int)pixType,
					pxData
				);

				hasMipMaps = false;
			}
		}
	}
}