#include "Textures.h"
#include "EngineMacros.h"
#include "EngineTypes.h"
#include <stdint.h>

Texture::Texture(int target) {
	targ = target;
	glGenTextures(1, &identifier);
}

uint Texture::GetIdentifier() {
	return identifier;
}

int Texture::GetTarget() {
	return targ;
}

DLL_Export Texture* Extern_CreateTexture(int target) {
	return new Texture(target);
}

DLL_Export void Extern_SetTexImage2D_Data(Texture* tex, GLint target, int level, GLint internalFormat,
	int width, int height, int border, GLint textureDataFormat, GLint textureDataType, const uint8_t data[]) {
	glTexImage2D(target, level, internalFormat, width, height, border, textureDataFormat, textureDataType, data);
}

DLL_Export void Extern_SetTexParameteri(Texture* tex,GLint param, int value) {
	glTexParameteri(tex->GetTarget(), param, value);
}

DLL_Export void Extern_SetTexParameterf(Texture* tex,GLint param, float value) {
	glTexParameterf(tex->GetTarget(), param, value);
}

DLL_Export void Extern_BindTexture(Texture* tex, uint textureunit) {
	glBindTextureUnit(GL_TEXTURE0 + textureunit,tex->GetIdentifier());
}

DLL_Export void Extern_SetActiveTexture(uint textureUnit) {
	glActiveTexture(GL_TEXTURE0 + textureUnit);
}