#pragma once
#include "EngineTypes.h"

namespace S3DECore {
	
	namespace Input {
		public ref class Cursor {
		public:
			static void SetCursor(uint mode);
			static void GetCursorPos(double* x, double *y);
			static void SetCursorPos(double x, double y);
		};

		public ref class Keyboard {
		public:
			static int GetKey(int key);
			static void PollEvents();
			
		};
	}
}