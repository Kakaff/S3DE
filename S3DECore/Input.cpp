#include "Input.h"
#include "EngineWindow.h"
#include "GLFW\glfw3.h"

namespace S3DECore {
	namespace Input {

		void Input::PollEvents() {
			glfwPollEvents();

			array<KeyCode>^ keys = (array<KeyCode>^)System::Enum::GetValues(KeyCode::typeid);
			for (int i = 0; i < keys->Length; i++) {
				Key k = Keyboard::GetKey(keys[i]);
				int state = glfwGetKey(S3DECore::Window::window_ptr, (int)keys[i]);
				KeyState newState,oldState = k.GetKeyState();

				if (state == GLFW_PRESS) {
					newState = KeyState::Down | KeyState::Pressed;
				}
				else if (state == GLFW_RELEASE) {
					newState = KeyState::Up | KeyState::Released;
				}
				else {
					if (k.CheckState(KeyState::Released)) {
						Console::WriteLine("Changing keystate to up!");
						newState = KeyState::Up;
					}
					else if (k.CheckState(KeyState::Pressed))
						newState = KeyState::Down;
				}

				k.SetKeyState(newState);
				Keyboard::SetKey(keys[i],k);
				
			}

			Mouse::Update();

		}

		void Input::Init() {
			if (!isInitialized) {
				Keyboard::Init();
				System::Console::WriteLine("Keyboard initlized");
				Mouse::Init();
			}
		}
	}
}