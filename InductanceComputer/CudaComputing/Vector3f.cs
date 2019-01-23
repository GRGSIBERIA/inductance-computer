using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CudaComputing
{
    public struct Vector3f : IEquatable<Vector3f>, IStructuralEquatable, IComparable<Vector3f>, IComparable, IStructuralComparable
    {
        public float x, y, z;

        public static Vector3f Create(float x, float y, float z)
        {
            Vector3f v;
            v.x = x; v.y = y; v.z = z;
            return v;
        }

        public Vector3f(float x, float y, float z)
        {
            this.x = x; this.y = y; this.z = z;
        }

        #region Compare
        public int CompareTo(object other, IComparer comparer)
        {
            return (object)this == other ? 1 : 0;
        }

        public int CompareTo(Vector3f other)
        {
            return other.x == x && other.y == y && other.z == z ? 1 : 0;
        }

        public int CompareTo(object obj)
        {
            return (object)this == obj ? 1 : 0;
        }

        public bool Equals(object other, IEqualityComparer comparer)
        {
            var obj = (Vector3f)other;
            return x == obj.x && y == obj.y && z == obj.z;
        }

        public bool Equals(Vector3f other)
        {
            return other.x == x && other.y == y && other.z == z;
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }
        #endregion

        public static Vector3f operator +(Vector3f a, Vector3f b)
        {
            return Create(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3f operator -(Vector3f a, Vector3f b)
        {
            return Create(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3f operator /(Vector3f a, float b)
        {
            float c = 1.0f / b;
            return Create(c * a.x, c * a.y, c * a.z);
        }

        public static Vector3f operator *(Vector3f a, float b)
        {
            return Create(a.x * b, a.y * b, a.z * b);
        }

        public static Vector3f operator *(float b, Vector3f a)
        {
            return Create(a.x * b, a.y * b, a.z * b);
        }

        public static Vector3f operator *(Vector3f a, Vector3f b)
        {
            return Create(
                a.y * b.z - a.z * b.y,
                a.z * b.x - a.x * b.z,
                a.x * b.y - a.y * b.x
                );
        }

        /// <summary>
        /// ベクトルの内積，内部で正規化はおこなわない　
        /// </summary>
        /// <param name="a">ベクトルA</param>
        /// <param name="b">ベクトルB</param>
        /// <returns>内積</returns>
        public static float Dot(Vector3f a, Vector3f b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        /// <summary>
        /// ベクトルの長さ
        /// </summary>
        public float Length
        {
            get
            {
                return (float)Math.Sqrt(x * x + y * y + z * z);
            }
        }

        /// <summary>
        /// ベクトルの正規化
        /// </summary>
        public Vector3f Normalized
        {
            get
            {
                return this / this.Length;
            }
        }
    }
}
