#pragma once
namespace iv
{
	/// 3次元ベクトル型
	struct Vector3
	{
	public:
		float xyz[3];

		Vector3();
		Vector3(const Vector3& v);
		Vector3(const float x, const float y, const float z);

		inline float X() const { return xyz[0]; }
		inline float Y() const { return xyz[1]; }
		inline float Z() const { return xyz[2]; }

		Vector3& operator=(const Vector3& v) &;
		Vector3& operator=(Vector3 && v) & noexcept;

		Vector3& operator+=(const Vector3& v) &;
		Vector3& operator-=(const Vector3& v) &;
		Vector3& operator*=(const float v) &;

		Vector3 operator+(const Vector3& v) const;
		Vector3 operator-(const Vector3& v) const;
		Vector3 operator*(const float v) const;
	};

	/// 4次元ベクトル型
	struct Vector4
	{
	public:
		float xyzw[4];

		Vector4();
		Vector4(const Vector4& v);
		Vector4(const Vector3& v);
		Vector4(const float x, const float y, const float z);
		Vector4(const float x, const float y, const float z, const float w);

		inline float X() const { return xyzw[0]; }
		inline float Y() const { return xyzw[1]; }
		inline float Z() const { return xyzw[2]; }
		inline float W() const { return xyzw[3]; }

		Vector4& operator=(const Vector4& v) &;
		Vector4& operator=(Vector4&& v) & noexcept;
		Vector4& operator=(const Vector3& v) &;
		Vector4& operator=(Vector3&& v) & noexcept;

		Vector4& operator+=(const Vector4& v) &;
		Vector4& operator-=(const Vector4& v) &;
		Vector4& operator*=(const float v) &;

		Vector4 operator-() const;	// 共役
		Vector4 operator*(const float v) const;

		Vector4 normalize() const;
	};
}