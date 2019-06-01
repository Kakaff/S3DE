#pragma once
#include "GL\glew.h"
#include "EngineTypes.h"

namespace S3DECore {
	namespace Graphics {
		public ref class Renderer {
		public:
			static bool Init();
			static void SetViewportSize(int x, int y, int width, int height);
			static void Enable(GLenum cap);
			static void Disable(GLenum cap);
			static void Clear(uint clearbit);
		private:
			static int dWidth, dHeight, rWidth, rHeight;
		};
	}
}