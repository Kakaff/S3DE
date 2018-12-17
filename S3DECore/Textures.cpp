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

DLL_Export Texture* Extern_Texture2D_Create(void) {
	Texture* tex = new Texture(GL_TEXTURE_2D);
	glBindTexture(GL_TEXTURE_2D, tex->GetIdentifier());
	return tex;
}

DLL_Export void Extern_SetTexImage2D_Data(Texture* tex, GLint target, int level, GLint internalFormat,
	int width, int height, int border, GLint textureDataFormat, GLint textureDataType, uint8_t data[]) {
	glTexImage2D(target, level, internalFormat, width, height, border, textureDataFormat, textureDataType, data);
}

DLL_Export void Extern_SetTexParameteri(Texture* tex,GLint param, int value) {
	glTexParameteri(tex->GetTarget(), param, value);
}

DLL_Export void Extern_SetTexParameterf(Texture* tex,GLint param, float value) {
	glTexParameterf(tex->GetTarget(), param, value);
}

DLL_Export void Extern_BindTexture(Texture* tex, uint textureunit) {
	glActiveTexture(GL_TEXTURE0 + textureunit);
	glBindTexture(tex->GetTarget(), tex->GetIdentifier());
}

DLL_Export void Extern_SetActiveTextureUnit(uint textureunit) {
	glActiveTexture(GL_TEXTURE0 + textureunit);
}