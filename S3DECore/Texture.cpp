#include "Textures.h"

namespace S3DECore {
	namespace Graphics {
		namespace Textures {

			void Texture::SetTexParameterf(int param, float val) {
				if (!isBound)
					Bind();
				else if (TextureUnits::GetActiveTextureUnit() != boundTexUnit)
					TextureUnits::SetActiveTextureUnit(boundTexUnit);

				glTexParameterf((int)target, param, val);
			}

			void Texture::SetTexParameteri(int param, int val) {
				if (!isBound)
					Bind();
				else if (TextureUnits::GetActiveTextureUnit() != boundTexUnit)
					TextureUnits::SetActiveTextureUnit(boundTexUnit);

				glTexParameteri((int)target, param, val);
			}

			void Texture::SetIsBound(bool b) {
				isBound = b;
			}

			uint Texture::GetIdentifier() {
				return gl_TexIdentifier;
			}

			uint Texture::GetInstanceID() {
				return instanceID;
			}

			uint Texture::GetBoundTexUnit() {
				return boundTexUnit;
			}

			void Texture::SetBoundTexUnit(uint tu) {
				boundTexUnit = tu;
			}

			TextureType Texture::GetTextureType() {
				return target;
			}

			bool Texture::IsBound() {
				return isBound;
			}

			void Texture::Apply() {
				if (!isBound)
					Bind();
				else if (TextureUnits::GetActiveTextureUnit() != boundTexUnit)
					TextureUnits::SetActiveTextureUnit(boundTexUnit);

				UploadPixelData();
				OnApply();
			}

			Texture::Texture(TextureType texType,Vector2 res, PixelType pt, PixelFormat pf, InternalFormat itf) {
				target = texType;
				pin_ptr<uint> id(&gl_TexIdentifier);
				glGenTextures(1, id);
				Console::WriteLine(String::Format("Created Texture {0}",gl_TexIdentifier));
				id = nullptr;
				instanceID = instanceCntr;
				instanceCntr++;

				wrapModeChanged = true;
				anisoChanged = true;
				filterModeChanged = true;
				this->res = res;
				this->pt = pt;
				this->pf = pf;
				this->itf = itf;
			}

			uint Texture::Bind() {
				if (!isBound)
					TextureUnits::BindTexture(this);

				return boundTexUnit;
			}

			uint Texture::Bind(uint texUnit) {
				if (isBound)
					Unbind();

				TextureUnits::BindTexture(this, texUnit);

				return boundTexUnit;
			}

			Texture::!Texture() {}

			Texture::~Texture() {}

			
			void Texture::Unbind() {
				if (isBound) {
					TextureUnits::UnbindTexture(this);
				}
			}
		}
	}
}