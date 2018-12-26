#include "Graphics.h"

VertexArrayObject::VertexArrayObject(void) {
	glGenVertexArrays(1, &id);
}

void VertexArrayObject::Bind(void) {
	glBindVertexArray(id);
}

void VertexArrayObject::EnableAttrib(uint index) {
	glEnableVertexAttribArray(index);
}

void VertexArrayObject::DisableAttrib(uint index) {
	glDisableVertexAttribArray(index);
}

void VertexArrayObject::SetAttrib(uint index,GLint size, GLenum type, GLboolean normalized,uint stride, uint offset) {
	glVertexAttribPointer(index,size,type,normalized,stride,(void*)offset);

}
