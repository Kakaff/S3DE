#include <Windows.h>
#include <stdbool.h>

LARGE_INTEGER start, end, eslaped, curr, freq;
long long int oversleep = 0;

bool isInitiated = false;
bool oslpadj = false;
bool pwrsav = true;

long long int yieldTime = 2000, sleepTime = 3500, pwrsvTime = 5000,frqchkintv = 500;

__declspec(dllexport) void Extern_SetYieldTime(long long int v) {
	yieldTime = v < 0 ? 0 : v;
}

__declspec(dllexport) void Extern_SetSleepTime(long long int v) {
	sleepTime = v < 0 ? 0 : v;;
}

__declspec(dllexport) void Extern_SetPowerSaveTime(long long int v) {
	pwrsvTime = v < 0 ? 0 : v;;
}

__declspec(dllexport) void Extern_SetFreqCheckInterval(long long int v) {
	frqchkintv = v < 0 ? 0 : v;;
}

void WaitForNextFrame(void) {
	long long int oslp = oversleep - (oversleep % 50);
	long long int frqchk = 0;
	oversleep = oversleep - oslp;

	long long int remainder;
	long long int eslpMod;
	QueryPerformanceCounter(&start);
	if (end.QuadPart == 0)
		end.QuadPart = start.QuadPart;

	QueryPerformanceFrequency(&freq);
	eslpMod = start.QuadPart - end.QuadPart;

	while (1) {
		QueryPerformanceCounter(&end);

		eslaped.QuadPart = (((end.QuadPart - start.QuadPart) + eslpMod) * 1000000) / freq.QuadPart;
		remainder = (16666 - oslp) - eslaped.QuadPart;

		if (eslaped.QuadPart - frqchk >= frqchkintv) {
			QueryPerformanceFrequency(&freq);
			frqchk = eslaped.QuadPart;
		}
		
		if (remainder <= 0) {
			if (oslpadj)
				oversleep += -remainder;
			else
				oversleep = 0;
			break;
		}
		else if (pwrsav || remainder >= pwrsvTime) {
			long int sleepDur = (remainder - sleepTime) / 1000; //Remove 2500 microsecs (2.5ms, to be used as yield/sleep(0) time).
			if (sleepDur > 0) {
				Sleep((DWORD)sleepDur);
				QueryPerformanceFrequency(&freq);
			}
			else if (remainder >= yieldTime) {
				Sleep(0);
			}

		}
	}
}

__declspec(dllexport) void Extern_EnableOversleepAdjustment(bool flag) {
	oslpadj = flag;
}

__declspec(dllexport) void InitClock(void) {
	QueryPerformanceFrequency(&freq);
	isInitiated = true;
}

__declspec(dllexport) long long int Extern_Time_GetTick() {
	if (!isInitiated)
		InitClock();

	QueryPerformanceCounter(&curr);
	return (curr.QuadPart * 10000000) / freq.QuadPart;
}