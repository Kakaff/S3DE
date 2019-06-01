#pragma once
#include "EngineMacros.h"
#include "GLFW\glfw3.h"

namespace S3DECore {

	public ref class Window {
	public:
		static void SetWindowHint(int hint, int value);
		static bool CreateWindow(int width, int height);
		static void DestroyWindow();
		static void SetResolution(int width, int height);
		static void SwapBuffers();
		static void SwapInterval(int v);
		static bool IsCloseRequested();
		static int GetAttribute(int attrib);
	internal:
		static GLFWwindow* window_ptr;
	};

	public ref class GLFW {
	public:
		static bool Init();
	};
}