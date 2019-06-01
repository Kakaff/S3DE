#pragma once
#include "EngineTypes.h"

namespace S3DECore {
	namespace Time {
		public ref class EngineClock {
		public:
			static void WaitForNextFrame(bool vsync);
			static void SetTargetFramerate(uint value);
			static void SetPowerSaving(bool powersav);
			static long int GetDeltaTime();
		};
	}
}