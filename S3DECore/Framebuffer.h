#pragma once
#include "Framebuffer.h"
#include "Textures.h"

using namespace S3DECore::Graphics::Textures;

namespace S3DECore {
	namespace Graphics {
		namespace Framebuffers {
			public ref class TextureAttachment abstract {
			public:
				Texture^ GetTexture();
			protected:
				TextureAttachment(Texture^ tex);
				Texture^ tex;
			};

			public ref class TextureAttachment2D : TextureAttachment {
			public:
				TextureAttachment2D(RenderTexture2D^ rt);
			private:
			};

			public enum class AttachmentLocation {
				Color0 = GL_COLOR_ATTACHMENT0,
				Color1 = GL_COLOR_ATTACHMENT1,
				Color2 = GL_COLOR_ATTACHMENT2,
				Color3 = GL_COLOR_ATTACHMENT3,
				Color4 = GL_COLOR_ATTACHMENT4,
				Color5 = GL_COLOR_ATTACHMENT5,
				Color6 = GL_COLOR_ATTACHMENT6,
				Color7 = GL_COLOR_ATTACHMENT7,
				Color8 = GL_COLOR_ATTACHMENT8,
				Color9 = GL_COLOR_ATTACHMENT9,
				Color10 = GL_COLOR_ATTACHMENT10,
				Color11 = GL_COLOR_ATTACHMENT11,
				Color12 = GL_COLOR_ATTACHMENT12,
				Color13 = GL_COLOR_ATTACHMENT13,
				Color14 = GL_COLOR_ATTACHMENT14,
				Color15 = GL_COLOR_ATTACHMENT15,

				Depth = GL_DEPTH_ATTACHMENT,
				Depth_Stencil = GL_DEPTH_STENCIL_ATTACHMENT
			};

			public ref class Framebuffer {
			public:
				Framebuffer();
				~Framebuffer();
				!Framebuffer();
				uint GetIdentifier();
				void Bind();
				void Unbind();
				bool IsComplete();
				bool IsBound();
				int GetID();
				static Framebuffer^ GetBoundFramebuffer();
				TextureAttachment^ GetTextureAttachment(AttachmentLocation loc);
				void AddTextureAttachment2D(TextureAttachment2D^ attachment,AttachmentLocation loc, int level);
			private:
				int instanceID;
				bool isBound;
				uint identifier;
				cli::array<TextureAttachment^>^ attachments;
				static Framebuffer^ boundFramebuffer;
				static int instanceCntr;
			};
		}
	}
}