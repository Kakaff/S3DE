#pragma once
#include "EngineTypes.h"
#include <vector>
#include "Vectors.h"
#include "GL\glew.h"

using namespace S3DECore::Math;

namespace S3DECore {
	namespace Graphics {
		class VertexArrayObject {
		public:
			VertexArrayObject(void);
			void Bind(void);
			void SetAttrib(uint index, GLint size, GLenum type, GLboolean normalized, uint stride, uint offset);
			void EnableAttrib(uint index);
			void DisableAttrib(uint index);
			void Destroy();
		private:
			GLuint id;
		};

		class DataBuffer {
		public:
			void Bind(void);
			DataBuffer(int target);
			void SetData(const uint8_t data[], uint length, GLenum usage);
			void SetData(std::vector<uint8_t> data, GLenum usage);
			void SetData(const float data[], uint length, GLenum usage);
			void SetData(const ushort data[], uint length, GLenum usage);
			void Destroy();
		private:
			GLuint id;
			int target;
		};

		public ref class Mesh abstract {
		public:
			Mesh();
			~Mesh();
			!Mesh();

			void Bind();
			void Draw(void);
			bool IsBound(void);
			bool IsEmpty();
			uint NumVertices(void);
			uint NumIndicies(void);
			virtual void Apply() abstract;
		protected:
			VertexArrayObject* vao;
			DataBuffer* vb;
			DataBuffer* eb;
			void SetVertexData(const uint8_t data[], uint length, GLenum usage);
			void SetVertexData(const float data[], uint length, GLenum usage);
			void SetIndicies(const uint8_t data[], uint length, GLenum usage);
			void SetIndicies(const ushort data[], uint length, GLenum usage);

			void SetVertexAttrib(uint index, GLint size, GLenum type, GLboolean normalized, uint stride, uint offset);
			void EnableVertexAttrib(uint index);
			void DisableVertexAttrib(uint index);
			bool isEmpty = true;
		private:
			void Destroy();
			uint vertCount;
			uint indCount;
			bool isBound;
			static Mesh^ activeMesh;
		};

		namespace Meshes {

			public ref class StandardMesh sealed : public Mesh {

			public:
				StandardMesh();
				!StandardMesh();
				~StandardMesh();

				static StandardMesh^ CreateCube(Vector3 scale);

				property array<Vector3>^ Vertices {
					void set(array<Vector3>^ arr) { verts = arr; }
					private: array<Vector3>^ get() { return verts; }
				}

				property array<Vector2>^ Uvs {
					void set(array<Vector2>^ arr) { uvs = arr; }
					private: array<Vector2>^ get() { return uvs; }
				}

				property array<ushort>^ Indicies {
					void set(array<ushort>^ arr) { indicies = arr; }
					private: array<ushort>^ get() { return indicies; }
				}

				void Apply() override;
			protected:
			private:
				array<Vector3>^ verts;
				array<Vector2>^ uvs;
				array<ushort>^ indicies;
			};
		}
	}
}