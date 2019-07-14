#include "GLFW\glfw3.h"
#include "Mouse.h"

namespace S3DECore {
	namespace Input {


		MouseState Mouse::GetState() {
			return state;
		}

		bool Mouse::CheckState(MouseState ms) {
			return (GetState() & ms) == ms;
		}

		MouseButton Mouse::GetMouseButton(int button) {
			MouseButton mb;
			if (buttons->TryGetValue(button, mb))
				return mb;
			else
				throw gcnew System::ArgumentOutOfRangeException();
		}

		void Mouse::Init() {
			buttons = gcnew System::Collections::Generic::Dictionary<int, MouseButton>(16);

			for (int i = 0; i < 16; i++)
				buttons->Add(i, MouseButton(ButtonState::Up));
		}

		double Mouse::GetMouseDeltaX() { return deltaX; }
		double Mouse::GetMouseDeltaY() { return deltaY; }

		void MouseButton::UpdateState(ButtonState bs) {
			state = bs;
		}

		ButtonState MouseButton::GetState() { return state; }

		bool MouseButton::CheckState(ButtonState bs) {
			return (GetState() & bs) == bs;
		}

		MouseButton::MouseButton(ButtonState bs) { state = bs; }
	}
}