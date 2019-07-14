#include <gl\glew.h>
#include "Mesh.h"
#include "Renderer.h"

namespace S3DECore {
	namespace Graphics {
		Mesh::Mesh() {
			vao = new VertexArrayObject();
			vb = new DataBuffer(GL_ARRAY_BUFFER);
			eb = new DataBuffer(GL_ELEMENT_ARRAY_BUFFER);
			Bind();
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

		bool Mesh::IsEmpty() { return isEmpty; }

		void Mesh::Draw() {
			Bind();
			glDrawElements(GL_TRIANGLES, indCount, GL_UNSIGNED_SHORT, (void*)0);
		}

		void Mesh::SetVertexAttrib(uint index, GLint size, GLenum type, GLboolean normalized, uint stride, uint offset) {
			Bind();
			vao->SetAttrib(index, size, type, normalized, stride, offset);
		}

		void Mesh::EnableVertexAttrib(uint index) {
			Bind();
			vao->EnableAttrib(index);
		}

		void Mesh::DisableVertexAttrib(uint index) {
			Bind();
			vao->DisableAttrib(index);
		}

		void Mesh::SetVertexData(const uint8_t data[], uint length, GLenum usage) {
			Bind();
			vertCount = length / 4;
			vb->SetData(data, length, usage);
		}

		void Mesh::SetVertexData(const float data[], uint length, GLenum usage) {
			Bind();
			vertCount = length / 5;
			vb->SetData(data, length, usage);
			printf("Mesh has %i vertices \n", vertCount);
			Renderer::CheckErrors(gcnew String("Tried uploading Vertex data"));
		}

		void Mesh::SetIndicies(const uint8_t data[], uint length, GLenum usage) {
			Bind();
			eb->SetData(data, length, usage);
			indCount = length / 2;
			printf("Mesh has %i indicies \n", indCount);
			Renderer::CheckErrors(gcnew String("Tried uploading indicie data"));
		}

		void Mesh::SetIndicies(const ushort data[], uint length, GLenum usage) {
			Bind();
			eb->SetData(data, length, usage);
			indCount = length;
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