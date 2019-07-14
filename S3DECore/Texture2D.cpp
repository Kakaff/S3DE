#include "Textures.h"

namespace S3DECore {
	namespace Graphics {
		namespace Textures {

			Texture2D::Texture2D(PixelType pt, PixelFormat pf, InternalTextureFormat itf, uint width, uint height) :
				RenderTexture2D(pt, pf, itf, width, height){

			}

			/*
			void Texture2D::SetTextureData(cli::array<uint8_t>^ textureData) {
				bytes->clear();
				bytes->resize(textureData->Length);
				System::Runtime::InteropServices::Marshal::Copy(
					textureData, 
					0, 
					(System::IntPtr)&bytes[0], 
					textureData->Length
				);
			}
			*/

			void Texture2D::SetPixel(int x, int y,Color c) {

			}

			void Texture2D::OnApply() {

			}
			void Texture2D::UploadPixelData() {
				if (!IsBound())
					Bind();
				else if (TextureUnits::GetActiveTextureUnit() != GetBoundTexUnit())
					TextureUnits::SetActiveTextureUnit(GetBoundTexUnit());

				glTexImage2D(
					(int)GetTextureType(),
					0,
					(int)intTexFrmt,
					width,
					height,
					0,
					(int)pixFrmt,
					(int)pixType,
					&bytes[0]
				);
			}
		}
	}
}