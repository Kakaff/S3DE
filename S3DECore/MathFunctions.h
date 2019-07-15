#pragma once

namespace S3DECore {
	namespace Math {

		public ref class MathFun abstract sealed {
		public:
			static int Clamp(int low, int max, int val) { return (val > max ? max : (val < low ? low : val)); }
			static long int Clamp(long int low, long int max, long int val) { return (val > max ? max : (val < low ? low : val)); }
			static float Clamp(float low, float max, float val) { return (val > max ? max : (val < low ? low : val)); }
			static double Clamp(double low, double max, double val) { return (val > max ? max : (val < low ? low : val)); }

			static int Normalize(int start, int end, int value) {
				int width = end - start;
				int offset = value - start;

				return (int)(offset - (System::Math::Floor((float)offset / (float)width) * width)) + start;
			}

			static float Normalize(float start, float end, float value) {
				float width = end - start;
				float offset = value - start;

				return (offset - (float)(System::Math::Floor(offset / width) * width)) + start;
			}

			static double Square(double d) {
				return d * d;
			}

			static float Square(float v) {
				return v * v;
			}

			static float Square(int i) {
				return i * i;
			}
		};
	}

}