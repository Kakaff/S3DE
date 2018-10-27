#include "Graphics.h"


VertexArrayObject* VertexArrayObject::boundVAO = NULL;

VertexArrayObject::VertexArrayObject(void) {
	glGenVertexArrays(1, &id);
	isBound = false;
}

void VertexArrayObject::Bind(void) {
	if (!isBound) {
		printf("VAO is not bound, binding now \n");
		if (boundVAO == NULL || boundVAO != this) {
			glBindVertexArray(id);
			boundVAO = this;
			isBound = true;
			printf("bound VAO \n");
		}
	}
}

bool VertexArrayObject::IsBound(void) {
	return isBound;
}

void VertexArrayObject::EnableAttrib(uint index) {
	if (!isBound)
		Bind();
	glEnableVertexAttribArray(index);
	printf("Enabling attribute %i \n", index);
}

void VertexArrayObject::DisableAttrib(uint index) {
	if (!isBound)
		Bind();
	glDisableVertexAttribArray(index);
}

void VertexArrayObject::SetAttrib(uint index,GLint size, GLenum type, GLboolean normalized,uint stride, uint offset) {
	if (!isBound)
		Bind();
	glVertexAttribPointer(index,size,type,normalized,stride,(void*)offset);

}
