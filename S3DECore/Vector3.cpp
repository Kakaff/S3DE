#include "Vectors.h"

namespace S3DECore {
	namespace Math {


		Vector4 Vector3::ToVector4() { return Vector4(x, y, z, 0); }
		Quaternion Vector3::ToQuaternion() { return Quaternion(x, y, z, 0); }

		Vector3 Vector3::Transform(Vector3 v, Quaternion q) {
			
			Quaternion res = (q * Quaternion(v.x, v.y, v.z, 0)) * q.Conjugate();

            return Vector3(res.x, res.y,res.z);
		}

		Vector3 Vector3::Transform(Vector3 v, Matrix4x4 m) {
			return Vector3(
				v.x * m.m00 + v.y * m.m10 + v.z * m.m20 + m.m30,
				v.x * m.m01 + v.y * m.m11 + v.z * m.m21 + m.m31,
				v.x * m.m02 + v.y * m.m12 + v.z * m.m22 + m.m32);
		}

		Vector3 Vector3::Transform(Quaternion q) {
			return Vector3::Transform(*this, q);
		}

		Vector3 Vector3::Transform(Matrix4x4 m) {
			return Vector3::Transform(*this, m);
		}

		float Vector3::Dot(Vector3 v1, Vector3 v2) {
			return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
		}

		Vector3 Vector3::Cross(Vector3 v1, Vector3 v2) {
			return Vector3(
				v1.y * v2.z - v1.z * v2.y,
				v1.z * v2.x - v1.x * v2.z,
				v1.x * v2.y - v1.y * v2.x);
		}

		float Vector3::SquaredNormal() { return Dot(*this, *this); }

		Vector3 Vector3::DirectionTo(Vector3 v1, Vector3 v2) {
			return Vector3(v2 - v1).Normalized();
		}

		float Vector3::DistanceTo(Vector3 v1, Vector3 v2) {
			return (v2 - v1).Length();
		}

		Vector3 Vector3::Normalized() {
			return Vector3::Normalize(*this);
		}

		Vector3 Vector3::Normalize(Vector3 v) {
			return v / v.Length();
		}

		Matrix4x4 Vector3::ToTranslationMatrix() {
			return Matrix4x4::CreateTranslationMatrix(*this);
		}

		Matrix4x4 Vector3::ToScaleMatrix() {
			return Matrix4x4::CreateScaleMatrix(*this);
		}
	}
}