#include "Framebuffer.h"
#include "EngineMacros.h"

FrameBuffer::FrameBuffer(void) {
	glGenFramebuffers(1, &identifier);
}

uint FrameBuffer::GetIdentifier(void) {
	return identifier;
}

void FrameBuffer::Bind(void) {
	glBindFramebuffer(GL_FRAMEBUFFER, identifier);
}

bool FrameBuffer::IsComplete(void) {
	return glCheckFramebufferStatus(GL_FRAMEBUFFER) == GL_FRAMEBUFFER_COMPLETE;
}

DLL_Export FrameBuffer* Extern_FrameBuffer_Create() { return new FrameBuffer();}
DLL_Export void Extern_FrameBuffer_Bind(FrameBuffer* fb) {fb->Bind();}
DLL_Export void Extern_FrameBuffer_Unbind() {glBindFramebuffer(GL_FRAMEBUFFER, 0);}

DLL_Export bool Extern_FrameBuffer_IsComplete(FrameBuffer* fb) {
	return fb->IsComplete();
}

DLL_Export void Extern_FrameBuffer_AddTextureAttachment2D(Texture* tex,GLenum attachmentLoc,int level) {
	glFramebufferTexture2D(GL_FRAMEBUFFER, attachmentLoc, GL_TEXTURE_2D, tex->GetIdentifier(),level);
	
}

DLL_Export void Extern_FrameBuffer_Clear(FrameBuffer* fb, int clearBit) {

}

DLL_Export int Extern_FrameBuffer_CheckStatus(void) {
	return glCheckFramebufferStatus(GL_FRAMEBUFFER);
}