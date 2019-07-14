#include "Vectors.h"

namespace S3DECore {
	namespace Math {

		Matrix4x4 Matrix4x4::operator*(Matrix4x4 m1, Matrix4x4 m2) {
			Matrix4x4 res;

			for (int i = 0; i < 4; i++)
				for (int j = 0; j < 4; j++) {
					res.Value[i, j] = m1.Value[i,0] * m2.Value[0,j] +
									  m1.Value[i, 1] * m2.Value[1, j] + 
									  m1.Value[i, 2] * m2.Value[2, j] + 
									  m1.Value[i, 3] * m2.Value[3, j];
				}

			return res;
		}

		Matrix4x4 Matrix4x4::CreateRotationMatrix(Quaternion quat) {
			
			//From: https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToMatrix/index.htm
			Matrix4x4 res;

			double sqw = quat.w * quat.w;
			double sqx = quat.x * quat.x;
			double sqy = quat.y * quat.y;
			double sqz = quat.z * quat.z;

			double invs = 1.0 / (sqx + sqy + sqz + sqw);

			res.m00 = (sqx - sqy - sqz + sqw) * invs;
			res.m11 = (-sqx + sqy - sqz + sqw)*invs;
			res.m22 = (-sqx - sqy + sqz + sqw)*invs;
			res.m33 = 1;

			double tmp1 = quat.x*quat.y;
			double tmp2 = quat.z*quat.w;
			res.m10 = 2.0 * (tmp1 + tmp2)*invs;
			res.m01 = 2.0 * (tmp1 - tmp2)*invs;

			tmp1 = quat.x*quat.z;
			tmp2 = quat.y*quat.w;
			res.m20 = 2.0 * (tmp1 - tmp2)*invs;
			res.m02 = 2.0 * (tmp1 + tmp2)*invs;
			tmp1 = quat.y*quat.z;
			tmp2 = quat.x*quat.w;
			res.m21 = 2.0 * (tmp1 + tmp2)*invs;
			res.m12 = 2.0 * (tmp1 - tmp2)*invs;

			return res;
		}

		Matrix4x4 Matrix4x4::CreateTranslationMatrix(Vector3 v) {
			Matrix4x4 m;

			m.m00 = 1.0f;
			m.m11 = 1.0f;
			m.m22 = 1.0f;

			m.m30 = v.x;
			m.m31 = v.y;
			m.m32 = v.z;
			m.m33 = 1.0f;

			return m;
		}

		Matrix4x4 Matrix4x4::CreateScaleMatrix(Vector3 v) {
			Matrix4x4 m;

			m.m00 = v.x;
			m.m11 = v.y;
			m.m22 = v.z;
			m.m33 = 1.0f;

			return m;
		}

		Matrix4x4 Matrix4x4::CreateFoVPerspectiveMatrix(float fov,float zNear, float zFar, float aspectRatio) {
			float fieldOfView = (float)(fov * Constants::ToRadians);

			float yScale = (float)(1.0f / tan(fieldOfView * 0.5f));
			float xScale = yScale / aspectRatio;

			Matrix4x4 result;

			result.m00 = xScale;

			result.m11 = yScale;
			result.m22 = -zFar / (zFar - zNear);
			result.m32 = -zFar * zNear / (zFar - zNear);
			result.m23 = -1.0f;
			result.m33 = 0;
			
			return result;
			
		}

		Matrix4x4 Matrix4x4::CreateTransformMatrix(Vector3 translation, Vector3 scale, Quaternion rotation) {
			Matrix4x4 res = CreateTranslationMatrix(translation) * CreateRotationMatrix(rotation) * CreateScaleMatrix(scale);
			return res;
		}

		Matrix4x4 Matrix4x4::CreateWorldMatrix(Vector3 translation, Vector3 scale, Quaternion rotation) {
			return  CreateScaleMatrix(scale) * CreateRotationMatrix(rotation) * CreateTranslationMatrix(translation);
		}
	}
}