#pragma once

namespace S3DECore {
	namespace Math {
		
		public ref class Vecf128 {
		public:
			static void MatrixMultiply(const float* m1, const float* m2, float* res);
			static void CreateTransformMatrix(const float* m1, const float* m2, const float* m3, float* res);
			static void CreateTransformMatrixFromMatricies(const float* m1, const float* m2, const float* m3, float* res);
			static void CreateRotationMatrix(const float* quat, float* res);
			static void QuaternionMultiply(const float* q1, const float* q2, float* rQ);
			/*
			static void VectorMul(const float* v1, const float* v2, float* res);
			static void VectorAdd(const float* v1, const float* v2, float* res);
			static void VectorSub(const float* v1, const float* v2, float* res);
			static void VectorDiv(const float* v1, const float* v2, float* res);
			static void VectorMulAdd(const float* v1, const float* v2, const float* v3, float* res);
			static void VectorMulSub(const float* v1, const float* v2, const float* v3, float* res);
			*/
		};

		public ref class Vecf256 {
		public:
			static void MatrixMultiply(const float* m1, const float* m2, float* res);
			static void CreateTransformMatrix(const float* scale, const float* rot, const float* transl, float* res);
		};
	}
}