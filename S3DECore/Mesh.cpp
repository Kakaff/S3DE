#include <gl\glew.h>
#include "Graphics.h"
#include "EngineMacros.h"


namespace S3DECore {
	namespace Graphics {
		Mesh::Mesh() {
			vao = new VertexArrayObject();
			vb = new DataBuffer(GL_ARRAY_BUFFER);
			eb = new DataBuffer(GL_ELEMENT_ARRAY_BUFFER);
		}

		Mesh::~Mesh() {
			Destroy();
		}

		Mesh::!Mesh() {

		}

		void Mesh::Bind() {

			if (!isBound) {
				vao->Bind();
				vb->Bind();
				eb->Bind();

				if (activeMesh != nullptr)
					activeMesh->isBound = false;

				activeMesh = this;
				isBound = true;
			}
		}

		void Mesh::Draw() {
			glDrawElements(GL_TRIANGLES, indCount, GL_UNSIGNED_SHORT, (void*)0);
		}

		void Mesh::SetVertexAttrib(uint index, GLint size, GLenum type, GLboolean normalized, uint stride, uint offset) {
			vao->SetAttrib(index, size, type, normalized, stride, offset);
		}

		void Mesh::EnableVertexAttrib(uint index) {
			vao->EnableAttrib(index);
		}

		void Mesh::DisableVertexAttrib(uint index) {
			vao->DisableAttrib(index);
		}

		void Mesh::SetVertexData(const uint8_t data[], uint length, GLenum usage) {
			vertCount = length / 4;
			vb->SetData(data, length, usage);
		}

		void Mesh::SetIndicies(const uint8_t data[], uint length, GLenum usage) {
			eb->SetData(data, length, usage);
			indCount = length / 2;
			printf("Mesh has %i indicies \n", indCount);
		}

		uint Mesh::NumVertices(void) {
			return vertCount;
		}

		uint Mesh::NumIndicies(void) {
			return indCount;
		}

		void Mesh::Destroy() {
			vb->Destroy();
			eb->Destroy();
			vao->Destroy();
			delete vb;
			delete eb;
			delete vao;
		}

		bool Mesh::IsBound() {
			return isBound;
		}
	}
}