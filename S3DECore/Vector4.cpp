#include "Vectors.h"

namespace S3DECore {
	namespace Math {

		Quaternion Vector4::ToQuaternion() { return Quaternion(x, y, z, w); }
	}
}