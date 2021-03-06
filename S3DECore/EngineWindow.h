#pragma once
#include "EngineMacros.h"

namespace S3DECore {

	DLL_Export void Extern_SetWindowHint(int hint, int value);

	class Window {
	public:
		static void SetWindowHint(int hint, int value);
		static bool CreateWindow();
		static void DestroyWindow();
		static void SetResolution(int width, int height);
		static void SwapBuffers();
		static void SwapInterval(int v);
		static bool IsCloseRequested();
		static bool VsyncEnabled();
		static void SetVsync(bool vsync);
	private:
		static bool vsync;
	};
}