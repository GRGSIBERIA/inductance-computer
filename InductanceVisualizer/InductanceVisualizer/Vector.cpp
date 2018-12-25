#include "Vector.h"
#include <cstring>
#include <cmath>
#include "Vector.h"
using namespace iv;


Vector3::Vector3()
	: xyz{0, 0, 0}
{
}

iv::Vector3::Vector3(const Vector3 & v)
	: xyz{v.X(), v.Y(), v.Z()}
{
}

iv::Vector3::Vector3(const float x, const float y, const float z)
	: xyz{x, y, z}
{
}

Vector3 & iv::Vector3::operator=(const Vector3& v) &
{
	std::memcpy(xyz, v.xyz, sizeof(xyz));
	return *this;
}

Vector3 & iv::Vector3::operator=(Vector3 && v) & noexcept
{
	std::memcpy(xyz, v.xyz, sizeof(xyz));
	return *this;
}

Vector3 & iv::Vector3::operator+=(const Vector3 & v) &
{
	for (int i = 0; i < 3; ++i)
		xyz[i] += v.xyz[i];
	return *this;
}

Vector3 & iv::Vector3::operator-=(const Vector3 & v) &
{
	for (int i = 0; i < 3; ++i)
		xyz[i] -= v.xyz[i];
	return *this;
}

Vector3 & iv::Vector3::operator*=(const float v) &
{
	for (int i = 0; i < 3; ++i)
		xyz[i] *= v;
	return *this;
}

Vector3 iv::Vector3::operator+(const Vector3 & v) const
{
	return Vector3(xyz[0] + v.xyz[0], xyz[1] + v.xyz[1], xyz[2] + v.xyz[2]);
}

Vector3 iv::Vector3::operator-(const Vector3 & v) const
{
	return Vector3(xyz[0] - v.xyz[0], xyz[1] - v.xyz[1], xyz[2] - v.xyz[2]);
}

Vector3 iv::Vector3::operator*(const float v) const
{
	return Vector3(xyz[0] * v, xyz[1] * v, xyz[2] * v);
}

iv::Vector4::Vector4()
	: xyzw{0, 0, 0, 0}
{
}

iv::Vector4::Vector4(const Vector4 & v)
	: xyzw{v.X(), v.Y(), v.Z(), v.W()}
{
}

iv::Vector4::Vector4(const Vector3 & v)
	: xyzw{v.X(), v.Y(), v.Z(), 0}
{
}

iv::Vector4::Vector4(const float x, const float y, const float z)
	: xyzw{x, y, z, 0}
{
}

iv::Vector4::Vector4(const float x, const float y, const float z, const float w)
	: xyzw{x, y, z, w}
{
}

Vector4 & iv::Vector4::operator=(const Vector4 & v) &
{
	std::memcpy(xyzw, v.xyzw, sizeof(xyzw));
	return *this;
}

Vector4 & iv::Vector4::operator=(Vector4 && v) & noexcept
{
	std::memcpy(xyzw, v.xyzw, sizeof(xyzw));
	return *this;
}

Vector4 & iv::Vector4::operator=(const Vector3 & v) &
{
	std::memcpy(xyzw, v.xyz, sizeof(v.xyz));
	xyzw[3] = 0;
	return *this;
}

Vector4 & iv::Vector4::operator=(Vector3 && v) & noexcept
{
	std::memcpy(xyzw, v.xyz, sizeof(v.xyz));
	xyzw[3] = 0;
	return *this;
}

Vector4 & iv::Vector4::operator+=(const Vector4 & v) &
{
	for (int i = 0; i < 4; ++i)
		xyzw[i] += v.xyzw[i];
	return *this;
}

Vector4 & iv::Vector4::operator-=(const Vector4 & v) &
{
	for (int i = 0; i < 4; ++i)
		xyzw[i] -= v.xyzw[i];
	return *this;
}

Vector4 & iv::Vector4::operator*=(const float v) &
{
	for (int i = 0; i < 4; ++i)
		xyzw[i] *= v;
	return *this;
}

Vector4 iv::Vector4::operator-() const
{
	return Vector4(-xyzw[0], -xyzw[1], -xyzw[2], xyzw[3]);
}

Vector4 iv::Vector4::operator*(const float v) const
{
	return Vector4(*this) * v;
}

Vector4 iv::Vector4::normalize() const
{
	float total;
	for (int i = 0; i < 4; ++i)
		total += xyzw[i] * xyzw[i];
	float s = sqrtf(total);
	return Vector4(*this) * s;
}
