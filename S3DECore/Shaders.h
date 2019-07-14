#pragma once
#include "EngineTypes.h"
#include "Vectors.h"

using namespace S3DECore::Math;

namespace S3DECore {
	namespace Graphics {
		namespace Shaders {
			public enum class UniformType : unsigned int {
				Vector2f = 0x8B50,
				Vector3f = 0x8B51,
				Vector4f = 0x8B52,
				Vector2i = 0x8B53,
				Vector3i = 0x8B54,
				Vector4i = 0x8B55,
				Bool = 0x8B56,
				Vector2b = 0x8B57,
				Vector3b = 0x8B58,
				Vector4b = 0x8B59,
				Matrixf2x2 = 0x8B5A,
				Matrixf3x3 = 0x8B5B,
				Matrixf4x4 = 0x8B5C,
				TextureSampler1D = 0x8B5D,
				TextureSampler2D = 0x8B5E,
				TextureSampler3D = 0x8B5F,
				TextureSamplerCube = 0x8B60,
				Shadow1D = 0x8B61,
				Shadow2D = 0x8B62,
			};

			public ref class Shader {
			public:
				Shader(int shaderstage);
				~Shader();
				!Shader();
				void SetSource(const char* src); //Use https://stackoverflow.com/questions/1300718/c-net-convert-systemstring-to-stdstring to marshal to std::string
				bool Compile();
				void Delete();
				uint GetId();
				int GetInstanceID();
			private:
				void Destroy();
				bool isDeleted;
				uint id;
				static int instanceCount;
				int instanceID;
			};

			public ref class ShaderProgram {
			public:
				ShaderProgram();
				~ShaderProgram();
				!ShaderProgram();
				void AttachShader(Shader^ s);
				void DetachShader(Shader^ s);
				bool Link();
				void Use();
				uint GetId();
				int GetInstanceID();
				bool IsBound();
				int GetUniformLocation(const char* name);
				int GetActiveUniformCount();
				UniformType GetActiveUniformType(int location);

				static void SetUniform1i(uint loc, int val);
				static void SetUniform1ui(uint loc, uint val);
				static void SetUniform1f(uint loc, float val);
				static void SetUniform2i(uint loc, int i1, int i2);
				static void SetUniform2ui(uint loc, uint ui1, uint ui2);
				static void SetUniform2f(uint loc, float f1, float f2);
				static void SetUniform3i(uint loc, int i1, int i2, int i3);
				static void SetUniform3ui(uint loc, uint ui1, uint ui2, uint ui3);
				static void SetUniform3f(uint loc, float f1, float f2, float f3);
				static void SetUniform3f(uint loc, Vector3 v);
				static void SetUniform4f(uint loc, float f1, float f2, float f3, float f4);
				static void SetUniform4i(uint loc, int i1, int i2, int i3, int i4);
				static void SetUniform4ui(uint loc, uint ui1, uint ui2, uint ui3, uint ui4);
				static void SetUniformMatrixf4v(uint loc, uint count, bool transpose, Matrix4x4 matr);
				static void SetUniformMatrixf3v(uint loc, uint count, bool transpose, const float* matr);
			private:
				static ShaderProgram^ boundShaderProgram;
				void Destroy();
				bool isBound;
				void SetIsBound(bool b);
				static int instanceCount;
				int instanceID;
				uint id;
			};
		}
	}
}