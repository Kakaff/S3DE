#include <gl\glew.h>
#include "Mesh.h"
namespace S3DECore {
	namespace Graphics {
		DataBuffer::DataBuffer(int targ) {
			glGenBuffers(1, &id);
			target = targ;
		}

		void DataBuffer::SetData(std::vector<uint8_t> data, GLenum usage) {
			glBufferData(target, data.size(), &data[0], usage);
		}

		void DataBuffer::SetData(const uint8_t data[], uint length, GLenum usage) {
			glBufferData(target, length, data, usage);
		}

		void DataBuffer::SetData(const float data[], uint length, GLenum usage) {
			glBufferData(target, length * 4, data, usage);
		}

		void DataBuffer::SetData(const ushort data[], uint length, GLenum usage) {
			glBufferData(target, length * 2, data, usage);
		}

		void DataBuffer::Bind(void) {
			glBindBuffer(target, id);
		}

		void DataBuffer::Destroy() {

		}
	}
}