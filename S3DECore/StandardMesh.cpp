#include "Mesh.h"
#include "Renderer.h"

namespace S3DECore {
	namespace Graphics {
		namespace Meshes {

			StandardMesh::StandardMesh() : Mesh() {
				Bind();
				SetVertexAttrib(0, 3, GL_FLOAT, GL_FALSE, 20, 0);
				SetVertexAttrib(1, 2, GL_FLOAT, GL_FALSE, 20, 12);
				EnableVertexAttrib(0);
				EnableVertexAttrib(1);
			}

			StandardMesh::~StandardMesh() {
				
			}

			StandardMesh::!StandardMesh() {

			}

			void StandardMesh::Apply() {
				array<float>^ vertData = gcnew array<float>((Vertices->Length * 3) + (uvs->Length * 2));
				
				//Copy the vertices to a byte array.
				for (int i = 0; i < verts->Length; i++) {
					int j = i * 5;
					vertData[j] = Vertices[i].x;
					vertData[j + 1] = Vertices[i].y;
					vertData[j + 2] = Vertices[i].z;
					vertData[j + 3] = uvs[i].x;
					vertData[j + 4] = uvs[i].y;
				}
				
				pin_ptr<float> vertPtr(&vertData[0]);
				SetVertexData(vertPtr, vertData->Length, GL_STATIC_DRAW);

				pin_ptr<ushort> indPtr(&indicies[0]);
				SetIndicies(indPtr, indicies->Length, GL_STATIC_DRAW);
			}

			StandardMesh^ StandardMesh::CreateCube(Vector3 scale) {
				StandardMesh^ m = gcnew StandardMesh();

				float vX = 0.5f * scale.x;
				float vY = 0.5f * scale.y;
				float vZ = 0.5f * scale.z;

				array<Vector3>^ verts = {   Vector3(-0.5f, -0.5f, -0.5f), Vector3(-0.5f, 0.5f, -0.5f), Vector3(0.5f, 0.5f, -0.5f),Vector3(0.5f,-0.5f,-0.5f),
											Vector3(-0.5f,0.5f,-0.5f),	  Vector3(-0.5f,0.5f,0.5f),	   Vector3(0.5f,0.5f,0.5f),   Vector3(0.5f,0.5f,-0.5f),
											Vector3(0.5f,-0.5f,-0.5f),	  Vector3(0.5f,0.5f,-0.5f),	   Vector3(0.5f,0.5f,0.5f),   Vector3(0.5f,-0.5f,0.5f),
											Vector3(-0.5f,-0.5f,0.5f),	  Vector3(-0.5f,0.5f,0.5f),	   Vector3(-0.5f,0.5f,-0.5f), Vector3(-0.5f,-0.5f,-0.5f),
											Vector3(0.5f,-0.5f,0.5f),	  Vector3(0.5f,0.5f,0.5f),	   Vector3(-0.5f,0.5f,0.5f),  Vector3(-0.5f,-0.5f,0.5f),
											Vector3(-0.5f,-0.5f,0.5f),	  Vector3(-0.5f,-0.5f,-0.5f),  Vector3(0.5f,-0.5f,-0.5f), Vector3(0.5f,-0.5f,0.5f)};

				m->Vertices = verts;

				array<Vector2>^ uvs = { Vector2(0, 0), Vector2(0, 1), Vector2(1, 1), Vector2(1, 0),
										Vector2(0, 0), Vector2(0, 1), Vector2(1, 1), Vector2(1, 0),
										Vector2(0, 0), Vector2(0, 1), Vector2(1, 1), Vector2(1, 0),
										Vector2(0, 0), Vector2(0, 1), Vector2(1, 1), Vector2(1, 0),
										Vector2(0, 0), Vector2(0, 1), Vector2(1, 1), Vector2(1, 0),
										Vector2(0, 0), Vector2(0, 1), Vector2(1, 1), Vector2(1, 0) };

				m->Uvs = uvs;

				array<ushort>^ inds =	  { 0,1,2,2,3,0,
											4,5,6,6,7,4,
											8,9,10,10,11,8,
											12,13,14,14,15,12,
											16,17,18,18,19,16,
											20,21,22,22,23,20 };


				m->Indicies = inds;

				m->Apply();

				return m;
			}
		}
	}
}