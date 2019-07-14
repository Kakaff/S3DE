#include "Keyboard.h"

namespace S3DECore {
	namespace Input {

		void Key::SetKeyState(KeyState ks) { state = ks; }
		
		Key Keyboard::GetKey(KeyCode key) {
			Key k;
			if (keys->TryGetValue(key, k))
				return k;
			else
				throw gcnew System::ArgumentOutOfRangeException(System::String::Format("Key {0} does not exist!",key));
		}

		void Keyboard::SetKey(KeyCode kc, Key k) {
			keys->Remove(kc);
			keys->Add(kc, k);
		}

		void Keyboard::Init() {
			array<KeyCode> ^arr = (array<KeyCode>^)(System::Enum::GetValues(KeyCode::typeid));
			keys = gcnew Dictionary<KeyCode, Key>(arr->Length);
			for (int i = 0; i < arr->Length; i++)
				keys->Add(arr[i], Key(KeyState::Up));

		}

		KeyState Key::GetKeyState() { return state; }

		bool Key::CheckState(KeyState ks) { return (state & ks) == ks;}

	}
}