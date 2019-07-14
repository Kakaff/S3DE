#pragma once

using namespace System::Collections::Generic;

namespace S3DECore {
	namespace Input {

		public enum class KeyState : int {
			Pressed = 0b0001,
			Down = 0b0010,
			Released = 0b0100,
			Up = 0b1000,
		};

		public enum class KeyboardState : int {
			KeyPressed,
			KeyReleased,
		};

		public enum class KeyCode : int {
			UNKNOWN = -1,

			/* Printable keys */
			SPACE = 32,
			/// <summary>
			/// Key: '
			/// </summary>
			APOSTROPHE = 39,  /* ' */
			/// <summary>
			/// Key: ,
			/// </summary>
			COMMA = 44,  /* , */
			/// <summary>
			/// Key: -
			/// </summary>
			MINUS = 45,  /* - */
			/// <summary>
			/// Key: .
			/// </summary>
			PERIOD = 46,  /* . */
			/// <summary>
			/// Key: /
			/// </summary>
			SLASH = 47,  /* / */
			/// <summary>
			/// Key: 0
			/// </summary>
			_0 = 48,
			/// <summary>
			/// Key: 1
			/// </summary>
			_1 = 49,
			/// <summary>
			/// Key: 2
			/// </summary>
			_2 = 50,
			/// <summary>
			/// Key: 3
			/// </summary>
			_3 = 51,
			/// <summary>
			/// Key: 4
			/// </summary>
			_4 = 52,
			/// <summary>
			/// Key: 5
			/// </summary>
			_5 = 53,
			/// <summary>
			/// Key: 6
			/// </summary>
			_6 = 54,
			/// <summary>
			/// Key: 7
			/// </summary>
			_7 = 55,
			/// <summary>
			/// Key: 8
			/// </summary>
			_8 = 56,
			/// <summary>
			/// Key: 9
			/// </summary>
			_9 = 57,
			/// <summary>
			/// Key: ;
			/// </summary>
			SEMICOLON = 59,  /* ; */
			/// <summary>
			/// Key: =
			/// </summary>
			EQUAL = 61, /* = */
			A = 65,
			B = 66,
			C = 67,
			D = 68,
			E = 69,
			F = 70,
			G = 71,
			H = 72,
			I = 73,
			J = 74,
			K = 75,
			L = 76,
			M = 77,
			N = 78,
			O = 79,
			P = 80,
			Q = 81,
			R = 82,
			S = 83,
			T = 84,
			U = 85,
			V = 86,
			W = 87,
			X = 88,
			Y = 89,
			Z = 90,
			/// <summary>
			/// Key: [
			/// </summary>
			LEFT_BRACKET = 91,  /* [ */
			/// <summary>
			/// Key: \
	        /// </summary>
			BACKSLASH = 92,  /* \ */
			/// <summary>
			/// Key: ]
			/// </summary>
			RIGHT_BRACKET = 93,  /* ] */
			/// <summary>
			/// Key: `
			/// </summary>
			GRAVE_ACCENT = 96,  /* ` */
			WORLD_1 = 161, /* non-US #1 */
			WORLD_2 = 162,/* non-US #2 */

			/* Function keys */
			ESCAPE = 256,
			ENTER = 257,
			TAB = 258,
			BACKSPACE = 259,
			INSERT = 260,
			DELETE = 261,
			RIGHT = 262,
			LEFT = 263,
			DOWN = 264,
			UP = 265,
			PAGE_UP = 266,
			PAGE_DOWN = 267,
			HOME = 268,
			END = 269,
			CAPS_LOCK = 280,
			SCROLL_LOCK = 281,
			NUM_LOCK = 282,
			PRINT_SCREEN = 283,
			PAUSE = 284,
			F1 = 290,
			F2 = 291,
			F3 = 292,
			F4 = 293,
			F5 = 294,
			F6 = 295,
			F7 = 296,
			F8 = 297,
			F9 = 298,
			F10 = 299,
			F11 = 300,
			F12 = 301,
			F13 = 302,
			F14 = 303,
			F15 = 304,
			F16 = 305,
			F17 = 306,
			F18 = 307,
			F19 = 308,
			F20 = 309,
			F21 = 310,
			F22 = 311,
			F23 = 312,
			F24 = 313,
			F25 = 314,
			KP_0 = 320,
			KP_1 = 321,
			KP_2 = 322,
			KP_3 = 323,
			KP_4 = 324,
			KP_5 = 325,
			KP_6 = 326,
			KP_7 = 327,
			KP_8 = 328,
			KP_9 = 329,
			KP_DECIMAL = 330,
			KP_DIVIDE = 331,
			KP_MULTIPLY = 332,
			KP_SUBTRACT = 333,
			KP_ADD = 334,
			KP_ENTER = 335,
			KP_EQUAL = 336,
			LEFT_SHIFT = 340,
			LEFT_CONTROL = 341,
			LEFT_ALT = 342,
			LEFT_SUPER = 343,
			RIGHT_SHIFT = 344,
			RIGHT_CONTROL = 345,
			RIGHT_ALT = 346,
			RIGHT_SUPER = 347,
			MENU = 348,
		};

		value class Key;

		public ref class Keyboard abstract sealed {
		public:
			static Key GetKey(KeyCode key);
		internal:
			static void Init();
			static void SetKey(KeyCode kc, Key k);
		
		private:
			static Dictionary<KeyCode, Key> ^keys;
		};

		public value class Key {
		public:
			KeyState GetKeyState();
			bool CheckState(KeyState ks);
		internal:
			Key(KeyState ks) { state = ks; }
			void SetKeyState(KeyState ks);
		private:
			KeyState state;
		};
	}
}