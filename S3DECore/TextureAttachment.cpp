#include "Framebuffer.h"

namespace S3DECore {
	namespace Graphics {
		namespace Framebuffers {

			Texture^ TextureAttachment::GetTexture() {
				return tex;
			}

			
			TextureAttachment::TextureAttachment(Texture^ tex) :
				FramebufferAttachment(tex->Resolution,tex->Pixeltype,tex->Pixelformat,tex->Internalformat) {
				this->tex = tex;
			}

			TextureAttachment2D::TextureAttachment2D(Vector2 res, PixelType pt, PixelFormat pf, InternalFormat itf) 
				: TextureAttachment(gcnew RenderTexture2D(pt,pf,itf,(uint)res.x,(uint)res.y)) {
			
			}
		}
	}
}