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


DLL_Export FrameBuffer* Extern_FrameBuffer_Create() { return new FrameBuffer();}
DLL_Export void Extern_FrameBuffer_Bind(FrameBuffer* fb) {fb->Bind();}
DLL_Export void Extern_FrameBuffer_Unbind() {glBindFramebuffer(GL_FRAMEBUFFER, 0);}

DLL_Export void Extern_Framebuffer_AddTextureAttachment2D(Texture* tex,GLenum attachmentLoc,int level) {
	glFramebufferTexture2D(GL_FRAMEBUFFER, attachmentLoc, GL_TEXTURE_2D, tex->GetIdentifier(),level);
	
}

DLL_Export int CheckFrameBufferStatus() {
	return glCheckFramebufferStatus(GL_FRAMEBUFFER);
}