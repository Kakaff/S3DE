#include "Graphics.h"

namespace S3DECore {
	namespace Graphics {
		
		void Uniforms::SetUniform1i(int location, int value) {
			glUniform1i(location, value);
		}

		void Uniforms::SetUniformMatrixf4v(uint loc, uint count, bool transpose, const float* matr) {
			glUniformMatrix4fv(loc, count, transpose, matr);
		}

		int ShaderProgram::GetActiveUniformCount() {
			int res = 0;
			glGetProgramiv(id, GL_ACTIVE_UNIFORMS, &res);
			return res;
		}

		UniformType ShaderProgram::GetActiveUniformType(int location) {
			int maxLength;
			glGetProgramiv(id, GL_ACTIVE_UNIFORM_MAX_LENGTH, &maxLength);
			uint type;
			int size;
			GLchar name[256];
			glGetActiveUniform(id, location, 0, NULL, &size, &type, &name[0]);

			return (UniformType)type;
		}

		ShaderProgram::ShaderProgram() {
			id = glCreateProgram();
			instanceID = instanceCount;
			instanceCount++;
		}
		ShaderProgram::~ShaderProgram() {
			//Deconstructor. Just call Destroy.
			Destroy();
		}

		ShaderProgram::!ShaderProgram() {

		}


		bool ShaderProgram::IsBound() {
			return isBound;
		}

		void ShaderProgram::AttachShader(Shader^ s) {
			glAttachShader(id, s->GetId());
		}

		void ShaderProgram::DetachShader(Shader^ s) {
			glDetachShader(id, s->GetId());
		}

		bool ShaderProgram::Link() {
			glLinkProgram(id);
			GLint res = GL_FALSE;
			int loglength;
			glGetProgramiv(id, GL_LINK_STATUS, &res);

			return res;
		}

		void ShaderProgram::Use() {
			if (!isBound) {
				glUseProgram(id);
				if (boundShaderProgram != nullptr && boundShaderProgram != this) {
					boundShaderProgram->SetIsBound(false);
					boundShaderProgram = this;
				}
				else if (boundShaderProgram == nullptr)
					boundShaderProgram = this;

				isBound = true;
			}
		}

		void ShaderProgram::SetIsBound(bool b) {
			isBound = b;
		}

		uint ShaderProgram::GetId() {
			return id;
		}

		int ShaderProgram::GetInstanceID() {
			return instanceID;
		}

		void ShaderProgram::Destroy() {
			//Delete the ShaderProgram here.
		}

		int ShaderProgram::GetUniformLocation(const char* name) {
			Use();
			return glGetUniformLocation(id, name);
		}

		void ShaderProgram::SetUniform1i(uint loc, int val) {
			glUniform1i(loc, val);
		}

		void ShaderProgram::SetUniform1ui(uint loc, uint val) {
			glUniform1ui(loc, val);
		}

		void ShaderProgram::SetUniform1f(uint loc, float val) {
			glUniform1f(loc, val);
		}

		void ShaderProgram::SetUniform2i(uint loc, int i1, int i2) {
			glUniform2i(loc, i1, i2);
		}

		void ShaderProgram::SetUniform2ui(uint loc, uint ui1, uint ui2) {
			glUniform2ui(loc, ui1, ui2);
		}

		void ShaderProgram::SetUniform2f(uint loc, float f1, float f2) {
			glUniform2f(loc, f1, f2);
		}

		void ShaderProgram::SetUniform3i(uint loc, int i1, int i2, int i3) {
			glUniform3i(loc, i1, i2, i3);
		}

		void ShaderProgram::SetUniform3ui(uint loc, uint ui1, uint ui2, uint ui3) {
			glUniform3ui(loc, ui1, ui2, ui3);
		}

		void ShaderProgram::SetUniform3f(uint loc, float f1, float f2, float f3) {
			glUniform3f(loc, f1, f2, f3);
		}

		void ShaderProgram::SetUniform4f(uint loc, float f1, float f2, float f3, float f4) {
			glUniform4f(loc, f1, f2, f3, f4);
		}

		void ShaderProgram::SetUniform4i(uint loc, int i1, int i2, int i3, int i4) {
			glUniform4i(loc, i1, i2, i3, i4);
		}

		void ShaderProgram::SetUniform4ui(uint loc, uint ui1, uint ui2, uint ui3, uint ui4) {
			glUniform4ui(loc, ui1, ui2, ui3, ui4);
		}

		void ShaderProgram::SetUniformMatrixf4v(uint loc, uint count, bool transpose, const float* matr) {
			glUniformMatrix4fv(loc, count, transpose, matr);
		}

		void ShaderProgram::SetUniformMatrixf3v(uint loc, uint count, bool transpose, const float* matr) {
			glUniformMatrix3fv(loc, count, transpose, matr);
		}

		void Shader::SetSource(const char* src) {
			//Use https://stackoverflow.com/questions/1300718/c-net-convert-systemstring-to-stdstring to marshal to std::string
			/*
			char const* shadersrc = src.c_str();
			glShaderSource(id, 1, &shadersrc, NULL);
			GLenum err = glGetError();
			*/
			
			glShaderSource(id, 1, &src, NULL);
		}

		bool Shader::Compile() {

			GLint res = GL_FALSE;
			int loglength;
			glCompileShader(id);
			glGetShaderiv(id, GL_COMPILE_STATUS, &res);
			glGetShaderiv(id, GL_INFO_LOG_LENGTH, &loglength);

			return !(res == GL_FALSE);
		}

		void Shader::Delete() {
			glDeleteShader(id);
			isDeleted = true;
		}

		uint Shader::GetId() {
			return id;
		}

		int Shader::GetInstanceID() {
			return instanceID;
		}

		Shader::Shader(int shaderstage) {
			instanceID = instanceCount;
			instanceCount++;
			id = glCreateShader(shaderstage);
			isDeleted = false;
		}

		Shader::~Shader() {
			if (!isDeleted)
				Delete();
		}

		Shader::!Shader() {

		}

		void Shader::Destroy() {
			Delete();
		}
	}
}

