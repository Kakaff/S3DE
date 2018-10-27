#include <gl\glew.h>
#include "Graphics.h"

DataBuffer::DataBuffer(int targ) {
	glGenBuffers(1, &id);
	target = targ;
	isBound = false;
}


void DataBuffer::SetData(std::vector<uint8_t> data, GLenum usage) {
	if (!isBound)
		Bind();

	glBufferData(target, data.size(), &data[0], usage);
}

void DataBuffer::SetData(const uint8_t data[],uint length,GLenum usage) {
	if (!isBound)
		Bind();
	glBufferData(target, length, data, usage);
}

bool DataBuffer::IsBound(void) {
	return isBound;
}

std::map<int,DataBuffer*> DataBuffer::boundVertexBuffers;
std::map<int, DataBuffer*>::iterator DataBuffer::iterator;

void DataBuffer::Bind(void) {
	if (isBound == false) {
		printf("VertexBuffer is not bound, binding now \n");
		iterator = boundVertexBuffers.find(target);
		if (iterator->second != this) {
			if (iterator != boundVertexBuffers.end()) {
				iterator->second->isBound = false;
				boundVertexBuffers.erase(iterator);
			}
			boundVertexBuffers[target] = this;
		}

		glBindBuffer(target, id);
		isBound = true;
	}
}