#include "EngineInput.h"
#include "EngineWindow.h"

void S3DECore::Input::Cursor::SetCursor(uint mode) {
	switch (mode) {
	case 0x0: glfwSetInputMode(S3DECore::Window::window_ptr, GLFW_CURSOR, GLFW_CURSOR_NORMAL); break;
	case 0x1: glfwSetInputMode(S3DECore::Window::window_ptr, GLFW_CURSOR, GLFW_CURSOR_HIDDEN); break;
	case 0x2: glfwSetInputMode(S3DECore::Window::window_ptr, GLFW_CURSOR, GLFW_CURSOR_DISABLED); break;
	}
}

void S3DECore::Input::Cursor::GetCursorPos(double* x, double *y) {
	glfwGetCursorPos(S3DECore::Window::window_ptr, x, y);
}

void S3DECore::Input::Cursor::SetCursorPos(double x, double y) {
	glfwSetCursorPos(S3DECore::Window::window_ptr, x, y);
}