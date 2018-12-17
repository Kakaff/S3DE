#pragma once

__declspec(align(16)) struct Vecf128 {
public:
	Vecf128(void);
	Vecf128(float*);
	Vecf128(float f1, float f2, float f3, float f4);
	Vecf128* Multiply(Vecf128* vec);
	static Vecf128* Multiply(Vecf128* v1, Vecf128* v2);
	static Vecf128* Multiply(float* fs1, float* fs2);
	static Vecf128* Add(Vecf128* v1, Vecf128* v2);
	float* GetDataBuffer(void);
private:
	float* data;
};