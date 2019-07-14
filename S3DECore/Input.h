#pragma once
#include "Keyboard.h"
#include "Mouse.h"

namespace S3DECore {
	namespace Input {
		public ref class Input abstract sealed {
		public:
			static void PollEvents();
			static void Init();

		private:
			static bool isInitialized;
		};
	}
}