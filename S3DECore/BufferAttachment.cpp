#include "Framebuffer.h"

namespace S3DECore {
	namespace Graphics {
		namespace Framebuffers {

			BufferAttachment::BufferAttachment(Vector2 res, PixelType pt, PixelFormat pf,InternalFormat itf) : FramebufferAttachment(res,pt,pf,itf) {

			}
		}
	}
}