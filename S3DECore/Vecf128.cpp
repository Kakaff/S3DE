#include <intrin.h>
#include <memory>
#include "EngineMacros.h"
#include "VecMath.h"


Vecf128::Vecf128() {
	data = new float[4];
}

Vecf128::Vecf128(float* fs) {
	data = new float[4] {fs[0], fs[1], fs[2], fs[3]};
}

Vecf128::Vecf128(float f1, float f2, float f3, float f4) {
	data = new float[4] {f1, f2, f3, f4};
}

Vecf128* Vecf128::Multiply(Vecf128* vec) {
	Vecf128* r = new Vecf128();
	__m128 a, b;
	a = _mm_load_ps(data);
	b = _mm_load_ps(vec->data);
	_mm_store_ps(r->data, _mm_mul_ps(a, b));
	return r;
}

Vecf128* Vecf128::Multiply(Vecf128* v1, Vecf128* v2) {
	Vecf128* r = new Vecf128();

	__m128 sseA, sseB;
	sseA = _mm_load_ps(v1->data);
	sseB = _mm_load_ps(v2->data);

	_mm_store_ps(r->data,_mm_mul_ps(sseA, sseB));

	return r;
}

Vecf128* Vecf128::Multiply(float* fs1, float* fs2) {
	return Vecf128::Multiply(&Vecf128(fs1), &Vecf128(fs2));
}


Vecf128* Vecf128::Add(Vecf128* v1, Vecf128* v2) {
	Vecf128* r = new Vecf128();

	__m128 sseA, sseB;
	sseA = _mm_load_ps(v1->data);
	sseB = _mm_load_ps(v2->data);

	_mm_store_ps(r->data, _mm_add_ps(sseA, sseB));

	return r;
}

float* Vecf128::GetDataBuffer() {
	return data;
}

DLL_Export float* Extern_Vecf128_GetDataBuffer(Vecf128* v) {
	return v->GetDataBuffer();
}

DLL_Export Vecf128* Extern_Vecf128_Create() {
	return new Vecf128();
}

DLL_Export void Extern_Vecf128_FastMulSub(float* f1, float* f2, float* f3, float* r) {
	_mm_store_ps(r, _mm_fmsub_ps(_mm_load_ps(f1), _mm_load_ps(f2), _mm_load_ps(f3)));
}

DLL_Export void Extern_Vecf128_FastMulAdd(float* f1, float* f2, float* f3, float* r) {
	_mm_store_ps(r, _mm_fmadd_ps(_mm_load_ps(f1), _mm_load_ps(f2), _mm_load_ps(f3)));
}

DLL_Export void Extern_Vecf128_FastMul(float* f1, float* f2, float* r) {
	_mm_store_ps(r,_mm_mul_ps(_mm_load_ps(f1), _mm_load_ps(f2)));
}

DLL_Export void Extern_Vecf128_FastMul_1(float* f1, float* f2, float* r) {
	_mm_store_ps(r, _mm_mul_ps(_mm_load_ps(f1), _mm_load1_ps(f2)));
}

DLL_Export void Extern_Vecf128_FastAdd(float* f1, float* f2, float* r) {
	_mm_store_ps(r, _mm_add_ps(_mm_load_ps(f1), _mm_load_ps(f2)));
}

DLL_Export void Extern_Vecf128_FastSub(float* f1, float* f2, float* r) {
	_mm_store_ps(r, _mm_sub_ps(_mm_load_ps(f1), _mm_load_ps(f2)));
}



DLL_Export void Extern_Vecf128_MatrixMul(float* m1, float* m2, float* res) {

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

DLL_Export void Extern_Vecf128_MatrixMul3(float* m1, float* m2,float* m3, float* res) {
	_declspec(align(16)) float tmpMul[16];
	Extern_Vecf128_MatrixMul(m1, m2, tmpMul);
	Extern_Vecf128_MatrixMul(tmpMul, m3, res);
}

DLL_Export Vecf128* Extern_Vecf128_Multiply(Vecf128* v1, Vecf128* v2) {
	return Vecf128::Multiply(v1, v2);
}

DLL_Export Vecf128* Extern_Vecf128_Add(Vecf128* v1, Vecf128* v2) {
	return Vecf128::Add(v1, v2);
}

DLL_Export void Extern_Vecf128_Destroy(Vecf128* vec) {
	delete vec->GetDataBuffer();
	delete vec;
}

DLL_Export void Extern_Vecf128_QuatToRotMatrix(float* quat, float* resMatr) {
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


	_mm_store_ps(resVec2, _mm_mul_ps(_2222,_mm_add_ps(xyyzzxww, wzwxwyww)));
	_mm_store_ps(resVec3, _mm_mul_ps(_2222,_mm_sub_ps(xyyzzxww, wzwxwyww)));

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

	resMatr[0] = resVec1[0]; // m00
	resMatr[5] = resVec1[1]; // m11
	resMatr[10] = resVec1[2]; // m22
	
	resMatr[4] = resVec2[0]; //m01
	resMatr[9] = resVec2[1]; //m12
	resMatr[2] = resVec2[2];  //m20

	resMatr[1] = resVec3[0]; //m10
	resMatr[6] = resVec3[1]; //m21
	resMatr[8] = resVec3[2]; //m02
	
	resMatr[15] = 1;

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

DLL_Export void Extern_Vecf128_CreateTransformMatrix(float* scale, float* quat, float* transl, float* res) {
	
	float sclMatr[] = {scale[0],0,0,0,
				       0,scale[1],0,0,
					   0,0,scale[2],0,
					   0,0,0,1};

	float rotMatr[] = { 0,0,0,0,
						0,0,0,0,
						0,0,0,0,
						0,0,0,0};

	float trnsMatr[] = {1,0,0,transl[0],
					    0,1,0,transl[1],
						0,0,1,transl[2],
						0,0,0,1};

	Extern_Vecf128_QuatToRotMatrix(quat, rotMatr);

	Extern_Vecf128_MatrixMul3(trnsMatr,rotMatr, sclMatr, res);

}
