#pragma once
#include <map>
#include <gl\glew.h>
#include "EngineTypes.h"
#include "EngineMacros.h"
#include <vector>

class VertexArrayObject {
public:
	VertexArrayObject(void);
	void Bind(void);
	void SetAttrib(uint index, GLint size, GLenum type, GLboolean normalized, uint stride, uint offset);
	void EnableAttrib(uint index);
	void DisableAttrib(uint index);
private:
	GLuint id;
};

class DataBuffer {
public:
	void Bind(void);
	DataBuffer(int target);
	void SetData(const uint8_t data[],uint length, GLenum usage);
	void SetData(std::vector<uint8_t> data, GLenum usage);
private:
	GLuint id;
	int target;
};

class Shader {
public:
	Shader(int shaderStage);
	void SetSource(const std::string &source);
	bool Compile(void);
	void Delete(void);
	uint GetID(void);
private:
	uint id;
	bool isCompiled;
};

class ShaderProgram {
public:
	ShaderProgram(void);
	void AttachShader(Shader s);
	void DetachShader(Shader s);
	uint GetID(void);
	bool Link(void);
	void Use(void);
	void Delete(void);
private:
	uint id;
	bool isLinked;
};

class Mesh {
public:
	Mesh(void);
	void Bind(void);
	void SetVertexData(const uint8_t data[],uint length, GLenum usage);
	void SetVertexData(std::vector<uint8_t> data, GLenum usage);
	void SetIndicies(const uint8_t data[], uint length, GLenum usage);
	void SetVertexAttrib(uint index, GLint size, GLenum type, GLboolean normalized, uint stride, uint offset);
	void EnableVertexAttrib(uint index);
	void DisableVertexAttrib(uint index);
	void Draw(void);
	bool IsBound(void);
	uint NumVertices(void);
	uint NumIndicies(void);
private:
	VertexArrayObject* vao;
	DataBuffer* vb;
	DataBuffer* eb;
	uint vertCount;
	uint indCount;
};
