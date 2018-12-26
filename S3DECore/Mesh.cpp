#include <gl\glew.h>
#include "Graphics.h"
#include "EngineMacros.h"


Mesh::Mesh(void) {
	vao = new VertexArrayObject();
	vb = new DataBuffer(GL_ARRAY_BUFFER);
	eb = new DataBuffer(GL_ELEMENT_ARRAY_BUFFER);
}

void Mesh::Bind(void) {
	vao->Bind();
	vb->Bind();
	eb->Bind();
}

void Mesh::Draw(void) {
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

void Mesh::SetVertexData(const uint8_t data[],uint length, GLenum usage) {
	vertCount = length / 4;
	vb->SetData(data,length, usage);
}

void Mesh::SetVertexData(std::vector<uint8_t> data, GLenum usage) {
	vertCount = data.size() / 4;
	printf("Mesh has %i vertices \n", vertCount);
	vb->SetData(data, usage);
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

DLL_Export void DrawMesh(Mesh* m) {
	m->Draw();
}

DLL_Export Mesh* CreateMesh() {
	return new Mesh();
}

DLL_Export void SetMeshData(Mesh* mesh, uint8_t vertices[],uint vertcount, uint8_t indicies[],uint indCount, int usage) {
	mesh->SetVertexData(vertices,vertcount, usage);
	mesh->SetIndicies(indicies, indCount, usage);
}

DLL_Export void EnableVertexAttrib(Mesh* mesh, uint index) {
	mesh->EnableVertexAttrib(index);
}

DLL_Export void DisableVertexAttrib(Mesh* mesh, uint index) {
	mesh->DisableVertexAttrib(index);
}

DLL_Export void SetVertexAttrib(Mesh* mesh,uint index, GLint size, GLenum type, GLboolean normalized, uint stride, uint offset) {
	mesh->SetVertexAttrib(index, size, type, normalized, stride, offset);
}

DLL_Export void Extern_Mesh_Bind(Mesh* m) {
	m->Bind();
}