#include "Textures.h"

using namespace S3DECore::Collections;

namespace S3DECore {
	namespace Graphics {
		namespace Textures {

			void TextureUnits::InitTextureUnits() {
				if (!isInitialized) {

					availableTextureUnits = S3DECore::Graphics::Renderer::GetParami(GL_MAX_COMBINED_TEXTURE_IMAGE_UNITS);
					//textureUnits = new vector<Texture^>(availableTextureUnits);
					//textureUnits = gcnew System::Collections::Generic::List<Texture^>(availableTextureUnits);

					textureUnits = gcnew cli::array<Texture^>(availableTextureUnits);
					unboundTexUnits = gcnew LinkedQueueList<int>();

					for (int i = 0; i < availableTextureUnits; i++)
						unboundTexUnits->Enqueue(i);

					boundTexUnits = gcnew LinkedQueueList<int>();
					isInitialized = true;
				}
			}

			Texture^ TextureUnits::GetTexture(uint texUnit) {
				return textureUnits[texUnit];
			}

			uint TextureUnits::BindTexture(Texture^ tex) {
				if (tex->IsBound())
					return tex->GetBoundTexUnit();

				return BindTexture(tex, GetAvailableTextureUnit());
			}

			void TextureUnits::UnbindTexture(Texture^ tex) {
				if (tex->IsBound())
					UnbindTexture(tex->GetBoundTexUnit());
			}

			void TextureUnits::UnbindTexture(uint texUnit) {

				Texture^ bt = GetTexture(texUnit);
				
				if (bt != nullptr) {
					Console::WriteLine(String::Format("Unbinding TextureUnit {0}",texUnit));
					bt->SetIsBound(false);
					boundTexUnits->TryRemove(texUnit);
					unboundTexUnits->Enqueue(texUnit);
					SetActiveTextureUnit(texUnit);
					glBindTexture((int)bt->GetTextureType(), NULL);
				}
			}

			uint TextureUnits::BindTexture(Texture^ tex, uint texUnit) {
				if (tex->IsBound())
					return tex->GetBoundTexUnit();

				Texture^ bt = GetTexture(texUnit);
				
				if (bt != nullptr) {
					bt->SetIsBound(false);
					bt->SetBoundTexUnit(0);
					boundTexUnits->TryRemove(texUnit);
				}
				else {
					unboundTexUnits->TryRemove(texUnit);
				}

				SetActiveTextureUnit(texUnit);
				Console::WriteLine(String::Format("Binding texture to TextureUnit {0}", texUnit));
				glBindTexture((int)tex->GetTextureType(), tex->GetIdentifier());
				textureUnits[texUnit] = tex;
				boundTexUnits->Enqueue(texUnit);
				tex->SetIsBound(true);
				tex->SetBoundTexUnit(texUnit);
				return texUnit;
			}

			uint TextureUnits::GetAvailableTextureUnit() {
				if (unboundTexUnits->Count() > 0)
					return unboundTexUnits->Dequeue();
				else {
					return boundTexUnits->Dequeue();
				}
			}

			uint TextureUnits::GetActiveTextureUnit() {
				return activeTextureUnit;
			}

			void TextureUnits::SetActiveTextureUnit(uint texUnit) {
				if (activeTextureUnit != texUnit) {
					activeTextureUnit = texUnit;
					glActiveTexture(GL_TEXTURE0 + texUnit);
				}
			}
		}
	}
}