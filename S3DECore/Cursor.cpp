#include "EngineWindow.h"
#include "EngineTypes.h"

#include "Input.h"

namespace S3DECore {
	namespace Input {
		void Mouse::Cursor::SetCursorMode(CursorMode mode) {
			switch (mode) {
			case CursorMode::Normal: glfwSetInputMode(S3DECore::Window::window_ptr, GLFW_CURSOR, GLFW_CURSOR_NORMAL); break;
			case CursorMode::Hidden: glfwSetInputMode(S3DECore::Window::window_ptr, GLFW_CURSOR, GLFW_CURSOR_HIDDEN); break;
			case CursorMode::Disabled: glfwSetInputMode(S3DECore::Window::window_ptr, GLFW_CURSOR, GLFW_CURSOR_DISABLED); break;
			}
		}

		void Mouse::Cursor::UpdateCursorPos() {
			double x, y;
			glfwGetCursorPos(S3DECore::Window::window_ptr, &x, &y);
			cursorPos = Vector2(x, y);
		}

		Vector2 Mouse::Cursor::GetCursorPos() {
			return cursorPos;
		}

		void Mouse::Cursor::SetCursorPos(double x, double y) {
			glfwSetCursorPos(S3DECore::Window::window_ptr, x, y);
		}
	}
}