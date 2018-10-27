#include "Graphics.h"

Shader::Shader(int shaderStage) {
	id = glCreateShader(shaderStage);
}

bool Shader::Compile(void) {
	GLint res = GL_FALSE;
	int loglength;
	glCompileShader(id);
	glGetShaderiv(id, GL_COMPILE_STATUS, &res);
	glGetShaderiv(id, GL_INFO_LOG_LENGTH, &loglength);

	if (res == GL_FALSE) {
		return false;
	}

	return true;
}

void Shader::SetSource(const std::string &src) {
	char const* shadersrc = src.c_str();
	glShaderSource(id, 1, &shadersrc, NULL);
	GLenum err = glGetError();
}

void Shader::Delete() {
	glDeleteShader(id);
}

uint Shader::GetID(void) {
	return id;
}

DLL_Export void Extern_SetShaderSource(Shader*s, const char* src) {
	std::string str(src);
	s->SetSource(str);
}

DLL_Export bool Extern_CompileShader(Shader *s) {
	return s->Compile();
}

DLL_Export Shader* Extern_CreateShader(int shaderstage) {
	return new Shader(shaderstage);
}

DLL_Export void Extern_DeleteShader(Shader* s) {
	s->Delete();
	delete s;
}

ShaderProgram::ShaderProgram() {
	id = glCreateProgram();
}

bool ShaderProgram::Link() {
	glLinkProgram(id);
	GLint res = GL_FALSE;
	int loglength;
	glGetProgramiv(id, GL_LINK_STATUS, &res);

	return res; 
}

uint ShaderProgram::GetID() {
	return id;
}

void ShaderProgram::Use() {
	glUseProgram(id);
}

void ShaderProgram::AttachShader(Shader s) {
	glAttachShader(id, s.GetID());
}

void ShaderProgram::DetachShader(Shader s) {
	glDetachShader(id, s.GetID());
}

DLL_Export ShaderProgram* Extern_Create_ShaderProgram(void) {
	return new ShaderProgram();
}

DLL_Export bool Extern_Link_Program(ShaderProgram* prog) {
	return prog->Link();
}

DLL_Export void Extern_Attach_Shader(ShaderProgram* prog, Shader* shad) {
	prog->AttachShader(Shader(*shad));
}

DLL_Export void Extern_Detach_Shader(ShaderProgram* prog, Shader* shad) {
	prog->DetachShader(Shader(*shad));
}

DLL_Export void Extern_Use_Program(ShaderProgram *prog) {
	prog->Use();
}

DLL_Export int Extern_GetUniformLocation(ShaderProgram* prog,const char* name) {
	return glGetUniformLocation(prog->GetID(), name);
}

DLL_Export void Extern_SetUniform1i(uint loc, int val) {
	glUniform1i(loc, val);
}

DLL_Export void Extern_SetUniform1ui(uint loc, uint val) {
	glUniform1ui(loc, val);
}

DLL_Export void Extern_SetUniform1f(uint loc, float val) {
	glUniform1f(loc, val);
}

DLL_Export void Extern_SetUniform2i(uint loc, int i1,int i2) {
	glUniform2i(loc, i1,i2);
}

DLL_Export void Extern_SetUniform2ui(uint loc, uint ui1,uint ui2) {
	glUniform2ui(loc, ui1,ui2);
}

DLL_Export void Extern_SetUniform2f(uint loc, float f1,float f2) {
	glUniform2f(loc, f1,f2);
}

DLL_Export void Extern_SetUniform3i(uint loc, int i1, int i2, int i3) {
	glUniform3i(loc, i1,i2,i3);
}

DLL_Export void Extern_SetUniform3ui(uint loc, uint ui1,uint ui2, uint ui3) {
	glUniform3ui(loc, ui1,ui2,ui3);
}

DLL_Export void Extern_SetUniform3f(uint loc, float f1, float f2,float f3) {
	glUniform3f(loc, f1, f2,f3);
}

DLL_Export void Extern_SetUniform4f(uint loc, float f1, float f2,float f3, float f4) {
	glUniform4f(loc, f1, f2,f3,f4);
}

DLL_Export void Extern_SetUniform4i(uint loc, int i1, int i2, int i3,int i4) {
	glUniform4i(loc, i1, i2, i3,i4);
}

DLL_Export void Extern_SetUniform4ui(uint loc, uint ui1, uint ui2, uint ui3,uint ui4) {
	glUniform4ui(loc, ui1, ui2, ui3,ui4);
}

DLL_Export void Extern_SetUniformMatrixf4v(uint loc,uint count, bool transpose, const float* matr) {
	glUniformMatrix4fv(loc,count,transpose,matr);
}

DLL_Export void Extern_SetUniformMatrixf3v(uint loc, uint count, bool transpose, const float* matr) {
	glUniformMatrix3fv(loc, count, transpose, matr);
}