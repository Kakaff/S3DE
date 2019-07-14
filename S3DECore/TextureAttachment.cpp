#include "Framebuffer.h"

namespace S3DECore {
	namespace Graphics {
		namespace Framebuffers {

			Texture^ TextureAttachment::GetTexture() {
				return tex;
			}

			TextureAttachment::TextureAttachment(Texture^ tex) {
				this->tex = tex;
			}

			TextureAttachment2D::TextureAttachment2D(RenderTexture2D^ tex) : TextureAttachment(tex) { }
		}
	}
}