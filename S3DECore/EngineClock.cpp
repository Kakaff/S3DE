
#include <Windows.h>
#include <stdbool.h>
#include "EngineClock.h"
#include "Renderer.h"

namespace S3DECore {
	namespace Time {
		/*

#pragma managed(push,off)
		namespace Unmanaged {
			static LARGE_INTEGER curr, freq, prevFrameEnd;
			static long long int sleepErrorHigh = 0, sleepErrorLow = 0;
			static int sleepErrLowerCounter, sleepErrResetCounter, SLEEP_DECREASE_ERR_COUNTER_MAX = 10, SLEEP_ERR_RESET_COUNTER_MAX = 5;

			static long long int deltaTime = 0;

			bool pwrsav = true;

			HANDLE tmr = NULL;

			long long int yieldTime = 1500, frqchkintv = 500;
			long long int SLEEP_ERR_THRESHOLD = 500;

			long long int eslaped, remainder, overlseep;

			int trgFps = 60;

			void WaitForNextFrame(bool b) {

			}
		}
		#pragma managed(pop)
		*/

		static LARGE_INTEGER curr, freq, prevFrameEnd;
		static long long int sleepErrorHigh = 0, sleepErrorLow = 0;
		static int sleepErrLowerCounter, sleepErrResetCounter, SLEEP_DECREASE_ERR_COUNTER_MAX = 10, SLEEP_ERR_RESET_COUNTER_MAX = 5;

		static long long int deltaTime = 0;
		bool pwrsav = true;

		HANDLE tmr = NULL;

		long long int yieldTime = 1500, frqchkintv = 500;
		long long int SLEEP_ERR_THRESHOLD = 500;

		long long int eslaped, remainder, oversleep;

		int trgFps = 60;

		void S3DECore::Time::EngineClock::SetPowerSaving(bool powersav) {
			pwrsav = powersav;
		}

		void S3DECore::Time::EngineClock::SetTargetFramerate(uint t) {
			trgFps = t;
		}

		void S3DECore::Time::EngineClock::WaitForNextFrame() {


			if (tmr == NULL) {
				tmr = CreateWaitableTimer(NULL, true, NULL);
				if (NULL == tmr)
					throw gcnew System::Exception("Error creating waitable timer");
			}

			long long int frqchk = 0;

			long long int trgDur = (1000000LL / trgFps) - oversleep;

			bool resetCounterFlag = false;

			oversleep = 0;
			if (trgDur < 0)
				trgDur = 0;

			if (prevFrameEnd.QuadPart == 0)
				QueryPerformanceCounter(&prevFrameEnd);

			QueryPerformanceFrequency(&freq);

			if (S3DECore::Graphics::Renderer::VSyncEnabled) {
				QueryPerformanceCounter(&curr);
				deltaTime = (((curr.QuadPart - prevFrameEnd.QuadPart) * 1000000) / freq.QuadPart) / 1000000.0;
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
					oversleep = -remainder; //If we overslept, remainder is negative.
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
	}
}

