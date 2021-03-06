
#include <Windows.h>
#include <stdbool.h>

static LARGE_INTEGER curr, freq,prevFrameEnd;
static long long int sleepErrorHigh = 0, sleepErrorLow = 0;
static int sleepErrLowerCounter,sleepErrResetCounter, SLEEP_DECREASE_ERR_COUNTER_MAX = 10,SLEEP_ERR_RESET_COUNTER_MAX = 5;

static long long int deltaTime = 0;
bool pwrsav = true;

HANDLE tmr = NULL;

long long int yieldTime = 1500,frqchkintv = 500;
long long int SLEEP_ERR_THRESHOLD = 500;

long long int eslaped, remainder,overlseep;

int trgFps = 60;

__declspec(dllexport) long long int Extern_GetDeltaTime() {
	return deltaTime;
}

__declspec(dllexport) void Extern_EnablePowerSaving(bool val) {
	pwrsav = val;
}

__declspec(dllexport) void Extern_SetYieldTime(long long int v) {
	yieldTime = v < 0 ? 0 : v;
}

__declspec(dllexport) void Extern_SetFreqCheckInterval(long long int v) {
	frqchkintv = v < 0 ? 0 : v;;
}

void WaitForNextFrame(bool vsync) {

	if (tmr == NULL) {
		tmr = CreateWaitableTimer(NULL, true, NULL);
		if (NULL == tmr)
			printf("Failed creating timer");
	}

	long long int frqchk = 0;

	long long int trgDur = (1000000LL / trgFps) - overlseep;

	bool resetCounterFlag = false;

	overlseep = 0;
	if (trgDur < 0)
		trgDur = 0;

	if (prevFrameEnd.QuadPart == 0)
		QueryPerformanceCounter(&prevFrameEnd);

	QueryPerformanceFrequency(&freq);
	
	if (vsync) {
		QueryPerformanceCounter(&curr);
		deltaTime = ((curr.QuadPart - prevFrameEnd.QuadPart) * 1000000) / freq.QuadPart;
		prevFrameEnd = curr;
		return;
	}

	while (1) {
		QueryPerformanceCounter(&curr);

		eslaped = ((curr.QuadPart - prevFrameEnd.QuadPart) * 1000000) / freq.QuadPart;
		remainder = trgDur - eslaped;

		if (eslaped - frqchk >= frqchkintv) {
			QueryPerformanceFrequency(&freq);
			frqchk = eslaped;
		}
		
		if (remainder <= 0) {
			deltaTime = eslaped;
			prevFrameEnd = curr;
			overlseep = -remainder;
			break;
		}
		else if (pwrsav) {
			LARGE_INTEGER dur;
			dur.QuadPart = -remainder + (yieldTime + sleepErrorHigh);
			if (dur.QuadPart < 0) {
				SetWaitableTimer(tmr, &dur, 0, NULL, NULL, 0);
				WaitForSingleObject(tmr, INFINITE);

				LARGE_INTEGER sleepEnd;
				QueryPerformanceCounter(&sleepEnd);
				long long int sleepErr = ((sleepEnd.QuadPart - curr.QuadPart) * 1000000LL) / freq.QuadPart;
				sleepErr -= -dur.QuadPart;
				sleepErr = sleepErr < 0 ? 0 : sleepErr;

				if (sleepErr > SLEEP_ERR_THRESHOLD) {
					sleepErrResetCounter = 0;
					if (sleepErr >= sleepErrorHigh) {
						sleepErrorLow = sleepErrorHigh;
						sleepErrorHigh = sleepErr;
						sleepErrLowerCounter = 0;
					}
					else {
						sleepErrLowerCounter++;
						if (sleepErr > sleepErrorLow)
							sleepErrorLow = sleepErr;

						if (sleepErrLowerCounter > SLEEP_DECREASE_ERR_COUNTER_MAX) {
							sleepErrLowerCounter = 0;
							sleepErrorHigh = sleepErrorLow;
							sleepErrorLow = 0;
						}
					}
				}
			}
			else {
				if (remainder > yieldTime && !resetCounterFlag) {
					sleepErrResetCounter++;
					resetCounterFlag = true;
					if (sleepErrResetCounter >= SLEEP_ERR_RESET_COUNTER_MAX) {
						sleepErrorHigh = 0;
						sleepErrorLow = 0;
						sleepErrLowerCounter = 0;
						sleepErrResetCounter = 0;
					}
				}
				Sleep(0);
			}
		}
	}
}


__declspec(dllexport) void Extern_SetOverSleepErrorThreshold(int val) {
	SLEEP_ERR_THRESHOLD = val;
}

__declspec(dllexport) void Extern_SetSleepResetCounterMax(int val) {
	SLEEP_ERR_RESET_COUNTER_MAX = val;
}

__declspec(dllexport) void Extern_SetSleepErrDecreaseCounterMax(int val) {
	SLEEP_DECREASE_ERR_COUNTER_MAX = val;
}

__declspec(dllexport) void Extern_SetTargetFramerate(int value) {
	trgFps = value;
}