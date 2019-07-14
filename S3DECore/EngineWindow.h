#pragma once
#include "GLFW\glfw3.h"
#include "Vectors.h"

using namespace S3DECore::Math;

namespace S3DECore {

	public enum class WindowAttribute : int {
		Focused = GLFW_FOCUSED,
		Resizable = GLFW_RESIZABLE,
	};


	delegate void WindowFocusCallback();

	public ref class Window abstract sealed {
	public:
		static void SetWindowHint(int hint, int value);
		static bool CreateWindow(Vector2 resolution);
		static void DestroyWindow();
		static void SetResolution(int width, int height);
		static void SwapBuffers();
		static void SwapInterval(int v);
		static bool IsCloseRequested();
		static int GetAttribute(int attrib);
		static void UpdateFocus();
		static WindowFocusCallback ^OnWindowGainedFocus, ^OnWindowLostFocus;
	internal:
		static GLFWwindow* window_ptr;
		static bool isFocused;
	};

	public ref class GLFW {
	public:
		static bool Init();
	};
}