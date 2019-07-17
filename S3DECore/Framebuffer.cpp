#include "Framebuffer.h"
#include "GL\glew.h"

namespace S3DECore {
	namespace Graphics {
		namespace Framebuffers {

			int AttachmentLocToIndex(AttachmentLocation loc) {
				switch (loc) {
				case AttachmentLocation::Color0: return 0;
				case AttachmentLocation::Color1: return 1;
				case AttachmentLocation::Color2: return 2;
				case AttachmentLocation::Color3: return 3;
				case AttachmentLocation::Color4: return 4;
				case AttachmentLocation::Color5: return 5;
				case AttachmentLocation::Color6: return 6;
				case AttachmentLocation::Color7: return 7;
				case AttachmentLocation::Color8: return 8;
				case AttachmentLocation::Color9: return 9;
				case AttachmentLocation::Color10: return 10;
				case AttachmentLocation::Color11: return 11;
				case AttachmentLocation::Color12: return 12;
				case AttachmentLocation::Color13: return 13;
				case AttachmentLocation::Color14: return 14;
				case AttachmentLocation::Color15: return 15;
				case AttachmentLocation::Depth: return 16;
				case AttachmentLocation::Depth_Stencil: return 16;
				default: return -1;
				}
			}

			Framebuffer::Framebuffer() {
				pin_ptr<uint> ID(&identifier);
				glGenFramebuffers(1, ID);
				attachments = gcnew cli::array<FramebufferAttachment^>(17);
				instanceID = instanceCntr;
				instanceCntr++;
				isBound = false;
			}

			Framebuffer::!Framebuffer() {

			}

			Framebuffer::~Framebuffer() {

			}

			bool Framebuffer::IsBound() { return isBound; }

			int Framebuffer::GetID() { return instanceID; }

			FramebufferAttachment^ Framebuffer::GetAttachment(AttachmentLocation loc) {
				return attachments[AttachmentLocToIndex(loc)];
			}

			uint Framebuffer::GetIdentifier() {
				return identifier;
			}

			void Framebuffer::Bind() {
				if (!isBound) {
					glBindFramebuffer(GL_FRAMEBUFFER, identifier);
					Renderer::CheckErrors(gcnew String("Tried binding framebuffer"));
					isBound = true;
					if (boundFramebuffer == nullptr)
						boundFramebuffer = this;
					else {
						boundFramebuffer->isBound = false;
						boundFramebuffer = this;
					}

					Renderer::SetViewportSize(0, 0, Renderer::RenderResolution);
				}
			}

			void Framebuffer::Unbind() {
				if (isBound) {
					glBindFramebuffer(GL_FRAMEBUFFER, 0);
					Renderer::CheckErrors(gcnew String("Tried unbinding framebuffer"));
					isBound = false;
					boundFramebuffer = nullptr;
					Renderer::SetViewportSize(0, 0, Renderer::DisplayResolution);
				}
			}

			bool Framebuffer::IsComplete() {
				if (!isBound)
					Bind();
				return glCheckFramebufferStatus(GL_FRAMEBUFFER) == GL_FRAMEBUFFER_COMPLETE;
			}

			Framebuffer^ Framebuffer::GetBoundFramebuffer() {
				return boundFramebuffer;
			}

			void Framebuffer::AddTextureAttachment2D(TextureAttachment2D^ attachment, AttachmentLocation loc) {
				if (!isBound)
					Bind();

				FramebufferAttachment^ tex = attachments[AttachmentLocToIndex(loc)];

				if (tex != nullptr)
					throw gcnew System::ArgumentException(String::Format("Framebuffer {0} already has a {1} attachment!",instanceID,loc));

				Console::WriteLine(String::Format("Attaching Texture2D {0} to Framebuffer {1} as {2} : {3}",attachment->GetTexture()->GetInstanceID(),instanceID, loc,(int)loc));
				attachment->GetTexture()->Unbind();

				glFramebufferTexture2D(
					GL_FRAMEBUFFER, 
					(GLenum)loc, 
					GL_TEXTURE_2D, 
					attachment->GetTexture()->GetIdentifier(), 
					0
				);

				Renderer::CheckErrors(String::Format("Tried attaching texture to location {0}",loc));

				attachments[AttachmentLocToIndex(loc)] = attachment;
			}

			void Framebuffer::AddBufferAttachment(BufferAttachment^ ba, AttachmentLocation loc) {

			}
		}
	}
}