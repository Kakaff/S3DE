#include <intrin.h>
#include "EngineMacros.h"
#include "VecMath.h"

DLL_Export void Extern_Vecf256_MatrixMul(const float* m1, const float* m2,  float* res) {

	//Sadly doesn't seem to be much faster than the SSE implementation on my i5 7500u.

	__m256 x0 = _mm256_load_ps(&m1[0]);
	__m256 x1 = _mm256_load_ps(&m1[8]);

	__m256 shuffx0 = _mm256_permute2f128_ps(x0, x0, 1);
	__m256 shuffx1 = _mm256_permute2f128_ps(x1, x1, 1);


	for (int i = 0; i < 2; i++) {
		int t = i * 8;
		_mm256_store_ps(&res[t], _mm256_fmadd_ps(x0, _mm256_set_m128(_mm_load_ps1(&m2[t + 5]), _mm_load_ps1(&m2[t])),
			_mm256_fmadd_ps(shuffx0, _mm256_set_m128(_mm_load_ps1(&m2[t + 4]), _mm_load_ps1(&m2[t + 1])),
				_mm256_fmadd_ps(x1, _mm256_set_m128(_mm_load_ps1(&m2[t + 7]), _mm_load_ps1(&m2[t + 2])),
					_mm256_mul_ps(shuffx1, _mm256_set_m128(_mm_load_ps1(&m2[t + 6]), _mm_load_ps1(&m2[t + 3]))
					)))));
	}

	/* Alternative using _mm256_set_ps instead, seems to get the same perf.
	for (int i = 0; i < 2; i++) {
		int t = i * 8;
		_mm256_store_ps(&res[t], _mm256_fmadd_ps(x0, _mm256_set_ps(m2[t + 5], m2[t + 5], m2[t + 5], m2[t + 5], m2[t], m2[t], m2[t], m2[t]),
			_mm256_fmadd_ps(shuffx0, _mm256_set_ps(m2[t + 4], m2[t + 4], m2[t + 4], m2[t + 4], m2[t + 1], m2[t + 1], m2[t + 1], m2[t + 1]),
				_mm256_fmadd_ps(x1, _mm256_set_ps(m2[t + 7], m2[t + 7], m2[t + 7], m2[t + 7], m2[t + 2], m2[t + 2], m2[t + 2], m2[t + 2]),
					_mm256_mul_ps(shuffx1, _mm256_set_ps(m2[t + 6], m2[t + 6], m2[t + 6], m2[t + 6], m2[t + 3], m2[t + 3], m2[t + 3], m2[t + 3])
					)))));
	}
	*/
}