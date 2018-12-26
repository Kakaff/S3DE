#include <gl\glew.h>
#include "Graphics.h"

DataBuffer::DataBuffer(int targ) {
	glGenBuffers(1, &id);
	target = targ;
}


void DataBuffer::SetData(std::vector<uint8_t> data, GLenum usage) {
	glBufferData(target, data.size(), &data[0], usage);
}

void DataBuffer::SetData(const uint8_t data[],uint length,GLenum usage) {
	glBufferData(target, length, data, usage);
}

void DataBuffer::Bind(void) {
	glBindBuffer(target, id);
}