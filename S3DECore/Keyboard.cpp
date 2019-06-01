#include "EngineInput.h"
#include "GLFW\glfw3.h"
#include "EngineWindow.h"

namespace S3DECore {
	namespace Input {
		int Keyboard::GetKey(int key) {
			return glfwGetKey(S3DECore::Window::window_ptr, key);
		}

		void Keyboard::PollEvents() {
			glfwPollEvents();
		}
	}
}