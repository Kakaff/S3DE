#include <intrin.h>
#include "VecMath.h"

#pragma managed(push,off)
namespace S3DECore {
	namespace Math {
		namespace Unmanaged {

			void SSE_CreateRotationMatrix(const float* quat, float* res) {
				__m128 xyzw = _mm_load_ps(quat);
				__m128 xxyyzzww = _mm_mul_ps(xyzw, xyzw);
				__m128 _2222 = _mm_set_ps1(2);

				/*
				0 = 1f - 2f * (yy + zz)
				1 = 1f - 2f * (zz + xx)
				2 = 1f - 2f * (yy + xx)
				*/
				_declspec(align(16)) float resVec1[4];
				/*
				* 0 = 2f * (x * y + w * z)
				* 1 = 2f * (y * z + w * x)
				* 2 = 2f * (z * x + w * y)
				*/
				_declspec(align(16)) float resVec2[4];
				/*
				* 0 = 2f * (x * y - w * z)
				* 1 = 2f * (y * z - w * x)
				* 2 = 2f * (z * x - w * y)
				*/
				_declspec(align(16)) float resVec3[4];

				_mm_store_ps(resVec1,
					_mm_sub_ps(_mm_set_ps1(1),
						_mm_mul_ps(_2222, _mm_add_ps(_mm_shuffle_ps(xxyyzzww, xxyyzzww, _MM_SHUFFLE(3, 1, 2, 1)),
							_mm_shuffle_ps(xxyyzzww, xxyyzzww, _MM_SHUFFLE(3, 0, 0, 2)))
						)
					)
				);

				__m128 wzwxwyww = _mm_mul_ps(
					_mm_shuffle_ps(xyzw, xyzw, _MM_SHUFFLE(3, 1, 0, 2)),
					_mm_set_ps1(quat[3])
				);

				__m128 xyyzzxww = _mm_mul_ps(xyzw,
					_mm_shuffle_ps(xyzw, xyzw, _MM_SHUFFLE(3, 0, 2, 1)));


				_mm_store_ps(resVec2, _mm_mul_ps(_2222, _mm_add_ps(xyyzzxww, wzwxwyww)));
				_mm_store_ps(resVec3, _mm_mul_ps(_2222, _mm_sub_ps(xyyzzxww, wzwxwyww)));

				/*
				3 Store
				3 Set
				4 Shuffle
				1 Load
				6 Mul
				2 Add
				2 Sub
				Tot: 21 operations
				*/

				res[0] = resVec1[0]; // m00
				res[5] = resVec1[1]; // m11
				res[10] = resVec1[2]; // m22

				res[4] = resVec2[0]; //m01
				res[9] = resVec2[1]; //m12
				res[2] = resVec2[2];  //m20

				res[1] = resVec3[0]; //m10
				res[6] = resVec3[1]; //m21
				res[8] = resVec3[2]; //m02

				res[15] = 1;

				/*
				Total: 4 shuffles.
				FMA3:
				1 MulSub, 2 MulAdd, 3 Mul, 1 Sub | 11 total operations
				5 Load, 3 Store | 8 Operations.
				Total with loads 'n stores : 19 Operations.
				NonSimd:
				6 Add 6 Sub,18 Mul | 30 Total operations.
				*/
			}

			void SSE_MatrixMul(const float* m1, const float* m2, float* res) {
				__m128 x0 = _mm_load_ps(&m1[0]);
				__m128 x1 = _mm_load_ps(&m1[4]);
				__m128 x2 = _mm_load_ps(&m1[8]);
				__m128 x3 = _mm_load_ps(&m1[12]);

				for (int i = 0; i < 4; i++) {
					int idx = i * 4;
					_mm_store_ps(&res[idx], _mm_fmadd_ps(x0, _mm_load1_ps(&m2[idx]),
						_mm_fmadd_ps(x1, _mm_load1_ps(&m2[idx + 1]),
							_mm_fmadd_ps(x2, _mm_load1_ps(&m2[idx + 2]),
								_mm_mul_ps(x3, _mm_set1_ps(m2[idx + 3])
								)))));
				}
			}

			void SSE_CreateTransformMatrixFromMatricies(const float* m1, const float* m2, const float* m3, float* res) {
				_declspec(align(16)) float tmpMul[16];
				SSE_MatrixMul(m1, m2, tmpMul);
				SSE_MatrixMul(tmpMul, m3, res);
			}

			void SSE_CreateTransformMatrix(const float* scale, const float* quat, const float* pos, float* res) {
				float m1[] = { 
					scale[0],0,0,0,
					0,scale[1],0,0,
					0,0,scale[2],0,
					0,0,0,1 };

				float m2[] = { 
					0,0,0,0,
					0,0,0,0,
					0,0,0,0,
					0,0,0,0 };

				float m3[] = {  1,0,0,pos[0],
								0,1,0,pos[1],
								0,0,1,pos[2],
								0,0,0,1 };
				
				SSE_CreateRotationMatrix(quat, res);
				_declspec(align(16)) float tmpMul[16];
				SSE_MatrixMul(m1, m2, tmpMul);
				SSE_MatrixMul(tmpMul, m3, res);
			}

			void SSE_QuaternionMul(const float* q1, const float* q2, float* rq) {
				/*
				float dot = q1x * q2x + q1y * q2y + q1z * q2z;

				float cx = q1y * q2z - q1z * q2y;
				float cy = q1z * q2x - q1x * q2z;
				float cz = q1x * q2y - q1y * q2x;

				ans.x = q1x * q2w + q2x * q1w + cx;
				ans.y = q1y * q2w + q2y * q1w + cy;
				ans.z = q1z * q2w + q2z * q1w + cz;
				ans.w = q1w * q2w + 0   * 1   - dot;
				*/

				__m128 q1m128 = _mm_load_ps(q1);
				__m128 q2m128 = _mm_load_ps(q2);
				
				/*
					shuff to get q1yzx
					shuff to get q2zxy

					shuff to get q1zxy
					shuff to get q2yzx
				*/
				//__m128 cxyz = 
			}
		}
	}

}
#pragma managed(pop)

/// <summary>Multiplies to matricies using SSE </summary>
void S3DECore::Math::Vecf128::MatrixMultiply(const float* m1, const float* m2, float* res) {
	Unmanaged::SSE_MatrixMul(m1, m2, res);
}

/// <summary>Takes in a rotation, translation and scale matrix.
/// Then multiplies them to create a transform matrix that is stored in res </summary>
void S3DECore::Math::Vecf128::CreateTransformMatrixFromMatricies(const float* m1, const float* m2, const float* m3, float* res) {
	Unmanaged::SSE_CreateTransformMatrixFromMatricies(m1, m2, m3, res);
}

/// <summary>Takes in a quaternion and creates a rotation matrix from it that is stored in res </summary>
void S3DECore::Math::Vecf128::CreateRotationMatrix(const float* quat, float* res) {
	Unmanaged::SSE_CreateRotationMatrix(quat, res);
}


void S3DECore::Math::Vecf128::CreateTransformMatrix(const float* scale, const float* quat,const float* transl, float* res) {
	
	Unmanaged::SSE_CreateTransformMatrix(scale, quat, transl, res);

}

void S3DECore::Math::Vecf128::QuaternionMultiply(const float* q1, const float* q2, float* rq) {
	Unmanaged::SSE_QuaternionMul(q1, q2, rq);
}
