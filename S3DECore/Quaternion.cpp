#include "Vectors.h"

namespace S3DECore {
	namespace Math {

		Quaternion Quaternion::Normalized() {
			double l = sqrt((double)(x * x + y * y + z * z + w * w));
			return Quaternion(x / l, y / l, z / l, w / l);
		}

		Quaternion Quaternion::Inverse(Quaternion q) {
			float ls = q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w;
			float invNorm = 1.0f / ls;

			return Quaternion(-q.x * invNorm, -q.y * invNorm, -q.z * invNorm, q.w * invNorm);
		}

		Quaternion Quaternion::Conjugate() {
			return Quaternion(-x, -y, -z, w);
		}

		Quaternion Quaternion::CreateFromAxisAngle(Vector3 axis, float angle) {
			Quaternion r;

			angle = (float)(angle * Constants::ToRadians);
			float halfAngle = angle * 0.5f;
			float s = (float)sin(halfAngle);
			float c = (float)cos(halfAngle);

			r.x = axis.x * s;
			r.y = axis.y * s;
			r.z = axis.z * s;
			r.w = c;
			return r;
		}

		Matrix4x4 Quaternion::ToRotationMatrix() {
			return Matrix4x4::CreateRotationMatrix(Quaternion(x,y,z,w));
		}

		Vector4 Quaternion::ToVector4() {
			return Vector4(x, y, z, w);
		}
	}
}