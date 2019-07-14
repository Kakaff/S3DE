#include "Textures.h"

namespace S3DECore {
	namespace Graphics {
		namespace Textures {

			RenderTexture2D::RenderTexture2D(PixelType pt, PixelFormat pf, InternalTextureFormat itf, uint width, uint height) :
				Texture(TextureType::Texture2D) {
				this->width = width;
				this->height = height;
				pixType = pt;
				pixFrmt = pf;
				intTexFrmt = itf;
				InitTexture();
			}

			void RenderTexture2D::OnApply() {

			}

			void RenderTexture2D::UploadPixelData() {

			}

			RenderTexture2D::!RenderTexture2D() {
				
			}

			RenderTexture2D::~RenderTexture2D() {

			}

			uint RenderTexture2D::GetWidth() { return width; }
			uint RenderTexture2D::GetHeight() { return height; }

			void RenderTexture2D::InitTexture() {
				Bind();
				System::Console::WriteLine(String::Format("Initializing texture, Resolution: {0} * {1}", width, height));
				glTexImage2D(
					(int)GetTextureType(),
					0,  //Level
					(int)intTexFrmt,
					width,
					height,
					0, //Border
					(int)pixFrmt,
					(int)pixType,
					NULL //Texture Data
				);
			}
		}
	}
}