#include "EngineWindow.h"
#include <GLFW\glfw3.h>
#include "Vectors.h"

using namespace S3DECore::Math;

namespace S3DECore {
	bool S3DECore::GLFW::Init() {
		return glfwInit();
	}

	bool Window::CreateWindow(Vector2 resolution) {

		SetWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
		SetWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
		SetWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

		Window::window_ptr = glfwCreateWindow((int)resolution.x, (int)resolution.y, "S3DE Game Window", NULL, NULL);
		if (Window::window_ptr == NULL)
			return false;

		glfwMakeContextCurrent(Window::window_ptr);

		return true;
	}

	int Window::GetAttribute(int attr) {
		return glfwGetWindowAttrib(S3DECore::Window::window_ptr, attr);
	}



	void Window::SetWindowHint(int hint, int value) {
		glfwWindowHint(hint, value);
	}


	void Window::DestroyWindow()
	{
		glfwDestroyWindow(S3DECore::Window::window_ptr);
	}

	void Window::SetResolution(int width, int height)
	{
		glfwSetWindowSize(S3DECore::Window::window_ptr, width, height);
		dispRes = Vector2((float)width, (float)height);
	}


	void Window::SwapBuffers()
	{
		glfwSwapBuffers(S3DECore::Window::window_ptr);
	}

	void Window::SwapInterval(int v)
	{
		glfwSwapInterval(v);
	}

	bool Window::IsCloseRequested() {
		return glfwWindowShouldClose(S3DECore::Window::window_ptr);
	}

	void Window::UpdateFocus() {
		if (glfwGetWindowAttrib(window_ptr, (int)WindowAttribute::Focused)) {
			if (!isFocused) {
				isFocused = true;
				if (OnWindowGainedFocus != nullptr)
					OnWindowGainedFocus();
			}
		}
		else {
			if (isFocused) {
				isFocused = false;
				if (OnWindowLostFocus != nullptr)
					OnWindowLostFocus();
			}
		}
	}

}

