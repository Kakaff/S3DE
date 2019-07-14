#pragma once
#include "EngineTypes.h"

namespace S3DECore {
	namespace Time {
		public ref class EngineClock abstract sealed {
		public:
			static void WaitForNextFrame();
			static void SetTargetFramerate(uint value);
			static void SetPowerSaving(bool powersav);
			static property double DeltaTime {
				double get() { return deltaTime; }
			}
		private:
			static double deltaTime;
		};
	}
}