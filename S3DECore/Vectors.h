#pragma once
#include "Constants.h"
#include <cmath>
#include <stdio.h>
using namespace System;
using namespace System::Runtime::InteropServices;

namespace S3DECore {
	namespace Math {

		value class Quaternion;
		value class Vector4;
		value class Vector3;
		value class Vector2;
		

		[StructLayout(LayoutKind::Sequential, Pack = 32)]
		public value class Matrix4x4 {
		public:

			float m00, m10, m20, m30,
				  m01, m11, m21, m31,
				  m02, m12, m22, m32,
				  m03, m13, m23, m33;

			
			static property Matrix4x4 Identity {
				Matrix4x4 get() {
					Matrix4x4 m;
					m.m00 = 1;
					m.m11 = 1;
					m.m22 = 1;
					m.m33 = 1;
					return m;
				}
			}

			static Matrix4x4 operator*(Matrix4x4 m1, Matrix4x4 m2);

			static Matrix4x4 CreateRotationMatrix(Quaternion rotation);
			static Matrix4x4 CreateScaleMatrix(Vector3 scale);
			static Matrix4x4 CreateTranslationMatrix(Vector3 translation);
			///<summary>
			/// Performs the scaling first, then the rotation and finally the translation.
			///</summary>
			static Matrix4x4 CreateTransformMatrix(Vector3 translation, Vector3 scale, Quaternion rotation);
			///<summary>
			/// Performs the translation first, then the scaling and finally the rotation.
			///</summary>
			static Matrix4x4 CreateWorldMatrix(Vector3 translation, Vector3 scale, Quaternion rotation);
			static Matrix4x4 CreateFoVPerspectiveMatrix(float fieldOfView,float zNear, float zFar, float aspectRatio);
			
			
			property float Value[int,int]{
				float get(int i,int j) { 
					switch (i + (j * 4)) {
					case 0: return m00;
					case 1: return m10;
					case 2: return m20;
					case 3: return m30;
					case 4: return m01;
					case 5: return m11;
					case 6: return m21;
					case 7: return m31;
					case 8: return m02;
					case 9: return m12;
					case 10: return m22;
					case 11: return m32;
					case 12: return m03;
					case 13: return m13;
					case 14: return m23;
					case 15: return m33;
					default: throw gcnew System::IndexOutOfRangeException();
					}
				}
				void set(int i, int j, float f) {
					switch (i + (j * 4)) {
					case 0: m00 = f; break;
					case 1: m10 = f; break;
					case 2: m20 = f; break;
					case 3: m30 = f; break;
					case 4: m01 = f; break;
					case 5: m11 = f; break;
					case 6: m21 = f; break;
					case 7: m31 = f; break;
					case 8: m02 = f; break;
					case 9: m12 = f; break;
					case 10: m22 = f; break;
					case 11: m32 = f; break;
					case 12: m03 = f; break;
					case 13: m13 = f; break;
					case 14: m23 = f; break; 
					case 15: m33 = f; break;
					default: throw gcnew System::IndexOutOfRangeException();
					}
				}
			}
			
		};


		[StructLayout(LayoutKind::Sequential, Pack = 16)]
		public value class Quaternion {
		public:
			float x, y, z, w;

			Quaternion(float x, float y, float z, float w) { this->x = x; this->y = y; this->z = z; this->w = w; }

			static property Quaternion Identity {
				Quaternion get() {
					return Quaternion(0, 0, 0, 1);
				}
			}

			Quaternion Conjugate();
			Quaternion Normalized();
			Matrix4x4 ToRotationMatrix();
			Vector4 ToVector4();
			static Quaternion Inverse(Quaternion q);

			static bool operator==(Quaternion % q1, Quaternion % q2) {
				return q1.x == q2.x && q1.y == q2.y && q1.z == q2.z && q1.w == q2.w;
			}

			static bool operator!=(Quaternion % q1, Quaternion % q2) { return !(q1 == q2); }
			/*
			static Quaternion% operator* (Quaternion % q1, Quaternion % q2) {
				Quaternion qR;
				pin_ptr<float> resQ(&q1.x), quat1(&q2.x), quat2(&qR.x);
				Vecf128::QuaternionMultiply(quat1, quat2, resQ);

				return qR;
			}
			*/
			static Quaternion operator*(const Quaternion q1,const Quaternion q2) {
				
				Quaternion ans(0,0,0,0);

				float q1x = q1.x;
				float q1y = q1.y;
				float q1z = q1.z;
				float q1w = q1.w;

				float q2x = q2.x;
				float q2y = q2.y;
				float q2z = q2.z;
				float q2w = q2.w;

				// cross(av, bv)
				float dot = q1x * q2x + q1y * q2y + q1z * q2z;

				float cx = q1y * q2z - q1z * q2y;
				float cy = q1z * q2x - q1x * q2z;
				float cz = q1x * q2y - q1y * q2x;

				ans.x = q1x * q2w + q2x * q1w + cx;
				ans.y = q1y * q2w + q2y * q1w + cy;
				ans.z = q1z * q2w + q2z * q1w + cz;
				ans.w = q1w * q2w - dot;

				return ans;
			}

			static Quaternion CreateFromAxisAngle(Vector3 axis, float angle);
		};

		[StructLayout(LayoutKind::Sequential, Pack = 16)]
		public value class Vector4 {
		public:
			float x, y, z, w;
			Vector4(float x, float y, float z, float w) { this->x = x; this->y = y; this->z = z; this->w = w; }

			static bool operator==(Vector4 % v1, Vector4 % v2) {
				return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w;
			}

			static bool operator!=(Vector4 % v1, Vector4 % v2) { return !(v1 == v2); }

			Quaternion ToQuaternion();
		};

		[StructLayout(LayoutKind::Sequential, Pack = 16)]
		public value class Vector3 {
		public:
			float x, y, z;
			Vector3(float x, float y, float z) { this->x = x; this->y = y; this->z = z; }
			Vector3(float x, float y) { this->x = x; this->y = y; z = 0; }
			Vector3(float v) { this->x = v; this->y = v; this->z = v; }
			//static bool operator==(Vector3 % v1, Vector3 % v2) { return  v1.x == v2.x && v1.y == v2.y && v1.z == v2.z; }
			static bool operator==(Vector3 v1, Vector3 v2) { return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z; }
			//static bool operator!=(Vector3 % v1, Vector3 % v2) { return !(v1 == v2); }
			static bool operator!=(Vector3 v1, Vector3 v2) { return !(v1 == v2); }

			//static Vector3% operator-(Vector3 % v1, Vector3 % v2) { return Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z); }
			static Vector3 operator-(Vector3 v1, Vector3 v2) { return Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z); }

			//static Vector3% operator+(Vector3 % v1, Vector3 % v2) { return Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z); }
			static Vector3 operator+(Vector3 v1, Vector3 v2) { return Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z); }

			//static Vector3 operator/(Vector3 % v1, Vector3 % v2) { return Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z); }
			static Vector3 operator/(Vector3 v1, Vector3 v2) { return Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z); }
			static Vector3 operator/(Vector3 v, float f) { return Vector3(v.x / f, v.y / f, v.z / f); }

			//static Vector3% operator*(Vector3 % v1, Vector3 % v2) { return Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z); }
			static Vector3 operator*(Vector3 v1, Vector3 v2) { return Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z); }
			//static Vector3% operator*(Vector3 % v, float % f) { return Vector3(v.x * f, v.y * f, v.z * f); }
			static Vector3 operator*(Vector3 v, float f) { return Vector3(v.x * f, v.y * f, v.z * f); }
			//static Vector3% operator/(Vector3 % v, float % f) { return Vector3(v.x / f, v.y / f, v.z / f); }
			
			float LengthSquared() { return x * x + y * y + z * z; }
			float Length() { return sqrt(LengthSquared()); }
			float SquaredNormal();
			Vector3 Inverse() { return Vector3(-x, -y, -z); }
			Vector3 Normalized();
			Vector4 ToVector4();
			Quaternion ToQuaternion();
			Matrix4x4 ToTranslationMatrix();
			Matrix4x4 ToScaleMatrix();

			static Vector3 DirectionTo(Vector3 v1, Vector3 v2);
			static float DistanceTo(Vector3 v1, Vector3 v2);

			static Vector3 Cross(Vector3 v1, Vector3 v2);
			static float Dot(Vector3 v1, Vector3 v2);
			static Vector3 Normalize(Vector3 v);
			Vector3 Transform(Quaternion q);
			Vector3 Transform(Matrix4x4 m);
			static Vector3 Transform(Vector3 v, Quaternion q);
			static Vector3 Transform(Vector3 v, Matrix4x4 m);

			static property Vector3 Right {Vector3 get() { return Vector3(1, 0, 0); }}
			static property Vector3 Left {Vector3 get() { return Vector3(-1, 0, 0); }}
			static property Vector3 Up {Vector3 get() { return Vector3(0, 1, 0); }}
			static property Vector3 Down {Vector3 get() { return Vector3(0, -1, 0); }}
			static property Vector3 Forward {Vector3 get() { return Vector3(0, 0, -1); }}
			static property Vector3 Backward {Vector3 get() { return Vector3(0, 0, 1); }}
			static property Vector3 One {Vector3 get() { return Vector3(1, 1, 1); }}
			static property Vector3 Zero {Vector3 get() { return Vector3(0, 0, 0); }}

			virtual String^ ToString() override {
				return String::Format("({0},{1},{2})",x,y,z);
			}
		};

		[StructLayout(LayoutKind::Sequential, Pack = 16)]
		public value class Vector2 {
		public:
			static bool operator== (Vector2 % v1, Vector2 % v2) { return v1.x == v2.x && v1.y == v2.y; }
			static bool operator!=(Vector2 % v1, Vector2 % v2) { return !(v1 == v2); }

			float LengthSquared();
			Vector2(float x, float y) { this->x = x; this->y = y; }
			float x, y;

			virtual String^ ToString() override {
				return String::Format("({0},{1})", x, y);
			}
		};
	}
}