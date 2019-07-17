#include "Textures.h"

namespace S3DECore {
	namespace Graphics {
		namespace Textures {

			RenderTexture2D::RenderTexture2D(PixelType pt, PixelFormat pf, InternalFormat itf, uint width, uint height) :
				Texture(TextureType::Texture2D, Vector2(width,height),pt,pf,itf) {
				this->width = width;
				this->height = height;
				pixType = pt;
				pixFrmt = pf;
				intTexFrmt = itf;
				anisoSamples = AnisotropicSamples::x1;
				filterMode = FilterMode::Nearest;
				wrapMode = WrapMode::None;
				InitTexture();
			}

			void RenderTexture2D::OnApply() {
				if (genMipMaps) {
					GenerateMipMaps();
					hasMipMaps = true;
				}

				if (wrapModeChanged) {
					switch (wrapMode) {
						case WrapMode::None: {
							SetTexParameteri(GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
							SetTexParameteri(GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
							break;
						}
					}
					wrapModeChanged = false;
				}
				if (filterModeChanged) {
					if (hasMipMaps) {
						switch (filterMode) {
						case FilterMode::Nearest: {
							SetTexParameteri(GL_TEXTURE_MIN_FILTER, GL_NEAREST_MIPMAP_NEAREST);
							SetTexParameteri(GL_TEXTURE_MAG_FILTER, GL_NEAREST);
							break;
						}
						case FilterMode::BiLinear: {
							SetTexParameteri(GL_TEXTURE_MIN_FILTER, GL_NEAREST_MIPMAP_LINEAR);
							SetTexParameteri(GL_TEXTURE_MAG_FILTER, GL_LINEAR);
							break;
						}
						case FilterMode::TriLinear: {
							SetTexParameteri(GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
							SetTexParameteri(GL_TEXTURE_MAG_FILTER, GL_LINEAR);
							break;
						}
						}
					}
					else {
						switch (filterMode) {
						case FilterMode::Nearest: {
							SetTexParameteri(GL_TEXTURE_MIN_FILTER, GL_NEAREST);
							SetTexParameteri(GL_TEXTURE_MAG_FILTER, GL_NEAREST);
							break;
						}
						case FilterMode::BiLinear: {
							SetTexParameteri(GL_TEXTURE_MIN_FILTER, GL_NEAREST);
							SetTexParameteri(GL_TEXTURE_MAG_FILTER, GL_LINEAR);
							break;
						}
						case FilterMode::TriLinear: {
							SetTexParameteri(GL_TEXTURE_MIN_FILTER, GL_LINEAR);
							SetTexParameteri(GL_TEXTURE_MAG_FILTER, GL_LINEAR);
							break;
						}
						}
					}
					filterModeChanged = false;
				}

				if (anisoChanged) {
					float samples = 1;
					switch (anisoSamples) {
					case AnisotropicSamples::x1: samples = 1; break;
					case AnisotropicSamples::x2: samples = 2; break;
					case AnisotropicSamples::x4: samples = 4; break;
					case AnisotropicSamples::x8: samples = 8; break;
					case AnisotropicSamples::x16: samples = 16; break;
					}
					SetTexParameterf(GL_TEXTURE_MAX_ANISOTROPY, samples);
					anisoChanged = false;
				}
				
			}

			void RenderTexture2D::UploadPixelData() {

			}

			void RenderTexture2D::GenerateMipMaps() {
				glGenerateMipmap(GL_TEXTURE_2D);
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
				OnApply();
				Unbind();
			}
		}
	}
}