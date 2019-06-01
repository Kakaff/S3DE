#include "Renderer.h"

bool S3DECore::Graphics::Renderer::Init() {
	//glewExperimental = true;
	return glewInit() == GLEW_OK;
}

void S3DECore::Graphics::Renderer::Clear(uint clearbit) {
	glClear(clearbit);
}
void S3DECore::Graphics::Renderer::SetViewportSize(int x, int y, int width, int height) {
	glViewport(x, y, width, height);
}

void S3DECore::Graphics::Renderer::Enable(GLenum cap) {
	glEnable(cap);
}

void S3DECore::Graphics::Renderer::Disable(GLenum cap) {
	glDisable(cap);
}