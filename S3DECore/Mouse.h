#pragma once
#include "Vectors.h"

namespace S3DECore {

	using namespace S3DECore::Math;

	namespace Input {

		public enum class ButtonState : int {
			Pressed = 0b0001,
			Down = 0b0010,
			Up = 0b0100,
			Released = 0b1000,
		};

		public enum class MouseState : int {
			Moved =   0b0000001,
			Moving =  0b0000010,
			Stopped = 0b0000100,
			Still =   0b0001000,
			ButtonPressed =  0b0010000,
			ButtonReleased = 0b0100000,
			ButtonHeld =	 0b1000000,
		};

		public enum class CursorMode : int {
			Normal,
			Hidden,
			Disabled,
		};


		value class MouseButton;

		public ref class Mouse abstract sealed {
		public:
			static MouseState GetState();
			static bool CheckState(MouseState ms);
			static MouseButton GetMouseButton(int button);
			static double GetMouseDeltaX();
			static double GetMouseDeltaY();

			ref class Cursor abstract sealed {
			public:
				static void SetCursorMode(CursorMode mode);
				static void SetCursorPos(const Vector2 v) { SetCursorPos(v.x, v.y); }
				static Vector2 GetCursorPos();
				static void SetCursorPos(double x, double y);

			internal:
				static void UpdateCursorPos();
			private:
				static Vector2 cursorPos;
			};
		internal:
			static void Init();
			static void SetState(MouseState ms) { state = ms; }
		private:
			static MouseState state;
			static double deltaX, deltaY;
			static System::Collections::Generic::Dictionary<int, MouseButton>^ buttons;
		};

		public value class MouseButton {
		public:
			bool CheckState(ButtonState bs);
			ButtonState GetState();
		internal:
			MouseButton(ButtonState bs);
			void UpdateState(ButtonState newState);
		private:
			ButtonState state;
		};
	}
}