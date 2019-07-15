#include "GLFW\glfw3.h"
#include "Mouse.h"
#include "EngineWindow.h"

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


		void MouseButton::UpdateState(ButtonState bs) {
			state = bs;
		}

		ButtonState MouseButton::GetState() { return state; }

		bool MouseButton::CheckState(ButtonState bs) {
			return (GetState() & bs) == bs;
		}

		MouseButton::MouseButton(ButtonState bs) { state = bs; }

		void Mouse::Update() {
			Vector2 prevPos = pos;
			Vector2 newPos;
			double x = 0, y = 0;
			glfwGetCursorPos(S3DECore::Window::window_ptr, &x, &y);
			Vector2 dispRes = S3DECore::Window::DisplayResolution;
			newPos = Vector2(x, y);
			delta = Vector2((newPos.x - pos.x) / dispRes.x,(newPos.y - pos.y) / dispRes.y);
			pos = newPos;

			if (delta.LengthSquared() > 0)
				if (!CheckState(MouseState::StartedMoving)) {
					state = MouseState::StartedMoving | MouseState::IsMoving;
				}
				else {
					state = MouseState::IsMoving;
				}
			else
				if (!CheckState(MouseState::Stopped)) {
					state = MouseState::Stopped | MouseState::Still;
				}
				else {
					state = MouseState::Still;
				}
		}
	}
}