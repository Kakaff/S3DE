#pragma once


class CPUID {
public:
	static void CheckInstructionSupport(void);
	static bool SSESupport(void) { return _sseSupported; }
	static bool SSE2Support(void) { return _sse2Supported; }
	static bool SSSE3Support(void) { return _ssSe3Supported; }
	static bool SSE4Support(void) { return _sse4Supported; }
	static bool SSE4_1Support(void) { return _sse4_1Supported; }
	static bool SSE4_2Support(void) { return _sse4_2Supported; }
	static bool SSE5Support(void) { return _sse5Supported; }
	static bool FMASupport(void) { return _fma3Supported; }
	static bool AVXSupport(void) { return _avxSupported; }
	static bool AVX2Support(void) { return _avx2Supported; }
private:
	static bool _sseSupported;
	static bool _sse2Supported;
	static bool _ssSe3Supported;
	static bool _sse4Supported;
	static bool _sse4_1Supported;
	static bool _sse4_2Supported;
	static bool _sse5Supported;
	static bool _avxSupported;
	static bool _avx2Supported;
	static bool _fma3Supported;
};